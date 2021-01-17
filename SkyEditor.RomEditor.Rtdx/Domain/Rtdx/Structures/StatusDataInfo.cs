using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IStatusDataInfo
    {
        public IList<StatusDataInfo.Entry> Entries { get; }

        public byte[] ToByteArray();
    }

    public class StatusDataInfo : IStatusDataInfo
    {
        public const int EntrySize = 0x30;

        public StatusDataInfo()
        {
            this.Entries = new List<Entry>();
        }

        public StatusDataInfo(byte[] data) : this(new BinaryFile(data))
        {
        }

        public StatusDataInfo(IReadOnlyBinaryDataAccessor data)
        {
            var entries = new List<Entry>();
            for (int i = 0; i < data.Length / EntrySize; i++)
            {
                entries.Add(new Entry((StatusIndex)i, data.Slice(i * EntrySize, EntrySize)));
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
            public Entry(StatusIndex index)
            {
                Index = index;
            }

            public Entry(StatusIndex index, IReadOnlyBinaryDataAccessor data)
            {
                Index = index;
                ApplyMessage = (TextIDHash)data.ReadInt32(0x00);
                RemoveMessage = (TextIDHash)data.ReadInt32(0x04);
                AlreadyAppliedMessage = (TextIDHash)data.ReadInt32(0x08);
                Byte0C = data.ReadByte(0x0C);
                Byte0D = data.ReadByte(0x0D);
                Byte0E = data.ReadByte(0x0E);
                Byte0F = data.ReadByte(0x0F);
                Short10 = data.ReadUInt16(0x10);
                Short12 = data.ReadUInt16(0x12);
                Short14 = data.ReadUInt16(0x14);
                Short16 = data.ReadUInt16(0x16);
                Short18 = data.ReadUInt16(0x18);
                Short1A = data.ReadUInt16(0x1A);
                MinDuration = data.ReadInt16(0x1C);
                MaxDuration = data.ReadInt16(0x1E);
                Short20 = data.ReadUInt16(0x20);
                Short22 = data.ReadUInt16(0x22);
                Short24 = data.ReadInt16(0x24);
                Short26 = data.ReadUInt16(0x26);
                Short28 = data.ReadUInt16(0x28);
                Short2A = data.ReadUInt16(0x2A);
                Short2C = data.ReadUInt16(0x2C);
                Short2E = data.ReadUInt16(0x2E);
            }

            public ReadOnlySpan<byte> ToBytes()
            {
                IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize]);
                data.WriteInt32(0x00, (int)ApplyMessage);
                data.WriteInt32(0x04, (int)RemoveMessage);
                data.WriteInt32(0x08, (int)AlreadyAppliedMessage);
                data.Write(0x0C, Byte0C);
                data.Write(0x0D, Byte0D);
                data.Write(0x0E, Byte0E);
                data.Write(0x0F, Byte0F);
                data.WriteUInt16(0x10, Short10);
                data.WriteUInt16(0x12, Short12);
                data.WriteUInt16(0x14, Short14);
                data.WriteUInt16(0x16, Short16);
                data.WriteUInt16(0x18, Short18);
                data.WriteUInt16(0x1A, Short1A);
                data.WriteInt16(0x1C, MinDuration);
                data.WriteInt16(0x1E, MaxDuration);
                data.WriteUInt16(0x20, Short20);
                data.WriteUInt16(0x22, Short22);
                data.WriteInt16(0x24, Short24);
                data.WriteUInt16(0x26, Short26);
                data.WriteUInt16(0x28, Short28);
                data.WriteUInt16(0x2A, Short2A);
                data.WriteUInt16(0x2C, Short2C);
                data.WriteUInt16(0x2E, Short2E);
                return data.ReadSpan();
            }

            public StatusIndex Index { get; }
            public TextIDHash ApplyMessage { get; set; }  // Message displayed when the status is newly applied
            public TextIDHash RemoveMessage { get; set; } // Message displayed when the status is removed
            public TextIDHash AlreadyAppliedMessage { get; set; } // Message displayed when trying to apply a status that was already applied
            public byte Byte0C { get; set; }
            public byte Byte0D { get; set; }
            public byte Byte0E { get; set; }  // 0 = neutral, 1 = good status, 2 = bad status, 4 = Substitute, 6 = Embargo
            public byte Byte0F { get; set; }  // always zero
            public ushort Short10 { get; set; }  // seems to be some kind of index?
            public ushort Short12 { get; set; }  // 12 and 14 seem to be related, might be graphics effects
            public ushort Short14 { get; set; }  // 12 and 14 seem to be related, might be graphics effects
            public ushort Short16 { get; set; }  // might be related to graphics effects
            public ushort Short18 { get; set; }
            public ushort Short1A { get; set; }
            public short MinDuration { get; set; }  // -1 = infinite
            public short MaxDuration { get; set; }  // -1 = infinite
            public ushort Short20 { get; set; }  // 20 and 22 seem to be related; seem to be used as # of turns between damage/heal ticks
            public ushort Short22 { get; set; }  // 20 and 22 seem to be related; seem to be used as # of turns between damage/heal ticks
            public short Short24 { get; set; }  // seems to be amount of damage/healing taken on every tick
            public ushort Short26 { get; set; }  // seems to be a percentage value for some effects
            public ushort Short28 { get; set; }
            public ushort Short2A { get; set; }
            public ushort Short2C { get; set; }
            public ushort Short2E { get; set; }  // always zero
        }
    }
}
