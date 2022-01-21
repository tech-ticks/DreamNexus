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
        [UI] private Switch? enableCodeInjectionSwitch;

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

            enableCodeInjectionSwitch!.Active = modpack.Metadata.EnableCodeInjection;
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
            modpack.Metadata.EnableCodeInjection = args.State;
        }
    }
}
