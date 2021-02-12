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
var itemGraphics = Rom.GetItemGraphics().Entries;
var strings = Rom.GetCommonStrings();
var i = 0;

Console.WriteLine("#;Kind;Symbol;Name;ItemGraphicsKey;Flags;Buy Price;Sell Price;TM Move;Short0C;Primary Action Index;Revive Action Index;Throw Action Index;Byte17;Byte18;Byte19;Byte1A;Byte1B;Byte1C;Byte1D;IconIndex;Byte1F;Byte20;ModelName");
foreach (var item in itemDataInfo)
{
    var itemName = strings.Items.ContainsKey((ItemIndex)i) ? strings.Items[(ItemIndex)i] : "(none)";
    var tmMoveName = strings.Moves.ContainsKey(item.TaughtMove) ? strings.Moves[item.TaughtMove] : "";
    //var desc = $"[{item.ItemKind}] ({item.Symbol}) {itemName}";
    var desc = $"[{item.ItemKind}] {itemName}";
    Console.Write($"{i};");
    Console.Write($"{item.ItemKind};");
    Console.Write($"{item.Symbol};");
    Console.Write($"{itemName};");
    Console.Write($"{item.ItemGraphicsKey};");
    Console.Write($"{FormatBits((ushort) item.Flags)};");
    Console.Write($"{item.BuyPrice};");
    Console.Write($"{item.SellPrice};");
    Console.Write($"{tmMoveName};");
    Console.Write($"{item.Short0C};");
    Console.Write($"{item.PrimaryActIndex};");
    Console.Write($"{item.ReviveActIndex};");
    Console.Write($"{item.ThrowActIndex};");
    Console.Write($"{item.Byte17};");
    Console.Write($"{item.Byte18};");
    Console.Write($"{item.Byte19};");
    Console.Write($"{item.Byte1A};");
    Console.Write($"{item.Byte1B};");
    Console.Write($"{item.Byte1C};");
    Console.Write($"{item.Byte1D};");
    Console.Write($"{item.IconIndex};");
    Console.Write($"{item.Byte1F};");
    Console.Write($"{item.Byte20};");
    if (itemGraphics.ContainsKey(item.ItemGraphicsKey))
    {
        Console.Write($"{itemGraphics[item.ItemGraphicsKey].ModelName}");
    }
    Console.WriteLine();
    /*Console.Write($"{i,3}:");
    Console.Write($" {desc,-40} ");
    Console.Write($" {item.ItemGraphicsKey,5} ");
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
    Console.Write($" {item.IconIndex,3} ");
    Console.Write($" {item.Byte1F,3} ");
    Console.Write($" {item.Byte20,3} ");
    Console.WriteLine();*/
    i++;
}
