#load "../../../Stubs/RTDX.csx"

using System;

var pokemonInfo = Rom.GetPokemonDataInfo();
var experience = Rom.GetExperience();
var strings = Rom.GetCommonStrings();

// CSV header for base Pokémon data
//Console.WriteLine("Pokedex Num,Pokemon Name,Pokemon Index,Type 1,Type 2,Ability 1,Ability 2,Hidden Ability,Base HP,Base Atk,Base Def,Base SpA,Base SpD,Base Spe");

// CSV header for Pokémon data per level
//Console.WriteLine("Pokedex Num,Pokemon Name,Pokemon Index,Level,Exp. Required,HP Gain,Atk Gain,Def Gain,SpA Gain,SpD Gain,Spe Gain,Total HP,Total Atk,Total Def,Total SpA,Total SpD,Total Spe");

foreach (var pokemon in pokemonInfo.Entries)
{
    if (pokemon.Id == default)
    {
        continue;
    }
    // CSV line for base Pokémon data
    //Console.WriteLine($"{pokemon.PokedexNumber},\"{strings.Pokemon[pokemon.Id]}\",{pokemon.Id:d},{pokemon.Type1},{pokemon.Type2},{strings.GetAbilityName(pokemon.Ability1)},{strings.GetAbilityName(pokemon.Ability2)},{strings.GetAbilityName(pokemon.HiddenAbility)},{pokemon.BaseHitPoints},{pokemon.BaseAttack},{pokemon.BaseDefense},{pokemon.BaseSpecialAttack},{pokemon.BaseSpecialDefense},{pokemon.BaseSpeed}");

    // CSV lines for Pokémon data per level
    /*var expTable = experience.Entries[pokemon.ExperienceEntry];
    var currHP = pokemon.BaseHitPoints;
    var currAtk = pokemon.BaseAttack;
    var currDef = pokemon.BaseDefense;
    var currSpA = pokemon.BaseSpecialAttack;
    var currSpD = pokemon.BaseSpecialDefense;
    var currSpe = pokemon.BaseSpeed;
    Console.WriteLine($"{pokemon.PokedexNumber},\"{strings.Pokemon[pokemon.Id]}\",{pokemon.Id:d},1,0,0,0,0,0,0,0,{pokemon.BaseHitPoints},{pokemon.BaseAttack},{pokemon.BaseDefense},{pokemon.BaseSpecialAttack},{pokemon.BaseSpecialDefense},{pokemon.BaseSpeed}");
    for (int i = 1; i < 100; i++)
    {
        var level = expTable.Levels[i];
        currHP += level.HitPointsGained;
        currAtk += level.AttackGained;
        currDef += level.DefenseGained;
        currSpA += level.SpecialAttackGained;
        currSpD += level.SpecialDefenseGained;
        currSpe += level.SpeedGained;
        Console.WriteLine($"{pokemon.PokedexNumber},\"{strings.Pokemon[pokemon.Id]}\",{pokemon.Id:d},{i+1},{level.MinimumExperience},{level.HitPointsGained},{level.AttackGained},{level.DefenseGained},{level.SpecialAttackGained},{level.SpecialDefenseGained},{level.SpeedGained},{currHP},{currAtk},{currDef},{currSpA},{currSpD},{currSpe}");
    }*/

    // Human-readable format
    Console.WriteLine($"{pokemon.PokedexNumber} {strings.Pokemon[pokemon.Id]} ({pokemon.Id:d} {pokemon.Id:f})");
    Console.WriteLine(strings.GetPokemonTaxonomy(pokemon.Taxon));
    Console.WriteLine($"Types: {pokemon.Type1} {pokemon.Type2}");
    Console.WriteLine($"Abilities: {strings.GetAbilityName(pokemon.Ability1)} ({pokemon.Ability1:d} {pokemon.Ability1:f}) {strings.GetAbilityName(pokemon.Ability2)} ({pokemon.Ability2:d} {pokemon.Ability2:f}) (HA: {strings.GetAbilityName(pokemon.HiddenAbility)} ({pokemon.HiddenAbility:d} {pokemon.HiddenAbility:f}))");
    Console.WriteLine($"HP: {pokemon.BaseHitPoints}");
    Console.WriteLine($"ATK: {pokemon.BaseAttack}");
    Console.WriteLine($"DEF: {pokemon.BaseDefense}");
    Console.WriteLine($"SPA: {pokemon.BaseSpecialAttack}");
    Console.WriteLine($"SPD: {pokemon.BaseSpecialDefense}");
    Console.WriteLine($"SPE: {pokemon.BaseSpeed}");
    Console.WriteLine($"Experience Table ID: {pokemon.ExperienceEntry}");
    var expTable = experience.Entries[pokemon.ExperienceEntry];
    var currHP = pokemon.BaseHitPoints;
    var currAtk = pokemon.BaseAttack;
    var currDef = pokemon.BaseDefense;
    var currSpA = pokemon.BaseSpecialAttack;
    var currSpD = pokemon.BaseSpecialDefense;
    var currSpe = pokemon.BaseSpeed;
    for (int i = 1; i < 100; i++)
    {
        var level = expTable.Levels[i];
        currHP += level.HitPointsGained;
        currAtk += level.AttackGained;
        currDef += level.DefenseGained;
        currSpA += level.SpecialAttackGained;
        currSpD += level.SpecialDefenseGained;
        currSpe += level.SpeedGained;
        Console.WriteLine($" - to {i + 1,3}:  XP: {level.MinimumExperience,7}   +{level.HitPointsGained} HP  +{level.AttackGained} Atk  +{level.DefenseGained} Def  +{level.SpecialAttackGained} SpA  +{level.SpecialDefenseGained} SpD  +{level.SpeedGained} Spe  =>  {currHP,3} HP  {currAtk,3} Atk  {currDef,3} Def  {currSpA,3} SpA  {currSpD,3} SpD  {currSpe,3} Spe");
    }
    if (!string.IsNullOrWhiteSpace(pokemon.RecruitPrereq))
    {
        Console.WriteLine($"Recruitment Prereq: {pokemon.RecruitPrereq}");
    }
    Console.WriteLine($"Moves:");
    foreach (var move in pokemon.LevelupLearnset)
    {
        if (move.Level == default && move.Move == default)
        {
            continue;
        }

        Console.WriteLine($" - {move.Level}: {strings.GetMoveName(move.Move)}");
    }
    Console.WriteLine();
}