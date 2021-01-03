using System;
using System.Collections.Generic;
using System.IO;
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

        public FixedMap(byte[] binData, byte[] entData)
        {
            var binFile = new BinaryFile(binData);
            var entFile = new BinaryFile(entData);

            var entCount = entFile.Length / sizeof(uint) - 1;
            Entries = new List<Entry>();
            for (var i = 0; i < entCount; i++)
            {
                var curr = entFile.ReadInt32(i * sizeof(int));
                var next = entFile.ReadInt32((i + 1) * sizeof(int));
                var sir0 = new Sir0(binFile.Slice(curr, next - curr));
                Entries.Add(new Entry(sir0));
            }
        }
        
        public (byte[] bin, byte[] ent) Build()
        {
            var binFile = new BinaryFile(new MemoryStream());
            var entryPointers = new List<int>();

            void align(int length) 
            {
                var paddingLength = length - (binFile!.Length % length);
                if (paddingLength != length)
                {
                    binFile.Write(binFile.Length, new byte[paddingLength]);
                }
            }

            foreach (var entry in Entries)
            {
                align(32);
                entryPointers.Add((int) binFile.Length);
                binFile.Write(binFile.Length, entry.ToSir0().Data.ReadArray());
            }
            entryPointers.Add((int) binFile.Length);

            // Build the .ent file data
            var ent = new byte[entryPointers.Count * sizeof(int)];
            for (int i = 0; i < entryPointers.Count; i++)
            {
                BitConverter.GetBytes(entryPointers[i]).CopyTo(ent, i * sizeof(int));
            }
            return (binFile.ReadArray(), ent);
        }

        public class Entry
        {
            public Entry(Sir0 sir0)
            {
                Width = sir0.SubHeader.ReadUInt16(0x0);
                Height = sir0.SubHeader.ReadUInt16(0x2);

                int creatureCount = sir0.SubHeader.ReadUInt16(0x4);
                int itemCount = sir0.SubHeader.ReadUInt16(0x6);

                var data = sir0.Data.Slice(0x20, sir0.Data.Length - 0x20);

                // Read floor layout
                int tileCount = Width * Height;
                Tiles = new FixedMapTile[tileCount];
                for (int i = 0; i < tileCount; i++)
                {
                    int relativeOffset = i * FixedMapTile.EntrySize;
                    Tiles[i] = new FixedMapTile
                    {
                        Type = (FixedMapTile.TileType) data.ReadByte(relativeOffset),
                        Byte01 = data.ReadByte(relativeOffset + 1),
                        RoomId = data.ReadByte(relativeOffset + 2),
                    };
                }

                int creaturesOffset = Width * Height * 4;

                // Ignore padding
                while (data.ReadByte(creaturesOffset) == 0)
                {
                    creaturesOffset++;
                }

                // Read creature data
                Creatures = new List<FixedMapCreature>();
                for (int i = 0; i < creatureCount; i++)
                {
                    int relativeOffset = i * FixedMapCreature.EntrySize + creaturesOffset;

                    // The two high bits contain the direction
                    ushort creatureIndexAndDirection = data.ReadUInt16(relativeOffset + 4);
                    Creatures.Add(new FixedMapCreature
                    {
                        XPos = data.ReadByte(relativeOffset),
                        YPos = data.ReadByte(relativeOffset + 1),
                        Index = (FixedCreatureIndex) (creatureIndexAndDirection & 0x3FFFu),
                        Direction = (EntityDirection) (creatureIndexAndDirection >> 14),
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
                    int relativeOffset = i * FixedMapItem.EntrySize + itemsOffset;
                    Items.Add(new FixedMapItem
                    {
                        XPos = data.ReadByte(relativeOffset),
                        YPos = data.ReadByte(relativeOffset + 1),
                        UnknownItemIndex = data.ReadByte(relativeOffset + 4),
                        Direction = (EntityDirection) data.ReadByte(relativeOffset + 5),
                        UnknownItemVariation = data.ReadByte(relativeOffset + 6),
                    });
                }
            }

            public Sir0 ToSir0()
            {
                if (Tiles.Length != Width * Height)
                {
                    throw new InvalidOperationException($"Length of {nameof(Tiles)} doesn't match room width * height");
                }

                var sir0 = new Sir0Builder(8);

                void align(int length)
                {
                    // Everything must be aligned inside the entire file, not just the subentry SIR0
                    var paddingLength = length - (sir0.Length % length);
                    if (paddingLength != length)
                    {
                        sir0.WritePadding(sir0.Length, paddingLength);
                    }
                }

                int layoutPointer = sir0.Length;
                foreach (var tile in Tiles)
                {
                    sir0.Write(sir0.Length, (byte) tile.Type);
                    sir0.Write(sir0.Length, tile.Byte01);
                    sir0.Write(sir0.Length, tile.RoomId);
                    sir0.Write(sir0.Length, 0);
                }

                align(16);

                int creaturesPointer = sir0.Length;
                foreach (var creature in Creatures)
                {
                    ushort creatureIndexAndDirection = (ushort) ((int) creature.Index | (((int) creature.Direction) << 14));
                    sir0.Write(sir0.Length, creature.XPos);
                    sir0.Write(sir0.Length, creature.YPos);
                    sir0.WriteInt16(sir0.Length, 0);
                    sir0.WriteUInt16(sir0.Length, creatureIndexAndDirection);
                    sir0.Write(sir0.Length, (byte) creature.Faction);
                    sir0.Write(sir0.Length, 0);
                }

                align(16);
                int itemsPointer = sir0.Length;
                foreach (var item in Items)
                {
                    sir0.Write(sir0.Length, item.XPos);
                    sir0.Write(sir0.Length, item.YPos);
                    sir0.WriteUInt16(sir0.Length, 0);
                    sir0.Write(sir0.Length, (byte) item.UnknownItemIndex);
                    sir0.Write(sir0.Length, (byte) item.Direction);
                    sir0.Write(sir0.Length, item.UnknownItemVariation);
                    sir0.Write(sir0.Length, 0);
                }

                align(16);
                
                int subHeaderPointer = sir0.Length;

                sir0.WriteUInt16(sir0.Length, Width);
                sir0.WriteUInt16(sir0.Length, Height);
                sir0.WriteUInt16(sir0.Length, (ushort) Creatures.Count);
                sir0.WriteUInt16(sir0.Length, (ushort) Items.Count);

                align(16);

                sir0.SubHeaderOffset = subHeaderPointer;
                sir0.WritePointer(sir0.Length, layoutPointer);
                sir0.WriteInt64(sir0.Length, 0);
                sir0.WritePointer(sir0.Length, creaturesPointer);
                sir0.WritePointer(sir0.Length, itemsPointer);

                return sir0.Build(alignFooter: false); // There shouldn't be any padding before the footer
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

            public byte XPos { get; set; }
            public byte YPos { get; set; }

            public FixedCreatureIndex Index { get; set; }

            public EntityDirection Direction { get; set; }

            public CreatureFaction Faction { get; set; }

            public enum CreatureFaction : byte
            {
                Player = 1,
                Ally = 2,
                MysteryHousePokemon = 3,
                Enemy = 5,
            }
        }

        public class FixedMapItem
        {
            public const int EntrySize = 8;

            public byte XPos { get; set; }
            public byte YPos { get; set; }

            // Item index. Probably related to fixed_item.bin
            // 0x9C = Stairs, 0xCC = Evolution crystal (Index might vary per dungeon)
            public byte UnknownItemIndex { get; set; }

            // It seems like non-zero values cause crashes on items that can't be rotated.
            public EntityDirection Direction { get; set; }

            // 1 for upwards stairs and 3 for downwards stairs. 2 causes all items to disappear (?)
            // Non-stairs items seem to behave the same no matter if the this is set to 1 or 3.
            // Might be bit flags?
            public byte UnknownItemVariation { get; set; }
        }

        public class FixedMapTile
        {
            public const int EntrySize = 4;

            // 0 = wall, 2 = floor, 3 = secondary terrain?, 5 = chasm, 14 = mystery house door
            public TileType Type { get; set; }

            // Unknown flags. Mostly set to 8 in rooms, 0 outside in corridors.
            // Other values like 4 and 128 are also sometimes used.
            public byte Byte01 { get; set; }

            // Identifies which room inside the map a tile belongs to (possibly relevant for moves
            // that hit all targets in the room). 0 in walls and corridors.
            public byte RoomId { get; set; }

            public enum TileType
            {
                Wall = 0,
                Floor = 2,
                MaybeSecondaryTerrain = 3, // Might be water, lava etc. depending on the dungeon. TODO: investigate
                Chasm = 5,
                MysteryHouseDoor = 15,

                // TODO: see if there are more, maybe some leftovers from PSMD?
            }
        }

        public enum EntityDirection : byte
        {
            Down = 0,
            Right = 1,
            Up = 2,
            Left = 3,
        }
    }
}
