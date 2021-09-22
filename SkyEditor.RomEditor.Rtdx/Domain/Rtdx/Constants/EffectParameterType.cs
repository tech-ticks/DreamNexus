using System;

namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public enum EffectParameterType : ushort
    {
        None = 0,
        EffectChance = 1,
        CriticalHitRatio = 2,
        // 3 is unused
        Unknown3 = 3,
        RecoilPercentOfMaxHP = 4,
        HPPercent = 5,
        // TODO: investigate 6  (related to warping teammates around a target, seen with values 0 or 1)
        Unknown6 = 6,
        ChanceToApplyFurtherEffects = 7,
        MinDamageLevelFactor = 8,
        MinVisitsDamageMultiplier = 9,
        DamageMultiplierAtMinimumPP = 10,
        DamageMultiplierAtMinimumHP = 11,
        DamageMultiplier = 12,
        DamageMultiplierWithOneDepletedMove = 13,
        // 14 is unused
        Unknown14 = 14,
        FixedDamage = 15,
        StockpileCount = 16,
        SpendPercentOfMaxHP = 17,
        // TODO: investigate 18  (related to Thief and Covet, seen with value 15 on both)
        Unknown18 = 18,
        SelectAttackerOrTargetStatBoosts = 19,
        HealPercentOfDamageDealt = 20,
        // 21 is unused
        HealPercentOfMaxHP = 22,
        BellyAmount = 23,
        ExcludeFloating = 24,
        // TODO: investigate 25  (used by effect 158 in a dummied out item -- action 807)
        Unknown25 = 25,
        PPAmount = 26,
        HPAmount = 27,
        LevelAmount = 28,
        PowerAmount = 29,
        AccuracyAmount = 30,
        DigTileCount = 31,
        MinMonsterCount = 32,
        // TODO: investigate 33  (related to Stealth Rock and dummied out Powder move)
        Unknown33 = 33,
        SparklingFloorEmpty = 34,
        ExplosionSize = 35,
        // TODO: investigate 36
        Unknown36 = 36,
        MinItemsToDrop = 37,
        HealPercentOfMaxHPInSunnyWeather = 38,
        RemoveStatusOnHit = 39,
        // TODO: investigate 40  (related to warping teammates around a target, seen with values 0 or 3)
        Unknown40 = 40,
        MaxDamageLevelFactor = 41,
        MaxVisitsDamageMultiplier = 42,
        DamageMultiplierAtMaximumPP = 43,
        DamageMultiplierAtMaximumHP = 44,
        DamageMultiplierWithTwoDepletedMoves = 45,
        MaxBellyAmount = 46,
        MaxHPAmount = 47,
        MaxMonsterCount = 48,
        // TODO: investigate 49  (related to level changes, always seen with value 10)
        Unknown49 = 49,
        // TODO: investigate 50  (related to status effects, seen with values 0 or 1)
        Unknown50 = 50,
        RecruitRateBoost = 51,
        MaxItemsToDrop = 52,
        HealPercentOfMaxHPInBadWeather = 53,
        PercentOfMaxHPThreshold = 54,
        MaxDungeonsVisited = 55,
        // TODO: investigate 56  (related to status effects, seen with values 0 or 1)
        Unknown56 = 56,
        PPThreshold = 57,
        DamageMultiplierWithThreeDepletedMoves = 58,
        HPThreshold = 59,
        // TODO: investigate 60  (related to warping teammates around a target or applying status effects, seen with values 0 or 1)
        Unknown60 = 60,
        // 61 is unused
        Unknown61 = 61,
        StatusEffect = 62,
        StatMultiplierIndex = 63,
        StatChangeIndex = 64,
        // 65 is unused
        Unknown65 = 65,
        StatIndex = 66,
        PokemonType = 67,
        CheckDungeonStatusEffect = 68,
        SetDungeonStatusEffect = 69, // TODO: investigate Rare Quality Orb; parameter type is zero, but corresponding value is correct,
        Max = 70
    }

    public static class EffectParameterTypeExtensions
    {
        public static string GetDescription(this EffectParameterType type)
        {
            switch (type)
            {
                case EffectParameterType.None: return "Unspecified";
                case EffectParameterType.EffectChance: return "Chance to execute effect";
                case EffectParameterType.CriticalHitRatio: return "Critical hit ratio";
                case EffectParameterType.RecoilPercentOfMaxHP: return "Percentage of max HP as recoil damage";
                case EffectParameterType.HPPercent: return "Percentage of HP";
                case EffectParameterType.ChanceToApplyFurtherEffects: return "Chance to apply further effects";
                case EffectParameterType.MinDamageLevelFactor: return "Minimum damage equal to percentage of attacker's level";
                case EffectParameterType.MinVisitsDamageMultiplier: return "Damage multiplier based on minimum dungeons visited";
                case EffectParameterType.DamageMultiplierAtMinimumPP: return "Damage multiplier at minimum PP";
                case EffectParameterType.DamageMultiplierAtMinimumHP: return "Damage multiplier at minimum HP";
                case EffectParameterType.DamageMultiplier: return "Damage multiplier";
                case EffectParameterType.DamageMultiplierWithOneDepletedMove: return "Damage multiplier with one depleted move";
                case EffectParameterType.FixedDamage: return "Fixed damage";
                case EffectParameterType.StockpileCount: return "Stockpile count";
                case EffectParameterType.SpendPercentOfMaxHP: return "Spend percentage of max HP";
                case EffectParameterType.SelectAttackerOrTargetStatBoosts: return "Use attacker's or target's stat boosts";
                case EffectParameterType.HealPercentOfDamageDealt: return "Heal for percentage of damage dealt";
                case EffectParameterType.HealPercentOfMaxHP: return "Heal percentage of max HP";
                case EffectParameterType.BellyAmount: return "Belly amount";
                case EffectParameterType.ExcludeFloating: return "Exclude floating targets?";
                case EffectParameterType.PPAmount: return "PP Amount";
                case EffectParameterType.HPAmount: return "HP Amount";
                case EffectParameterType.LevelAmount: return "Level(s)";
                case EffectParameterType.PowerAmount: return "Power";
                case EffectParameterType.AccuracyAmount: return "Accuracy";
                case EffectParameterType.DigTileCount: return "Number of tiles digged";
                case EffectParameterType.MinMonsterCount: return "Minimum number of monsters";
                case EffectParameterType.SparklingFloorEmpty: return "Sparkling floor is empty?";
                case EffectParameterType.ExplosionSize: return "Large explosion?";
                case EffectParameterType.MinItemsToDrop: return "Minimum number of dropped items";
                case EffectParameterType.HealPercentOfMaxHPInSunnyWeather: return "Heal percentage of max HP in sunny weather";
                case EffectParameterType.RemoveStatusOnHit: return "Remove status effect on hit?";
                case EffectParameterType.MaxDamageLevelFactor: return "Maximum damage equal to percentage of attacker's level";
                case EffectParameterType.MaxVisitsDamageMultiplier: return "Damage multiplier based on maximum dungeons visited";
                case EffectParameterType.DamageMultiplierAtMaximumPP: return "Damage multiplier at maximum PP";
                case EffectParameterType.DamageMultiplierAtMaximumHP: return "Damage multiplier at maximum HP";
                case EffectParameterType.DamageMultiplierWithTwoDepletedMoves: return "Damage multiplier with two depleted moves";
                case EffectParameterType.MaxBellyAmount: return "Max Belly";
                case EffectParameterType.MaxHPAmount: return "Max HP";
                case EffectParameterType.MaxMonsterCount: return "Maximum number of monsters";
                case EffectParameterType.RecruitRateBoost: return "Recruit rate boost";
                case EffectParameterType.MaxItemsToDrop: return "Maximum number of dropped items";
                case EffectParameterType.HealPercentOfMaxHPInBadWeather: return "Heal percentage of max HP in bad weather";
                case EffectParameterType.PercentOfMaxHPThreshold: return "Percentage of max HP";
                case EffectParameterType.MaxDungeonsVisited: return "Maximum number of visited dungeons";
                case EffectParameterType.PPThreshold: return "PP threshold";
                case EffectParameterType.DamageMultiplierWithThreeDepletedMoves: return "Damage multiplier with three depleted moves";
                case EffectParameterType.HPThreshold: return "HP threshold";
                case EffectParameterType.StatusEffect: return "Status effect";
                case EffectParameterType.StatMultiplierIndex: return "Stat change multiplier index";
                case EffectParameterType.StatChangeIndex: return "Stat change index";
                case EffectParameterType.StatIndex: return "Stat index";
                case EffectParameterType.PokemonType: return "Pokémon type";
                case EffectParameterType.CheckDungeonStatusEffect: return "Check dungeon status effect";
                case EffectParameterType.SetDungeonStatusEffect: return "Set dungeon status effect";
                default: return $"(unknown {type})";
            }
        }

        public static Type GetDisplayType(this EffectParameterType type)
        {
            switch (type)
            {
                case EffectParameterType.ExcludeFloating: return typeof(bool);
                case EffectParameterType.SparklingFloorEmpty: return typeof(bool);
                case EffectParameterType.ExplosionSize: return typeof(bool);
                case EffectParameterType.RemoveStatusOnHit: return typeof(bool);
                case EffectParameterType.PokemonType: return typeof(PokemonType);
                case EffectParameterType.StatusEffect: return typeof(StatusIndex);
                case EffectParameterType.CheckDungeonStatusEffect: return typeof(DungeonStatusIndex);
                case EffectParameterType.SetDungeonStatusEffect: return typeof(DungeonStatusIndex);
                default: return typeof(ushort);
            }
        }
    }
}
