#load "../../Stubs/RTDX.csx"

using System;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

if (Mod == null)
{
    throw new InvalidOperationException("Script must be run in the context of The Zorua Mod");
}

// Recommended prerequisite mods:
// - ImportMissingPokemonStats
// - AddMissingPokemonModels

// Add the starter
var starterModel = Rom.GetStarters();
var skitty = starterModel.GetStarterById(CreatureIndex.ENEKO);
if (skitty != null)
{
    skitty.PokemonId = CreatureIndex.ZOROA; // Zorua
    skitty.Move1 = WazaIndex.HIKKAKU; // Scratch
    skitty.Move2 = WazaIndex.NIRAMITSUKERU; // Leer
    skitty.Move3 = WazaIndex.OIUCHI; // Pursuit
    skitty.Move4 = WazaIndex.DOROBOU; // Thief
}
else
{
    throw new InvalidOperationException("Could not find starter Skitty.");
}