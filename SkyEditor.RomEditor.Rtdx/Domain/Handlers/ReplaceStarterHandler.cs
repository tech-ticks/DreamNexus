using Newtonsoft.Json;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Rtdx.Domain.Commands;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Reverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Handlers
{
    public class ReplaceStarterHandler
    {
        public ReplaceStarterHandler(IFileSystem rom)
        {
            this.rom = rom ?? throw new ArgumentNullException(nameof(rom));
        }

        private readonly IFileSystem rom;


        public void Handle(ReplaceStarterCommand command)
        {
            // Load files
            const string nsoPath = "exefs/main";
            IMainExecutable nso = MainExecutable.LoadFromNso(rom.ReadAllBytes(nsoPath));

            const string natureDiagnosisPath = "romfs/Data/StreamingAssets/data/nature_diagnosis/diagnosis.json";
            var natureDiagnosis = JsonConvert.DeserializeObject<NDConverterSharedData.DataStore>(rom.ReadAllText(natureDiagnosisPath));

            const string fixedPokemonPath = "romfs/Data/StreamingAssets/native_data/dungeon/fixed_pokemon.bin";
            IFixedPokemon fixedPokemon = new FixedPokemon(rom.ReadAllBytes(fixedPokemonPath));

            // Process
            var map = nso.StarterFixedPokemonMaps.First(m => m.PokemonId == command.OldPokemonId);
            map.PokemonId = command.NewPokemonId;

            var fixedPokemonEntry = fixedPokemon.Entries[(int)map.FixedPokemonId];
            fixedPokemonEntry.PokemonId = command.NewPokemonId;
            if (command.Move1 != default)
            {
                fixedPokemonEntry.Move1 = command.Move1;
            }
            if (command.Move2 != default)
            {
                fixedPokemonEntry.Move2 = command.Move2;
            }
            if (command.Move3 != default)
            {
                fixedPokemonEntry.Move3 = command.Move3;
            }
            if (command.Move4 != default)
            {
                fixedPokemonEntry.Move4 = command.Move4;
            }

            var ndEntry = natureDiagnosis.m_pokemonNatureAndTypeList.First(p => p.m_nameLabel == command.OldPokemonId);
            ndEntry.m_nameLabel = command.NewPokemonId;

            var symbolCandiate = PegasusActDatabase.ActorDataList
                .Where(a => a.raw_pokemonIndex == command.NewPokemonId
                    && a.bIsFemale == false) // bIsFemale is out of scope since this is just a proof-of-concept
                .OrderByDescending(a => (int)a.raw_formType)
                .FirstOrDefault();

            if (symbolCandiate != null)
            {
                ndEntry.m_symbolName = symbolCandiate.symbolName!;
                ndEntry.m_symbolNameFemale = "";
            }

            // Save files
            rom.WriteAllBytes(nsoPath, nso.ToNso());
            rom.WriteAllText(natureDiagnosisPath, JsonConvert.SerializeObject(natureDiagnosis));
            rom.WriteAllBytes(fixedPokemonPath, fixedPokemon.Build().ReadArray());
        }
    }
}
