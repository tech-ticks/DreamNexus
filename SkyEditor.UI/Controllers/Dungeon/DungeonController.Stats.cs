using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditorUI.Infrastructure;
using System;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

namespace SkyEditorUI.Controllers
{
    partial class DungeonController : Widget
    {
        private const int StatsSpeciesColumn = 0;
        private const int StatsXpYieldColumn = 1;
        private const int StatsHitPointsColumn = 2;
        private const int StatsAttackColumn = 3;
        private const int StatsSpecialAttackColumn = 4;
        private const int StatsDefenseColumn = 5;
        private const int StatsSpecialDefenseColumn = 6;
        private const int StatsSpeedColumn = 7;
        private const int StatsStrongFoeColumn = 8;
        private const int StatsLevelColumn = 9;

        [UI] private TreeView? wildPokemonStatsTree;
        [UI] private ListStore? wildPokemonStatsStore;
        [UI] private ListStore? creaturesStore;
        [UI] private EntryCompletion? creaturesCompletion;

        private void LoadStatsTab()
        {
            var stats = dungeon.PokemonStats;
            wildPokemonStatsStore!.Clear();
            if (stats == null)
            {
                wildPokemonStatsTree!.Sensitive = false;
                return;
            }

            foreach (var pokemonStats in stats)
            {
                AddStatsToTree(pokemonStats);
            }

            creaturesStore!.AppendAll(AutocompleteHelpers.GetPokemon(rom));
        }

        private void OnStatsSpeciesEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = creaturesCompletion;
        }

        private void OnStatsSpeciesEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (wildPokemonStatsStore!.GetIter(out var iter, path))
            {
                var creatureIndex = AutocompleteHelpers.ExtractPokemon(args.NewText);
                if (creatureIndex.HasValue)
                {
                    wildPokemonStatsStore.SetValue(iter, StatsSpeciesColumn,
                    AutocompleteHelpers.FormatPokemon(rom!, creatureIndex.Value));
                    dungeon.PokemonStats![path.Indices[0]].CreatureIndex = creatureIndex.Value;
                }
            }
        }

        private void OnStatsLevelEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (wildPokemonStatsStore!.GetIter(out var iter, path))
            {
                var stat = dungeon.PokemonStats![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    stat.Level = value;
                }
                wildPokemonStatsStore.SetValue(iter, StatsLevelColumn, stat.Level);
            }
        }

        private void OnStatsStrongFoeToggled(object sender, ToggledArgs args)
        {
            var path = new TreePath(args.Path);
            if (wildPokemonStatsStore!.GetIter(out var iter, path))
            {
                var stat = dungeon.PokemonStats![path.Indices[0]];
                stat.StrongFoe = !stat.StrongFoe;
                wildPokemonStatsStore.SetValue(iter, StatsStrongFoeColumn, stat.StrongFoe);
            }
        }

        private void OnStatsXpYieldEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (wildPokemonStatsStore!.GetIter(out var iter, path))
            {
                var stat = dungeon.PokemonStats![path.Indices[0]];
                if (int.TryParse(args.NewText, out int value))
                {
                    stat.XpYield = value;
                }
                wildPokemonStatsStore.SetValue(iter, StatsXpYieldColumn, stat.XpYield);
            }
        }

        private void OnStatsHpEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (wildPokemonStatsStore!.GetIter(out var iter, path))
            {
                var stat = dungeon.PokemonStats![path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    stat.HitPoints = value;
                }
                wildPokemonStatsStore.SetValue(iter, StatsHitPointsColumn, stat.HitPoints);
            }
        }

        private void OnStatsAttackEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (wildPokemonStatsStore!.GetIter(out var iter, path))
            {
                var stat = dungeon.PokemonStats![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    stat.Attack = value;
                }
                wildPokemonStatsStore.SetValue(iter, StatsAttackColumn, stat.Attack);
            }
        }

        private void OnStatsDefenseEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (wildPokemonStatsStore!.GetIter(out var iter, path))
            {
                var stat = dungeon.PokemonStats![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    stat.Defense = value;
                }
                wildPokemonStatsStore.SetValue(iter, StatsDefenseColumn, stat.Defense);
            }
        }

        private void OnStatsSpecialAttackEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (wildPokemonStatsStore!.GetIter(out var iter, path))
            {
                var stat = dungeon.PokemonStats![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    stat.SpecialAttack = value;
                }
                wildPokemonStatsStore.SetValue(iter, StatsSpecialAttackColumn, stat.SpecialAttack);
            }
        }

        private void OnStatsSpecialDefenseEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (wildPokemonStatsStore!.GetIter(out var iter, path))
            {
                var stat = dungeon.PokemonStats![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    stat.SpecialDefense = value;
                }
                wildPokemonStatsStore.SetValue(iter, StatsSpecialDefenseColumn, stat.SpecialDefense);
            }
        }

        private void OnStatsSpeedEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (wildPokemonStatsStore!.GetIter(out var iter, path))
            {
                var stat = dungeon.PokemonStats![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    stat.Speed = value;
                }
                wildPokemonStatsStore.SetValue(iter, StatsSpeedColumn, stat.Speed);
            }
        }

        private void OnAddStatsClicked(object sender, EventArgs args)
        {
            if (dungeon.PokemonStats == null)
            {
                return;
            }

            var newStats = new DungeonPokemonStatsModel();
            dungeon.PokemonStats.Add(newStats);
            AddStatsToTree(newStats);
        }

        private void OnRemoveStatsClicked(object sender, EventArgs args)
        {
            if (dungeon.PokemonStats == null)
            {
                return;
            }

           if (wildPokemonStatsTree!.Selection.GetSelected(out var model, out var iter))
           {
               dungeon.PokemonStats.RemoveAt(model.GetPath(iter).Indices[0]);
               wildPokemonStatsStore!.Remove(ref iter);
           }
        }

        private void AddStatsToTree(DungeonPokemonStatsModel stats)
        {
            wildPokemonStatsStore!.AppendValues(
                AutocompleteHelpers.FormatPokemon(rom, stats.CreatureIndex),
                stats.XpYield,
                stats.HitPoints,
                stats.Attack,
                stats.SpecialAttack,
                stats.Defense,
                stats.SpecialDefense,
                stats.Speed,
                stats.StrongFoe,
                stats.Level
            );
        }
    }
}
