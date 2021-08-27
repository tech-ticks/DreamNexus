using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IWazaDataInfo
    {
        public IList<WazaDataInfo.Entry> Entries { get; }

        public byte[] ToByteArray();
    }

    public class WazaDataInfo : IWazaDataInfo
    {
        public const int EntrySize = 0x12;

        public WazaDataInfo()
        {
            this.Entries = new List<Entry>();
        }

        public WazaDataInfo(byte[] data)
        {
            var entries = new List<Entry>();
            for (int i = 0; i < data.Length / EntrySize; i++)
            {
                entries.Add(new Entry((WazaIndex)i, data.AsSpan(i * EntrySize, EntrySize)));
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

        [DebuggerDisplay("{Short00}|{Short02}|{Short04}|{ActIndex}|{Short0E}|{Byte10}|{Byte11}")]
        public class Entry
        {
            public Entry()
            {
            }

            public Entry(WazaIndex index)
            {
                Index = index;
            }

            public Entry(WazaIndex index, Span<byte> data)
            {
                Index = index;
                Short00 = MemoryMarshal.Read<ushort>(data.Slice(0x00, sizeof(ushort)));
                Short02 = MemoryMarshal.Read<ushort>(data.Slice(0x02, sizeof(ushort)));
                Short04 = MemoryMarshal.Read<ushort>(data.Slice(0x04, sizeof(ushort)));
                ActIndex = MemoryMarshal.Read<ushort>(data.Slice(0x0C, sizeof(ushort)));
                Short0E = MemoryMarshal.Read<ushort>(data.Slice(0x0E, sizeof(ushort)));
                Byte10 = data[0x10];
                Byte11 = data[0x11];
            }

            public ReadOnlySpan<byte> ToBytes()
            {
                IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize]);
                data.WriteUInt16(0x00, Short00);
                data.WriteUInt16(0x02, Short02);
                data.WriteUInt16(0x04, Short04);
                data.WriteUInt16(0x0C, ActIndex);
                data.WriteUInt16(0x0E, Short0E);
                data.Write(0x10, Byte10);
                data.Write(0x11, Byte11);
                return data.ReadSpan();
            }

            public Entry Clone()
            {
                return new Entry
                {
                    Index = Index,
                    Short00 = Short00,
                    Short02 = Short02,
                    Short04 = Short04,
                    ActIndex = ActIndex,
                    Short0E = Short0E,
                    Byte10 = Byte10,
                    Byte11 = Byte11,
                };
            }

            public WazaIndex Index { get; set; }
            public ushort Short00 { get; set; } // only entry 82 (Dragon Rage) has a non-zero value (0x07)
            public ushort Short02 { get; set; } // only entry 82 (Dragon Rage) has a non-zero value (0xA7)
            public ushort Short04 { get; set; } // only entry 82 (Dragon Rage) has a non-zero value (0xC4)
            public ushort ActIndex { get; set; }  // Index into the ActDataInfo table
            public ushort Short0E { get; set; } // Most values seem to be 10000, some go as low as ~5000, a few as high as ~19000
            public byte Byte10 { get; set; } // Seems to indicate if the move is usable by the player
            public byte Byte11 { get; set; } // All 100s
        }
    }
}
