using AssetStudio;
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
        
        DungeonDataInfo GetDungeonDataInfo();

        DungeonExtra GetDungeonExtra();
        #endregion

        #region StreamingAssets/native_data
        ICommonStrings GetCommonStrings();
        #endregion

        #region Models
        StarterCollection GetStarters();
        
        DungeonCollection GetDungeons();
        #endregion

        /// <summary>
        /// Saves all loaded files to disk
        /// </summary>
        void Save(string directory, IFileSystem fileSystem);

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
                mainExecutable = MainExecutable.LoadFromNso(fileSystem.ReadAllBytes(GetNsoPath(this.directory)));
            }
            return mainExecutable;
        }
        private IMainExecutable? mainExecutable;

        protected static string GetNsoPath(string directory) => Path.Combine(directory, "exefs", "main");
        #endregion

        #region StreamingAssets/data
        /// <summary>
        /// Gets the personality test settings, loading it if needed
        /// </summary>
        public NDConverterSharedData.DataStore GetNatureDiagnosis()
        {
            if (natureDiagnosis == null)
            {
                natureDiagnosis = JsonConvert.DeserializeObject<NDConverterSharedData.DataStore>(fileSystem.ReadAllText(GetNatureDiagnosisPath(this.directory)));
            }
            return natureDiagnosis;
        }
        private NDConverterSharedData.DataStore? natureDiagnosis;
        protected static string GetNatureDiagnosisPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/data/nature_diagnosis/diagnosis.json");
        #endregion

        #region StreamingAssets/data/ab
        /// <summary>
        /// Loads asset bundles
        /// Note: If the ROM is not a physical file system, exceptions may occur.
        /// </summary>
        public AssetsManager GetAssetBundles()
        {
            if (_assetBundles == null)
            {
                _assetBundles = new AssetsManager();
                _assetBundles.LoadFolder(this.fileSystem, GetAssetBundlesPath(this.directory));
            }
            return _assetBundles;
        }
        private AssetsManager? _assetBundles;
        protected static string GetAssetBundlesPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/ab");

        #endregion

        #region StreamingAssets/native_data/pokemon
        public PokemonDataInfo GetPokemonDataInfo()
        {
            if (pokemonDataInfo == null)
            {
                pokemonDataInfo = new PokemonDataInfo(new BinaryFile(fileSystem.ReadAllBytes(GetPokemonDataInfoPath(this.directory))));
            }
            return pokemonDataInfo;
        }
        private PokemonDataInfo? pokemonDataInfo;
        protected static string GetPokemonDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/pokemon/pokemon_data_info.bin");

        #endregion

        #region StreamingAssets/native_data/dungeon
        /// <summary>
        /// Gets static Pokemon data, loading it if needed
        /// </summary>
        public IFixedPokemon GetFixedPokemon()
        {
            if (fixedPokemon == null)
            {
                fixedPokemon = new FixedPokemon(fileSystem.ReadAllBytes(GetFixedPokemonPath(this.directory)));
            }
            return fixedPokemon;
        }
        private IFixedPokemon? fixedPokemon;

        protected static string GetFixedPokemonPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/fixed_pokemon.bin");
        
        public DungeonDataInfo GetDungeonDataInfo()
        {
            if (dungeonDataInfo == null)
            {
                dungeonDataInfo = new DungeonDataInfo(fileSystem.ReadAllBytes(GetDungeonDataInfoPath(this.directory)));
            }
            return dungeonDataInfo;
        }
        private DungeonDataInfo? dungeonDataInfo;
        protected static string GetDungeonDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/dungeon_data_info.bin");
        
        public DungeonExtra GetDungeonExtra()
        {
            if (dungeonExtra == null)
            {
                dungeonExtra = new DungeonExtra(fileSystem.ReadAllBytes(GetDungeonExtraPath(this.directory)));
            }
            return dungeonExtra;
        }
        private DungeonExtra? dungeonExtra;
		protected static string GetDungeonExtraPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/dungeon_extra.bin");
        #endregion

        #region StreamingAssets/native_data
        public PokemonFormDatabase GetPokemonFormDatabase()
        {
            if (pokemonFormDatabase == null)
            {
                pokemonFormDatabase = new PokemonFormDatabase(File.ReadAllBytes(GetPokemonFormDatabasePath(this.directory)));
            }
            return pokemonFormDatabase;
        }
        private PokemonFormDatabase? pokemonFormDatabase;
        protected static string GetPokemonFormDatabasePath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/pokemon_form_database.bin");

        public PokemonGraphicsDatabase GetPokemonGraphicsDatabase()
        {
            if (pokemonGraphicsDatabase == null)
            {
                pokemonGraphicsDatabase = new PokemonGraphicsDatabase(File.ReadAllBytes(GetPokemonGraphicsDatabasePath(this.directory)));
            }
            return pokemonGraphicsDatabase;
        }
        private PokemonGraphicsDatabase? pokemonGraphicsDatabase;
        protected static string GetPokemonGraphicsDatabasePath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/pokemon_graphics_database.bin");

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

        public DungeonCollection GetDungeons()
        {
            if (dungeonCollection == null)
            {
                dungeonCollection = new DungeonCollection(this);
            }
            return dungeonCollection;
        }
        private DungeonCollection? dungeonCollection;
        #endregion

        /// <summary>
        /// Saves all loaded files to disk
        /// </summary>
        public void Save(string directory, IFileSystem fileSystem)
        {
            // Save wrappers around files
            if (starterCollection != null)
            {
                starterCollection.Flush();
            }

            // Save the files themselves
            if (mainExecutable != null)
            {
                fileSystem.WriteAllBytes(GetNsoPath(directory), mainExecutable.ToNso());
            }
            if (natureDiagnosis != null)
            {
                fileSystem.WriteAllText(GetNatureDiagnosisPath(directory), JsonConvert.SerializeObject(natureDiagnosis));
            }
            if (fixedPokemon != null)
            {
                fileSystem.WriteAllBytes(GetFixedPokemonPath(directory), fixedPokemon.Build().Data.ReadArray());
            }

            // To-do: save pokemonDataInfo when implemented
            // To-do: save commonStrings when implemented
            // To-do: save messageBin when implemented
            // To-do: save pokemonFormDatabase when implemented
            // To-do: save dungeonBalance when implemented
            // To-do: save dungeonExtra when implemented

            if (pokemonGraphicsDatabase != null)
            {
                fileSystem.WriteAllBytes(GetPokemonGraphicsDatabasePath(directory), pokemonGraphicsDatabase.ToByteArray());
            }
        }

        /// <summary>
        /// Saves all loaded files to disk
        /// </summary>
        public void Save()
        {
            this.Save(this.directory, this.fileSystem);
        }
    }
}
