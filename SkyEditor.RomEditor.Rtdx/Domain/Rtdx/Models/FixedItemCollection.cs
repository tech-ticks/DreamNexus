using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IFixedItemCollection
    {
        List<FixedItemModel> Entries { get; }
        void Flush(IRtdxRom rom);
    }

    public class FixedItemCollection : IFixedItemCollection
    {
        public FixedItemCollection()
        {
            Entries = new List<FixedItemModel>();
        }

        public FixedItemCollection(IRtdxRom rom)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }
            
            var entries = new List<FixedItemModel>();
            var romEntries = rom.GetFixedItem().Entries;

            foreach (var romEntry in romEntries)
            {
                entries.Add(new FixedItemModel
                {
                    Index = romEntry.Index,
                    Quantity = romEntry.Quantity,
                    Short04 = romEntry.Short04,
                    Byte06 = romEntry.Byte06,
                });
            }

            this.Entries = entries;
        }

        public List<FixedItemModel> Entries { get; set; }

        public void Flush(IRtdxRom rom)
        {
            var romEntries = rom.GetFixedItem().Entries;
            romEntries.Clear();

            for (int i = 0; i < Entries.Count; i++)
            {
                var entry = Entries[i];
                romEntries.Add(new FixedItem.Entry
                {
                    Index = entry.Index,
                    Quantity = entry.Quantity,
                    Short04 = entry.Short04,
                    Byte06 = entry.Byte06,
                });
            }
        }
    }
}
