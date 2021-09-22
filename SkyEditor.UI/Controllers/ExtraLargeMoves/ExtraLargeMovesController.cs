using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI.Controllers
{
    class ExtraLargeMovesController : Widget
    {
        [UI] private ListStore? extraLargeMovesStore;
        [UI] private TreeView? extraLargeMovesTree;

        [UI] private ListStore? movesStore;
        [UI] private EntryCompletion? movesCompletion;

        private IExtraLargeMoveCollection extraLargeMoves;
        private IRtdxRom rom;

        private const int IndexColumn = 0;
        private const int BaseMoveColumn = 1;
        private const int BaseActionColumn = 2;
        private const int LargeMoveColumn = 3;
        private const int LargeActionColumn = 4;

        public ExtraLargeMovesController(IRtdxRom rom) : this(new Builder("ExtraLargeMoves.glade"), rom)
        {
        }

        private ExtraLargeMovesController(Builder builder, IRtdxRom rom) : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            this.rom = rom;
            this.extraLargeMoves = rom.GetExtraLargeMoveCollection();

            movesStore!.AppendAll(AutocompleteHelpers.GetMoves(rom));

            for (int i = 0; i < extraLargeMoves.Entries.Count; i++)
            {
                AddToStore(extraLargeMoves.Entries[i], i);
            }
        }

        private void AddToStore(ExtraLargeMoveModel model, int index)
        {
            extraLargeMovesStore!.AppendValues(
                index,
                AutocompleteHelpers.FormatMove(rom, model.BaseMove),
                (int) model.BaseAction,
                AutocompleteHelpers.FormatMove(rom, model.LargeMove),
                (int) model.LargeAction
            );
        }

        private void OnBaseMoveEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (extraLargeMovesStore!.GetIter(out var iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    extraLargeMovesStore.SetValue(iter, BaseMoveColumn,
                    AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    extraLargeMoves.Entries[path.Indices[0]].BaseMove = moveIndex.Value;
                }
            }
        }

        private void OnBaseMoveEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = movesCompletion;
        }

        private void OnLargeMoveEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = movesCompletion;
        }

        private void OnBaseActionIdEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (extraLargeMovesStore!.GetIter(out var iter, path))
            {
                var entry = extraLargeMoves.Entries[path.Indices[0]];
                if (ushort.TryParse(args.NewText, out ushort value))
                {
                    entry.BaseAction = value;
                }
                extraLargeMovesStore.SetValue(iter, BaseActionColumn, (int) entry.BaseAction);
            }
        }

        private void OnLargeMoveEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (extraLargeMovesStore!.GetIter(out var iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    extraLargeMovesStore.SetValue(iter, LargeMoveColumn,
                    AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    extraLargeMoves.Entries[path.Indices[0]].LargeMove = moveIndex.Value;
                }
            }
        }

        private void OnLargeActionIdEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (extraLargeMovesStore!.GetIter(out var iter, path))
            {
                var entry = extraLargeMoves.Entries[path.Indices[0]];
                if (ushort.TryParse(args.NewText, out ushort value))
                {
                    entry.LargeAction = value;
                }
                extraLargeMovesStore.SetValue(iter, LargeActionColumn, (int) entry.LargeAction);
            }
        }

        private void OnAddClicked(object sender, EventArgs args)
        {
            var entry = new ExtraLargeMoveModel();
            extraLargeMoves.Entries.Add(entry);
            AddToStore(entry, extraLargeMoves.Entries.Count - 1);
        }

        private void OnRemoveClicked(object sender, EventArgs args)
        {
            if (extraLargeMovesTree!.Selection.GetSelected(out var model, out var iter))
            {
                var path = model.GetPath(iter);
                int index = path.Indices[0];
                extraLargeMoves.Entries.RemoveAt(index);
                var store = (ListStore) model;
                store.Remove(ref iter);
                store.FixIndices(IndexColumn);
            }
        }
    }
}
