#load "../../../Stubs/Rtdx.csx"

using System;

var db = Rom.GetWazaDataInfo();
var acts = Rom.GetActDataInfo().Entries;
var tameMoves = Rom.GetTameMoveList().Entries;
var xlMoves = Rom.GetXLMoveList().Entries;
var strings = Rom.GetCommonStrings();
var dungeonBin = Rom.GetDungeonBinEntry();
var i = 0;
foreach (var entry in db.Entries)
{
    var moveName = strings.Moves.ContainsKey((WazaIndex)i) ? strings.Moves[(WazaIndex)i] : ((WazaIndex)i).ToString();
    var act = acts[entry.ActIndex];
    var text1 = dungeonBin.GetStringByHash((int)act.Text08);
    var text2 = dungeonBin.GetStringByHash((int)act.Text0C);
    Console.WriteLine($"#{i,-3} {moveName,-25}  {entry.ActIndex,5}  {entry.Short00,3}  {entry.Short02,3}  {entry.Short04,3}  {entry.Short0E,5}  {entry.Byte10,3}  {entry.Byte11,3}");
    if (!string.IsNullOrEmpty(text1))
    {
        Console.WriteLine($"  Dungeon message 1: \"{text1}\"");
    }
    if (!string.IsNullOrEmpty(text2))
    {
        Console.WriteLine($"  Dungeon message 2: \"{text2}\"");
    }
    i++;
}
Console.WriteLine();

/*Console.WriteLine("\"Tame\" moves:");
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
Console.WriteLine();*/

/*Console.WriteLine(@"XL moves:");
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
}*/
