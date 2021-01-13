using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SkyEditor.IO.Binary;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IDungeonMapDataInfo
    {
        public IList<DungeonMapDataInfo.Entry> Entries { get; }

        public byte[] ToByteArray();
    }

    /// <summary>
    /// Contains reusable data for dungeon floors, such as the music and fixed map index.
    /// For each entry in this file, there's also an entry for the tileset in dungeon_map_symbol.bin
    /// with the same index.
    /// </summary>
    public class DungeonMapDataInfo : IDungeonMapDataInfo
    {
        public const int EntrySize = 0xc;

        public DungeonMapDataInfo()
        {
            this.Entries = new List<Entry>();
        }

        public DungeonMapDataInfo(byte[] data)
        {
            var entries = new List<Entry>();
            for (int i = 0; i < data.Length / EntrySize; i++)
            {
                entries.Add(new Entry(data.AsSpan(i * EntrySize, EntrySize)));
            }
            this.Entries = entries;
        }

        public byte[] ToByteArray()
        {
            IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize * Entries.Count]);
            int currentIndex = 0;
            foreach (var entry in Entries)
            {
                data.Write(currentIndex, entry.ToBytes());
                currentIndex += EntrySize;
            }
            return data.ReadArray();
        }

        public IList<Entry> Entries { get; }

        [DebuggerDisplay("{Name}")]
        public class Entry
        {
            public Entry() { }

            public Entry(Span<byte> data)
            {
                // short 0x0 is a redundant entry index and seems to be ignored by the game
                Index = MemoryMarshal.Read<ushort>(data.Slice(0x0, sizeof(ushort)));
                FixedMapIndex = MemoryMarshal.Read<ushort>(data.Slice(0x4, sizeof(ushort)));
                Byte06 = data[0x6];
                Byte07 = data[0x7];
                DungeonBgmSymbolIndex = data[0x8];
                Byte09 = data[0x9];
                Byte0A = data[0xA];
                Byte0B = data[0xB];
            }

            public ReadOnlySpan<byte> ToBytes()
            {
                IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize]);
                data.WriteUInt16(0x0, Index);
                data.WriteUInt16(0x4, FixedMapIndex);
                data.Write(0x6, Byte06);
                data.Write(0x7, Byte07);
                data.Write(0x8, DungeonBgmSymbolIndex);
                data.Write(0x9, Byte09);
                data.Write(0xA, Byte0A);
                data.Write(0xB, Byte0B);

                return data.ReadSpan();
            }

            // Seems to be ignored by the game
            public ushort Index { get; set; }

            // The index of the last value in fixed_map.ent (pointing to the end) if no fixed map is used.
            public ushort FixedMapIndex { get; set; }

            public byte Byte06 { get; set; }
            public byte Byte07 { get; set; }
            public byte DungeonBgmSymbolIndex { get; set; }
            public byte Byte09 { get; set; }
            public byte Byte0A { get; set; }
            public byte Byte0B { get; set; }

        }
    }
}
