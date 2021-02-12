using System.IO;
using System.Text;
using System.Collections.Generic;
using SkyEditor.IO.Binary;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class MapRandomGraphics
    {
        public List<Entry> Entries { get; } = new List<Entry>();

        public MapRandomGraphics()
        {
        }

        public MapRandomGraphics(byte[] data)
        {
            var accessor = new BinaryFile(data);
            accessor.Position = 0;
            int entryCount = accessor.ReadNextInt32();
            Entries.Capacity = entryCount;

            for (int i = 0; i < entryCount; i++)
            {
                Entries.Add(new Entry(accessor));
            }
        }

        public byte[] ToByteArray()
        {
            var writer = new BinaryFile(new MemoryStream());
            writer.WriteInt32(writer.Length, Entries.Count);
            foreach (var entry in Entries)
            {
                entry.WriteTo(writer);
            }
            return writer.ReadArray();
        }
        
        public class Entry
        {
            public string Symbol { get; set; } = "";
            public string AssetBundleName { get; set; } = "";
            public string ExtraFileName { get; set; } = ""; // Name of the .ibrfbin, .nbbin and .ubbin files, empty for static models
            public int Unk1 { get; set; } // Could be a hash or float?
            public byte Unk2 { get; set; } // Range from 0 to 3. Zero for maps that use a static model instead of a tileset.
            public byte Unk3 { get; set; } // Always 1

            public Entry()
            {
            }

            public Entry(IReadOnlyBinaryDataAccessor data)
            {
                data.ReadNextInt32(); // String length is duplicated
                Symbol = data.ReadNextString(data.ReadNextInt32(), Encoding.ASCII);
                data.ReadNextInt32();
                AssetBundleName = data.ReadNextString(data.ReadNextInt32(), Encoding.ASCII);
                data.ReadNextInt32();
                ExtraFileName = data.ReadNextString(data.ReadNextInt32(), Encoding.ASCII);

                data.Position += 0x8;
                Unk1 = data.ReadNextInt32();

                data.Position += 0x40;
                Unk2 = data.ReadNextByte();

                data.Position += 0x8;
                Unk3 = data.ReadNextByte();
            }

            public void WriteTo(BinaryFile data)
            {
                void WriteString(string str)
                {
                    // The length is always duplicated
                    data.WriteInt32(data.Length, str.Length);
                    data.WriteInt32(data.Length, str.Length);
                    data.WriteString(data.Length, Encoding.ASCII, str);
                }

                WriteString(Symbol);
                WriteString(AssetBundleName);
                WriteString(ExtraFileName);

                data.SetLength(data.Length + 0x8);
                data.WriteInt32(data.Length, Unk1);

                data.SetLength(data.Length + 0x40);
                data.Write(data.Length, Unk2);

                data.SetLength(data.Length + 0x8);
                data.Write(data.Length, Unk3);
            }
        }
    }
}
