using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Infrastructure.Automation.CSharp
{
    public interface ICSharpChangeScriptGenerator
    {
        string GenerateCSharpChangeScript(int indentLevel = 0);
    }
}
