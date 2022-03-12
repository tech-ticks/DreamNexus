using System;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IEffectSymbolCollection
    {
        List<string> Entries { get; }
        void Flush(IRtdxRom rom);
    }

    public class EffectSymbolCollection : IEffectSymbolCollection
    {
        public EffectSymbolCollection()
        {
            Entries = new List<string>();
        }

        public EffectSymbolCollection(IRtdxRom rom)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }
            
            var romEntries = rom.GetEffectSymbol().Entries;

            this.Entries = new List<string>(romEntries);
        }

        public List<string> Entries { get; set; }

        public void Flush(IRtdxRom rom)
        {
            var romEntries = rom.GetEffectSymbol().Entries;
            romEntries.Clear();

            foreach (var entry in Entries)
            {
                romEntries.Add(entry);
            }
        }
    }
}
