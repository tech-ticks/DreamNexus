using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IDungeonMusicCollection
    {
        List<string> Music { get; }
        void Flush(IRtdxRom rom);
    }

    public class DungeonMusicCollection : IDungeonMusicCollection
    {
        public DungeonMusicCollection()
        {
            Music = new List<string>();
        }

        public DungeonMusicCollection(IRtdxRom rom)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }

            this.Music = rom.GetDungeonBgmSymbol().Entries.ToList(); // Copy
        }

        public List<string> Music { get; set; }

        public void Flush(IRtdxRom rom)
        {
            var dungeonBgmSymbol = rom.GetDungeonBgmSymbol();
            dungeonBgmSymbol.Entries.Clear();
            dungeonBgmSymbol.Entries.AddRange(Music);
        }
    }    
}
