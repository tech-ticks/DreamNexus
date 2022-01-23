using System.Collections.Generic;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IStatGrowthCollection
    {
        IDictionary<int, StatGrowthModel> LoadedEntries { get; }
        int Count { get; }
        void SetEntry(int id, StatGrowthModel model);
        bool IsEntryDirty(int id);
        StatGrowthModel? GetEntryById(int id, bool markAsDirty = true);
        void Flush(IRtdxRom rom);
    }

    public class StatGrowthCollection : IStatGrowthCollection
    {
        public IDictionary<int, StatGrowthModel> LoadedEntries { get; } = new Dictionary<int, StatGrowthModel>();
        public HashSet<int> DirtyEntries { get; } = new HashSet<int>();
        public int Count { get; private set; }

        private IRtdxRom rom;

        public StatGrowthCollection(IRtdxRom rom)
        {
            Count = rom.GetExperience().Entries.Count;
            this.rom = rom;
        }

        public StatGrowthModel LoadEntry(int id)
        {
            var data = rom.GetExperience().Entries[id];
            var levels = new List<StatGrowthLevel>();

            foreach (var level in data.Levels)
            {
                levels.Add(new StatGrowthLevel
                {
                    MinimumExperience = level.MinimumExperience,
                    HitPointsGained = level.HitPointsGained,
                    AttackGained = level.AttackGained,
                    SpecialAttackGained = level.SpecialAttackGained,
                    DefenseGained = level.DefenseGained,
                    SpecialDefenseGained = level.SpecialDefenseGained,
                    SpeedGained = level.SpeedGained,
                    LevelsGained = level.LevelsGained,
                });
            }

            return new StatGrowthModel
            {
                Levels = levels
            };
        }

        public StatGrowthModel GetEntryById(int id, bool markAsDirty = true)
        {
            if (!LoadedEntries.ContainsKey(id))
            {
                LoadedEntries.Add(id, LoadEntry(id));
            }
            if (markAsDirty)
            {
                DirtyEntries.Add(id);
            }
            return LoadedEntries[id];
        }

        public void SetEntry(int id, StatGrowthModel model)
        {
            LoadedEntries[id] = model;
        }

        public bool IsEntryDirty(int id)
        {
            return DirtyEntries.Contains(id);
        }

        public void Flush(IRtdxRom rom)
        {
            var romEntries = rom.GetExperience().Entries;
            foreach (var kv in LoadedEntries)
            {
                var id = kv.Key;
                var entry = kv.Value;

                var romEntryLevels = romEntries[id].Levels;
                romEntryLevels.Clear();

                foreach (var level in entry.Levels)
                {
                    romEntryLevels.Add(new Experience.ExperienceEntry.Level
                    {
                        MinimumExperience = level.MinimumExperience,
                        HitPointsGained = level.HitPointsGained,
                        AttackGained = level.AttackGained,
                        SpecialAttackGained = level.SpecialAttackGained,
                        DefenseGained = level.DefenseGained,
                        SpecialDefenseGained = level.SpecialDefenseGained,
                        SpeedGained = level.SpeedGained,
                        LevelsGained = level.LevelsGained,
                    });
                }
            }
        }
    }    
}
