using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class FixedItemModel
    {
        public ItemIndex Index { get; set; }
        public ushort Quantity { get; set; } // Poké amount for MONEY_POKE, stack size for throwables, etc.
        public ushort Short04 { get; set; } // Used with chests, unsure what that means
        public byte Byte06 { get; set; } // Set to 1 for traps, also unsure what that means
    }
}
