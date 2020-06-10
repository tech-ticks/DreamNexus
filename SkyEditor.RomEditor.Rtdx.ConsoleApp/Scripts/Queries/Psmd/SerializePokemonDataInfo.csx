#load "../../../Stubs/PSMD.csx"

using System;
using System.Collections.Generic;
using System.Linq;
using SkyEditor.RomEditor.Domain.Psmd.Structures;
using SkyEditor.RomEditor.Domain.Psmd.Constants;

// Json.Net converts the whole object regardless of what object we give it
// But we just want these properties and not all the unknowns not in the interface
class CommonPokemonDataInfoEntry
{
    public CommonPokemonDataInfoEntry(PokemonDataInfo.PokemonDataInfoEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentNullException(nameof(entry));
        }

        this.Id = entry.Id.ToString("F");
        this.LevelupLearnset = entry.LevelupLearnset;
        this.PokedexNumber = entry.PokedexNumber;
        this.Taxon = entry.Taxon;
        this.BaseHitPoints = entry.BaseHitPoints;
        this.BaseAttack = entry.BaseAttack;
        this.BaseDefense = entry.BaseDefense;
        this.BaseSpecialAttack = entry.BaseSpecialAttack;
        this.BaseSpecialDefense = entry.BaseSpecialDefense;
        this.BaseSpeed = entry.BaseSpeed;
        this.Ability1 = entry.Ability1;
        this.Ability2 = entry.Ability2;
        this.HiddenAbility = entry.HiddenAbility;
        this.Type1 = entry.Type1;
        this.Type2 = entry.Type2;
    }

    public string Id { get; }

    public IReadOnlyList<PokemonDataInfo.LevelUpMove> LevelupLearnset { get; }
    public short PokedexNumber { get; set; }

    public short Taxon { get; set; }
    public short BaseHitPoints { get; set; }
    public short BaseAttack { get; set; }
    public short BaseDefense { get; set; }
    public short BaseSpecialAttack { get; set; }
    public short BaseSpecialDefense { get; set; }
    public short BaseSpeed { get; set; }

    public AbilityIndex Ability1 { get; set; }
    public AbilityIndex Ability2 { get; set; }
    public AbilityIndex HiddenAbility { get; set; }
    public PokemonType Type1 { get; set; }
    public PokemonType Type2 { get; set; }
}

Console.Write(Utilities.JsonSerialize(Rom.GetPokemonDataInfo().Entries.Select(e => new CommonPokemonDataInfoEntry(e))));
