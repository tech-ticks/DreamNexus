namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public enum EffectParameterType : ushort
    {
        // No parameter
        None = 0,

        // Effect chance
        EffectChance = 1,

        // Critical hit ratio
        CriticalHitRatio = 2,

        // TODO: investigate 3

        PercentOfMaxHP = 4,

        // TODO: investigate 5 through 11

        DamageMultiplier = 12,

        // TODO: investigate 13 through 14

        FixedDamage = 15,

        // TODO: investigate 16 through 61

        StatusEffect = 62,

        // TODO: investigate 63

        StatChangeIndex = 64,

        // TODO: investigate 65 through 69
    }
}
