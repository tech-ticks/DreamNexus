#load "../../../Stubs/Rtdx.csx"

using System;

var table = Rom.GetCodeTable();
foreach (var entry in table.Entries)
{
    Console.WriteLine($"[{entry.CodeString}] = {entry.UnicodeValue.ToString("X")} (ukn = {entry.Unknown.ToString("X")})");
}
