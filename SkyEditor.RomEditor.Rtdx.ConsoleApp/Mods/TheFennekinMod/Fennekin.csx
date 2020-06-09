#load "../../Stubs/RTDX.csx"

using System;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

if (Mod == null)
{
    throw new InvalidOperationException("Script must be run in the context of The Fennekin Mod");
}

// Add the model
var fennekinModel = Mod.ReadResourceArray("Resources/fokko_00.ab");
Rom.WriteFile("romfs/Data/StreamingAssets/ab/fokko_00.ab", fennekinModel);

// Update pokemon_graphics_database to reference the model
var graphics = Rom.GetPokemonGraphicsDatabase();
var graphicsEntry = Rom.FindGraphicsDatabaseEntryByCreature(CreatureIndex.FOKKO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "fokko_00";
graphicsEntry.AnimationName = "4leg_fokko_00";

// Set the stats
var stats = Rom.GetPokemonDataInfo();
var fennekinStats = stats.GetByPokemonId(CreatureIndex.FOKKO);
// Importing stats from PSMD isn't implemented yet. Copy from Vulpix for now.
var vulpixStats = stats.GetByPokemonId(CreatureIndex.ROKON);
var charmanderStats = stats.GetByPokemonId(CreatureIndex.HITOKAGE); // I don't have English ability names handy, so I'm copying from Charmander.

fennekinStats.Ability1 = charmanderStats.Ability1;
fennekinStats.Ability2 = AbilityIndex.NONE;
fennekinStats.HiddenAbility = AbilityIndex.NONE;
fennekinStats.Type1 = PokemonType.FIRE;
fennekinStats.Type2 = PokemonType.NONE;

fennekinStats.BaseHitPoints = vulpixStats.BaseHitPoints;
fennekinStats.BaseAttack = vulpixStats.BaseAttack;
fennekinStats.BaseDefense = vulpixStats.BaseDefense;
fennekinStats.BaseSpecialAttack = vulpixStats.BaseSpecialAttack;
fennekinStats.BaseSpecialDefense = vulpixStats.BaseSpecialDefense;
fennekinStats.BaseSpeed = vulpixStats.BaseSpeed;
fennekinStats.ExperienceEntry = vulpixStats.ExperienceEntry;

fennekinStats.LevelupLearnset[0].Level = 1; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Scratch
fennekinStats.LevelupLearnset[1].Level = 1; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Tail Whip
fennekinStats.LevelupLearnset[2].Level = 5; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Ember
fennekinStats.LevelupLearnset[3].Level = 11; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Howl
fennekinStats.LevelupLearnset[4].Level = 14; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Flame Charge
fennekinStats.LevelupLearnset[5].Level = 17; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Psybeam
fennekinStats.LevelupLearnset[6].Level = 20; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Fire Spin
fennekinStats.LevelupLearnset[7].Level = 25; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Lucky Chant
fennekinStats.LevelupLearnset[8].Level = 27; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Light Screen
fennekinStats.LevelupLearnset[9].Level = 31; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Psyshock
fennekinStats.LevelupLearnset[10].Level = 35; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Flamethrower
fennekinStats.LevelupLearnset[11].Level = 38; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Will-O-Wisp
fennekinStats.LevelupLearnset[12].Level = 41; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Psychic
fennekinStats.LevelupLearnset[13].Level = 43; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Sunny Day
fennekinStats.LevelupLearnset[14].Level = 46; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Magic Room
fennekinStats.LevelupLearnset[15].Level = 48; fennekinStats.LevelupLearnset[0].Move = WazaIndex.HIKKAKU; // Fire Blast

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