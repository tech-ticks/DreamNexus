#load "../../../Stubs/RTDX.csx"

using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using System.Linq;
using System.Runtime.CompilerServices;

var strings = Rom.GetCommonStrings();
var fixedPokemon = Rom.GetFixedPokemon().Entries;
var dungeons = Rom.GetDungeons().LoadAllDungeons();

// -- Formatters ---------------------------------

public string GetPokemonName(CreatureIndex id)
{
    return strings.Pokemon.ContainsKey(id) ? strings.Pokemon[id] : "(Unknown :" + (int)id + ")";
}

public string FormatFloor(DungeonModel dungeon, int floor)
{
    string prefix = (dungeon.Data.Features.HasFlag(DungeonFeature.FloorDirectionUp)) ? "" : "B";
    return prefix + floor + "F";
}

public string FormatFloors(DungeonModel dungeon)
{
    if (dungeon.Extra == null) return "--";
    return FormatFloor(dungeon, dungeon.Extra.Floors);
}

public string FormatTeammates(int teammates)
{
    return (teammates > 1) ? "yes" : "no";
}

public string FormatItems(int items)
{
    return (items > 0) ? "yes" : "no";
}

public string FormatFeature(DungeonFeature features, DungeonFeature feature)
{
    return (features.HasFlag(feature)) ? "yes" : "no";
}

public string FormatFeatures(DungeonFeature features)
{
    var featureNames = new List<string>();
    if (!features.HasFlag(DungeonFeature.AutoRevive)) featureNames.Add("Auto-revive");
    if (features.HasFlag(DungeonFeature.Scanner)) featureNames.Add("Scanner");
    if (features.HasFlag(DungeonFeature.Radar)) featureNames.Add("Radar");
    return string.Join(", ", featureNames);
}

public string FormatFeaturesBits(DungeonFeature features)
{
    var binary = Convert.ToString((int)features, 2);
    return binary.Replace('0', '-').Replace('1', '#').PadLeft(19, '-');
}

// -----------------------------------------------

//Console.WriteLine("#    Index   Dungeon                    Floors");
Console.WriteLine("#    Index   Dungeon                    Floors   Teammates   Items   Level reset   Recruitable   Features                       210FEDCBA9876543210   NameID 0x0A   d_balance   0x13   0x17   0x18   0x19");
foreach (var dungeon in dungeons)
{
    if (dungeon.Id == default)
    {
        continue;
    }

    var data = dungeon.Data;
    var extra = dungeon.Extra;
    var balance = dungeon.Balance;
    var floorInfos = balance.FloorInfos;
    var items = dungeon.ItemArrange;
    var wildPokemon = balance.WildPokemon;
    var trapWeights = balance.TrapWeights;
    var data4 = balance.Data4;

    // Print basic dungeon info
    //Console.Write($"{dungeon.Id,-4} {data.Index,5}   {dungeon.DungeonName,-28} {FormatFloors(dungeon),4}");

    // Print complete dungeon info
    Console.WriteLine($"{(int)dungeon.Id,-4} {data.Index,5}   {dungeon.DungeonName,-28} {FormatFloors(dungeon),4}      "
        + $"{FormatTeammates(data.MaxTeammates),3}       "
        + $"{FormatItems(data.MaxItems),3}        "
        + $"{FormatFeature(data.Features, DungeonFeature.LevelReset),3}           "
        + $"{FormatFeature(data.Features, DungeonFeature.WildPokemonRecruitable),3}       "
        + $"{FormatFeatures(data.Features),-30} "
        + $"{FormatFeaturesBits(data.Features)}    "
        + $"{data.NameID,3}    {data.Short0A,3}      {data.DungeonBalanceIndex,3}       {data.Byte13,3}    {data.Byte17,3}    {data.Byte18,3}    {data.Byte19,3}");

    // Print floor infos
    Console.WriteLine($"   {"Index",5}  {"Invit",5}  {"Weather",-9} {"S02",5}  {"Turns",5}  {"Money",12}  {"MapDat",6}  {"NameID",7}  {"B2D",3}  {"B2E",3}  "
            + $"{"B2F",3}  {"B30",5}  {"B32",5}  {"B34",3}  {"B35",3}  {"Enemies",9}  {"Traps",9}  {"B46",3}  {"B47",3}  {"B49",3}  {"B4A",3}  {"B4F",3}  {"B50",3}  {"B51",3}  "
            + $"{"ItemSet",7}  {"BuriedSet",9}  {"KecShop%",8}  {"MonHouse%",9}  {"MysHouse%",9}  {"MysHsSize",9}  {"B56",3}  {"B57",3}  {"B58",3}");
    foreach (var info in floorInfos)
    {
        string weather = info.Weather != DungeonStatusIndex.NONE ? strings.DungeonStatuses[info.Weather] : "(none)";
        if (string.IsNullOrWhiteSpace(weather))
        {
            weather = $"(\"{info.Weather}\")";
        }
        string mysteryHouseSize = info.MysteryHouseSize == 0 ? "small"
            : info.MysteryHouseSize == 1 ? "large"
            : $"({info.MysteryHouseSize})";

        Console.WriteLine($"   {info.Index,5}  "
            + $"{info.InvitationIndex,5}  "
            + $"{weather,-9} "
            + $"{info.Short02,5}  "
            + $"{info.TurnLimit,5}  "
            + $"{$"{info.MinMoneyStackSize}..{info.MaxMoneyStackSize}",12}  "
            + $"{info.DungeonMapDataInfoIndex,6}  "
            + $"{info.NameID,7}  "
            + $"{info.Byte2D,3}  "
            + $"{info.Byte2E,3}  "
            + $"{info.Byte2F,3}  "
            + $"{info.Short30,5}  "
            + $"{info.Short32,5}  "
            + $"{info.Byte34,3}  "
            + $"{info.Byte35,3}  "
            + $"{$"{info.MinEnemyDensity}..{info.MaxEnemyDensity}",9}  "
            + $"{$"{info.MinTrapDensity}..{info.MaxTrapDensity}",9}  "
            + $"{info.Byte46,3}  "
            + $"{info.Byte47,3}  "
            + $"{info.Byte49,3}  "
            + $"{info.Byte4A,3}  "
            + $"{info.Byte4F,3}  "
            + $"{info.Byte50,3}  "
            + $"{info.Byte51,3}  "
            + $"{info.ItemSetIndex,7}  "
            + $"{info.BuriedItemSetIndex,9}  "
            + $"{info.KecleonShopChance,8}  "
            + $"{info.MonsterHouseChance,9}  "
            + $"{info.MysteryHouseChance,9}  "
            + $"{mysteryHouseSize,9}  "
            + $"{info.Byte56,3}  "
            + $"{info.Byte57,3}  "
            + $"{info.Byte58,3}  "
            + $"{info.Event,3}  "
            + $"{string.Join(",", info.Bytes37to43)}  "
            + $"{string.Join(",", info.Bytes5Ato61)}");

        // Print floor items
        /*Console.WriteLine($"   Floor {info.Index} items:");
        Console.Write("     ");
        int i = 0;
        foreach (var itemWeight in items.ItemSets[info.ItemSetIndex].ItemWeights)
        {
            var itemName = strings.GetItemName(itemWeight.Index);
            if (string.IsNullOrEmpty(itemName))
            {
                itemName = itemWeight.Index.ToString();
            }
            Console.Write($"{itemName} ({itemWeight.Weight}), ");
            if (i == 8)
            {
                Console.WriteLine();
                Console.Write("     ");
                i = 0;
            }
            i++;
        }
        Console.WriteLine();*/
    }

    // Print fainted Pokemon
    /*var faintedPokemon = (from pokemon in fixedPokemon
                          where (int)pokemon.DungeonIndex == data.Index
                          select GetPokemonName(pokemon.PokemonId)).ToList();
    if (faintedPokemon.Count > 0)
    {
        Console.WriteLine($"  Fainted Pokemon: {string.Join(", ", faintedPokemon)}");
    }*/

    // Print Pokemon in Mystery Houses
    /*int prevInvitationIndex = -1;
    var mysteryHousePokemon = new List<string>();
    foreach (var info in from info in floorInfos
                         where info.InvitationIndex != 0 && prevInvitationIndex != info.InvitationIndex
                         select info)
    {
        prevInvitationIndex = info.InvitationIndex;
        mysteryHousePokemon.AddRange(from pokemon in fixedPokemon
                                     where pokemon.InvitationIndex == info.InvitationIndex
                                     select GetPokemonName(pokemon.PokemonId));
    }

    if (mysteryHousePokemon.Count > 0)
    {
        Console.WriteLine($"  Mystery House Pokemon: {string.Join(", ", mysteryHousePokemon)}");
    }*/

    // Print wild Pokemon
    /*if (wildPokemon != null)
    {
        Console.WriteLine("      #   Pokemon         Lvl    HP   Atk   Def   SpA   SpD   Spe    XP Yield");
        Console.WriteLine("             Spawn  Recruit");
        Console.WriteLine("      Floor   rate   level   0x0B");
        var stats = wildPokemon.Stats;
        var floors = wildPokemon.Floors;

        foreach (var stat in stats)
        {
            var index = stat.Index + 1;
            var name = GetPokemonName(stat.CreatureIndex);
            if (true)
            {
                var strongFoe = (stat.StrongFoe != 0) ? "Strong Foe" : "";
                Console.WriteLine($"   {index,4}   "
                    + $"{name,-14}  "
                    + $"{stat.Level,3}   "
                    + $"{stat.HitPoints,3}   "
                    + $"{stat.Attack,3}   "
                    + $"{stat.Defense,3}   "
                    + $"{stat.SpecialAttack,3}   "
                    + $"{stat.SpecialDefense,3}   "
                    + $"{stat.Speed,3}    "
                    + $"{stat.XPYield,8}   "
                    + $"{strongFoe}");

                for (int k = 0; k < dungeon.Extra.Floors; k++)
                {
                    var floor = floors[k].Entries[stat.Index];
                    if (floor.SpawnRate != 0)
                    {
                        Console.WriteLine($"       {FormatFloor(dungeon, k + 1),4}    {floor.SpawnRate,3}    {floor.RecruitmentLevel,3}    {floor.Byte0B,3}");
                    }
                }
            }
        }
    }*/

    // Print trap weights
    /*if (trapWeights != null)
    {
        var records = trapWeights.Records;
        int prevIndex = -1;
        var prevWeights = new List<short>();
        var len = dungeon.Extra.Floors; // records.Length - 1
        for (int j = 0; j < len; j++)
        {
            var record = records[j];
            var weights = new List<short>();
            
            foreach (var entry in record.Entries)
            {
                weights.Add(entry.Weight);
            }

            if (prevWeights.Count == 0 || !prevWeights.SequenceEqual(weights))
            {
                prevWeights = weights;
                if (prevIndex != -1 && prevIndex != j)
                {
                    Console.WriteLine($"..{FormatFloor(dungeon, len)}: *");
                }
                Console.Write($"  {FormatFloor(dungeon, j + 1)}: ");

                int totalWeight = weights.Where(weight => weight >= 0).Sum(weight => weight);
                for (int i = 0; i < weights.Count - 1; i++)
                {
                    int weight = weights[i];
                    if (weight == 0)
                    {
                        continue;
                    }

                    var itemIndex = ItemIndexConstants.TRAP_MIN + i;
                    string trapName = strings.GetItemName(itemIndex);
                    if (string.IsNullOrEmpty(trapName))
                    {
                        trapName = itemIndex.ToString();
                    }
                    float probability = ((float) weight / totalWeight) * 100f;
                    Console.Write($"{trapName} ({weight}, {probability:0.00}%), ");
                }
                Console.WriteLine();

                prevIndex = j + 1;
            }

        }
        if (prevIndex != -1 && prevIndex != len) {
            Console.WriteLine($"..{FormatFloor(dungeon, len)}: *");
        }
    }*/

    // Print unknown (and mostly uninteresting) data from the fourth SIR0 file in dungeon_balance.bin
    /*if (data4 != null)
    {
        for (int i = 0; i < data4.Records.Length; i++)
        {
            DungeonBalance.DungeonBalanceDataEntry4.Record record = data4.Records[i];
            var short00s = new List<short>();
            var short02s = new List<short>();
            var int04s = new List<int>();

            foreach (var entry in record.Entries)
            {
                short00s.Add(entry.Short00);
                short02s.Add(entry.Short02);
                int04s.Add(entry.Int04);
            }

            Console.WriteLine($"  {FormatFloor(dungeon, i + 1)}: {string.Join("  ", short00s)}");
            Console.WriteLine($"     {string.Join("  ", short02s)}");
            Console.WriteLine($"     {string.Join("  ", int04s)}");
        }
    }*/
}
