#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

var entries = Rom.GetMapRandomGraphics().Entries;

Console.WriteLine($"#   {"Symbol",-30}  {"AbName",-30}  {"ExFileName",-20}  {"Unk1",12}   Unk2");
Console.WriteLine("".PadRight(110, '-'));

for (int i = 0; i < entries.Count; i++)
{
    var entry = entries[i];
    Console.WriteLine($"{i,-2}  "
        + $"{entry.Symbol,-30}  "
        + $"{entry.AssetBundleName,-30}  "
        + $"{entry.ExtraFileName,-20}  "
        + $"{entry.Unk1,12}   "
        + $"{entry.Unk2,3}   "
    );
}
