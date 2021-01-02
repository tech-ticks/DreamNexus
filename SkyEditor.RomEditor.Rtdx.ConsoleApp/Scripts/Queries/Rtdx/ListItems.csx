#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

foreach (var item in Rom.GetCommonStrings().Items)
{
    Console.WriteLine($"{(int) item.Key} {0x((int) item.Key).ToString("x")} {item.Key}: {item.Value}");
}
