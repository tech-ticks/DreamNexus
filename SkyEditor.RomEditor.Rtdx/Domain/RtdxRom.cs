using Newtonsoft.Json;
using SkyEditor.IO.Binary;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Rtdx.Domain.Models;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Reverse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain
{
    public interface IRtdxRom
    {
        #region Exefs
        /// <summary>
        /// Gets the main executable, loading it if needed
        /// </summary>
        IMainExecutable GetMainExecutable();
        #endregion

        #region StreamingAssets/data
        /// <summary>
        /// Gets the personality test settings, loading it if needed
        /// </summary>
        NDConverterSharedData.DataStore GetNatureDiagnosis();
        #endregion

        #region StreamingAssets/native_data/pokemon
        PokemonDataInfo GetPokemonDataInfo();
        #endregion

        #region StreamingAssets/native_data/dungeon
        /// <summary>
        /// Gets static Pokemon data, loading it if needed
        /// </summary>
        IFixedPokemon GetFixedPokemon();
        #endregion

        #region StreamingAssets/native_data
        ICommonStrings GetCommonStrings();
        #endregion

        #region Models
        StarterCollection GetStarters();
        #endregion

        /// <summary>
        /// Saves all loaded files to disk
        /// </summary>
        void Save();
    }

    public class RtdxRom : IRtdxRom
    {
        public RtdxRom(string directory, IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentException("Directory must not be null or empty", nameof(directory));
            }
            if (!fileSystem.DirectoryExists(directory))
            {
                throw new DirectoryNotFoundException("Directory must exist in the given file system");
            }
            this.directory = directory;
        }

        protected readonly string directory;
        protected readonly IFileSystem fileSystem;

        #region ExeFs
        /// <summary>
        /// Gets the main executable, loading it if needed
        /// </summary>
        public IMainExecutable GetMainExecutable()
        {
            if (mainExecutable == null)
            {
                mainExecutable = MainExecutable.LoadFromNso(fileSystem.ReadAllBytes(NsoPath));
            }
            return mainExecutable;
        }
        private IMainExecutable? mainExecutable;
        protected string NsoPath => Path.Combine(directory, "exefs", "main");
        #endregion

        #region StreamingAssets/data
        /// <summary>
        /// Gets the personality test settings, loading it if needed
        /// </summary>
        public NDConverterSharedData.DataStore GetNatureDiagnosis()
        {
            if (natureDiagnosis == null)
            {
                natureDiagnosis = JsonConvert.DeserializeObject<NDConverterSharedData.DataStore>(fileSystem.ReadAllText(NatureDiagnosisPath));
            }
            return natureDiagnosis;
        }
        private NDConverterSharedData.DataStore? natureDiagnosis;
        protected string NatureDiagnosisPath => Path.Combine(directory, "romfs/Data/StreamingAssets/data/nature_diagnosis/diagnosis.json");
        #endregion

        #region StreamingAssets/native_data/pokemon
        public PokemonDataInfo GetPokemonDataInfo()
        {
            if (pokemonDataInfo == null)
            {
                pokemonDataInfo = new PokemonDataInfo(new BinaryFile(fileSystem.ReadAllBytes(PokemonDataInfoPath)));
            }
            return pokemonDataInfo;
        }
        private PokemonDataInfo? pokemonDataInfo;
        protected string PokemonDataInfoPath => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/pokemon/pokemon_data_info.bin");

        #endregion

        #region StreamingAssets/native_data/dungeon
        /// <summary>
        /// Gets static Pokemon data, loading it if needed
        /// </summary>
        public IFixedPokemon GetFixedPokemon()
        {
            if (fixedPokemon == null)
            {
                fixedPokemon = new FixedPokemon(fileSystem.ReadAllBytes(FixedPokemonPath));
            }
            return fixedPokemon;
        }
        private IFixedPokemon? fixedPokemon;
        protected string FixedPokemonPath => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/fixed_pokemon.bin");
        #endregion

        #region StreamingAssets/native_data
        public PokemonGraphicsDatabase GetPokemonGraphicsDatabase()
        {
            if (pokemonGraphicsDatabase == null)
            {
                pokemonGraphicsDatabase = new PokemonGraphicsDatabase(File.ReadAllBytes(PokemonGraphicsDatabasePath));
            }
            return pokemonGraphicsDatabase;
        }
        private PokemonGraphicsDatabase? pokemonGraphicsDatabase;
        protected string PokemonGraphicsDatabasePath => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/pokemon_graphics_database.bin");


        public Farc GetUSMessageBin()
        {
            if (messageBin == null)
            {
                var messageBinPath = MessageBinUSPath;
                messageBin = new Farc(File.ReadAllBytes(messageBinPath));
            }
            return messageBin;
        }
        private Farc? messageBin;
        protected string MessageBinUSPath => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/message_us.bin");

        public ICommonStrings GetCommonStrings()
        {
            if (commonStrings == null)
            {
                var commonData = GetUSMessageBin().GetFile("common.bin");
                if (commonData == null)
                {
                    throw new Exception("Unable to load common.bin from " + MessageBinUSPath);
                }

                var common = new MessageBinEntry(commonData);
                commonStrings = new CommonStrings(common);
            }
            return commonStrings;
        }
        private ICommonStrings? commonStrings;
        #endregion

        #region Models
        public StarterCollection GetStarters()
        {
            if (starterCollection == null)
            {
                starterCollection = new StarterCollection(this);
            }
            return starterCollection;
        }
        private StarterCollection? starterCollection;
        #endregion

        /// <summary>
        /// Saves all loaded files to disk
        /// </summary>
        public void Save()
        {
            if (starterCollection != null)
            {
                starterCollection.Flush();
            }
            if (mainExecutable != null)
            {
                fileSystem.WriteAllBytes(NsoPath, mainExecutable.ToNso());
            }
            if (natureDiagnosis != null)
            {
                fileSystem.WriteAllText(NatureDiagnosisPath, JsonConvert.SerializeObject(natureDiagnosis));
            }
            if (fixedPokemon != null)
            {
                fileSystem.WriteAllBytes(FixedPokemonPath, fixedPokemon.Build().Data.ReadArray());
            }
            // To-do: save pokemonDataInfo
            // To-do: save commonStrings
            // To-do: save messageBin
        }
    }
}
