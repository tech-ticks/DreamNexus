#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

public string FormatBits(ushort b)
{
    var binary = Convert.ToString(b, 2);
    return binary.Replace('0', '-').Replace('1', '#').PadLeft(16, '-');
}

var itemDataInfo = Rom.GetItemDataInfo().Entries;
var strings = Rom.GetCommonStrings();
var i = 0;
foreach (var item in itemDataInfo)
{
    //Console.WriteLine($"{(int) item.Key} {0x((int) item.Key).ToString("x")} {item.Key}: {item.Value}");
    var itemName = strings.Items.ContainsKey((ItemIndex)i) ? strings.Items[(ItemIndex)i] : "(none)";
    var tmMoveName = strings.Moves.ContainsKey(item.TaughtMove) ? strings.Moves[item.TaughtMove] : "";
    //var desc = $"[{item.ItemKind}] ({item.Symbol}) {itemName}";
    var desc = $"[{item.ItemKind}] {itemName}";
    Console.Write($"{i,3}:");
    Console.Write($" {desc,-40} ");
    Console.Write($" {item.Short00,5} ");
    Console.Write($" {item.Short02,5} ");
    Console.Write($" {FormatBits(item.Short04)} ");
    Console.Write($" {item.BuyPrice,5} ");
    Console.Write($" {item.SellPrice,5} ");
    Console.Write($" {tmMoveName,-20} ");
    Console.Write($" {item.Short0C,5} ");
    Console.Write($" {item.PrimaryActIndex,5} ");
    Console.Write($" {item.ReviveActIndex,5} ");

    Console.Write($" {item.ThrowActIndex,5} ");
    Console.Write($" {item.Byte17,3} ");
    Console.Write($" {item.Byte18,3} ");
    Console.Write($" {item.Byte19,3} ");
    Console.Write($" {item.Byte1A,3} ");
    Console.Write($" {item.Byte1B,3} ");
    Console.Write($" {item.Byte1C,3} ");
    Console.Write($" {item.Byte1D,3} ");
    Console.Write($" {item.Byte1E,3} ");
    Console.Write($" {item.Byte1F,3} ");
    Console.Write($" {item.Byte20,3} ");
    Console.WriteLine();
    i++;
}
