#load "../../../Stubs/PSMD.csx"

using System;

var db = Rom.GetPokemonGraphicsDatabase();
foreach (var entry in db.Entries)
{
    Console.WriteLine($"{entry.BchFilename}|{entry.AnimationName}|{entry.PortraitName}");
}