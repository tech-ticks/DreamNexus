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
        public ReplaceStarterHandler(IRtdxRom rom)
        {
            this.rom = rom ?? throw new ArgumentNullException(nameof(rom));
        }

        private readonly IRtdxRom rom;

        public void Handle(ReplaceStarterCommand command)
        {
            // Load files
            var nso = rom.GetMainExecutable();
            var natureDiagnosis = rom.GetNatureDiagnosis();
            var fixedPokemon = rom.GetFixedPokemon();

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
        }
    }
}
