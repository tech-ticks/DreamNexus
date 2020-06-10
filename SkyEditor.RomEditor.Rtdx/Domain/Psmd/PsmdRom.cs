using DotNet3dsToolkit;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Domain.Psmd.Constants;
using SkyEditor.RomEditor.Domain.Psmd.Structures;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using System;
using System.IO;

namespace SkyEditor.RomEditor.Domain.Psmd
{
    public interface IPsmdRom : IModTarget
    {
        PokemonFormDatabase GetPokemonFormDatabase();
        PokemonGraphicsDatabase GetPokemonGraphicsDatabase();
        Farc GetPokemonGraphics();
        public Farc GetUSMessageBin();
        ICommonStrings GetCommonStrings();

        PokemonDataInfo GetPokemonDataInfo();

        PokemonGraphicsDatabase.PokemonGraphicsDatabaseEntry? FindGraphicsDatabaseEntryByCreature(CreatureIndex creatureIndex, PokemonFormType formIndex);
    }

    public class PsmdRom : IPsmdRom
    {
        public PsmdRom(string directory, IFileSystem fileSystem)
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
        }

        public PsmdRom(ThreeDsRom rom)
        {
            this.Rom = rom ?? throw new ArgumentNullException(nameof(rom));
            this.FileSystem = rom;
            this.RomDirectory = "/";
        }

        public string RomDirectory { get; }
        protected IFileSystem FileSystem { get; }
        protected ThreeDsRom? Rom { get; }

        #region RomFS
        public PokemonFormDatabase GetPokemonFormDatabase()
        {
            if (pokemonFormDatabase == null)
            {
                pokemonFormDatabase = new PokemonFormDatabase(FileSystem.ReadAllBytes(GetPokemonFormDatabasePath(this.RomDirectory)));
            }
            return pokemonFormDatabase;
        }
        private PokemonFormDatabase? pokemonFormDatabase;
        protected static string GetPokemonFormDatabasePath(string directory) => Path.Combine(directory, "RomFS/pokemon_form_database.bin");

        public PokemonGraphicsDatabase GetPokemonGraphicsDatabase()
        {
            if (pokemonGraphicsDatabase == null)
            {
                pokemonGraphicsDatabase = new PokemonGraphicsDatabase(FileSystem.ReadAllBytes(GetPokemonGraphicsDatabasePath(this.RomDirectory)));
            }
            return pokemonGraphicsDatabase;
        }
        private PokemonGraphicsDatabase? pokemonGraphicsDatabase;
        protected static string GetPokemonGraphicsDatabasePath(string directory) => Path.Combine(directory, "RomFS/pokemon_graphics_database.bin");

        public Farc GetPokemonGraphics()
        {
            if (pokemonGraphics == null)
            {
                pokemonGraphics = new Farc(FileSystem.ReadAllBytes(GetPokemonGraphicsPath(this.RomDirectory)));
            }
            return pokemonGraphics;
        }
        private Farc? pokemonGraphics;
        protected static string GetPokemonGraphicsPath(string directory) => Path.Combine(directory, "RomFS/pokemon_graphic.bin");

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
        protected string GetMessageBinUSPath(string directory) => Path.Combine(directory, "RomFS/message_us.bin");

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
        #endregion

        #region RomFS/pokemon
        public PokemonDataInfo GetPokemonDataInfo()
        {
            if (pokemonDataInfo == null)
            {
                var data = FileSystem.ReadAllBytes(GetPokemonDataInfoPath(this.RomDirectory));
                pokemonDataInfo = new PokemonDataInfo(data.AsSpan());
            }
            return pokemonDataInfo;
        }
        private PokemonDataInfo? pokemonDataInfo;
        protected static string GetPokemonDataInfoPath(string directory) => Path.Combine(directory, "RomFS/pokemon/pokemon_data_info.bin");

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
    }
}
