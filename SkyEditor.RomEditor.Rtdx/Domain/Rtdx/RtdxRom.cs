using AssetStudio;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Infrastructure.Automation.CSharp;
using SkyEditor.RomEditor.Infrastructure.Automation.Lua;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Common.Structures;
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
        
        /// <summary>
        /// Whether custom files that can be used with code injection projects should be preferred over
        /// directly editing the ROM's binary. This setting is experimental but required for some functionality.
        /// You need to download a build from https://github.com/tech-ticks/hyperbeam to use these
        /// custom file formats.
        /// </summary>
        bool EnableCustomFiles { get; set; }

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

        IExperience GetExperience();

        IWazaDataInfo GetWazaDataInfo();
        #endregion

        #region StreamingAssets/native_data/dungeon
        /// <summary>
        /// Gets static Pokemon data, loading it if needed
        /// </summary>
        IFixedPokemon GetFixedPokemon();

        IDungeonDataInfo GetDungeonDataInfo();

        IDungeonExtra GetDungeonExtra();

        IDungeonBalance GetDungeonBalance();

        IItemArrange GetItemArrange();

        IDungeonMapDataInfo GetDungeonMapDataInfo();

        IFixedMap GetFixedMap();

        IFixedItem GetFixedItem();

        IRandomParts GetRandomParts();

        IActDataInfo GetActDataInfo();

        IActEffectDataInfo GetActEffectDataInfo();

        IActHitCountTableDataInfo GetActHitCountTableDataInfo();

        IActParamDataInfo GetActParamDataInfo();

        IActStatusTableDataInfo GetActStatusTableDataInfo();

        IChargedMoves GetChargedMoves();

        IExtraLargeMoves GetExtraLargeMoves();

        IStatusDataInfo GetStatusDataInfo();

        RequestLevel GetRequestLevel();
        #endregion

        #region StreamingAssets/native_data
        public MessageBinEntry GetCommonBinEntry();
        public MessageBinEntry GetDungeonBinEntry();
        public MessageBinEntry GetScriptBinEntry();
        public MessageBinEntry GetDebugBinEntry();
        public MessageBinEntry GetTestBinEntry();
        ICommonStrings GetCommonStrings();
        PokemonGraphicsDatabase GetPokemonGraphicsDatabase();
        Farc GetUSMessageBin();
        PokemonFormDatabase GetPokemonFormDatabase();
        Sir0StringList GetDungeonMapSymbol();
        Sir0StringList GetDungeonBgmSymbol();
        Sir0StringList GetDungeonSeSymbol();
        Sir0StringList GetEffectSymbol();
        MapRandomGraphics GetMapRandomGraphics();
        ItemGraphics GetItemGraphics();
        ICodeTable GetCodeTable();
        IItemDataInfo GetItemDataInfo();
        Camp GetCamps();
        CampHabitat GetCampHabitat();
        PokemonEvolution GetPokemonEvolution();
        Rank GetRanks();
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
        public bool EnableCustomFiles { get; set; }
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
                byte[] nso = FileSystem.ReadAllBytes(GetNsoPath(this.RomDirectory));
                byte[] metadata = FileSystem.ReadAllBytes(GetIl2CppMetadataPath(this.RomDirectory));
                mainExecutable = MainExecutable.LoadFromNso(nso, metadata);
            }
            return mainExecutable;
        }
        private IMainExecutable? mainExecutable;

        protected static string GetNsoPath(string directory) => Path.Combine(directory, "exefs", "main");

        protected static string GetIl2CppMetadataPath(string directory) => Path.Combine(directory,
            "romfs/Data/Managed/Metadata/global-metadata.dat");
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
                pokemonDataInfo = new PokemonDataInfo(FileSystem.ReadAllBytes(GetPokemonDataInfoPath(this.RomDirectory)));
            }
            return pokemonDataInfo;
        }
        private PokemonDataInfo? pokemonDataInfo;
        protected static string GetPokemonDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/pokemon/pokemon_data_info.bin");

        public IExperience GetExperience()
        {
            if (experience == null)
            {
                experience = new Experience(FileSystem.ReadAllBytes(GetExperiencePath(this.RomDirectory) + ".bin"), FileSystem.ReadAllBytes(GetExperiencePath(this.RomDirectory) + ".ent"));
            }
            return experience;
        }
        private IExperience? experience;
        protected static string GetExperiencePath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/pokemon/experience");

        public IWazaDataInfo GetWazaDataInfo()
        {
            if (wazaDataInfo == null)
            {
                wazaDataInfo = new WazaDataInfo(FileSystem.ReadAllBytes(GetWazaDataInfoPath(this.RomDirectory)));
            }
            return wazaDataInfo;
        }
        private WazaDataInfo? wazaDataInfo;
        protected static string GetWazaDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/pokemon/waza_data_info.bin");
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

        public IItemArrange GetItemArrange()
        {
            if (itemArrange == null)
            {
                itemArrange = new ItemArrange(FileSystem.ReadAllBytes(GetItemArrangePath(this.RomDirectory) + ".bin"), FileSystem.ReadAllBytes(GetItemArrangePath(this.RomDirectory) + ".ent"));
            }
            return itemArrange;
        }
        private ItemArrange? itemArrange;
        protected static string GetItemArrangePath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/item_arrange");

        public IDungeonMapDataInfo GetDungeonMapDataInfo()
        {
            if (dungeonMapDataInfo == null)
            {
                dungeonMapDataInfo = new DungeonMapDataInfo(FileSystem.ReadAllBytes(GetDungeonMapDataInfoPath(this.RomDirectory)));
            }
            return dungeonMapDataInfo;
        }
        private DungeonMapDataInfo? dungeonMapDataInfo;
        protected static string GetDungeonMapDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/dungeon_map_data_info.bin");

        public IFixedMap GetFixedMap()
        {
            if (fixedMap == null)
            {
                fixedMap = new FixedMap(
                    FileSystem.ReadAllBytes(GetFixedMapPath(this.RomDirectory) + ".bin"),
                    FileSystem.ReadAllBytes(GetFixedMapPath(this.RomDirectory) + ".ent")
                );
            }
            return fixedMap;
        }
        private FixedMap? fixedMap;
        protected static string GetFixedMapPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/fixed_map");

        public IFixedItem GetFixedItem()
        {
            if (fixedItem == null)
            {
                fixedItem = new FixedItem(FileSystem.ReadAllBytes(GetFixedItemPath(this.RomDirectory)));
            }
            return fixedItem;
        }
        private FixedItem? fixedItem;
        protected static string GetFixedItemPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/fixed_item.bin");

        public IRandomParts GetRandomParts()
        {
            if (randomParts == null)
            {
                randomParts = new RandomParts(FileSystem.ReadAllBytes(GetRandomPartsPath(this.RomDirectory) + ".bin"), FileSystem.ReadAllBytes(GetRandomPartsPath(this.RomDirectory) + ".ent"));
            }
            return randomParts;
        }
        private IRandomParts? randomParts;
        protected static string GetRandomPartsPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/random_parts");

        public IActDataInfo GetActDataInfo()
        {
            if (actDataInfo == null)
            {
                actDataInfo = new ActDataInfo(FileSystem.ReadAllBytes(GetActDataInfoPath(this.RomDirectory)));
            }
            return actDataInfo;
        }
        private IActDataInfo? actDataInfo;
        protected static string GetActDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/act_data_info.bin");

        public IActEffectDataInfo GetActEffectDataInfo()
        {
            if (actEffectDataInfo == null)
            {
                actEffectDataInfo = new ActEffectDataInfo(FileSystem.ReadAllBytes(GetActEffectDataInfoPath(this.RomDirectory)));
            }
            return actEffectDataInfo;
        }
        private IActEffectDataInfo? actEffectDataInfo;
        protected static string GetActEffectDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/act_effect_data_info.bin");

        public IActHitCountTableDataInfo GetActHitCountTableDataInfo()
        {
            if (actHitCountTableDataInfo == null)
            {
                actHitCountTableDataInfo = new ActHitCountTableDataInfo(FileSystem.ReadAllBytes(GetActHitCountTableDataInfoPath(this.RomDirectory)));
            }
            return actHitCountTableDataInfo;
        }
        private IActHitCountTableDataInfo? actHitCountTableDataInfo;
        protected static string GetActHitCountTableDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/act_hit_count_table_data_info.bin");

        public IActParamDataInfo GetActParamDataInfo()
        {
            if (actParamDataInfo == null)
            {
                actParamDataInfo = new ActParamDataInfo(FileSystem.ReadAllBytes(GetActParamDataInfoPath(this.RomDirectory)));
            }
            return actParamDataInfo;
        }
        private IActParamDataInfo? actParamDataInfo;
        protected static string GetActParamDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/act_param_data_info.bin");

        public IActStatusTableDataInfo GetActStatusTableDataInfo()
        {
            if (actStatusTableDataInfo == null)
            {
                actStatusTableDataInfo = new ActStatusTableDataInfo(FileSystem.ReadAllBytes(GetActStatusTableDataInfoPath(this.RomDirectory)));
            }
            return actStatusTableDataInfo;
        }
        private IActStatusTableDataInfo? actStatusTableDataInfo;
        protected static string GetActStatusTableDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/act_status_table_data_info.bin");

        public IChargedMoves GetChargedMoves()
        {
            if (chargedMoves == null)
            {
                chargedMoves = new ChargedMoves(FileSystem.ReadAllBytes(GetChargedMovesPath(this.RomDirectory)));
            }
            return chargedMoves;
        }
        private ChargedMoves? chargedMoves;
        protected static string GetChargedMovesPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/act_tamewaza.bin");

        public IExtraLargeMoves GetExtraLargeMoves()
        {
            if (extraLargeMoves == null)
            {
                extraLargeMoves = new ExtraLargeMoves(FileSystem.ReadAllBytes(GetExtraLargeMovesPath(this.RomDirectory)));
            }
            return extraLargeMoves;
        }
        private ExtraLargeMoves? extraLargeMoves;
        protected static string GetExtraLargeMovesPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/act_xlwaza.bin");

        public IStatusDataInfo GetStatusDataInfo()
        {
            if (statusDataInfo == null)
            {
                statusDataInfo = new StatusDataInfo(FileSystem.ReadAllBytes(GetStatusDataInfoPath(this.RomDirectory)));
            }
            return statusDataInfo;
        }
        private StatusDataInfo? statusDataInfo;
        protected static string GetStatusDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/status_data_info.bin");

        public RequestLevel GetRequestLevel()
        {
            if (requestLevel == null)
            {
                requestLevel = new RequestLevel(FileSystem.ReadAllBytes(GetRequestLevel(this.RomDirectory)));
            }
            return requestLevel;
        }
        private RequestLevel? requestLevel;
        protected static string GetRequestLevel(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon/request_level.bin");
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

        public MessageBinEntry GetCommonBinEntry()
        {
            if (commonBinEntry == null)
            {
                var data = GetUSMessageBin().GetFile("common.bin");
                if (data == null)
                {
                    throw new Exception("Unable to load common.bin from US message bin");
                }

                commonBinEntry = new MessageBinEntry(data, GetCodeTable());
            }
            return commonBinEntry;
        }
        private MessageBinEntry? commonBinEntry;

        public MessageBinEntry GetDungeonBinEntry()
        {
            if (dungeonBinEntry == null)
            {
                var data = GetUSMessageBin().GetFile("dungeon.bin");
                if (data == null)
                {
                    throw new Exception("Unable to load dungeon.bin from US message bin");
                }

                dungeonBinEntry = new MessageBinEntry(data, GetCodeTable());
            }
            return dungeonBinEntry;
        }
        private MessageBinEntry? dungeonBinEntry;

        public MessageBinEntry GetScriptBinEntry()
        {
            if (scriptBinEntry == null)
            {
                var data = GetUSMessageBin().GetFile("script.bin");
                if (data == null)
                {
                    throw new Exception("Unable to load script.bin from US message bin");
                }

                scriptBinEntry = new MessageBinEntry(data, GetCodeTable());
            }
            return scriptBinEntry;
        }
        private MessageBinEntry? scriptBinEntry;

        public MessageBinEntry GetDebugBinEntry()
        {
            if (debugBinEntry == null)
            {
                var data = GetUSMessageBin().GetFile("debug.bin");
                if (data == null)
                {
                    throw new Exception("Unable to load debug.bin from US message bin");
                }

                debugBinEntry = new MessageBinEntry(data, GetCodeTable());
            }
            return debugBinEntry;
        }
        private MessageBinEntry? debugBinEntry;

        public MessageBinEntry GetTestBinEntry()
        {
            if (testBinEntry == null)
            {
                var data = GetUSMessageBin().GetFile("test.bin");
                if (data == null)
                {
                    throw new Exception("Unable to load test.bin from US message bin");
                }

                testBinEntry = new MessageBinEntry(data, GetCodeTable());
            }
            return testBinEntry;
        }
        private MessageBinEntry? testBinEntry;

        public ICommonStrings GetCommonStrings()
        {
            if (commonStrings == null)
            {
                var common = GetCommonBinEntry();
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

        public Sir0StringList GetDungeonSeSymbol()
        {
            if (dungeonSeSymbol == null)
            {
                dungeonSeSymbol = new Sir0StringList(FileSystem.ReadAllBytes(GetDungeonSeSymbolPath(this.RomDirectory)));
            }
            return dungeonSeSymbol;
        }
        private Sir0StringList? dungeonSeSymbol;
        protected static string GetDungeonSeSymbolPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/dungeon_se_symbol.bin");

        public Sir0StringList GetEffectSymbol()
        {
            if (effectSymbol == null)
            {
                effectSymbol = new Sir0StringList(FileSystem.ReadAllBytes(GetEffectSymbolPath(this.RomDirectory)), Encoding.ASCII);
            }
            return effectSymbol;
        }
        private Sir0StringList? effectSymbol;
        protected static string GetEffectSymbolPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/effect_symbol.bin");

        public MapRandomGraphics GetMapRandomGraphics()
        {
            if (mapRandomGraphics == null)
            {
                mapRandomGraphics = new MapRandomGraphics(FileSystem.ReadAllBytes(GetMapRandomGraphicsPath(this.RomDirectory)));
            }
            return mapRandomGraphics;
        }
        private MapRandomGraphics? mapRandomGraphics;
        protected static string GetMapRandomGraphicsPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/map_random_graphics.bin");

        public ItemGraphics GetItemGraphics()
        {
            if (itemGraphics == null)
            {
                itemGraphics = new ItemGraphics(FileSystem.ReadAllBytes(GetItemGraphicsPath(this.RomDirectory)));
            }
            return itemGraphics;
        }
        private ItemGraphics? itemGraphics;
        protected static string GetItemGraphicsPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/item_graphics.bin");

        public ICodeTable GetCodeTable()
        {
            if (codeTable == null)
            {
                codeTable = new RtdxCodeTable(FileSystem.ReadAllBytes(GetCodeTablePath(this.RomDirectory)));
            }
            return codeTable;
        }
        private ICodeTable? codeTable;
        protected static string GetCodeTablePath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/code_table.bin");

        public IItemDataInfo GetItemDataInfo()
        {
            if (itemDataInfo == null)
            {
                itemDataInfo = new ItemDataInfo(FileSystem.ReadAllBytes(GetItemDataInfoPath(this.RomDirectory)));
            }
            return itemDataInfo;
        }
        private IItemDataInfo? itemDataInfo;
        protected static string GetItemDataInfoPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/item_data_info.bin");

        public Camp GetCamps()
        {
            if (camps == null)
            {
                camps = new Camp(FileSystem.ReadAllBytes(GetCampPath(this.RomDirectory)));
            }
            return camps;
        }
        private Camp? camps;
        protected static string GetCampPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/camp/camp.bin");

        public CampHabitat GetCampHabitat()
        {
            if (campHabitat == null)
            {
                campHabitat = new CampHabitat(FileSystem.ReadAllBytes(GetCampHabitatPath(this.RomDirectory)));
            }
            return campHabitat;
        }
        private CampHabitat? campHabitat;
        protected static string GetCampHabitatPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/camp/camp_habitat.bin");

        public PokemonEvolution GetPokemonEvolution()
        {
            if (pokemonEvolution == null)
            {
                pokemonEvolution = new PokemonEvolution(FileSystem.ReadAllBytes(GetPokemonEvolutionPath(this.RomDirectory)));
            }
            return pokemonEvolution;
        }
        private PokemonEvolution? pokemonEvolution;
        protected static string GetPokemonEvolutionPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/pokemon_evolution.bin");

        public Rank GetRanks()
        {
            if (ranks == null)
            {
                ranks = new Rank(FileSystem.ReadAllBytes(GetRankPath(this.RomDirectory)));
            }
            return ranks;
        }
        private Rank? ranks;
        protected static string GetRankPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/native_data/rank.bin");

        #endregion

        #region StreamingAssets/custom_data

        protected static string GetStartersBinPath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/custom_data/starters.bin");
        protected static string GetActorDatabasePath(string directory) => Path.Combine(directory, "romfs/Data/StreamingAssets/custom_data/actor_database.bin");

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
            if (EnableCustomFiles)
            {
                // Write custom files. actor_database.bin must exist even if
                // the actor database was not modified.
                GetMainExecutable();
                var startersPath = GetStartersBinPath(directory);
                EnsureDirectoryExists(startersPath);
                fileSystem.WriteAllBytes(startersPath, mainExecutable!.StartersToByteArray());

                var actorDatabasePath = GetActorDatabasePath(directory);
                EnsureDirectoryExists(actorDatabasePath);
                fileSystem.WriteAllBytes(actorDatabasePath, mainExecutable.ActorDatabase.ToByteArray());
            }
            else if (mainExecutable != null)
            {
                // Edit the executable instead
                var path = GetNsoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, mainExecutable!.ToNso());
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
            if (experience != null)
            {
                var path = GetExperiencePath(directory);
                EnsureDirectoryExists(path);
                var (binData, entData) = experience.Build();
                fileSystem.WriteAllBytes(path + ".bin", binData);
                fileSystem.WriteAllBytes(path + ".ent", entData);
            }
            if (wazaDataInfo != null)
            {
                var path = GetWazaDataInfoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, wazaDataInfo.ToByteArray());
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
            if (fixedMap != null)
            {
                var path = GetFixedMapPath(directory);
                EnsureDirectoryExists(path);
                var (binData, entData) = fixedMap.Build();
                fileSystem.WriteAllBytes(path + ".bin", binData);
                fileSystem.WriteAllBytes(path + ".ent", entData);
            }
            if (fixedItem != null)
            {
                var path = GetFixedItemPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, fixedItem.ToByteArray());
            }
            if (randomParts != null)
            {
                var path = GetRandomPartsPath(directory);
                EnsureDirectoryExists(path);
                var (binData, entData) = randomParts.Build();
                fileSystem.WriteAllBytes(path + ".bin", binData);
                fileSystem.WriteAllBytes(path + ".ent", entData);
            }
            if (actDataInfo != null)
            {
                var path = GetActDataInfoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, actDataInfo.ToByteArray());
            }
            if (actEffectDataInfo != null)
            {
                var path = GetActEffectDataInfoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, actEffectDataInfo.ToByteArray());
            }
            if (actHitCountTableDataInfo != null)
            {
                var path = GetActHitCountTableDataInfoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, actHitCountTableDataInfo.Build());
            }
            if (actParamDataInfo != null)
            {
                var path = GetActParamDataInfoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, actParamDataInfo.Build());
            }
            if (actStatusTableDataInfo != null)
            {
                var path = GetActStatusTableDataInfoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, actStatusTableDataInfo.Build());
            }
            if (chargedMoves != null)
            {
                var path = GetChargedMovesPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, chargedMoves.ToByteArray());
            }
            if (extraLargeMoves != null)
            {
                var path = GetExtraLargeMovesPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, extraLargeMoves.ToByteArray());
            }
            if (statusDataInfo != null)
            {
                var path = GetStatusDataInfoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, statusDataInfo.ToByteArray());
            }

            if (dungeonBalance != null)
            {
                var path = GetDungeonBalancePath(directory);
                EnsureDirectoryExists(path);
                var (binData, entData) = dungeonBalance.Build();
                fileSystem.WriteAllBytes(path + ".bin", binData);
                fileSystem.WriteAllBytes(path + ".ent", entData);
            }
            if (itemArrange != null)
            {
                var path = GetItemArrangePath(directory);
                EnsureDirectoryExists(path);
                var (binData, entData) = itemArrange.Build();
                fileSystem.WriteAllBytes(path + ".bin", binData);
                fileSystem.WriteAllBytes(path + ".ent", entData);
            }
            if (dungeonMapDataInfo != null)
            {
                var path = GetDungeonMapDataInfoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, dungeonMapDataInfo.ToByteArray());
            }
            if (dungeonExtra != null)
            {
                var path = GetDungeonExtraPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, dungeonExtra.ToByteArray());
            }
            if (requestLevel != null)
            {
                var path = GetRequestLevel(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, requestLevel.ToByteArray());
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
            if (dungeonSeSymbol != null)
            {
                var path = GetDungeonSeSymbolPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, dungeonSeSymbol.ToByteArray());
            }
            if (effectSymbol != null)
            {
                var path = GetEffectSymbolPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, effectSymbol.ToByteArray());
            }
            if (mapRandomGraphics != null)
            {
                var path = GetMapRandomGraphicsPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, mapRandomGraphics.ToByteArray());
            }
            if (itemGraphics != null)
            {
                var path = GetItemGraphicsPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, itemGraphics.ToByteArray());
            }
            if (itemDataInfo != null)
            {
                var path = GetItemDataInfoPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, itemDataInfo.ToByteArray());
            }
            if (camps != null)
            {
                var path = GetCampPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, camps.ToByteArray());
            }
            if (campHabitat != null)
            {
                var path = GetCampHabitatPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, campHabitat.ToByteArray());
            }
            if (pokemonEvolution != null)
            {
                var path = GetPokemonEvolutionPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, pokemonEvolution.ToByteArray());
            }
            if (ranks != null)
            {
                var path = GetRankPath(directory);
                EnsureDirectoryExists(path);
                fileSystem.WriteAllBytes(path, ranks.ToByteArray());
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
