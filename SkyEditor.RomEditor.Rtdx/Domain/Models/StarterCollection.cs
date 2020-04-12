using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Reverse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using CreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;
using NatureType = SkyEditor.RomEditor.Rtdx.Reverse.NDConverterSharedData.NatureType;
using WazaIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.waza.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Models
{
    public class StarterCollection
    {
        public StarterCollection(IRtdxRom rom)
        {
            this.rom = rom ?? throw new ArgumentNullException(nameof(rom));

            this.Starters = LoadStarters();
            this.originalStarterIds = Starters.Select(s => s.PokemonId).ToArray();
        }

        protected readonly IRtdxRom rom;

        public StarterModel[] Starters { get; }
        private int[] originalStarterIds;

        private StarterModel[] LoadStarters()
        {
            var commonStrings = rom.GetCommonStrings();
            var mainExecutable = rom.GetMainExecutable();
            var natureDiagnosis = rom.GetNatureDiagnosis();
            var fixedPokemon = rom.GetFixedPokemon();

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
                starters.Add(new StarterModel(commonStrings)
                {
                    PokemonId = (int)starter.m_nameLabel,
                    NatureDiagnosisMaleModelSymbol = starter.m_symbolName,
                    NatureDiagnosisFemaleModelSymbol = starter.m_symbolNameFemale,
                    Move1 = (int)fixedPokemonEntry.Move1,
                    Move2 = (int)fixedPokemonEntry.Move2,
                    Move3 = (int)fixedPokemonEntry.Move3,
                    Move4 = (int)fixedPokemonEntry.Move4,
                    MaleNature = (int)starter.m_maleNature,
                    FemaleNature = (int)starter.m_femaleNature
                });
            }
            return starters.ToArray();
        }

        /// <summary>
        /// Saves changes to <see cref="Starters"/> to the underlying file structures (without saving the file structures themselves)
        /// </summary>
        public void Flush()
        {
            var mainExecutable = rom.GetMainExecutable();
            var natureDiagnosis = rom.GetNatureDiagnosis();
            var fixedPokemon = rom.GetFixedPokemon();
            for (int i = 0;i<Starters.Length;i++)
            {
                var starter = Starters[i];
                var oldPokemonId = originalStarterIds[i];

                var map = mainExecutable.StarterFixedPokemonMaps.First(m => (int)m.PokemonId == oldPokemonId);
                map.PokemonId = (CreatureIndex)starter.PokemonId;

                var fixedPokemonEntry = fixedPokemon.Entries[(int)map.FixedPokemonId];
                fixedPokemonEntry.PokemonId = (CreatureIndex)starter.PokemonId;
                fixedPokemonEntry.Move1 = (WazaIndex)starter.Move1;
                fixedPokemonEntry.Move2 = (WazaIndex)starter.Move2;
                fixedPokemonEntry.Move3 = (WazaIndex)starter.Move3;
                fixedPokemonEntry.Move4 = (WazaIndex)starter.Move4;

                var ndEntry = natureDiagnosis.m_pokemonNatureAndTypeList.First(p => (int)p.m_nameLabel == oldPokemonId);
                ndEntry.m_nameLabel = (CreatureIndex)starter.PokemonId;

                var symbolCandiate = PegasusActDatabase.ActorDataList
                    .Where(a => (int)a.raw_pokemonIndex == starter.PokemonId
                        && a.bIsFemale == false) // bIsFemale is out of scope since this is just a proof-of-concept
                    .OrderByDescending(a => (int)a.raw_formType)
                    .FirstOrDefault();

                if (symbolCandiate != null)
                {
                    ndEntry.m_symbolName = symbolCandiate.symbolName;
                    ndEntry.m_symbolNameFemale = "";
                }
            }
            this.originalStarterIds = Starters.Select(s => s.PokemonId).ToArray();
        }

        public StarterModel? GetStarterById(int id)
        {
            return Starters.FirstOrDefault(s => s.PokemonId == id);
        }

        [DebuggerDisplay("StarterModel: {PokemonName}")]
        public class StarterModel
        {
            public StarterModel(ICommonStrings commonStrings)
            {
                this.commonStrings = commonStrings ?? throw new ArgumentNullException(nameof(commonStrings));
            }

            private readonly ICommonStrings commonStrings;

            public int PokemonId { get; set; }
            public string PokemonName => commonStrings.Pokemon.GetValueOrDefault(PokemonId) ?? $"(Unknown: {PokemonId})";
            public string? NatureDiagnosisMaleModelSymbol { get; set; }
            public string? NatureDiagnosisFemaleModelSymbol { get; set; }
            public int Move1 { get; set; }
            public string Move1Name => commonStrings.Moves.GetValueOrDefault(Move1) ?? $"(Unknown: {Move1})";
            public int Move2 { get; set; }
            public string Move2Name => commonStrings.Moves.GetValueOrDefault(Move2) ?? $"(Unknown: {Move2})";
            public int Move3 { get; set; }
            public string Move3Name => commonStrings.Moves.GetValueOrDefault(Move3) ?? $"(Unknown: {Move3})";
            public int Move4 { get; set; }
            public string Move4Name => commonStrings.Moves.GetValueOrDefault(Move4) ?? $"(Unknown: {Move4})";

            public int? MaleNature { get; set; }
            public int? FemaleNature { get; set; }
        }
    }
}
