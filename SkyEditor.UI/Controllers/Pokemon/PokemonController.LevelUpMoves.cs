using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditorUI.Infrastructure;
using System;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditorUI.Controllers
{
    partial class PokemonController : Widget
    {
        [UI] private ListStore? levelUpMovesStore;
        [UI] private EntryCompletion? moveCompletion;

        const int LevelUpMoveLevelColumn = 0;
        const int LevelUpMoveIdColumn = 1;
        const int LevelUpMoveNameColumn = 2;

        private void LoadLevelUpMovesTab()
        {
            foreach (var move in pokemon.LevelupLearnset)
            {
                levelUpMovesStore!.AppendValues(move.Level, (int) move.Move,
                    AutocompleteHelpers.FormatMove(rom, move.Move));
            }
        }

        private void OnLevelUpMoveLevelEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (levelUpMovesStore!.GetIter(out var iter, path))
            {
                var move = pokemon.LevelupLearnset[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    move.Level = value;
                    levelUpMovesStore.SetValue(iter, LevelUpMoveLevelColumn, value);
                }
            }
        }

        private void OnLevelUpMoveEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = moveCompletion;
        }

        private void OnLevelUpMoveNameEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (levelUpMovesStore!.GetIter(out var iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    levelUpMovesStore.SetValue(iter, LevelUpMoveIdColumn, (int) moveIndex.Value);
                    levelUpMovesStore.SetValue(iter, LevelUpMoveNameColumn,
                        AutocompleteHelpers.FormatMove(rom, moveIndex.Value));

                    var move = pokemon.LevelupLearnset[path.Indices[0]];
                    move.Move = moveIndex.Value;
                }
            }
        }
    }
}
