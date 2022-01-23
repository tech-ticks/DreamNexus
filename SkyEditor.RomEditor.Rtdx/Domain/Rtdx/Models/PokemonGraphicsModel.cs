using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using EnabledPortraitsFlags = SkyEditor.RomEditor.Domain.Rtdx.Structures.PokemonGraphicsDatabase.PokemonGraphicsDatabaseEntry.EnabledPortraitsFlags;
using Flags = SkyEditor.RomEditor.Domain.Rtdx.Structures.PokemonGraphicsDatabase.PokemonGraphicsDatabaseEntry.PokemonGraphicsDatabaseEntryFlags;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class PokemonGraphicsModel
    {
        public string ModelName { get; set; } = "";
        public string AnimationName { get; set; } = "";
        public string BaseFormModelName { get; set; } = "";
        public string PortraitSheetName { get; set; } = "";
        public string RescueCampSheetName { get; set; } = "";
        public string RescueCampSheetReverseName { get; set; } = "";

        public float BaseScale { get; set; }
        public float DungeonBaseScale { get; set; }
        public float UnkX38 { get; set; } // Possibly DotOffsetX
        public float UnkX3C { get; set; } // Possibly DotOffsetY

        public float UnkX40 { get; set; }
        public float UnkX44 { get; set; }
        public float YOffset { get; set; }
        public float WalkSpeedDistance { get; set; }

        public float UnkX50 { get; set; } // Possibly WalkSpeedDistanceGround
        public float RunSpeedRatioGround { get; set; }
        public float UnkX58 { get; set; }
        public float UnkX5C { get; set; }

        public float UnkX60 { get; set; }
        public float UnkX64 { get; set; }
        public GraphicsBodySizeType UnknownBodyType1 { get; set; }
        public GraphicsBodySizeType UnknownBodyType2 { get; set; }

        public Flags Flags { get; set; }
        public EnabledPortraitsFlags EnabledPortraits { get; set; }
        public int UnkX78 { get; set; }
        public int UnkX7C { get; set; }

        public int UnkX80 { get; set; }
        public float UnkX84 { get; set; }
        public float UnkX88 { get; set; }
        public float UnkX8C { get; set; }

        public float UnkX90 { get; set; }
        public float UnkX94 { get; set; }
        public float UnkX98 { get; set; }
        public float UnkX9C { get; set; }


        public float UnkXA0 { get; set; }
        public int Padding1 { get; set; }
        public int Padding2 { get; set; }
        public int Padding3 { get; set; }
    }
}
