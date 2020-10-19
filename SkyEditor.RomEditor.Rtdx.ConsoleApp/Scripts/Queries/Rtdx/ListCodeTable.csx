#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

var table = (RtdxCodeTable) Rom.GetCodeTable();
foreach (var entry in table.EntriesByCodeString.Values)
{
    Console.WriteLine($"[{entry.CodeString}] = {entry.UnicodeValue.ToString("X")} (ukn = {entry.Unknown.ToString("X")}, length = {entry.Length}, "
        + $"flags = {entry.Flags.ToString("X")}), digitFlag = {entry.DigitFlag}");
}
