#load "../../../Stubs/Rtdx.csx"

using System;

var db = Rom.GetPokemonGraphicsDatabase();
foreach (var entry in db.Entries)
{
    Console.WriteLine($"{entry.ModelName}|{entry.AnimationName}");
}