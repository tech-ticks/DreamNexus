namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public enum EffectParameterType : ushort
    {
        None = 0,
        EffectChance = 1,
        CriticalHitRatio = 2,
        // TODO: investigate 3
        RecoilPercentOfMaxHP = 4,
        DamagePercentOfCurrentHP = 5,
        // TODO: investigate 6  (related to warping teammates)
        // TODO: investigate 7
        MinDamageLevelFactor = 8,
        MinVisitsDamageMultiplier = 9,
        DamageMultiplierAtMinimumPP = 10,
        DamageMultiplierAtMinimumHP = 11,
        DamageMultiplier = 12,
        DamageMultiplierWithOneDepletedMove = 13,
        // TODO: investigate 14
        FixedDamage = 15,
        StockpileCount = 16,
        SpendPercentOfMaxHP = 17,
        // TODO: investigate 18
        SelectAttackerOrTargetStatBoosts = 19,
        HealPercentOfDamageDealt = 20,
        // TODO: investigate 21
        HealPercentOfMaxHP = 22,
        BellyAmount = 23,
        ExcludeFloating = 24,
        // TODO: investigate 25
        PPAmount = 26,
        HPAmount = 27,
        LevelAmount = 28,
        PowerAmount = 29,
        AccuracyAmount = 30,
        DigTileCount = 31,
        MinMonsterCount = 32,
        // TODO: investigate 33
        SparklingFloorEmpty = 34,
        ExplosionSize = 35,
        // TODO: investigate 36
        MinItemsToDrop = 37,
        HealPercentOfMaxHPInSunnyWeather = 38,
        RemoveStatusOnHit = 39,
        // TODO: investigate 40  (related to warping teammates)
        MaxDamageLevelFactor = 41,
        MaxVisitsDamageMultiplier = 42,
        DamageMultiplierAtMaximumPP = 43,
        DamageMultiplierAtMaximumHP = 44,
        DamageMultiplierWithTwoDepletedMoves = 45,
        MaxBellyAmount = 46,
        MaxHPAmount = 47,
        MaxMonsterCount = 48,
        // TODO: investigate 49  (related to level increase/decrease)
        // TODO: investigate 50  (related to status effects)
        RecruitRateBoost = 51,
        MaxItemsToDrop = 52,
        HealPercentOfMaxHPInBadWeather = 53,
        PercentOfMaxHPThreshold = 54,
        MaxDungeonsVisited = 55,
        // TODO: investigate 56  (related to status effects)
        PPThreshold = 57,
        DamageMultiplierWithThreeDepletedMoves = 58,
        HPThreshold = 59,
        // TODO: investigate 60  (related to warping teammates)
        // TODO: investigate 61
        StatusEffect = 62,
        StatMultiplierIndex = 63,
        StatChangeIndex = 64,
        // TODO: investigate 65
        StatIndex = 66,
        PokemonType = 67,
        CheckDungeonStatusEffect = 68,
        SetDungeonStatusEffect = 69,
    }
}
