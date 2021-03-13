#load "../../../Stubs/Rtdx.csx"

// This script helps create binary patches that are resilient against changes between versions by
// generating code that uses offsets relative to the start of the current function. Call it with
// [SkyEditor Deluxe console app path] [path to ROM] Scripts/Utilities/Rtdx/CreateMethodRelativeOffsetCode.csx [offset]
// where the offset is the one displayed in the reverse engineering tool (relative to .text).
// You can copy the output to your own script to write to the correct offset.

using System;
using System.Collections.Generic;
using System.IO;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Structures.Executable;
using SkyEditor.RomEditor.Infrastructure;
using Newtonsoft.Json;
using Il2CppInspector.Reflection;

var args = Environment.GetCommandLineArgs();
if (Convert.ToInt64(args.Last(), 16) < 0)
{
  throw new ArgumentException("Call this script with the offset in .text as the last argument.");
}
ulong offset = Convert.ToUInt64(args.Last(), 16);

public struct MethodData
{
  public string TypeName;
  public string MethodName;
  public ulong Offset;
  public string[] ParameterTypes;
  public bool IsConstructor;
}

string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
string cacheDir = $"{homeDir}/.skyeditor";
string cacheFile = $"{cacheDir}/ilMethodData.json";
if (!Directory.Exists(cacheDir))
{
  Directory.CreateDirectory(cacheDir);
}

MethodData[] transformedMethodData;
if (!File.Exists(cacheFile))
{
  // Write to a cache since it would be too slow to read this every time 
  // TODO: this still takes a while, see if we can make it faster
  var executable = Rom.GetMainExecutable();
  ulong textOffset = executable.CodeSectionOffset;

  var allMethods = executable.IlAppModel.TypeModel.Types.SelectMany(type => 
    type.GetAllMethods().Cast<MethodBase>().Concat(type.DeclaredConstructors));

  transformedMethodData = allMethods
    .Where(m => executable.IlAppModel.Methods.ContainsKey(m))
    .Select(m => new MethodData {
      TypeName = m.DeclaringType.FullName,
      MethodName = m.Name,
      Offset = executable.IlAppModel.Methods[m].MethodCodeAddress,
      ParameterTypes = m.DeclaredParameters.Select(p => p.ParameterType.FullName).ToArray(),
      IsConstructor = m is ConstructorInfo,
    }).ToArray();

  string serializedData = JsonConvert.SerializeObject(transformedMethodData);
  File.WriteAllText(cacheFile, serializedData);
}
else
{
  string serializedData = File.ReadAllText(cacheFile);
  transformedMethodData = JsonConvert.DeserializeObject<MethodData[]>(serializedData);
}

var closestMethod = transformedMethodData
  .Where(m => m.Offset <= offset)
  .OrderByDescending(m => m.Offset)
  .First();

ulong methodRelativeOffset = offset - closestMethod.Offset;
string joinedParameters = string.Join(", ", closestMethod.ParameterTypes.Select(t => $"\"{t}\""));

if (closestMethod.IsConstructor)
{
  Console.WriteLine($"codeHelper.SetOffsetToConstructor(\"{closestMethod.TypeName}\", "
    + $"new string[] {{{joinedParameters}}});");
}
else
{
  Console.WriteLine($"codeHelper.SetOffsetToMethod(\"{closestMethod.TypeName}\", \"{closestMethod.MethodName}\", "
    + $"new string[] {{{joinedParameters}}});");
}
Console.WriteLine($"codeHelper.Offset += 0x{methodRelativeOffset.ToString("x")};");
