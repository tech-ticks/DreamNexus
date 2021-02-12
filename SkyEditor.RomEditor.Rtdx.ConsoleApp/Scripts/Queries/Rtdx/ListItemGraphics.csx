#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

var entries = Rom.GetItemGraphics().Entries.Values.ToArray();

Console.WriteLine($"#     {"ModelName",-20}   B00   B01   B02   B03   Height           Key   B0C   B0D   B0E   S0F");
Console.WriteLine("".PadRight(120, '-'));

for (int i = 0; i < entries.Length; i++)
{
    var entry = entries[i];
    Console.WriteLine($"{i,-3}   "
        + $"{entry.ModelName,-20}   "
        + $"{entry.Byte00,3}   "
        + $"{entry.Byte01,3}   "
        + $"{entry.Byte02,3}   "
        + $"{entry.Byte03,3}   "
        + $"{entry.Height,-5}   "
        + $"{entry.Key,12}   "
        + $"{entry.Byte0C,3}   "
        + $"{entry.Byte0D,3}   "
        + $"{entry.Byte0E,3}   "
        + $"{entry.Short0F,3}   "
    );
}
