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

var db = Rom.GetWazaDataInfo();
var acts = Rom.GetActDataInfo().Entries;
var hitCountData = Rom.GetActHitCountTableDataInfo().Entries;
var tameMoves = Rom.GetTameMoveList().Entries;
var xlMoves = Rom.GetXLMoveList().Entries;
var strings = Rom.GetCommonStrings();
var dungeonBin = Rom.GetDungeonBinEntry();
var i = 0;
foreach (var entry in db.Entries)
{
    var act = acts[entry.ActIndex];
    var moveName = strings.Moves.ContainsKey((WazaIndex)i) ? strings.Moves[(WazaIndex)i] : ((WazaIndex)i).ToString();
    var type = strings.PokemonTypes.ContainsKey(act.MoveType) ? strings.PokemonTypes[act.MoveType] : act.MoveType.ToString();
    var category = GetCategoryName(act.MoveCategory);
    var hitCountEntry = hitCountData[act.ActHitCountIndex];
    var text1 = dungeonBin.GetStringByHash((int)act.Text08);
    var text2 = dungeonBin.GetStringByHash((int)act.Text0C);
    Console.Write($"#{i,-3} {moveName,-25}  {type,-8}  {category,-7}");
    //Console.Write($"  {entry.ActIndex,5}  {entry.Short00,3}  {entry.Short02,3}  {entry.Short04,3}  {entry.Short0E,5}  {entry.Byte10,3}  {entry.Byte11,3}");
    if (hitCountEntry.Index > 1)
    {
        if (hitCountEntry.StopOnMiss != 0)
        {
            Console.Write($"  up to {hitCountEntry.MaxHits} hits");
        }
        else if (hitCountEntry.MinHits == hitCountEntry.MaxHits)
        {
            Console.Write($"  {hitCountEntry.MaxHits} hits");
        }
        else
        {
            double weightSum = 0;
            for (var i = hitCountEntry.MinHits; i <= hitCountEntry.MaxHits; i++)
            {
                weightSum += hitCountEntry.Weights[i - hitCountEntry.MinHits];
            }

            Console.Write($"  {hitCountEntry.MinHits} to {hitCountEntry.MaxHits} hits (");
            for (var i = hitCountEntry.MinHits; i <= hitCountEntry.MaxHits; i++)
            {
                var weight = hitCountEntry.Weights[i - hitCountEntry.MinHits];
                double chance = weight / weightSum * 100.0;
                if (i > hitCountEntry.MinHits)
                {
                    Console.Write(" ");
                }
                Console.Write($"{chance:f1}%");
            }
            Console.Write(")  (");
            for (var i = hitCountEntry.MinHits; i <= hitCountEntry.MaxHits; i++)
            {
                var weight = hitCountEntry.Weights[i - hitCountEntry.MinHits];
                if (i > hitCountEntry.MinHits)
                {
                    Console.Write(" ");
                }
                Console.Write($"{weight}");
            }
            Console.Write(")");
        }
    }
    Console.WriteLine();

    /*if (!string.IsNullOrEmpty(text1))
    {
        Console.WriteLine($"  Dungeon message 1: \"{text1}\"");
    }
    if (!string.IsNullOrEmpty(text2))
    {
        Console.WriteLine($"  Dungeon message 2: \"{text2}\"");
    }*/
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
