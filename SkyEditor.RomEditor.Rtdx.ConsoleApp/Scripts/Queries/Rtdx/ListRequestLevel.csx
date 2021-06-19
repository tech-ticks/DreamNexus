#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

var dungeons = Rom.GetDungeons();
var entries = Rom.GetRequestLevel().Entries;

Console.WriteLine($"#    {"Name",-30}   {"DungeonIndex",-13}  {"AccFloorCnt",9}  {"Unk1",11}  {"TotalFloorCnt",11}");
for (int i = 1; i < (int)DungeonIndex.END; i++)
{
    var entry = entries[(DungeonIndex)i];
    var main = entry.MainEntry;
    Console.WriteLine($"{i,-3}  "
        + $"{dungeons.GetDungeonById((DungeonIndex)i).DungeonName,-30}  "
        + $"{main.DungeonIndex,-13}  "
        + $"{main.AccessibleFloorCount,9}  "
        + $"{main.Unk1,11}  "
        + $"{main.TotalFloorCount,11}  ");
    Console.WriteLine($"     Entry2 ({entry.UnkEntry2.Count} items): ");
    Console.WriteLine("       " + string.Join(", ", entry.UnkEntry2));

    Console.WriteLine($"#     NameID    FloorIndex   S4   S6   S8   IsBoss");

    for (int j = 0; j < main.FloorData.Count; j++) {
        var data = main.FloorData[j];
        Console.WriteLine($"{j,-3}   "
            + $"{data.NameID,6}  "
            + $"{data.FloorIndex,12}  "
            + $"{data.Short4,3}  "
            + $"{data.Short6,3}  "
            + $"{data.Short8,3}  "
            + $"{data.IsBossFloor,7}  ");
    }

    Console.WriteLine();
}
