#load "../../../Stubs/PSMD.csx"

using System;
using System.Collections.Generic;
using System.Linq;
using SkyEditor.RomEditor.Domain.Psmd.Structures;
using SkyEditor.RomEditor.Domain.Psmd.Constants;

// Json.Net converts the whole object regardless of what object we give it
// But we just want these properties and not all the unknowns not in the interface
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

var models = new List<PokemonGraphicsModel>();

var graphics = Rom.GetPokemonGraphics();
var dataInfo = Rom.GetPokemonDataInfo().Entries;
foreach (var pokemon in dataInfo)
{
    if (pokemon.Id == CreatureIndex.NONE) continue;

    for (var formType = PokemonFormType.NORMAL; formType < PokemonFormType.MAX; formType++)
    {
        var graphicsEntry = Rom.FindGraphicsDatabaseEntryByCreature(pokemon.Id, formType);
        if (graphicsEntry != null)
        {
            var bgrsData = graphics.GetFile(graphicsEntry.BgrsFilename);
            var altBgrsData = graphics.GetFile(graphicsEntry.SecondaryBgrsFilename);
            var bgrs = bgrsData != null ? new BGRS(bgrsData) : null;
            var altBgrs = altBgrsData != null ? new BGRS(altBgrsData) : null;
            models.Add(new PokemonGraphicsModel
            {
                Id = pokemon.Id.ToString("f"),
                Form = formType.ToString("f"),
                BgrsFilename = graphicsEntry.BgrsFilename,
                SecondaryBgrsFilename = graphicsEntry.SecondaryBgrsFilename,
                PortraitName = graphicsEntry.PortraitName,
                BchName = bgrs?.ReferencedBchFileName,
                PrimaryAnimationName = bgrs?.BgrsName,
                FallbackAnimationName = altBgrs?.BgrsName ?? bgrs?.GetFallbackAnimationSetName()
            });
        }
    }
}

Console.Write(Utilities.JsonSerialize(models));