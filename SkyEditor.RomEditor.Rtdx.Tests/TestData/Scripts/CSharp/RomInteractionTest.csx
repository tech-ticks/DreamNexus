using System;

var starters = Rom.GetStarters().Starters;

var riolu = starters[0];
if (riolu.PokemonId != CreatureIndex.RIORU) throw new Exception("Failed to read first starter's PokemonId");
if (riolu.PokemonName != "Riolu") throw new Exception("Failed to read first starter's PokemonId");

var mew = starters[1];
if (mew.PokemonId != CreatureIndex.MYUU) throw new Exception("Failed to read second starter's PokemonId");
if (mew.PokemonName != "Mew") throw new Exception("Failed to read second starter's PokemonId");
