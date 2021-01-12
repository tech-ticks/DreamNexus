#load "../../../Stubs/Rtdx.csx"

using System;

var db = Rom.GetDungeonMapDataInfo();
Console.WriteLine($"Index: FixedMapIndex, DungeonBgmSymbolIndex");

foreach (var entry in db.Entries)
{
    Console.WriteLine($"{entry.Index}: {entry.FixedMapIndex}, {entry.DungeonBgmSymbolIndex}");
}
