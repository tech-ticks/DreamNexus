using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI.Controllers
{
    partial class MoveController : Widget
    {
        [UI] private Label? labelIdName;
        [UI] private Entry? entryActionId;
        [UI] private ComboBoxText? cbLanguage;
        [UI] private Entry? entryName;
        [UI] private TextView? tvDescription;
        [UI] private Entry? entryShort00;
        [UI] private Entry? entryShort02;
        [UI] private Entry? entryShort04;
        [UI] private Entry? entryShort0E;
        [UI] private Entry? entryByte10;
        [UI] private Entry? entryByte11;

        private WazaDataInfo.Entry move;
        private IRtdxRom rom;
        private Modpack modpack;

        private LocalizedStringCollection? strings;

        public MoveController(IRtdxRom rom, Modpack modpack, ControllerContext context)
            : this(new Builder("Move.glade"), rom, modpack, context)
        {
        }

        private MoveController(Builder builder, IRtdxRom rom, Modpack modpack, ControllerContext context) 
            : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);
            
            var moveId = (context as MoveControllerContext)!.Index;

            this.move = rom.GetMoves().GetMoveById(moveId)
                ?? throw new ArgumentException("Item from context ID is null", nameof(context));
            this.rom = rom;
            this.modpack = modpack;

            var englishStrings = rom.GetStrings().English;

            string formattedId = ((int) moveId).ToString("0000");
            string? name = englishStrings.GetMoveName(moveId);
            labelIdName!.Text = $"#{formattedId}: {name}\n({moveId.ToString()})";

            entryActionId!.Text = move.ActIndex.ToString();
            entryShort00!.Text = move.Short00.ToString();
            entryShort02!.Text = move.Short02.ToString();
            entryShort04!.Text = move.Short04.ToString();
            entryShort0E!.Text = move.Short0E.ToString();
            entryByte10!.Text = move.Byte10.ToString();
            entryByte11!.Text = move.Byte11.ToString();

            LoadText(LanguageType.EN);
        }

        private void OnActionIdChanged(object sender, EventArgs args)
        {
            move.ActIndex = entryActionId!.ParseUShort(move.ActIndex);
        }

        private void OnLanguageChanged(object sender, EventArgs args)
        {
            LoadText((LanguageType) cbLanguage!.Active);
        }

        private void OnNameChanged(object sender, EventArgs args)
        {
            strings!.SetCommonString(strings.GetMoveNameHash(move.Index), entryName!.Text);
        }

        private void OnDescriptionChanged(object sender, EventArgs args)
        {
            strings!.SetCommonString(strings.GetMoveDescriptionHash(move.Index), tvDescription!.Buffer!.Text);
        }
        
        private void LoadText(LanguageType language)
        {
            strings = rom.GetStrings().GetStringsForLanguage(language);
            entryName!.Text = strings.GetMoveName(move.Index);
            tvDescription!.Buffer.Text = strings.GetMoveDescription(move.Index);
        }

        private void OnShort00Changed(object sender, EventArgs args)
        {
            move.Short00 = entryShort00!.ParseUShort(move.Short00);
        }

        private void OnShort02Changed(object sender, EventArgs args)
        {
            move.Short02 = entryShort02!.ParseUShort(move.Short02);
        }

        private void OnShort04Changed(object sender, EventArgs args)
        {
            move.Short04 = entryShort04!.ParseUShort(move.Short04);
        }

        private void OnShort0EChanged(object sender, EventArgs args)
        {
            move.Short0E = entryShort0E!.ParseUShort(move.Short0E);
        }

        private void OnByte10Changed(object sender, EventArgs args)
        {
            move.Byte10 = entryByte10!.ParseByte(move.Byte10);
        }

        private void OnByte11Changed(object sender, EventArgs args)
        {
            move.Byte11 = entryByte11!.ParseByte(move.Byte11);
        }

        private void OnHelpByte10Clicked(object sender, EventArgs args)
        {
            var dialog = new MessageDialog(MainWindow.Instance, DialogFlags.DestroyWithParent, MessageType.Info,
                            ButtonsType.Ok, "Seems to indicate if the move is usable by the player.");
            dialog.Title = "Unknown Byte10";
            dialog.Run();
            dialog.Destroy();
        }

    }
}
