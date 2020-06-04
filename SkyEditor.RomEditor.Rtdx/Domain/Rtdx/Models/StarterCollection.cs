using SkyEditor.RomEditor.Domain.Automation.CSharp;
using SkyEditor.RomEditor.Domain.Automation.Lua;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IStarterCollection
    {
        IStarterModel[] Starters { get; }
        IStarterModel? GetStarterById(CreatureIndex id);
        string GenerateLuaChangeScript(int indentLevel = 0);
        void Flush();
    }

    public class StarterCollection : IStarterCollection
    {
        public StarterCollection(IRtdxRom rom, ILuaGenerator luaGenerator, ICSharpGenerator cSharpGenerator)
        {
            this.rom = rom ?? throw new ArgumentNullException(nameof(rom));
            this.luaGenerator = luaGenerator ?? throw new ArgumentNullException(nameof(luaGenerator));
            this.cSharpGenerator = cSharpGenerator ?? throw new ArgumentNullException(nameof(cSharpGenerator));

            this.Starters = LoadStarters();
            this.OriginalStarters = Starters.Select(s => s.Clone()).ToArray();
        }

        protected readonly IRtdxRom rom;
        protected readonly ILuaGenerator luaGenerator;
        protected readonly ICSharpGenerator cSharpGenerator;

        public IStarterModel[] Starters { get; }
        private IStarterModel[] OriginalStarters { get; set; }

        private IStarterModel[] LoadStarters()
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
            return starters.Cast<IStarterModel>().ToArray();
        }

        public string GenerateLuaChangeScript(int indentLevel = 0)
        {
            var script = new StringBuilder();
            script.AppendLine(@"local starters = Rom:GetStarters()");
            script.AppendLine();
            for (int i = 0; i < Starters.Length; i++)
            {
                var starter = Starters[i];
                var oldPokemon = OriginalStarters[i];
                if (starter.PokemonId != oldPokemon.PokemonId) 
                {
                    var variableName = $"starter{oldPokemon.PokemonId:d}";
                    script.AppendLine($"{LuaGenerator.GenerateIndentation(indentLevel)}local {variableName} = starters:GetStarterById({luaGenerator.GenerateExpression(oldPokemon.PokemonId)})");
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

        public string GenerateCSharpChangeScript(int indentLevel = 0)
        {
            var script = new StringBuilder();
            script.AppendLine(@"var starters = Rom.GetStarters();");
            script.AppendLine();
            for (int i = 0; i < Starters.Length; i++)
            {
                var starter = Starters[i];
                var oldPokemon = OriginalStarters[i];
                if (starter.PokemonId != oldPokemon.PokemonId)
                {
                    var variableName = $"starter{oldPokemon.PokemonId:d}";
                    script.AppendLine($"{CSharpGenerator.GenerateIndentation(indentLevel)}var {variableName} = starters.GetStarterById({cSharpGenerator.GenerateExpression(oldPokemon.PokemonId)});");
                    script.AppendLine($"{CSharpGenerator.GenerateIndentation(indentLevel)}if ({variableName} != null)");
                    script.AppendLine(CSharpGenerator.GenerateIndentation(indentLevel) + "{");
                    script.Append(cSharpGenerator.GenerateSimpleObjectDiff(oldPokemon, starter, variableName, indentLevel + 1));
                    script.AppendLine(CSharpGenerator.GenerateIndentation(indentLevel) + "}");
                    script.AppendLine($"{CSharpGenerator.GenerateIndentation(indentLevel)}else");
                    script.AppendLine(CSharpGenerator.GenerateIndentation(indentLevel) + "{");
                    script.AppendLine($"{CSharpGenerator.GenerateIndentation(indentLevel + 1)}throw new System.Exception(\"Could not find starter '{oldPokemon.PokemonName}' with ID {oldPokemon.PokemonId:d}. This ROM may have already been modified.\");");
                    script.AppendLine(CSharpGenerator.GenerateIndentation(indentLevel) + "}");
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
                map.PokemonId = starter.PokemonId;

                var fixedPokemonEntry = fixedPokemon.Entries[(int)map.FixedPokemonId];
                fixedPokemonEntry.PokemonId = starter.PokemonId;
                fixedPokemonEntry.Move1 = starter.Move1;
                fixedPokemonEntry.Move2 = starter.Move2;
                fixedPokemonEntry.Move3 = starter.Move3;
                fixedPokemonEntry.Move4 = starter.Move4;

                var ndEntry = natureDiagnosis.m_pokemonNatureAndTypeList.First(p => p.m_nameLabel == oldPokemon.PokemonId);
                ndEntry.m_nameLabel = starter.PokemonId;

                var natureDiagnosisActorMale = mainExecutable.ActorDatabase.ActorDataList
                    .FirstOrDefault(a => a.SymbolName == ndEntry.m_symbolName);

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
            this.OriginalStarters = Starters.Select(s => s.Clone()).ToArray();
        }

        public IStarterModel? GetStarterById(CreatureIndex id)
        {
            return Starters.FirstOrDefault(s => s.PokemonId == id);
        }
    }    
}
