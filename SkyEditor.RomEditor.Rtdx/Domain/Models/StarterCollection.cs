using SkyEditor.RomEditor.Rtdx.Domain.Automation;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Infrastructure;
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
        public StarterCollection(IRtdxRom rom, ILuaGenerator luaGenerator)
        {
            this.rom = rom ?? throw new ArgumentNullException(nameof(rom));
            this.luaGenerator = luaGenerator ?? throw new ArgumentNullException(nameof(luaGenerator));

            this.Starters = LoadStarters();
            this.OriginalStarters = Starters.Select(s => s.Clone()).ToArray();
        }

        protected readonly IRtdxRom rom;
        protected readonly ILuaGenerator luaGenerator;

        public StarterModel[] Starters { get; }
        private StarterModel[] OriginalStarters { get; set; }

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

        public string GenerateLuaChangeScript(int indentLevel = 0)
        {
            var script = new StringBuilder();
            script.AppendLine(@"local starters = rom:GetStarters()");
            script.AppendLine();
            for (int i = 0; i < Starters.Length; i++)
            {
                var starter = Starters[i];
                var oldPokemon = OriginalStarters[i];
                if (starter.PokemonId != oldPokemon.PokemonId) 
                {
                    var variableName = $"starter{oldPokemon.PokemonId:d}";
                    script.AppendLine($"{LuaGenerator.GenerateIndentation(indentLevel)}local {variableName} = starters:GetStarterById({luaGenerator.GenerateLuaExpression(oldPokemon.PokemonId)})");
                    script.AppendLine($"{LuaGenerator.GenerateIndentation(indentLevel)}if {variableName} ~= nil then");
                    script.Append(luaGenerator.GenerateSimpleObjectDiff(oldPokemon, starter, variableName, indentLevel + 1));
                    script.AppendLine($"{LuaGenerator.GenerateIndentation(indentLevel)}else");
                    script.AppendLine($"{LuaGenerator.GenerateIndentation(indentLevel + 1)}error(\"Could not find starter '{oldPokemon.PokemonName}' with ID {oldPokemon.PokemonId:d}. This ROM may have already been modified.\")");
                    script.AppendLine($"{LuaGenerator.GenerateIndentation(indentLevel)}end");
                    script.AppendLine();
                }
            }
            return script.ToString();
        }

        /// <summary>
        /// Saves changes to <see cref="Starters"/> to the underlying file structures (without saving the file structures themselves)
        /// </summary>
        public void Flush()
        {
            var mainExecutable = rom.GetMainExecutable();
            var natureDiagnosis = rom.GetNatureDiagnosis();
            var fixedPokemon = rom.GetFixedPokemon();
            for (int i = 0; i < Starters.Length; i++)
            {
                var starter = Starters[i];
                var oldPokemon = OriginalStarters[i];

                var map = mainExecutable.StarterFixedPokemonMaps.First(m => m.PokemonId == oldPokemon.PokemonId);
                map.PokemonId = (CreatureIndex)starter.PokemonId;

                var fixedPokemonEntry = fixedPokemon.Entries[(int)map.FixedPokemonId];
                fixedPokemonEntry.PokemonId = (CreatureIndex)starter.PokemonId;
                fixedPokemonEntry.Move1 = (WazaIndex)starter.Move1;
                fixedPokemonEntry.Move2 = (WazaIndex)starter.Move2;
                fixedPokemonEntry.Move3 = (WazaIndex)starter.Move3;
                fixedPokemonEntry.Move4 = (WazaIndex)starter.Move4;

                var ndEntry = natureDiagnosis.m_pokemonNatureAndTypeList.First(p => p.m_nameLabel == oldPokemon.PokemonId);
                ndEntry.m_nameLabel = (CreatureIndex)starter.PokemonId;

                var symbolCandiate = PegasusActDatabase.ActorDataList
                    .Where(a => a.raw_pokemonIndex == starter.PokemonId
                        && a.bIsFemale == false) // bIsFemale is out of scope since this is just a proof-of-concept
                    .OrderByDescending(a => (int)a.raw_formType)
                    .FirstOrDefault();

                if (symbolCandiate != null)
                {
                    ndEntry.m_symbolName = symbolCandiate.symbolName;
                    ndEntry.m_symbolNameFemale = "";
                }
            }
            this.OriginalStarters = Starters.Select(s => s.Clone()).ToArray();
        }

        public StarterModel? GetStarterById(CreatureIndex id)
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

            public CreatureIndex PokemonId { get; set; }
            public string PokemonName => commonStrings.Pokemon.GetValueOrDefault(PokemonId) ?? $"(Unknown: {PokemonId})";
            public string? NatureDiagnosisMaleModelSymbol { get; set; }
            public string? NatureDiagnosisFemaleModelSymbol { get; set; }
            public WazaIndex Move1 { get; set; }
            public string Move1Name => commonStrings.Moves.GetValueOrDefault(Move1) ?? $"(Unknown: {Move1})";
            public WazaIndex Move2 { get; set; }
            public string Move2Name => commonStrings.Moves.GetValueOrDefault(Move2) ?? $"(Unknown: {Move2})";
            public WazaIndex Move3 { get; set; }
            public string Move3Name => commonStrings.Moves.GetValueOrDefault(Move3) ?? $"(Unknown: {Move3})";
            public WazaIndex Move4 { get; set; }
            public string Move4Name => commonStrings.Moves.GetValueOrDefault(Move4) ?? $"(Unknown: {Move4})";

            public NatureType? MaleNature { get; set; }
            public NatureType? FemaleNature { get; set; }

            public StarterModel Clone()
            {
                return new StarterModel(commonStrings)
                {
                    PokemonId = this.PokemonId,
                    NatureDiagnosisMaleModelSymbol = this.NatureDiagnosisMaleModelSymbol,
                    NatureDiagnosisFemaleModelSymbol = this.NatureDiagnosisFemaleModelSymbol,
                    Move1 = this.Move1,
                    Move2 = this.Move2,
                    Move3 = this.Move3,
                    Move4 = this.Move4,
                    MaleNature = this.MaleNature,
                    FemaleNature = this.FemaleNature
                };
            }
        }
    }
}
