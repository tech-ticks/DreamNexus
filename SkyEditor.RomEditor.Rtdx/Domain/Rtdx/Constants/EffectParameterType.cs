namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public enum EffectParameterType : ushort
    {
        // No parameter
        None = 0,

        EffectChance = 1,
        CriticalHitRatio = 2,

        // TODO: investigate 3

        RecoilPercentOfMaxHP = 4,
        DamagePercentOfCurrentHP = 5,
        
        // TODO: investigate 6
        // TODO: investigate 7

        MinDamageLevelFactor = 8,
        MinVisitsDamageMultiplier = 9,

        // TODO: investigate 10
        
        DamageMultiplierAtMinimumHP = 11,
        DamageMultiplier = 12,

        // TODO: investigate 13 through 14

        FixedDamage = 15,
        StockpileCount = 16,
        SpendPercentOfMaxHP = 17,

        // TODO: investigate 18
        // TODO: investigate 19

        HealPercentOfDamageDealt = 20,
        
        // TODO: investigate 21
        
        HealPercentOfMaxHP = 22,
        SetBellyAmount = 23,
        
        // TODO: investigate 24
        // TODO: investigate 25

        PPAmount = 26,

        // TODO: investigate 27 through 30

        DigTileCount = 31,

        // TODO: investigate 32 through 37

        HealPercentOfMaxHPInSunnyWeather = 38,
        RemoveStatusOnHit = 39,

        // TODO: investigate 40

        MaxDamageLevelFactor = 41,
        MaxVisitsDamageMultiplier = 42,

        // TODO: investigate 43

        DamageMultiplierAtMaximumHP = 44,

        // TODO: investigate 45 through 50

        RecruitRateBoost = 51,

        // TODO: investigate 52
        
        HealPercentOfMaxHPInBadWeather = 53,
        
        // TODO: investigate 54

        MaxDungeonsVisited = 55,

        // TODO: investigate 56 through 61

        StatusEffect = 62,
        StatMultiplierIndex = 63,
        StatChangeIndex = 64,

        // TODO: investigate 65 through 67

        CheckDungeonStatusEffect = 68,
        SetDungeonStatusEffect = 69,
    }
}
