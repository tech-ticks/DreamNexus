// This script aids in the creation of Mods/AddMissingPokemonModels/AddModels.csx
// This script is not a mod for the game
// Usage: `dotnet SkyEditor.RomEditor.ConsoleApp.dll myPsmdRom.3ds Scripts/Queries/Psmd/SerializePokemonGraphicsDatabase.csx | dotnet SkyEditor.RomEditor.ConsoleApp.dll myRtdxRom/ Mods/AddMissingPokemonModels/GenerateAddModelsScript.csx > AddModels.csx`

#load "../../Stubs/RTDX.csx"

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

Console.WriteLine(@"#load ""../../Stubs/RTDX.csx""

using System;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

var pokemon = Rom.GetPokemon();
var graphics = Rom.GetPokemonGraphics();

PokemonGraphicsModel GetGraphicsModelByCreatureAndForm(CreatureIndex creatureIndex, PokemonFormType formType)
{
    int graphicsDatabaseId = pokemon.GetPokemonById(creatureIndex).PokemonGraphicsDatabaseEntryIds[(int) formType];
    return graphics.GetEntryById(graphicsDatabaseId);
}

PokemonGraphicsModel graphicsEntry;
");

var json = new StringBuilder();
int c;
while ((c = Console.Read()) != -1)
{
    json.Append((char)c);
}

class PokemonGraphicsModel
{
    public string Id { get; set; }
    public string Form { get; set; }

    public string BgrsFilename { get; set; }
    public string SecondaryBgrsFilename { get; set; }
    public string PortraitName { get; set; }

    public string BchName { get; set; }
    public string PrimaryAnimationName { get; set; }
    public string FallbackAnimationName { get; set; }
}

// Set the stats
var psmdGraphics = Utilities.DeserializeJson<List<PokemonGraphicsModel>>(json.ToString());
var rtdxFormDb = Rom.GetPokemonFormDatabase();
var rtdxStats = Rom.GetPokemonDataInfo();
var assetBundles = Rom.ListAssetBundles();
foreach (var rtdxEntry in rtdxStats.Entries)
{
    if (rtdxEntry.LevelupLearnset.All(l => l.Level == default && l.Move == default))
    {
        var forms = psmdGraphics.Where(g => g.Id == rtdxEntry.Id.ToString("f"));
        foreach (var formData in forms)
        {
            if (formData.BchName == "dummypokemon_00.bch")
            {
                continue;
            }

            var pokemonId = (CreatureIndex)Enum.Parse(typeof(CreatureIndex), formData.Id);
            var formId = (PokemonFormType)Enum.Parse(typeof(PokemonFormType), formData.Form);
            var graphicsDatabaseSlot = rtdxFormDb.GetGraphicsDatabaseIndex(pokemonId, formId) - 1;
            if (graphicsDatabaseSlot < 0)
            {
                continue;
            }

            var primaryAnimationExists = assetBundles.Any(b => Path.GetFileNameWithoutExtension(b) == formData.PrimaryAnimationName);
            var fallbackAnimationExists = assetBundles.Any(b => Path.GetFileNameWithoutExtension(b) == formData.FallbackAnimationName);
            var animation = primaryAnimationExists
                ? formData.PrimaryAnimationName
                : fallbackAnimationExists
                    ? formData.FallbackAnimationName
                    : "dummypokemon_00";

            var modelName = formData.BchName.Replace(".bch", "");

            Console.WriteLine($@"graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.{pokemonId:f}, PokemonFormType.{formId:f});
graphicsEntry.ModelName = ""{modelName}"";
graphicsEntry.AnimationName = ""{animation}"";");
            Console.WriteLine($@"Rom.WriteFile(""romfs/Data/StreamingAssets/ab/{modelName}.ab"", Mod.ReadResourceArray(""Resources/{modelName}.ab""));");
            Console.WriteLine();
        }
    }
}
