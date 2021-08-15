using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class PokemonModel
    {
        public CreatureIndex Id { get; set; }
        public short PokedexNumber { get; set; }

        public short Taxon { get; set; }
        public AbilityIndex Ability1 { get; set; }
        public AbilityIndex Ability2 { get; set; }
        public AbilityIndex HiddenAbility { get; set; }
        public PokemonType Type1 { get; set; }
        public PokemonType Type2 { get; set; }

        public List<ItemIndex> LearnableTMs { get; set; } = new List<ItemIndex>();

        public List<PokemonDataInfo.LevelUpMove> LevelupLearnset { get; set; } = new List<PokemonDataInfo.LevelUpMove>();

        public PokemonDataInfo.FeatureFlags Features;

        public ushort Unknown62 { get; set; }
        public short Unknown66 { get; set; }

        public short BoostedRecruitRate { get; set; }  // Used on the second and further recruitments
        public short BaseRecruitRate { get; set; }  // Used on the first recruitment
        public short Unknown6E { get; set; }
        public short Unknown70 { get; set; }
        public short Unknown72 { get; set; }

        public short BaseHitPoints { get; set; }
        public short BaseAttack { get; set; }
        public short BaseSpecialAttack { get; set; }
        public short BaseDefense { get; set; }
        public short BaseSpecialDefense { get; set; }
        public short BaseSpeed { get; set; }

        public short Unknown80 { get; set; }
        public CreatureIndex EvolvesFrom { get; set; }

        public byte ExperienceEntry { get; set; }

        // -2, -2, 0 and 0 (respectively) for the Kangaskhan Child (index 1001, GARUURA_CHILD)
        // 2, 2, 1 and 1 (respectively) for everyone
        public sbyte Unknown8B { get; set; }
        public sbyte Unknown8C { get; set; }
        public sbyte Unknown8D { get; set; }
        public sbyte Unknown8E { get; set; }

        // 2 for the Deoxys Speed form (index 495, DEOKISHISU_S)
        // 1 for everyone else
        public byte Unknown95 { get; set; }

        // 100 for Snorlax and Sudowoodo (all variants)
        //   0 for Mega Mewtwo Y
        //  10 for everyone else
        public byte Unknown96 { get; set; }

        public byte Size { get; set; }

        // Set to 1 on several Megas, except:
        //   Beedrill, Pidgeot, Steelix, Slowbro, Kangaskhan, Sceptile,
        //   Swampert, Gallade, Sharpedo, Camerupt, Altaria, Glalie,
        //   Salamence, Metagross, Kyogre, Groudon, Rayquaza
        public byte MegaRelatedProperty { get; set; }

        // Level range for ???
        // - Wild spawns?
        // - Recruits?
        public byte SomeMinimumLevel { get; set; }
        public byte SomeMaximumLevel { get; set; }

        public string RecruitPrereq { get; set; } = "";

        // From pokemon_evolution.bin
        public (CreatureIndex First, CreatureIndex Second) MegaEvolutions { get; set; }

        public List<PokemonEvolution.PokemonEvolutionBranch> EvolutionBranches { get; set; }
            = new List<PokemonEvolution.PokemonEvolutionBranch>();
    }
}
