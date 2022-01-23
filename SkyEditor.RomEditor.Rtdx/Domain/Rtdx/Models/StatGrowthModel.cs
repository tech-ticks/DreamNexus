using System.Collections.Generic;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class StatGrowthModel
    {
        public List<StatGrowthLevel> Levels { get; set; } = new List<StatGrowthLevel>();
    }

    public class StatGrowthLevel
    {
        public int MinimumExperience { get; set; }
        public byte HitPointsGained { get; set; }
        public byte AttackGained { get; set; }
        public byte SpecialAttackGained { get; set; }
        public byte DefenseGained { get; set; }
        public byte SpecialDefenseGained { get; set; }
        public byte SpeedGained { get; set; }
        public byte LevelsGained { get; set; }
    }
}
