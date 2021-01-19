using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Infrastructure.Automation
{
    public interface IScriptHost
    {
        IModTarget Target { get; }

        void ExecuteLua(string luaScript);
        Task ExecuteCSharp(string cSharpScript);
    }

    public class ScriptHost<TTarget> : IScriptHost where TTarget : IModTarget
    {
        private readonly static Regex CSharpPreprocessorRegex = new Regex(@"^\s*\#.*?$", RegexOptions.Compiled | RegexOptions.Multiline);

        public ScriptHost(TTarget rom, Mod? mod = null)
        {
            this.Globals = new ScriptContext<TTarget>(rom, mod);
            this.LuaState = new NLua.Lua();
            this.CSharpScriptImports = new List<string>();
        }

        private ScriptContext<TTarget> Globals { get; }
        IModTarget IScriptHost.Target => Globals.Rom;

        public NLua.Lua LuaState { get; }

        public List<string> CSharpScriptImports { get; set; }

        private bool luaStateInitialized = false;

        private void InitLuaState()
        {
            if (luaStateInitialized)
            {
                return;
            }

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

            luaStateInitialized = true;
        }

        public void ExecuteLua(string luaScript)
        {
            InitLuaState();
            LuaState.DoString(luaScript);
        }

        public async Task ExecuteCSharp(string cSharpScript)
        {
            // Don't allow preprocessor directives
            var scriptWithoutPreprocessorDirectives = CSharpPreprocessorRegex.Replace(cSharpScript, "");

            await CSharpScript
                .RunAsync(string.Join(Environment.NewLine, CSharpScriptImports) + scriptWithoutPreprocessorDirectives,
                ScriptOptions.Default
                .WithReferences(typeof(ScriptHost<>).Assembly)
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
    }
}
