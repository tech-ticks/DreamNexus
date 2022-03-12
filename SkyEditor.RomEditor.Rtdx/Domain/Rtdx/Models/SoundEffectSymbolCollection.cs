using System;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface ISoundEffectSymbolCollection
    {
        List<string> Entries { get; }
        void Flush(IRtdxRom rom);
    }

    public class SoundEffectSymbolCollection : ISoundEffectSymbolCollection
    {
        public SoundEffectSymbolCollection()
        {
            Entries = new List<string>();
        }

        public SoundEffectSymbolCollection(IRtdxRom rom)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }

            var romEntries = rom.GetDungeonSeSymbol().Entries;

            this.Entries = new List<string>(romEntries);
        }

        public List<string> Entries { get; set; }

        public void Flush(IRtdxRom rom)
        {
            var romEntries = rom.GetDungeonSeSymbol().Entries;
            romEntries.Clear();

            foreach (var entry in Entries)
            {
                romEntries.Add(entry);
            }
        }
    }
}
