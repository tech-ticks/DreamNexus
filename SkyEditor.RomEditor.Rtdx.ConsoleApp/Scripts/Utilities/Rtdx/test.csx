// This script dumps all dungeon items into individual CSV files per dungeon in the current working directory.
#load "../../../Stubs/RTDX.csx"

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

// var dungeons = Rom.GetDungeons();
// // Rom.GetDungeonMapDataInfo().Entries[130].FixedMapIndex = 2;
// var fixedCreature = Rom.GetFixedMap().Entries[2].Creatures[0].Index;
// Rom.GetFixedPokemon().Entries[(int) fixedCreature].Level = 1;
// Rom.GetFixedPokemon().Entries[(int) fixedCreature].AttackBoost = 0;
// Rom.GetFixedPokemon().Entries[(int) fixedCreature].HitPoints = 10;
// Rom.GetFixedPokemon().Entries[(int) fixedCreature].DefenseBoost = 0;
// Rom.GetFixedPokemon().Entries[(int) fixedCreature].SpDefenseBoost = 0;
// Rom.GetFixedPokemon().Entries[(int) fixedCreature].SpeedBoost = 0;

// dungeons.GetDungeonById((DungeonIndex) 49).RequestLevel.MainEntry.FloorData[2].IsBossFloor = 1;
// for (int i = 1; i < 105; i++) {
//   System.Console.WriteLine();
//   System.Console.WriteLine(i + ": ");

//   foreach (var evt in dungeons.GetDungeonById((DungeonIndex) i).Extra.DungeonEvents)
//   {
//     System.Console.WriteLine(evt.Floor + ": " + evt.Name);
//   }

// }
// // dungeons.GetDungeonById((DungeonIndex) 49).Extra.DungeonEvents = new DungeonExtra.Entry.DungeonEvent[2];
// // dungeons.GetDungeonById((DungeonIndex) 49).Extra.DungeonEvents[0] = new DungeonExtra.Entry.DungeonEvent {
// //   Name = "@BOSS#0",
// //   Floor = 2,
// // };
// // dungeons.GetDungeonById((DungeonIndex) 49).Extra.DungeonEvents[1] = new DungeonExtra.Entry.DungeonEvent {
// //   Name = "@END",
// //   Floor = 4,
// // };
// dungeons.GetDungeonById((DungeonIndex) 49).Balance.FloorInfos[2].Event = "@BOSS#0";
// dungeons.GetDungeonById((DungeonIndex) 49).Balance.FloorInfos[2].DungeonMapDataInfoIndex = 5;
