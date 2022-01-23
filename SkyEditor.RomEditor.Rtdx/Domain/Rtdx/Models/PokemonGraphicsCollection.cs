using System.Collections.Generic;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IPokemonGraphicsCollection
    {
        IDictionary<int, PokemonGraphicsModel> LoadedEntries { get; }
        int Count { get; }
        void SetEntry(int id, PokemonGraphicsModel model);
        bool IsEntryDirty(int id);
        PokemonGraphicsModel? GetEntryById(int id, bool markAsDirty = true);
        void Flush(IRtdxRom rom);
    }

    public class PokemonGraphicsCollection : IPokemonGraphicsCollection
    {
        public IDictionary<int, PokemonGraphicsModel> LoadedEntries { get; } = new Dictionary<int, PokemonGraphicsModel>();
        public HashSet<int> DirtyEntries { get; } = new HashSet<int>();
        public int Count { get; private set; }

        private IRtdxRom rom;

        public PokemonGraphicsCollection(IRtdxRom rom)
        {
            this.rom = rom;
            Count = rom.GetPokemonGraphicsDatabase().Entries.Count;
        }

        public PokemonGraphicsModel LoadEntry(int id)
        {
            var data = rom.GetPokemonGraphicsDatabase().Entries[id - 1]; // 1-indexed in Pokémon data
            return new PokemonGraphicsModel
            {
                ModelName = data.ModelName,
                AnimationName = data.AnimationName,
                BaseFormModelName = data.BaseFormModelName,
                PortraitSheetName = data.PortraitSheetName,
                RescueCampSheetName = data.RescueCampSheetName,
                RescueCampSheetReverseName = data.RescueCampSheetReverseName,
                BaseScale = data.BaseScale,
                DungeonBaseScale = data.DungeonBaseScale,
                UnkX38 = data.UnkX38,
                UnkX3C = data.UnkX3C,
                UnkX40 = data.UnkX40,
                UnkX44 = data.UnkX44,
                YOffset = data.YOffset,
                WalkSpeedDistance = data.WalkSpeedDistance,
                UnkX50 = data.UnkX50,
                RunSpeedRatioGround = data.RunSpeedRatioGround,
                UnkX58 = data.UnkX58,
                UnkX5C = data.UnkX5C,
                UnkX60 = data.UnkX60,
                UnkX64 = data.UnkX64,
                UnknownBodyType1 = data.UnknownBodyType1,
                UnknownBodyType2 = data.UnknownBodyType2,
                Flags = data.Flags,
                EnabledPortraits = data.EnabledPortraits,
                UnkX78 = data.UnkX78,
                UnkX7C = data.UnkX7C,
                UnkX80 = data.UnkX80,
                UnkX84 = data.UnkX84,
                UnkX88 = data.UnkX88,
                UnkX8C = data.UnkX8C,
                UnkX90 = data.UnkX90,
                UnkX94 = data.UnkX94,
                UnkX98 = data.UnkX98,
                UnkX9C = data.UnkX9C,
                UnkXA0 = data.UnkXA0,
            };
        }

        public PokemonGraphicsModel GetEntryById(int id, bool markAsDirty = true)
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

        public void SetEntry(int id, PokemonGraphicsModel model)
        {
            LoadedEntries[id] = model;
        }

        public bool IsEntryDirty(int id)
        {
            return DirtyEntries.Contains(id);
        }

        public void Flush(IRtdxRom rom)
        {
            var romEntries = rom.GetPokemonGraphicsDatabase().Entries;
            foreach (var kv in LoadedEntries)
            {
                var id = kv.Key;
                var entry = kv.Value;
                romEntries[id + 1] = new PokemonGraphicsDatabase.PokemonGraphicsDatabaseEntry
                {
                    ModelName = entry.ModelName,
                    AnimationName = entry.AnimationName,
                    BaseFormModelName = entry.BaseFormModelName,
                    PortraitSheetName = entry.PortraitSheetName,
                    RescueCampSheetName = entry.RescueCampSheetName,
                    RescueCampSheetReverseName = entry.RescueCampSheetReverseName,
                    BaseScale = entry.BaseScale,
                    DungeonBaseScale = entry.DungeonBaseScale,
                    UnkX38 = entry.UnkX38,
                    UnkX3C = entry.UnkX3C,
                    UnkX40 = entry.UnkX40,
                    UnkX44 = entry.UnkX44,
                    YOffset = entry.YOffset,
                    WalkSpeedDistance = entry.WalkSpeedDistance,
                    UnkX50 = entry.UnkX50,
                    RunSpeedRatioGround = entry.RunSpeedRatioGround,
                    UnkX58 = entry.UnkX58,
                    UnkX5C = entry.UnkX5C,
                    UnkX60 = entry.UnkX60,
                    UnkX64 = entry.UnkX64,
                    UnknownBodyType1 = entry.UnknownBodyType1,
                    UnknownBodyType2 = entry.UnknownBodyType2,
                    Flags = entry.Flags,
                    EnabledPortraits = entry.EnabledPortraits,
                    UnkX78 = entry.UnkX78,
                    UnkX7C = entry.UnkX7C,
                    UnkX80 = entry.UnkX80,
                    UnkX84 = entry.UnkX84,
                    UnkX88 = entry.UnkX88,
                    UnkX8C = entry.UnkX8C,
                    UnkX90 = entry.UnkX90,
                    UnkX94 = entry.UnkX94,
                    UnkX98 = entry.UnkX98,
                    UnkX9C = entry.UnkX9C,
                    UnkXA0 = entry.UnkXA0,
                };
            }
        }
    }    
}
