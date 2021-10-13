using SkyEditor.RomEditor.Infrastructure.Automation.CSharp;
using SkyEditor.RomEditor.Infrastructure.Automation.Lua;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IStarterCollection
    {
        StarterModel[] Starters { get; }
        StarterModel? GetStarterById(CreatureIndex id);
        string GenerateLuaChangeScript(int indentLevel = 0);
        void Flush(IRtdxRom rom);

        CreatureIndex HeroCreature { get; set; }
        PokemonGenderType HeroGender { get; set; }
        CreatureIndex PartnerCreature { get; set; }
        PokemonGenderType PartnerGender { get; set; }
    }

    public class StarterCollection : IStarterCollection
    {
        public StarterCollection()
        {
            Starters = new StarterModel[0];
        }

        public StarterCollection(IRtdxRom rom, ILuaGenerator luaGenerator, ICSharpGenerator cSharpGenerator)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }

            var defaultStarters = rom.GetDefaultStarters();
            HeroCreature = defaultStarters.HeroCreature;
            HeroGender = defaultStarters.HeroGender;
            PartnerCreature = defaultStarters.PartnerCreature;
            PartnerGender = defaultStarters.PartnerGender;

            this.Starters = LoadStarters(rom);
        }

        public CreatureIndex HeroCreature { get; set; }
        public PokemonGenderType HeroGender { get; set; }
        public CreatureIndex PartnerCreature { get; set; }
        public PokemonGenderType PartnerGender { get; set; }

        public StarterModel[] Starters { get; set; }

        private StarterModel[] LoadStarters(IRtdxRom rom)
        {
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
                starters.Add(new StarterModel
                {
                    PokemonId = starter.m_nameLabel,
                    NatureDiagnosisMaleModelSymbol = starter.m_symbolName,
                    NatureDiagnosisFemaleModelSymbol = starter.m_symbolNameFemale,
                    Move1 = fixedPokemonEntry.Move1,
                    Move2 = fixedPokemonEntry.Move2,
                    Move3 = fixedPokemonEntry.Move3,
                    Move4 = fixedPokemonEntry.Move4,
                    MaleNature = starter.m_maleNature,
                    FemaleNature = starter.m_femaleNature
                });
            }
            return starters.ToArray();
        }

        [Obsolete]
        public string GenerateLuaChangeScript(int indentLevel = 0)
        {
            // TODO: remove
            throw new NotImplementedException("Change scripts are no longer supported");
        }

        [Obsolete]
        public string GenerateCSharpChangeScript(int indentLevel = 0)
        {
            // TODO: remove
            throw new NotImplementedException("Change scripts are no longer supported");
        }

        /// <summary>
        /// Saves changes to <see cref="Starters"/> to the underlying file structures (without saving the file structures themselves)
        /// </summary>
        public void Flush(IRtdxRom rom)
        {
            var originalStarters = LoadStarters(rom);
            var mainExecutable = rom.GetMainExecutable();
            var natureDiagnosis = rom.GetNatureDiagnosis();
            var fixedPokemon = rom.GetFixedPokemon();

            var natureDiagnosisActorMaleLookup = mainExecutable.ActorDatabase.ActorDataList
                .ToLookup(actor => actor.SymbolName);

            for (int i = 0; i < Starters.Length; i++)
            {
                var starter = Starters[i];
                var oldPokemon = originalStarters[i];

                var map = mainExecutable.StarterFixedPokemonMaps.First(m => m.PokemonId == oldPokemon.PokemonId);
                map.PokemonId = starter.PokemonId;

                var fixedPokemonEntry = fixedPokemon.Entries[(int)map.FixedPokemonId];
                fixedPokemonEntry.PokemonId = starter.PokemonId;
                fixedPokemonEntry.Move1 = starter.Move1;
                fixedPokemonEntry.Move2 = starter.Move2;
                fixedPokemonEntry.Move3 = starter.Move3;
                fixedPokemonEntry.Move4 = starter.Move4;

                var ndEntry = natureDiagnosis.m_pokemonNatureAndTypeList.First(p => p.m_nameLabel == oldPokemon.PokemonId);
                ndEntry.m_nameLabel = starter.PokemonId;

                var natureDiagnosisActorMale = natureDiagnosisActorMaleLookup[ndEntry.m_symbolName]
                    .FirstOrDefault();

                if (natureDiagnosisActorMale != null)
                {
                    natureDiagnosisActorMale.PokemonIndex = starter.PokemonId;
                }

                // This may work in some cases but not all of them
                // I expect this will fail in-game for any Pokemon without a Female form
                //var natureDiagnosisActorFemale = mainExecutable.ActorDatabase.ActorDataList
                //    .FirstOrDefault(a => a.SymbolName == ndEntry.m_symbolNameFemale);
                //if (natureDiagnosisActorFemale != null)
                //{
                //    natureDiagnosisActorFemale.PokemonIndex = starter.PokemonId;
                //}
            }

            var defaultStarters = rom.GetDefaultStarters();
            defaultStarters.HeroCreature = HeroCreature;
            defaultStarters.HeroGender = HeroGender;
            defaultStarters.PartnerCreature = PartnerCreature;
            defaultStarters.PartnerGender = PartnerGender;
            System.Console.WriteLine("Flush finished");
        }

        public StarterModel? GetStarterById(CreatureIndex id)
        {
            return Starters.FirstOrDefault(s => s.PokemonId == id);
        }
    }    
}
