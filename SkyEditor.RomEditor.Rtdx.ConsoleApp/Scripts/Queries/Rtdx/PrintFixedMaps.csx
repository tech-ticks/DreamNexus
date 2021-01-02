#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

var maps = Rom.GetFixedMap().Entries;
for (int i = 0; i < maps.Count; i++)
{
    var map = maps[i];
    // TODO: change capacity to count when reading is implemented
    Console.WriteLine($"#{i}: {map.Width}x{map.Height}, contains {map.Creatures.Count} creatures and {map.Items.Count} items");

    for (int y = 0; y < map.Height; y++)
    {
        for (int x = 0; x < map.Width; x++)
        {
            var type = map.GetTile(x, y).Type;
            Console.Write(type == 0 ? "█" : type.ToString());
        }
        Console.Write("  ");
        for (int x = 0; x < map.Width; x++)
        {
            var roomId = map.GetTile(x, y).RoomId;
            Console.Write(roomId == 0 ? "█" : roomId.ToString());
        }
        Console.Write("  ");
        for (int x = 0; x < map.Width; x++)
        {
            var byte01 = map.GetTile(x, y).Byte01;
            if (byte01 > 9)
            {
                Console.Write("+");
            }
            else
            {
                Console.Write(byte01 == 0 ? "█" : byte01.ToString());
            }
        }
        Console.WriteLine();
    }

    Console.WriteLine("Creatures:");
    for (int j = 0; j < map.Creatures.Count; j++)
    {
        var creature = map.Creatures[j];
        Console.WriteLine($"#{j} @ {creature.XPos},{creature.YPos}: {creature.Index}, {creature.Faction}, {Convert.ToString(creature.UnknownFlags, 2)}");
    }
    Console.WriteLine();

    Console.WriteLine("Items:");
    for (int j = 0; j < map.Items.Count; j++)
    {
        var item = map.Items[j];
        Console.WriteLine($"#{j} @ {item.XPos},{item.YPos}: {item.UnknownItemIndex}, {item.Direction}, {Convert.ToString(item.UnknownItemType, 2)}");
    }

    Console.WriteLine();
    Console.WriteLine();
}
