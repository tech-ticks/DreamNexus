namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public enum EffectParameterType : ushort
    {
        None = 0,
        EffectChance = 1,
        CriticalHitRatio = 2,
        // 3 is unused
        RecoilPercentOfMaxHP = 4,
        DamagePercentOfCurrentHP = 5,
        // TODO: investigate 6  (related to warping teammates around a target, seen with values 0 or 1)
        ChanceToApplyFurtherEffects = 7,
        MinDamageLevelFactor = 8,
        MinVisitsDamageMultiplier = 9,
        DamageMultiplierAtMinimumPP = 10,
        DamageMultiplierAtMinimumHP = 11,
        DamageMultiplier = 12,
        DamageMultiplierWithOneDepletedMove = 13,
        // 14 is unused
        FixedDamage = 15,
        StockpileCount = 16,
        SpendPercentOfMaxHP = 17,
        // TODO: investigate 18  (related to Thief and Covet, seen with value 15 on both)
        SelectAttackerOrTargetStatBoosts = 19,
        HealPercentOfDamageDealt = 20,
        // 21 is unused
        HealPercentOfMaxHP = 22,
        BellyAmount = 23,
        ExcludeFloating = 24,
        // TODO: investigate 25  (used by effect 158 in a dummied out item -- action 807)
        PPAmount = 26,
        HPAmount = 27,
        LevelAmount = 28,
        PowerAmount = 29,
        AccuracyAmount = 30,
        DigTileCount = 31,
        MinMonsterCount = 32,
        // TODO: investigate 33  (related to Stealth Rock and dummied out Powder move)
        SparklingFloorEmpty = 34,
        ExplosionSize = 35,
        // TODO: investigate 36
        MinItemsToDrop = 37,
        HealPercentOfMaxHPInSunnyWeather = 38,
        RemoveStatusOnHit = 39,
        // TODO: investigate 40  (related to warping teammates around a target, seen with values 0 or 3)
        MaxDamageLevelFactor = 41,
        MaxVisitsDamageMultiplier = 42,
        DamageMultiplierAtMaximumPP = 43,
        DamageMultiplierAtMaximumHP = 44,
        DamageMultiplierWithTwoDepletedMoves = 45,
        MaxBellyAmount = 46,
        MaxHPAmount = 47,
        MaxMonsterCount = 48,
        // TODO: investigate 49  (related to level changes, always seen with value 10)
        // TODO: investigate 50  (related to status effects, seen with values 0 or 1)
        RecruitRateBoost = 51,
        MaxItemsToDrop = 52,
        HealPercentOfMaxHPInBadWeather = 53,
        PercentOfMaxHPThreshold = 54,
        MaxDungeonsVisited = 55,
        // TODO: investigate 56  (related to status effects, seen with values 0 or 1)
        PPThreshold = 57,
        DamageMultiplierWithThreeDepletedMoves = 58,
        HPThreshold = 59,
        // TODO: investigate 60  (related to warping teammates around a target or applying status effects, seen with values 0 or 1)
        // 61 is unused
        StatusEffect = 62,
        StatMultiplierIndex = 63,
        StatChangeIndex = 64,
        // 65 is unused
        StatIndex = 66,
        PokemonType = 67,
        CheckDungeonStatusEffect = 68,
        SetDungeonStatusEffect = 69, // TODO: investigate Rare Quality Orb; parameter type is zero, but corresponding value is correct
    }
}
