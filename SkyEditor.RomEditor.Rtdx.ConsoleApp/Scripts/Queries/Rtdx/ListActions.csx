#load "../../../Stubs/Rtdx.csx"

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using ActionKindStrings = SkyEditor.RomEditor.Resources.Strings.ActionKind;
using ActionAreaStrings = SkyEditor.RomEditor.Resources.Strings.ActionArea;
using ActionTargetStrings = SkyEditor.RomEditor.Resources.Strings.ActionTarget;
using EffectTypeStrings = SkyEditor.RomEditor.Resources.Strings.EffectType;

// Grab all the data we'll need
var actionData = Rom.GetActDataInfo().Entries;
var paramData = Rom.GetActParamDataInfo().Entries;
var statChangeData = Rom.GetActStatusTableDataInfo().Entries;
var hitCountData = Rom.GetActHitCountTableDataInfo().Entries;
var effectData = Rom.GetActEffectDataInfo().Entries;
var moveData = Rom.GetWazaDataInfo().Entries;
var itemData = Rom.GetItemDataInfo().Entries;
var strings = Rom.GetCommonStrings();
var dungeonBin = Rom.GetDungeonBinEntry();

// ----- Helper functions -----

// Add an entry to a map of lists. Creates a new list for new entries.
public static void AddToList<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dictionary, TKey key, TValue value)
{
    if (!dictionary.ContainsKey(key))
    {
        dictionary.Add(key, new List<TValue>());
    }
    dictionary[key].Add(value);
}

// Retrieves the list corresponding to the given key from the map, or an empty list if the map doesn't contain that entry
public static IList<TValue> GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dictionary, TKey key)
{
    return dictionary.TryGetValue(key, out IList<TValue> value) ? value : Array.Empty<TValue>();
}

// Retrieves the value corresponding to the given key from the map or the specified default value
public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
{
    return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
}

// Format a 64-bit number as a series of dashes and hashes that are easy to read.
public string FormatBits(ulong b)
{
    var binary = Convert.ToString((long)b, 2);
    var text = binary.Replace('0', '-').Replace('1', '#').PadLeft(64, '-');

    // Split into groups of 8
    var groups = Enumerable.Range(0, text.Length / 8).Select(i => text.Substring(i * 8, 8));

    // Join back into a string, separating each group with a space and each set of four with an additional space
    return string.Join(" ", groups).Insert(35, " ");
}

// Format a hit count entry
string FormatHits(ActHitCountTableDataInfo.Entry hitCountEntry)
{
    if (hitCountEntry.Index > 1)
    {
        if (hitCountEntry.StopOnMiss != 0)
        {
            return $"up to {hitCountEntry.MaxHits} hits";
        }
        else if (hitCountEntry.MinHits == hitCountEntry.MaxHits)
        {
            return $"{hitCountEntry.MaxHits} hits";
        }
        else
        {
            double weightSum = 0;
            for (var i = hitCountEntry.MinHits; i <= hitCountEntry.MaxHits; i++)
            {
                weightSum += hitCountEntry.Weights[i - hitCountEntry.MinHits];
            }

            var str = new StringBuilder();
            str.Append($"{hitCountEntry.MinHits} to {hitCountEntry.MaxHits} hits (");
            for (var i = hitCountEntry.MinHits; i <= hitCountEntry.MaxHits; i++)
            {
                var weight = hitCountEntry.Weights[i - hitCountEntry.MinHits];
                double chance = weight / weightSum * 100.0;
                if (i > hitCountEntry.MinHits)
                {
                    str.Append(" ");
                }
                str.Append($"{chance:f1}%");
            }
            str.Append(")  (");
            for (var i = hitCountEntry.MinHits; i <= hitCountEntry.MaxHits; i++)
            {
                var weight = hitCountEntry.Weights[i - hitCountEntry.MinHits];
                if (i > hitCountEntry.MinHits)
                {
                    str.Append(" ");
                }
                str.Append($"{weight}");
            }
            str.Append(")");
            return str.ToString();
        }
    }
    else
    {
        return "1";
    }
}
// Get the name of a move category
public string GetCategoryName(MoveCategory category)
{
    switch (category)
    {
        case MoveCategory.Physical: return "Physical";
        case MoveCategory.Special: return "Special";
        case MoveCategory.Status: return "Status";
        case MoveCategory.None: return "None";
        default: return category.ToString();
    }
}

public string GetActionKindString(ActDataInfo.ActionKind kind)
{
    return ActionKindStrings.ResourceManager.GetString(kind.ToString(), Thread.CurrentThread.CurrentUICulture) ?? $"Unknown ActionKind {kind}";
}

// Get the name of an ActionArea value
public string GetActionAreaString(ActDataInfo.ActionArea area)
{
    return ActionAreaStrings.ResourceManager.GetString(area.ToString(), Thread.CurrentThread.CurrentUICulture) ?? $"Unknown ActionArea {area}";
}

// Get the name of an ActionTarget value
public string GetActionTargetString(ActDataInfo.ActionTarget target)
{
    return ActionTargetStrings.ResourceManager.GetString(target.ToString(), Thread.CurrentThread.CurrentUICulture) ?? $"Unknown ActionTarget {target}";
}

// Get the name of an effect
public string GetEffectName(EffectType type)
{
    return EffectTypeStrings.ResourceManager.GetString(type.ToString(), Thread.CurrentThread.CurrentUICulture) ?? $"Unknown effect {type}";
}

// Get the name of a stat
// TODO: see if there is an enum or some other relation
public string GetStatName(ushort index)
{
    switch (index)
    {
        case 148: return "Attack";
        case 149: return "Defense";
        case 150: return "Special Attack";
        case 151: return "Special Defense";
        case 152: return "Speed";
        default: return $"Unknown stat {index}";
    }
}

// Describes stat changes
public string DescribeStatChanges(ushort index, bool percentage)
{
    if (index < statChangeData.Count)
    {
        var entry = statChangeData[index];
        var changes = new List<string>();

        void addEntry(short mod, string name)
        {
            if (mod != 0)
            {
                if (percentage)
                {
                    changes.Add($"{mod}% {name}");
                }
                else
                {
                    changes.Add($"{mod:+#;-#} {name}");
                }
            }
        }

        addEntry(entry.AttackMod, "Attack");
        addEntry(entry.SpecialAttackMod, "Special Attack");
        addEntry(entry.DefenseMod, "Defense");
        addEntry(entry.SpecialDefenseMod, "Special Defense");
        addEntry(entry.SpeedMod, "Speed");
        addEntry(entry.AccuracyMod, "Accuracy");
        addEntry(entry.EvasionMod, "Evasion");
        return string.Join(", ", changes);
    }
    else
    {
        return $"{index} (out of range)";
    }
}

// Describes an effect parameter
public string DescribeEffectParameter(EffectParameterType paramType, ushort value)
{
    switch (paramType)
    {
        case EffectParameterType.None: return $"(unspecified) {value}";
        case EffectParameterType.EffectChance: return $"{value}% chance to execute effect";
        case EffectParameterType.CriticalHitRatio: return $"{value}% critical hit ratio";
        case EffectParameterType.RecoilPercentOfMaxHP: return $"{value}% of max HP as recoil damage";
        case EffectParameterType.DamagePercentOfCurrentHP: return $"{value}% of current HP as damage";
        case EffectParameterType.ChanceToApplyFurtherEffects: return $"{value}% chance to apply further effects";
        case EffectParameterType.MinDamageLevelFactor: return $"Minimum damage equal to {value}% of attacker's level";
        case EffectParameterType.MinVisitsDamageMultiplier: return $"{value}% damage at minimum dungeons visited";
        case EffectParameterType.DamageMultiplierAtMinimumPP: return $"{value}% damage multiplier at minimum PP";
        case EffectParameterType.DamageMultiplierAtMinimumHP: return $"{value}% damage multiplier at minimum HP";
        case EffectParameterType.DamageMultiplier: return $"{value}% damage multiplier";
        case EffectParameterType.DamageMultiplierWithOneDepletedMove: return $"{value}% damage multiplier with one depleted move";
        case EffectParameterType.FixedDamage: return $"{value} fixed damage";
        case EffectParameterType.StockpileCount: return $"{value} stockpile count";
        case EffectParameterType.SpendPercentOfMaxHP: return $"Spend {value}% of max HP";
        case EffectParameterType.SelectAttackerOrTargetStatBoosts: return (value == 0) ? "Use attacker's stat boosts" : "Use target's stat boosts";
        case EffectParameterType.HealPercentOfDamageDealt: return $"Heal {value}% of damage dealt";
        case EffectParameterType.HealPercentOfMaxHP: return $"Heal {value}% of max HP";
        case EffectParameterType.BellyAmount: return $"{value} Belly";
        case EffectParameterType.ExcludeFloating: return (value == 0) ? "All targets" : "Exclude floating targets";
        case EffectParameterType.PPAmount: return $"{value} PP";
        case EffectParameterType.HPAmount: return $"{value} HP";
        case EffectParameterType.LevelAmount: return $"{value} level(s)";
        case EffectParameterType.PowerAmount: return $"{value} Power";
        case EffectParameterType.AccuracyAmount: return $"{value} Accuracy";
        case EffectParameterType.DigTileCount: return $"Dig {value} tile(s)";
        case EffectParameterType.MinMonsterCount: return $"At least {value} monsters";
        case EffectParameterType.SparklingFloorEmpty: return (value == 0) ? "Contains items" : "Does not contain items";
        case EffectParameterType.ExplosionSize: return (value == 0) ? "Small (3x3) explosion" : "Large (5x5) explosion";
        case EffectParameterType.MinItemsToDrop: return $"At least {value} items";
        case EffectParameterType.HealPercentOfMaxHPInSunnyWeather: return $"Heal {value}% of max HP in sunny weather";
        case EffectParameterType.RemoveStatusOnHit: return (value == 0) ? "Keep status effect on hit" : "Remove status effect on hit";
        case EffectParameterType.MaxDamageLevelFactor: return $"Maximum damage equal to {value}% of attacker's level";
        case EffectParameterType.MaxVisitsDamageMultiplier: return $"{value}% damage at maximum dungeons visited";
        case EffectParameterType.DamageMultiplierAtMaximumPP: return $"{value}% damage multiplier at maximum PP";
        case EffectParameterType.DamageMultiplierAtMaximumHP: return $"{value}% damage multiplier at maximum HP";
        case EffectParameterType.DamageMultiplierWithTwoDepletedMoves: return $"{value}% damage multiplier with two depleted moves";
        case EffectParameterType.MaxBellyAmount: return $"{value} max Belly";
        case EffectParameterType.MaxHPAmount: return $"{value} max HP";
        case EffectParameterType.MaxMonsterCount: return $"At most {value} monsters";
        case EffectParameterType.RecruitRateBoost: return $"{value / 10.0f:f1}% increased recruitment rate";
        case EffectParameterType.MaxItemsToDrop: return $"At most {value} items";
        case EffectParameterType.HealPercentOfMaxHPInBadWeather: return $"Heal {value}% of max HP in bad weather";
        case EffectParameterType.PercentOfMaxHPThreshold: return $"Threshold of {value}% of max HP";
        case EffectParameterType.MaxDungeonsVisited: return $"Maximum effect at {value} dungeons visited";
        case EffectParameterType.PPThreshold: return $"Threshold of {value} PP";
        case EffectParameterType.DamageMultiplierWithThreeDepletedMoves: return $"{value}% damage multiplier with three depleted moves";
        case EffectParameterType.HPThreshold: return $"Threshold of {value} HP";
        case EffectParameterType.StatusEffect: return $"{strings.Statuses.GetValueOrDefault((StatusIndex)value, $"Unknown ({value})")} status effect";
        case EffectParameterType.StatMultiplierIndex: return DescribeStatChanges(value, true);
        case EffectParameterType.StatChangeIndex: return DescribeStatChanges(value, false);
        case EffectParameterType.StatIndex: return GetStatName(value);
        case EffectParameterType.PokemonType: return $"{strings.PokemonTypes.GetValueOrDefault((PokemonType)value, $"Unknown ({value})")} type";
        case EffectParameterType.CheckDungeonStatusEffect: return $"{strings.DungeonStatuses.GetValueOrDefault((DungeonStatusIndex)value, $"Unknown ({value})")} dungeon status effect";
        case EffectParameterType.SetDungeonStatusEffect: return $"{strings.DungeonStatuses.GetValueOrDefault((DungeonStatusIndex)value, $"Unknown ({value})")} dungeon status effect";
        default: return $"(unknown parameter {paramType}) {value}";
    }
}

// ----------------------------

// Build lookup tables of action indices to moves and items for fast lookups
var actionsToMoves = new Dictionary<int, IList<WazaDataInfo.Entry>>();
var actionsToItems = new Dictionary<int, IList<ItemDataInfo.Entry>>();

foreach (var move in moveData)
{
    if (move.ActIndex != 0)
    {
        actionsToMoves.AddToList(move.ActIndex, move);
    }
}

foreach (var item in itemData)
{
    if (item.PrimaryActIndex != 0)
    {
        actionsToItems.AddToList(item.PrimaryActIndex, item);
    }
    if (item.ReviveActIndex != 0)
    {
        actionsToItems.AddToList(item.ReviveActIndex, item);
    }
    if (item.ThrowActIndex != 0)
    {
        actionsToItems.AddToList(item.ThrowActIndex, item);
    }
}

// ----------------------------

// Print everything nicely formatted for humans
for (var i = 1; i < actionData.Count; i++)
{
    var act = actionData[i];
    var effectEntry = effectData[i];
    var hitCountEntry = hitCountData[act.ActHitCountIndex];
    var text1 = dungeonBin.GetStringByHash((int)act.DungeonMessage1);
    var text2 = dungeonBin.GetStringByHash((int)act.DungeonMessage2);
    var moves = actionsToMoves.GetValueOrDefault(i);
    var items = actionsToItems.GetValueOrDefault(i);
    var type = strings.PokemonTypes.GetValueOrDefault(act.MoveType, act.MoveType.ToString());
    var category = GetCategoryName(act.MoveCategory);

    // Print flags for moves and items related to actions
    /*foreach (var move in moves)
    {
        var name = strings.Moves.GetValueOrDefault(move.Index, move.Index.ToString());
        Console.WriteLine($"{i,3}  {(int)move.Index,3}  {name,-30}  {FormatBits(act.Flags)}");
    }

    foreach (var item in items)
    {
        var name = strings.Items.GetValueOrDefault(item.Index, item.Index.ToString());
        Console.WriteLine($"{i,3}  {(int)item.Index,3}  {name,-30}  {FormatBits(act.Flags)}");
    }

    continue;*/
    
    Console.WriteLine($"Action {i}: {GetActionKindString(act.Kind)} - {GetActionTargetString(act.Target)}, {GetActionAreaString(act.Area)}, range {act.Range}");

    // Print moves and items that invoke the action
    var totalUseCount = moves.Count + items.Count;
    if (totalUseCount == 1)
    {
        if (moves.Count == 1)
        {
            var move = moves[0];
            var name = strings.Moves.GetValueOrDefault(move.Index, move.Index.ToString());
            Console.WriteLine($"  Used by move {(int)move.Index}: {name} ({type}, {category})");
        }
        else
        {
            var item = items[0];
            var name = strings.Items.GetValueOrDefault(item.Index, item.Index.ToString());
            Console.WriteLine($"  Used by item {(int)item.Index}: {name}");
        }
    }
    else if (totalUseCount > 1)
    {
        Console.WriteLine("  Used by:");
        foreach (var move in moves)
        {
            var name = strings.Moves.GetValueOrDefault(move.Index, move.Index.ToString());
            Console.WriteLine($"    Move {(int)move.Index}: {name} ({type}, {category})");
        }
        foreach (var item in items)
        {
            var name = strings.Items.GetValueOrDefault(item.Index, item.Index.ToString());
            Console.WriteLine($"    Item {(int)item.Index}: {name}");
        }
    }

    string formatAccuracy(ushort acc)
    {
        return (acc >= 101) ? "Sure shot" : acc.ToString();
    }

    string formatRange(byte min, byte max)
    {
        return (min == max) ? $"{min}" : $"{min} to {max}";
    }

    string formatAccuracyRange(ushort min, ushort max)
    {
        return (min == max) ? $"{formatAccuracy(min)}" : $"{formatAccuracy(min)} to {formatAccuracy(max)}";
    }

    // Print attributes
    // Console.WriteLine($"  Flags:    {FormatBits(act.Flags)}");
    if (act.Kind == ActDataInfo.ActionKind.Move)
    {
        Console.WriteLine($"  Hits:     {FormatHits(hitCountEntry)}");
        Console.WriteLine($"  Power:    {formatRange(act.MinPower, act.MaxPower)}");
        Console.WriteLine($"  PP:       {formatRange(act.MinPP, act.MaxPP)}");
    }
    Console.WriteLine($"  Accuracy: {formatAccuracyRange(act.MinAccuracy, act.MaxAccuracy)}");

    // Print effects
    Console.WriteLine($"  Effects:");
    foreach (var effect in act.Effects)
    {
        if (effect.Type == default)
        {
            continue;
        }
        Console.WriteLine($"    ({(int)effect.Type}) {GetEffectName(effect.Type)}");
        for (var j = 0; j < 8; j++)
        {
            if (effect.ParamTypes[j] != 0)
            {
                Console.WriteLine($"      [{j}] ({(int)effect.ParamTypes[j]}) {DescribeEffectParameter(effect.ParamTypes[j], effect.Params[j])}");
            }
        }
    }

    // Print messages
    if (!string.IsNullOrEmpty(text1))
    {
        Console.WriteLine($"  Dungeon message 1: \"{text1}\"");
    }
    if (!string.IsNullOrEmpty(text2))
    {
        Console.WriteLine($"  Dungeon message 2: \"{text2}\"");
    }

    // Print unknown fields
    Console.WriteLine("  Unknown fields:");
    Console.WriteLine($"    Flags: {FormatBits(act.Flags)}");
    Console.WriteLine($"    0x80..0x8F:   -    -    -  {act.Byte83,3}  {act.Byte84,3}  {act.Byte85,3}  {act.Byte86,3}  {act.Byte87,3}    -  {act.Byte89,3}  {act.Byte8A,3}  {act.Byte8B,3}    -    -  {act.Byte8E,3}  {act.Byte8F,3}");
    Console.WriteLine($"    0x90..0x9F: {act.Byte90,3}  {act.Byte91,3}  {act.Byte92,3}    -  {act.Byte94,3}  {act.Byte95,3}  {act.Byte96,3}  {act.Byte97,3}  {act.Byte98,3}  {act.Byte99,3}  {act.Byte9A,3}  {act.Byte9B,3}  {act.Byte9C,3}  {act.Byte9D,3}  {act.Byte9E,3}  {act.Byte9F,3}");

    // Print ActEffectDataInfo
    Console.WriteLine("  ActEffectDataInfo:");
    Console.WriteLine($"    0x00: {effectEntry.Byte00,3}  {effectEntry.Byte01,3}  {effectEntry.Short02,5}  {effectEntry.Float04,7:f2}  {effectEntry.Float08,7:f2}  {effectEntry.Int0C,10}");
    Console.WriteLine($"    0x10: {effectEntry.Short10,5}  {effectEntry.Short12,5}  {effectEntry.Short14,5}  {effectEntry.Short16,5}  {effectEntry.Short18,5}  {effectEntry.Short1A,5}  {effectEntry.Short1C,5}  {effectEntry.Short1E,5}");
    Console.WriteLine($"    0x20: {effectEntry.Short20,5}  {effectEntry.Short22,5}  {effectEntry.Short24,5}  {effectEntry.Short26,5}  {effectEntry.Short28,5}  {effectEntry.Short2A,5}  {effectEntry.Short2C,5}  {effectEntry.Short2E,5}");
    Console.WriteLine($"    0x30: {effectEntry.Short30,5}  {effectEntry.Short32,5}  {effectEntry.Short34,5}  {effectEntry.Short36,5}  {effectEntry.Short38,5}  {effectEntry.Int3C,10}");
    
    Console.WriteLine();
}

/*Console.WriteLine("#;Type;Category;00;10;12;14;16;18;1A;1C;1E;20;22;24;26;StatusChance;2A;2C;2E;30;32;34;36;38;3A;3C;3E;40;42;44;46;48;4A;4C;4E;50;52;54;56;58;5A;5C;5D;5E;5F;60;61;62;63;64;65;66;67;68;69;6A;6B;6C;6D;6E;6F;70;71;72;73;74;75;76;77;78;79;7A;7B;7C;7F;80;81;82;83;84;85;86;87;Range;89;8A;8B;Area;Target;8E;8F;90;91;92;94;95;96;97;98;99;9A;9B;9C;9D;9E;9F;::;00;01;02;04;08;0C;10;12;14;16;18;1A;1C;1E;20;22;24;26;28;2A;2C;2E;30;32;34;36;38;3C;::;HitCountIdx;MinHits;MaxHits;StopOnMiss;::;Text 1;Text 2");
for (var i = 0; i < actionData.Count; i++)
{
    var entry = actionData[i];
    var effectEntry = effectData[i];
    var hitCountEntry = hitCountData[entry.ActHitCountIndex];
    var text1 = dungeonBin.GetStringByHash((int)entry.Text08);
    var text2 = dungeonBin.GetStringByHash((int)entry.Text0C);
    Console.Write($"{i};");
    Console.Write($"{entry.MoveType};");
    Console.Write($"{entry.MoveCategory};");
    Console.Write($"{FormatBits(entry.Long00)};");
    Console.Write($"{entry.Short10};");
    Console.Write($"{entry.Short12};");
    Console.Write($"{entry.Short14};");
    Console.Write($"{entry.Short16};");
    Console.Write($"{entry.Short18};");
    Console.Write($"{entry.Short1A};");
    Console.Write($"{entry.Short1C};");
    Console.Write($"{entry.Short1E};");
    Console.Write($"{entry.Short20};");
    Console.Write($"{entry.Short22};");
    Console.Write($"{entry.Short24};");
    Console.Write($"{entry.Short26};");
    Console.Write($"{entry.StatusChance};");
    Console.Write($"{entry.Short2A};");
    Console.Write($"{entry.Short2C};");
    Console.Write($"{entry.Short2E};");
    Console.Write($"{entry.Short30};");
    Console.Write($"{entry.Short32};");
    Console.Write($"{entry.Short34};");
    Console.Write($"{entry.Short36};");
    Console.Write($"{entry.Short38};");
    Console.Write($"{entry.Short3A};");
    Console.Write($"{entry.Short3C};");
    Console.Write($"{entry.Short3E};");
    Console.Write($"{entry.Short40};");
    Console.Write($"{entry.Short42};");
    Console.Write($"{entry.Short44};");
    Console.Write($"{entry.Short46};");
    Console.Write($"{entry.Short48};");
    Console.Write($"{entry.Short4A};");
    Console.Write($"{entry.Short4C};");
    Console.Write($"{entry.Short4E};");
    Console.Write($"{entry.Short50};");
    Console.Write($"{entry.Short52};");
    Console.Write($"{entry.Short54};");
    Console.Write($"{entry.Short56};");
    Console.Write($"{entry.Short58};");
    Console.Write($"{entry.Short5A};");
    Console.Write($"{entry.Byte5C};");
    Console.Write($"{entry.Byte5D};");
    Console.Write($"{entry.Byte5E};");
    Console.Write($"{entry.Byte5F};");
    Console.Write($"{entry.Byte60};");
    Console.Write($"{entry.Byte61};");
    Console.Write($"{entry.Byte62};");
    Console.Write($"{entry.Byte63};");
    Console.Write($"{entry.Byte64};");
    Console.Write($"{entry.Byte65};");
    Console.Write($"{entry.Byte66};");
    Console.Write($"{entry.Byte67};");
    Console.Write($"{entry.Byte68};");
    Console.Write($"{entry.Byte69};");
    Console.Write($"{entry.Byte6A};");
    Console.Write($"{entry.Byte6B};");
    Console.Write($"{entry.Byte6C};");
    Console.Write($"{entry.Byte6D};");
    Console.Write($"{entry.Byte6E};");
    Console.Write($"{entry.Byte6F};");
    Console.Write($"{entry.Byte70};");
    Console.Write($"{entry.Byte71};");
    Console.Write($"{entry.Byte72};");
    Console.Write($"{entry.Byte73};");
    Console.Write($"{entry.Byte74};");
    Console.Write($"{entry.Byte75};");
    Console.Write($"{entry.Byte76};");
    Console.Write($"{entry.Byte77};");
    Console.Write($"{entry.Byte78};");
    Console.Write($"{entry.Byte79};");
    Console.Write($"{entry.Byte7A};");
    Console.Write($"{entry.Byte7B};");
    Console.Write($"{entry.Byte7C};");
    Console.Write($"{entry.Byte7F};");
    Console.Write($"{entry.Byte80};");
    Console.Write($"{entry.Byte81};");
    Console.Write($"{entry.Byte82};");
    Console.Write($"{entry.Byte83};");
    Console.Write($"{entry.Byte84};");
    Console.Write($"{entry.Byte85};");
    Console.Write($"{entry.Byte86};");
    Console.Write($"{entry.Byte87};");
    Console.Write($"{entry.Range};");
    Console.Write($"{entry.Byte89};");
    Console.Write($"{entry.Byte8A};");
    Console.Write($"{entry.Byte8B};");
    Console.Write($"{entry.Area};");
    Console.Write($"{entry.Target};");
    Console.Write($"{entry.Byte8E};");
    Console.Write($"{entry.Byte8F};");
    Console.Write($"{entry.Byte90};");
    Console.Write($"{entry.Byte91};");
    Console.Write($"{entry.Byte92};");
    Console.Write($"{entry.Byte94};");
    Console.Write($"{entry.Byte95};");
    Console.Write($"{entry.Byte96};");
    Console.Write($"{entry.Byte97};");
    Console.Write($"{entry.Byte98};");
    Console.Write($"{entry.Byte99};");
    Console.Write($"{entry.Byte9A};");
    Console.Write($"{entry.Byte9B};");
    Console.Write($"{entry.Byte9C};");
    Console.Write($"{entry.Byte9D};");
    Console.Write($"{entry.Byte9E};");
    Console.Write($"{entry.Byte9F};");
    Console.Write($"::;");
    Console.Write($"{effectEntry.Byte00};");
    Console.Write($"{effectEntry.Byte01};");
    Console.Write($"{effectEntry.Short02};");
    Console.Write($"{effectEntry.Float04};");
    Console.Write($"{effectEntry.Float08};");
    Console.Write($"{effectEntry.Int0C};");
    Console.Write($"{effectEntry.Short10};");
    Console.Write($"{effectEntry.Short12};");
    Console.Write($"{effectEntry.Short14};");
    Console.Write($"{effectEntry.Short16};");
    Console.Write($"{effectEntry.Short18};");
    Console.Write($"{effectEntry.Short1A};");
    Console.Write($"{effectEntry.Short1C};");
    Console.Write($"{effectEntry.Short1E};");
    Console.Write($"{effectEntry.Short20};");
    Console.Write($"{effectEntry.Short22};");
    Console.Write($"{effectEntry.Short24};");
    Console.Write($"{effectEntry.Short26};");
    Console.Write($"{effectEntry.Short28};");
    Console.Write($"{effectEntry.Short2A};");
    Console.Write($"{effectEntry.Short2C};");
    Console.Write($"{effectEntry.Short2E};");
    Console.Write($"{effectEntry.Short30};");
    Console.Write($"{effectEntry.Short32};");
    Console.Write($"{effectEntry.Short34};");
    Console.Write($"{effectEntry.Short36};");
    Console.Write($"{effectEntry.Short38};");
    Console.Write($"{effectEntry.Int3C};");
    Console.Write($"::;");
    Console.Write($"{entry.ActHitCountIndex};");
    Console.Write($"{hitCountEntry.MinHits};");
    Console.Write($"{hitCountEntry.MaxHits};");
    Console.Write($"{hitCountEntry.StopOnMiss};");
    Console.Write($"::;");
    Console.Write($"{text1};");
    Console.Write($"{text2}");
    Console.WriteLine();
}*/
