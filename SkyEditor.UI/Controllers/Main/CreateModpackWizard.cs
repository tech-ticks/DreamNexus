using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditorUI.Infrastructure;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;

namespace SkyEditorUI.Controllers
{
  class CreateModpackWizard : Dialog
  {
    [UI] private Entry? modpackIdEntry;
    [UI] private Entry? modpackNameEntry;
    [UI] private Entry? modpackAuthorEntry;

    public string ModpackId => modpackIdEntry?.Text.Trim() ?? "";
    public string ModpackName => modpackNameEntry?.Text.Trim() ?? "";
    public string ModpackAuthor => modpackAuthorEntry?.Text.Trim() ?? "";

    private string prevName = "";
    private string prevAuthor = "";

    public CreateModpackWizard() : this(new Builder("CreateModpackWizard.glade"))
    {
    }

    private CreateModpackWizard(Builder builder) : base(builder.GetRawOwnedObject("main"))
    {
      builder.Autoconnect(this);
    }

    private void OnNameAuthorChanged(object sender, EventArgs args)
    {
      if (!string.IsNullOrWhiteSpace(ModpackName) && !string.IsNullOrWhiteSpace(ModpackAuthor))
      {
        string? generatedId = Modpack.GenerateId(ModpackName, ModpackAuthor);
        if (generatedId != null
          && (string.IsNullOrWhiteSpace(ModpackId) || ModpackId == Modpack.GenerateId(prevName, prevAuthor)))
        {
          modpackIdEntry!.Text = generatedId;
        }
      }

      prevName = ModpackName;
      prevAuthor = ModpackAuthor;
    }

    private void OkClicked(object sender, EventArgs args)
    {
      if (Modpack.IsValidId(ModpackId))
      {
        Respond(ResponseType.Accept);
      }
      else
      {
        UIUtils.ShowErrorDialog(this, "Malformed modpack ID",
          "Modpack IDs must use the format: authorname.modpackname\n"
          + "Only lowercase letters, digits and \".\" are allowed.");
      }
    }
  }
}
