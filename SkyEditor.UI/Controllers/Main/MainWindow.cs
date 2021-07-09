using System;
using System.Threading;
using System.IO;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using IOPath = System.IO.Path;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Domain;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditorUI.Infrastructure;
using Settings = SkyEditorUI.Infrastructure.Settings;
using SkyEditor.RomEditor.Domain.Rtdx;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SkyEditorUI.Controllers
{
    class MainWindow : Window
    {
        public static MainWindow? Instance;

        [UI] private Dialog? loadingDialog;
        [UI] private Label? openFileDialogLabel;
        [UI] private Widget? updateInfo;
        [UI] private TreeStore? itemStore;
        [UI] private Stack? editorStack;

        [UI] private Button? saveButton;
        [UI] private Button? buildButton;
        [UI] private Button? deployButton;
        [UI] private SearchEntry? mainItemListSearch;
        [UI] private TreeView? mainItemList;
        [UI] private Box? esLoading;
        [UI] private TreeView? recentModpacksList;
        [UI] private ListStore? recentModpacksStore;
        [UI] private AboutDialog? aboutDialog;

        private Widget? currentController;
        private IRtdxRom? rom;
        private Modpack? modpack;
        private List<SourceFile> sourceFiles = new List<SourceFile>();

        public MainWindow() : this(new Builder("Main.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("main_window"))
        {
            Instance = this;

            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;

            updateInfo?.Hide();

            var col = new TreeViewColumn("Title", new CellRendererText());
            var iconRenderer = new CellRendererPixbuf();
            var textRenderer = new CellRendererText();
            col.PackStart(iconRenderer, true);
            col.PackStart(textRenderer, true);
            col.AddAttribute(iconRenderer, "icon_name", 0);
            col.AddAttribute(textRenderer, "text", 1);

            mainItemList!.AppendColumn(col);

            Settings? settings = null;
            try
            {
                settings = Settings.TryLoad();
                UpdateRecentModpacks(settings);
            }
            catch (Exception e)
            {
                UIUtils.ShowErrorDialog(this, "Failed to load settings", "Settings couldn't be loaded."
                    + "Your settings will be reverted to the default settings (close the application if you "
                    + "want to fix them manually, otherwise your settings will be overwritten once they are saved).\n"
                    + "Exception message:\n\n" + e.ToString());
            }
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void OnCreateModpackClicked(object sender, EventArgs args)
        {
            LoadRtdxRom(async () =>
            {
                var createWizard = new CreateModpackWizard();
                var response = (ResponseType) createWizard.Run();
                createWizard.Destroy();

                if (response == ResponseType.Accept)
                {
                    var dialog = new FileChooserNative("Select the modpack directory", this, FileChooserAction.Save | FileChooserAction.CreateFolder, null, null);
                    response = (ResponseType) dialog.Run();

                    if (response == ResponseType.Accept)
                    {
                        string folder = dialog.Filename;

                        var metadata = new ModpackMetadata
                        {
                            Id = createWizard.ModpackId,
                            Name = createWizard.ModpackName,
                            Author = createWizard.ModpackAuthor,
                            Target = "RTDX",
                            Version = "0.0.1",
                            Mods = new List<ModMetadata>
                            {
                                new ModMetadata
                                {
                                    Id = $"{createWizard.ModpackId}.default",
                                    Name = "Default",
                                    Target = "RTDX",
                                    Enabled = true,
                                    Scripts = new List<string>(),
                                }
                            }
                        };
                        modpack?.Dispose();
                        modpack = RtdxModpack.CreateInDirectory(metadata, folder, PhysicalFileSystem.Instance);

                        await modpack.Apply(rom!).ConfigureAwait(false);
                        OnModpackLoaded(null);
                    }

                    dialog.Destroy();
                }
            });
        }

        private void OnOpenModpackClicked(object sender, EventArgs args)
        {
            var dialog = new FileChooserNative("Select Modpack", this, FileChooserAction.Open | FileChooserAction.SelectFolder, null, null);
            var response = (ResponseType) dialog.Run();

            if (response == ResponseType.Accept)
            {
                LoadModpack(dialog.Filename);
            }
            dialog.Destroy();
        }

        private void OnRecentModpackSelectionChanged(object sender, EventArgs args)
        {
            if (modpack != null)
            {
                // TODO: workaround since it keeps selecting other items after loading a modpack.
                // this is a workaround to stop it from happening, but it prevents loading any other modpacks from the recent list
                return;
            }

            var selection = (TreeSelection) sender;
            if (selection.GetSelected(out ITreeModel model, out TreeIter iter))
            {
                var path = model.GetValue(iter, 1) as string;
                if (path != null)
                {
                    LoadModpack(path);
                }
            }
        }

        private void LoadModpack(string path)
        {
            LoadRtdxRom(() =>
            {
                new Thread(async () =>
                {
                    // The progress dialog sometimes doesn't show up if we don't do this
                    Thread.Sleep(50);

                    Exception? exception = null;

                    try
                    {
                        modpack?.Dispose();
                        modpack = new RtdxModpack(path, PhysicalFileSystem.Instance);

                        await modpack.Apply(rom!).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        // Save the exception and throw it on the main thread to show the correct exception handler
                        exception = e;
                    }

                    GLib.Idle.Add(() =>
                    {
                        loadingDialog!.Hide();
                        SetTopButtonsEnabled(true);
                        OnModpackLoaded(exception);
                        return false;
                    });
                }).Start();

                openFileDialogLabel!.Text = $"Loading {path}";
                loadingDialog!.Show();
            });
        }

        private void OnSaveClicked(object sender, EventArgs args)
        {
            CheckRomAndModpackLoaded();

            string? directory = null;
            if (modpack!.ReadOnly)
            {
                UIUtils.ShowInfoDialog(this, "Read-only mod",
                            "This mod is read-only. Please select another directory to create a new modpack based on this mod.");

                var dialog = new FileChooserNative("Save modpack", this, FileChooserAction.Save | FileChooserAction.SelectFolder, null, null);
                var response = (ResponseType) dialog.Run();
                string path = dialog.Filename;
                dialog.Dispose();

                if (response == ResponseType.Accept && Directory.Exists(path))
                {
                    directory = IOPath.Combine(path, modpack.Metadata.Id ?? "modpack");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    AddModpackToRecents();
                }
            }

            SetTopButtonsEnabled(false);

            new Thread(async () =>
            {
                // The progress dialog sometimes doesn't show up if we don't do this
                Thread.Sleep(50);

                Exception? exception = null;
                try
                {
                    await SaveModpack(directory);
                }
                catch (Exception e)
                {
                    // Save the exception and throw it on the main thread to show the correct exception handler
                    exception = e;
                }

                GLib.Idle.Add(() =>
                {
                    SetTopButtonsEnabled(true);
                    loadingDialog!.Hide();

                    if (exception != null)
                    {
                        throw exception;
                    }
                    return false;
                });
            }).Start();

            openFileDialogLabel!.Text = "Saving modpack...";
            loadingDialog!.Show();
        }

        private async Task SaveModpack(string? directory = null)
        {
            if (rom == null || modpack == null)
            {
                throw new InvalidOperationException("ROM or modpack is not loaded");
            }

            var saveModpackTask = directory != null ? modpack.SaveTo(rom, directory) : modpack.Save(rom);
            
            Console.WriteLine($"Saving models and source files.");
            await Task.WhenAll(saveModpackTask, SaveSourceFiles());
        }

        private async Task SaveSourceFiles()
        {
            var tasks = sourceFiles
                .Where(file => file.InProject && file.IsDirty)
                .Select(file => file.Save());
            await Task.WhenAll(tasks);
        }

        private void OnBuildClicked(object sender, EventArgs args)
        {
            CheckRomAndModpackLoaded();

            var dialog = new FileChooserNative("Select a directory to save ROM files to...", this, FileChooserAction.Save | FileChooserAction.SelectFolder, null, null);
            var response = (ResponseType) dialog.Run();

            if (modpack?.Metadata.Id == null)
            {
                UIUtils.ShowErrorDialog(this, "Missing ID",
                            "Please set a modpack ID in modpack settings.");
            }

            if (response == ResponseType.Accept)
            {
                BuildRom(dialog.Filename, PhysicalFileSystem.Instance, Settings.Load().BuildFileStructure,
                    () => UIUtils.OpenInFileBrowser(dialog.Filename));
            }

            dialog.Destroy();
        }

        private void BuildRom(string folder, IFileSystem fileSystem, BuildFileStructureType structure, System.Action? onFinished = null)
        {
            SetTopButtonsEnabled(false);

            new Thread(async () =>
            {
                Exception? exception = null;
                try
                {
                    string? codeInjectionDirectory = null;
                    if (modpack!.Metadata.EnableCodeInjection)
                    {
                        codeInjectionDirectory = CodeInjectionHelpers.GetDirectoryForVersion(
                            Settings.DataPath,
                            modpack!.Metadata.CodeInjectionVersion,
                            modpack!.Metadata.CodeInjectionReleaseType ?? "debug");

                        if (codeInjectionDirectory == null)
                        {
                            throw new Exception($"The code injection version is not available."
                                + "Click \"Update code injection binaries\" in modpack settings to fix this.");
                        }
                    }

                    rom!.EnableCustomFiles = modpack.Metadata.EnableCodeInjection;

                    await SaveSourceFiles();
                    if (structure == BuildFileStructureType.Atmosphere)
                    {
                        var paths = BuildHelpers.CreateAtmosphereFolderStructure(Settings.Load(), folder, fileSystem);
                        await rom.Save(paths.ContentRoot, fileSystem);
                        if (codeInjectionDirectory != null)
                        {
                            BuildHelpers.CopyCodeInjectionBinariesForAtmosphere(paths, codeInjectionDirectory);
                        }
                    }
                    else if (structure == BuildFileStructureType.Emulator)
                    {
                        await rom.Save(folder, fileSystem);
                        if (codeInjectionDirectory != null)
                        {
                            BuildHelpers.CopyCodeInjectionBinariesForEmulator(folder, codeInjectionDirectory);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("unknown structure", nameof(structure));
                    }
                }
                catch (Exception e)
                {
                    exception = e;
                }

                GLib.Idle.Add(() =>
                {
                    loadingDialog!.Hide();
                    SetTopButtonsEnabled(true);
                    if (exception != null)
                    {
                        throw exception;
                    }
                    if (onFinished != null)
                    {
                        onFinished();
                    }
                    return false;
                });
            }).Start();
            
            openFileDialogLabel!.Text = "Building modpack...";
            loadingDialog!.Show();
        }

        private void OnDeployClicked(object sender, EventArgs args)
        {
            CheckRomAndModpackLoaded();
            var settings = Settings.Load();
            if (!settings.FtpSettingsComplete())
            {
                UIUtils.ShowErrorDialog(this, "Deployment failed", "Please enter all FTP credentials in the settings.");
                OpenSettings();
                return;
            }

            var tempDir = IOPath.Combine(IOPath.GetTempPath(), IOPath.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            BuildRom(tempDir, PhysicalFileSystem.Instance, BuildFileStructureType.Atmosphere, () =>
            {
                Console.WriteLine($"Built to temp dir '{tempDir}', starting deployment");
                SetTopButtonsEnabled(false);

                new Thread(() =>
                {
                    Exception? exception = null;
                    try
                    {
                        FTPDeployment.Deploy(settings, tempDir);
                    }
                    catch (Exception e)
                    {
                        exception = e;
                    }
                    finally
                    {
                        Directory.Delete(tempDir, true);
                    }

                    GLib.Idle.Add(() =>
                    {
                        SetTopButtonsEnabled(true);
                        loadingDialog!.Hide();
                        if (exception != null)
                        {
                            throw exception;
                        }
                        return false;
                    });
                }).Start();
                
                openFileDialogLabel!.Text = $"Deploying to {settings.SwitchIp}...";
                loadingDialog!.Show();
            });
        }

        private void OnOpenSettingsClicked(object sender, EventArgs args)
        {
            OpenSettings();
        }

        private void OpenSettings()
        {
            var settings = new SettingsDialog();
            var response = (ResponseType) settings.Run();
            settings.Destroy();
        }

        private void OnOpenSettingsDirectoryClicked(object sender, EventArgs args)
        {
            UIUtils.OpenInFileBrowser(Settings.DataPath);
        }

        private void OnOpenModpackDirectoryClicked(object sender, EventArgs args)
        {
            if (modpack?.Directory != null)
            {
                UIUtils.OpenInFileBrowser(modpack.Directory);
            }
            else
            {
                UIUtils.ShowErrorDialog(this, "Error", "No modpack loaded");
            }
        }

        private void OnOpenModpackInVSCodeClicked(object sender, EventArgs args)
        {
            if (modpack?.Directory != null)
            {
                UIUtils.OpenInVSCode(modpack.Directory, this);
            }
            else
            {
                UIUtils.ShowErrorDialog(this, "Error", "No modpack loaded");
            }
        }

        private void OnOpenAboutDialogClicked(object sender, EventArgs args)
        {
            aboutDialog!.Run();
            aboutDialog.Destroy();
        }

        private void OnMainItemListButtonPressed(object sender, ButtonPressEventArgs args)
        {
            if (mainItemList!.Selection.GetSelected(out var model, out var iter))
            {
                if (model != null)
                {
                    var controllerType = model.GetValue(iter, 2) as Type;
                    if (controllerType != null)
                    {
                        var context = (ControllerContext) model.GetValue(iter, 3);

                        LoadView(controllerType, context);
                
                        var path = model.GetPath(iter);
                        mainItemList.ExpandToPath(path); // Expand the node
                        mainItemList.Selection.SelectPath(path); // Expand the node

                        // Scroll into view
                        mainItemList.ScrollToCell(path, null, true, 0.5f, 0.5f);
                    }
                }
            }
        }

        private void LoadView(Type type, ControllerContext context)
        {
            editorStack!.VisibleChild = esLoading;

            SetTopButtonsEnabled(false);
            UIUtils.ConsumePendingEvents();

            //new Thread(() =>
            {
                Exception? exception = null;
                try
                {
                    if (type.GetConstructor(new [] { typeof(IRtdxRom), typeof(Modpack), typeof(ControllerContext) }) != null)
                    {
                        // Constructor with ROM, modpack and context
                        currentController = Activator.CreateInstance(type, rom, modpack, context) as Widget;
                    }
                    else if (type.GetConstructor(new [] { typeof(IRtdxRom), typeof(Modpack) }) != null)
                    {
                        // Constructor with ROM and modpack
                        currentController = Activator.CreateInstance(type, rom, modpack) as Widget;
                    }
                    else if (type.GetConstructor(new [] { typeof(IRtdxRom), typeof(ControllerContext) }) != null)
                    {
                        // Constructor with ROM and context
                        currentController = Activator.CreateInstance(type, rom, context) as Widget;
                    }
                    else if (type.GetConstructor(new [] { typeof(IRtdxRom)} ) != null)
                    {
                        // Constructor with only the ROM
                        currentController = Activator.CreateInstance(type, rom) as Widget;
                    }
                    else if (type.GetConstructor(new [] { typeof(ControllerContext)} ) != null)
                    {
                        // Constructor with only the context
                        currentController = Activator.CreateInstance(type, context) as Widget;
                    }
                    else
                    {
                        // Parameterless constructor
                        currentController = Activator.CreateInstance(type) as Widget;
                    }
                }
                catch (Exception e)
                {
                    // Save the exception and throw it on the main thread to show the correct exception handler
                    exception = e;
                }

                GLib.Idle.Add(() =>
                {
                    OnViewLoaded(exception);
                    return false;
                });
            }//).Start();
        }

        private void OnViewLoaded(Exception? exception)
        {
            SetTopButtonsEnabled(true);

            if (exception != null)
            {
                throw exception;
            }

            if (currentController == null)
            {
                Console.WriteLine("currentController is null!");
                return;
            }

            var oldView = editorStack!.GetChildByName("es__loaded_view");
            if (oldView != null)
            {
                editorStack.Remove(oldView);
                oldView.Destroy();
            }

            editorStack.AddNamed(currentController, "es__loaded_view");
            editorStack.VisibleChild = currentController;
            Console.WriteLine("Loaded view.");
        }

        private void LoadRtdxRom(System.Action onLoaded)
        {
            var romFolder = Settings.Load().RtdxRomPath;

            if (string.IsNullOrWhiteSpace(romFolder))
            {
                UIUtils.ShowErrorDialog(this, "ROM not configured", 
                    "Please specify a path to an unpacked ROM in the Settings menu.");

                OpenSettings();
                return;
            }
            
            // Sanity checks if the path looks like an unpacked Rescue Team DX ROM
            var exefsFolder = IOPath.Combine(romFolder, "exefs");
            var romfsFolder = IOPath.Combine(romFolder, "romfs");
            var metadataFolder = IOPath.Combine(romfsFolder, "Data", "Managed", "Metadata");
            var dungeonFolder = IOPath.Combine(romfsFolder, "Data", "StreamingAssets", "native_data", "dungeon");
            if (!Directory.Exists(exefsFolder) || !Directory.Exists(romfsFolder)|| !Directory.Exists(metadataFolder) ||
                !Directory.Exists(dungeonFolder))
            {
                 UIUtils.ShowErrorDialog(this, "Invalid ROM directory", "Can't load Rescue Team DX files. Please make " 
                    + "sure that the ROM path contains an unpacked Rescue Team DX ROM with a exefs and romfs directory.");

                OpenSettings();
                return;
            }

            new Thread(async () =>
            {
                // The progress dialog sometimes doesn't show up if we don't do this
                Thread.Sleep(50);

                Exception? exception = null;
                try
                {
                    rom = await RomLoader.LoadRom(romFolder, PhysicalFileSystem.Instance).ConfigureAwait(false) as IRtdxRom;

                    if (rom == null)
                    {
                        throw new Exception("Failed to load rom");
                    }
                }
                catch (Exception e)
                {
                    // Save the exception and throw it on the main thread to show the correct exception handler
                    exception = e;
                }

                GLib.Idle.Add(() =>
                {
                    loadingDialog!.Hide();

                    if (exception != null)
                    {
                        throw exception;
                    }
                    onLoaded();
                    return false;
                });
            }).Start();

            openFileDialogLabel!.Text = $"Loading ROM \"{romFolder}\"";
            loadingDialog!.Show();
        }

        private void OnModpackLoaded(Exception? exception)
        {
            if (exception != null)
            {
                throw exception;
            }

            Console.WriteLine("Loaded modpack.");

            AddModpackToRecents();
            
            LoadView(typeof(ModpackSettingsController), ControllerContext.Null);
            InitMainList();
            SetTopButtonsEnabled(true);
        }

        public void InitMainList()
        {
            itemStore!.Clear();

            string? displayName = !string.IsNullOrEmpty(modpack?.Metadata.Name)
                ? modpack?.Metadata.Name : modpack?.Metadata.Id;
            var root = AddMainListItem<ModpackSettingsController>(displayName ?? "Unknown modpack", "skytemple-e-rom-symbolic");
            var modsIter = AddMainListItem<ModsController>(root, "Mods", "skytemple-e-patch-symbolic");
            AddAllModScripts(modsIter);

            AddMainListItem<StartersController>(root, "Starters", "skytemple-e-monster-symbolic");

            var gameScriptsIter = AddMainListItem(root, "Game Scripts", "skytemple-e-variable-symbolic");
            AddGameScripts(gameScriptsIter);

            var automationScriptsIter = AddMainListItem(root, "Modpack Automation Scripts", "skytemple-e-variable-symbolic");
            AddDefaultModScripts(automationScriptsIter);

            mainItemList!.ExpandToPath(mainItemList.Model.GetPath(root));
        }

        private void AddAllModScripts(TreeIter parent)
        {
            if (modpack == null) 
            {
                return;
            }

            var mods = modpack.Mods ?? Enumerable.Empty<Mod>();
            var defaultMod = GetDefaultMod();
            foreach (var mod in mods)
            {
                if (mod != defaultMod)
                {
                    string formattedName = mod.Metadata.Name ?? mod.Metadata.Id ?? "Unknown mod";
                    var modIter = AddMainListItem(parent, $"{formattedName}{(mod.Enabled ? "" : " (disabled)")}",
                        "skytemple-e-variable-symbolic");
                    AddModScripts(modIter, mod);
                }
            }
        }

        private void AddDefaultModScripts(TreeIter parent)
        {
           var defaultMod = GetDefaultMod();
           if (defaultMod == null)
           {
               return;
           }

           AddModScripts(parent, defaultMod);
        }

        private Mod? GetDefaultMod()
        {
            if (modpack == null || modpack.Mods == null) 
            {
                return null;
            }

            var defaultMod = modpack.Mods
                .FirstOrDefault(mod => mod.Metadata.Id == $"{modpack.Metadata.Id}.default");
            if (defaultMod == null && modpack.Mods.Count == 1)
            {
                defaultMod = modpack.Mods.FirstOrDefault();
            }

            return defaultMod;
        }

        private void AddModScripts(TreeIter parent, Mod mod)
        {
            foreach (var script in mod.Scripts)
            {
                var path = IOPath.Combine(mod.GetBaseDirectory(), script.RelativePath);
                var sourceFile = new SourceFile(path, false);
                sourceFiles.Add(sourceFile);

                AddMainListItem<SourceFileController>(parent, IOPath.GetFileName(path), "skytemple-e-variable-symbolic",
                    new SourceFileControllerContext(sourceFile));
            }
        }

        private void AddGameScripts(TreeIter parent)
        {
            if (rom == null || modpack == null)
            {
                return;
            }

            var scriptsRoot = IOPath.Combine(rom.RomDirectory, "romfs", "Data", "StreamingAssets", "native_data", "script");
            AddGameScripts(parent, new DirectoryInfo(scriptsRoot));
        }

        private void AddGameScripts(TreeIter parent, DirectoryInfo currentDir)
        {
            foreach (var item in currentDir.EnumerateFileSystemInfos().OrderBy(info => info.Name))
            {
                if (item is FileInfo file && file.Extension != ".bin" && !file.Name.StartsWith("."))
                {
                    var sourceFile = new SourceFile(file.FullName, true);
                    sourceFile.OverrideFromModpackIfExists(rom!, modpack!);
                    sourceFiles.Add(sourceFile);
                    AddMainListItem<SourceFileController>(parent, file.Name, "skytemple-e-variable-symbolic",
                        new SourceFileControllerContext(sourceFile));
                }
                else if (item is DirectoryInfo directory && !item.Name.StartsWith("."))
                {
                    var directoryIter = AddMainListItem(parent, directory.Name, "skytemple-e-variable-symbolic");
                    AddGameScripts(directoryIter, directory);
                }
            }
        }

        public void SetTopButtonsEnabled(bool enabled)
        {
            saveButton!.Sensitive = enabled;
            buildButton!.Sensitive = enabled;
            deployButton!.Sensitive = enabled;
            mainItemList!.Sensitive = enabled;
            recentModpacksList!.Sensitive = enabled;
        }

        private void CheckRomAndModpackLoaded()
        {
            if (modpack == null || rom == null)
            {
                throw new Exception("The modpack or rom is not loaded");
            }

            if (modpack.Metadata.Id == null)
            {
                throw new Exception("Missing ID. Please set a modpack ID in the modpack settings.");
            }
        }

        private void AddModpackToRecents()
        {
            var settings = Settings.Load();
            if (modpack != null && modpack.Directory != null)
            {
                settings.AddRecentModpack(modpack.Metadata.Name ?? modpack.Metadata.Id ?? modpack.Directory,
                    modpack.Directory);
            }
            settings.Save();
            UpdateRecentModpacks(settings);
        }

        private void UpdateRecentModpacks(Settings settings)
        {
            recentModpacksStore!.Clear();
            foreach (var recentModpack in settings?.RecentModpacks?.Take(20)
                ?? Enumerable.Empty<(string nameOrId, string path)>())
            {
                recentModpacksStore.AppendValues($"{recentModpack.nameOrId} ({recentModpack.path})", recentModpack.path);
            }
        }

        public TreeIter AddMainListItem<T>(string name, string icon, ControllerContext? context = null) where T : Widget
        {
            return itemStore!.AppendValues(icon, name, typeof(T), context ?? ControllerContext.Null, false);
        }

        public TreeIter AddMainListItem<T>(TreeIter parent, string name, string icon, ControllerContext? context = null)
            where T : Widget
        {
            return itemStore!.AppendValues(parent, icon, name, typeof(T), context ?? ControllerContext.Null, false);
        }

        public TreeIter AddMainListItem(string name, string icon, ControllerContext? context = null)
        {
            return itemStore!.AppendValues(icon, name, null, context ?? ControllerContext.Null, false);
        }

        public TreeIter AddMainListItem(TreeIter parent, string name, string icon, ControllerContext? context = null)
        {
            return itemStore!.AppendValues(parent, icon, name, null, context ?? ControllerContext.Null, false);
        }
    }
}
