using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Rtdx.Domain;
using SkyEditor.RomEditor.Rtdx.Domain.Automation;
using System;
using System.IO;
using System.Linq;

namespace SkyEditor.RomEditor.Rtdx.ConsoleApp
{
    class Program
    {
        static void PrintUsage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll <RomDirectory> <Script1.lua> [Script2.lua] [...] [--save | --save-to <RomDirectory>]");
            Console.WriteLine();
            Console.WriteLine("Examples: ");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Queries/ListStarters.lua");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Samples/ChangeStarters.lua --save");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Samples/ChangeStarters.lua --save-to ./RTDX-modified");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Samples/ChangeStarters.lua ./Scripts/Hypothetical/ChangeMoreStarters.lua --save-to ./RTDX-modified");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Queries/ListStarters.lua ./RTDX-Copy ./Scripts/Samples/ChangeStarters.lua ./Scripts/Hypothetical/ChangeMoreStarters.lua --save");
        }

        static void Main(string[] args)
        {
            if (args.Length < 2 || string.Equals(args[0], "--help", StringComparison.OrdinalIgnoreCase))
            {
                PrintUsage();
                return;
            }

            var fileSystem = PhysicalFileSystem.Instance;
            RtdxRom? rom = null;
            SkyEditorLuaContext? context = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], "--save", StringComparison.OrdinalIgnoreCase))
                {
                    if (rom == null)
                    {
                        throw new InvalidOperationException("Argument '--save' must follow a ROM directory argument");
                    }
                    rom.Save();
                    Console.WriteLine("Saved");
                }
                else if (string.Equals(args[i], "--save-to", StringComparison.OrdinalIgnoreCase))
                {
                    if (rom == null)
                    {
                        throw new InvalidOperationException("Argument '--save-to' must follow a ROM directory argument");
                    }
                    if (i + 1 < args.Length)
                    {
                        throw new ArgumentException("Argument '--save-to' must be followed by a ROM directory argument");
                    }
                    rom.Save(args[i + 1], fileSystem);
                    Console.WriteLine("Saved to " + args[i + 1]);
                }
                else if (Directory.Exists(args[i]))
                {
                    rom = new RtdxRom(args[i], fileSystem);
                    context = new SkyEditorLuaContext(rom);
                    Console.WriteLine("Loaded " + args[i]);
                }
                else if (File.Exists(args[i]))
                {
                    var extension = Path.GetExtension(args[i]);
                    if (string.Equals(extension, ".lua", StringComparison.OrdinalIgnoreCase))
                    {
                        if (context == null)
                        {
                            throw new InvalidOperationException("ROM directory argument must precede Lua script argument");
                        }

                        Console.WriteLine(args[i] + ": ");
                        context.Execute(File.ReadAllText(args[i]));
                    }
                }
                else
                {
                    Console.WriteLine($"Unrecognized argument '{args[i]}'");
                    return;
                }
            }
        }
    }
}
