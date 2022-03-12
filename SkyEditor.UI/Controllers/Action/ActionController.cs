using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.ActDataInfo;

namespace SkyEditorUI.Controllers
{
    partial class ActionController : Widget
    {
        [UI] private Label? labelIdName;
        [UI] private ComboBoxText? cbKind;
        [UI] private ComboBoxText? cbCategory;
        [UI] private ComboBox? cbType;

        [UI] private ListStore? typesStore;

        private ActionModel action;
        private IRtdxRom rom;
        private Modpack modpack;
        private Builder builder;
        private LocalizedStringCollection englishStrings;

        public ActionController(IRtdxRom rom, Modpack modpack, ControllerContext context)
            : this(new Builder("Action.glade"), rom, modpack, context)
        {
        }

        private ActionController(Builder builder, IRtdxRom rom, Modpack modpack, ControllerContext context)
            : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            var actionId = (context as ActionControllerContext)!.Index;

            this.action = rom.GetActions().GetActionById(actionId)
                ?? throw new ArgumentException("Action from context ID is null", nameof(context));
            this.rom = rom;
            this.modpack = modpack;
            this.builder = builder;

            englishStrings = rom.GetStrings().English;

            string formattedId = actionId.ToString("0000");
            string? name = rom.GetActions().GetUsedByString(actionId);
            labelIdName!.Text = $"#{formattedId}: {name}";

            for (PokemonType i = 0; i <= PokemonType.NASHI; i++)
            {
                typesStore!.AppendValues((int)i, englishStrings.GetPokemonTypeName(i) ?? $"({i.ToString()})");
            }

            cbKind!.Active = (int)action.Kind;
            cbCategory!.Active = (int)action.MoveCategory;
            cbType!.Active = (int)action.MoveType;

            LoadGeneralTab();
            LoadEffectsTab();
            LoadFlagsTab();
            LoadTextTab();
            LoadVisualsTab();
        }

        private void OnKindChanged(object sender, EventArgs args)
        {
            action.Kind = (ActionKind)cbKind!.Active;
        }

        private void OnCategoryChanged(object sender, EventArgs args)
        {
            action.MoveCategory = (MoveCategory)cbCategory!.Active;
        }

        private void OnTypeChanged(object sender, EventArgs args)
        {
            action.MoveType = (PokemonType)cbType!.Active;
        }
    }
}
