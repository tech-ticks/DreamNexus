using System;
using ItemKindStrings = SkyEditor.RomEditor.Resources.Strings.ItemKind;

namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public enum ItemKind : byte
    {
        NONE = 0,
        ARROW = 1,
        STONE = 2,
        EQUIP = 3,
        FOOD = 4,
        SEED = 5,
        PP = 6,
        PARAMETER = 7,
        ORB = 8,
        WAND = 9,
        EVOLUTION = 10,
        OTHER = 11,
        MONEY = 12,
        BOX = 13,
        CHEST = 14,
        WAZAMACHINE = 15,
        BROKENMACHINE = 16,
        TRAP = 17,
        MAX = 18
    }

    public static class ItemKindExtensions
    {
        public static string GetFriendlyName(this ItemKind itemKind)
        {
            return itemKind switch
            {
                ItemKind.NONE => ItemKindStrings.None,
                ItemKind.ARROW => ItemKindStrings.Arrow,
                ItemKind.STONE => ItemKindStrings.Stone,
                ItemKind.EQUIP => ItemKindStrings.Equip,
                ItemKind.FOOD => ItemKindStrings.Food,
                ItemKind.SEED => ItemKindStrings.Seed,
                ItemKind.PP => ItemKindStrings.PP,
                ItemKind.PARAMETER => ItemKindStrings.Parameter,
                ItemKind.ORB => ItemKindStrings.Orb,
                ItemKind.WAND => ItemKindStrings.Wand,
                ItemKind.EVOLUTION => ItemKindStrings.Evolution,
                ItemKind.OTHER => ItemKindStrings.Other,
                ItemKind.MONEY => ItemKindStrings.Money,
                ItemKind.BOX => ItemKindStrings.Box,
                ItemKind.CHEST => ItemKindStrings.Chest,
                ItemKind.WAZAMACHINE => ItemKindStrings.Wazamachine,
                ItemKind.BROKENMACHINE => ItemKindStrings.Brokenmachine,
                ItemKind.TRAP => ItemKindStrings.Trap,
                ItemKind.MAX => ItemKindStrings.Max,
                _ => throw new ArgumentOutOfRangeException(nameof(itemKind), itemKind, null)
            };
        }
    }
}
