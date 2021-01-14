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
var strings = Rom.GetDungeonBinEntry();
var i = 0;
foreach (var entry in data)
{
    var text1 = strings.GetStringByHash((int)entry.Text08);
    var text2 = strings.GetStringByHash((int)entry.Text0C);
    Console.Write($"#{i,-3} ");
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

    Console.Write($" {entry.Byte80,3}");
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
    Console.Write($" {entry.Byte97,3}");

    Console.WriteLine();
    if (!string.IsNullOrEmpty(text1))
    {
        Console.WriteLine($"  Text line 1: \"{text1}\"");
    }
    if (!string.IsNullOrEmpty(text2))
    {
        Console.WriteLine($"  Text line 2: \"{text2}\"");
    }
    i++;
}
