using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class DungeonPokemonStatsModel
    {
        public CreatureIndex CreatureIndex { get; set; }
        public int XpYield { get; set; }
        public short HitPoints { get; set; }
        public byte Attack { get; set; }
        public byte SpecialAttack { get; set; }
        public byte Defense { get; set; }
        public byte SpecialDefense { get; set; }
        public byte Speed { get; set; }
        public bool StrongFoe { get; set; }
        public byte Level { get; set; }
    }
}
