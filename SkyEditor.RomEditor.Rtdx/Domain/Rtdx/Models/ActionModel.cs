using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.ActDataInfo;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class ParameterModel
    {
        public EffectParameterType Type { get; set; }
        public ushort Value { get; set; }
    }

    public class EffectModel
    {
        public EffectType Type { get; set; }
        public List<ParameterModel> Parameters { get; set; } = new List<ParameterModel>();
    }

    public class ActionModel
    {
        // From act_data_info.bin
        public ActionFlags Flags { get; set; }

        public TextIDHash DungeonMessage1 { get; set; }
        public TextIDHash DungeonMessage2 { get; set; }

        public EffectModel[] Effects { get; set; } = new EffectModel[4];

        public ushort MinAccuracy { get; set; }
        public ushort MaxAccuracy { get; set; }

        public ActionKind Kind { get; set; }
        public PokemonType MoveType { get; set; }
        public MoveCategory MoveCategory { get; set; }

        public byte MinPower { get; set; }
        public byte MaxPower { get; set; }
        public byte MinPP { get; set; }
        public byte MaxPP { get; set; }

        public byte ActDataInfoByte83 { get; set; }
        public byte ActDataInfoByte84 { get; set; }
        public byte ActDataInfoByte85 { get; set; }
        public byte ActDataInfoByte86 { get; set; }
        public byte ActDataInfoByte87 { get; set; }

        // Maximum distance for moves: 2 tiles, 4 tiles, 10 tiles, etc.
        // For charged moves, the first turn has a range of 0, while the second turn has the correct range.
        public byte Range { get; set; }

        public byte ActDataInfoByte89 { get; set; }
        public byte ActDataInfoByte8A { get; set; }
        public byte ActDataInfoByte8B { get; set; }

        public ActionArea Area { get; set; }
        public ActionTarget Target { get; set; }

        public byte ActDataInfoByte8E { get; set; }
        public byte ActDataInfoByte8F { get; set; }
        public byte ActDataInfoByte90 { get; set; }
        public byte ActDataInfoByte91 { get; set; }
        public byte ActDataInfoByte92 { get; set; }

        public byte ActHitCountIndex { get; set; }

        public byte ActDataInfoByte94 { get; set; }
        public byte ActDataInfoByte95 { get; set; }
        public byte ActDataInfoByte96 { get; set; }
        public byte ActDataInfoByte97 { get; set; }
        public byte ActDataInfoByte98 { get; set; }
        public byte ActDataInfoByte99 { get; set; }
        public byte ActDataInfoByte9A { get; set; }
        public byte ActDataInfoByte9B { get; set; }
        public byte ActDataInfoByte9C { get; set; }
        public byte ActDataInfoByte9D { get; set; }
        public byte ActDataInfoByte9E { get; set; }
        public byte ActDataInfoByte9F { get; set; }
    }
}
