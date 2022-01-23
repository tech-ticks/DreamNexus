using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SkyEditor.IO.Binary;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IActEffectDataInfo
    {
        public IList<ActEffectDataInfo.Entry> Entries { get; }

        public byte[] ToByteArray();
    }

    public class ActEffectDataInfo : IActEffectDataInfo
    {
        public const int EntrySize = 0x4C;

        public ActEffectDataInfo()
        {
            this.Entries = new List<Entry>();
        }

        public ActEffectDataInfo(byte[] data)
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
                Byte00 = data[0x00];
                Byte01 = data[0x01];
                Short02 = MemoryMarshal.Read<ushort>(data.Slice(0x02, sizeof(ushort)));
                Float04 = MemoryMarshal.Read<float>(data.Slice(0x04, sizeof(float)));
                Float08 = MemoryMarshal.Read<float>(data.Slice(0x08, sizeof(float)));
                Int0C = MemoryMarshal.Read<int>(data.Slice(0x0C, sizeof(int)));
                Short10 = MemoryMarshal.Read<ushort>(data.Slice(0x10, sizeof(ushort)));
                Short12 = MemoryMarshal.Read<ushort>(data.Slice(0x12, sizeof(ushort)));
                Short14 = MemoryMarshal.Read<ushort>(data.Slice(0x14, sizeof(ushort)));
                Short16 = MemoryMarshal.Read<ushort>(data.Slice(0x16, sizeof(ushort)));
                AllyInvokeGfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x18, sizeof(ushort)));
                EnemyInvokeGfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x1A, sizeof(ushort)));
                UserGfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x1C, sizeof(ushort)));
                Short1E = MemoryMarshal.Read<ushort>(data.Slice(0x1E, sizeof(ushort)));
                AreaGfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x20, sizeof(ushort)));
                ImpactGfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x22, sizeof(ushort)));
                ProjectileGfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x24, sizeof(ushort)));
                ProjectileImpactGfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x26, sizeof(ushort)));
                AllyInvokeSfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x28, sizeof(ushort)));
                EnemyInvokeSfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x2A, sizeof(ushort)));
                InitiateSfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x2C, sizeof(ushort)));
                ImpactSfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x2E, sizeof(ushort)));
                FireProjectileSfxSymbol = MemoryMarshal.Read<ushort>(data.Slice(0x30, sizeof(ushort)));
                Short32 = MemoryMarshal.Read<ushort>(data.Slice(0x32, sizeof(ushort)));
                Short34 = MemoryMarshal.Read<ushort>(data.Slice(0x34, sizeof(ushort)));
                Short36 = MemoryMarshal.Read<ushort>(data.Slice(0x36, sizeof(ushort)));
                Short38 = MemoryMarshal.Read<ushort>(data.Slice(0x38, sizeof(ushort)));

                Int3C = MemoryMarshal.Read<int>(data.Slice(0x3C, sizeof(int)));
            }

            public ReadOnlySpan<byte> ToBytes()
            {
                IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize]);
                data.Write(0x00, Byte00);
                data.Write(0x01, Byte01);
                data.WriteUInt16(0x02, Short02);
                data.WriteSingle(0x04, Float04);
                data.WriteSingle(0x08, Float08);
                data.WriteInt32(0x0C, Int0C);
                data.WriteUInt16(0x10, Short10);
                data.WriteUInt16(0x12, Short12);
                data.WriteUInt16(0x14, Short14);
                data.WriteUInt16(0x16, Short16);
                data.WriteUInt16(0x18, AllyInvokeGfxSymbol);
                data.WriteUInt16(0x1A, EnemyInvokeGfxSymbol);
                data.WriteUInt16(0x1C, UserGfxSymbol);
                data.WriteUInt16(0x1E, Short1E);
                data.WriteUInt16(0x20, AreaGfxSymbol);
                data.WriteUInt16(0x22, ImpactGfxSymbol);
                data.WriteUInt16(0x24, ProjectileGfxSymbol);
                data.WriteUInt16(0x26, ProjectileImpactGfxSymbol);
                data.WriteUInt16(0x28, AllyInvokeSfxSymbol);
                data.WriteUInt16(0x2A, EnemyInvokeSfxSymbol);
                data.WriteUInt16(0x2C, InitiateSfxSymbol);
                data.WriteUInt16(0x2E, ImpactSfxSymbol);
                data.WriteUInt16(0x30, FireProjectileSfxSymbol);
                data.WriteUInt16(0x32, Short32);
                data.WriteUInt16(0x34, Short34);
                data.WriteUInt16(0x36, Short36);
                data.WriteUInt16(0x38, Short38);

                data.WriteInt32(0x3C, Int3C);
                return data.ReadSpan();
            }

            public byte Byte00 { get; set; }
            public byte Byte01 { get; set; }
            public ushort Short02 { get; set; }
            public float Float04 { get; set; }
            public float Float08 { get; set; }
            public int Int0C { get; set; }
            public ushort Short10 { get; set; }
            public ushort Short12 { get; set; }
            public ushort Short14 { get; set; }
            public ushort Short16 { get; set; }

            public ushort AllyInvokeGfxSymbol { get; set; }
            public ushort EnemyInvokeGfxSymbol { get; set; }
            public ushort UserGfxSymbol { get; set; }
            public ushort Short1E { get; set; }
            public ushort AreaGfxSymbol { get; set; }
            public ushort ImpactGfxSymbol { get; set; }
            public ushort ProjectileGfxSymbol { get; set; }
            public ushort ProjectileImpactGfxSymbol { get; set; }
            public ushort AllyInvokeSfxSymbol { get; set; }
            public ushort EnemyInvokeSfxSymbol { get; set; }
            public ushort InitiateSfxSymbol { get; set; }
            public ushort ImpactSfxSymbol { get; set; }
            public ushort FireProjectileSfxSymbol { get; set; }
            public ushort Short32 { get; set; }
            public ushort Short34 { get; set; }
            public ushort Short36 { get; set; }
            public ushort Short38 { get; set; }
            public int Int3C { get; set; }
        }
    }
}
