using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class DungeonFloorModel
    {
        // From dungeon_balance.bin
        public short Index { get; set; }
        public short BalanceFloorInfoShort02 { get; set; } // Same as RequestLevel Short4
        public string Event { get; set; } = "";
        public short BalanceFloorInfoShort24 { get; set; }
        public short BalanceFloorInfoShort26 { get; set; } // Same as RequestLevel Short6
        public short BalanceFloorInfoShort28 { get; set; }  // Same as RequestLevel Short8
        public short DungeonMapDataInfoIndex { get; set; }
        public byte NameId { get; set; } // From dungeon_balance.bin and request_level.bin
        public byte BalanceFloorInfoByte2D { get; set; }
        public byte BalanceFloorInfoByte2E { get; set; }
        public byte BalanceFloorInfoByte2F { get; set; }
        public short BalanceFloorInfoShort30 { get; set; }
        public short BalanceFloorInfoShort32 { get; set; }
        public byte BalanceFloorInfoByte34 { get; set; }
        public byte BalanceFloorInfoByte35 { get; set; }
        public byte UnknownItemSetIndex { get; set; }
        public byte InvitationIndex { get; set; }
        public byte BalanceFloorInfoByte55 { get; set; }
        public byte BalanceFloorInfoByte56 { get; set; }
        public byte BalanceFloorInfoByte57 { get; set; }
        public byte BalanceFloorInfoByte58 { get; set; }
        public DungeonStatusIndex Weather { get; set; }
        public byte[] BalanceFloorInfoBytes37to53 { get; set; } = new byte[0];
        public byte[] BalanceFloorInfoBytes5Ato61 { get; set; } = new byte[0];

        // From request_level.bin
        public bool IsBossFloor { get; set; }

        // From dungeon_balance.bin
        public Dictionary<ItemIndex, short>? TrapWeights { get; set; }
        public List<DungeonPokemonSpawnModel>? Spawns { get; set; }
    }
}
