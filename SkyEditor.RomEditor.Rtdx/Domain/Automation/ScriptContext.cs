using SkyEditor.RomEditor.Domain.Automation.Modpacks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SkyEditor.RomEditor.Domain.Automation
{
    public class ScriptContext<TTarget> where TTarget : IModTarget
    {
        public ScriptContext(TTarget rom, IScriptModAccessor? mod = null)
        {
            this.Rom = rom ?? throw new ArgumentNullException(nameof(rom));
            this.Mod = mod;
        }

        public TTarget Rom { get; }
        public IScriptModAccessor? Mod { get; }
    }
}
