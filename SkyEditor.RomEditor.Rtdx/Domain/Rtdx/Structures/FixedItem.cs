using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IFixedItem
    {
        IList<FixedItem.Entry> Entries { get; }
        Sir0 Build();
    }

    public class FixedItem : IFixedItem
    {
        public IList<Entry> Entries { get; }

        public FixedItem()
        {
            Entries = new List<Entry>();
        }

        public FixedItem(byte[] data) : this(new BinaryFile(data))
        {
        }

        public FixedItem(IReadOnlyBinaryDataAccessor data)
        {
            Sir0 sir0 = new Sir0(data);
            var count = sir0.SubHeader.ReadInt32(0x0);
            var entries = new List<Entry>();
            for (var i = 0; i < count; i++)
            {
                var pointer = sir0.SubHeader.ReadInt32(8 + i * 8);
                entries.Add(new Entry(data.Slice(pointer, Entry.EntrySize)));
            }
            Entries = entries;
        }

        public Sir0 Build()
        {
            var sir0 = new Sir0Builder(8);

            var offsets = new List<int>();
            foreach (var entry in Entries)
            {
                offsets.Add(sir0.Length);
                var entryData = entry.ToByteArray();
                sir0.Write(sir0.Length, entryData);
            }

            sir0.Align(16);
            sir0.SubHeaderOffset = sir0.Length;
            sir0.WriteInt32(sir0.Length, Entries.Count);
            sir0.Align(8);
            foreach (var offset in offsets)
            {
                sir0.WritePointer(sir0.Length, offset);
            }

            return sir0.Build();
        }
        
        public byte[] ToByteArray() => Build().Data.ReadArray();

        public class Entry
        {
            public const int EntrySize = 0x10;

            public ItemIndex Index { get; set; }
            public ushort Quantity { get; set; } // Poké amount for MONEY_POKE, stack size for throwables, etc.
            public ushort Short04 { get; set; } // Used with chests, unsure what that means
            public byte Byte06 { get; set; } // Set to 1 for traps, also unsure what that means

            public Entry()
            {
            }

            public Entry(ItemIndex index)
            {
                Index = index;
            }

            public Entry(IReadOnlyBinaryDataAccessor data)
            {
                Index = (ItemIndex)data.ReadInt16(0x0);
                Quantity = data.ReadUInt16(0x2);
                Short04 = data.ReadByte(0x4);
                Byte06 = data.ReadByte(0x6);
            }

            public byte[] ToByteArray()
            {
                var data = new byte[EntrySize];
                BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan().Slice(0x0), (short)Index);
                BinaryPrimitives.WriteUInt16LittleEndian(data.AsSpan().Slice(0x2), Quantity);
                BinaryPrimitives.WriteUInt16LittleEndian(data.AsSpan().Slice(0x4), Short04);
                data[0x6] = Byte06;
                return data;
            }
        }
    }
}
