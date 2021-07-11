using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class ItemSetModel
    {
        public Dictionary<ItemKind, ushort> ItemKindWeights { get; set; } = new Dictionary<ItemKind, ushort>();
        public List<ItemArrange.Entry.ItemWeightEntry> ItemWeights { get; set; } = new List<ItemArrange.Entry.ItemWeightEntry>();
    }
}
