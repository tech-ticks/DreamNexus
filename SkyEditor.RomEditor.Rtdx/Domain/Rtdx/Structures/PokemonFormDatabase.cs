using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;

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
        
        public List<PokemonFormDatabaseEntry> Entries { get; }

        /// <summary>
        /// Gets the 1-based index of the entry in <see cref="PokemonGraphicsDatabase"/> corresponding to the given Pokémon and form.
        /// </summary>
        public int GetGraphicsDatabaseIndex(CreatureIndex creatureIndex, PokemonFormType formType)
        {
            return Entries[(int)creatureIndex - 1].PokemonGraphicsDatabaseEntryIds[(int)formType];
        }

        public class PokemonFormDatabaseEntry
        {
            public PokemonFormDatabaseEntry(byte[] data, int index)
            {
                PokemonGraphicsDatabaseEntryIds = new short[20];
                for (int i = 0; i < PokemonGraphicsDatabaseEntryIds.Length; i++)
                {
                    PokemonGraphicsDatabaseEntryIds[i] = BitConverter.ToInt16(data, index + (2 * i));
                }
            }

            /// <summary>
            /// 1-based indexes of entries in pokemon_graphics_database.bin
            /// </summary>
            public short[] PokemonGraphicsDatabaseEntryIds { get; set; }
        }
    }
}
