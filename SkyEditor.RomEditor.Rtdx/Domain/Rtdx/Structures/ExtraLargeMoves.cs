using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IExtraLargeMoves
    {
        public IList<ExtraLargeMoves.Entry> Entries { get; }

        public byte[] ToByteArray();
    }

    public class ExtraLargeMoves : IExtraLargeMoves
    {
        public const int EntrySize = 0x8;

        public ExtraLargeMoves()
        {
            this.Entries = new List<Entry>();
        }

        public ExtraLargeMoves(byte[] data)
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

        [DebuggerDisplay("{Short00}|{Short02}|{Short04}|{Short06}")]
        public class Entry
        {
            public Entry()
            {
            }

            public Entry(WazaIndex index, Span<byte> data)
            {
                BaseMove = (WazaIndex)MemoryMarshal.Read<ushort>(data.Slice(0x00, sizeof(ushort)));
                LargeMove = (WazaIndex)MemoryMarshal.Read<ushort>(data.Slice(0x02, sizeof(ushort)));
                BaseAction = MemoryMarshal.Read<ushort>(data.Slice(0x04, sizeof(ushort)));
                LargeAction = MemoryMarshal.Read<ushort>(data.Slice(0x06, sizeof(ushort)));
            }

            public ReadOnlySpan<byte> ToBytes()
            {
                IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize]);
                data.WriteUInt16(0x00, (ushort)BaseMove);
                data.WriteUInt16(0x02, (ushort)LargeMove);
                data.WriteUInt16(0x04, BaseAction);
                data.WriteUInt16(0x06, LargeAction);
                return data.ReadSpan();
            }

            public WazaIndex BaseMove { get; set; }
            public WazaIndex LargeMove { get; set; }
            public ushort BaseAction { get; set; }
            public ushort LargeAction { get; set; }
        }
    }
}
