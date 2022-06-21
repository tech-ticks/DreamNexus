using System;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class DungeonPokemonSpawnModel
    {
        public CreatureIndex StatsIndex { get; set; }
        public byte SpawnWeight { get; set; }

        [Obsolete($"Replaced with {nameof(SpawnWeight)}"), DeserializeOnly]
        public byte SpawnRate
        {
            get => (byte) (SpawnWeight >> 1);
            set => SpawnWeight |= (byte) (value << 1);
        }
        
        [Obsolete($"Part of {nameof(SpawnWeight)}"), DeserializeOnly]
        public bool IsSpecial
        {
            get => (SpawnWeight & 0b1) != 0;
            set => SpawnWeight |= value ? (byte) 1 : (byte) 0;
        }

        public byte RecruitmentLevel { get; set; }
        public byte Byte0B { get; set; }
    }
}
