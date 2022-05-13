using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.FixedMap;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.FixedMap.FixedMapCreature;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.FixedMap.FixedMapItem;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.FixedMap.FixedMapTile;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class FixedMapModel
    {
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public List<FixedMapCreatureModel> Creatures { get; set; } = new List<FixedMapCreatureModel>();
        public List<FixedMapItemModel> Items { get; set; } = new List<FixedMapItemModel>();
        public FixedMapTileModel[] Tiles { get; set; } = new FixedMapTileModel[0];

        public FixedMapModel()
        {
        }

        public FixedMapModel(ushort width, ushort height)
        {
            Resize(width, height);
        }

        public FixedMapTileModel GetTile(int x, int y)
        {
            return Tiles[y * Width + x];
        }

        public void SetTile(int x, int y, FixedMapTileModel tile)
        {
            Tiles[y * Width + x] = tile;
        }

        public FixedMapCreatureModel? GetCreature(int x, int y)
        {
            return Creatures.Find(c => c.XPos == x && c.YPos == y);
        }

        public FixedMapItemModel? GetItem(int x, int y)
        {
            return Items.Find(i => i.XPos == x && i.YPos == y);
        }

        public void Resize(ushort width, ushort height)
        {
            Width = width;
            Height = height;
            Tiles = new FixedMapTileModel[width * height];
        }
    }

    public class FixedMapCreatureModel
    {
        public byte XPos { get; set; }
        public byte YPos { get; set; }

        // Always zero
        public byte Byte02 { get; set; }

        // Always zero
        public byte Byte03 { get; set; }

        public FixedCreatureIndex Index { get; set; }

        public EntityDirection Direction { get; set; }

        public CreatureFaction Faction { get; set; }

        // Always zero
        public byte Byte07 { get; set; }
    }

    public class FixedMapItemModel
    {
        public byte XPos { get; set; }
        public byte YPos { get; set; }

        // Apparently always zero on maps that weren't copied from PSMD
        public byte Byte02 { get; set; }

        // Apparently always zero on maps that weren't copied from PSMD
        public byte Byte03 { get; set; }

        // Fixed item index (an index into fixed_item.bin)
        public ushort FixedItemIndex { get; set; }

        public ItemVariation Variation { get; set; }

        // Apparently always zero on maps that weren't copied from PSMD
        public byte Byte07 { get; set; }
    }

    public class FixedMapTileModel
    {
        // 0 = wall, 2 = floor, 3 = secondary terrain?, 5 = chasm, 14 = mystery house door
        public TileType Type { get; set; }

        // Unknown flags. Mostly set to 8 in rooms, 0 outside in corridors.
        // Other values like 4 and 128 are also sometimes used.
        public byte Byte01 { get; set; }

        // Identifies which room inside the map a tile belongs to (possibly relevant for moves
        // that hit all targets in the room). 0 in walls and corridors.
        public byte RoomId { get; set; }
    }
}
