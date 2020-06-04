// Load SkyEditor.csx to give Visual Studio hints about what globals and imports will be available
// Note that preprocessor directives will not be run by Sky Editor
#load "../SkyEditor.csx"

using System;
using SkyEditor.RomEditor.Rtdx.Constants;

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