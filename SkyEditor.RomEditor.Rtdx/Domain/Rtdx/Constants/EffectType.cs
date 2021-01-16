namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public enum EffectType : ushort
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

        // Deal random typed damage; damage is increased if target is afflicted with an specific status
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

        // Deal damage equal to a percentage of target's current HP
        //   Params[1] = (percentage) Percentage of current HP
        DamagePercentageCurrentHP = 29,

        // Reduce target's HP to user's HP if the user has less HP than the target (Endeavor)
        EqualizeHP = 30,

        // Deal damage equal to a percentage of target's max HP
        //   Params[1] = (percentage) Percentage of max HP
        DamagePercentageMaxHP = 31,

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

        // TODO: confirm this
        // Deal random typed damage; continue executing effects if target is defeated (Fell Stinger)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Chance on defeat to continue executing actions?
        DamageAndContinueActionOnDefeat = 38,

        // Deal random typed damage if the target shares a type with the user (Synchronoise)
        //   Params[0] = (percentage) Critical hit ratio
        DamageIfTargetSharesTypeWithAttacker = 39,

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

        // Take recoil damage from Struggle
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

        // Swap raw Attack stat with raw Defense stat (Power Trick)
        SwapAttackWithDefense = 56,

        // Copy target's stat changes (Psych Up)
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

        // Chance to apply status effect
        //   Params[0] = (percentage) Chance to apply stat changes
        //   Params[5] = (StatusIndex) Status effect to apply
        // Bit 10 of action flags indicate if the target is user (when set) or target (when clear)
        ApplyStatusEffectWithChance = 66,

        // Chance to apply status effect, with an additional unknown parameter
        //   Params[0] = (percentage) Chance to apply status effect
        //   Params[3] = (unknown)
        //   Params[4] = (StatusIndex) Status effect to apply
        // Bit 10 of action flags indicate if the target is user (when set) or target (when clear)
        ApplyStatusEffectWithChanceAndSomethingElse = 67,

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

        // Seal the last move used by the target
        SealLastUsedMove = 72,

        // Seal all moves from enemies that the user also knows
        SealMovesKnownByUser = 73,

        // Transfer all negative status effects to the target and unseals all moves
        TransferNegativeStatusEffectsToTarget = 74,

        // Grant Protect to user and all teammates; duration depends on team size
        AllProtect = 75,

        // TODO: investigate -- used by action 796, which is not used by any moves or items
        //   Params[0] = (percent) Unknown purpose
        Unknown76 = 76,

        // TODO: investigate -- used by action 797, which is not used by any moves or items
        Unknown77 = 77,

        // Throw the target at another Pokémon (Seismic Toss)
        ThrowTargetAtOther = 78,

        // Puts the target to Sleep if not sleeping already.
        // If target is sleeping, applies Terrified on NPCs or Puppet on the player.
        Hypnosis = 79,

        // Blow away target up to 10 tiles; cause 5 damage on collision
        BlowAwayTarget = 80,

        // TODO: investigate -- used by Pounce Wand (237 81 237 238)
        Unknown81 = 81,

        // Warp to a random place on the same floor
        Warp = 82,

        // TODO: investigate -- used by action 769, which is not used by any moves or items
        Unknown83 = 83,

        // Steal target's item (Thief, Covet)
        Steal = 84,

        // Part two of Thief and Covet (unknown effect)
        //   Params[1] = (unknown)  (15 in both moves)
        ThiefCovetPartTwo = 85,

        // Warp and surround target by teleporting the target one square in front of the user and teleporting all teammates around it
        WarpAndSurround = 86,

        // Decrease target's belly. Will not reduce belly below zero.
        //   Params[0] = (percentage) Chance to reduce belly
        //   Params[1] = (integer) Amount of belly to reduce
        DecreaseBelly = 87,

        // Restore HP equal to a percentage of maximum HP
        //   Params[1] = (percentage) Percentage of HP to restore
        RestoreHPPercent = 88,

        // Fully heal HP and remove all negative status conditions
        RestoreFullHPAndHealAllNegativeStatusEffects = 89,

        // Fully heal HP and PP and remove all negative status conditions
        RestoreFullHPAndPPAndHealAllNegativeStatusEffects = 90,

        // Deal damage to creatures surrounding the target (Flame Burst)
        //   Params[1] = (percentage) Damage dealt equal to percentage of max HP
        DealDamageToSurroundingTargets = 91,

        // Apply the Grounded status effect
        //   Params[0] = (percentage) Chance to apply status effect
        ApplyGrounded = 92,

        // Unknown secondary effect applied by Smack Down
        // (perhaps this is used to cancel Fly, Bounce and Sky Drop?)
        SmackDownSecondaryEffect = 93,

        // Jump over the target until the next square not occupied by a creature in a straight line
        SplashBounce = 94,

        // Transform into the target, copying their appearance, types, ability, moves and stat modifications
        Transform = 95,

        // TODO: confirm this
        // Cause all effects of the action to only apply to targets that have the Plus or Minus ability (Magnetic Flux)
        PlusAndMinusSelect = 96,

        // TODO: confirm this
        // Finishes the Magnetic Flux move
        FinishMagneticFlux = 97,

        // TODO: confirm this
        // Cause all effects of the action to only apply to targets of the specified type (Rototiller, Flower Shield)
        //   Params[1] = (boolean) Unknown purpose, (possibly Grounded only (1) or all (0))
        //   Params[6] = (PokemonType) Type of Pokémon to affect
        TypeSelect = 98,

        // TODO: confirm this
        // Finish the TypeSelect action (Flower Shield)
        // (possibly all targets)
        FinishTypeSelect1 = 99,

        // TODO: confirm this
        // Finish the TypeSelect action (Rototiller)
        // (possibly only Grounded targets)
        FinishTypeSelect2 = 100,

        // TODO: confirm this
        // Cause all effects of the action to only apply to targets affected by the specified status effect
        StatusSelect = 101,
        //   Params[3] = (boolean) Unknown purpose
        //   Params[4] = (StatusIndex) Status effect

        // TODO: confirm this
        // Cause all effects of the action to only apply to targets affected by the specified status effect
        StatusSelect2 = 102,
        //   Params[3] = (boolean) Unknown purpose
        //   Params[4] = (StatusIndex) Status effect

        // Spend HP equal to a percentage of max HP
        //   Params[1] = (percentage) Percentage of max HP to spend
        SpendHP = 103,

        // 104 is unused

        // Cause the action to fail if the belly is empty
        CheckBellyNotEmpty = 105,

        // 106 is unused

        // Reduce PP of target's last used move
        //   Params[1] = (integer) Amount of PP to reduce
        ReducePP = 107,

        // Add a type to the target
        //   Params[6] = (PokemonType) Type to add
        AddType = 108,

        // TODO: investigate if there is a hidden parameter to add a second type
        // Change target's type
        //   Params[6] = (PokemonType) Type to change to
        ChangeType = 109,

        // Copy target's type
        CopyType = 110,

        // Set user's type to its first move's type
        ChangeToFirstMoveType = 111,

        // Set user's type to one that has defensive advantage against the target's last used move
        ChangeToTypeWithAdvantageOverLastMove = 112,

        // Set user's type based on the dungeon
        ChangeTypeBasedOnDungeon = 113,

        // TODO: confirm this
        // Remove Flying type temporarily
        RemoveFlyingType = 114,

        // Switch places with the teammate directly behind user
        //   Params[0] = (percentage?) Chance to switch places?
        SwitchWithTeammateBehind = 115,

        // Apply the specified dungeon status
        //   Params[0] = (percentage) Change to apply status
        //   Params[8] = (DungeonStatusIndex) Dungeon status to apply
        ApplyDungeonStatus = 116,

        // Use a random move (Metronome)
        UseRandomMove = 117,

        // Use the last move used by the target (Mirror Move, Copycat)
        UseLastMove = 118,

        // Use a random move from a teammate (Assist)
        UseRandomTeammateMove = 119,

        // Use a random move from the enemy (Me First)
        UseRandomEnemyMove = 120,

        // TODO: confirm this
        // Use a random known move (Sleep Talk)
        UseRandomKnownMove = 121,

        // Use a move based on the current terrain (Nature Power)
        UseMoveBasedOnTerrain = 122,

        // Copy target's last used move (Mimic)
        CopyLastUsedMove = 123,

        // Replace move with target's last used move (Sketch)
        ReplaceWithLastUsedMove = 124,

        // TODO: investigate -- used by actions 674 and 675 (second "versions" of Snore and Sleep Talk)
        Unknown125 = 125,

        // Set a Spikes Trap where the user stands
        SetSpikesTrap = 126,

        // Set a Toxic Spikes Trap where the user stands
        SetToxicSpikesTrap = 127,

        // Set a Stealth Rock Trap where the user stands
        SetStealthRockTrap = 128,

        // Apply status effect
        //   Params[4] = (StatusIndex) Status effect to apply
        // Bit 10 of action flags indicate if the target is user (when set) or target (when clear)
        ApplyStatusEffect = 129,

        // Set user's and target's HP to the average of both HPs. Excess HP above max is lost.
        //   Param[0] = (percentage) Chance to apply effect
        SetHPToAverage = 130,

        // Switch places between user and target
        SwitchPlaces = 131,

        // Teleport all teammates around the target
        //   Params[1] = (boolean?) Unknown purpose
        //   Params[2] = (integer?) Unknown purpose
        //   Params[3] = (boolean?) Unknown purpose
        SurroundTarget = 132,

        // Increase stockpile count
        //   Params[1] = (integer) Amount to increase
        Stockpile = 133,

        // Deal damage based on stockpile count
        //   Params[0] = (percentage) Critical hit ratio
        SpitUp = 134,

        // Heal HP based on stockpile count
        //   Params[1] = (percentage) Percentage of max HP to heal per stockpile count
        Swallow = 135,

        // Reset stockpile count to zero
        ClearStockpile = 136,

        // Set user's HP to a percentage of max HP, regardless of the target
        //   Params[1] = (percentage) Percentage of max HP to set user's current HP to. Will not reduce HP below 1.
        SetUserHPToPercentageOfMaximum = 137,

        // TODO: confirm this
        // Take percentage of max HP as damage if user is Ghost type (Curse)
        //   Params[1] = (percentage) Percentage of max HP to take damage
        TakeDamageIfGhost = 138,

        // Set target's HP to a percentage of current HP
        //   Params[1] = (percentage) Percentage of current HP to set user's current HP to. Will not reduce HP below 1.
        MultiplyHPByPercentage = 139,

        // Set target's HP to a percentage of max HP
        //   Params[1] = (percentage) Percentage of max HP to set user's current HP to. Will not reduce HP below 1.
        SetTargetHPToPercentageOfMaximum = 140,

        // Warp user and set user's HP to one
        WarpAndSetHPToOne = 141,

        // Replace user's Ability with target's Ability (Role Play)
        ReplaceUserAbility = 142,

        // Replace target's Ability with user's Ability (Entrainment)
        ReplaceTargetAbility = 143,

        // Swap user's and target's Abilities (Skill Swap)
        SwapAbility = 144,

        // Replace target's Ability with Insomnia (Worry Seed)
        SetTargetAbilityToInsomnia = 145,

        // Replace target's Ability with Simple (Simple Beam)
        SetTargetAbilityToSimple = 146,

        // TODO: investigate -- used by action 941, which is not used by any moves or items
        Unknown147 = 147,

        // Force target to drop held item
        //   Params[0] = (percentage) Chance to knock item off
        DropItem = 148,

        // Eat target's food
        EatTargetsFood = 149,

        // Convert a Plain Seed into another seed or an Oran Berry
        ConvertPlainSeedToOtherFood = 150,

        // Fling an item
        FlingItem = 151,

        // Destroy target's food
        DestroyTargetsFood = 152,

        // Switch user's and target's held items
        SwitchHeldItems = 153,

        // Give held item to target
        GiveItemToTarget = 154,

        // Make target's held item or a random item in the player's inventory sticky
        //   Params[0] = (percentage) Chance to make item sticky
        MakeItemSticky = 155,

        // Remove sticky status from all items
        CleanseStickyItems = 156,

        // TODO: investigate 157 through 173

        // Deal random typed damage; damage is increased if target is in the semi-invulnerable turn of Bounce, Fly, and Sky Drop
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier
        DamageWithBoostIfTargetIsFlying = 174,

        // TODO: investigate 175 through 179

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

        // Increase max HP
        //   Params[2] = (integer) Amount of max HP to increase
        IncreaseMaxHP = 190,

        // TODO: investigate 191 through 221

        // Heal HP and increase max HP until the threshold; increase max HP further if HP is at max above the threshold
        //   Params[1] = (integer) Amount of HP to restore
        //   Params[2] = (integer) Amount of max HP to increase
        //   Params[3] = (integer) HP threshold
        //                - The max HP will always be increased if below the threshold,
        //                  otherwise max HP will only be increased if user is at full HP
        RestoreAndIncreaseHP = 222,

        // TODO: investigate 223 through 235

        // TODO: investigate -- used by Switcher Wand (236 131 236 238)
        Unknown236 = 236,

        // TODO: investigate -- used by Pounce Wand (237 81 237 238)
        Unknown237 = 237,

        // TODO: investigate -- used by Switcher Wand and Pounce Wand
        Unknown238 = 238,

        // TODO: investigate 239 through 258
    }
}
