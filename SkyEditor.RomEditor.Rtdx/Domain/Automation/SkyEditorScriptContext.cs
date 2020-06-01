using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Automation
{
    public class SkyEditorScriptContext
    {
        private readonly static Regex CSharpPreprocessorRegex = new Regex(@"^\s*\#.*?$", RegexOptions.Compiled | RegexOptions.Multiline);

        public SkyEditorScriptContext(IRtdxRom rom)
        {
            this.Globals = new CSharpGlobals
            {
                Rom = rom ?? throw new ArgumentNullException(nameof(rom))
            };

            this.LuaState = new NLua.Lua();
            this.CSharpScriptImports = new List<string>();

            InitLuaState();
        }

        private CSharpGlobals Globals { get; }

        public NLua.Lua LuaState { get; }

        public List<string> CSharpScriptImports { get; set; }

        private void InitLuaState()
        {
            this.LuaState.LoadCLRPackage();

            // Load constants
            // Create the table structure manually, since this.luaState.NewTable doesn't support the depth we're going for
            this.LuaState.DoString(@"
                Const = {
                    ability = {
                        Index = {}
                    },
                    creature = {
                        Index = {}
                    },
                    fixed_creature = {
                        Index = {}
                    },
                    item = {
                        Index = {},
                        Kind = {},
                        PriceType = {}
                    },
                    order = {
                        Index = {}
                    },
                    pokemon = {
                        FixedWarehouseId = {},
                        FormType = {},
                        GenderType = {},
                        SallyType = {},
                        Type = {},
                    },
                    sugowaza = {
                        Index = {}
                    },
                    waza = {
                        Index = {}
                    },
                    EvolutionCameraType = {},
                    GraphicsBodySizeType = {},
                    TextIDHash = {}
                }
");

            RegisterEnum<Reverse.Const.ability.Index>("Const.ability.Index", "AbilityIndex");
            RegisterEnum<Reverse.Const.creature.Index>("Const.creature.Index", "CreatureIndex");
            RegisterEnum<Reverse.Const.fixed_creature.Index>("Const.fixed_creature.Index", "FixedCreatureIndex");
            RegisterEnum<Reverse.Const.item.Index>("Const.item.Index", "ItemIndex");
            RegisterEnum<Reverse.Const.item.Kind>("Const.item.Kind", "ItemKind");
            RegisterEnum<Reverse.Const.item.PriceType>("Const.item.PriceType", "ItemPriceType");
            RegisterEnum<Reverse.Const.order.Index>("Const.order.Index", "OrderIndex");
            RegisterEnum<Reverse.Const.pokemon.FixedWarehouseId>("Const.pokemon.FixedWarehouseId", "PokemonFixedWarehouseId");
            RegisterEnum<Reverse.Const.pokemon.FormType>("Const.pokemon.FormType", "PokemonFormType");
            RegisterEnum<Reverse.Const.pokemon.GenderType>("Const.pokemon.GenderType", "PokemonGenderType");
            RegisterEnum<Reverse.Const.pokemon.SallyType>("Const.pokemon.SallyType", "PokemonSallyType");
            RegisterEnum<Reverse.Const.pokemon.Type>("Const.pokemon.Type", "PokemonType");
            RegisterEnum<Reverse.Const.sugowaza.Index>("Const.sugowaza.Index", "SugowazaIndex");
            RegisterEnum<Reverse.Const.waza.Index>("Const.waza.Index", "WazaIndex");
            RegisterEnum<Reverse.Const.EvolutionCameraType>("Const.EvolutionCameraType", "EvolutionCameraType");
            RegisterEnum<Reverse.Const.GraphicsBodySizeType>("Const.GraphicsBodySizeType", "GraphicsBodySizeType");
            RegisterEnum<Reverse.Const.TextIDHash>("Const.TextIDHash", "TextIDHash");

            // Import globals, such as the ROM
            var globalsType = Globals.GetType();
            foreach (var property in globalsType.GetProperties())
            {
                this.LuaState[property.Name] = property.GetValue(Globals);
            }

            // Sandbox script to prevent loading additional .Net libraries
            // This is not comprehensive
            // To-do: Use http://lua-users.org/wiki/SandBoxes to further protect against malicious scripts
            this.LuaState.DoString(@"
		        import = function () end
	        ");
        }

        public void ExecuteLua(string luaScript)
        {
            LuaState.DoString(luaScript);
        }

        public async Task ExecuteCSharp(string cSharpScript)
        {
            // We can't run preprocessor directives since we'll have already run the enum imports
            // Combine that with us not wanting to run the stub csx for intellisense,
            // and it's easier to just ignore them
            var scriptWithoutPreprocessorDirectives = CSharpPreprocessorRegex.Replace(cSharpScript, "");

            await CSharpScript
                .RunAsync(string.Join(Environment.NewLine, CSharpScriptImports) + scriptWithoutPreprocessorDirectives,                
                ScriptOptions.Default
                    .WithReferences(typeof(SkyEditorScriptContext).Assembly)
                    .WithImports("SkyEditor.RomEditor.Rtdx", "System", "System.Linq"),
                globals: Globals)
                .ConfigureAwait(false);
        }

        public void RegisterEnum<T>(string targetLuaEnumName, string cSharpImportAlias)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("T must be an enum");
            }

            // This doesn't have enough depth for our needs
            // this.luaState.NewTable(targetEnumName);

            var names = Enum.GetNames(type);
            var values = (T[])Enum.GetValues(type);
            for (int i = 0; i < names.Length; i++)
            {
                string path = targetLuaEnumName + "." + names[i];
                this.LuaState.SetObjectToPath(path, values[i]);
            }

            this.CSharpScriptImports.Add($"using {cSharpImportAlias} = {type.FullName};");
        }

        // This class must be public for CSharp scripts to use it
        public class CSharpGlobals
        {
            public IRtdxRom Rom { get; internal set; } = default!;
        }
    }
}
