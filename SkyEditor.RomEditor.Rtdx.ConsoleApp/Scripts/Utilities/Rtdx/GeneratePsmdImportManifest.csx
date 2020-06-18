// This script aids in the import of models by generating a yaml file like this: https://github.com/tech-ticks/RTDXTools/blob/master/Data/psmd_import.manifest.yml

// To use this script, pipe the output of Queries/Psmd/SerializePokemonGraphicsDatabase.csx
// Sample commandline: `dotnet SkyEditor.RomEditor.ConsoleApp.dll myPsmdRom.3ds Scripts/Queries/Psmd/SerializePokemonGraphicsDatabase.csx | dotnet SkyEditor.RomEditor.ConsoleApp.dll myRtdxRom/ Scripts/Utilities/GeneratePsmdImportManifest.csx > psmd_import.manifest.yml`

/* Sample output:

psmdPkGraphicPath: D:\3ds\pk_graphic

models:
- targetName: fokko_00
  graphicsDatabaseSlot: 801
  psmdModel: fokko_10.bch
  rtdxAnimation: 4leg_fokko_00

- targetName: zoroa_00
  graphicsDatabaseSlot: 699
  psmdModel: zoroa_00.bch
  rtdxAnimation: 4leg_beast_00

*/

#load "../../../Stubs/RTDX.csx"

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

Console.WriteLine("psmdPkGraphicPath: $(psmdPkGraphicPath)");
Console.WriteLine();
Console.WriteLine("models:");

var json = new StringBuilder();
int c;
while ((c = Console.Read()) != -1) {
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

            Console.WriteLine($@"- targetName: {formData.BchName.Replace(".bch", "")}
  graphicsDatabaseSlot: {graphicsDatabaseSlot}
  psmdModel: {formData.BchName}
  rtdxAnimation: {animation}
");
        }        
    }
}