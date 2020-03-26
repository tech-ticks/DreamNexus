using NsoElfConverterDotNet;
using SkyEditor.RomEditor.Rtdx.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx
{
    public class MainExecutable
    {
        public static MainExecutable LoadFromNso(string filename, INsoElfConverter? nsoElfConverter = null)
        {
            nsoElfConverter ??= NsoElfConverter.Instance;
            var data = nsoElfConverter.ConvertNsoToElf(filename);
            return new MainExecutable(data);
        }

        public MainExecutable(byte[] elfData)
        {
            if (elfData == null)
            {
                throw new ArgumentNullException(nameof(elfData));
            }
            if (elfData.Length <= (0x04BA3B0C + 0x90))
            {
                throw new ArgumentException("Data is not long enough to contain known sections", nameof(elfData));
            }

            this.Data = elfData ?? throw new ArgumentNullException(nameof(elfData));

            Init();
        }

        const int starterFixedPokemonMapOffset = 0x04BA3B0C;

        private void Init()
        {
            var starterFixedPokemonMaps = new List<StarterFixedPokemonMap>();
            for (int i = 0; i < 16; i++)
            {
                var offset = starterFixedPokemonMapOffset + (8 * i);
                starterFixedPokemonMaps.Add(new StarterFixedPokemonMap
                {
                    PokemonId = (Creature)BitConverter.ToInt32(Data, offset),
                    FixedPokemonId = (FixedCreature)BitConverter.ToInt32(Data, offset + 4)
                });
            }
            this.StarterFixedPokemonMaps = starterFixedPokemonMaps;
        }

        public byte[] ToElf()
        {
            for (int i = 0; i < 16; i++)
            {
                var map = StarterFixedPokemonMaps[i];
                var offset = starterFixedPokemonMapOffset + (8 * i);
                BitConverter.GetBytes((int)map.PokemonId).CopyTo(Data, offset);
                BitConverter.GetBytes((int)map.FixedPokemonId).CopyTo(Data, offset + 4);
            }
            return this.Data;
        }

        public byte[] ToNso(INsoElfConverter? nsoElfConverter = null)
        {
            nsoElfConverter ??= NsoElfConverter.Instance;
            return nsoElfConverter.ConvertElfToNso(ToElf());
        }

        private byte[] Data { get; }
        public IReadOnlyList<StarterFixedPokemonMap> StarterFixedPokemonMaps { get; private set; } = default!;

        [DebuggerDisplay("StarterFixedPokemonMap: {PokemonId} -> {FixedPokemonId}")]
        public class StarterFixedPokemonMap
        {
            public Creature PokemonId { get; set; }
            public FixedCreature FixedPokemonId { get; set; }
        }
    }
}
