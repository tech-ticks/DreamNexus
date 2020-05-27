using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Automation
{
    public class SkyEditorLuaContext
    {
        public SkyEditorLuaContext(IRtdxRom rom)
        {
            this.rom = rom ?? throw new ArgumentNullException(nameof(rom));

            this.LuaState = new Lua();
            InitLuaState();
        }

        protected readonly IRtdxRom rom;

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

            RegisterEnum<Reverse.Const.ability.Index>("Const.ability.Index");
            RegisterEnum<Reverse.Const.creature.Index>("Const.creature.Index");
            RegisterEnum<Reverse.Const.fixed_creature.Index>("Const.fixed_creature.Index");
            RegisterEnum<Reverse.Const.item.Index>("Const.item.Index");
            RegisterEnum<Reverse.Const.item.Kind>("Const.item.Kind");
            RegisterEnum<Reverse.Const.item.PriceType>("Const.item.PriceType");
            RegisterEnum<Reverse.Const.order.Index>("Const.order.Index");
            RegisterEnum<Reverse.Const.pokemon.FixedWarehouseId>("Const.pokemon.FixedWarehouseId");
            RegisterEnum<Reverse.Const.pokemon.FormType>("Const.pokemon.FormType");
            RegisterEnum<Reverse.Const.pokemon.GenderType>("Const.pokemon.GenderType");
            RegisterEnum<Reverse.Const.pokemon.SallyType>("Const.pokemon.SallyType");
            RegisterEnum<Reverse.Const.pokemon.Type>("Const.pokemon.Type");
            RegisterEnum<Reverse.Const.sugowaza.Index>("Const.sugowaza.Index");
            RegisterEnum<Reverse.Const.waza.Index>("Const.waza.Index");
            RegisterEnum<Reverse.Const.EvolutionCameraType>("Const.EvolutionCameraType");
            RegisterEnum<Reverse.Const.GraphicsBodySizeType>("Const.GraphicsBodySizeType");
            RegisterEnum<Reverse.Const.TextIDHash>("Const.TextIDHash");

            // Make the ROM available to the script
            this.LuaState["rom"] = rom;

            // Sandbox script to prevent loading additional .Net libraries
            // This is not comprehensive
            // To-do: Use http://lua-users.org/wiki/SandBoxes to further protect against malicious scripts
            this.LuaState.DoString(@"
		        import = function () end
	        ");
        }

        public void Execute(string luaScript)
        {
            LuaState.DoString(luaScript);
        }

        public void RegisterEnum<T>(string targetEnumName)
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
    }
}
