using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditorUI.Infrastructure;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditorUI.Controllers
{
    class FixedPokemonController : Widget
    {
        [UI] private ListStore? fixedPokemonStore;
        [UI] private TreeView? fixedPokemonTree;

        [UI] private ListStore? movesStore;
        [UI] private ListStore? dungeonsStore;
        [UI] private ListStore? speciesStore;
        [UI] private EntryCompletion? movesCompletion;
        [UI] private EntryCompletion? dungeonsCompletion;
        [UI] private EntryCompletion? speciesCompletion;

        private IFixedPokemonCollection fixedPokemon;
        private IRtdxRom rom;

        private const int IndexColumn = 0;
        private const int EnumNameColumn = 1;
        private const int SpeciesColumn = 2;
        private const int Move1Column = 3;
        private const int Move2Column = 4;
        private const int Move3Column = 5;
        private const int Move4Column = 6;
        private const int DungeonColumn = 7;
        private const int LevelColumn = 8;
        private const int HpColumn = 9;
        private const int AtkBoostColumn = 10;
        private const int SpAtkBoostColumn = 11;
        private const int DefBoostColumn = 12;
        private const int SpDefBoostColumn = 13;
        private const int SpeedBoostColumn = 14;
        private const int InvitationIndexColumn = 15;
        private const int Short04Column = 16;
        private const int Short06Column = 17;
        private const int Short12Column = 18;
        private const int Byte14Column = 19;
        private const int Byte15Column = 20;
        private const int Byte16Column = 21;
        private const int Byte17Column = 22;
        private const int Byte18Column = 23;
        private const int Byte19Column = 24;
        private const int Byte20Column = 25;
        private const int Byte21Column = 26;
        private const int Byte22Column = 27;
        private const int Byte23Column = 28;
        private const int Byte24Column = 29;
        private const int Byte25Column = 30;
        private const int Byte26Column = 31;
        private const int Byte28Column = 32;

        public FixedPokemonController(IRtdxRom rom) : this(new Builder("FixedPokemon.glade"), rom)
        {
        }

        private FixedPokemonController(Builder builder, IRtdxRom rom) : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            this.rom = rom;
            this.fixedPokemon = rom.GetFixedPokemonCollection();

            movesStore!.AppendAll(AutocompleteHelpers.GetMoves(rom));
            dungeonsStore!.AppendAll(AutocompleteHelpers.GetDungeons(rom));
            speciesStore!.AppendAll(AutocompleteHelpers.GetPokemon(rom));

            for (int i = 0; i < fixedPokemon.Entries.Count; i++)
            {
                AddToStore(fixedPokemon.Entries[i], i);
            }
        }

        private void AddToStore(FixedPokemonModel model, int index)
        {
            fixedPokemonStore!.AppendValues(
                index,
                ((FixedCreatureIndex) index).ToString(),
                AutocompleteHelpers.FormatPokemon(rom, model.PokemonId),
                AutocompleteHelpers.FormatMove(rom, model.Move1),
                AutocompleteHelpers.FormatMove(rom, model.Move2),
                AutocompleteHelpers.FormatMove(rom, model.Move3),
                AutocompleteHelpers.FormatMove(rom, model.Move4),
                AutocompleteHelpers.FormatDungeon(rom, model.DungeonIndex),
                (int) model.Level,
                (int) model.HitPoints,
                (int) model.AttackBoost,
                (int) model.SpAttackBoost,
                (int) model.DefenseBoost,
                (int) model.SpDefenseBoost,
                (int) model.SpeedBoost,
                (int) model.InvitationIndex,
                (int) model.Short04,
                (int) model.Short06,
                (int) model.Short12,
                (int) model.Byte14,
                (int) model.Byte15,
                (int) model.Byte16,
                (int) model.Byte17,
                (int) model.Byte18,
                (int) model.Byte19,
                (int) model.Byte20,
                (int) model.Byte21,
                (int) model.Byte22,
                (int) model.Byte23,
                (int) model.Byte24,
                (int) model.Byte25,
                (int) model.Byte26,
                (int) model.Byte28,
                (int) model.Byte29,
                (int) model.Byte2A,
                (int) model.Byte2B,
                (int) model.Byte2C,
                (int) model.Byte2D,
                (int) model.Byte2E,
                (int) model.Byte2F
            );
        }

        private void OnSpeciesEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var pokemonIndex = AutocompleteHelpers.ExtractPokemon(args.NewText);
                if (pokemonIndex.HasValue)
                {
                    fixedPokemonStore.SetValue(iter, SpeciesColumn,
                        AutocompleteHelpers.FormatPokemon(rom!, pokemonIndex.Value));
                    fixedPokemon.Entries[path.Indices[0]].PokemonId = pokemonIndex.Value;
                }
            }
        }

        private void OnSpeciesEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = speciesCompletion;
        }

        private void OnMove1Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    fixedPokemonStore.SetValue(iter, Move1Column,
                        AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    fixedPokemon.Entries[path.Indices[0]].Move1 = moveIndex.Value;
                }
            }
        }

        private void OnMove2Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    fixedPokemonStore.SetValue(iter, Move2Column,
                        AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    fixedPokemon.Entries[path.Indices[0]].Move2 = moveIndex.Value;
                }
            }
        }

        private void OnMove3Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    fixedPokemonStore.SetValue(iter, Move3Column,
                        AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    fixedPokemon.Entries[path.Indices[0]].Move3 = moveIndex.Value;
                }
            }
        }

        private void OnMove4Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    fixedPokemonStore.SetValue(iter, Move4Column,
                        AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    fixedPokemon.Entries[path.Indices[0]].Move4 = moveIndex.Value;
                }
            }
        }

        private void OnMoveEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = movesCompletion;
        }

        private void OnLevelEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Level = value;
                }
                fixedPokemonStore.SetValue(iter, LevelColumn, (int) entry.Level);
            }
        }

        private void OnHpEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    entry.HitPoints = value;
                }
                fixedPokemonStore.SetValue(iter, HpColumn, (int) entry.HitPoints);
            }
        }

        private void OnAtkBoostEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.AttackBoost = value;
                }
                fixedPokemonStore.SetValue(iter, AtkBoostColumn, (int) entry.AttackBoost);
            }
        }

        private void OnSpAtkBoostEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.SpAttackBoost = value;
                }
                fixedPokemonStore.SetValue(iter, SpAtkBoostColumn, (int) entry.SpAttackBoost);
            }
        }

        private void OnDefBoostEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.DefenseBoost = value;
                }
                fixedPokemonStore.SetValue(iter, DefBoostColumn, (int) entry.DefenseBoost);
            }
        }

        private void OnSpDefBoostEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.SpDefenseBoost = value;
                }
                fixedPokemonStore.SetValue(iter, SpDefBoostColumn, (int) entry.SpDefenseBoost);
            }
        }

        private void OnSpeedBoostEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.SpeedBoost = value;
                }
                fixedPokemonStore.SetValue(iter, SpeedBoostColumn, (int) entry.SpeedBoost);
            }
        }

        private void OnInvitationIndexEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.InvitationIndex = value;
                }
                fixedPokemonStore.SetValue(iter, InvitationIndexColumn, (int) entry.InvitationIndex);
            }
        }

        private void OnDungeonIndexEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var dungeonIndex = AutocompleteHelpers.ExtractDungeon(args.NewText);
                if (dungeonIndex.HasValue)
                {
                    fixedPokemonStore.SetValue(iter, DungeonColumn,
                        AutocompleteHelpers.FormatDungeon(rom!, dungeonIndex.Value));
                    fixedPokemon.Entries[path.Indices[0]].DungeonIndex = dungeonIndex.Value;
                }
            }
        }

        private void OnDungeonIndexEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = dungeonsCompletion;
        }

        private void OnShort04Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    entry.Short04 = value;
                }
                fixedPokemonStore.SetValue(iter, Short04Column, (int) entry.Short04);
            }
        }

        private void OnShort06Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    entry.Short06 = value;
                }
                fixedPokemonStore.SetValue(iter, Short06Column, (int) entry.Short06);
            }
        }

        private void OnShort12Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    entry.Short12 = value;
                }
                fixedPokemonStore.SetValue(iter, Short12Column, (int) entry.Short12);
            }
        }

        private void OnByte14Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte14 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte14Column, (int) entry.Byte14);
            }
        }

        private void OnByte15Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte15 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte15Column, (int) entry.Byte15);
            }
        }

        private void OnByte16Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte16 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte16Column, (int) entry.Byte16);
            }
        }

        private void OnByte17Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte17 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte17Column, (int) entry.Byte17);
            }
        }

        private void OnByte18Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte18 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte18Column, (int) entry.Byte18);
            }
        }

        private void OnByte19Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte19 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte19Column, (int) entry.Byte19);
            }
        }

        private void OnByte20Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte20 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte20Column, (int) entry.Byte20);
            }
        }

        private void OnByte21Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte21 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte21Column, (int) entry.Byte21);
            }
        }

        private void OnByte22Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte22 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte22Column, (int) entry.Byte22);
            }
        }

        private void OnByte23Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte23 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte23Column, (int) entry.Byte23);
            }
        }

        private void OnByte24Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte24 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte24Column, (int) entry.Byte24);
            }
        }

        private void OnByte25Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte25 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte25Column, (int) entry.Byte25);
            }
        }

        private void OnByte26Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedPokemonStore!.GetIter(out var iter, path))
            {
                var entry = fixedPokemon.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte26 = value;
                }
                fixedPokemonStore.SetValue(iter, Byte26Column, (int) entry.Byte26);
            }
        }

        private void OnAddClicked(object sender, EventArgs args)
        {
            var entry = new FixedPokemonModel();
            fixedPokemon.Entries.Add(entry);
            AddToStore(entry, fixedPokemon.Entries.Count - 1);
        }

        private void OnRemoveClicked(object sender, EventArgs args)
        {
            if (fixedPokemonTree!.Selection.GetSelected(out var model, out var iter))
            {
                var path = model.GetPath(iter);
                int index = path.Indices[0];
                fixedPokemon.Entries.RemoveAt(index);
                var store = (ListStore) model;
                store.Remove(ref iter);
                store.FixIndices(IndexColumn);
            }
        }
    }
}
