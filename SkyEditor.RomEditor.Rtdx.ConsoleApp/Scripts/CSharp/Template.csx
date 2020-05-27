#load "SkyEditor.csx"

using System;

if (Rom == null) 
{
    throw new Exception("Script must be run in the context of Sky Editor");
}

var starters = Rom.GetStarters().Starters;
for (int i = 0; i < starters.Length; i++)
{
    var starter = starters[i];
    Console.WriteLine($"{i} {starter.PokemonName}");
}
