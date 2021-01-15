#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

public string FormatBits(ulong b)
{
    var binary = Convert.ToString((long)b, 2);
    return binary.Replace('0', '-').Replace('1', '#').PadLeft(64, '-');
}

var data = Rom.GetActDataInfo().Entries;
var effectData = Rom.GetActEffectDataInfo().Entries;
var hitCountData = Rom.GetActHitCountTableDataInfo().Entries;
var strings = Rom.GetDungeonBinEntry();
for (var i = 0; i < data.Count; i++)
{
    var entry = data[i];
    var effectEntry = effectData[i];
    var hitCountEntry = hitCountData[entry.ActHitCountIndex];
    var text1 = strings.GetStringByHash((int)entry.Text08);
    var text2 = strings.GetStringByHash((int)entry.Text0C);
    Console.Write($"{i};");
    Console.Write($"{entry.ActHitCountIndex};");
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
    Console.Write($"{entry.Short28};");
    Console.Write($"{entry.Short2A};");
    Console.Write($"{entry.Short2C};");
    Console.Write($"{entry.Short2E};");

    Console.Write($"{entry.Short32};");
    Console.Write($"{entry.Short34};");
    Console.Write($"{entry.Short36};");
    Console.Write($"{entry.Short38};");
    Console.Write($"{entry.Short3A};");
    Console.Write($"{entry.Short40};");
    Console.Write($"{entry.Short42};");

    Console.Write($"{entry.Short48};");
    Console.Write($"{entry.Short4A};");

    Console.Write($"{entry.Short50};");
    Console.Write($"{entry.Short52};");
    Console.Write($"{entry.Short54};");

    Console.Write($"{entry.Short58};");
    Console.Write($"{entry.Short5A};");

    Console.Write($"{entry.Short98};");
    Console.Write($"{entry.Short9A};");

    Console.Write($"{entry.Byte5C};");
    Console.Write($"{entry.Byte5D};");
    Console.Write($"{entry.Byte5E};");
    Console.Write($"{entry.Byte5F};");
    Console.Write($"{entry.Byte60};");
    Console.Write($"{entry.Byte61};");
    Console.Write($"{entry.Byte62};");

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

    Console.Write($"{entry.Byte70};");
    Console.Write($"{entry.Byte71};");

    Console.Write($"{entry.Byte74};");
    Console.Write($"{entry.Byte75};");
    Console.Write($"{entry.Byte76};");
    Console.Write($"{entry.Byte77};");
    Console.Write($"{entry.Byte78};");
    Console.Write($"{entry.Byte79};");
    Console.Write($"{entry.Byte7A};");

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
    Console.Write($"{entry.Byte88};");
    Console.Write($"{entry.Byte89};");
    Console.Write($"{entry.Byte8A};");
    Console.Write($"{entry.Byte8B};");
    Console.Write($"{entry.Byte8C};");
    Console.Write($"{entry.Byte8D};");
    Console.Write($"{entry.Byte8E};");
    Console.Write($"{entry.Byte8F};");
    Console.Write($"{entry.Byte90};");
    Console.Write($"{entry.Byte91};");
    Console.Write($"{entry.Byte92};");
    Console.Write($"{entry.Byte94};");
    Console.Write($"{entry.Byte95};");
    Console.Write($"{entry.Byte96};");
    Console.Write($"{entry.Byte97};");
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
    Console.Write($"{hitCountEntry.MinHits};");
    Console.Write($"{hitCountEntry.MaxHits};");
    Console.Write($"{hitCountEntry.StopOnMiss};");
    Console.Write($"::;");
    Console.Write($"{text1};");
    Console.Write($"{text2}");
    Console.WriteLine();

    //Console.Write($"#{i,-3} ");
    //Console.Write($" {FormatBits(entry.Long00)}");
    /*Console.Write($" {entry.Short10,5}");
    Console.Write($" {entry.Short12,5}");
    Console.Write($" {entry.Short14,5}");
    Console.Write($" {entry.Short16,5}");
    Console.Write($" {entry.Short18,5}");
    Console.Write($" {entry.Short1A,5}");
    Console.Write($" {entry.Short1C,5}");
    Console.Write($" {entry.Short1E,5}");
    Console.Write($" {entry.Short20,5}");
    Console.Write($" {entry.Short22,5}");
    Console.Write($" {entry.Short24,5}");
    Console.Write($" {entry.Short26,5}");
    Console.Write($" {entry.Short28,5}");
    Console.Write($" {entry.Short2A,5}");
    Console.Write($" {entry.Short2C,5}");
    Console.Write($" {entry.Short2E,5}");

    Console.Write($" {entry.Short32,5}");
    Console.Write($" {entry.Short34,5}");
    Console.Write($" {entry.Short36,5}");
    Console.Write($" {entry.Short38,5}");
    Console.Write($" {entry.Short3A,5}");*/

    /*Console.Write($" {entry.Short40,5}");
    Console.Write($" {entry.Short42,5}");

    Console.Write($" {entry.Short48,5}");
    Console.Write($" {entry.Short4A,5}");

    Console.Write($" {entry.Short50,5}");
    Console.Write($" {entry.Short52,5}");
    Console.Write($" {entry.Short54,5}");

    Console.Write($" {entry.Short58,5}");
    Console.Write($" {entry.Short5A,5}");

    Console.Write($" {entry.Short98,5}");
    Console.Write($" {entry.Short9A,5}");*/

    /*Console.Write($" {entry.Byte5C,3}");
    Console.Write($" {entry.Byte5D,3}");
    Console.Write($" {entry.Byte5E,3}");
    Console.Write($" {entry.Byte5F,3}");
    Console.Write($" {entry.Byte60,3}");
    Console.Write($" {entry.Byte61,3}");
    Console.Write($" {entry.Byte62,3}");

    Console.Write($" {entry.Byte64,3}");
    Console.Write($" {entry.Byte65,3}");
    Console.Write($" {entry.Byte66,3}");
    Console.Write($" {entry.Byte67,3}");
    Console.Write($" {entry.Byte68,3}");
    Console.Write($" {entry.Byte69,3}");
    Console.Write($" {entry.Byte6A,3}");
    Console.Write($" {entry.Byte6B,3}");
    Console.Write($" {entry.Byte6C,3}");
    Console.Write($" {entry.Byte6D,3}");*/

    /*Console.Write($" {entry.Byte70,3}");
    Console.Write($" {entry.Byte71,3}");

    Console.Write($" {entry.Byte74,3}");
    Console.Write($" {entry.Byte75,3}");
    Console.Write($" {entry.Byte76,3}");
    Console.Write($" {entry.Byte77,3}");
    Console.Write($" {entry.Byte78,3}");
    Console.Write($" {entry.Byte79,3}");
    Console.Write($" {entry.Byte7A,3}");

    Console.Write($" {entry.Byte7C,3}");
    Console.Write($" {entry.Byte7D,3}");
    Console.Write($" {entry.Byte7E,3}");
    Console.Write($" {entry.Byte7F,3}");*/

    /*Console.Write($" {entry.Byte80,3}");
    Console.Write($" {entry.Byte81,3}");
    Console.Write($" {entry.Byte82,3}");
    Console.Write($" {entry.Byte83,3}");
    Console.Write($" {entry.Byte84,3}");
    Console.Write($" {entry.Byte85,3}");
    Console.Write($" {entry.Byte86,3}");
    Console.Write($" {entry.Byte87,3}");
    Console.Write($" {entry.Byte88,3}");
    Console.Write($" {entry.Byte89,3}");
    Console.Write($" {entry.Byte8A,3}");
    Console.Write($" {entry.Byte8B,3}");
    Console.Write($" {entry.Byte8C,3}");
    Console.Write($" {entry.Byte8D,3}");
    Console.Write($" {entry.Byte8E,3}");
    Console.Write($" {entry.Byte8F,3}");
    Console.Write($" {entry.Byte90,3}");
    Console.Write($" {entry.Byte91,3}");
    Console.Write($" {entry.Byte92,3}");
    Console.Write($" {entry.Byte93,3}");
    Console.Write($" {entry.Byte94,3}");
    Console.Write($" {entry.Byte95,3}");
    Console.Write($" {entry.Byte96,3}");
    Console.Write($" {entry.Byte97,3}");*/

    /*Console.WriteLine();
    if (!string.IsNullOrEmpty(text1))
    {
        Console.WriteLine($"  Text line 1: \"{text1}\"");
    }
    if (!string.IsNullOrEmpty(text2))
    {
        Console.WriteLine($"  Text line 2: \"{text2}\"");
    }*/
}
