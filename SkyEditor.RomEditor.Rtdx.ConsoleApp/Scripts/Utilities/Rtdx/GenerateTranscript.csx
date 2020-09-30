// This script generates a text file containing all dialogues.
// The resulting transcript.txt file will be saved in the current working directory.

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
var scriptStrings = new MessageBinEntry(Rom.GetUSMessageBin().GetFile("script.bin"), Rom.GetCodeTable());

string LuaScriptsPath = Path.Combine(Rom.RomDirectory, "romfs", "Data", "StreamingAssets", "native_data", "script", "event");

var transcript = new StringBuilder();

foreach (var luaFile in new DirectoryInfo(LuaScriptsPath).GetFiles("*.lua", SearchOption.AllDirectories))
{
  transcript.AppendLine("------");
  transcript.AppendLine(luaFile.Name);
  transcript.AppendLine("------");

  string[] lines = File.ReadAllLines(luaFile.FullName);
  for (int i = 0; i < lines.Length; i++)
  {
    string line = lines[i];
    var textIdMatch = Regex.Match(line, "TextID\\(\"(?<textIdValue>.+?)\",");
    if (!textIdMatch.Success)
    {
      continue;
    }

    string textId = textIdMatch.Groups["textIdValue"].Value;

    var actorNameMatch = Regex.Match(line, "LuaSymAct\\(\"(?<actorName>.+?)\"");

    string actorName;
    if (line.ToLower().Contains("monologue"))
    {
      actorName = "(monologue)";
    }
    else
    {
      actorName = actorNameMatch.Success ? actorNameMatch.Groups["actorName"].Value : "";
    }

    // Story script strings are prefixed with "SCRIPT__"
    int textIdHash = (int)Crc32Hasher.Crc32Hash("SCRIPT__" + textId);

    if (!scriptStrings.Strings.ContainsKey(textIdHash))
    {
      continue;
    }

    string dialogueLine = scriptStrings.Strings[textIdHash].First().Value.Replace("\n", "\n  ");
    transcript.AppendLine($"{actorName}: {dialogueLine}");
  }

  transcript.AppendLine();
}

File.WriteAllText("transcript.txt", transcript.ToString());
