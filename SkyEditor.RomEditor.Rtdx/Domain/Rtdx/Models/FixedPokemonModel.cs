using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class FixedPokemonModel
    {
        public CreatureIndex PokemonId { get; set; }
        public WazaIndex Move1 { get; set; }
        public WazaIndex Move2 { get; set; }
        public WazaIndex Move3 { get; set; }
        public WazaIndex Move4 { get; set; }
        public DungeonIndex DungeonIndex { get; set; }
        public byte Level { get; set; }
        public short HitPoints { get; set; }
        public byte AttackBoost { get; set; }
        public byte SpAttackBoost { get; set; }
        public byte DefenseBoost { get; set; }
        public byte SpDefenseBoost { get; set; }
        public byte SpeedBoost { get; set; }
        public byte InvitationIndex { get; set; }
    }
}
