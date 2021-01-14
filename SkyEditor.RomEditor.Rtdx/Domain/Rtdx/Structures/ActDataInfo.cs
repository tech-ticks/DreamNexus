using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IActDataInfo
    {
        public IList<ActDataInfo.Entry> Entries { get; }

        public byte[] ToByteArray();
    }

    public class ActDataInfo : IActDataInfo
    {
        public const int EntrySize = 0xA0;

        public ActDataInfo()
        {
            this.Entries = new List<Entry>();
        }

        public ActDataInfo(byte[] data)
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
                Long00 = MemoryMarshal.Read<ulong>(data.Slice(0x00, sizeof(ulong)));
                Text08 = (TextIDHash)MemoryMarshal.Read<int>(data.Slice(0x08, sizeof(int)));
                Text0C = (TextIDHash)MemoryMarshal.Read<int>(data.Slice(0x0C, sizeof(int)));
                Short10 = MemoryMarshal.Read<ushort>(data.Slice(0x10, sizeof(ushort)));
                Short12 = MemoryMarshal.Read<ushort>(data.Slice(0x12, sizeof(ushort)));
                Short14 = MemoryMarshal.Read<ushort>(data.Slice(0x14, sizeof(ushort)));
                Short16 = MemoryMarshal.Read<ushort>(data.Slice(0x16, sizeof(ushort)));
                Short18 = MemoryMarshal.Read<ushort>(data.Slice(0x18, sizeof(ushort)));
                Short1A = MemoryMarshal.Read<ushort>(data.Slice(0x1A, sizeof(ushort)));
                Short1C = MemoryMarshal.Read<ushort>(data.Slice(0x1C, sizeof(ushort)));
                Short1E = MemoryMarshal.Read<ushort>(data.Slice(0x1E, sizeof(ushort)));
                Short20 = MemoryMarshal.Read<ushort>(data.Slice(0x20, sizeof(ushort)));
                Short22 = MemoryMarshal.Read<ushort>(data.Slice(0x22, sizeof(ushort)));
                Short24 = MemoryMarshal.Read<ushort>(data.Slice(0x24, sizeof(ushort)));
                Short26 = MemoryMarshal.Read<ushort>(data.Slice(0x26, sizeof(ushort)));
                Short28 = MemoryMarshal.Read<ushort>(data.Slice(0x28, sizeof(ushort)));
                Short2A = MemoryMarshal.Read<ushort>(data.Slice(0x2A, sizeof(ushort)));
                Short2C = MemoryMarshal.Read<ushort>(data.Slice(0x2C, sizeof(ushort)));
                Short2E = MemoryMarshal.Read<ushort>(data.Slice(0x2E, sizeof(ushort)));

                Short32 = MemoryMarshal.Read<ushort>(data.Slice(0x32, sizeof(ushort)));
                Short34 = MemoryMarshal.Read<ushort>(data.Slice(0x34, sizeof(ushort)));
                Short36 = MemoryMarshal.Read<ushort>(data.Slice(0x36, sizeof(ushort)));
                Short38 = MemoryMarshal.Read<ushort>(data.Slice(0x38, sizeof(ushort)));
                Short3A = MemoryMarshal.Read<ushort>(data.Slice(0x3A, sizeof(ushort)));
                
                Short40 = MemoryMarshal.Read<ushort>(data.Slice(0x40, sizeof(ushort)));
                Short42 = MemoryMarshal.Read<ushort>(data.Slice(0x42, sizeof(ushort)));
                
                Short48 = MemoryMarshal.Read<ushort>(data.Slice(0x48, sizeof(ushort)));
                Short4A = MemoryMarshal.Read<ushort>(data.Slice(0x4A, sizeof(ushort)));
                
                Short4E = MemoryMarshal.Read<ushort>(data.Slice(0x4E, sizeof(ushort)));
                Short50 = MemoryMarshal.Read<ushort>(data.Slice(0x50, sizeof(ushort)));
                Short52 = MemoryMarshal.Read<ushort>(data.Slice(0x52, sizeof(ushort)));
                Short54 = MemoryMarshal.Read<ushort>(data.Slice(0x54, sizeof(ushort)));
                
                Short58 = MemoryMarshal.Read<ushort>(data.Slice(0x58, sizeof(ushort)));
                Short5A = MemoryMarshal.Read<ushort>(data.Slice(0x5A, sizeof(ushort)));
                Byte5C = data[0x5C];
                Byte5D = data[0x5D];
                Byte5E = data[0x5E];
                Byte5F = data[0x5F];
                Byte60 = data[0x60];
                Byte61 = data[0x61];
                Byte62 = data[0x62];
                
                Byte64 = data[0x64];
                Byte65 = data[0x65];
                Byte66 = data[0x66];
                Byte67 = data[0x67];
                Byte68 = data[0x68];
                Byte69 = data[0x69];
                Byte6A = data[0x6A];
                Byte6B = data[0x6B];
                Byte6C = data[0x6C];
                Byte6D = data[0x6D];
                
                Byte70 = data[0x70];
                Byte71 = data[0x71];
                
                Byte74 = data[0x74];
                Byte75 = data[0x75];
                Byte76 = data[0x76];
                Byte77 = data[0x77];
                Byte78 = data[0x78];
                Byte79 = data[0x79];
                Byte7A = data[0x7A];
                
                Byte7C = data[0x7C];
                Byte7D = data[0x7D];
                Byte7E = data[0x7E];
                Byte7F = data[0x7F];
                Byte80 = data[0x80];
                Byte81 = data[0x81];
                Byte82 = data[0x82];
                Byte83 = data[0x83];
                Byte84 = data[0x84];
                Byte85 = data[0x85];
                Byte86 = data[0x86];
                Byte87 = data[0x87];
                Byte88 = data[0x88];
                Byte89 = data[0x89];
                Byte8A = data[0x8A];
                Byte8B = data[0x8B];
                Byte8C = data[0x8C];
                Byte8D = data[0x8D];
                Byte8E = data[0x8E];
                Byte8F = data[0x8F];
                Byte90 = data[0x90];
                Byte91 = data[0x91];
                Byte92 = data[0x92];
                ActHitCountIndex = data[0x93];
                Byte94 = data[0x94];
                Byte95 = data[0x95];
                Byte96 = data[0x96];
                Byte97 = data[0x97];
                Short98 = MemoryMarshal.Read<ushort>(data.Slice(0x98, sizeof(ushort)));
                Short9A = MemoryMarshal.Read<ushort>(data.Slice(0x9A, sizeof(ushort)));
                Byte9C = data[0x9C];
                Byte9D = data[0x9D];
            }

            public ReadOnlySpan<byte> ToBytes()
            {
                IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize]);
                data.WriteUInt64(0x00, Long00);
                data.WriteInt32(0x08, (int)Text08);
                data.WriteInt32(0x0C, (int)Text0C);
                data.WriteUInt16(0x10, Short10);
                data.WriteUInt16(0x12, Short12);
                data.WriteUInt16(0x14, Short14);
                data.WriteUInt16(0x16, Short16);
                data.WriteUInt16(0x18, Short18);
                data.WriteUInt16(0x1A, Short1A);
                data.WriteUInt16(0x1C, Short1C);
                data.WriteUInt16(0x1E, Short1E);
                data.WriteUInt16(0x20, Short20);
                data.WriteUInt16(0x22, Short22);
                data.WriteUInt16(0x24, Short24);
                data.WriteUInt16(0x26, Short26);
                data.WriteUInt16(0x28, Short28);
                data.WriteUInt16(0x2A, Short2A);
                data.WriteUInt16(0x2C, Short2C);
                data.WriteUInt16(0x2E, Short2E);

                data.WriteUInt16(0x32, Short32);
                data.WriteUInt16(0x34, Short34);
                data.WriteUInt16(0x36, Short36);
                data.WriteUInt16(0x38, Short38);
                data.WriteUInt16(0x3A, Short3A);
                
                data.WriteUInt16(0x40, Short40);
                data.WriteUInt16(0x42, Short42);
                
                data.WriteUInt16(0x48, Short48);
                data.WriteUInt16(0x4A, Short4A);
                
                data.WriteUInt16(0x4E, Short4E);
                data.WriteUInt16(0x50, Short50);
                data.WriteUInt16(0x52, Short52);
                data.WriteUInt16(0x54, Short54);
                
                data.WriteUInt16(0x58, Short58);
                data.WriteUInt16(0x5A, Short5A);
                data.Write(0x5C, Byte5C);
                data.Write(0x5D, Byte5D);
                data.Write(0x5E, Byte5E);
                data.Write(0x5F, Byte5F);
                data.Write(0x60, Byte60);
                data.Write(0x61, Byte61);
                data.Write(0x62, Byte62);
                
                data.Write(0x64, Byte64);
                data.Write(0x65, Byte65);
                data.Write(0x66, Byte66);
                data.Write(0x67, Byte67);
                data.Write(0x68, Byte68);
                data.Write(0x69, Byte69);
                data.Write(0x6A, Byte6A);
                data.Write(0x6B, Byte6B);
                data.Write(0x6C, Byte6C);
                data.Write(0x6D, Byte6D);
                
                data.Write(0x70, Byte70);
                data.Write(0x71, Byte71);
                
                data.Write(0x74, Byte74);
                data.Write(0x75, Byte75);
                data.Write(0x76, Byte76);
                data.Write(0x77, Byte77);
                data.Write(0x78, Byte78);
                data.Write(0x79, Byte79);
                data.Write(0x7A, Byte7A);
                
                data.Write(0x7C, Byte7C);
                data.Write(0x7D, Byte7D);
                data.Write(0x7E, Byte7E);
                data.Write(0x7F, Byte7F);
                data.Write(0x80, Byte80);
                data.Write(0x81, Byte81);
                data.Write(0x82, Byte82);
                data.Write(0x83, Byte83);
                data.Write(0x84, Byte84);
                data.Write(0x85, Byte85);
                data.Write(0x86, Byte86);
                data.Write(0x87, Byte87);
                data.Write(0x88, Byte88);
                data.Write(0x89, Byte89);
                data.Write(0x8A, Byte8A);
                data.Write(0x8B, Byte8B);
                data.Write(0x8C, Byte8C);
                data.Write(0x8D, Byte8D);
                data.Write(0x8E, Byte8E);
                data.Write(0x8F, Byte8F);
                data.Write(0x90, Byte90);
                data.Write(0x91, Byte91);
                data.Write(0x92, Byte92);
                data.Write(0x93, ActHitCountIndex);
                data.Write(0x94, Byte94);
                data.Write(0x95, Byte95);
                data.Write(0x96, Byte96);
                data.Write(0x97, Byte97);
                data.WriteUInt16(0x98, Short98);
                data.WriteUInt16(0x9A, Short9A);
                data.Write(0x9C, Byte9C);
                data.Write(0x9D, Byte9D);
                return data.ReadSpan();
            }

            public ulong Long00 { get; set; }  // Seems to contain a bunch of flags
            public TextIDHash Text08 { get; set; }
            public TextIDHash Text0C { get; set; }
            public ushort Short10 { get; set; }
            public ushort Short12 { get; set; }
            public ushort Short14 { get; set; }
            public ushort Short16 { get; set; }
            public ushort Short18 { get; set; }
            public ushort Short1A { get; set; }
            public ushort Short1C { get; set; }
            public ushort Short1E { get; set; }
            public ushort Short20 { get; set; }
            public ushort Short22 { get; set; }
            public ushort Short24 { get; set; }
            public ushort Short26 { get; set; }
            public ushort Short28 { get; set; }
            public ushort Short2A { get; set; }
            public ushort Short2C { get; set; }
            public ushort Short2E { get; set; }

            public ushort Short32 { get; set; }
            public ushort Short34 { get; set; }
            public ushort Short36 { get; set; }
            public ushort Short38 { get; set; }
            public ushort Short3A { get; set; }
            
            public ushort Short40 { get; set; }
            public ushort Short42 { get; set; }
            
            public ushort Short48 { get; set; }
            public ushort Short4A { get; set; }
            
            public ushort Short4E { get; set; }
            public ushort Short50 { get; set; }
            public ushort Short52 { get; set; }
            public ushort Short54 { get; set; }
            
            public ushort Short58 { get; set; }
            public ushort Short5A { get; set; }
            public byte Byte5C { get; set; }
            public byte Byte5D { get; set; }
            public byte Byte5E { get; set; }
            public byte Byte5F { get; set; }
            public byte Byte60 { get; set; }
            public byte Byte61 { get; set; }
            public byte Byte62 { get; set; }

            public byte Byte64 { get; set; }
            public byte Byte65 { get; set; }
            public byte Byte66 { get; set; }
            public byte Byte67 { get; set; }
            public byte Byte68 { get; set; }
            public byte Byte69 { get; set; }
            public byte Byte6A { get; set; }
            public byte Byte6B { get; set; }
            public byte Byte6C { get; set; }
            public byte Byte6D { get; set; }

            public byte Byte70 { get; set; }
            public byte Byte71 { get; set; }

            public byte Byte74 { get; set; }
            public byte Byte75 { get; set; }
            public byte Byte76 { get; set; }
            public byte Byte77 { get; set; }
            public byte Byte78 { get; set; }
            public byte Byte79 { get; set; }
            public byte Byte7A { get; set; }

            public byte Byte7C { get; set; }
            public byte Byte7D { get; set; }
            public byte Byte7E { get; set; }
            public byte Byte7F { get; set; }
            public byte Byte80 { get; set; }
            public byte Byte81 { get; set; }
            public byte Byte82 { get; set; }
            public byte Byte83 { get; set; }
            public byte Byte84 { get; set; }
            public byte Byte85 { get; set; }
            public byte Byte86 { get; set; }
            public byte Byte87 { get; set; }
            public byte Byte88 { get; set; }
            public byte Byte89 { get; set; }
            public byte Byte8A { get; set; }
            public byte Byte8B { get; set; }
            public byte Byte8C { get; set; }
            public byte Byte8D { get; set; }
            public byte Byte8E { get; set; }
            public byte Byte8F { get; set; }
            public byte Byte90 { get; set; }
            public byte Byte91 { get; set; }
            public byte Byte92 { get; set; }
            public byte ActHitCountIndex { get; set; }
            public byte Byte94 { get; set; }
            public byte Byte95 { get; set; }
            public byte Byte96 { get; set; }
            public byte Byte97 { get; set; }
            public ushort Short98 { get; set; }
            public ushort Short9A { get; set; }
            public byte Byte9C { get; set; }
            public byte Byte9D { get; set; }
        }
    }
}
