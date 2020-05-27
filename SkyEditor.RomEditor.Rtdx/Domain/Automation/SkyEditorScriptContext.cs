using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Automation
{
    public class SkyEditorScriptContext
    {
        public SkyEditorScriptContext(IRtdxRom rom)
        {
            this.Globals = new CSharpGlobals
            {
                Rom = rom ?? throw new ArgumentNullException(nameof(rom))
            };

            this.LuaState = new Lua();
            InitLuaState();
        }

        private CSharpGlobals Globals { get; }

        public Lua LuaState { get; }

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

            RegisterLuaEnum<Reverse.Const.ability.Index>("Const.ability.Index");
            RegisterLuaEnum<Reverse.Const.creature.Index>("Const.creature.Index");
            RegisterLuaEnum<Reverse.Const.fixed_creature.Index>("Const.fixed_creature.Index");
            RegisterLuaEnum<Reverse.Const.item.Index>("Const.item.Index");
            RegisterLuaEnum<Reverse.Const.item.Kind>("Const.item.Kind");
            RegisterLuaEnum<Reverse.Const.item.PriceType>("Const.item.PriceType");
            RegisterLuaEnum<Reverse.Const.order.Index>("Const.order.Index");
            RegisterLuaEnum<Reverse.Const.pokemon.FixedWarehouseId>("Const.pokemon.FixedWarehouseId");
            RegisterLuaEnum<Reverse.Const.pokemon.FormType>("Const.pokemon.FormType");
            RegisterLuaEnum<Reverse.Const.pokemon.GenderType>("Const.pokemon.GenderType");
            RegisterLuaEnum<Reverse.Const.pokemon.SallyType>("Const.pokemon.SallyType");
            RegisterLuaEnum<Reverse.Const.pokemon.Type>("Const.pokemon.Type");
            RegisterLuaEnum<Reverse.Const.sugowaza.Index>("Const.sugowaza.Index");
            RegisterLuaEnum<Reverse.Const.waza.Index>("Const.waza.Index");
            RegisterLuaEnum<Reverse.Const.EvolutionCameraType>("Const.EvolutionCameraType");
            RegisterLuaEnum<Reverse.Const.GraphicsBodySizeType>("Const.GraphicsBodySizeType");
            RegisterLuaEnum<Reverse.Const.TextIDHash>("Const.TextIDHash");

            // Make the ROM available to the script
            this.LuaState["rom"] = Globals.Rom;

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
            await CSharpScript
                .RunAsync(cSharpScript,                
                ScriptOptions.Default
                    .WithReferences(typeof(SkyEditorScriptContext).Assembly)
                    .WithImports("SkyEditor.RomEditor.Rtdx.Reverse", "SkyEditor.RomEditor.Rtdx.Reverse.Const"),
                globals: Globals)
                .ConfigureAwait(false);
        }

        public void RegisterLuaEnum<T>(string targetEnumName)
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
                string path = targetEnumName + "." + names[i];
                this.LuaState.SetObjectToPath(path, values[i]);
            }
        }

        // This class must be public for CSharp scripts to use it
        public class CSharpGlobals
        {
            public IRtdxRom Rom { get; internal set; } = default!;
        }
    }
}
