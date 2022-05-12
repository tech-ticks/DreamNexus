using System.Collections.Generic;
using System.Linq;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.FixedMap;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IFixedMapCollection
    {
        IDictionary<int, FixedMapModel> LoadedEntries { get; }
        int Count { get; }
        void SetEntry(int id, FixedMapModel model);
        bool IsEntryDirty(int id);
        FixedMapModel? GetEntryById(int id, bool markAsDirty = true);
        void Flush(IRtdxRom rom);
    }

    public class FixedMapCollection : IFixedMapCollection
    {
        public IDictionary<int, FixedMapModel> LoadedEntries { get; } = new Dictionary<int, FixedMapModel>();
        public HashSet<int> DirtyEntries { get; } = new HashSet<int>();
        public int Count { get; private set; }

        private IRtdxRom rom;

        public FixedMapCollection(IRtdxRom rom)
        {
            Count = rom.GetFixedMap().Entries.Count;
            this.rom = rom;
        }

        public FixedMapModel LoadEntry(int id)
        {
            var data = rom.GetFixedMap().Entries[id];

            return new FixedMapModel(data.Width, data.Height)
            {
                Width = data.Width,
                Height = data.Height,
                Creatures = data.Creatures.Select(creature => new FixedMapCreatureModel
                {
                    XPos = creature.XPos,
                    YPos = creature.YPos,
                    Byte02 = creature.Byte02,
                    Byte03 = creature.Byte03,
                    Index = creature.Index,
                    Direction = creature.Direction,
                    Faction = creature.Faction,
                    Byte07 = creature.Byte07,
                }).ToList(),
                Items = data.Items.Select(item => new FixedMapItemModel
                {
                    XPos = item.XPos,
                    YPos = item.YPos,
                    Byte02 = item.Byte02,
                    Byte03 = item.Byte03,
                    FixedItemIndex = item.FixedItemIndex,
                    Variation = item.Variation,
                    Byte07 = item.Byte07,
                }).ToList(),
                Tiles = data.Tiles.Select(tile => new FixedMapTileModel
                {
                    Type = tile.Type,
                    Byte01 = tile.Byte01,
                    RoomId = tile.RoomId,
                }).ToArray()
            };
        }

        public FixedMapModel? GetEntryById(int id, bool markAsDirty = true)
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

        public void SetEntry(int id, FixedMapModel model)
        {
            LoadedEntries[id] = model;
        }

        public bool IsEntryDirty(int id)
        {
            return DirtyEntries.Contains(id);
        }

        public void Flush(IRtdxRom rom)
        {
            var romEntries = rom.GetFixedMap().Entries;
            foreach (var kv in LoadedEntries)
            {
                var id = kv.Key;
                var entry = kv.Value;

                var romEntry = romEntries[id];
                
                romEntry.Width = entry.Width;
                romEntry.Height = entry.Height;
                romEntry.Creatures = entry.Creatures.Select(creature => new FixedMapCreature
                {
                    XPos = creature.XPos,
                    YPos = creature.YPos,
                    Byte02 = creature.Byte02,
                    Byte03 = creature.Byte03,
                    Index = creature.Index,
                    Direction = creature.Direction,
                    Faction = creature.Faction,
                    Byte07 = creature.Byte07,
                }).ToList();
                romEntry.Items = entry.Items.Select(item => new FixedMapItem
                {
                    XPos = item.XPos,
                    YPos = item.YPos,
                    Byte02 = item.Byte02,
                    Byte03 = item.Byte03,
                    FixedItemIndex = item.FixedItemIndex,
                    Variation = item.Variation,
                    Byte07 = item.Byte07,
                }).ToList();
                romEntry.Tiles = entry.Tiles.Select(tile => new FixedMapTile
                {
                    Type = tile.Type,
                    Byte01 = tile.Byte01,
                    RoomId = tile.RoomId,
                }).ToArray();
            }
        }
    }
}
