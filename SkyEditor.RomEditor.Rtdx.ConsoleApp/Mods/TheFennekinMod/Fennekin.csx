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

// Add the model
// This should be extracted into another mod in the future once graphics can be bulk-imported
var fennekinModel = Mod.ReadResourceArray("Resources/fokko_00.ab");
Rom.WriteFile("romfs/Data/StreamingAssets/ab/fokko_00.ab", fennekinModel);
// - Update pokemon_graphics_database to reference the model
var graphics = Rom.GetPokemonGraphicsDatabase();
var graphicsEntry = Rom.FindGraphicsDatabaseEntryByCreature(CreatureIndex.FOKKO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "fokko_00";
graphicsEntry.AnimationName = "4leg_fokko_00";

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