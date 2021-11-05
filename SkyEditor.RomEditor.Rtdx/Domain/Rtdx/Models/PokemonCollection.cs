using System;
using System.Collections.Generic;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IPokemonCollection
    {
        IDictionary<CreatureIndex, PokemonModel> LoadedPokemon { get; }
        void SetPokemon(CreatureIndex id, PokemonModel model);
        bool IsPokemonDirty(CreatureIndex id);
        PokemonModel? GetPokemonById(CreatureIndex id);
        void Flush(IRtdxRom rom);
    }

    public class PokemonCollection : IPokemonCollection
    {
        public IDictionary<CreatureIndex, PokemonModel> LoadedPokemon { get; } = new Dictionary<CreatureIndex, PokemonModel>();
        public HashSet<CreatureIndex> DirtyPokemon { get; } = new HashSet<CreatureIndex>();

        private IRtdxRom rom;
        private Dictionary<CreatureIndex, PokemonDataInfo.PokemonDataInfoEntry> dataInfoEntries;

        public PokemonCollection(IRtdxRom rom)
        {
            this.rom = rom;
            this.dataInfoEntries = rom.GetPokemonDataInfo().Entries.ToDictionary(entry => entry.Id);
        }

        public PokemonModel LoadPokemon(CreatureIndex index)
        {
            var data = dataInfoEntries[index];
            var evolution = rom.GetPokemonEvolution().Entries[index];

            // Convert learnable TMs bitfield to list
            var learnableTMs = new List<ItemIndex>();
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var tmIndex = j + i * 8;
                    if ((data.LearnableTMs[i] & (1 << j)) != 0)
                    {
                       learnableTMs.Add(ItemIndexConstants.BROKENMACHINE_MIN + tmIndex);
                    }
                }
            }
                
            return new PokemonModel
            {
                Id = data.Id,
                LearnableTMs = learnableTMs,
                LevelupLearnset = data.LevelupLearnset.Select(learnset =>
                    new PokemonDataInfo.LevelUpMove(learnset.Level, learnset.Move)).ToList(),
                Features = data.Features,
                Unknown62 = data.Unknown62,
                PokedexNumber = data.PokedexNumber,
                Unknown66 = data.Unknown66,
                Taxon = data.Taxon,
                BoostedRecruitRate = data.BoostedRecruitRate,
                BaseRecruitRate = data.BaseRecruitRate,
                Unknown6E = data.Unknown6E,
                Unknown70 = data.Unknown70,
                Unknown72 = data.Unknown72,

                BaseHitPoints = data.BaseHitPoints,
                BaseAttack = data.BaseAttack,
                BaseSpecialAttack = data.BaseSpecialAttack,
                BaseDefense = data.BaseDefense,
                BaseSpecialDefense = data.BaseSpecialDefense,
                BaseSpeed = data.BaseSpeed,

                Unknown80 = data.Unknown80,
                EvolvesFrom = (CreatureIndex) data.EvolvesFrom,
                ExperienceEntry = data.ExperienceEntry,

                Unknown8B = data.Unknown8B,
                Unknown8C = data.Unknown8C,
                Unknown8D = data.Unknown8D,
                Unknown8E = data.Unknown8E,

                Ability1 = data.Ability1,
                Ability2 = data.Ability2,
                HiddenAbility = data.HiddenAbility,
                Type1 = data.Type1,
                Type2 = data.Type2,

                Unknown95 = data.Unknown95,
                Unknown96 = data.Unknown96,
                Size = data.Size,
                MegaRelatedProperty = data.MegaRelatedProperty,
                SomeMinimumLevel = data.SomeMinimumLevel,
                SomeMaximumLevel = data.SomeMaximumLevel,
                RecruitPrereq = data.RecruitPrereq,

                MegaEvolutions = evolution.MegaEvos,
                EvolutionBranches = evolution.Branches.Select(branch => branch.Clone()).ToList(),
            };
        }

        public PokemonModel GetPokemonById(CreatureIndex id)
        {
            DirtyPokemon.Add(id);
            if (!LoadedPokemon.ContainsKey(id))
            {
                LoadedPokemon.Add(id, LoadPokemon(id));
            }
            return LoadedPokemon[id];
        }

        public void SetPokemon(CreatureIndex id, PokemonModel model)
        {
            LoadedPokemon[id] = model;
        }

        public bool IsPokemonDirty(CreatureIndex id)
        {
            return DirtyPokemon.Contains(id);
        }

        public void Flush(IRtdxRom rom)
        {
            var pokemonIndexLookup = rom.GetPokemonDataInfo().Entries.ToDictionary(entry => entry.Id);
            foreach (var pokemon in LoadedPokemon.Values)
            {
                var data = pokemonIndexLookup[pokemon.Id];
                var evolution = rom.GetPokemonEvolution().Entries[pokemon.Id];

                // Convert list of TM indices to bitfield
                var tmsSet = new HashSet<ItemIndex>(pokemon.LearnableTMs);
                for (var i = 0; i < 16; i++)
                {
                    byte tmBits = 0;
                    for (var j = 0; j < 8; j++)
                    {
                        var tmIndex = j + i * 8;
                        var itemIndex = ItemIndexConstants.BROKENMACHINE_MIN + tmIndex;
                        if (tmsSet.Contains(itemIndex))
                        {
                            tmBits |= (byte) (1 << j);
                        }
                    }
                    data.LearnableTMs[i] = tmBits;
                }

                data.LevelupLearnset = pokemon.LevelupLearnset.Select(learnset =>
                    new PokemonDataInfo.LevelUpMove(learnset.Level, learnset.Move)).ToList();
                data.Features = pokemon.Features;
                data.Unknown62 = pokemon.Unknown62;
                data.PokedexNumber = pokemon.PokedexNumber;
                data.Unknown66 = pokemon.Unknown66;
                data.Taxon = pokemon.Taxon;
                data.BoostedRecruitRate = pokemon.BoostedRecruitRate;
                data.BaseRecruitRate = pokemon.BaseRecruitRate;
                data.Unknown6E = pokemon.Unknown6E;
                data.Unknown70 = pokemon.Unknown70;
                data.Unknown72 = pokemon.Unknown72;
                data.BaseHitPoints = pokemon.BaseHitPoints;
                data.BaseAttack = pokemon.BaseAttack;
                data.BaseSpecialAttack = pokemon.BaseSpecialAttack;
                data.BaseDefense = pokemon.BaseDefense;
                data.BaseSpecialDefense = pokemon.BaseSpecialDefense;
                data.BaseSpeed = pokemon.BaseSpeed;
                data.Unknown80 = pokemon.Unknown80;
                data.EvolvesFrom = (short) pokemon.EvolvesFrom;
                data.ExperienceEntry = pokemon.ExperienceEntry;
                data.Unknown8B = pokemon.Unknown8B;
                data.Unknown8C = pokemon.Unknown8C;
                data.Unknown8D = pokemon.Unknown8D;
                data.Unknown8E = pokemon.Unknown8E;
                data.Ability1 = pokemon.Ability1;
                data.Ability2 = pokemon.Ability2;
                data.HiddenAbility = pokemon.HiddenAbility;
                data.Type1 = pokemon.Type1;
                data.Type2 = pokemon.Type2;
                data.Unknown95 = pokemon.Unknown95;
                data.Unknown96 = pokemon.Unknown96;
                data.Size = pokemon.Size;
                data.MegaRelatedProperty = pokemon.MegaRelatedProperty;
                data.SomeMinimumLevel = pokemon.SomeMinimumLevel;
                data.SomeMaximumLevel = pokemon.SomeMaximumLevel;
                data.RecruitPrereq = pokemon.RecruitPrereq;

                evolution.MegaEvos = pokemon.MegaEvolutions;
                evolution.Branches = pokemon.EvolutionBranches.Select(branch => branch.Clone()).ToList();
            }
        }
    }    
}
