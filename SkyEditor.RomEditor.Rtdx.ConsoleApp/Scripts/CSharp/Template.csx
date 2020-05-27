// Load SkyEditor.csx to give Visual Studio hints about what globals and imports will be available
// Note that preprocessor directives will not be run by Sky Editor
#load "SkyEditor.csx"

using System;
using SkyEditor.RomEditor.Rtdx.Reverse.Const;

// These imports are automatically applied by Sky Editor, 
// but including them in SkyEditor.csx doesn't affect this file
// Including them in this file will break things when running inside Sky Editor
// We need a better solution

//using CreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;
//using FixedCreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.fixed_creature.Index;
//using ItemIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.item.Index;
//using ItemKind = SkyEditor.RomEditor.Rtdx.Reverse.Const.item.Kind;
//using ItemPriceType = SkyEditor.RomEditor.Rtdx.Reverse.Const.item.PriceType;
//using OrderIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.order.Index;
//using PokemonFixedWarehouseId = SkyEditor.RomEditor.Rtdx.Reverse.Const.pokemon.FixedWarehouseId;
//using PokemonFormType = SkyEditor.RomEditor.Rtdx.Reverse.Const.pokemon.FormType;
//using PokemonGenderType = SkyEditor.RomEditor.Rtdx.Reverse.Const.pokemon.GenderType;
//using PokemonSallyType = SkyEditor.RomEditor.Rtdx.Reverse.Const.pokemon.SallyType;
//using PokemonType = SkyEditor.RomEditor.Rtdx.Reverse.Const.pokemon.Type;
//using SugowazaIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.sugowaza.Index;
//using WazaIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.waza.Index;
//using EvolutionCameraType = SkyEditor.RomEditor.Rtdx.Reverse.Const.EvolutionCameraType;
//using GraphicsBodySizeType = SkyEditor.RomEditor.Rtdx.Reverse.Const.GraphicsBodySizeType;
//using TextIDHash = SkyEditor.RomEditor.Rtdx.Reverse.Const.TextIDHash;

if (Rom == null) 
{
    throw new Exception("Script must be run in the context of Sky Editor");
}

var starterCollection = Rom.GetStarters();
var eevee = starterCollection.GetStarterById(CreatureIndex.IIBUI);
if (eevee != null)
{
    Console.WriteLine("Eevee is a starter");
}
else
{
    Console.WriteLine("Eevee is not a starter");
}