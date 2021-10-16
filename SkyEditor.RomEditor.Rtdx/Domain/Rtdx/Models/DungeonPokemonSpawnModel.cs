using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class DungeonPokemonSpawnModel
    {
        public CreatureIndex StatsIndex { get; set; }
        public byte SpawnRate { get; set; }
        
        // Some special Pokémon like Kecleon and Strong Foes don't spawn randomly
        public bool IsSpecial { get; set; }
        public byte RecruitmentLevel { get; set; }
        public byte Byte0B { get; set; }
    }
}
