#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

bool detailed = false;

var hitCounts = Rom.GetActHitCountTableDataInfo();
foreach (var entry in hitCounts.Entries)
{
    if (entry.MinHits == entry.MaxHits)
    {
        Console.WriteLine($"Entry {entry.Index}: {entry.MinHits} hits");
    }
    else
    {
        double weightSum = 0;
        for (var i = entry.MinHits; i <= entry.MaxHits; i++)
        {
            weightSum += entry.Weights[i - entry.MinHits];
        }

        Console.Write($"Entry {entry.Index}: {entry.MinHits} to {entry.MaxHits} hits");
        if (detailed)
        {
            Console.WriteLine();
            for (var i = entry.MinHits; i <= entry.MaxHits; i++)
            {
                var weight = entry.Weights[i - entry.MinHits];
                double chance = weight / weightSum * 100.0;
                Console.WriteLine($"  {chance:f2}% chance to do {i} hits (weight {weight})");
            }
        }
        else
        {
            Console.Write(" (");
            for (var i = entry.MinHits; i <= entry.MaxHits; i++)
            {
                var weight = entry.Weights[i - entry.MinHits];
                double chance = weight / weightSum * 100.0;
                if (i > entry.MinHits) {
                    Console.Write(" ");
                }
                Console.Write($"{chance:f1}%");
            }
            Console.WriteLine(")");
        }
    }
}
