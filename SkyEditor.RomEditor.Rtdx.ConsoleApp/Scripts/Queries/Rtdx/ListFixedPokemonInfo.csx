#load "../../../Stubs/RTDX.csx"

using System;

var fixedPokemon = Rom.GetFixedPokemon();
var strings = Rom.GetCommonStrings();
Console.WriteLine("                                                                 --- Stat boosts ---");
Console.WriteLine("Fixed Creature ID                    #  Pokemon       Lvl    HP  Atk Def SpA SpD Spd");
foreach (var pokemon in fixedPokemon.Entries)
{
    if (pokemon.FixedCreatureId == default) continue;
    if ((int)pokemon.PokemonId < 0) continue;

    Console.WriteLine($"{pokemon.FixedCreatureId,-32}  {pokemon.PokemonId,4:d}  {strings.Pokemon[pokemon.PokemonId],-12}  {pokemon.Level,3}  {pokemon.HitPoints,4}  "
        + $"{pokemon.AttackBoost,3} {pokemon.DefenseBoost,3} {pokemon.SpAttackBoost,3} {pokemon.SpDefenseBoost,3} {pokemon.SpeedBoost,3}");
}