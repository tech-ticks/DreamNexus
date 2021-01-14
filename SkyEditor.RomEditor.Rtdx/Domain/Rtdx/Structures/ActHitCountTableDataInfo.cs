using AssetStudio;
using SkyEditor.IO.Binary;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IActHitCountTableDataInfo
    {
        IList<ActHitCountTableDataInfo.Entry> Entries { get; }
        byte[] Build();
    }

    public class ActHitCountTableDataInfo : IActHitCountTableDataInfo
    {
        public IList<Entry> Entries { get; }

        public ActHitCountTableDataInfo()
        {
            Entries = new List<Entry>();
        }

        public ActHitCountTableDataInfo(byte[] data) : this(new BinaryFile(data))
        {
        }

        public ActHitCountTableDataInfo(IReadOnlyBinaryDataAccessor data)
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
            public const int EntrySize = 0x10;

            public int Index { get; }
            public byte StopOnMiss { get; set; } // Rollout, Ice Ball
            public byte MinHits { get; set; }
            public byte MaxHits { get; set; }
            public short[] Weights = new short[4];

            public Entry(int index)
            {
                Index = index;
            }

            public Entry(int index, IReadOnlyBinaryDataAccessor data)
            {
                Index = index;
                StopOnMiss = data.ReadByte(0x0);
                MinHits = data.ReadByte(0x1);
                MaxHits = data.ReadByte(0x2);
                for (var i = 0; i < 4; i++)
                {
                    Weights[i] = data.ReadInt16(0x4 + i * 2);
                }
            }

            public byte[] ToByteArray()
            {
                var data = new byte[EntrySize];
                data[0] = StopOnMiss;
                data[1] = MinHits;
                data[2] = MaxHits;
                for (var i = 0; i < 4; i++)
                {
                    BinaryPrimitives.WriteInt16LittleEndian(data.AsSpan().Slice(0x4 + i * 2), Weights[i]);
                }
                return data;
            }
        }
    }
}
