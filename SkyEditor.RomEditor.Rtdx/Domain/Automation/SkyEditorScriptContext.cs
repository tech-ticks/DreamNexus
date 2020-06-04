using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Domain.Automation
{
    public class SkyEditorScriptContext
    {
        private readonly static Regex CSharpPreprocessorRegex = new Regex(@"^\s*\#.*?$", RegexOptions.Compiled | RegexOptions.Multiline);

        public SkyEditorScriptContext(IRtdxRom rom)
        {
            this.Globals = new CSharpGlobals(rom);
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

            RegisterLuaEnum<AbilityIndex>("Const.ability.Index");
            RegisterLuaEnum<CreatureIndex>("Const.creature.Index");
            RegisterLuaEnum<FixedCreatureIndex>("Const.fixed_creature.Index");
            RegisterLuaEnum<ItemIndex>("Const.item.Index");
            RegisterLuaEnum<ItemKind>("Const.item.Kind");
            RegisterLuaEnum<ItemPriceType>("Const.item.PriceType");
            RegisterLuaEnum<OrderIndex>("Const.order.Index");
            RegisterLuaEnum<PokemonFixedWarehouseId>("Const.pokemon.FixedWarehouseId");
            RegisterLuaEnum<PokemonFormType>("Const.pokemon.FormType");
            RegisterLuaEnum<PokemonGenderType>("Const.pokemon.GenderType");
            RegisterLuaEnum<PokemonSallyType>("Const.pokemon.SallyType");
            RegisterLuaEnum<PokemonType>("Const.pokemon.Type");
            RegisterLuaEnum<SugowazaIndex>("Const.sugowaza.Index");
            RegisterLuaEnum<WazaIndex>("Const.waza.Index");
            RegisterLuaEnum<EvolutionCameraType>("Const.EvolutionCameraType");
            RegisterLuaEnum<GraphicsBodySizeType>("Const.GraphicsBodySizeType");
            RegisterLuaEnum<TextIDHash>("Const.TextIDHash");

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
                    .WithImports(
                        "System",
                        "System.Linq",
                        "SkyEditor.RomEditor",
                        "SkyEditor.RomEditor.Domain.Rtdx.Constants"
                    ),
                globals: Globals)
                .ConfigureAwait(false);
        }

        public void RegisterLuaEnum<T>(string targetLuaEnumName)
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
        }

        // This class must be public for CSharp scripts to use it
        public class CSharpGlobals
        {
            public CSharpGlobals(IRtdxRom rom)
            {
                this.Rom = rom ?? throw new ArgumentNullException(nameof(rom));
            }

            public IRtdxRom Rom { get; }
        }
    }
}
