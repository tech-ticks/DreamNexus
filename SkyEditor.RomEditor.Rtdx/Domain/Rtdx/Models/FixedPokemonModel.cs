using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class FixedPokemonModel
    {
        public CreatureIndex PokemonId { get; set; }
        public short Short04 { get; set; }
        public short Short06 { get; set; }
        public WazaIndex Move1 { get; set; }
        public WazaIndex Move2 { get; set; }
        public WazaIndex Move3 { get; set; }
        public WazaIndex Move4 { get; set; }
        public DungeonIndex DungeonIndex { get; set; }
        public short Short12 { get; set; }
        public byte Byte14 { get; set; }
        public byte Byte15 { get; set; }
        public byte Byte16 { get; set; }
        public byte Byte17 { get; set; }
        public byte Byte18 { get; set; }
        public byte Byte19 { get; set; }
        public byte Level { get; set; }
        public short HitPoints { get; set; }
        public byte AttackBoost { get; set; }
        public byte SpAttackBoost { get; set; }
        public byte DefenseBoost { get; set; }
        public byte SpDefenseBoost { get; set; }
        public byte SpeedBoost { get; set; }
        public byte Byte20 { get; set; }
        public byte Byte21 { get; set; }
        public byte Byte22 { get; set; }
        public byte Byte23 { get; set; }
        public byte Byte24 { get; set; }
        public byte Byte25 { get; set; }
        public byte Byte26 { get; set; }
        public byte InvitationIndex { get; set; }
        public byte Byte28 { get; set; }
        public byte Byte29 { get; set; }
        public byte Byte2A { get; set; }
        public byte Byte2B { get; set; }
        public byte Byte2C { get; set; }
        public byte Byte2D { get; set; }
        public byte Byte2E { get; set; }
        public byte Byte2F { get; set; }
    }
}
