using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class DungeonPokemonSpawnModel
    {
        public CreatureIndex StatsIndex { get; set; }
        public byte SpawnRate { get; set; }
        public byte RecruitmentLevel { get; set; }
        public byte Byte0B { get; set; }
    }
}
