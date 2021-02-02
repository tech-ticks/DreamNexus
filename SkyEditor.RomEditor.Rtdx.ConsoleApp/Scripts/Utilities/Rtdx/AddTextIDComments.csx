// This script adds comments with the english string values to TextIDHash.cs

#load "../../../Stubs/RTDX.csx"

using System;
using System.IO;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;

var args = Environment.GetCommandLineArgs();
var textIdHashFile = args.Last().TrimStart('#');
if (!File.Exists(textIdHashFile))
{
  throw new ArgumentException("Call this script with the path to TextIDHash.cs preceded with '#' as the last argument.");
}

var usMessageBin = Rom.GetUSMessageBin();
var codeTable = Rom.GetCodeTable();
var messageBinFiles = new string[]
{
  "common.bin",
  "debug.bin",
  "dungeon.bin",
  "script.bin",
  "test.bin",
};

var messageBinEntries = messageBinFiles
  .Select(fileName => new MessageBinEntry(Rom.GetUSMessageBin().GetFile(fileName), codeTable))
  .ToArray();

string[] lines = File.ReadAllLines(textIdHashFile);
for (int i = 0; i < lines.Length; i++)
{
  string line = lines[i];
  var split = line.Split("=");
  if (split.Length != 2)
  {
    continue;
  }

  int hash = Convert.ToInt32(split[1].Trim().TrimEnd(','));

  string textValue = null;
  foreach (var entry in messageBinEntries)
  {
    if (entry.Strings.ContainsKey(hash))
    {
      textValue = entry.Strings[hash].First().Value;
    }
  }

  if (string.IsNullOrWhiteSpace(textValue))
  {
    continue;
  }
  
  if (textValue.Length > 100)
  {
    textValue = textValue.Substring(0, 100) + '…';
  }
  lines[i] += " // " + textValue;
}

File.WriteAllLines(textIdHashFile, lines);
