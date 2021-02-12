#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

var entries = Rom.GetItemGraphics().Entries;

Console.WriteLine($"#     {"ModelName",-20}   B00   B01   B02   B03   F04     B08   B09   B0A   B0B   B0C   B0D   B0E   S0F");
Console.WriteLine("".PadRight(120, '-'));

for (int i = 0; i < entries.Count; i++)
{
    var entry = entries[i];
    Console.WriteLine($"{i,-3}   "
        + $"{entry.ModelName,-20}   "
        + $"{entry.Byte00,3}   "
        + $"{entry.Byte01,3}   "
        + $"{entry.Byte02,3}   "
        + $"{entry.Byte03,3}   "
        + $"{entry.Float04,-5}   "
        + $"{entry.Byte08,3}   "
        + $"{entry.Byte09,3}   "
        + $"{entry.Byte0A,3}   "
        + $"{entry.Byte0B,3}   "
        + $"{entry.Byte0C,3}   "
        + $"{entry.Byte0D,3}   "
        + $"{entry.Byte0E,3}   "
        + $"{entry.Short0F,3}   "
    );
}
