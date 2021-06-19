// Creates a CSV dump with strings and their hashes

#load "../../../Stubs/RTDX.csx"

using System;
using System.IO;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

var usMessageBin = Rom.GetUSMessageBin();
// var scriptStrings = new MessageBinEntry(Rom.GetUSMessageBin().GetFile("script.bin"), Rom.GetCodeTable());
var commonStrings = new MessageBinEntry(Rom.GetUSMessageBin().GetFile("common.bin"), Rom.GetCodeTable());

var csv = new StringBuilder();

foreach (var str in commonStrings.Strings)
{
  csv.AppendLine($"{Convert.ToString((uint) str.Key, 16)};{str.Value.FirstOrDefault()?.Value}");
}

File.WriteAllText("strings_and_hashes.csv", csv.ToString());
