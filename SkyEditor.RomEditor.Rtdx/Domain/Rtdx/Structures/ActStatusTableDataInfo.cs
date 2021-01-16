using AssetStudio;
using SkyEditor.IO.Binary;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IActStatusTableDataInfo
    {
        IList<ActStatusTableDataInfo.Entry> Entries { get; }
        byte[] Build();
    }

    public class ActStatusTableDataInfo : IActStatusTableDataInfo
    {
        public IList<Entry> Entries { get; }

        public ActStatusTableDataInfo()
        {
            Entries = new List<Entry>();
        }

        public ActStatusTableDataInfo(byte[] data) : this(new BinaryFile(data))
        {
        }

        public ActStatusTableDataInfo(IReadOnlyBinaryDataAccessor data)
        {
            var count = data.Length / Entry.EntrySize;
            var entries = new List<Entry>();
            for (var i = 0; i < count; i++)
            {
                entries.Add(new Entry(i, data.Slice(i * Entry.EntrySize, data.Length)));
            }
            Entries = entries;
        }

        public byte[] Build()
        {
            MemoryStream bin = new MemoryStream();
            foreach (var entry in Entries)
            {
                var entryData = entry.ToByteArray();
                bin.Write(entryData, 0, entryData.Length);
            }
            return bin.ToArray();
        }

        public class Entry
        {
            public const int EntrySize = 0x0E;

            public int Index { get; }
            public short AttackMod { get; set; }
            public short SpecialAttackMod { get; set; }
            public short DefenseMod { get; set; }
            public short SpecialDefenseMod { get; set; }
            public short SpeedMod { get; set; }
            public short AccuracyMod { get; set; }
            public short EvasionMod { get; set; }

            public Entry(int index)
            {
                Index = index;
            }

            public Entry(int index, IReadOnlyBinaryDataAccessor data)
            {
                Index = index;
                AttackMod = data.ReadInt16(0x00);
                SpecialAttackMod = data.ReadInt16(0x02);
                DefenseMod = data.ReadInt16(0x04);
                SpecialDefenseMod = data.ReadInt16(0x06);
                SpeedMod = data.ReadInt16(0x08);
                AccuracyMod = data.ReadInt16(0x0A);
                EvasionMod = data.ReadInt16(0x0C);
            }

            public byte[] ToByteArray()
            {
                var data = new byte[EntrySize];
                BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan().Slice(0x0), AttackMod);
                BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan().Slice(0x2), SpecialAttackMod);
                BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan().Slice(0x4), DefenseMod);
                BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan().Slice(0x6), SpecialDefenseMod);
                BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan().Slice(0x8), SpeedMod);
                BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan().Slice(0xA), AccuracyMod);
                BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan().Slice(0xC), EvasionMod);
                return data;
            }
        }
    }
}
