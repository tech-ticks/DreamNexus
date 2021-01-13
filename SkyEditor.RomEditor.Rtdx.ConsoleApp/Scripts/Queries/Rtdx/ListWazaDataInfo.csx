#load "../../../Stubs/Rtdx.csx"

using System;

var db = Rom.GetWazaDataInfo();
var strings = Rom.GetCommonStrings();
foreach (var entry in db.Entries)
{
    if (strings.Moves.TryGetValue(entry.Index, out string name))
    {
        Console.WriteLine($"{entry.Index,-25}  {name,-17}  {entry.Short00,3}  {entry.Short02,3}  {entry.Short04,3}  {entry.Short0E,5}  {entry.Byte10,3}  {entry.Byte11,3}");
    }
    else
    {
        Console.WriteLine($"{entry.Index,-25}  (none)             {entry.Short00,3}  {entry.Short02,3}  {entry.Short04,3}  {entry.Short0E,5}  {entry.Byte10,3}  {entry.Byte11,3}");
    }
}
