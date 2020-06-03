// Load SkyEditor.csx to give Visual Studio hints about what globals and imports will be available
// Note that preprocessor directives will not be run by Sky Editor
#load "SkyEditor.csx"

if (Rom == null) 
{
    throw new Exception("Script must be run in the context of Sky Editor");
}

var actDatabase = Rom.GetMainExecutable().ActorDatabase;

// Personality quiz model actors start with "SEIKAKU_". This one is Bulbasaur.
var seikakuFushigidane = actDatabase.ActorDataList.First(actor => actor.SymbolName == "SEIKAKU_FUSHIGIDANE");
Console.WriteLine("Old Pokémon index: " + seikakuFushigidane.PokemonIndex);

// Change Bulbasaur's model to Pikachu's. Only the PokemonIndex can be edited, changing any other fields has no effect.
seikakuFushigidane.PokemonIndex = CreatureIndex.PIKACHUU;
Console.WriteLine("Index changed.");
