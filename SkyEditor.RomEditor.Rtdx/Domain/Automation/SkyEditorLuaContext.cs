using NLua;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Automation
{
    public class SkyEditorLuaContext
    {
        public SkyEditorLuaContext(IRtdxRom rom)
        {
            this.rom = rom ?? throw new ArgumentNullException(nameof(rom));

            this.luaState = new Lua();
            InitLuaState();
        }

        protected readonly Lua luaState;
        protected readonly IRtdxRom rom;

        private void InitLuaState()
        {
            this.luaState.LoadCLRPackage();

            // Load constants
            this.luaState.DoString($@"
                Const = {{
                    ability = {{
                        Index = {EnumToLuaObject(typeof(Reverse.Const.ability.Index))}
                    }},
                    creature = {{
                        Index = {EnumToLuaObject(typeof(Reverse.Const.creature.Index))}
                    }},
                    fixed_creature = {{
                        Index = {EnumToLuaObject(typeof(Reverse.Const.fixed_creature.Index))}
                    }},
                    item = {{
                        Index = {EnumToLuaObject(typeof(Reverse.Const.item.Index))},
                        Kind = {EnumToLuaObject(typeof(Reverse.Const.item.Kind))},
                        PriceType = {EnumToLuaObject(typeof(Reverse.Const.item.PriceType))}
                    }},
                    order = {{
                        Index = {EnumToLuaObject(typeof(Reverse.Const.order.Index))}
                    }},
                    pokemon = {{
                        FixedWarehouseId = {EnumToLuaObject(typeof(Reverse.Const.pokemon.FixedWarehouseId))},
                        FormType = {EnumToLuaObject(typeof(Reverse.Const.pokemon.FormType))},
                        GenderType = {EnumToLuaObject(typeof(Reverse.Const.pokemon.GenderType))},
                        SallyType = {EnumToLuaObject(typeof(Reverse.Const.pokemon.SallyType))},
                        Type = {EnumToLuaObject(typeof(Reverse.Const.pokemon.Type))},
                    }},
                    sugowaza = {{
                        Index = {EnumToLuaObject(typeof(Reverse.Const.sugowaza.Index))}
                    }},
                    waza = {{
                        Index = {EnumToLuaObject(typeof(Reverse.Const.waza.Index))}
                    }},
                    EvolutionCameraType = {EnumToLuaObject(typeof(Reverse.Const.EvolutionCameraType))},
                    GraphicsBodySizeType = {EnumToLuaObject(typeof(Reverse.Const.GraphicsBodySizeType))},
                    TextIDHash = {EnumToLuaObject(typeof(Reverse.Const.TextIDHash))}
                }}
");

            // Make the ROM available to the script
            this.luaState["rom"] = rom;

            // Sandbox script to prevent loading additional .Net libraries
            // This is not comprehensive
            // To-do: Use http://lua-users.org/wiki/SandBoxes to further protect against malicious scripts
            this.luaState.DoString(@"
		        import = function () end
	        ");
        }

        public void Execute(string luaScript)
        {
            luaState.DoString(luaScript);
        }

        public static string EnumToLuaObject(Type enumType) 
        {
            var underlyingType = Enum.GetUnderlyingType(enumType);
            var luaScript = new StringBuilder();
            luaScript.Append("{");
            foreach (var enumValue in Enum.GetValues(enumType))
            {                
                if (enumValue == null)
                {
                    continue;
                }

                var underlyingValue = Convert.ChangeType(enumValue, underlyingType);
                luaScript.Append($"{Enum.GetName(enumType, enumValue)}={underlyingValue},");
            }
            luaScript.Length -= 1; // Trim trailing ','
            luaScript.Append("}");
            return luaScript.ToString();
        }
    }
}
