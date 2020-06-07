#load "../../Stubs/SkyEditor.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

var starterModel = Rom.GetStarters();

var machop = starterModel.GetStarterById(CreatureIndex.WANRIKII);
if (machop != null)
{
    machop.PokemonId = CreatureIndex.RIORU; // Riolu
    machop.Move1 = WazaIndex.DENKOUSEKKA; // Quick Attack
    machop.Move2 = WazaIndex.SHINKUUHA; // Vacuum Wave
    machop.Move3 = WazaIndex.KAMITSUKU; // Bite
    machop.Move4 = WazaIndex.KORAERU; // Endure
}    
else
{
    throw new InvalidOperationException("Could not find starter Machop.");
}