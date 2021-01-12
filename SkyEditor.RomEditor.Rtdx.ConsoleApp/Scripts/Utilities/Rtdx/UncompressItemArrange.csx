#load "../../../Stubs/Rtdx.csx"

using System;
using System.IO;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

int i = 0;
foreach (var entry in Rom.GetItemArrange().Entries)
{
  File.WriteAllBytes($"item_arrange_{i}.bin", entry.Data);
  i++;
}
