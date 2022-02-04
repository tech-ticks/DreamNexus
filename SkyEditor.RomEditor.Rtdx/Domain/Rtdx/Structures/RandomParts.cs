using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IRandomParts
    {
        IList<RandomParts.RandomPartsEntry> Entries { get; }
        (byte[] bin, byte[] ent) Build();
    }

    public class RandomParts : IRandomParts
    {
        public IList<RandomPartsEntry> Entries { get; }

        public RandomParts()
        {
            Entries = new List<RandomPartsEntry>();
        }

        public RandomParts(byte[] data, byte[] entryList) : this(new BinaryFile(data), new BinaryFile(entryList))
        {
        }

        public RandomParts(IReadOnlyBinaryDataAccessor data, IReadOnlyBinaryDataAccessor entryList)
        {
            var entryCount = checked((int)entryList.Length / sizeof(int));
            var entries = new List<RandomPartsEntry>(entryCount);
            for (int i = 0; i < entryCount - 1; i++)
            {
                var entryOffset = entryList.ReadInt32(i * sizeof(int));
                var entryEnd = entryList.ReadInt32((i + 1) * sizeof(int));
                entries.Add(new RandomPartsEntry(i, data.Slice(entryOffset, entryEnd - entryOffset)));
            }
            this.Entries = entries;
        }

        public (byte[] bin, byte[] ent) Build()
        {
            MemoryStream bin = new MemoryStream();
            var entryPointers = new List<int>();

            // Build the .bin file data
            entryPointers.Add(0);
            foreach (var entry in Entries)
            {
                // Write data to .bin and the pointer to .ent
                // Align data to 16 bytes
                var binData = entry.ToSir0().Data.ReadArray();
                bin.Write(binData, 0, binData.Length);
                var paddingLength = 16 - (bin.Length % 16);
                if (paddingLength != 16)
                {
                    bin.SetLength(bin.Length + paddingLength);
                    bin.Position = bin.Length;
                }
                entryPointers.Add((int)bin.Position);
            }

            // Build the .ent file data
            var ent = new byte[entryPointers.Count * sizeof(int)];
            for (int i = 0; i < entryPointers.Count; i++)
            {
                BinaryPrimitives.WriteInt32LittleEndian(ent.AsSpan().Slice(i * sizeof(int)), entryPointers[i]);
            }

            return (bin.ToArray(), ent);
        }

        public class RandomPartsEntry
        {
            public int Index { get; set; }
            public short Width { get; set; }
            public short Height { get; set; }
            public IList<Entry> Entries { get; }

            public RandomPartsEntry(int index, short width, short height)
            {
                Index = index;
                Width = width;
                Height = height;
                Entries = new List<Entry>();
            }

            public RandomPartsEntry(int index, IReadOnlyBinaryDataAccessor data)
            {
                Index = index;
                var sir0 = new Sir0(data);
                Width = sir0.SubHeader.ReadInt16(0x0);
                Height = sir0.SubHeader.ReadInt16(0x2);
                var count = sir0.SubHeader.ReadInt32(0x4);
                var tableOffset = sir0.SubHeader.ReadInt32(0x8);
                var entries = new List<Entry>();
                for (var i = 0; i < count; i++)
                {
                    var offset = data.ReadInt32(tableOffset + i * 8);
                    entries.Add(new Entry(i, data.Slice(offset, data.Length), Width, Height));
                }
                Entries = entries;
            }

            public Sir0 ToSir0()
            {
                var sir0 = new Sir0Builder(8);

                var entryPointers = new List<int>();
                foreach (var entry in Entries)
                {
                    entryPointers.Add(sir0.Length);
                    sir0.Write(sir0.Length, entry.ToByteArray());
                    sir0.Align(16);
                }

                var pointerTablePos = sir0.Length;
                foreach (var pointer in entryPointers)
                {
                    sir0.WritePointer(sir0.Length, pointer);
                }

                sir0.SubHeaderOffset = sir0.Length;
                sir0.WriteInt16(sir0.Length, Width);
                sir0.WriteInt16(sir0.Length, Height);
                sir0.WriteInt32(sir0.Length, Entries.Count);
                sir0.WritePointer(sir0.Length, pointerTablePos);
                return sir0.Build();
            }

            public class Entry
            {
                public int Index { get; set; }
                public ConnectionFlags Connections { get; set; }
                public Tile[,] Tiles { get; }

                private readonly short width;
                private readonly short height;

                public Entry(int index, short width, short height)
                {
                    Index = index;
                    this.width = width;
                    this.height = height;
                    Tiles = new Tile[width, height];
                }

                public Entry(int index, IReadOnlyBinaryDataAccessor data, short width, short height)
                {
                    Index = index;
                    this.width = width;
                    this.height = height;
                    Connections = (ConnectionFlags)data.ReadInt16(0x0);
                    var tiles = new Tile[width, height];
                    for (var y = 0; y < height; y++)
                    {
                        for (var x = 0; x < width; x++)
                        {
                            var offset = 2 + (x + y * width) * 2;
                            tiles[x, y] = new Tile
                            {
                                Byte0 = data.ReadByte(offset),
                                Byte1 = data.ReadByte(offset + 1)
                            };
                        }
                    }
                    Tiles = tiles;
                }

                public byte[] ToByteArray()
                {
                    var data = new byte[2 + width * height * 2];
                    BinaryPrimitives.WriteInt64LittleEndian(data.AsSpan().Slice(0), (short)Connections);
                    for (var y = 0; y < height; y++)
                    {
                        for (var x = 0; x < width; x++)
                        {
                            data[2 + (x + y * width) * 2 + 0] = Tiles[x, y].Byte0;
                            data[2 + (x + y * width) * 2 + 1] = Tiles[x, y].Byte1;
                        }
                    }
                    return data;
                }

                [Flags]
                public enum ConnectionFlags : ushort
                {
                    None = 0,

                    South = 1 << 0,
                    East = 1 << 1,
                    North = 1 << 2,
                    West = 1 << 3
                }

                public struct Tile
                {
                    public byte Byte0;
                    public byte Byte1;
                }
            }
        }
    }
}
