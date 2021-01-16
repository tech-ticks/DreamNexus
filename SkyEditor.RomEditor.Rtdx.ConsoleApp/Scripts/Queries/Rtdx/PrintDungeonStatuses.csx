#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

var statuses = Rom.GetCommonStrings().DungeonStatuses;
foreach (var status in statuses)
{
    Console.WriteLine($"{(int)status.Key,3}: {status.Key,-15}  {status.Value}");
}
