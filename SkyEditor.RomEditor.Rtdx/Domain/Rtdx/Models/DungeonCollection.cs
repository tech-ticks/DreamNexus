using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IDungeonCollection
    {
        IDictionary<DungeonIndex, DungeonModel> LoadedDungeons { get; }
        List<DungeonModel> LoadAllDungeons(bool markAsDirty = true);
        void SetDungeon(DungeonIndex id, DungeonModel model);
        bool IsDungeonDirty(DungeonIndex id);
        DungeonModel? GetDungeonById(DungeonIndex id, bool markAsDirty = true);
        void Flush(IRtdxRom rom);
    }

    public class DungeonCollection : IDungeonCollection
    {
        public IDictionary<DungeonIndex, DungeonModel> LoadedDungeons { get; } = new Dictionary<DungeonIndex, DungeonModel>();
        public HashSet<DungeonIndex> DirtyDungeons { get; } = new HashSet<DungeonIndex>();

        private readonly IRtdxRom rom;

        public DungeonCollection(IRtdxRom rom)
        {
            this.rom = rom ?? throw new ArgumentNullException(nameof(rom));
        }

        public DungeonModel GetDungeonById(DungeonIndex id, bool markAsDirty = true)
        {
            if (markAsDirty)
            {
                DirtyDungeons.Add(id);
            }
            if (!LoadedDungeons.ContainsKey(id))
            {
                LoadedDungeons.Add(id, LoadDungeon(id));
            }
            return LoadedDungeons[id];
        }

        public void SetDungeon(DungeonIndex id, DungeonModel model)
        {
            LoadedDungeons[id] = model;
        }

        public bool IsDungeonDirty(DungeonIndex id)
        {
            return DirtyDungeons.Contains(id);
        }

        public List<DungeonModel> LoadAllDungeons(bool markAsDirty = true)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < (int) DungeonIndex.END; i++)
            {
                GetDungeonById((DungeonIndex) i, markAsDirty);
            }
            return LoadedDungeons.Values.OrderBy(dungeon => dungeon.SortKey).ToList();
        }

        private DungeonModel LoadDungeon(DungeonIndex index)
        {
            var commonStrings = rom.GetCommonStrings();
            var dungeonData = rom.GetDungeonDataInfo();
            var dungeonExtra = rom.GetDungeonExtra();
            var dungeonBalance = rom.GetDungeonBalance();
            var itemArrange = rom.GetItemArrange();
            var requestLevels = rom.GetRequestLevel();

            var data = dungeonData.Entries[index];
            var extra = dungeonExtra.Entries.GetValueOrDefault(index);
            var balance = dungeonBalance.Entries[data.DungeonBalanceIndex];
            var requestLevel = requestLevels.Entries.GetValueOrDefault(index);
            var itemArrangeEntry = index > DungeonIndex.NONE
                ? itemArrange.Entries[((int) index) - 1] : null;
            var accessibleFloorCount = requestLevel?.MainEntry?.AccessibleFloorCount
                ?? extra?.Floors ?? -1;

            return new DungeonModel(data)
            {
                Id = index,
                DungeonName = commonStrings.Dungeons.ContainsKey(index)
                    ? commonStrings.Dungeons[index] : $"(Unknown: {index})",

                Features = data.Features,
                DataInfoShort0A = data.Short0A,
                SortKey = data.SortKey,
                DataInfoByte13 = data.Byte13,
                MaxItems = data.MaxItems,
                MaxTeammates = data.MaxTeammates,
                DataInfoByte17 = data.Byte17,
                DataInfoByte18 = data.Byte18,
                DataInfoByte19 = data.Byte19,
                AccessibleFloorCount = (short) accessibleFloorCount,
                UnknownFloorCount = requestLevel?.MainEntry.Unk1 ?? -1,
                TotalFloorCount = requestLevel?.MainEntry.TotalFloorCount ?? -1,

                ItemSets = LoadItemSets(itemArrangeEntry),
                PokemonStats = balance.WildPokemon != null ? LoadPokemonStats(balance.WildPokemon) : null,
                Floors = LoadFloors(balance, requestLevel),

                Extra = extra,
                Balance = balance,
                ItemArrange = itemArrangeEntry,
                RequestLevel = requestLevel,
            };
        }

        private List<DungeonPokemonStatsModel> LoadPokemonStats(DungeonBalance.WildPokemonInfo data)
        {
            // Only include stats that are actually used (every dungeon has empty stats for all unused Pokémon)
            var lookup = data.Floors
                .SelectMany(floor => floor.Entries)
                .Where(entry => entry.SpawnRate > 0)
                .ToLookup(entry => entry.PokemonIndex);

            return data.Stats
                .Where(stat => lookup.Contains((short) stat.Index) || stat.StrongFoe != 0)
                .Select(stat => new DungeonPokemonStatsModel
                {
                    CreatureIndex = (CreatureIndex) stat.Index,
                    XpYield = stat.XPYield,
                    HitPoints = stat.HitPoints,
                    Attack = stat.Attack,
                    SpecialAttack = stat.SpecialAttack,
                    SpecialDefense = stat.SpecialDefense,
                    Speed = stat.Speed,
                    StrongFoe = stat.StrongFoe != 0,
                    Level = stat.Level
                }).ToList();
        }

        private List<ItemSetModel> LoadItemSets(ItemArrange.Entry? itemArrangeEntry)
        {
            if (itemArrangeEntry == null)
            {
                return new List<ItemSetModel>();
            }

            var itemSets = new List<ItemSetModel>();
            itemSets.Capacity = itemArrangeEntry.ItemSets.Count;
            foreach (var entry in itemArrangeEntry.ItemSets)
            {
                var kindWeights = new Dictionary<ItemKind, ushort>();
                foreach (var tuple in entry.ItemKindWeights.Select((weight, index) => (weight, index)))
                {
                    if (!kindWeights.ContainsKey((ItemKind) tuple.index))
                    {
                        kindWeights.Add((ItemKind) tuple.index, tuple.weight);
                    }
                }
                
                itemSets.Add(new ItemSetModel
                {
                    ItemKindWeights = kindWeights,
                    ItemWeights = entry.ItemWeights
                });
            }
            return itemSets;
        }

        private List<DungeonFloorModel> LoadFloors(DungeonBalance.Entry data, RequestLevel.Entry? requestLevelEntry)
        {
            var dungeonFloors = new List<DungeonFloorModel>();
            dungeonFloors.Capacity = data.FloorInfos.Length;
            for (int i = 0; i < data.FloorInfos.Length; i++)
            {
                var entry = data.FloorInfos[i];
                var requestLevelData = requestLevelEntry?.MainEntry?.FloorData.ElementAtOrDefault(i);
                var trapWeightsEntry = data.TrapWeights?.Records.ElementAtOrDefault(i - 1);
                var spawnsEntry = data.WildPokemon?.Floors.ElementAtOrDefault(i);

                dungeonFloors.Add(new DungeonFloorModel
                {
                    Index = entry.Index,
                    BalanceFloorInfoShort02 = entry.Short02,
                    Event = entry.Event,
                    BalanceFloorInfoShort24 = entry.Short24,
                    BalanceFloorInfoShort26 = entry.Short26,
                    BalanceFloorInfoShort28 = entry.Short28,
                    DungeonMapDataInfoIndex = entry.DungeonMapDataInfoIndex,
                    NameId = entry.NameID,
                    BalanceFloorInfoByte2D = entry.Byte2D,
                    BalanceFloorInfoByte2E = entry.Byte2E,
                    BalanceFloorInfoByte2F = entry.Byte2F,
                    BalanceFloorInfoShort30 = entry.Short30,
                    BalanceFloorInfoShort32 = entry.Short32,
                    BalanceFloorInfoByte34 = entry.Byte34,
                    BalanceFloorInfoByte35 = entry.Byte35,
                    UnknownItemSetIndex = entry.ItemSetIndex,
                    InvitationIndex = entry.InvitationIndex,
                    BalanceFloorInfoByte55 = entry.Byte55,
                    BalanceFloorInfoByte56 = entry.Byte56,
                    BalanceFloorInfoByte57 = entry.Byte57,
                    BalanceFloorInfoByte58 = entry.Byte58,
                    Weather = entry.Weather,
                    BalanceFloorInfoBytes37to53 = entry.Bytes37to53,
                    BalanceFloorInfoBytes5Ato61 = entry.Bytes5Ato61,
                    IsBossFloor = requestLevelData?.IsBossFloor != null ? requestLevelData.IsBossFloor != 0 : false,

                    TrapWeights = trapWeightsEntry != null ? LoadTrapWeights(trapWeightsEntry) : null,
                    Spawns = spawnsEntry != null ? LoadSpawns(spawnsEntry) : null,
                });
            }
            return dungeonFloors;
        }

        private Dictionary<ItemIndex, short> LoadTrapWeights(DungeonBalance.TrapWeights.Record weights)
        {
            var dict = new Dictionary<ItemIndex, short>();
            foreach (var weight in weights.Entries.SkipLast(1)) // End terminator entry, always -1
            {
                // Can't use LINQ ToDictionary due to duplicate keys
                var index = ItemIndex.TRAP_MIN + weight.Index;
                if (!dict.ContainsKey(index))
                {
                    dict.Add(index, weight.Weight);
                }
            }

            return dict;
        }

        private List<DungeonPokemonSpawnModel> LoadSpawns(DungeonBalance.WildPokemonInfo.FloorInfo data)
        {
            return data.Entries.Where(entry => entry.SpawnRate > 0).Select(entry => new DungeonPokemonSpawnModel
            {
                StatsIndex = (CreatureIndex) entry.PokemonIndex,
                SpawnRate = entry.SpawnRate,
                RecruitmentLevel = entry.RecruitmentLevel,
                Byte0B = entry.Byte0B,
            }).ToList();
        }

        public void Flush(IRtdxRom rom)
        {
            var commonStrings = rom.GetCommonStrings();
            var commonStringsEntry = new MessageBinEntry(rom.GetUSMessageBin().GetFile("common.bin")!, rom.GetCodeTable());
            var dungeonData = rom.GetDungeonDataInfo();
            var dungeonExtra = rom.GetDungeonExtra();
            var dungeonBalance = rom.GetDungeonBalance();
            var itemArrange = rom.GetItemArrange();
            var requestLevels = rom.GetRequestLevel();

            foreach (var dungeon in LoadedDungeons.Values)
            {
                var data = dungeonData.Entries[dungeon.Id];
                var extra = dungeonExtra.Entries.GetValueOrDefault(dungeon.Id);
                var requestLevel = requestLevels.Entries.GetValueOrDefault(dungeon.Id);
                var balance = dungeonBalance.Entries[data.DungeonBalanceIndex];
                var itemArrangeEntry = dungeon.Id > DungeonIndex.NONE
                    ? itemArrange.Entries[((int) dungeon.Id) - 1] : null;

                data.Features = dungeon.Features;
                data.Short0A = dungeon.DataInfoShort0A;
                data.SortKey = dungeon.SortKey;
                data.Byte13 = dungeon.DataInfoByte13;
                data.MaxItems = dungeon.MaxItems;
                data.MaxTeammates = dungeon.MaxTeammates;
                data.Byte17 = dungeon.DataInfoByte17;
                data.Byte18 = dungeon.DataInfoByte18;
                data.Byte19 = dungeon.DataInfoByte19;
                if (dungeon.AccessibleFloorCount > -1)
                {
                    if (extra != null)
                    {
                        extra.Floors = dungeon.AccessibleFloorCount;
                    }
                    if (requestLevel != null && requestLevel.MainEntry != null)
                    {
                        requestLevel.MainEntry.AccessibleFloorCount = dungeon.AccessibleFloorCount;
                    }
                }
                if (requestLevel != null && requestLevel.MainEntry != null)
                {
                    if (dungeon.UnknownFloorCount > -1)
                    {
                        requestLevel.MainEntry.Unk1 = dungeon.UnknownFloorCount;
                    }
                    if (dungeon.TotalFloorCount > -1)
                    {
                        requestLevel.MainEntry.TotalFloorCount = dungeon.TotalFloorCount;
                    }
                }

                if (dungeon.PokemonStats != null && balance.WildPokemon != null)
                {
                    FlushStats(dungeon.PokemonStats, balance.WildPokemon);
                }

                if (dungeon.ItemSets != null && itemArrangeEntry != null)
                {
                    FlushItemSets(dungeon.ItemSets, itemArrangeEntry);
                }

                FlushFloors(dungeon.Floors, balance, requestLevel);

                string hash = "DUNGEON_NAME__DUNGEON_" + dungeon.Id.ToString().ToUpper();
                commonStringsEntry.SetString(hash, dungeon.DungeonName);
            }


            rom.GetUSMessageBin().SetFile("common.bin", commonStringsEntry.ToByteArray());
        }

        private void FlushStats(List<DungeonPokemonStatsModel> models, DungeonBalance.WildPokemonInfo data)
        {
            var modelsDict = models.ToDictionary(model => (int) model.CreatureIndex);
            for (int i = 0; i < data.Stats.Length; i++)
            {
                var stats = data.Stats[i];
                if (modelsDict.ContainsKey(stats.Index))
                {
                    var model = modelsDict[i];
                    stats.XPYield = model.XpYield;
                    stats.HitPoints = model.HitPoints;
                    stats.Attack = model.Attack;
                    stats.SpecialAttack = model.SpecialAttack;
                    stats.Defense = model.Defense;
                    stats.SpecialDefense = model.SpecialDefense;
                    stats.Speed = model.Speed;
                    stats.StrongFoe = model.StrongFoe ? (byte) 1 : (byte) 0;
                    stats.Level = model.Level;
                }
                else
                {
                    stats.XPYield = 0;
                    stats.HitPoints = 0;
                    stats.Attack = 0;
                    stats.SpecialAttack = 0;
                    stats.Defense = 0;
                    stats.SpecialDefense = 0;
                    stats.Speed = 0;
                    stats.StrongFoe = 0;
                    stats.Level = 0;
                }
            }
        }

        private void FlushItemSets(List<ItemSetModel> models, ItemArrange.Entry data)
        {
            data.ItemSets.Clear();

            foreach (var model in models)
            {
                var kindWeights = new ushort[(int) ItemKind.MAX];
                foreach (var pair in model.ItemKindWeights)
                {
                    kindWeights[(int) pair.Key] = pair.Value;
                }
                data.ItemSets.Add(new ItemArrange.Entry.ItemSet(kindWeights, model.ItemWeights));
            }
        }

        private void FlushFloors(List<DungeonFloorModel> models, DungeonBalance.Entry balance,
            RequestLevel.Entry? requestLevel)
        {
            for (int i = 0; i < balance.FloorInfos.Length; i++)
            {
                var model = models.ElementAtOrDefault(i);
                if (model == null)
                {
                    continue;
                }

                var floorInfo = balance.FloorInfos[i];
                var requestLevelEntry = requestLevel?.MainEntry?.FloorData.ElementAtOrDefault(i);

                floorInfo.Short02 = model.BalanceFloorInfoShort02;
                floorInfo.Event = model.Event;
                floorInfo.Short24 = model.BalanceFloorInfoShort24;
                floorInfo.Short26 = model.BalanceFloorInfoShort26;
                floorInfo.Short28 = model.BalanceFloorInfoShort28;
                floorInfo.DungeonMapDataInfoIndex = model.DungeonMapDataInfoIndex;
                floorInfo.NameID = model.NameId;
                floorInfo.Byte2D = model.BalanceFloorInfoByte2D;
                floorInfo.Byte2E = model.BalanceFloorInfoByte2E;
                floorInfo.Byte2F = model.BalanceFloorInfoByte2F;
                floorInfo.Short30 = model.BalanceFloorInfoShort30;
                floorInfo.Short32 = model.BalanceFloorInfoShort32;
                floorInfo.Byte34 = model.BalanceFloorInfoByte34;
                floorInfo.Byte35 = model.BalanceFloorInfoByte35;
                floorInfo.ItemSetIndex = model.UnknownItemSetIndex;
                floorInfo.InvitationIndex = model.InvitationIndex;
                floorInfo.Byte55 = model.BalanceFloorInfoByte55;
                floorInfo.Byte56 = model.BalanceFloorInfoByte56;
                floorInfo.Byte57 = model.BalanceFloorInfoByte57;
                floorInfo.Byte58 = model.BalanceFloorInfoByte58;
                floorInfo.Weather = model.Weather;
                floorInfo.Bytes37to53 = model.BalanceFloorInfoBytes37to53;
                floorInfo.Bytes5Ato61 = model.BalanceFloorInfoBytes5Ato61;

                if (requestLevelEntry != null)
                {
                    requestLevelEntry.Short4 = model.BalanceFloorInfoShort02;
                    requestLevelEntry.Short6 = model.BalanceFloorInfoShort26;
                    requestLevelEntry.Short8 = model.BalanceFloorInfoShort28;
                    requestLevelEntry.NameID = model.NameId;
                    requestLevelEntry.IsBossFloor = model.IsBossFloor ? (short) 1 : (short) 0;
                }

                if (model.TrapWeights != null && balance.TrapWeights != null)
                {
                    FlushTrapWeights(model, balance.TrapWeights.Records[i]);
                }

                if (model.Spawns != null && balance.WildPokemon != null)
                {
                    FlushSpawns(model.Spawns, balance.WildPokemon.Floors[i]);
                }
            }
        }

        private void FlushTrapWeights(DungeonFloorModel model, DungeonBalance.TrapWeights.Record data)
        {
            foreach (var entry in data.Entries.SkipLast(1))
            {
                var modelWeight = model.TrapWeights![entry.Index + ItemIndex.TRAP_MIN];
                entry.Weight = modelWeight;
            }
        }

        private void FlushSpawns(List<DungeonPokemonSpawnModel> models, DungeonBalance.WildPokemonInfo.FloorInfo data)
        {
            var modelsDict = models.ToDictionary(model => model.StatsIndex);
            foreach (var entry in data.Entries)
            {
                if (modelsDict.ContainsKey((CreatureIndex) entry.PokemonIndex))
                {
                    var model = modelsDict[(CreatureIndex) entry.PokemonIndex];
                    entry.SpawnRate = model.SpawnRate;
                    entry.RecruitmentLevel = model.RecruitmentLevel;
                    entry.Byte0B = model.Byte0B;
                }
                else
                {
                    entry.SpawnRate = 0;
                    entry.RecruitmentLevel = 0;
                    entry.Byte0B = 0;
                }
            }
        }
    }
}
