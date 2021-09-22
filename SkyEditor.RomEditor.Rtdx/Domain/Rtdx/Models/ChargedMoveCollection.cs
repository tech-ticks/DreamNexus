using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IChargedMoveCollection
    {
        List<ChargedMoveModel> Entries { get; }
        void Flush(IRtdxRom rom);
    }

    public class ChargedMoveCollection : IChargedMoveCollection
    {
        public ChargedMoveCollection()
        {
            Entries = new List<ChargedMoveModel>();
        }

        public ChargedMoveCollection(IRtdxRom rom)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }
            
            var entries = new List<ChargedMoveModel>();
            var romEntries = rom.GetChargedMoves().Entries;

            foreach (var romEntry in romEntries)
            {
                entries.Add(new ChargedMoveModel
                {
                    BaseMove = romEntry.BaseMove,
                    BaseAction = romEntry.BaseAction,
                    FinalMove = romEntry.FinalMove,
                    FinalAction = romEntry.FinalAction,
                    Short08 = romEntry.Short08,
                });
            }

            this.Entries = entries;
        }

        public List<ChargedMoveModel> Entries { get; set; }

        public void Flush(IRtdxRom rom)
        {
            var romEntries = rom.GetChargedMoves().Entries;
            romEntries.Clear();

            foreach (var entry in Entries)
            {
                romEntries.Add(new ChargedMoves.Entry
                {
                    BaseMove = entry.BaseMove,
                    BaseAction = entry.BaseAction,
                    FinalMove = entry.FinalMove,
                    FinalAction = entry.FinalAction,
                    Short08 = entry.Short08,
                });
            }
        }
    }
}
