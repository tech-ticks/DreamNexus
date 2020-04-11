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
    }
}
