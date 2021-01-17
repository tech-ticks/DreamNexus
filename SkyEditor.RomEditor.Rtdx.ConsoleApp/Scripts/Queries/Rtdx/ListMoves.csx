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
var chargedMoves = Rom.GetChargedMoves().Entries;
var xlMoves = Rom.GetExtraLargeMoves().Entries;
var strings = Rom.GetCommonStrings();
var dungeonBin = Rom.GetDungeonBinEntry();
var i = 0;
/*Console.WriteLine("#;ActIndex;Name;Type;Category;MinHits;MaxHits;Stop on miss;Short00;Short02;Short04;Short0E;Byte10;Byte11;Text 1;Text 2");
foreach (var entry in db.Entries)
{
    var act = acts[entry.ActIndex];
    var moveName = strings.Moves.ContainsKey((WazaIndex)i) ? strings.Moves[(WazaIndex)i] : ((WazaIndex)i).ToString();
    var type = strings.PokemonTypes.ContainsKey(act.MoveType) ? strings.PokemonTypes[act.MoveType] : act.MoveType.ToString();
    var category = GetCategoryName(act.MoveCategory);
    var hitCountEntry = hitCountData[act.ActHitCountIndex];
    var text1 = dungeonBin.GetStringByHash((int)act.DungeonMessage1);
    var text2 = dungeonBin.GetStringByHash((int)act.DungeonMessage2);
    Console.Write($"{i};");
    Console.Write($"{entry.ActIndex};");
    Console.Write($"{moveName};");
    Console.Write($"{type};");
    Console.Write($"{category};");
    Console.Write($"{hitCountEntry.MinHits};");
    Console.Write($"{hitCountEntry.MaxHits};");
    Console.Write($"{hitCountEntry.StopOnMiss};");
    Console.Write($"{entry.Short00};");
    Console.Write($"{entry.Short02};");
    Console.Write($"{entry.Short04};");
    Console.Write($"{entry.Short0E};");
    Console.Write($"{entry.Byte10};");
    Console.Write($"{entry.Byte11};");
    Console.Write($"{text1};");
    Console.Write($"{text2};");
    Console.WriteLine();

    Console.Write($"#{i,-3} {moveName,-25}  {type,-8}  {category,-7}");
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
Console.WriteLine();*/

public string GetMoveName(WazaIndex index)
{
    if (strings.Moves.TryGetValue(index, out string name))
    {
        return $"({(ushort)index}) {name}";
    }
    else
    {
        return $"({(ushort)index}) {index}";
    }
}

Console.WriteLine("Charged moves:");
foreach (var entry in chargedMoves)
{
    var baseName = GetMoveName(entry.BaseMove);
    var finalName = GetMoveName(entry.FinalMove);
    Console.WriteLine($"  {baseName,-25}  {entry.BaseAction,5}  {finalName,-25}  {entry.FinalAction,5}  {entry.Short08,5}");
}
Console.WriteLine();

Console.WriteLine("XL moves:");
foreach (var entry in xlMoves)
{
    var baseName = GetMoveName(entry.BaseMove);
    var xlName = GetMoveName(entry.LargeMove);
    Console.WriteLine($"  {baseName,-25}  {xlName,-25}  {entry.BaseAction,5}  {entry.LargeAction,5}");
}
