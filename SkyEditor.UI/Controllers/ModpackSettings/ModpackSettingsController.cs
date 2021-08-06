using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using Settings = SkyEditorUI.Infrastructure.Settings;
using SkyEditorUI.Infrastructure;
using System.Threading;
using System.Linq;

namespace SkyEditorUI.Controllers
{
    class ModpackSettingsController : Widget
    {
        [UI] private Entry? idEntry;
        [UI] private Entry? nameEntry;
        [UI] private Entry? authorEntry;
        [UI] private Entry? versionEntry;
        [UI] private TextBuffer? descriptionBuffer;

        [UI] private Stack? stack;
        [UI] private Alignment? mainView;
        [UI] private Alignment? downloadingView;
        [UI] private Switch? enableCodeInjectionSwitch;
        [UI] private ListStore? codeInjectionVersionsStore;
        [UI] private ComboBox? codeInjectionVersionsComboBox;
        [UI] private ComboBoxText? codeInjectionReleaseTypeComboBox;

        private Modpack modpack;

        public ModpackSettingsController(IRtdxRom rom, Modpack modpack)
            : this(new Builder("ModpackSettings.glade"), rom, modpack)
        {
        }

        private ModpackSettingsController(Builder builder, IRtdxRom rom, Modpack modpack) : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            this.modpack = modpack;
            idEntry!.Text = modpack.Metadata.Id ?? "";
            nameEntry!.Text = modpack.Metadata.Name ?? "";
            authorEntry!.Text = modpack.Metadata.Author ?? "";
            versionEntry!.Text = modpack.Metadata.Version ?? "";
            descriptionBuffer!.Text = modpack.Metadata.Description ?? "";

            var versions = CodeInjectionHelpers.GetAvailableVersions(Settings.DataPath);
            AddVersionsToStore(versions);

            bool versionInstalled = versions.Contains(modpack.Metadata.CodeInjectionVersion);

            if (string.IsNullOrWhiteSpace(modpack.Metadata.CodeInjectionVersion))
            {
                modpack.Metadata.EnableCodeInjection = false;
            }

            enableCodeInjectionSwitch!.Active = modpack.Metadata.EnableCodeInjection;

            if (modpack.Metadata.EnableCodeInjection && !versionInstalled)
            {
                GLib.Idle.Add(() =>
                {
                    UIUtils.ShowErrorDialog(MainWindow.Instance, "Warning", "Loaded a modpack with an unavailable code "
                        + $"injection version {modpack.Metadata.CodeInjectionVersion}. "
                        + "Click \"Update code injection binaries\" to fix this.");
                    return false;
                });
            }
        }

        private void OnIdFocusOut(object sender, FocusOutEventArgs args)
        {
            if (Modpack.IsValidId(idEntry!.Text))
            {
                modpack.Metadata.Id = idEntry!.Text;
            }
            else
            {
                idEntry!.Text = modpack.Metadata.Id;
            }
        }

        private void OnNameChanged(object sender, EventArgs args)
        {
            modpack.Metadata.Name = nameEntry!.Text;
        }

        private void OnAuthorChanged(object sender, EventArgs args)
        {
            modpack.Metadata.Author = authorEntry!.Text;
        }

        private void OnVersionChanged(object sender, EventArgs args)
        {
            modpack.Metadata.Version = versionEntry!.Text;
        }

        private void OnDescriptionChanged(object sender, EventArgs args)
        {
            modpack.Metadata.Description = descriptionBuffer!.Text;
        }

        [GLib.ConnectBefore] // Required for some reason, otherwise the signal is ignored
        private void OnEnableCodeInjectionStateSet(object sender, StateSetArgs args)
        {
            if (!args.State)
            {
                codeInjectionVersionsComboBox!.Sensitive = false;
                codeInjectionReleaseTypeComboBox!.Sensitive = false;
                modpack.Metadata.EnableCodeInjection = false;
            }

            if (args.State)
            {
                if (!CodeInjectionHelpers.HasDownloadedBinaries(Settings.DataPath))
                {
                    RedownloadCodeInjectionBinaries(() =>
                    {
                        OnEnableCodeInjection();
                    });
                }
                else
                {
                    OnEnableCodeInjection();
                }
            }
        }

        private void OnEnableCodeInjection()
        {
            codeInjectionVersionsComboBox!.Sensitive = true;
            codeInjectionReleaseTypeComboBox!.Sensitive = true;
            modpack.Metadata.EnableCodeInjection = true;
            var versions = CodeInjectionHelpers.GetAvailableVersions(Settings.DataPath);
            AddVersionsToStore(versions);
            modpack.Metadata.CodeInjectionVersion = versions[codeInjectionVersionsComboBox!.Active];
            modpack.Metadata.CodeInjectionReleaseType = codeInjectionReleaseTypeComboBox.Active != 0
                ? "release" : "debug";
        }

        private void OnCodeInjectionVersionChanged(object sender, EventArgs args)
        {
            var versions = CodeInjectionHelpers.GetAvailableVersions(Settings.DataPath);
            int active = codeInjectionVersionsComboBox!.Active;
            if (active >= 0 && active < versions.Length)
            {
                modpack.Metadata.CodeInjectionVersion = versions[active];
            }
        }

        private void OnCodeInjectionReleaseTypeChanged(object sender, EventArgs args)
        {
            modpack.Metadata.CodeInjectionReleaseType = codeInjectionReleaseTypeComboBox!.Active != 0
                ? "release" : "debug";
        }

        private void OnRedownloadInjectionBinariesClicked(object sender, EventArgs args)
        {
            RedownloadCodeInjectionBinaries(() =>
            {
                var versions = CodeInjectionHelpers.GetAvailableVersions(Settings.DataPath);
                AddVersionsToStore(versions);
            });
        }

        private void AddVersionsToStore(string[] versions)
        {
            codeInjectionVersionsStore!.Clear();
            if (versions.Length > 0)
            {
                codeInjectionVersionsStore.AppendAll(versions);
                if (codeInjectionVersionsComboBox!.Active == -1)
                {
                    var metadataVersionIndex = Array.IndexOf(versions, modpack.Metadata.CodeInjectionVersion);
                    if (metadataVersionIndex != -1)
                    {
                        codeInjectionVersionsComboBox!.Active = metadataVersionIndex;
                    }
                    else
                    {
                        codeInjectionVersionsComboBox.Active = 0;
                    }
                }
            }
        }

        private void RedownloadCodeInjectionBinaries(System.Action onCompleted)
        {
            stack!.VisibleChild = downloadingView;
            MainWindow.Instance?.SetTopButtonsEnabled(false);

            new Thread(() =>
            {
                Exception? exception = null;
                try
                {
                    string repository = modpack.Metadata.CodeInjectionRepository 
                        ?? ModpackMetadata.DefaultCodeInjectionRepository;
                    CodeInjectionHelpers.DownloadBinaries(repository, Settings.DataPath);
                }
                catch (Exception e)
                {
                    exception = e;
                }

                GLib.Idle.Add(() =>
                {
                    stack!.VisibleChild = mainView;
                    MainWindow.Instance?.SetTopButtonsEnabled(true);

                    if (exception != null)
                    {
                        throw exception;
                    }
                    onCompleted();
                    return false;
                });
            }).Start();
        }
    }
}
