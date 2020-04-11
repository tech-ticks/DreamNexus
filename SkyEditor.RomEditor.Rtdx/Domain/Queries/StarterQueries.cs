using SkyEditor.RomEditor.Rtdx.Domain.Models;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Reverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Queries
{
    public interface IStarterQueries
    {
        StarterModel[] GetStarters();
    }

    public class StarterQueries : IStarterQueries
    {
        public StarterQueries(ICommonStrings commonStrings,
            IMainExecutable mainExecutable,
            NDConverterSharedData.DataStore natureDiagnosis,
            IFixedPokemon fixedPokemon)
        {
            this.commonStrings = commonStrings ?? throw new ArgumentNullException(nameof(commonStrings));
            this.mainExecutable = mainExecutable ?? throw new ArgumentNullException(nameof(mainExecutable));
            this.natureDiagnosis = natureDiagnosis ?? throw new ArgumentNullException(nameof(natureDiagnosis));
            this.fixedPokemon = fixedPokemon ?? throw new ArgumentNullException(nameof(fixedPokemon));
        }

        protected readonly ICommonStrings commonStrings;
        protected readonly IMainExecutable mainExecutable;
        protected readonly NDConverterSharedData.DataStore natureDiagnosis;
        protected readonly IFixedPokemon fixedPokemon;

        public StarterModel[] GetStarters()
        {
            var starters = new List<StarterModel>();
            foreach (var starter in natureDiagnosis.m_pokemonNatureAndTypeList)
            {
                var fixedPokemonSymbol = mainExecutable.StarterFixedPokemonMaps.FirstOrDefault(m => m.PokemonId == starter.m_nameLabel);
                if (fixedPokemonSymbol == default)
                {
                    // This isn't a usable starter
                    // The game WILL crash when loading the initial move set
                    continue;
                }

                var fixedPokemonEntry = fixedPokemon.Entries[(int)fixedPokemonSymbol.FixedPokemonId];
                starters.Add(new StarterModel
                {
                    PokemonId = starter.m_nameLabel,
                    PokemonName = commonStrings.Pokemon.GetValueOrDefault((int)starter.m_nameLabel) ?? $"(Unknown: {starter.m_nameLabel})",
                    NatureDiagnosisModelMale = !string.IsNullOrEmpty(starter.m_symbolName) ? PegasusActDatabase.FindActorData(starter.m_symbolName) : null,
                    NatureDiagnosisModelFemale = !string.IsNullOrEmpty(starter.m_symbolNameFemale) ? PegasusActDatabase.FindActorData(starter.m_symbolNameFemale) : null,
                    Move1 = fixedPokemonEntry.Move1,
                    Move2 = fixedPokemonEntry.Move2,
                    Move3 = fixedPokemonEntry.Move3,
                    Move4 = fixedPokemonEntry.Move4,
                    Move1Name = commonStrings.Moves.GetValueOrDefault((int)fixedPokemonEntry.Move1) ?? $"(Unknown: {fixedPokemonEntry.Move1})",
                    Move2Name = commonStrings.Moves.GetValueOrDefault((int)fixedPokemonEntry.Move2) ?? $"(Unknown: {fixedPokemonEntry.Move2})",
                    Move3Name = commonStrings.Moves.GetValueOrDefault((int)fixedPokemonEntry.Move3) ?? $"(Unknown: {fixedPokemonEntry.Move3})",
                    Move4Name = commonStrings.Moves.GetValueOrDefault((int)fixedPokemonEntry.Move4) ?? $"(Unknown: {fixedPokemonEntry.Move4})",
                    MaleNature = starter.m_maleNature,
                    FemaleNature = starter.m_femaleNature
                });
            }
            return starters.ToArray();
        }
    }
}
