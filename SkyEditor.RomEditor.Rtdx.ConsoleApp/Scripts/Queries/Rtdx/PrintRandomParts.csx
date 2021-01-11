#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

var randomParts = Rom.GetRandomParts();
foreach (var entry in randomParts.Entries)
{
    Console.WriteLine($"Entry {entry.Index}: {entry.Width} x {entry.Height}");
    foreach (var subentry in entry.Entries)
    {
        Console.WriteLine($"  Sub-entry {subentry.Index} - {subentry.Connections}");
        for (var y = 0; y < entry.Height; y++)
        {
            for (var x = 0; x < entry.Width; x++)
            {
                var nibble = subentry.Tiles[x, y].Byte0 & 0xF;
                if (nibble == 0)
                {
                    Console.Write("█");
                }
                else
                {
                    Console.Write($"{nibble:x}");
                }
            }
            Console.Write("  ");
            for (var x = 0; x < entry.Width; x++)
            {
                var nibble = (subentry.Tiles[x, y].Byte0 >> 4) & 0xF;
                if (nibble == 0)
                {
                    Console.Write("█");
                }
                else
                {
                    Console.Write($"{nibble:x}");
                }
            }
            Console.Write("  ");

            for (var x = 0; x < entry.Width; x++)
            {
                var nibble = subentry.Tiles[x, y].Byte1 & 0xF;
                if (nibble == 0)
                {
                    Console.Write("█");
                }
                else
                {
                    Console.Write($"{nibble:x}");
                }
            }
            Console.Write("  ");
            for (var x = 0; x < entry.Width; x++)
            {
                var nibble = (subentry.Tiles[x, y].Byte1 >> 4) & 0xF;
                if (nibble == 0)
                {
                    Console.Write("█");
                }
                else
                {
                    Console.Write($"{nibble:x}");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}
