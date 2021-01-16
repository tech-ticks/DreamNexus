namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public enum ActionType : ushort
    {
        // No action
        None = 0,

        // 1 is unused

        // Deal random basic attack damage
        BasicAttackDamage = 2,

        // Deal random typed damage
        //   Params[0] = (percentage) Critical hit ratio
        Damage = 3,

        // 4 is unused

        // Deal random typed damage using Defense instead of Special Defense
        //   Params[0] = (percentage) Critical hit ratio
        DamageUsingDefense = 5,

        // Deal random typed damage ignoring target's Defense and Evasion stat changes
        //   Params[0] = (percentage) Critical hit ratio
        DamageIgnoringDefenseAndEvasion = 6,

        // Remove target's Reflect/Light Screen and deal random typed damage
        //   Params[0] = (percentage) Critical hit ratio
        DamageAndBreakDefenses = 7,

        // Deal random typed damage; double damage if target is afflicted with an specific status
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier
        //   Params[2] = (boolean) Remove status on hit
        //   Params[3] = (boolean) ???  (either 0 or 1)
        //   Params[4] = (StatusIndex) Status to check
        DamageWithBoostIfTargetHasStatus = 8,

        // Deal random typed damage; damage is increased the faster the attacker is compared to the target (based on the Speed stat)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Maximum damage multiplier
        DamageWithBoostIfFaster = 9,

        // Deal random typed damage; damage is increased the slower the attacker is compared to the target (based on the Speed stat)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Maximum damage multiplier
        DamageWithBoostIfSlower = 10,

        // Deal random typed damage; damage is increased if not holding an item
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier
        DamageWithBoostIfNotHoldingItem = 11,

        // Deal random typed damage; damage is modified based on attacker's HP
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier at minimum HP
        //   Params[2] = (percentage) Damage multiplier at maximum HP
        DamageWithBoostBasedOnAttackersHP = 12,

        // Deal random typed damage; damage is modified based on target's HP
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier at minimum HP
        //   Params[2] = (percentage) Damage multiplier at maximum HP
        DamageWithBoostBasedOnTargetsHP = 13,

        // Deal fixed typed damage based on how many dungeons the attacker was taken into
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier at zero visits
        //   Params[2] = (percentage) Damage multiplier at maximum visits (count unknown)
        //   Params[3] = Base damage value?
        DamageWithBoostBasedOnDungeonsVisited = 14,

        // 15 = Deal random typed damage; damage is increased the less PP the move has (Trump Card)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier at zero PP
        //   Params[2] = (percentage) Damage multiplier at PP threshold
        //   Params[3] = PP threshold
        DamageWithBoostBasedOnPP = 15,

        // Deal random typed damage; damage is increased based on how many of the user's moves have zero PP (Last Resort)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier with one move depleted
        //   Params[2] = (percentage) Damage multiplier with two moves depleted
        //   Params[3] = (percentage) Damage multiplier with three moves depleted
        DamageWithBoostBasedOnDepletedMoves = 16,

        // Deal random typed damage; damage is increased when the move is used consecutively (Fury Cutter)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier when used consecutively
        DamageWithBoostOnConsecutiveUses = 17,

        // Deal random typed damage; damage is increased when the move was used by a teammate in the same turn (Echoed Voice)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier when used after a teammate also used the move
        DamageWithBoostWhenTeammateUsedTheMoveInTheSameTurn = 18,

        // Deal random typed damage; damage is increased when the move was used by a teammate in the same turn and in the same room (Round)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier when used after a teammate also used the move in the same room
        DamageWithBoostWhenTeammateUsedTheMoveInTheSameTurnInTheSameRoom = 19,

        // Deal random typed damage; damage is increased if the target is holding an item (Knock Off)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier if the target was holding an item
        DamageWithBoostIfTargetHoldsItem = 20,

        // Deal random typed damage; damage is increased if the target is afflicted with any status
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier if the target is afflicted with any status
        DamageWithBoostIfTargetHasAnyStatus = 21,

        // Deal random typed damage; damage is increased if the user is afflicted with Burn or Poison
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier if the target is afflicted with Burn or Poison
        DamageWithBoostIfTargetHasBurnOrPoison = 22,

        // Deal random typed damage; damage is increased if the target's HP is below the threshold
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier if the target is at or below the HP threshold
        //   Params[2] = (unknown)
        //   Params[3] = (percentage) HP threshold
        DamageWithBoostIfTargetsHPIsAtOrBelowThreshold = 23,

        // Randomly deal fixed 25, 50 or 75 damage or heal target by 25% of their max HP (Present)
        //   Params[0] = (percentage) Critical hit ratio
        PresentDamageOrHeal = 24,

        // Deal damage based on a "magnitude" (Magnitude)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier at maximum threshold
        MagnitudeDamage = 25,

        // Deal random typed damage; type is based on the held item (Natural Gift)
        //   Params[0] = (percentage) Critical hit ratio
        DamageWithTypeBasedOnHeldItem = 26,

        // Deal random typed damage; super-effective against Water types (Freeze-Dry)
        //   Params[0] = (percentage) Critical hit ratio
        DamageAlwaysSuperEffectiveAgainstWater = 27,

        // Deal fixed damage
        //   Params[0] = (percentage) Critical hit ratio?
        //   Params[1] = (integer) Damage amount
        FixedDamage = 28,

        // Deal damage equal to a percentage of current target's HP
        //   Params[0] = (percentage) Critical hit ratio?
        //   Params[1] = (percentage) Percentage of current target's HP
        PercentageDamage = 29,

        // Reduce target's HP to user's HP if the user has less HP than the target (Endeavor)
        EqualizeHP = 30,

        // Deal damage equal to the user's HP minus 1 (Final Gambit)
        DamageEqualToUsersHPMinusOne = 31,

        // Deal damage based on the attacker's level
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Minimum damage (equal to the attacker's level multiplied by this factor)
        //   Params[2] = (percentage) Maximum damage (equal to the attacker's level multiplied by this factor)
        DamageBasedOnAttackersLevel = 32,

        // Deal random typed damage using the target's Attack stat
        //   Params[0] = (percentage) Critical hit ratio
        DamageUsingTargetsAttack = 33,

        // Deal typed damage based on the target's weight; damage is increased the heavier the target is
        //   Params[0] = (percentage) Critical hit ratio
        DamageBasedOnTargetsWeight = 34,

        // Deal random typed damage; damage is increased the based on how lighter the target is compared to the attacker
        //   Params[0] = (percentage) Critical hit ratio
        DamageBasedOnAttackerAndTargetWeights = 35,

        // Deal random typed damage and cause an explosion (Hyper Beam)
        //   Params[0] = (percentage) Critical hit ratio
        DamageAndExplode = 36,

        // Deal random typed damage; enemy drops Poké if defeated (Pay Day)
        //   Params[0] = (percentage) Critical hit ratio
        DamageAndDropMoney = 37,

        // Deal random typed damage; raises user's Attack stat if target is defeated (Fell Stinger)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Chance to increase Attack stat?
        DamageAndRaiseAttackOnDefeat = 38,

        // Deal random typed damage if the target shares a type with the user (Synchronoise)
        //   Params[0] = (percentage) Critical hit ratio
        DamageWithBoostIfTargetSharesTypeWithAttacker = 39,

        // Deal random typed damage; damage is boosted based on the number of positive stat changes of the attacker or target
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (enum) 0 = Attacker's stat boosts; 1 = Target's stat boosts
        DamageWithBoostBasedOnPositiveStatChanges = 40,

        // Charged move
        ChargedMove = 41,

        // Deal damage stored by Bide/Revenge/Avalanche
        //   Params[0] = (percentage) Critical hit ratio?
        //   Params[1] = (percentage) Damage multiplier
        DealStoredDamage = 42,

        // Grab target and fly; apply Sky Drop status (first turn of Sky Drop)
        SkyDropFirstTurn = 43,

        // Drop target and deal damage (second turn of Sky Drop)
        //   Params[0] = (percentage) Critical hit ratio
        SkyDropSecondTurn = 44,

        // Take recoil damage
        //   Params[0] = (unknown)
        //   Params[1] = (percentage) Damage taken as a percentage of HP
        RecoilDamage = 45,

        // Struggle recoil damage
        //   Params[0] = (unknown)
        //   Params[1] = (percentage) Damage taken as a percentage of HP
        StruggleRecoilDamage = 46,

        // Take recoil damage on miss
        //   Params[0] = (unknown)
        //   Params[1] = (percentage) Damage taken as a percentage of HP
        RecoilDamageOnMiss = 47,

        // Reduce user's HP to 1 (Final Gambit)
        ReduceHPToOne = 48,

        // Heal self for % of damage dealt
        //   Params[0] = (unknown)
        //   Params[1] = (percentage) Amount healed as a percentage of damage dealt
        HealSelfBasedOnDamageDealt = 49,

        // One-hit KO
        OneHitKO = 50,

        // Clear stat changes
        //   Params[0] = (percentage) Chance to clear all stat changes
        ClearStatChanges = 51,

        // Apply stat changes
        //   Params[0] = (percentage) Chance to apply stat changes
        //   Params[5] = (index) Index into ActStatusTableDataInfo for the stat changes to apply
        // Bit 10 of action flags indicate if the target is user (when set) or target (when clear)
        ApplyStatChanges = 52,

        // Apply stat changes
        //   Params[0] = (percentage) Chance to apply stat changes
        //   Params[5] = (index) Index into ActStatusTableDataInfo for the stat changes to apply
        //   Params[7] = (DungeonStatusIndex) Dungeon status effect under which the stat boosts are doubled
        // Bit 10 of action flags indicate if the target is user (when set) or target (when clear)
        ApplyStatChangesWithWeatherBoost = 53,

        // Modify attack/defense stats (Screech, Charm, Aurora Beam, Memento)
        //   Params[0] = (percentage) Chance to apply stat changes
        //   Params[5] = (index) Index into ActStatusTableDataInfo for the stat changes to apply
        // Bit 10 of action flags indicate if the target is user (when set) or target (when clear)
        ModifyStats = 54,

        // Reverse stat changes (making positive changes negative and vice-versa) (Topsy-Turvy)
        ReverseStatChanges = 55,
        
        // Swap raw Attack stat with raw Defense state (Power Trick)
        SwapAttackWithDefense = 56,
        
        // Copy the target's stat changes (Psych Up)
        CopyTargetStatChanges = 57,
        
        // Raise a random stat by two stages (Acupressure)
        //   Params[0] = (percentage) Chance to apply stat change?
        //   Params[5] = (unknown)
        RaiseRandomStat = 58,

        // Lower belly to the specified value and maximize Attack boost (Belly Drum)
        //   Params[1] = (integer) Amount of belly to reduce to; if current belly is below this value, the action fails
        ReduceBellyAndMaximizeAttack = 59,

        // Reset positive Evasion stat changes to zero
        ResetPositiveEvasion = 60,
        
        // Swap Attack and Special Attack boosts and multipliers between attacker and target (Power Swap)
        SwapAttackAndSpAttackBoosts = 61,

        // Swap Defense and Special Defense boosts and multipliers between attacker and target (Guard Swap)
        SwapDefenseAndSpDefenseBoosts = 62,

        // Swap all stat boosts and multipliers (except Speed) between attacker and target (Heart Swap)
        SwapStatBoostsExceptSpeed = 63,

        // Average Attack and Special Attack stats, replacing both attacker's and target's stats (Power Split)
        AverageAttackAndSpecialAttack = 64,

        // Average Defense and Special Defense stats, replacing both attacker's and target's stats (Guard Split)
        AverageDefenseAndSpecialDefense = 65,

        // Apply status effect
        //   Params[0] = (percentage) Chance to apply stat changes
        //   Params[5] = (StatusIndex) Status effect to apply
        // Bit 10 of action flags indicate if the target is user (when set) or target (when clear)
        ApplyStatusEffect = 66,

        // Apply status effect
        //   Params[0] = (percentage) Chance to apply status effect
        //   Params[3] = (unknown)
        //   Params[4] = (StatusIndex) Status effect to apply
        // Bit 10 of action flags indicate if the target is user (when set) or target (when clear)
        ApplyStatusEffect2 = 67,

        // Apply freeze, burn or paralysis (Tri Attack)
        //   Params[0] = (percentage) Chance to apply status effect
        // Bit 10 of action flags indicate if the target is user (when set) or target (when clear)
        ApplyFreezeBurnOrParalysis = 68,

        // Apply status effect based on current location (Secret Power)
        //   Params[0] = (percentage) Chance to apply status effect
        // Bit 10 of action flags indicate if the target is user (when set) or target (when clear)
        ApplyStatusEffectBasedOnLocation = 69,

        // Heal status effect
        //   Params[0] = (percentage) Chance to heal status effect
        //   Params[2] = (boolean) Unknown purpose
        //   Params[3] = (boolean) Unknown purpose
        //   Params[4] = (StatusIndex) Status effect to remove
        // Bit 10 of action flags indicate if the target is user (when set) or target (when clear)
        HealStatusEffect = 70,

        // Heal all negative status effects and remove seals
        HealAllNegativeStatusEffects = 71,
        
        // TODO: investigate 72 through 79

        // Blow away target up to 10 tiles; cause 5 damage on collision
        BlowAwayTarget = 80,

        // TODO: investigate 81 through 179

        // Restore belly
        //   Params[1] = (integer) Amount of belly to restore
        //   Params[2] = (integer) Amount of max belly to increase
        RestoreBelly = 180,

        // Randomly apply Stuck, Poison, Paralysis or Burn (Grimy Food)
        //   Params[0] = (percentage) Effect chance?
        BadFoodStatus = 181,

        // Heal HP or increase max HP if at max (Sitrus Berry)
        //   Params[1] = (integer) Amount of HP to restore
        //   Params[2] = (integer) Amount of max HP to increase
        RestoreHP = 182,

        // TODO: investigate 183 through 189

        // 190 = Increase max HP
        //   Params[2] = (integer) Amount of max HP to increase
        IncreaseMaxHP = 182,

        // TODO: investigate 191 through 221

        // 222 = Heal HP and increase max HP until the threshold; increase max HP further if HP is at max above the threshold
        //   Params[1] = (integer) Amount of HP to restore
        //   Params[2] = (integer) Amount of max HP to increase
        //   Params[3] = (integer) HP threshold
        //                - The max HP will always be increased if below the threshold,
        //                  otherwise max HP will only be increased if user is at full HP
        RestoreAndIncreaseHP = 182,

        // TODO: investigate 223 through 258
    }
}
