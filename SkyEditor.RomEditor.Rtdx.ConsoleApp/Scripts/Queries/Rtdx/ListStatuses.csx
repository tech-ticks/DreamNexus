#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

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

var db = Rom.GetStatusDataInfo();
var strings = Rom.GetCommonStrings();
var dungeonBin = Rom.GetDungeonBinEntry();
var i = 0;
foreach (var entry in db.Entries)
{
    var statusName = strings.Statuses.ContainsKey(entry.Index) ? strings.Statuses[entry.Index] : entry.Index.ToString();
    var messageAdded = dungeonBin.GetStringByHash((int)entry.ApplyMessage);
    var messageRemoved = dungeonBin.GetStringByHash((int)entry.RemoveMessage);
    var messageAlreadyApplied = dungeonBin.GetStringByHash((int)entry.AlreadyAppliedMessage);
    Console.WriteLine($"{i,3}: {statusName,-20}  {entry.Byte0C,3}  {entry.Byte0D,3}  {entry.Byte0E,3}  {entry.Byte0F,3}  {entry.Short10,5}  {entry.Short12,5}  {entry.Short14,5}  {entry.Short16,5}  {entry.Short18,5}  {entry.Short1A,5}  {entry.MinDuration,6}  {entry.MaxDuration,6}  {entry.Short20,5}  {entry.Short22,5}  {entry.Short24,6}  {entry.Short26,5}  {entry.Short28,5}  {entry.Short2A,5}  {entry.Short2C,5}  {entry.Short2E,5}");
    // Console.WriteLine($"       \"{messageAdded}\"");
    // Console.WriteLine($"       \"{messageRemoved}\"");
    // Console.WriteLine($"       \"{messageAlreadyApplied}\"");
    // Console.WriteLine();
    i++;
}
