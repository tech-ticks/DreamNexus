#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

var camps = Rom.GetCamps().Entries;
var campHabitat = Rom.GetCampHabitat().Entries.ToList();
var commonStrings = Rom.GetCommonStrings();

Console.WriteLine($"{"Internal Name",-30}  {"Name",-30}  {"Lineup",-30}  {"Price",9}  {"BG texture",-20}  {"BG music",-20}  {"UnlockCondition",-30}  ");
for (int i = 0; i < camps.Count; i++)
{
    var campIndex = (CampIndex) i;
    var camp = camps[campIndex];
    Console.WriteLine($"{campIndex,-30}  "
        + $"{commonStrings.Camps[campIndex],-30}  "
        + $"{(string.IsNullOrWhiteSpace(camp.Lineup) ? "-" : camp.Lineup),-30}  "
        + $"{camp.Price,9}  "
        + $"{camp.BackgroundTexture,-20}  "
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
