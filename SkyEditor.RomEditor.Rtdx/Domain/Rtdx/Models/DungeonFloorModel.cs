using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public class DungeonFloorModel
    {
        // From dungeon_balance.bin
        public short Index { get; set; }
        public short BalanceFloorInfoShort02 { get; set; } // Same as RequestLevel Short4
        public string Event { get; set; } = "";

        public short TurnLimit { get; set; }
        public short MinMoneyStackSize { get; set; } // Same as RequestLevel Short6
        public short MaxMoneyStackSize { get; set; } // Same as RequestLevel Short8
        public short DungeonMapDataInfoIndex { get; set; }
        public byte NameId { get; set; } // From dungeon_balance.bin and request_level.bin
        public byte BalanceFloorInfoByte2D { get; set; }
        public byte BalanceFloorInfoByte2E { get; set; }
        public byte BalanceFloorInfoByte2F { get; set; }
        public short BalanceFloorInfoShort30 { get; set; }
        public short BalanceFloorInfoShort32 { get; set; }
        public byte BalanceFloorInfoByte34 { get; set; }
        public byte BalanceFloorInfoByte35 { get; set; }
        public byte RoomCount { get; set; }
        public byte BalanceFloorInfoByte37 { get; set; }
        public byte BalanceFloorInfoByte38 { get; set; }
        public byte BalanceFloorInfoByte39 { get; set; }
        public byte FloorItemSetIndex { get; set; }
        public byte KecleonShopItemSetIndex { get; set; }
        public byte PossibleItemSetIndex3C { get; set; }
        public byte NormalTreasureBoxItemSetIndex { get; set; }
        public byte MonsterHouseItemSetIndex { get; set; }
        public byte DeluxeTreasureBoxItemSetIndex { get; set; }
        public byte BalanceFloorInfoByte40 { get; set; }
        public byte BalanceFloorInfoByte41 { get; set; }
        public byte MinItemDensity { get; set; }
        public byte MaxItemDensity { get; set; }
        public byte BuriedItemSetIndex { get; set; }
        public byte MaxBuriedItems { get; set; }
        public byte BalanceFloorInfoByte46 { get; set; }
        public byte BalanceFloorInfoByte47 { get; set; }
        public byte KecleonShopChance { get; set; }
        public byte BalanceFloorInfoByte49 { get; set; }
        public byte BalanceFloorInfoByte4A { get; set; }
        public byte MinTrapDensity { get; set; }
        public byte MaxTrapDensity { get; set; }
        public byte MinEnemyDensity { get; set; }
        public byte MaxEnemyDensity { get; set; }
        public byte BalanceFloorInfoByte4F { get; set; }
        public byte BalanceFloorInfoByte50 { get; set; }
        public byte BalanceFloorInfoByte51 { get; set; }
        public byte MysteryHouseChance { get; set; }
        public byte MysteryHouseSize { get; set; }
        public byte InvitationIndex { get; set; }
        public byte MonsterHouseChance { get; set; }
        public byte BalanceFloorInfoByte56 { get; set; }
        public byte BalanceFloorInfoByte57 { get; set; }
        public byte BalanceFloorInfoByte58 { get; set; }
        public DungeonStatusIndex Weather { get; set; }
        public byte[] BalanceFloorInfoBytes5Ato61 { get; set; } = new byte[0];

        // From request_level.bin
        public bool IsBossFloor { get; set; }

        // From dungeon_balance.bin
        public Dictionary<ItemIndex, short>? TrapWeights { get; set; }
        public List<DungeonPokemonSpawnModel>? Spawns { get; set; }

        #region Obsolete fields
        [Obsolete($"Renamed to {nameof(TurnLimit)}"), DeserializeOnly]
        public short BalanceFloorInfoShort24 { get => TurnLimit; set => TurnLimit = value; }
        [Obsolete($"Renamed to {nameof(MinMoneyStackSize)}"), DeserializeOnly]
        public short BalanceFloorInfoShort26 { get => MinMoneyStackSize; set => MinMoneyStackSize = value; }
        [Obsolete($"Renamed to {nameof(MaxMoneyStackSize)}"), DeserializeOnly]
        public short BalanceFloorInfoShort28 { get => MaxMoneyStackSize; set => MaxMoneyStackSize = value; }
        [Obsolete($"Renamed to {nameof(RoomCount)}"), DeserializeOnly]
        public byte UnknownItemSetIndex { get => RoomCount; set => RoomCount = value; }
        [Obsolete($"Replaced with {nameof(BalanceFloorInfoByte37)} and several others"), DeserializeOnly]
        public byte[] BalanceFloorInfoBytes37to53 { get => BalanceFloorInfoBytes37to43; set => BalanceFloorInfoBytes37to43 = value; }
        [Obsolete($"Replaced with {nameof(BalanceFloorInfoByte37)} and several others"), DeserializeOnly]
        public byte[] BalanceFloorInfoBytes37to43
        {
            get => Array.Empty<byte>();
            set
            {
                BalanceFloorInfoByte37 = value[0x37 - 0x37];
                BalanceFloorInfoByte38 = value[0x38 - 0x37];
                BalanceFloorInfoByte39 = value[0x39 - 0x37];
                FloorItemSetIndex = value[0x3A - 0x37];
                KecleonShopItemSetIndex = value[0x3B - 0x37];
                PossibleItemSetIndex3C = value[0x3C - 0x37];
                NormalTreasureBoxItemSetIndex = value[0x3D - 0x37];
                MonsterHouseItemSetIndex = value[0x3E - 0x37];
                DeluxeTreasureBoxItemSetIndex = value[0x3F - 0x37];
                BalanceFloorInfoByte40 = value[0x40 - 0x37];
                BalanceFloorInfoByte41 = value[0x41 - 0x37];
                MinItemDensity = value[0x42 - 0x37];
                MaxItemDensity = value[0x43 - 0x37];
                BuriedItemSetIndex = value[0x44 - 0x37];
                MaxBuriedItems = value[0x45 - 0x37];
                BalanceFloorInfoByte46 = value[0x46 - 0x37];
                BalanceFloorInfoByte47 = value[0x47 - 0x37];
                KecleonShopChance = value[0x48 - 0x37];
                BalanceFloorInfoByte49 = value[0x49 - 0x37];
                BalanceFloorInfoByte4A = value[0x4A - 0x37];
                MinTrapDensity = value[0x4B - 0x37];
                MaxTrapDensity = value[0x4C - 0x37];
                MinEnemyDensity = value[0x4D - 0x37];
                MaxEnemyDensity = value[0x4E - 0x37];
                BalanceFloorInfoByte4F = value[0x4F - 0x37];
                BalanceFloorInfoByte50 = value[0x50 - 0x37];
                BalanceFloorInfoByte51 = value[0x51 - 0x37];
                MysteryHouseChance = value[0x52 - 0x37];
                MysteryHouseSize = value[0x53 - 0x37];
            }
        }
        [Obsolete($"Renamed to {nameof(MonsterHouseChance)}"), DeserializeOnly]
        public byte BalanceFloorInfoByte55 { get => MonsterHouseChance; set => MonsterHouseChance = value; }
        #endregion
    }
}
