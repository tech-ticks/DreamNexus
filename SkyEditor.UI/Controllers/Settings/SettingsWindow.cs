using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using Settings = SkyEditorUI.Infrastructure.Settings;
using SkyEditorUI.Infrastructure;

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
