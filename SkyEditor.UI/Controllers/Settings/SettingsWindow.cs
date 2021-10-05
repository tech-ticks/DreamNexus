using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using Settings = SkyEditorUI.Infrastructure.Settings;
using SkyEditorUI.Infrastructure;
using System.Threading;
using System.Runtime.ExceptionServices;

namespace SkyEditorUI.Controllers
{
    class SettingsDialog : Dialog
    {
        [UI] private Entry? rtdxRomPathEntry;
        [UI] private Entry? ipAddressEntry;
        [UI] private Entry? portEntry;
        [UI] private Entry? ftpUserEntry;
        [UI] private Entry? ftpPasswordEntry;
        [UI] private ComboBox? buildFileStructureComboBox;
        [UI] private Dialog? loadingDialog;
        [UI] private Label? loadingLabel;

        private Settings? settings;

        public SettingsDialog() : this(new Builder("Settings.glade")) { }

        private SettingsDialog(Builder builder) : base(builder.GetRawOwnedObject("dialog_settings"))
        {
            builder.Autoconnect(this);

            settings = Settings.Load();
            rtdxRomPathEntry!.Text = settings.RtdxRomPath ?? "";
            ipAddressEntry!.Text = settings.SwitchIp ?? "";
            portEntry!.Text = settings.SwitchFtpPort ?? "";
            ftpUserEntry!.Text = settings.SwitchFtpUser ?? "";
            ftpPasswordEntry!.Text = settings.SwitchFtpPassword ?? "";
            buildFileStructureComboBox!.Active = (int) settings.BuildFileStructure;
        }

        private void OnBrowseRtdxRomPathClick(object sender, EventArgs args)
        {
            var dialog = new FileChooserNative("Select unpacked Rescue Team DX ROM", this, FileChooserAction.Open | FileChooserAction.SelectFolder, null, null);
            var response = (ResponseType) dialog.Run();

            if (response == ResponseType.Accept)
            {
                string folder = dialog.Filename;
                rtdxRomPathEntry!.Text = folder;
            }
        }

        private void OnBrowseRtdxRomFileClick(object sender, EventArgs args)
        {
            UIUtils.ShowInfoDialog(this, "DreamNexus", "A file with decryption keys must be provided to extract "
                + "encrypted Switch ROMs. Provide a keys file (commonly named \"prod.keys\" or \"keys.txt\").");

            var keysDialog = new FileChooserNative("Select prod.keys/keys.txt file", this, FileChooserAction.Open,
                null, null);
            var response = (ResponseType) keysDialog.Run();
            var filter = new FileFilter();
            filter.AddPattern("*.keys");
            filter.AddPattern("*.txt");
            keysDialog.AddFilter(filter);

            if (response != ResponseType.Accept)
            {
                return;
            }

            string keysFile = keysDialog.Filename;
            keysDialog.Destroy();

            UIUtils.ShowInfoDialog(this, "DreamNexus", "Select the ROM file (.xci) you want to unpack. The ROM will be "
                + "unpacked into a folder next to its location with the same name. Please ensure that you have "
                + "at least around 10 GB of free disk space before continuing.");

            var romDialog = new FileChooserNative("Select ROM file", this, FileChooserAction.Open,
                null, null);
            response = (ResponseType) romDialog.Run();
            filter = new FileFilter();
            filter.AddPattern("*.xci");
            romDialog.AddFilter(filter);

            if (response != ResponseType.Accept)
            {
                return;
            }

            string romFile = romDialog.Filename;
            romDialog.Destroy();

            if (!romFile.EndsWith(".xci"))
            {
                UIUtils.ShowErrorDialog(this, "Error", "The ROM file must be a .xci file.");
                return;
            }

            loadingDialog!.Show();
            loadingLabel!.Text = "Unpacking ROM...";

            var thread = new Thread(() =>
            {
                Exception? exception = null;
                string path = "";
                try
                {
                    path = RomExtraction.UnpackRom(romFile, keysFile, (progressString) =>
                    {
                        GLib.Idle.Add(() =>
                        {
                            loadingLabel!.Text = progressString;
                            return false;
                        });
                    });
                }
                catch (Exception e)
                {
                    exception = e;
                }

                GLib.Idle.Add(() =>
                {
                    loadingDialog!.Hide();
                    if (exception != null)
                    {
                        ExceptionDispatchInfo.Capture(exception).Throw();
                    }
                    else
                    {
                        rtdxRomPathEntry!.Text = path;
                    }
                    return false;
                });
            });
            thread.Start();
        }

        private void OnApplyClicked(object sender, EventArgs args)
        {
            settings!.RtdxRomPath = rtdxRomPathEntry!.Text;
            settings.SwitchIp = ipAddressEntry!.Text;
            settings.SwitchFtpPort = portEntry!.Text;
            settings.SwitchFtpUser = ftpUserEntry!.Text;
            settings.SwitchFtpPassword = ftpPasswordEntry!.Text;
            settings.BuildFileStructure = (BuildFileStructureType) buildFileStructureComboBox!.Active;

            settings.Save();
            Respond(ResponseType.Accept);
        }
    }
}
