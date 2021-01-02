using System.Collections.Generic;
using System.Text;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class FixedMap
    {
        public List<Entry> Entries { get; } = new List<Entry>();

        public FixedMap()
        {
        }

        public FixedMap(byte[] data)
        {
            // The file contains multiple SIR0 containers
            int startOffset = 0;
            for (int i = 4; i < data.Length - 4; i += 4)
            {
                if (Encoding.ASCII.GetString(data, i, 4) == "SIR0")
                {
                    var sir0 = new Sir0(data, startOffset, i - startOffset);
                    Entries.Add(new Entry(sir0));
                    startOffset = i;
                }
            }
            var lastSir0 = new Sir0(data, startOffset, data.Length - startOffset);
            Entries.Add(new Entry(lastSir0));
        }
        
        public byte[] ToByteArray() => throw new System.NotImplementedException();

        public class Entry
        {
            public Entry(Sir0 sir0)
            {
                Width = sir0.SubHeader.ReadUInt16(0x0);
                Height = sir0.SubHeader.ReadUInt16(0x2);

                int creatureCount = sir0.SubHeader.ReadUInt16(0x4);
                int itemCount = sir0.SubHeader.ReadUInt16(0x6);

                var data = sir0.Data.Slice(0x18, sir0.Data.Length - 0x18);

                // Read floor layout
                int tileCount = Width * Height;
                Tiles = new FixedMapTile[tileCount];
                for (int i = 0; i < tileCount; i++)
                {
                    int relativeOffset = i * FixedMapTile.EntrySize;
                    Tiles[i] = new FixedMapTile
                    {
                        Type = data.ReadByte(relativeOffset),
                        Byte01 = data.ReadByte(relativeOffset + 1),
                        RoomId = data.ReadByte(relativeOffset + 2),
                    };
                }

                // Read creature data
                Creatures = new List<FixedMapCreature>();

                int creaturesOffset = Width * Height * 4;

                // Ignore padding
                while (data.ReadByte(creaturesOffset) == 0)
                {
                    creaturesOffset++;
                }

                for (int i = 0; i < creatureCount; i++)
                {
                    int relativeOffset = i * FixedMapCreature.EntrySize + creaturesOffset;
                    Creatures.Add(new FixedMapCreature
                    {
                        XPos = data.ReadByte(relativeOffset),
                        YPos = data.ReadByte(relativeOffset + 1),
                        Index = (FixedCreatureIndex) data.ReadByte(relativeOffset + 4),
                        UnknownFlags = data.ReadByte(relativeOffset + 5),
                        Faction = (FixedMapCreature.CreatureFaction) data.ReadByte(relativeOffset + 6),
                    });
                }

                int itemsOffset = creaturesOffset + creatureCount * FixedMapCreature.EntrySize;
                // Ignore padding
                while (data.ReadByte(itemsOffset) == 0)
                {
                    itemsOffset++;
                }

                // Read item data
                Items = new List<FixedMapItem>();
                for (int i = 0; i < itemCount; i++)
                {
                    int relativeOffset = i * FixedMapItem.EntrySize + creaturesOffset;
                    Items.Add(new FixedMapItem
                    {
                        XPos = data.ReadByte(relativeOffset),
                        YPos = data.ReadByte(relativeOffset + 1),
                        UnknownItemIndex = data.ReadByte(relativeOffset + 4),
                        Direction = data.ReadByte(relativeOffset + 5),
                        UnknownItemType = data.ReadByte(relativeOffset + 6),
                    });
                }
            }

            public FixedMapTile GetTile(int x, int y)
            {
                return Tiles[y * Width + x];
            }

            public void SetTile(int x, int y, FixedMapTile tile)
            {
                Tiles[y * Width + x] = tile;
            }

            public ushort Width { get; set; }
            public ushort Height { get; set; }

            public List<FixedMapCreature> Creatures { get; set; }
            public List<FixedMapItem> Items { get; set; }
            public FixedMapTile[] Tiles { get; set; }
        }

        public class FixedMapCreature
        {
            public const int EntrySize = 8;

            public ushort XPos { get; set; }
            public ushort YPos { get; set; }

            public FixedCreatureIndex Index { get; set; }

            // Seems to encode where the Pokémon is facing and maybe other things
            // (0x00 = facing down, 0x80 = facing up)
            public byte UnknownFlags { get; set; }

            public CreatureFaction Faction { get; set; }

            // TODO: try what happens with values 3 and 4
            public enum CreatureFaction : byte
            {
                Player = 1,
                Ally = 2,
                Enemy = 5,
            }
        }

        public class FixedMapItem
        {
            public const int EntrySize = 8;

            public ushort XPos { get; set; }
            public ushort YPos { get; set; }

            // Item index. Probably related to fixed_item.bin
            // 0x9C = Stairs, 0xCC = Evolution crystal (Index might vary per dungeon)
            public byte UnknownItemIndex { get; set; }

            // Seems to encode where the item is facing
            // 0 = down, 1 = right, 2 = up, 3 = left?
            // TODO: investigate
            public byte Direction { get; set; }

            // 1 for normal items and 3 for stairs. Might be related to whether the item can be picked up?
            // TODO: check this and investigate fixed map 9 (contains an item with value 2)
            public byte UnknownItemType { get; set; }
        }

        public class FixedMapTile
        {
            public const int EntrySize = 4;

            // 0 = wall, 2 = floor, 3 = secondary terrain?, 5 = chasm
            public byte Type { get; set; }

            // Unknown flags. Mostly set to 8 in rooms, 0 outside in corridors.
            // Other values like 128 are also sometimes used.
            public byte Byte01 { get; set; }

            // Identifies which room inside the map a tile belongs to (possibly relevant for moves
            // that hit all targets in the room). 0 in walls and corridors.
            public byte RoomId { get; set; }
        }
    }
}
