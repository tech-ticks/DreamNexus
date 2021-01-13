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
                Short00 = MemoryMarshal.Read<ushort>(data.Slice(0x00, sizeof(ushort)));
                Short02 = MemoryMarshal.Read<ushort>(data.Slice(0x02, sizeof(ushort)));
                Short04 = MemoryMarshal.Read<ushort>(data.Slice(0x04, sizeof(ushort)));
                Index = (WazaIndex)MemoryMarshal.Read<ushort>(data.Slice(0x0C, sizeof(ushort)));
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
                data.WriteUInt16(0x0C, (ushort)Index);
                data.WriteUInt16(0x0E, Short0E);
                data.Write(0x10, Byte10);
                data.Write(0x11, Byte11);
                return data.ReadSpan();
            }

            public ushort Short00 { get; set; } // only entry 82 has a non-zero value (0x07)
            public ushort Short02 { get; set; } // only entry 82 has a non-zero value (0xA7)
            public ushort Short04 { get; set; } // only entry 82 has a non-zero value (0xC4)
            public WazaIndex Index { get; set; }
            public ushort Short0E { get; set; }
            public byte Byte10 { get; set; }
            public byte Byte11 { get; set; }
        }
    }
}
