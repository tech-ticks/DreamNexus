using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Infrastructure.Automation.Modpacks
{
    public struct Script
    {
        public Script(ScriptType type, string relativePath, string script)
        {
            this.Type = type;
            this.RelativePath = relativePath;
            this.Value = script;
        }

        public ScriptType Type { get; }
        public string RelativePath { get; }
        public string Value { get; }
    }
}
