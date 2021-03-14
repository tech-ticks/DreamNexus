// This script dumps all dungeon items into individual CSV files per dungeon in the current working directory.
#load "../../../Stubs/RTDX.csx"

using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Structures.Executable;

Rom.GetMainExecutable().ActorDatabase.WriteToBinaryFile("actor_database.bin");
Rom.GetMainExecutable().WriteStartersToBinaryFile("starters.bin");
