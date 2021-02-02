using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using SkyEditor.IO.Binary;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class PokemonFormDatabase
    {
        const int entrySize = 0x28;

        public PokemonFormDatabase(byte[] data)
        {
            var sir0 = new Sir0(data);
            var indexOffset = BitConverter.ToInt32(data, (int)sir0.SubHeaderOffset + 8);
            var entryCount = BitConverter.ToInt32(data, (int)sir0.SubHeaderOffset + 16);
            var entries = new List<PokemonFormDatabaseEntry>();
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add(new PokemonFormDatabaseEntry(data, indexOffset + (i * entrySize)));
            }
            this.Entries = entries;
        }

        public PokemonFormDatabase()
        {
            this.Entries = new List<PokemonFormDatabaseEntry>();
        }
        
        public List<PokemonFormDatabaseEntry> Entries { get; }

        /// <summary>
        /// Gets the 1-based index of the entry in <see cref="PokemonGraphicsDatabase"/> corresponding to the given Pokémon and form.
        /// </summary>
        public int GetGraphicsDatabaseIndex(CreatureIndex creatureIndex, PokemonFormType formType)
        {
            return Entries[(int)creatureIndex - 1].PokemonGraphicsDatabaseEntryIds[(int)formType];
        }
        
        public Sir0 ToSir0()
        {
            var sir0 = new Sir0Builder(8);
            
            var entriesSectionStart = sir0.Length;
            
            // Write the entries
            foreach (var entry in Entries)
            {
                sir0.Align(8);
                sir0.Write(sir0.Length, entry.ToByteArray());
            }

            // Write the content header
            sir0.SubHeaderOffset = sir0.Length;
            sir0.WriteString(sir0.Length, Encoding.ASCII, "PFDB");
            sir0.Align(8);
            sir0.WritePointer(sir0.Length, entriesSectionStart);
            sir0.WriteInt64(sir0.Length, Entries.Count);
            return sir0.Build();
        }
        
        public byte[] ToByteArray() => ToSir0().Data.ReadArray();

        public class PokemonFormDatabaseEntry
        {
            public const int ExpectedIdCount = 20;
            
            private short[] pokemonGraphicsDatabaseEntryIds = new short[ExpectedIdCount];

            public PokemonFormDatabaseEntry(byte[] data, int index)
            {
                for (int i = 0; i < ExpectedIdCount; i++)
                {
                    PokemonGraphicsDatabaseEntryIds[i] = BitConverter.ToInt16(data, index + (sizeof(short) * i));
                }
            }

            public PokemonFormDatabaseEntry()
            {
            }

            /// <summary>
            /// 1-based indexes of entries in pokemon_graphics_database.bin
            /// </summary>
            public short[] PokemonGraphicsDatabaseEntryIds
            {
                get => pokemonGraphicsDatabaseEntryIds;
                set
                {
                    if (value.Length != ExpectedIdCount)
                    {
                        throw new Exception($"{nameof(PokemonGraphicsDatabaseEntryIds)} must have exactly {ExpectedIdCount} elements.");
                    }
                    pokemonGraphicsDatabaseEntryIds = value;
                }
            }

            public byte[] ToByteArray()
            {
                var data = new byte[ExpectedIdCount * sizeof(short)];

                using var accessor = new BinaryFile(data);
                for (int i = 0; i < ExpectedIdCount; i++)
                {
                    accessor.WriteInt16(sizeof(short) * i, PokemonGraphicsDatabaseEntryIds[i]);
                }

                return data;
            }
        }
    }
}
