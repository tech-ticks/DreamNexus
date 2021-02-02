#load "../../../Stubs/Rtdx.csx"

using System;

var db = Rom.GetDungeonMapDataInfo();
var dungeons = Rom.GetDungeons().Dungeons;

Console.WriteLine($"   {"Index",5}  {"FixedMap",8}  {"BgmSymbol",9}  {"06",3}  {"07",3}  {"09",3}  {"0A",3}  {"0B",3}");
foreach (var info in db.Entries)
{
    Console.Write($"   {info.Index,-5}  "
        + $"{info.FixedMapIndex,8}  "
        + $"{info.DungeonBgmSymbolIndex,9}  "
        + $"{info.Byte06,3}  "
        + $"{info.Byte07,3}  "
        + $"{info.Byte09,3}  "
        + $"{info.Byte0A,3}  "
        + $"{info.Byte0B,3}  ");

    if (info.Index == 0)
    {
        Console.WriteLine();
        continue;
    }

    foreach (var dungeon in dungeons)
    {
        var floorsWithMap = dungeon.Balance.FloorInfos
            .Where(floorInfo => floorInfo.DungeonMapDataInfoIndex == info.Index)
            .ToArray();

        if (floorsWithMap.Length == 0)
        {
            continue;
        }

        var formattedFloors = floorsWithMap.Select(floor => floor.Index.ToString()).ToList();
        Console.Write($"{dungeon.DungeonName} (");
        Console.Write(string.Join(", ", formattedFloors));
        Console.Write("), ");
    }

    Console.WriteLine();
}
