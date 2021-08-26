using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

namespace SkyEditorUI.Controllers
{
    partial class ItemController : Widget
    {
        [UI] private ComboBoxText? cbLanguage;
        [UI] private Entry? entryNameSingular;
        [UI] private Entry? entryNamePlural;
        [UI] private TextView? tvDescription;

        private LocalizedStringCollection? strings;

        private void LoadTextTab()
        {
            LoadText(LanguageType.EN);
        }

        private void OnLanguageChanged(object sender, EventArgs args)
        {
            LoadText((LanguageType) cbLanguage!.Active);
        }

        private void OnNameSingularChanged(object sender, EventArgs args)
        {
            strings!.SetCommonString(strings.GetItemNameHash(internalName, false), entryNameSingular!.Text);
        }

        private void OnNamePluralChanged(object sender, EventArgs args)
        {
            strings!.SetCommonString(strings.GetItemNameHash(internalName, true), entryNamePlural!.Text);
        }

        private void OnDescriptionChanged(object sender, EventArgs args)
        {
            strings!.SetCommonString(strings.GetItemDescriptionHash(internalName), tvDescription!.Buffer!.Text);
        }
        
        private void LoadText(LanguageType language)
        {
            strings = rom.GetStrings().GetStringsForLanguage(language);
            entryNameSingular!.Text = strings.GetItemNameByInternalName(internalName, false);
            entryNamePlural!.Text = strings.GetItemNameByInternalName(internalName, true);
            tvDescription!.Buffer.Text = strings.GetItemDescriptionByInternalName(internalName);
        }
    }
}
