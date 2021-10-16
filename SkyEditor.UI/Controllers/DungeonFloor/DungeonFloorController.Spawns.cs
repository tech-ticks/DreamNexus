using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditorUI.Infrastructure;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System.Linq;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

namespace SkyEditorUI.Controllers
{
    partial class DungeonFloorController : Widget
    {
        private const int SpawnCreatureIdColumn = 0;
        private const int SpawnRecruitmentLevelColumn = 2;
        private const int SpawnWeightColumn = 3;
        private const int IsSpecialColumn = 6;

        [UI] private TreeView? creatureSpawnsTree;
        [UI] private ListStore? creaturesStore;
        [UI] private ListStore? creatureSpawnsStore;
        [UI] private EntryCompletion? creaturesCompletion;

        private HashSet<CreatureIndex> allowedCreatures = new HashSet<CreatureIndex>();

        private void LoadSpawnsTab()
        {
            allowedCreatures = (dungeon.PokemonStats ?? Enumerable.Empty<DungeonPokemonStatsModel>())
                .Select(stat => stat.CreatureIndex)
                .ToHashSet();
            creaturesStore!.AppendAll(allowedCreatures
                .Select(creature => AutocompleteHelpers.FormatPokemon(rom, creature)));

            RefreshSpawns();
        }

        private void RefreshSpawns()
        {
            if (floor.Spawns == null)
            {
                return;
            }

            creatureSpawnsStore!.Clear();
            // HACK: exclude CreatureIndex.WANTED_LV from the weights since it doesn't seem like a "real" entry
            int weightSum = floor.Spawns.Where(spawn => !spawn.IsSpecial && spawn.StatsIndex != CreatureIndex.WANTED_LV)
                .Sum(spawn => spawn.SpawnRate);
            foreach (var spawn in floor.Spawns)
            {
                float percentage = ((float) spawn.SpawnRate / weightSum) * 100f;
                creatureSpawnsStore!.AppendValues(
                    (int) spawn.StatsIndex,
                    AutocompleteHelpers.FormatPokemon(rom, spawn.StatsIndex),
                    (int) spawn.RecruitmentLevel,
                    (int) spawn.SpawnRate,
                    !spawn.IsSpecial && spawn.StatsIndex != CreatureIndex.WANTED_LV ? $"{percentage:F2}%" : "-",
                    (int) spawn.Byte0B,
                    spawn.IsSpecial
                );
            }
        }

        private void OnSpawnSpeciesEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (creatureSpawnsStore!.GetIter(out var iter, path))
            {
                var creatureIndex = AutocompleteHelpers.ExtractPokemon(args.NewText);
                if (creatureIndex.HasValue && allowedCreatures.Contains(creatureIndex.Value))
                {
                    var oldIndex = (CreatureIndex) creatureSpawnsStore.GetValue(iter, SpawnCreatureIdColumn);
                    if (creatureIndex.Value != oldIndex
                        && floor.Spawns!.Any(spawn => spawn.StatsIndex == creatureIndex.Value))
                    {
                        UIUtils.ShowInfoDialog(MainWindow.Instance, "Cannot add Pokémon spawn",
                            "The Pokémon is already in the list.");
                        return;
                    }
                    creatureSpawnsStore!.SetValue(iter, SpawnCreatureIdColumn,
                        AutocompleteHelpers.FormatPokemon(rom!, creatureIndex.Value));
                    floor.Spawns![path.Indices[0]].StatsIndex = creatureIndex.Value;
                    RefreshSpawns();
                }
            }
        }

        private void OnSpawnSpeciesEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = creaturesCompletion;
        }

        private void OnSpawnRecruitmentLevelEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (creatureSpawnsStore!.GetIter(out var iter, path))
            {
                var index = (CreatureIndex) creatureSpawnsStore.GetValue(iter, SpawnCreatureIdColumn);
                if (byte.TryParse(args.NewText, out byte value))
                {
                    var spawn = floor.Spawns!.Find(spawn => spawn.StatsIndex == index);
                    if (spawn != null)
                    {
                        spawn.RecruitmentLevel = value;
                        RefreshSpawns();
                    }
                }
            }
        }

        private void OnIsSpecialToggled(object sender, ToggledArgs args)
        {
            var path = new TreePath(args.Path);
            if (creatureSpawnsStore!.GetIter(out var iter, path))
            {
                var index = (CreatureIndex) creatureSpawnsStore.GetValue(iter, SpawnCreatureIdColumn);
                var spawn = floor.Spawns!.Find(spawn => spawn.StatsIndex == index);
                if (spawn != null)
                {
                    spawn.IsSpecial = !spawn.IsSpecial;
                    creatureSpawnsStore.SetValue(iter, IsSpecialColumn, spawn.IsSpecial);
                    RefreshSpawns();
                }
            }
        }

        private void OnSpawnWeightEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (creatureSpawnsStore!.GetIter(out var iter, path))
            {
                var index = (CreatureIndex) creatureSpawnsStore.GetValue(iter, SpawnCreatureIdColumn);
                if (byte.TryParse(args.NewText, out byte value) && value <= 128)
                {
                    var spawn = floor.Spawns!.Find(spawn => spawn.StatsIndex == index);
                    if (spawn != null)
                    {
                        spawn.SpawnRate = value;
                    }
                    RefreshSpawns();
                }
            }
        }

        private void OnByte0BEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (creatureSpawnsStore!.GetIter(out var iter, path))
            {
                var index = (CreatureIndex) creatureSpawnsStore.GetValue(iter, SpawnCreatureIdColumn);
                if (byte.TryParse(args.NewText, out byte value))
                {
                    var spawn = floor.Spawns!.Find(spawn => spawn.StatsIndex == index);
                    if (spawn != null)
                    {
                        spawn.Byte0B = value;
                    }
                    RefreshSpawns();
                }
            }
        }

        private void OnAddSpawnClicked(object sender, EventArgs args)
        {
            if (floor.Spawns == null)
            {
                return;
            }

            var nextSpecies = allowedCreatures.FirstOrDefault(species => 
                !floor.Spawns.Any(spawn => spawn.StatsIndex == species));

            if (nextSpecies == default)
            {
                UIUtils.ShowInfoDialog(MainWindow.Instance, "Cannot add Pokémon spawn",
                    "All Pokémon with stats in the dungeon were already added. To add a new Pokémon, open the "
                    + "\"Wild Pokémon Stats\" page of the current dungeon.");
                return;
            }

            floor.Spawns.Add(new DungeonPokemonSpawnModel
            {
                StatsIndex = nextSpecies,
                RecruitmentLevel = 5,
                SpawnRate = 10,
                Byte0B = 0
            });
            RefreshSpawns();
        }

        private void OnRemoveSpawnClicked(object sender, EventArgs args)
        {
            if (floor.Spawns == null)
            {
                return;
            }

            if (creatureSpawnsTree!.Selection.GetSelected(out var model, out var iter))
            {
                floor.Spawns.RemoveAt(model.GetPath(iter).Indices[0]);
                creatureSpawnsStore!.Remove(ref iter);
            }
        }
    }
}
