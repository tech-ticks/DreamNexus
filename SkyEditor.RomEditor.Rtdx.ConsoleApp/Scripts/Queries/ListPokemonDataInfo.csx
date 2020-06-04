#load "../SkyEditor.csx"

using System;

var pokemonInfo = Rom.GetPokemonDataInfo();
var strings = Rom.GetCommonStrings();
foreach (var pokemon in pokemonInfo.Entries)
{
    if (pokemon.Id == default)
    {
        continue;
    }
    Console.WriteLine($"{pokemon.PokedexNumber} {strings.Pokemon[pokemon.Id]} ({pokemon.Id:d} {pokemon.Id:f})");
    Console.WriteLine(strings.GetPokemonTaxonomy(pokemon.Taxon));
    Console.WriteLine($"Types: {pokemon.Type1} {pokemon.Type2}");
    Console.WriteLine($"Abilities: {strings.GetAbilityName(pokemon.Ability1)} {strings.GetAbilityName(pokemon.Ability2)} (HA: {strings.GetAbilityName(pokemon.HiddenAbility)})");
    Console.WriteLine($"HP: {pokemon.BaseHitPoints}");
    Console.WriteLine($"ATK: {pokemon.BaseAttack}");
    Console.WriteLine($"DEF: {pokemon.BaseDefense}");
    Console.WriteLine($"SPA: {pokemon.BaseSpecialAttack}");
    Console.WriteLine($"SPD: {pokemon.BaseSpecialDefense}");
    Console.WriteLine($"SPE: {pokemon.BaseSpeed}");
    Console.WriteLine($"Experience Table ID: {pokemon.ExperienceEntry}");
    Console.WriteLine($"Recruitment Prereq: {pokemon.RecruitPrereq}");
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