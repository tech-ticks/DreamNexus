using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Infrastructure.Automation.Modpacks
{
    public class UnsupportedScriptTypeException : Exception
    {
        public UnsupportedScriptTypeException(string scriptExtension) : base("Unsupported script extension: " + scriptExtension)
        {
        }
    }
}
