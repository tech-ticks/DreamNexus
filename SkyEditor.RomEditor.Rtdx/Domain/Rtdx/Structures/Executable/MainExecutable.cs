using NsoElfConverterDotNet;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using SkyEditor.IO.Binary;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures.Executable
{
    public interface IMainExecutable
    {
        IReadOnlyList<StarterFixedPokemonMap> StarterFixedPokemonMaps { get; }
        PegasusActDatabase ActorDatabase { get; }
        public ExecutableVersion Version { get; }
        public byte[] Data { get; set; }
        int GetPlaceName0HashForNameId(int nameId);
        int GetPlaceName1HashForNameId(int nameId);
        int GetPlaceName2HashForNameId(int nameId);
        int GetPlaceName3HashForNameId(int nameId);

        byte[] StartersToByteArray();
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
        }

        const int starterFixedPokemonMapOffsetOriginal = 0x04BA3B0C;
        const int starterFixedPokemonMapOffsetUpdate = 0x04BA4DBC;

        const int placeName0HashesOffsetOriginal = 0x4BA9390;
        const int placeName0HashesOffsetUpdate = 0x4BAA640;
        const int placeName1HashesOffsetOriginal = 0x4BA959C;
        const int placeName1HashesOffsetUpdate = 0x4BAA84C;
        const int placeName2HashesOffsetOriginal = 0x4BA97A8;
        const int placeName2HashesOffsetUpdate = 0x4BAAA58;
        const int placeName3HashesOffsetOriginal = 0x4BA99B4;
        const int placeName3HashesOffsetUpdate = 0x4BAAC64;

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

        // Converts a "Name ID" used for dungeons to a PLACE_NAME0_... string hash  
        public int GetPlaceName0HashForNameId(int nameId)
        {
            int baseOffset = Version == ExecutableVersion.Update1
                ? placeName0HashesOffsetUpdate : placeName0HashesOffsetOriginal;

            return BitConverter.ToInt32(Data, baseOffset + nameId * 4);
        }

        public int GetPlaceName1HashForNameId(int nameId)
        {
            int baseOffset = Version == ExecutableVersion.Update1
                ? placeName1HashesOffsetUpdate : placeName1HashesOffsetOriginal;

            return BitConverter.ToInt32(Data, baseOffset + nameId * 4);
        }

        public int GetPlaceName2HashForNameId(int nameId)
        {
            int baseOffset = Version == ExecutableVersion.Update1
                ? placeName2HashesOffsetUpdate : placeName2HashesOffsetOriginal;

            return BitConverter.ToInt32(Data, baseOffset + nameId * 4);
        }

        public int GetPlaceName3HashForNameId(int nameId)
        {
            int baseOffset = Version == ExecutableVersion.Update1
                ? placeName3HashesOffsetUpdate : placeName3HashesOffsetOriginal;

            return BitConverter.ToInt32(Data, baseOffset + nameId * 4);
        }

        public byte[] StartersToByteArray()
        {
            using var file = new BinaryFile(new MemoryStream());

            foreach (var starter in StarterFixedPokemonMaps)
            {
                file.WriteInt32(file.Length, (int) starter.PokemonId);
                file.WriteInt32(file.Length, (int) starter.FixedPokemonId);
            }
            
            return file.ReadArray();
        }

        public byte[] Data { get; set; }
        public ExecutableVersion Version { get; private set; }

        public IReadOnlyList<StarterFixedPokemonMap> StarterFixedPokemonMaps { get; private set; } = default!;

        private PegasusActDatabase? actorDatabase;
        public PegasusActDatabase ActorDatabase
        {
            get
            {
                if (actorDatabase == null)
                {
                    actorDatabase = new PegasusActDatabase(this);
                }
                return actorDatabase;
            }
        }
    }

    [DebuggerDisplay("StarterFixedPokemonMap: {PokemonId} -> {FixedPokemonId}")]
    public class StarterFixedPokemonMap
    {
        public CreatureIndex PokemonId { get; set; }
        public FixedCreatureIndex FixedPokemonId { get; set; }
    }
}
