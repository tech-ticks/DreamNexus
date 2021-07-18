using System.IO;
using System.Text;
using System.Collections.Generic;
using SkyEditor.IO.Binary;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class ItemGraphics
    {
        public Dictionary<int, Entry> Entries { get; } = new Dictionary<int, Entry>();

        public ItemGraphics()
        {
        }

        public ItemGraphics(byte[] data)
        {
            var accessor = new BinaryFile(data);
            accessor.Position = 0;
            int entryCount = accessor.ReadNextInt32();

            for (int i = 0; i < entryCount; i++)
            {
                var entry = new Entry(accessor);
                Entries.Add(entry.Key, entry);
            }
        }

        public byte[] ToByteArray()
        {
            var writer = new BinaryFile(new MemoryStream());
            writer.WriteInt32(writer.Length, Entries.Count);
            foreach (var entry in Entries.Values)
            {
                entry.WriteTo(writer);
            }
            return writer.ReadArray();
        }
        
        public class Entry
        {
            // Name of the model prefab in item.ab or trap.ab (with prefix "item_" or "trap_")
            public string ModelName { get; set; } = "";
            public byte Byte00 { get; set; }
            public byte Byte01 { get; set; }
            public byte Byte02 { get; set; }
            public byte Byte03 { get; set; }

            // Number of units added to the Y coordinate of a character model when standing on top of the item
            public float Height { get; set; }
            public int Key { get; set; } // Used as the index in ItemDataInfo
            public byte Byte0C { get; set; }
            public byte Byte0D { get; set; }
            public byte Byte0E { get; set; }
            public short Short0F { get; set; }

            public Entry()
            {
            }

            public Entry(IReadOnlyBinaryDataAccessor data)
            {
                // String length is duplicated
                data.ReadNextInt32();
                ModelName = data.ReadNextString(data.ReadNextInt32(), Encoding.ASCII);
                Byte00 = data.ReadNextByte();
                Byte01 = data.ReadNextByte();
                Byte02 = data.ReadNextByte();
                Byte03 = data.ReadNextByte();
                Height = data.ReadNextSingle();
                Key = data.ReadNextInt32();
                Byte0C = data.ReadNextByte();
                Byte0D = data.ReadNextByte();
                Byte0E = data.ReadNextByte();
                Short0F = data.ReadNextInt16();
            }

            public void WriteTo(BinaryFile data)
            {
                data.WriteInt32(data.Length, ModelName.Length);
                data.WriteInt32(data.Length, ModelName.Length);
                data.WriteString(data.Length, Encoding.ASCII, ModelName);
                data.Write(data.Length, Byte00);
                data.Write(data.Length, Byte01);
                data.Write(data.Length, Byte02);
                data.Write(data.Length, Byte03);
                data.WriteSingle(data.Length, Height);
                data.WriteInt32(data.Length, Key);
                data.Write(data.Length, Byte0C);
                data.Write(data.Length, Byte0D);
                data.Write(data.Length, Byte0E);
                data.WriteInt16(data.Length, Short0F);
            }
        }
    }
}
