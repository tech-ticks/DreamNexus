using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.IO;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IItemArrange
    {
        public ItemArrange.Entry[] Entries { get; set; }

        public (byte[] bin, byte[] ent) Build();
    }

    public class ItemArrange : IItemArrange
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

        public ItemArrange()
        {
            Entries = new Entry[(int)DungeonIndex.END];
            for (int i = 0; i < (int)DungeonIndex.END; i++)
            {
                Entries[i] = new Entry();
            }
        }
        
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
            public const int ItemEntrySize = 8;

            public List<ItemSet> ItemSets { get; set; } = new List<ItemSet>();

            public Entry()
            {
            }

            public Entry(IReadOnlyBinaryDataAccessor accessor)
            {
                var buffer = Gyu0.Decompress(accessor);
                var sir0 = new Sir0(buffer);

                long itemSetCount = sir0.SubHeader.ReadInt64(0x0);
                
                for (long i = 0; i < itemSetCount; i++)
                {
                    long itemSetPointer = sir0.SubHeader.ReadInt64((i+1) * 8);
                    ushort[] itemKindWeights = new ushort[(int) ItemKind.MAX];
                    for (int j = 0; j < itemKindWeights.Length; j++)
                    {
                        itemKindWeights[j] = sir0.Data.ReadUInt16(itemSetPointer + j*2);
                    }

                    var itemWeights = new List<ItemWeightEntry>();
                    for (int j = 0;; j++)
                    {
                        long itemOffset = itemSetPointer + 0x24 + ItemEntrySize * j;
                        var weightEntry = new ItemWeightEntry
                        {
                            Index = (ItemIndex) sir0.Data.ReadUInt16(itemOffset + 0),
                            Weight = sir0.Data.ReadUInt16(itemOffset + 2),
                            Short04 = sir0.Data.ReadUInt16(itemOffset + 4),
                            Short06 = sir0.Data.ReadUInt16(itemOffset + 6),
                        };
                        
                        if ((ushort) weightEntry.Index == 0xFFFF) // Marks the end of the list
                        {
                            break;
                        }
                        
                        itemWeights.Add(weightEntry);
                    }
                    
                    ItemSets.Add(new ItemSet(itemKindWeights, itemWeights));
                }
            }

            public Sir0 ToSir0()
            {
                throw new NotImplementedException();
            }

            public class ItemSet
            {
                /// <summary>
                /// Item category weights indexed by ItemKind
                /// </summary>
                public ushort[] ItemKindWeights { get; set; }

                /// <summary>
                /// Weights of the individual items
                /// </summary>
                public List<ItemWeightEntry> ItemWeights { get; set; }

                public ItemSet(ushort[] itemKindWeights, List<ItemWeightEntry> itemWeights)
                {
                    ItemKindWeights = itemKindWeights;
                    ItemWeights = itemWeights;
                }
            }
            
            public class ItemWeightEntry
            {
                public ItemIndex Index { get; set; }
                public ushort Weight { get; set; }
                public ushort Short04 { get; set; } // Always zero
                public ushort Short06 { get; set; } // Always zero
            }
            
        }
    }
}
