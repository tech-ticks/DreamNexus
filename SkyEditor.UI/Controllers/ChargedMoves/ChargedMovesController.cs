using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI.Controllers
{
    class ChargedMovesController : Widget
    {
        [UI] private ListStore? chargedMovesStore;
        [UI] private TreeView? chargedMovesTree;

        [UI] private ListStore? movesStore;
        [UI] private EntryCompletion? movesCompletion;

        private IChargedMoveCollection chargedMoves;
        private IRtdxRom rom;

        private const int IndexColumn = 0;
        private const int BaseMoveColumn = 1;
        private const int BaseActionColumn = 2;
        private const int FinalMoveColumn = 3;
        private const int FinalActionColumn = 4;
        private const int Short08Column = 5;

        public ChargedMovesController(IRtdxRom rom) : this(new Builder("ChargedMoves.glade"), rom)
        {
        }

        private ChargedMovesController(Builder builder, IRtdxRom rom) : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            this.rom = rom;
            this.chargedMoves = rom.GetChargedMoveCollection();

            movesStore!.AppendAll(AutocompleteHelpers.GetMoves(rom));

            for (int i = 0; i < chargedMoves.Entries.Count; i++)
            {
                AddToStore(chargedMoves.Entries[i], i);
            }
        }

        private void AddToStore(ChargedMoveModel model, int index)
        {
            chargedMovesStore!.AppendValues(
                index,
                AutocompleteHelpers.FormatMove(rom, model.BaseMove),
                (int) model.BaseAction,
                AutocompleteHelpers.FormatMove(rom, model.FinalMove),
                (int) model.FinalAction,
                (int) model.Short08
            );
        }

        private void OnBaseMoveEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (chargedMovesStore!.GetIter(out var iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    chargedMovesStore.SetValue(iter, BaseMoveColumn,
                    AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    chargedMoves.Entries[path.Indices[0]].BaseMove = moveIndex.Value;
                }
            }
        }

        private void OnBaseMoveEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = movesCompletion;
        }

        private void OnFinalMoveEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = movesCompletion;
        }

        private void OnBaseActionIdEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (chargedMovesStore!.GetIter(out var iter, path))
            {
                var entry = chargedMoves.Entries[path.Indices[0]];
                if (ushort.TryParse(args.NewText, out ushort value))
                {
                    entry.BaseAction = value;
                }
                chargedMovesStore.SetValue(iter, BaseActionColumn, (int) entry.BaseAction);
            }
        }

        private void OnFinalMoveEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (chargedMovesStore!.GetIter(out var iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    chargedMovesStore.SetValue(iter, FinalMoveColumn,
                    AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    chargedMoves.Entries[path.Indices[0]].FinalMove = moveIndex.Value;
                }
            }
        }

        private void OnFinalActionIdEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (chargedMovesStore!.GetIter(out var iter, path))
            {
                var entry = chargedMoves.Entries[path.Indices[0]];
                if (ushort.TryParse(args.NewText, out ushort value))
                {
                    entry.FinalAction = value;
                }
                chargedMovesStore.SetValue(iter, FinalActionColumn, (int) entry.FinalAction);
            }
        }

        private void OnShort08Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (chargedMovesStore!.GetIter(out var iter, path))
            {
                var entry = chargedMoves.Entries[path.Indices[0]];
                if (ushort.TryParse(args.NewText, out ushort value))
                {
                    entry.Short08 = value;
                }
                chargedMovesStore.SetValue(iter, Short08Column, (int) entry.Short08);
            }
        }

        private void OnAddClicked(object sender, EventArgs args)
        {
            var entry = new ChargedMoveModel();
            chargedMoves.Entries.Add(entry);
            AddToStore(entry, chargedMoves.Entries.Count - 1);
        }

        private void OnRemoveClicked(object sender, EventArgs args)
        {
            if (chargedMovesTree!.Selection.GetSelected(out var model, out var iter))
            {
                var path = model.GetPath(iter);
                int index = path.Indices[0];
                chargedMoves.Entries.RemoveAt(index);
                var store = (ListStore) model;
                store.Remove(ref iter);
                store.FixIndices(IndexColumn);
            }
        }
    }
}
