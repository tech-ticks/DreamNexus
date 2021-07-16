namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class DungeonMapModel
    {
        // From dungeon_map_symbol.bin
        public string Symbol { get; set; } = "";

        // From dungeon_map_data_info.bin

        // The index of the last value in fixed_map.ent (pointing to the end) if no fixed map is used.
        public ushort FixedMapIndex { get; set; }

        public byte Byte06 { get; set; }
        public byte Byte07 { get; set; }
        public byte DungeonBgmSymbolIndex { get; set; }
        public byte Byte09 { get; set; }
        public byte Byte0A { get; set; }
        public byte Byte0B { get; set; }
    }
}
