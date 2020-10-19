// This script adds comments with the english dialogue lines to all Lua scripts used by the game.

#load "../../../Stubs/RTDX.csx"

using System;
using System.IO;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;
using System.Text.RegularExpressions;

var usMessageBin = Rom.GetUSMessageBin();
var scriptStrings = new MessageBinEntry(Rom.GetUSMessageBin().GetFile("script.bin"), Rom.GetCodeTable());

string LuaScriptsPath = Path.Combine(Rom.RomDirectory, "romfs", "Data", "StreamingAssets", "native_data", "script", "event");

foreach (var luaFile in new DirectoryInfo(LuaScriptsPath).GetFiles("*.lua", SearchOption.AllDirectories))
{
  string[] lines = File.ReadAllLines(luaFile.FullName);
  for (int i = 0; i < lines.Length; i++)
  {
    string line = lines[i];
    var match = Regex.Match(line, "TextID\\(\"(?<textIdValue>.+?)\",");
    if (!match.Success)
    {
      continue;
    }

    string textId = match.Groups["textIdValue"].Value;

    // Story script strings are prefixed with "SCRIPT__"
    int textIdHash = (int)Crc32Hasher.Crc32Hash("SCRIPT__" + textId);

    if (!scriptStrings.Strings.ContainsKey(textIdHash))
    {
      continue;
    }

    string dialogueLine = scriptStrings.Strings[textIdHash].First().Value.Replace("\n", "\\n");
    lines[i] = $"{line} -- {dialogueLine}";
  }

  File.WriteAllLines(luaFile.FullName, lines);
}
