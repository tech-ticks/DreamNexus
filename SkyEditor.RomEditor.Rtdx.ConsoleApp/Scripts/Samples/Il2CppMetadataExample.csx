#load "../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

ulong offset = Rom.GetMainExecutable().GetIlConstructorOffset("PegasusActDatabase", new string[]{});
Console.WriteLine($"PegasusActDatabase.ctor() at offset {offset}");

offset = Rom.GetMainExecutable().GetIlMethodOffset("CharacterModel", "SetScarfVisible",
  new string[]{ typeof(bool).FullName });
Console.WriteLine($"CharacterModel.SetScarfVisible(System.Boolean) at offset {offset}");

offset = Rom.GetMainExecutable().GetIlMethodOffset("UIWorldMapSet", "OpenDungeonName",
  new string[]{ "Const.dungeon.Index" });
Console.WriteLine($"UIWorldMapSet.OpenDungeonName(Const.dungeon.index) at offset {offset}");
