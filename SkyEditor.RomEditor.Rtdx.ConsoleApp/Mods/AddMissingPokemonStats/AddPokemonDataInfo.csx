#load "../../Stubs/RTDX.csx"

using System;
using System.Collections.Generic;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;

class CommonPokemonDataInfoEntry
{
    public CreatureIndex Id { get; set; }
    public IReadOnlyList<PokemonDataInfo.LevelUpMove> LevelupLearnset { get; set; }
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

// Set the stats
var psmdStats = Utilities.DeserializeJson<List<CommonPokemonDataInfoEntry>>(Mod.ReadResourceText("Resources/PsmdPokemonDataInfo.json")).ToDictionary(e => (int)e.Id, e => e);
var rtdxStats = Rom.GetPokemon();
for (CreatureIndex i = CreatureIndex.NONE; i < CreatureIndex.END; i++)
{
    var rtdxEntry = rtdxStats.GetPokemonById(i);
    if (rtdxEntry.LevelupLearnset.All(l => l.Level == default && l.Move == default))
    {
        if (!psmdStats.TryGetValue((int)rtdxEntry.Id, out var psmdEntry))
        {
            continue;
        }
        rtdxEntry.LevelupLearnset = psmdEntry.LevelupLearnset.ToList();
        rtdxEntry.PokedexNumber = psmdEntry.PokedexNumber;
        rtdxEntry.Taxon = psmdEntry.Taxon;
        rtdxEntry.BaseHitPoints = psmdEntry.BaseHitPoints;
        rtdxEntry.BaseAttack = psmdEntry.BaseAttack;
        rtdxEntry.BaseDefense = psmdEntry.BaseDefense;
        rtdxEntry.BaseSpecialAttack = psmdEntry.BaseSpecialAttack;
        rtdxEntry.BaseSpecialDefense = psmdEntry.BaseSpecialDefense;
        rtdxEntry.BaseSpeed = psmdEntry.BaseSpeed;
        rtdxEntry.Ability1 = psmdEntry.Ability1;
        rtdxEntry.Ability2 = psmdEntry.Ability2;
        rtdxEntry.HiddenAbility = psmdEntry.HiddenAbility;
        rtdxEntry.Type1 = psmdEntry.Type1;
        rtdxEntry.Type2 = psmdEntry.Type2;
    }
}
