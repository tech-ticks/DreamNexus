#load "SkyEditor.csx"

using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

if (Rom == null)
{
    throw new Exception("Script must be run in the context of Sky Editor");
}

var random = new Random();

// The game contains Pokémon from Pokémon Super Mystery dungeon, but they're not all useable unless they appear in Rescue Team Deluxe naturally.
// All the missing Pokémon have dummy graphics, so that'll be our cue for which ones we can use
var usableGraphicsEntryIndexes = new HashSet<int>();
var graphics = Rom.GetPokemonGraphicsDatabase();
for (int i = 0; i < graphics.Entries.Count; i++)
{
    var graphicsEntry = graphics.Entries[i];
    if (!graphicsEntry.ModelName.Contains("dummy"))
    {
        usableGraphicsEntryIndexes.Add(i);
    }
}

bool isPokemonUsable(CreatureIndex creatureIndex)
{
    var formDatabase = Rom.GetPokemonFormDatabase();
    var graphicsIndex = formDatabase.GetGraphicsDatabaseIndex(creatureIndex, PokemonFormType.NORMAL);
    return usableGraphicsEntryIndexes.Contains(graphicsIndex - 1);
}

// Now we randomize
CreatureIndex getRandomCreature()
{
    var creature = CreatureIndex.NONE;
    while (creature == default || !isPokemonUsable(creature))
    {
        if (creature != CreatureIndex.NONE)
        {
            Console.WriteLine($"CreatureIndex.{creature:f} is unusable");
        }
        creature = (CreatureIndex)random.Next(1, (int)CreatureIndex.END);
    }
    return creature;    
}
WazaIndex getRandomMove()
{
    return (WazaIndex)random.Next(1, (int)WazaIndex.END);
}

var starters = Rom.GetStarters();
foreach (var starter in starters.Starters)
{
    starter.PokemonId = getRandomCreature();
    starter.Move1 = getRandomMove();
    starter.Move2 = getRandomMove();
    starter.Move3 = getRandomMove();
    starter.Move4 = getRandomMove();
}
