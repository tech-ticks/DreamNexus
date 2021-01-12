using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.IO;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class ItemArrange
    {
        public ItemArrange(byte[] binData, byte[] entData)
        {
            IReadOnlyBinaryDataAccessor binFile = new BinaryFile(binData);
            IReadOnlyBinaryDataAccessor entFile = new BinaryFile(entData);

            var entCount = entFile.Length / sizeof(uint) - 1;
            Entries = new Entry[entCount];
            for (var i = 0; i < entCount; i++)
            {
                var curr = entFile.ReadInt32(i * sizeof(int));
                var next = entFile.ReadInt32((i + 1) * sizeof(int));
                Entries[i] = new Entry(binFile.Slice(curr, next - curr));
            }
        }

        /*public ItemArrange()
        {
            Entries = new Entry[(int)DungeonIndex.END];
            for (int i = 0; i < (int)DungeonIndex.END; i++)
            {
                Entries[i] = new Entry();
            }
        }*/

        public (byte[] bin, byte[] ent) Build()
        {
            MemoryStream bin = new MemoryStream();
            var entryPointers = new List<int>();

            // Build the .bin file data
            entryPointers.Add(0);
            foreach (var entry in Entries)
            {
                // Build SIR0 and compress to GYU0
                var sir0 = entry.ToSir0();
                var data = Gyu0.Compress(sir0.Data);

                // Write data to .bin and the pointer to .ent
                // Align data to 16 bytes
                var binData = data.ReadArray();
                bin.Write(binData, 0, binData.Length);
                var paddingLength = 16 - (bin.Length % 16);
                if (paddingLength != 16)
                {
                    bin.SetLength(bin.Length + paddingLength);
                    bin.Position = bin.Length;
                }
                entryPointers.Add((int)bin.Position);
            }

            // Build the .ent file data
            var ent = new byte[entryPointers.Count * sizeof(int)];
            for (int i = 0; i < entryPointers.Count; i++)
            {
                BitConverter.GetBytes(entryPointers[i]).CopyTo(ent, i * sizeof(int));
            }

            return (bin.ToArray(), ent);
        }

        public Entry[] Entries { get; set; }

        public class Entry
        {
            public Entry(IReadOnlyBinaryDataAccessor accessor)
            {
                var buffer = Gyu0.Decompress(accessor);
                Sir0 sir0 = new Sir0(buffer);

                Data = sir0.Data.ReadArray();

                // TODO: decode this
            }

            public Sir0 ToSir0()
            {
                throw new System.NotImplementedException();
                //var sir0 = new Sir0Builder(8);
                //return sir0.Build();
            }

            public byte[]? Data { get; set; }
        }
    }
}
