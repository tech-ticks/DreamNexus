using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Automation.Lua
{
    public class LuaSnippets
    {
        /// <summary>
        /// A guard to be placed at the top of the script to ensure the script is run within the context of Sky Editor
        /// </summary>
        public static readonly string RequireSkyEditor = @"
if Rom == nil then
    error(""Script must be run in the context of Sky Editor"")
end
";

    }
}
