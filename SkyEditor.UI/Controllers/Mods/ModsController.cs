using System;
using System.IO;
using IOPath = System.IO.Path;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditorUI.Infrastructure;
using SkyEditor.IO.FileSystem;
using System.Linq;

namespace SkyEditorUI.Controllers
{
    class ModsController : Widget
    {
        public static string ModsDirectory = IOPath.Combine(Directory.GetCurrentDirectory(), "Mods");

        private static int StatusColumn = 3;
        private static int ModpackColumn = 4;

        private IRtdxRom rom;
        private Modpack modpack;

        [UI] private ListStore? modsStore;
        [UI] private TreeView? modsTree;
        [UI] private Button? toggleButton;

        private bool addingMod;

        public ModsController(IRtdxRom rom, Modpack modpack)
            : this(new Builder("Mods.glade"), rom, modpack)
        {
        }

        private ModsController(Builder builder, IRtdxRom rom, Modpack modpack) 
            : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);
            
            this.rom = rom;
            this.modpack = modpack;
            
            Refresh();
        }

        private void OnRefreshClicked(object sender, EventArgs args)
        {
            if (addingMod)
            {
                return;
            }

            Refresh();
        }

        private void OnToggleClicked(object sender, EventArgs args)
        {
            if (addingMod)
            {
                return;
            }

            if (!modsTree!.Selection.GetSelected(out var model, out var iter))
            {
                return;
            }

            var selectedModpack = model.GetValue(iter, ModpackColumn) as Modpack;
            if (selectedModpack == null || selectedModpack.Mods == null || selectedModpack.Mods.Count == 0)
            {
                throw new Exception("The modpack is empty, there are no mods to add.");
            }

            if (!IsModpackIncluded(selectedModpack)) 
            {
                model.SetValue(iter, StatusColumn, "Adding...");
                AddModsFromModpack(selectedModpack, () =>
                {
                    model.SetValue(iter, StatusColumn, "Added");
                    toggleButton!.Label = "Remove";
                  });
            }
            else
            {
                foreach (var mod in selectedModpack.Mods)
                {
                    if (mod.Metadata.Id == null)
                    {
                        throw new Exception("Encountered a mod with a null ID");
                    }
                    modpack.RemoveMod(mod.Metadata.Id);
                }
                model.SetValue(iter, StatusColumn, "Not added");
                toggleButton!.Label = "Add";
                
                UIUtils.ShowInfoDialog(MainWindow.Instance, "Mod removed", "Please save and re-open the modpack to apply the changes.");
                MainWindow.Instance?.InitMainList();
            }
        }

        private void OnBrowseClicked(object sender, EventArgs args)
        {
            UIUtils.ShowWarningDialog(MainWindow.Instance, "Warning", "Mods can execute arbitrary code. Only add mods "
                + "from trusted sources!");

            var dialog = new FileChooserNative("Add mod or modpack", MainWindow.Instance,
                FileChooserAction.Open | FileChooserAction.SelectFolder, null, null);
            var response = (ResponseType) dialog.Run();
            string path = dialog.File.Path;
            dialog.Dispose();

            if (response == ResponseType.Accept)
            {
                var modpack = new RtdxModpack(path, PhysicalFileSystem.Instance);
                AddModsFromModpack(modpack);
            }
        }

        private void OnSelectionChanged(object sender, EventArgs args)
        {
            toggleButton!.Sensitive = false;
            toggleButton!.Label = "Add";

            var selection = (TreeSelection) sender;
            if (selection.GetSelected(out ITreeModel model, out var iter))
            {
                var status = model.GetValue(iter, StatusColumn) as string;
                if (status == "Not added")
                {
                    toggleButton!.Sensitive = true;
                    toggleButton!.Label = "Add";
                }
                else if (status == "Added")
                {
                    toggleButton!.Sensitive = true;
                    toggleButton!.Label = "Remove";
                }
            }
        }

        private void OnOpenModDirectoryClicked(object sender, EventArgs args)
        {
            UIUtils.OpenInFileBrowser(ModsDirectory);
        }
        
        private void Refresh()
        {
            Directory.CreateDirectory(ModsDirectory);

            modsStore!.Clear();
            foreach (var dir in Directory.EnumerateDirectories(ModsDirectory))
            {
                var tempModpack = new RtdxModpack(dir, PhysicalFileSystem.Instance);
                var metadata = tempModpack.Metadata;
                modsStore.AppendValues(metadata.Name ?? metadata.Id, metadata.Author ?? "", metadata.Description ?? "",
                    IsModpackIncluded(tempModpack) ? "Added" : "Not added", tempModpack);
            }
        }

        private async void AddModsFromModpack(Modpack otherModpack, System.Action? onFinished = null)
        {
            addingMod = true;
            toggleButton!.Label = "Adding...";
            toggleButton!.Sensitive = false;
            MainWindow.Instance?.SetTopButtonsEnabled(false);

            Exception? exception = null;
            try
            {
                foreach (var mod in otherModpack.Mods ?? Enumerable.Empty<Mod>())
                {
                    await modpack.AddMod(mod).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            GLib.Idle.Add(() =>
            {
                // We have to use the idle callback here because for some reason, it otherwise crashes because
                // we're not on the main thread although ConfigureAwait is used
                addingMod = false;

                MainWindow.Instance?.InitMainList();
                MainWindow.Instance?.SetTopButtonsEnabled(true);
                toggleButton!.Label = "Add";
                toggleButton!.Sensitive = true;

                if (exception != null)
                {
                    throw exception;
                }

                UIUtils.ShowInfoDialog(MainWindow.Instance, "Mod added", "Please save and re-open the modpack to apply the changes.");
                onFinished?.Invoke();
                return false;
            });
        }

        private bool IsModpackIncluded(Modpack otherModpack)
        {
            // We consider a modpack as included if the main modpack contains at least one of the mod IDs
            foreach (var mod in otherModpack.Mods ?? Enumerable.Empty<Mod>())
            {
                if (modpack.Mods != null && modpack.Mods.Any(addedMod => addedMod.Metadata.Id == mod.Metadata.Id))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
