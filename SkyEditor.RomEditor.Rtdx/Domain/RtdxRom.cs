using Newtonsoft.Json;
using SkyEditor.IO.FileSystem;
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
        /// <summary>
        /// Gets the main executable, loading it if needed
        /// </summary>
        IMainExecutable GetMainExecutable();

        /// <summary>
        /// Gets the personality test settings, loading it if needed
        /// </summary>
        NDConverterSharedData.DataStore GetNatureDiagnosis();

        /// <summary>
        /// Gets static Pokemon data, loading it if needed
        /// </summary>
        IFixedPokemon GetFixedPokemon();

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

        /// <summary>
        /// Saves all loaded files to disk
        /// </summary>
        public void Save()
        {
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
        }
    }
}
