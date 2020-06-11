#load "../../Stubs/RTDX.csx"

using System;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

if (Mod == null)
{
    throw new InvalidOperationException("Script must be run in the context of The Fennekin Mod");
}

// Recommended prerequisite mods:
// - ImportMissingPokemonStats
// - AddMissingPokemonModels

// Add the starter
var starterModel = Rom.GetStarters();
var psyduck = starterModel.GetStarterById(CreatureIndex.KODAKKU);
if (psyduck != null)
{
    psyduck.PokemonId = CreatureIndex.FOKKO; // Fennekin
    psyduck.Move1 = WazaIndex.HIKKAKU; // Scratch
    psyduck.Move2 = WazaIndex.HINOKO; // Ember
    psyduck.Move3 = WazaIndex.SHIPPOWOFURU; // Tail Whip
    psyduck.Move4 = WazaIndex.PSYCHEKOUSEN; // Psybeam
}
else
{
    throw new InvalidOperationException("Could not find starter Psyduck.");
}