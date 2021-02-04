using SkyEditor.IO.Binary;
using System.Collections.Generic;
using System.Text;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class Camp
    {
        // Credit to AntyMew
        private const int EntrySize = 0x114;

        public IDictionary<CampIndex, Entry> Entries { get; } = new Dictionary<CampIndex, Entry>();

        public Camp(byte[] data) : this(new BinaryFile(data))
        {
        }

        public Camp(IReadOnlyBinaryDataAccessor data)
        {
            var entryCount = checked((int)data.Length / EntrySize);
            for (int i = 0; i < entryCount; i++)
            {
                Entries.Add((CampIndex)i, new Entry(data.Slice(i * EntrySize, EntrySize)));
            }
        }

        public byte[] ToByteArray()
        {
            var data = new BinaryFile(new byte[Entries.Count * EntrySize]);
            for (int i = 0; i < Entries.Count; i++)
            {
                var entry = Entries[(CampIndex) i];
                entry.WriteTo(data, i);
            }
            return data.ReadArray();
        }

        public class Entry
        {
            public byte Byte00 { get; set; } // Always 1
            public byte Byte01 { get; set; } // 0 or 1
            public string Lineup { get; set; }
            public string UnlockCondition { get; set; }
            public short Short82 { get; set; } // 1 on some camps inhabited by Legendaries, otherwise 0
            public int Price { get; set; }
            public short Short86 { get; set; } // Always 0
            public uint UInt88 { get; set; } // Mostly 0xFFFFFFFF
            public int SortKey { get; set; }
            public int Int90 { get; set; }
            public string BackgroundTexture { get; set; }
            public string BackgroundMusic { get; set; }

            public Entry(IReadOnlyBinaryDataAccessor data)
            {
                Byte00 = data.ReadByte(0x0);
                Byte01 = data.ReadByte(0x1);
                Lineup = data.ReadString(0x2, 0x40, Encoding.ASCII).TrimEnd('\0');
                UnlockCondition = data.ReadString(0x42, 0x40, Encoding.ASCII).TrimEnd('\0');
                Short82 = data.ReadInt16(0x82);
                Price = data.ReadInt32(0x84);
                Short86 = data.ReadInt16(0x86);
                UInt88 = data.ReadUInt32(0x88);
                SortKey = data.ReadInt32(0x8C);
                Int90 = data.ReadInt32(0x90);
                BackgroundTexture = data.ReadString(0x94, 0x40, Encoding.ASCII).TrimEnd('\0');
                BackgroundMusic = data.ReadString(0xD4, 0x40, Encoding.ASCII).TrimEnd('\0');
            }

            public void WriteTo(IBinaryDataAccessor data, int index)
            {
                int offset = index * EntrySize;
                data.Write(offset + 0x0, Byte00);
                data.Write(offset + 0x1, Byte01);
                data.WriteString(offset + 0x2, Encoding.ASCII, Lineup);
                data.WriteString(offset + 0x42, Encoding.ASCII, UnlockCondition);
                data.WriteInt16(offset + 0x82, Short82);
                data.WriteInt32(offset + 0x84, Price);
                data.WriteInt16(offset + 0x86, Short86);
                data.WriteInt32(offset + 0x8C, SortKey);
                data.WriteInt32(offset + 0x90, Int90);
                data.WriteUInt32(offset + 0x88, UInt88);
                data.WriteString(offset + 0x94, Encoding.ASCII, BackgroundTexture);
                data.WriteString(offset + 0xD4, Encoding.ASCII, BackgroundMusic);
            }
        }
    }
}
