using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Automation
{
    public interface IScriptExpressionGenerator
    {
        string Generate(object? value);
    }
}
