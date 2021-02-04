#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

var camps = Rom.GetCamps().Entries;
var campHabitat = Rom.GetCampHabitat().Entries.ToList();
var commonStrings = Rom.GetCommonStrings();

Console.WriteLine($"{"Internal Name",-30}  {"Name",-25}  B01   S82   {"UInt88",8}   {"SortKey",7}   {"Int90",5}   {"Lineup",-25}  {"Price",9}  {"BG texture",-10}  {"BG music",-20}  {"UnlockCondition",-30}  ");
for (int i = 0; i < camps.Count; i++)
{
    var campIndex = (CampIndex) i;
    var camp = camps[campIndex];
    Console.WriteLine($"{campIndex,-30}  "
        + $"{commonStrings.Camps[campIndex],-25}  "
        + $"{camp.Byte01,3}   "
        + $"{camp.Short82,3}   "
        + $"{camp.UInt88.ToString("X"),8}   "
        + $"{camp.SortKey,7}   "
        + $"{camp.Int90,5}   "
        + $"{(string.IsNullOrWhiteSpace(camp.Lineup) ? "-" : camp.Lineup),-25}  "
        + $"{camp.Price,9}  "
        + $"{camp.BackgroundTexture,-10}  "
        + $"{camp.BackgroundMusic,-20}  "
        + $"{camp.UnlockCondition,-30}  "
    );

    var campPokemon = campHabitat
        .Where(habitat => habitat.Key != CreatureIndex.NONE)
        .Where(habitat => habitat.Value == campIndex)
        .Select(habitat => commonStrings.Pokemon[habitat.Key]);
    Console.Write("  Pokémon: ");
    Console.WriteLine(string.Join(", ", campPokemon));
}
