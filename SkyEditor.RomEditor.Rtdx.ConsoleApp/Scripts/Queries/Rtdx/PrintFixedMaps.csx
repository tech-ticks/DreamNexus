#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

var maps = Rom.GetFixedMap().Entries;
var fixedPokemon = Rom.GetFixedPokemon().Entries;
var fixedItems = Rom.GetFixedItem().Entries;
var strings = Rom.GetCommonStrings();
for (int i = 0; i < maps.Count; i++)
{
    var map = maps[i];
    Console.WriteLine($"#{i}: {map.Width}x{map.Height}, contains {map.Creatures.Count} creatures and {map.Items.Count} items");

    for (int y = 0; y < map.Height; y++)
    {
        for (int x = 0; x < map.Width; x++)
        {
            var type = (byte)map.GetTile(x, y).Type;
            if (type > 9)
            {
                Console.Write("+");
            }
            else
            {
                Console.Write(type == 0 ? "█" : type.ToString());
            }
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
        Console.Write($"#{j,-2} @ {creature.XPos,2}x{creature.YPos,-2}: ({creature.Faction}, {creature.Direction}) ");
        if ((int)creature.Index < fixedPokemon.Count)
        {
            var pokemon = fixedPokemon[(int)creature.Index];
            if (strings.Pokemon.TryGetValue(pokemon.PokemonId, out string name))
            {
                Console.WriteLine($"{name}, Lv {pokemon.Level}");
            }
            else
            {
                switch (creature.Index)
                {
                    case FixedCreatureIndex.FIXEDMAP_HERO: Console.WriteLine("Player"); break;
                    case FixedCreatureIndex.FIXEDMAP_PARTNER: Console.WriteLine("Partner"); break;
                    case FixedCreatureIndex.FIXEDMAP_MEMBER1: Console.WriteLine("Teammate 1"); break;
                    case FixedCreatureIndex.FIXEDMAP_MEMBER2: Console.WriteLine("Teammate 2"); break;
                    case FixedCreatureIndex.FIXEDMAP_MEMBER3: Console.WriteLine("Teammate 3"); break;
                    case FixedCreatureIndex.FIXEDMAP_MEMBER4: Console.WriteLine("Teammate 4"); break;
                    case FixedCreatureIndex.FIXEDMAP_MEMBER5: Console.WriteLine("Teammate 5"); break;
                    case FixedCreatureIndex.FIXEDMAP_MEMBER6: Console.WriteLine("Teammate 6"); break;
                    case FixedCreatureIndex.FIXEDMAP_MEMBER7: Console.WriteLine("Teammate 7"); break;
                    case FixedCreatureIndex.FIXEDMAP_INVITATION1: Console.WriteLine("Recruit 1"); break;
                    case FixedCreatureIndex.FIXEDMAP_INVITATION2: Console.WriteLine("Recruit 2"); break;
                    case FixedCreatureIndex.FIXEDMAP_INVITATION3: Console.WriteLine("Recruit 3"); break;
                    default: Console.WriteLine($"{creature.Index}"); break;
                }

            }
        }
        else
        {
            Console.WriteLine($"{creature.Index} (out of range)");
        }
    }
    Console.WriteLine();

    Console.WriteLine("Items:");
    for (int j = 0; j < map.Items.Count; j++)
    {
        var item = map.Items[j];
        Console.Write($"#{j,-2} @ {item.XPos,2}x{item.YPos,-2}: ({item.Variation}) ");
        if (item.FixedItemIndex < fixedItems.Count)
        {
            var fixedItem = fixedItems[item.FixedItemIndex];
            var itemName = strings.Items[fixedItem.Index];
            if (fixedItem.Quantity > 0)
            {
                Console.Write($"{fixedItem.Quantity}x ");
            }
            Console.WriteLine($"{itemName}, {fixedItem.Short04}, {fixedItem.Byte06}");
        }
        else
        {
            Console.WriteLine($"{item.FixedItemIndex} (out of range)");
        }
    }

    Console.WriteLine();
    Console.WriteLine();
}
