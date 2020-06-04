using DotNet3dsToolkit;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Domain.Psmd.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Psmd
{
    public class PsmdRom
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
    }
}
