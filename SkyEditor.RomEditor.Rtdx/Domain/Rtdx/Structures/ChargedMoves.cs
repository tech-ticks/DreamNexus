using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IChargedMoves
    {
        public IList<ChargedMoves.Entry> Entries { get; }

        public byte[] ToByteArray();
    }

    public class ChargedMoves : IChargedMoves
    {
        public const int EntrySize = 0xA;

        public ChargedMoves()
        {
            this.Entries = new List<Entry>();
        }

        public ChargedMoves(byte[] data)
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

        [DebuggerDisplay("{Short00}|{Short02}|{Short04}|{Short06}|{Short08}")]
        public class Entry
        {
            public Entry()
            {
            }

            public Entry(WazaIndex index, Span<byte> data)
            {
                BaseMove = (WazaIndex)MemoryMarshal.Read<ushort>(data.Slice(0x00, sizeof(ushort)));
                BaseAction = MemoryMarshal.Read<ushort>(data.Slice(0x02, sizeof(ushort)));
                FinalMove = (WazaIndex)MemoryMarshal.Read<ushort>(data.Slice(0x04, sizeof(ushort)));
                FinalAction = MemoryMarshal.Read<ushort>(data.Slice(0x06, sizeof(ushort)));
                Short08 = MemoryMarshal.Read<ushort>(data.Slice(0x08, sizeof(ushort)));
            }

            public ReadOnlySpan<byte> ToBytes()
            {
                IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize]);
                data.WriteUInt16(0x00, (ushort)BaseMove);
                data.WriteUInt16(0x02, BaseAction);
                data.WriteUInt16(0x04, (ushort)FinalMove);
                data.WriteUInt16(0x06, FinalAction);
                data.WriteUInt16(0x08, Short08);
                return data.ReadSpan();
            }

            public WazaIndex BaseMove { get; set; }
            public ushort BaseAction { get; set; }
            public WazaIndex FinalMove { get; set; }
            public ushort FinalAction { get; set; }
            public ushort Short08 { get; set; }
        }
    }
}
