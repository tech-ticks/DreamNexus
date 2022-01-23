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
        private const int StatGrowthLevelColumn = 0;
        private const int StatGrowthExpColumn = 1;
        private const int StatsGrowthHitPointsColumn = 2;
        private const int StatsGrowthAttackColumn = 3;
        private const int StatsGrowthSpecialAttackColumn = 4;
        private const int StatsGrowthDefenseColumn = 5;
        private const int StatsGrowthSpecialDefenseColumn = 6;
        private const int StatsGrowthSpeedColumn = 7;

        [UI] private SpinButton? sbStatGrowthGroup;
        [UI] private ListStore? statGrowthStore;

        private IStatGrowthCollection? statGrowth;
        private StatGrowthModel? statGrowthEntry;

        private void LoadStatsTab()
        {
            statGrowth = rom.GetStatGrowth();
            sbStatGrowthGroup!.Value = pokemon.ExperienceEntry;
            sbStatGrowthGroup.Adjustment.Upper = statGrowth!.Count - 1;

            LoadStatGrowth();
        }

        private void LoadStatGrowth()
        {
            statGrowthEntry = statGrowth!.GetEntryById(pokemon.ExperienceEntry);
            if (statGrowthEntry == null)
            {
                return;
            }

            statGrowthStore!.Clear();
            for (int i = 0; i < statGrowthEntry.Levels.Count; i++)
            {
                var level = statGrowthEntry.Levels[i];
                statGrowthStore!.AppendValues(i+1, level.MinimumExperience, level.HitPointsGained,
                    level.AttackGained, level.SpecialAttackGained, level.DefenseGained, level.SpecialDefenseGained,
                    level.SpeedGained);
            }
        }

        private void OnStatGrowthGroupChanged(object sender, EventArgs args)
        {
            pokemon.ExperienceEntry = (byte) sbStatGrowthGroup!.ValueAsInt;
            LoadStatGrowth();
        }

        private void OnStatGrowthExpEdited(object sender, EditedArgs args)
        {
            if (statGrowthEntry == null)
            {
                return;
            }

            var path = new TreePath(args.Path);
            if (statGrowthStore!.GetIter(out var iter, path))
            {
                var level = statGrowthEntry.Levels![path.Indices[0]];
                if (int.TryParse(args.NewText, out int value))
                {
                    level.MinimumExperience = value;
                }
                statGrowthStore.SetValue(iter, StatGrowthExpColumn, level.MinimumExperience);
            }
        }

        private void OnStatGrowthHpEdited(object sender, EditedArgs args)
        {
            if (statGrowthEntry == null)
            {
                return;
            }

            var path = new TreePath(args.Path);
            if (statGrowthStore!.GetIter(out var iter, path))
            {
                var level = statGrowthEntry.Levels![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    level.HitPointsGained = value;
                }
                statGrowthStore.SetValue(iter, StatsGrowthHitPointsColumn, level.HitPointsGained);
            }
        }

        private void OnStatGrowthAtkEdited(object sender, EditedArgs args)
        {
            if (statGrowthEntry == null)
            {
                return;
            }

            var path = new TreePath(args.Path);
            if (statGrowthStore!.GetIter(out var iter, path))
            {
                var level = statGrowthEntry.Levels![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    level.AttackGained = value;
                }
                statGrowthStore.SetValue(iter, StatsGrowthAttackColumn, level.AttackGained);
            }
        }

        private void OnStatGrowthSpAtkEdited(object sender, EditedArgs args)
        {
            if (statGrowthEntry == null)
            {
                return;
            }

            var path = new TreePath(args.Path);
            if (statGrowthStore!.GetIter(out var iter, path))
            {
                var level = statGrowthEntry.Levels![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    level.SpecialAttackGained = value;
                }
                statGrowthStore.SetValue(iter, StatsGrowthSpecialAttackColumn, level.SpecialAttackGained);
            }
        }

        private void OnStatGrowthDefEdited(object sender, EditedArgs args)
        {
            if (statGrowthEntry == null)
            {
                return;
            }

            var path = new TreePath(args.Path);
            if (statGrowthStore!.GetIter(out var iter, path))
            {
                var level = statGrowthEntry.Levels![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    level.DefenseGained = value;
                }
                statGrowthStore.SetValue(iter, StatsGrowthDefenseColumn, level.DefenseGained);
            }
        }

        private void OnStatGrowthSpDefEdited(object sender, EditedArgs args)
        {
            if (statGrowthEntry == null)
            {
                return;
            }

            var path = new TreePath(args.Path);
            if (statGrowthStore!.GetIter(out var iter, path))
            {
                var level = statGrowthEntry.Levels![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    level.SpecialDefenseGained = value;
                }
                statGrowthStore.SetValue(iter, StatsGrowthSpecialDefenseColumn, level.SpecialDefenseGained);
            }
        }

        private void OnStatGrowthSpeedEdited(object sender, EditedArgs args)
        {
            if (statGrowthEntry == null)
            {
                return;
            }

            var path = new TreePath(args.Path);
            if (statGrowthStore!.GetIter(out var iter, path))
            {
                var level = statGrowthEntry.Levels![path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    level.SpeedGained = value;
                }
                statGrowthStore.SetValue(iter, StatsGrowthSpeedColumn, level.SpeedGained);
            }
        }
    }
}
