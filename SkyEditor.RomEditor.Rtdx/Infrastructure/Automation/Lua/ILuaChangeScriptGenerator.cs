using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Infrastructure.Automation.Lua
{
    public interface ILuaChangeScriptGenerator
    {
        string GenerateLuaChangeScript(int indentLevel = 0);
    }
}
