using AssetStudio;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SkyEditor.IO.Binary;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Infrastructure.Automation.CSharp;
using SkyEditor.RomEditor.Infrastructure.Automation.Lua;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Structures.Executable;
using SkyEditor.RomEditor.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SkyEditor.RomEditor.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Domain.Rtdx
{
    public interface IRtdxRom : IModTarget, ISaveable, ISaveableToDirectory, ICSharpChangeScriptGenerator, ILuaChangeScriptGenerator
    {
        string RomDirectory { get; }

        #region Exefs
        /// <summary>
        /// Gets the main executable, loading it if needed
        /// </summary>
        IMainExecutable GetMainExecutable();
        #endregion

        #region StreamingAssets/data/ab

        AssetsManager GetAssetBundles();
        string[] ListAssetBundles();

        #endregion

        #region StreamingAssets/data
        /// <summary>
        /// Gets the personality test settings, loading it if needed
        /// </summary>
        NatureDiagnosisConfiguration GetNatureDiagnosis();
        #endregion

        #region StreamingAssets/native_data/pokemon
        PokemonDataInfo GetPokemonDataInfo();
        #endregion

        #region StreamingAssets/native_data/dungeon
        /// <summary>
        /// Gets static Pokemon data, loading it if needed
        /// </summary>
        IFixedPokemon GetFixedPokemon();
        
        IDungeonDataInfo GetDungeonDataInfo();

        IDungeonExtra GetDungeonExtra();
        
        IDungeonBalance GetDungeonBalance();
        #endregion

        #region StreamingAssets/native_data
        ICommonStrings GetCommonStrings();
        PokemonGraphicsDatabase GetPokemonGraphicsDatabase();
        Farc GetUSMessageBin();
        PokemonFormDatabase GetPokemonFormDatabase();
        Sir0StringList GetDungeonMapSymbol();
        Sir0StringList GetDungeonBgmSymbol();
        CodeTable GetCodeTable();
        #endregion

        #region Models
        IStarterCollection GetStarters();
        
        IDungeonCollection GetDungeons();
        #endregion

        #region Helpers
        PokemonGraphicsDatabase.PokemonGraphicsDatabaseEntry? FindGraphicsDatabaseEntryByCreature(CreatureIndex creatureIndex, PokemonFormType formIndex);
        #endregion

        void WriteFile(string relativePath, byte[] data);
    }

    public class RtdxRom : IRtdxRom
    {
        public RtdxRom(string directory, IFileSystem fileSystem)
        {
            this.FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentException("Directory must not be null or empty", nameof(directory));
            }
            if (!fileSystem.DirectoryExists(directory))
            {
                throw new DirectoryNotFoundException("Directory must exist in the given file system");
            }
            this.RomDirectory = directory;
            filesToWrite = new List<(string relativePath, byte[] data)>();
        }

        private readonly List<(string relativePath, byte[] data)> filesToWrite;

        public string RomDirectory { get; }
        protected IFileSystem FileSystem { get; }

        protected IServiceProvider GetServiceProvider()
        {
            if (serviceProvider == null)
            {
                serviceProvider = ServiceProviderBuilder.CreateRtdxRomServiceProvider(this);
            }
            return serviceProvider;
        }
        private IServiceProvider? serviceProvider;

        #region ExeFs
        /// <summary>
        /// Gets the main executable, loading it if needed
        /// </summary>
        public IMainExecutable GetMainExecutable()
        {
            if (mainExecutable == null)
            {
                mainExecutable = MainExecutable.LoadFromNso(FileSystem.ReadAllBytes(GetNsoPath(this.RomDirectory)));
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
        public NatureDiagnosisConfiguration GetNatureDiagnosis()
        {
            if (natureDiagnosis == null)
            {
                natureDiagnosis = JsonConvert.DeserializeObject<NatureDiagnosisConfiguration>(FileSystem.ReadAllText(GetNatureDiagnosisPath(this.RomDirectory)));
            }
            return natureDiagnosis;
        }
        private NatureDiagnosisConfiguration? natureDiagnosis;
        protected static string GetNatureDiagnosisPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/data/nature_diagnosis/diagnosis.json");
        #endregion

        #region StreamingAssets/data/ab
        protected static string GetAssetBundlesPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/ab");

        /// <summary>
        /// Loads asset bundles
        /// Note: If the ROM is not a physical file system, exceptions may occur.
        /// </summary>
        public AssetsManager GetAssetBundles()
        {
            if (_assetBundles == null)
            {
                _assetBundles = new AssetsManager();
                _assetBundles.LoadFolder(this.FileSystem, GetAssetBundlesPath(this.RomDirectory));
            }
            return _assetBundles;
        }
        private AssetsManager? _assetBundles;

        public string[] ListAssetBundles()
        {
            return this.FileSystem.GetFiles(GetAssetBundlesPath(this.RomDirectory), "*.ab", true);
        }

        #endregion

        #region StreamingAssets/native_data/pokemon
        public PokemonDataInfo GetPokemonDataInfo()
        {
            if (pokemonDataInfo == null)
            {
                pokemonDataInfo = new PokemonDataInfo(new BinaryFile(FileSystem.ReadAllBytes(GetPokemonDataInfoPath(this.RomDirectory))));
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
                fixedPokemon = new FixedPokemon(FileSystem.ReadAllBytes(GetFixedPokemonPath(this.RomDirectory)));
            }
            return fixedPokemon;
        }
        private IFixedPokemon? fixedPokemon;

        protected static string GetFixedPokemonPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/fixed_pokemon.bin");
        
        public IDungeonDataInfo GetDungeonDataInfo()
        {
            if (dungeonDataInfo == null)
            {
                dungeonDataInfo = new DungeonDataInfo(FileSystem.ReadAllBytes(GetDungeonDataInfoPath(this.RomDirectory)));
            }
            return dungeonDataInfo;
        }
        private IDungeonDataInfo? dungeonDataInfo;
        protected static string GetDungeonDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/dungeon_data_info.bin");
        
        public IDungeonExtra GetDungeonExtra()
        {
            if (dungeonExtra == null)
            {
                dungeonExtra = new DungeonExtra(FileSystem.ReadAllBytes(GetDungeonExtraPath(this.RomDirectory)));
            }
            return dungeonExtra;
        }
        private IDungeonExtra? dungeonExtra;
		protected static string GetDungeonExtraPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/dungeon_extra.bin");
        
        public IDungeonBalance GetDungeonBalance()
        {
            if (dungeonBalance == null)
            {
                dungeonBalance = new DungeonBalance(FileSystem.ReadAllBytes(GetDungeonBalancePath(this.RomDirectory) + ".bin"), FileSystem.ReadAllBytes(GetDungeonBalancePath(this.RomDirectory) + ".ent"));
            }
            return dungeonBalance;
        }
        private IDungeonBalance? dungeonBalance;
        protected static string GetDungeonBalancePath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/dungeon_balance");
        #endregion

        #region StreamingAssets/native_data
        public PokemonFormDatabase GetPokemonFormDatabase()
        {
            if (pokemonFormDatabase == null)
            {
                pokemonFormDatabase = new PokemonFormDatabase(FileSystem.ReadAllBytes(GetPokemonFormDatabasePath(this.RomDirectory)));
            }
            return pokemonFormDatabase;
        }
        private PokemonFormDatabase? pokemonFormDatabase;
        protected static string GetPokemonFormDatabasePath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/pokemon_form_database.bin");

        public PokemonGraphicsDatabase GetPokemonGraphicsDatabase()
        {
            if (pokemonGraphicsDatabase == null)
            {
                pokemonGraphicsDatabase = new PokemonGraphicsDatabase(FileSystem.ReadAllBytes(GetPokemonGraphicsDatabasePath(this.RomDirectory)));
            }
            return pokemonGraphicsDatabase;
        }
        private PokemonGraphicsDatabase? pokemonGraphicsDatabase;
        protected static string GetPokemonGraphicsDatabasePath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/pokemon_graphics_database.bin");

        public Farc GetUSMessageBin()
        {
            if (messageBin == null)
            {
                var messageBinPath = GetMessageBinUSPath(RomDirectory);
                messageBin = new Farc(FileSystem.ReadAllBytes(messageBinPath));
            }
            return messageBin;
        }
        private Farc? messageBin;
        protected string GetMessageBinUSPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/message_us.bin");

        public ICommonStrings GetCommonStrings()
        {
            if (commonStrings == null)
            {
                var commonData = GetUSMessageBin().GetFile("common.bin");
                if (commonData == null)
                {
                    throw new Exception("Unable to load common.bin from US message bin");
                }

                var common = new MessageBinEntry(commonData);
                commonStrings = new CommonStrings(common);
            }
            return commonStrings;
        }
        private ICommonStrings? commonStrings;
        
        public Sir0StringList GetDungeonMapSymbol()
        {
            if (dungeonMapSymbol == null)
            {
                dungeonMapSymbol = new Sir0StringList(FileSystem.ReadAllBytes(GetDungeonMapSymbolPath(this.RomDirectory)));
            }
            return dungeonMapSymbol;
        }
        private Sir0StringList? dungeonMapSymbol;
        protected static string GetDungeonMapSymbolPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon_map_symbol.bin");
        
        public Sir0StringList GetDungeonBgmSymbol()
        {
            if (dungeonBgmSymbol == null)
            {
                dungeonBgmSymbol = new Sir0StringList(FileSystem.ReadAllBytes(GetDungeonBgmSymbolPath(this.RomDirectory)));
            }
            return dungeonBgmSymbol;
        }
        private Sir0StringList? dungeonBgmSymbol;
        protected static string GetDungeonBgmSymbolPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon_bgm_symbol.bin");

        public CodeTable GetCodeTable()
        {
            if (codeTable == null)
            {
                codeTable = new CodeTable(FileSystem.ReadAllBytes(GetCodeTablePath(this.RomDirectory)));
            }
            return codeTable;
        }
        private CodeTable? codeTable;
        protected static string GetCodeTablePath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/code_table.bin");
        #endregion

        #region Models
        public IStarterCollection GetStarters()
        {
            if (starterCollection == null)
            {
                var sp = GetServiceProvider();
                starterCollection = new StarterCollection(this,
                    sp.GetRequiredService<ILuaGenerator>(),
                    sp.GetRequiredService<ICSharpGenerator>());
            }
            return starterCollection;
        }
        private StarterCollection? starterCollection;

        public IDungeonCollection GetDungeons()
        {
            if (dungeonCollection == null)
            {
                dungeonCollection = new DungeonCollection(this);
            }
            return dungeonCollection;
        }
        private DungeonCollection? dungeonCollection;
        #endregion

        #region Helpers
        public PokemonGraphicsDatabase.PokemonGraphicsDatabaseEntry? FindGraphicsDatabaseEntryByCreature(CreatureIndex creatureIndex, PokemonFormType formIndex)
        {
            var formDatabase = GetPokemonFormDatabase();
            var graphics = GetPokemonGraphicsDatabase();

            var graphics1Index = formDatabase.GetGraphicsDatabaseIndex(creatureIndex, formIndex);
            var graphics0Index = graphics1Index - 1;
            if (graphics0Index < 0 || graphics0Index > graphics.Entries.Count)
            {
                return null;
            }

            return graphics.Entries[graphics0Index];
        }
        #endregion

        /// <summary>
        /// Registers a file to be written to the ROM on save
        /// </summary>
        /// <param name="relativePath">Relative path of the destination file in the ROM</param>
        /// <param name="data">Data to write</param>
        public void WriteFile(string relativePath, byte[] data)
        {
            filesToWrite.Add((relativePath, data));
        }

        /// <summary>
        /// Saves all loaded files to disk
        /// </summary>
        public Task Save(string directory, IFileSystem fileSystem)
        {
            void EnsureDirectoryExists(string path)
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
            }
            
            // Save wrappers around files
            if (starterCollection != null)
            {
                starterCollection.Flush();
            }

            // Save the files themselves
            if (mainExecutable != null)
            {
                var path = GetNsoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, mainExecutable.ToNso());
            }
            if (natureDiagnosis != null)
            {
                var path = GetNatureDiagnosisPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllText(path, JsonConvert.SerializeObject(natureDiagnosis));
            }
            if (fixedPokemon != null)
            {
                var path = GetFixedPokemonPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, fixedPokemon.Build().Data.ReadArray());
            }
            if (pokemonDataInfo != null)
            {
                var path = GetPokemonDataInfoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, pokemonDataInfo.ToByteArray());
            }

            // To-do: save commonStrings when implemented
            
            if (messageBin != null)
            {
                var path = GetMessageBinUSPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, messageBin.ToByteArray());
            }
            if (dungeonDataInfo != null)
            {
                var path = GetDungeonDataInfoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, dungeonDataInfo.ToByteArray());
            }
            if (dungeonBalance != null)
            {
                var path = GetDungeonBalancePath(directory);
                EnsureDirectoryExists(path);
                var (binData, entData) = dungeonBalance.Build();
                fileSystem.WriteAllBytes(path + ".bin", binData);
                fileSystem.WriteAllBytes(path + ".ent", entData);
            }
            if (dungeonExtra != null)
            {
                var path = GetDungeonExtraPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, dungeonExtra.ToByteArray());
            }
            if (pokemonFormDatabase != null)
            {
                var path = GetPokemonFormDatabasePath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, pokemonFormDatabase.ToByteArray());
            }
            if (pokemonGraphicsDatabase != null)
            {
                var path = GetPokemonGraphicsDatabasePath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, pokemonGraphicsDatabase.ToByteArray());
            }
            if (dungeonMapSymbol != null)
            {
                var path = GetDungeonMapSymbolPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, dungeonMapSymbol.ToByteArray());
            }
            if (dungeonBgmSymbol != null)
            {
                var path = GetDungeonBgmSymbolPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, dungeonBgmSymbol.ToByteArray());
            }

            foreach (var (relativePath, data) in filesToWrite)
            {
                var path = Path.Combine(directory, relativePath);
                var pathDirectory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(pathDirectory) && !fileSystem.DirectoryExists(pathDirectory))
                {
                    fileSystem.CreateDirectory(pathDirectory);
                }
                fileSystem.WriteAllBytes(path, data);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Saves all loaded files to disk
        /// </summary>
        public Task Save()
        {
            this.Save(this.RomDirectory, this.FileSystem);
            return Task.CompletedTask;
        }

        public string GenerateLuaChangeScript(int indentLevel = 0)
        {
            var script = new StringBuilder();
            script.Append(LuaSnippets.RequireSkyEditor);
            script.AppendLine();
            if (starterCollection != null)
            {
                script.Append(starterCollection.GenerateLuaChangeScript(indentLevel));
            }
            return script.ToString();
        }

        public string GenerateCSharpChangeScript(int indentLevel = 0)
        {
            var script = new StringBuilder();
            script.Append(CSharpSnippets.RequireSkyEditor);
            script.AppendLine();
            if (starterCollection != null)
            {
                script.Append(starterCollection.GenerateCSharpChangeScript(indentLevel));
            }
            return script.ToString();
        }
    }
}
