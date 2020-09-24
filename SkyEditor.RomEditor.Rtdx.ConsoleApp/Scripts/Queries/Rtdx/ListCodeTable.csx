#load "../../../Stubs/Rtdx.csx"

using System;

var table = Rom.GetCodeTable();
foreach (var entry in table.EntriesByCodeString.Values)
{
    Console.WriteLine($"[{entry.CodeString}] = {entry.UnicodeValue.ToString("X")} (ukn = {entry.Unknown.ToString("X")}, "
        + $"flags = {entry.Flags.ToString("X")}), replaceByte0 = {entry.DigitFlag}");
}
