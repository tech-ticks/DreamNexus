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
        [UI] private SpinButton? sbStatGrowthGroup;
        [UI] private ListStore? statGrowthStore;

        private IExperience? experience;

        private void LoadStatsTab()
        {
            experience = rom.GetExperience();
            sbStatGrowthGroup!.Value = pokemon.ExperienceEntry;
            sbStatGrowthGroup.Adjustment.Upper = experience!.Entries.Count - 1;

            LoadStatGrowth();
        }

        private void LoadStatGrowth()
        {
            var experienceEntry = experience!.Entries[pokemon.ExperienceEntry];
            statGrowthStore!.Clear();
            for (int i = 0; i < experienceEntry.Levels.Count; i++)
            {
                var level = experienceEntry.Levels[i];
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
    }
}
