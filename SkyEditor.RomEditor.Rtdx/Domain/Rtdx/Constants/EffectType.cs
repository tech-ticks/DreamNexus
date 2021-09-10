namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public enum EffectType : ushort
    {
        // No action
        None = 0,

        // 1 is unused
        Unknown1 = 1,

        // Deal random basic attack damage
        BasicAttackDamage = 2,

        // Deal random damage
        //   Params[0] = (percentage) Critical hit ratio
        Damage = 3,

        // 4 is unused
        Unknown4 = 4,

        // Deal random damage using Defense instead of Special Defense
        //   Params[0] = (percentage) Critical hit ratio
        DamageUsingDefense = 5,

        // Deal random damage ignoring target's Defense and Evasion stat changes
        //   Params[0] = (percentage) Critical hit ratio
        DamageIgnoringDefenseAndEvasion = 6,

        // Remove target's Reflect/Light Screen and deal random damage
        //   Params[0] = (percentage) Critical hit ratio
        DamageAndBreakDefenses = 7,

        // Deal random damage; damage is increased if target is afflicted with an specific status
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier
        //   Params[2] = (boolean) Remove status on hit
        //   Params[3] = (boolean) ???  (either 0 or 1)
        //   Params[4] = (StatusIndex) Status to check
        DamageWithBoostIfTargetHasStatus = 8,

        // Deal random damage; damage is increased the faster the attacker is compared to the target (based on the Speed stat)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Maximum damage multiplier
        DamageWithBoostIfFaster = 9,

        // Deal random damage; damage is increased the slower the attacker is compared to the target (based on the Speed stat)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Maximum damage multiplier
        DamageWithBoostIfSlower = 10,

        // Deal random damage; damage is increased if not holding an item
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier
        DamageWithBoostIfNotHoldingItem = 11,

        // Deal random damage; damage is modified based on attacker's HP
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier at minimum HP
        //   Params[2] = (percentage) Damage multiplier at maximum HP
        DamageWithBoostBasedOnAttackersHP = 12,

        // Deal random damage; damage is modified based on target's HP
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier at minimum HP
        //   Params[2] = (percentage) Damage multiplier at maximum HP
        DamageWithBoostBasedOnTargetsHP = 13,

        // Deal fixed damage based on how many dungeons the attacker was taken into
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier at zero visits
        //   Params[2] = (percentage) Damage multiplier at maximum visits (count unknown)
        //   Params[3] = (integer) Number of dungeons visited for maximum effect
        DamageWithBoostBasedOnDungeonsVisited = 14,

        // 15 = Deal random damage; damage is increased the less PP the move has (Trump Card)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier at zero PP
        //   Params[2] = (percentage) Damage multiplier at PP threshold
        //   Params[3] = PP threshold
        DamageWithBoostBasedOnPP = 15,

        // Deal random damage; damage is increased based on how many of the user's moves have zero PP (Last Resort)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier with one move depleted
        //   Params[2] = (percentage) Damage multiplier with two moves depleted
        //   Params[3] = (percentage) Damage multiplier with three moves depleted
        DamageWithBoostBasedOnDepletedMoves = 16,

        // Deal random damage; damage is increased when the move is used consecutively (Fury Cutter)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier when used consecutively
        DamageWithBoostOnConsecutiveUses = 17,

        // Deal random damage; damage is increased when the move was used by a teammate in the same turn (Echoed Voice)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier when used after a teammate also used the move
        DamageWithBoostWhenTeammateUsedTheMoveInTheSameTurn = 18,

        // Deal random damage; damage is increased when the move was used by a teammate in the same turn and in the same room (Round)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier when used after a teammate also used the move in the same room
        DamageWithBoostWhenTeammateUsedTheMoveInTheSameTurnInTheSameRoom = 19,

        // Deal random damage; damage is increased if the target is holding an item (Knock Off)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier if the target was holding an item
        DamageWithBoostIfTargetHoldsItem = 20,

        // Deal random damage; damage is increased if the target is afflicted with any status
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier if the target is afflicted with any status
        DamageWithBoostIfTargetHasAnyStatus = 21,

        // Deal random damage; damage is increased if the user is afflicted with Burn or Poison
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier if the target is afflicted with Burn or Poison
        DamageWithBoostIfTargetHasBurnOrPoison = 22,

        // Deal random damage; damage is increased if the target's HP is below the threshold
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier if the target is at or below the HP threshold
        //   Params[3] = (percentage) HP threshold
        DamageWithBoostIfTargetsHPIsAtOrBelowThreshold = 23,

        // Randomly deal fixed 25, 50 or 75 damage or heal target by 25% of their max HP (Present)
        //   Params[0] = (percentage) Critical hit ratio
        PresentDamageOrHeal = 24,

        // Deal damage based on a "magnitude" (Magnitude)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier at maximum threshold
        MagnitudeDamage = 25,

        // Deal random damage; type is based on the held item (Natural Gift)
        //   Params[0] = (percentage) Critical hit ratio
        DamageWithTypeBasedOnHeldItem = 26,

        // Deal random damage; super-effective against Water types (Freeze-Dry)
        //   Params[0] = (percentage) Critical hit ratio
        DamageAlwaysSuperEffectiveAgainstWater = 27,

        // Deal fixed damage
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (integer) Damage amount
        FixedDamage = 28,

        // Deal damage equal to a percentage of target's current HP
        //   Params[1] = (percentage) Percentage of current HP
        DamagePercentageCurrentHP = 29,

        // Reduce target's HP to user's HP if the user has less HP than the target (Endeavor)
        EqualizeHP = 30,

        // Deal damage equal to the amount of HP that would be lost to reach the target value
        //   Params[1] = (percentage) Target HP as a percentage of max HP
        DealDamageEqualToLostHP = 31,

        // Deal damage based on the attacker's level
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Minimum damage (equal to the attacker's level multiplied by this factor)
        //   Params[2] = (percentage) Maximum damage (equal to the attacker's level multiplied by this factor)
        DamageBasedOnAttackersLevel = 32,

        // Deal random damage using the target's Attack stat
        //   Params[0] = (percentage) Critical hit ratio
        DamageUsingTargetsAttack = 33,

        // Deal damage based on the target's weight; damage is increased the heavier the target is
        //   Params[0] = (percentage) Critical hit ratio
        DamageBasedOnTargetsWeight = 34,

        // Deal random damage; damage is increased the based on how lighter the target is compared to the attacker
        //   Params[0] = (percentage) Critical hit ratio
        DamageBasedOnAttackerAndTargetWeights = 35,

        // Deal random damage and cause an explosion (Hyper Beam)
        //   Params[0] = (percentage) Critical hit ratio
        DamageAndExplode = 36,

        // Deal random damage; enemy drops Poké if defeated (Pay Day)
        //   Params[0] = (percentage) Critical hit ratio
        DamageAndDropMoney = 37,

        // TODO: confirm this
        // Deal random damage; continue executing effects if target is defeated (Fell Stinger)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Chance on defeat to continue executing effects
        DamageAndContinueActionOnDefeat = 38,

        // Deal random damage if the target shares a type with the user (Synchronoise)
        //   Params[0] = (percentage) Critical hit ratio
        DamageIfTargetSharesTypeWithAttacker = 39,

        // Deal random damage; damage is boosted based on the number of positive stat changes of the attacker or target
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (enum) 0 = Attacker's stat boosts; 1 = Target's stat boosts
        DamageWithBoostBasedOnPositiveStatChanges = 40,

        // Charged move
        ChargedMove = 41,

        // Deal damage stored by Bide/Revenge/Avalanche
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier
        DealStoredDamage = 42,

        // Grab target and fly; apply Sky Drop status (first turn of Sky Drop)
        SkyDropFirstTurn = 43,

        // Drop target and deal damage (second turn of Sky Drop)
        //   Params[0] = (percentage) Critical hit ratio
        SkyDropSecondTurn = 44,

        // Take recoil damage
        //   Params[1] = (percentage) Damage taken as a percentage of HP
        RecoilDamage = 45,

        // Take recoil damage from Struggle
        //   Params[1] = (percentage) Damage taken as a percentage of HP
        StruggleRecoilDamage = 46,

        // Take recoil damage on miss
        //   Params[1] = (percentage) Damage taken as a percentage of HP
        RecoilDamageOnMiss = 47,

        // Reduce user's HP to 1 (Final Gambit)
        ReduceHPToOne = 48,

        // Heal self for % of damage dealt
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
        ApplyStatChanges = 52,

        // Apply stat changes
        //   Params[0] = (percentage) Chance to apply stat changes
        //   Params[5] = (index) Index into ActStatusTableDataInfo for the stat changes to apply
        //   Params[7] = (DungeonStatusIndex) Dungeon status effect under which the stat boosts are doubled
        ApplyStatChangesWithWeatherBoost = 53,

        // Modify stats (Screech, Charm, Aurora Beam, Memento)
        //   Params[0] = (percentage) Chance to apply stat changes
        //   Params[5] = (index) Index into ActStatusTableDataInfo for the stat changes to apply
        ModifyStats = 54,

        // Reverse stat changes (making positive changes negative and vice-versa) (Topsy-Turvy)
        ReverseStatChanges = 55,

        // Swap raw Attack stat with raw Defense stat (Power Trick)
        SwapAttackWithDefense = 56,

        // Copy target's stat changes (Psych Up)
        CopyTargetStatChanges = 57,

        // Apply a random stat change (Acupressure)
        //   Params[0] = (percentage) Chance to apply stat change
        //   Params[5] = (index) Index into ActStatusTableDataInfo for the stat changes to apply
        ApplyRandomStatChange = 58,

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
        ApplyStatusEffectWithChance = 66,

        // Chance to apply status effect, with an additional unknown parameter
        //   Params[0] = (percentage) Chance to apply status effect
        //   Params[3] = (unknown)
        //   Params[4] = (StatusIndex) Status effect to apply
        ApplyStatusEffectWithChanceAndSomethingElse = 67,

        // Apply freeze, burn or paralysis (Tri Attack)
        //   Params[0] = (percentage) Chance to apply status effect
        ApplyFreezeBurnOrParalysis = 68,

        // Apply status effect based on current location (Secret Power)
        //   Params[0] = (percentage) Chance to apply status effect
        ApplyStatusEffectBasedOnLocation = 69,

        // Remove status effect
        //   Params[0] = (percentage) Chance to remove status effect
        //   Params[2] = (boolean) Unknown purpose
        //   Params[3] = (boolean) Unknown purpose
        //   Params[4] = (StatusIndex) Status effect to remove
        RemoveStatusEffect = 70,

        // Remove all negative status effects and remove seals
        RemoveAllNegativeStatusEffects = 71,

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

        // Pull user towards target
        PullTowardsTarget = 81,

        // Warp to a random place on the same floor
        Warp = 82,

        // TODO: investigate -- used by action 769, which is not used by any moves or items
        Unknown83 = 83,

        // Steal target's item (Thief, Covet)
        Steal = 84,

        // Part two of Thief and Covet (unknown effect)
        //   Params[1] = (unknown)  (15 in both moves)
        ThiefCovetPartTwo = 85,

        // Warp a random target to the front of the user
        WarpRandomTargetToFront = 86,

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

        // Warp user and set user's HP to a percentage of current HP
        WarpAndSetHPToPercentOfCurrentHP = 141,

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

        // TODO: investigate 157 -- used by Gastro Acid and a dummied out trap (action 979)
        Unknown157 = 157,

        // TODO: investigate 158 -- used by a dummied out item (action 807)
        Unknown158 = 158,

        // Exit the dungeon
        ExitDungeon = 159,

        // Pull all items on the floor toward the user
        PullAllItems = 160,

        // Reveal all hidden traps on the floor
        RevealAllTraps = 161,

        // Destroy all traps on the floor
        DestroyAllTraps = 162,

        // Reveal the entire floor
        RevealFloor = 163,

        // TODO: investigate 164 -- used by a dummied out item (action 781)
        Unknown164 = 164,

        // Turn the current room into a Monster House
        CreateMonsterHouse = 165,

        // Apply the Petrify status effect to all targets
        PetrifyAllTargets = 166,

        // Deal random damage; damage is modified under Rain, Sandstorm or Hail
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier under Rain, Sandstorm or Hail
        DamageWithModifierUnderBadWeather = 167,

        // Warp target to the room containing the stairs
        WarpToStairsRoom = 168,

        // Restore HP with a boost in sunny weather or penalty in bad weather
        //   Params[1] = (percentage) Percentage of HP restored in normal weather
        //   Params[2] = (percentage) Percentage of HP restored in sunny weather
        //   Params[3] = (percentage) Percentage of HP restored in bad weather
        RestoreHPWithBoostInSunnyWeather = 169,

        // Warp teammates around target
        WarpTeammatesAroundTarget = 170,

        // Deal damage equal to attacker's level
        DamageEqualToAttackersLevel = 171,

        // Pull all targets in the room together
        PullTargetsTogether = 172,

        // 173 is unused
        Unknown173 = 173,

        // Deal random damage; damage is increased if target is in the semi-invulnerable turn of Bounce, Fly, and Sky Drop
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier
        DamageWithBoostIfTargetIsFlying = 174,

        // Deal random damage, ignoring Protect
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier
        DamageIgnoreProtect = 175,

        // Dig walls
        //   Params[1] = (integer) Number of tiles to dig
        DigWalls = 176,

        // Turn the entire floor into one giant room
        OneRoom = 177,

        // Deal random damage until attack misses
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier
        DamageUntilMiss = 178,

        // Chance to apply status effect
        //   Params[0] = (percentage) Chance to apply stat changes
        //   Params[4] = (StatusIndex) Status effect to apply
        ApplyStatusEffectWithChance2 = 179,

        // Restore belly and increase max belly if fully sated
        //   Params[1] = (integer) Amount of belly to restore
        //   Params[2] = (integer) Amount of max belly to increase
        RestoreAndIncreaseBelly = 180,

        // Randomly apply Stuck, Poison, Paralysis or Burn (Grimy Food)
        //   Params[0] = (percentage) Effect chance?
        BadFoodStatus = 181,

        // Heal HP or increase max HP if at max (Sitrus Berry)
        //   Params[1] = (integer) Amount of HP to restore
        //   Params[2] = (integer) Amount of max HP to increase
        RestoreHP = 182,

        // TODO: investigate 183 -- used by Tiny Reviver Seeds and Reviver Seeds
        Unknown183 = 183,

        // TODO: investigate 184 -- used by Tiny Reviver Seeds and Reviver Seeds
        Unknown184 = 184,

        // Decrease target's level
        //   Params[1] = (integer) Amount of levels to decrease
        //   Params[2] = (integer) Unknown purpose
        DecreaseLevel = 185,

        // Increase target's level
        //   Params[1] = (integer) Amount of levels to decrease
        //   Params[2] = (integer) Unknown purpose
        IncreaseLevel = 186,

        // TODO: investigate 187 -- used by Tiny Reviver Seeds, Reviver Seeds and Plain Seeds
        Unknown187 = 187,

        // Disable the target's last used move for everyone
        DisableMove = 188,

        // Restore PP on all of the target's moves
        //   Params[1] = (integer) Amount of PP to restore
        RestorePPAll = 189,

        // Increase max HP
        //   Params[2] = (integer) Amount of max HP to increase
        IncreaseMaxHP = 190,

        // Increase a stat
        IncreaseStat = 191,

        // Increase a move's power
        //   Params[1] = (integer) Amount to increase
        IncreaseMovePower = 192,

        // Increase a move's accuracy
        //   Params[1] = (integer) Amount to increase
        IncreaseMoveAccuracy = 193,

        // Increase a move's PP
        //   Params[1] = (integer) Amount to increase
        IncreaseMovePP = 194,

        // Increase max HP, Attack, Defense, Special Attack, Special Defense or Speed at random
        IncreaseRandomStats = 195,
        
        // 196 is unused
        Unknown196 = 196,

        // 197 is unused
        Unknown197 = 197,

        // Swap the user's and target's current HP
        SwapHP = 198,

        // Spawn a projectile that flies towards the stairs
        RevealStairsDirection = 199,

        // TODO: investigate 200 -- used by a dummied out item (action 944)
        Unknown200 = 200,

        // TODO: investigate 201 -- used by a dummied out item (action 945)
        Unknown201 = 201,

        // Summon a random number of monsters
        //   Params[1] = (integer) Minimum number of monsters to summon
        //   Params[2] = (integer) Maximum number of monsters to summon
        SummonMonsters = 202,

        // Deal fixed damage and fall to the next floor
        //   Params[1] = (integer) Amount of damage to take
        DamageAndFallToNextFloor = 203,

        // Throw target to a random direction up to 10 tiles, causing 5 damage on collision
        ThrowTargetToARandomDirection = 204,
        
        // Seal a random move
        //   Params[0] = (percentage) Chance to apply effect
        SealRandomMove = 205,

        // Drain all PP of a random move
        DrainPP = 206,

        // Transform all items in the room into random wild Pokémon appropriate for the floor
        TransformItemsIntoPokemon = 207,
        
        // TODO: investigate further
        // Deal Stealth Rock damage
        //   Params[1] = (integer) Unknown purpose
        StealthRockDamage = 208,

        // Force target to drop items
        //   Params[1] = (integer) Minimum number of items
        //   Params[2] = (integer) Maximum number of items
        DropItems = 209,
        
        // Produce the effects of a random trap
        RandomTrapEffect = 210,

        // Spawn a random wild Pokémon or give a random rare item (Sparkling Floor)
        //   Params[1] = (unknown)
        SpawnPokemonOrGiveRandomItem = 211,

        // Give Gold Bars and rare items if the leader is holding a Gold Scope (Gold Floor; GtI leftover)
        GiveGoldOrRareItem = 212,

        // Turn a random food item in the inventory into Grimy Food
        //   Params[0] = (percentage) Chance to apply effect
        TurnFoodIntoGrimyFood = 213,

        // TODO: investigate 214 -- used by a dummied out trap (action 980)
        Unknown214 = 214,

        // Turn a random item in the inventory into an Apple
        TurnRandomItemIntoApple = 215,

        // Spawn an enemy Stunfisk nearby (Stunfisk "trap")
        SpawnStunfisk = 216,

        // Cause an explosion that reduces current HP of affected target by 50% and destroys items and walls in the area
        //   Params[1] = (integer) Size of the explosion: 0 = small (3x3), 1 = large (5x5)
        Explode = 217,

        // TODO: investigate 218 -- used by Sleep Talk and a dummied out move (action 638)
        Unknown218 = 218,

        // Open Kangaskhan storage window
        OpenKangaskhanStorage = 219,

        // TODO: investigate 220 -- used by dummied out items (actions 924 and 925)
        Unknown220 = 220,

        // TODO: investigate the difference between this and effect 3 (Damage)
        // Deal random damage (Vital Throw, Storm Throw)
        //   Params[0] = (percentage) Critical hit ratio
        DamageThrow = 221,

        // Heal HP and increase max HP until the threshold; increase max HP further if HP is at max above the threshold
        //   Params[1] = (integer) Amount of HP to restore
        //   Params[2] = (integer) Amount of max HP to increase
        //   Params[3] = (integer) HP threshold
        //                - The max HP will always be increased if below the threshold,
        //                  otherwise max HP will only be increased if user is at full HP
        RestoreAndIncreaseHP = 222,

        // Deal random damage boosted by Pledge moves used by teammates
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier
        PledgeBoostedDamage = 223,

        // Apply dungeon status effect if another teammate has used a matching Pledge move:
        // - Grass -> Water: applies the Swamp dungeon status effect
        // - Fire -> Grass Pledge: applies the Sea of Fire dungeon status effect
        // - Water -> Fire Pledge: applies the Rainbow dungeon status effect
        ApplyPledgeEffect = 224,

        // Respawn all defeated teammates
        //   Params[0] = (percentage) Chance to respawn
        //   Params[1] = (percentage) Percentage of HP to heal
        RespawnDefeatedTeammates = 225,

        // Inflict a random status condition or reduce a random stat on a random target (Shadow Casting)
        //   Params[0] = (percentage) Chance to inflict status effect or reduce random stat
        InflictRandomStatusOrReduceRandomStat = 226,

        // Deal random damage; damage is modified when there are many enemies nearby (Diamond Storm)
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier
        DamageWithBoostBasedOnNumberOfEnemiesNearby = 227,

        // TODO: investigate 228 -- used by dummied out move 679 (action 645)
        Unknown228 = 228,

        DarkMatterChargingStart = 229,

        // TODO: investigate 230 -- used by dummied out item (action 986)
        Unknown230 = 230,

        // TODO: investigate 231 -- used by dummied out items (actions 946, 947, 948, 949 and 950)
        Unknown231 = 231,

        // Spawn an enemy Ditto nearby (Ditto "trap")
        SpawnDitto = 232,

        // Apply status effect to summoned monsters
        //   Params[0] = (percentage) Chance to apply status effect
        //   Params[1] = (StatusIndex) Status effect to apply
        ApplyStatusEffectToSummonedMonsters = 233,

        // Begin or end storing damage taken
        BeginEndStoringDamage = 234,

        // TODO: investigate 235 -- used by dummied out items (actions 888, 889 and 
        Unknown235 = 235,

        // TODO: investigate -- used by Switcher Wand (236 131 236 238)
        Unknown236 = 236,

        // TODO: investigate -- used by Pounce Wand (237 81 237 238)
        Unknown237 = 237,

        // Teleport all teammates to the user
        //   Params[1] = (integer) Unknown purpose, always zero
        //   Params[2] = (integer) Unknown purpose, always zero
        //   Params[3] = (integer) Unknown purpose, always zero
        SurroundUser = 238,

        // TODO: investigate 239 -- used by dummied out item (action 890)
        Unknown239 = 239,

        // Deal random damage; damage is increased if the move is used in any weather other than Clear
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier if the move is used in any weather other than Clear
        DamageWithBoostIfNotClearWeather = 240,

        // Reset all lowered stats
        //   Params[0] = (percentage) Chance to apply effect
        ResetLoweredStats = 241,

        // Increase a random stat, with a chance for a bigger increase
        //   Params[0] = (percentage) Chance to apply effect
        IncreaseRandomStat = 242,

        // Clear all water/magma tiles on the floor
        //   Params[0] = (percentage) Chance to apply effect
        ClearAllWaterAndMagma = 243,

        // Summon a team of three helpers
        //   Params[0] = (percentage) Chance to apply effect
        SummonHelpers = 244,

        // Open the Bank window (Bank Orb)
        OpenBank = 245,

        // Open the Rescue Team Camps window (Wigglytuff Orb)
        //   Params[0] = (percentage) Chance to apply effect
        OpenRescueTeamCamps = 246,

        // TODO: investigate 247 -- used by dummied out action 988

        // Deal random damage with an increased recruitment rate on defeat
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[2] = (per-mille) Recruitment rate increase
        DamageWithBoostedRecruitRate = 248,

        // TODO: investigate 249 -- used by dummied out action 992
        Unknown249 = 249,

        // TODO: investigate 250 -- used by Reset Orb
        Unknown250 = 250,

        // TODO: investigate 251 -- used by dummied out action 990 (with parameters specifying 5 to 10 items)
        Unknown251 = 251,

        // TODO: investigate 252 -- used by item 612 - Unidentified Floating Object (action 991)
        Unknown252 = 252,

        // Restore PP on one of the target's moves
        //   Params[1] = (integer) Amount of PP to restore
        RestorePPOne = 253,

        // Deal random damage; damage is increased if the move missed in the last turn
        //   Params[0] = (percentage) Critical hit ratio
        //   Params[1] = (percentage) Damage multiplier if the move missed in the last turn
        DamageWithBoostIfPreviousTurnMissed = 254,

        // 255 is unused
        Unknown255 = 255,

        // Remove the specified type from the target
        //   Params[6] = (PokemonType) Type to remove
        RemoveType = 256,

        // TODO: investigate 257 -- used by dummied out move (action 659) - also deals 10 fixed damage
        Unknown257 = 257,

        // TODO: investigate 258 -- used by dummied out move (action 638)
        Unknown258 = 258,
        Max = 259,
    }
}
