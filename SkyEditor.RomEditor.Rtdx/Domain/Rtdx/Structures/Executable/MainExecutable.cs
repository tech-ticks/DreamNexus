using NsoElfConverterDotNet;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures.Executable
{
    public interface IMainExecutable
    {
        IReadOnlyList<StarterFixedPokemonMap> StarterFixedPokemonMaps { get; }
        PegasusActDatabase ActorDatabase { get; }
        public ExecutableVersion Version { get; }
        public byte[] Data { get; set; }

        byte[] ToElf();
        byte[] ToNso(INsoElfConverter? nsoElfConverter = null);
    }

    public class MainExecutable : IMainExecutable
    {
        public static MainExecutable LoadFromNso(byte[] file, INsoElfConverter? nsoElfConverter = null)
        {
            nsoElfConverter ??= NsoElfConverter.Instance;
            var data = nsoElfConverter.ConvertNsoToElf(file);
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
            
            ActDatabaseLazy = new Lazy<PegasusActDatabase>(() => new PegasusActDatabase(elfData, Version));
        }

        const int starterFixedPokemonMapOffsetOriginal = 0x04BA3B0C;
        const int starterFixedPokemonMapOffsetUpdate = 0x04BA4DBC;

        private void Init()
        {
            Init(starterFixedPokemonMapOffsetOriginal);
            if (StarterFixedPokemonMaps.All(m => m.PokemonId == CreatureIndex.NONE))
            {
                Init(starterFixedPokemonMapOffsetUpdate);
                Version = ExecutableVersion.Update1;
            }
            else
            {
                Version = ExecutableVersion.Original;
            }
        }

        private void Init(int starterFixedPokemonMapOffset)
        {
            var starterFixedPokemonMaps = new List<StarterFixedPokemonMap>();
            for (int i = 0; i < 16; i++)
            {
                var offset = starterFixedPokemonMapOffset + (8 * i);
                starterFixedPokemonMaps.Add(new StarterFixedPokemonMap
                {
                    PokemonId = (CreatureIndex)BitConverter.ToInt32(Data, offset),
                    FixedPokemonId = (FixedCreatureIndex)BitConverter.ToInt32(Data, offset + 4)
                });
            }
            this.StarterFixedPokemonMaps = starterFixedPokemonMaps;
        }

        public byte[] ToElf()
        {
            if (ActDatabaseLazy.IsValueCreated) // No need to write if we haven't even accessed it
            {
                ActorDatabase.Write();
            }

            int starterFixedPokemonMapOffset = Version switch
            {
                ExecutableVersion.Original => starterFixedPokemonMapOffsetOriginal,
                ExecutableVersion.Update1 => starterFixedPokemonMapOffsetUpdate,
                _ => throw new InvalidOperationException("Unsupported executable version")
            };

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
        
        public byte[] Data { get; set; }
        public ExecutableVersion Version { get; private set; }

        public IReadOnlyList<StarterFixedPokemonMap> StarterFixedPokemonMaps { get; private set; } = default!;
        
        private Lazy<PegasusActDatabase> ActDatabaseLazy { get; }
        public PegasusActDatabase ActorDatabase => ActDatabaseLazy.Value;
    }

    [DebuggerDisplay("StarterFixedPokemonMap: {PokemonId} -> {FixedPokemonId}")]
    public class StarterFixedPokemonMap
    {
        public CreatureIndex PokemonId { get; set; }
        public FixedCreatureIndex FixedPokemonId { get; set; }
    }
}
