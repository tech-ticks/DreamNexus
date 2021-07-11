using System;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    // TODO: things to check:
    // - whether some of the bits correspond to blocking certain options in the menus
    // - whether they have any relation with the story progression
    // - possible relation with the "give up" option following up with the "start over" question
    [Flags]
    public enum DungeonFeature
    {
        FloorDirectionUp = (1 << 0),           // Floor direction (0 = BxxF, 1 = xxF)
        _Bit1 = (1 << 1),                    //   ??  (always 0)
        _Bit2 = (1 << 2),                    //   ??  (always 1)
        _Bit3 = (1 << 3),                    //   ??  (always 0)
        LevelReset = (1 << 4),               // Level reset to 5 upon entry
        _Bit5 = (1 << 5),                    //   ??  (0 on Purity Forest, Wish Cave, Joyous Tower, Illusory Grotto, Makuhita Dojo and the dojo mazes)
        _Bit6 = (1 << 6),                    //   ??  (always 1)
        _Bit7 = (1 << 7),                    //   ??  (always 1)
        _Bit8 = (1 << 8),                    //   ??  (always 0)
        AutoRevive = (1 << 9),               // (possibly) Auto-revive  (0 on Tiny Woods (index 49) and Thunderwave Cave (index 50), Makuhita Dojo and the dojo mazes)
        _Bit10 = (1 << 10),                  //   ??  (0 on Oddity Cave, Fiery/Lightning/Northwind Field and Mt. Faraway)
        _Bit11 = (1 << 11),                  //   ??  (always 1)
        _Bit12 = (1 << 12),                  //   ??  (always 0)
        _Bit13 = (1 << 13),                  //   ??  (always 0)
        _Bit14 = (1 << 14),                  //   ??  (always 0)
        WildPokemonRecruitable = (1 << 15),  // Leader can recruit wild Pokemon
        _Bit16 = (1 << 16),                  //   ??  (0 on both versions of Tiny Woods and Thunderwave Cave, Lapis Cave, Rock/Snow Path, Mt. Blaze, Frosty Forest and Mt. Freeze)
                                            //   Since Tiny Woods/Thunderwave Cave are early dungeons and Lapis Cave through Mt. Freeze are visited during the fugitive arc,
                                            //   I suspect this bit is related to not being able to go back to the rescue team base after fainting/giving up.
        Scanner = (1 << 17),                // Permanent Scanner status
        Radar = (1 << 18)                    // Permanent Radar status
        // bits 19+ are always 0
    }
}
