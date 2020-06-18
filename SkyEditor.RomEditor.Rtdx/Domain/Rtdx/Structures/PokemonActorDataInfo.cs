using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class PokemonActorDataInfo
    {
        public PokemonActorDataInfo(byte[] data)
        {
            const int entrySize = 0x58;
            var entries = new List<PokemonActorDataInfoEntry>();
            for (int i = 0; i < data.Length / entrySize; i++) 
            {
                entries.Add(new PokemonActorDataInfoEntry(data, i * entrySize));
            }
            this.Entries = entries;
        } 

        public IReadOnlyList<PokemonActorDataInfoEntry> Entries { get; }

        [DebuggerDisplay("{Name}")]
        public class PokemonActorDataInfoEntry
        {
            public PokemonActorDataInfoEntry(byte[] data, int index)
            {
                this.Name = Encoding.ASCII.GetString(data, index, 0x20).TrimEnd('\0');
            }

            public string Name { get; set; } = default!;
        }
    }
}
