#load "../../../Stubs/Rtdx.csx"

using System;

var db = Rom.GetWazaDataInfo();
var tameMoves = Rom.GetTameMoveList().Entries;
var xlMoves = Rom.GetXLMoveList().Entries;
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

Console.WriteLine("\"Tame\" moves:");
foreach (var entry in tameMoves)
{
    if (strings.Moves.TryGetValue(entry, out string name))
    {
        Console.WriteLine($"  {entry}  {name}");
    }
    else
    {
        Console.WriteLine($"  {entry}");
    }
}

Console.WriteLine(@"XL moves:");
foreach (var entry in xlMoves)
{
    if (strings.Moves.TryGetValue(entry, out string name))
    {
        Console.WriteLine($"  {entry}  {name}");
    }
    else
    {
        Console.WriteLine($"  {entry}");
    }
}
