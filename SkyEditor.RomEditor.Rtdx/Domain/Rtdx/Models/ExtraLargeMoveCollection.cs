using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IExtraLargeMoveCollection
    {
        List<ExtraLargeMoveModel> Entries { get; }
        void Flush(IRtdxRom rom);
    }

    public class ExtraLargeMoveCollection : IExtraLargeMoveCollection
    {
        public ExtraLargeMoveCollection()
        {
            Entries = new List<ExtraLargeMoveModel>();
        }

        public ExtraLargeMoveCollection(IRtdxRom rom)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }
            
            var entries = new List<ExtraLargeMoveModel>();
            var romEntries = rom.GetExtraLargeMoves().Entries;

            foreach (var romEntry in romEntries)
            {
                entries.Add(new ExtraLargeMoveModel
                {
                    BaseMove = romEntry.BaseMove,
                    BaseAction = romEntry.BaseAction,
                    LargeMove = romEntry.LargeMove,
                    LargeAction = romEntry.LargeAction
                });
            }

            this.Entries = entries;
        }

        public List<ExtraLargeMoveModel> Entries { get; set; }

        public void Flush(IRtdxRom rom)
        {
            var romEntries = rom.GetExtraLargeMoves().Entries;
            romEntries.Clear();

            foreach (var entry in Entries)
            {
                romEntries.Add(new ExtraLargeMoves.Entry
                {
                    BaseMove = entry.BaseMove,
                    BaseAction = entry.BaseAction,
                    LargeMove = entry.LargeMove,
                    LargeAction = entry.LargeAction,
                });
            }
        }
    }
}
