#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

var rareQualities = Rom.GetCommonStrings().RareQualities;
foreach (var rareQuality in rareQualities)
{
    Console.WriteLine($"{(int)rareQuality.Key,2}: {rareQuality.Key,-22}  {rareQuality.Value}");
}
