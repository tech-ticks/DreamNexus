using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Rtdx.Domain;
using SkyEditor.RomEditor.Rtdx.Domain.Automation;
using SkyEditor.RomEditor.Rtdx.Domain.Automation.Modpacks;
using SkyEditor.RomEditor.Rtdx.Domain.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.ConsoleApp
{
    class Program
    {
        static void PrintUsage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll <RomDirectory|library:itemName> <Script1.lua|Command> [Script2.lua|Command2] [...] [--save | --save-to <RomDirectory>]");
            Console.WriteLine();
            Console.WriteLine("Examples: ");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Queries/ListStarters.lua");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Samples/ChangeStarters.lua --save");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Samples/ChangeStarters.lua --save-to ./RTDX-modified");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Samples/ChangeStarters.lua ./Scripts/Hypothetical/ChangeMoreStarters.lua --save-to ./RTDX-modified");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Queries/ListStarters.lua ./RTDX-Copy ./Scripts/Samples/ChangeStarters.lua ./Scripts/Hypothetical/ChangeMoreStarters.lua --save");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX Import MyRom");
            Console.WriteLine("dotnet SkyEditor.RomEditor.Rtdx.Console.dll library:MyRom ./Scripts/Queries/ListStarters.lua");
            Console.WriteLine();
            Console.WriteLine("Built-in commands: ");
            Console.WriteLine("Import <TargetName> - Adds the currently loaded ROM to the library for future ease of use");
            Console.WriteLine("ListLibrary - Lists items in the library");
            Console.WriteLine("LuaGen [Output.lua] - Creates a Lua change script to automate supported unsaved edits");

        }

        static async Task Main(string[] args)
        {
            if (args.Length < 2 || string.Equals(args[0], "--help", StringComparison.OrdinalIgnoreCase))
            {
                PrintUsage();
                return;
            }

            var arguments = new Queue<string>();
            foreach (var argument in args)
            {
                arguments.Enqueue(argument);
            }

            var fileSystem = PhysicalFileSystem.Instance;
            var context = new ConsoleContext
            {
                FileSystem = fileSystem,
                RomLibrary = new RomLibrary("Library", fileSystem)
            };

            while (arguments.TryDequeue(out var arg))
            {
                if (string.Equals(arg, "--save", StringComparison.OrdinalIgnoreCase))
                {
                    if (context.Rom == null)
                    {
                        throw new InvalidOperationException("Argument '--save' must follow a ROM directory argument");
                    }
                    context.Rom.Save();
                    Console.WriteLine("Saved");
                }
                else if (string.Equals(arg, "--save-to", StringComparison.OrdinalIgnoreCase))
                {
                    if (context.Rom == null)
                    {
                        throw new InvalidOperationException("Argument '--save-to' must follow a ROM directory argument");
                    }

                    if (!arguments.TryDequeue(out var target))
                    {
                        throw new ArgumentException("Argument '--save-to' must be followed by a ROM directory argument");
                    }

                    var targetDirectory = GetRomDirectory(target, context);

                    context.Rom.Save(targetDirectory, fileSystem);
                    Console.WriteLine("Saved to " + targetDirectory);
                }
                else if (arg.StartsWith("library:"))
                {
                    var libraryItemName = arg.Split(':', 2)[1];
                    var libraryItem = context.RomLibrary.GetItem(libraryItemName);
                    if (libraryItem == null)
                    {
                        throw new DirectoryNotFoundException($"Could not find a library item with the name '{libraryItemName}'");
                    }
                    context.Rom = new RtdxRom(libraryItem.FullPath, fileSystem);
                    context.RomDirectory = libraryItem.FullPath;
                    context.ScriptContext = new SkyEditorScriptContext(context.Rom);
                    Console.WriteLine($"Loaded {arg}");
                }
                else if (Directory.Exists(arg))
                {
                    if (File.Exists(Path.Combine(arg, "modpack.json")))
                    {
                        await ApplyMod(arg, context);
                    }
                    else
                    {
                        context.Rom = new RtdxRom(arg, fileSystem);
                        context.RomDirectory = arg;
                        context.ScriptContext = new SkyEditorScriptContext(context.Rom);
                        Console.WriteLine($"Loaded {arg}");
                    }
                }
                else if (File.Exists(arg))
                {
                    await ApplyMod(arg, context);
                }
                else if (Commands.TryGetValue(arg, out var command))
                {
                    await command(arguments, context);
                }
                else
                {
                    Console.WriteLine($"Unrecognized argument '{arg}'");
                    return;
                }
            }
        }

        private static async Task ApplyMod(string modPath, ConsoleContext context)
        {
            if (context.ScriptContext == null)
            {
                throw new InvalidOperationException("Modpack or script argument must follow a ROM argument");
            }

            using var modpack = new Modpack(modPath, context.FileSystem);
            await modpack.Apply(context.ScriptContext);
        }

        private delegate Task ConsoleCommand(Queue<string> arguments, ConsoleContext context);

        private static readonly Dictionary<string, ConsoleCommand> Commands = new Dictionary<string, ConsoleCommand>(StringComparer.OrdinalIgnoreCase)
        {
            { "Import", Import },
            { "ListLibrary", ListLibrary },
            { "LuaGen", GenerateLuaChangeScript },
            { "CSGen", GenerateCSharpChangeScript },
            { "GenerateLuaChangeScript", GenerateLuaChangeScript },
            { "LoadAssets", LoadAssets },
            { "Test", Test }
        };

        private static async Task Import(Queue<string> arguments, ConsoleContext context)
        {
            if (context.RomDirectory == null)
            {
                throw new InvalidOperationException("Import must follow a ROM argument");
            }

            if (!arguments.TryDequeue(out var targetName))
            {
                throw new ArgumentException("Argument 'Import' must be followed by a target library item name");
            }

            Console.WriteLine($"Importing '{context.RomDirectory}' to library:{targetName}");
            await context.RomLibrary.AddDirectoryAsync(context.RomDirectory, context.FileSystem, targetName);
            Console.WriteLine("Import complete");
        }

        private static Task ListLibrary(Queue<string> arguments, ConsoleContext context)
        {
            Console.WriteLine("Items in library:");
            foreach (var item in context.RomLibrary.GetItems().OrderBy(i => i.Name))
            {
                Console.WriteLine(item.Name);
            }
            return Task.CompletedTask;
        }

        private static Task GenerateLuaChangeScript(Queue<string> arguments, ConsoleContext context)
        {
            if (context.Rom == null)
            {
                throw new InvalidOperationException("Import must follow a ROM argument");
            }
            var script = context.Rom.GenerateLuaChangeScript();

            Console.WriteLine("Change script:");
            Console.WriteLine(script);

            if (arguments.TryDequeue(out var targetFileName))
            {
                File.WriteAllText(targetFileName, script);
            }

            return Task.CompletedTask;
        }

        private static Task GenerateCSharpChangeScript(Queue<string> arguments, ConsoleContext context)
        {
            if (context.Rom == null)
            {
                throw new InvalidOperationException("Import must follow a ROM argument");
            }
            var script = context.Rom.GenerateCSharpChangeScript();

            Console.WriteLine("Change script:");
            Console.WriteLine(script);

            if (arguments.TryDequeue(out var targetFileName))
            {
                File.WriteAllText(targetFileName, script);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Place C# code here for development/testing purposes, for when lua scripts either aren't enough or when more advanced debugging features are needed.
        /// </summary>
        private static Task LoadAssets(Queue<string> arguments, ConsoleContext context)
        {
            if (context.Rom == null)
            {
                throw new InvalidOperationException("LoadAssets must follow a ROM argument");
            }

#pragma warning disable IDE0059 // Unnecessary assignment of a value (Need it in a variable to browse it with the debugger)
            var assets = context.Rom.GetAssetBundles();
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            return Task.CompletedTask;
        }

        /// <summary>
        /// For those times you need to throw together some temporary test code, but don't want to bother with Lua scripts
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task Test(Queue<string> arguments, ConsoleContext context)
        {
            if (context.Rom == null)
            {
                throw new InvalidOperationException("Test must follow a ROM argument");
            }

            context.Rom.GetPokemonGraphicsDatabase();
            context.Rom.Save("test-output", PhysicalFileSystem.Instance);

            return Task.CompletedTask;
        }

        private static string GetRomDirectory(string directoryOrLibrary, ConsoleContext context)
        {
            if (directoryOrLibrary.StartsWith("library:"))
            {
                var libraryItemName = directoryOrLibrary.Split(':', 2)[1];
                var libraryItem = context.RomLibrary.GetItem(libraryItemName);
                if (libraryItem == null)
                {
                    throw new DirectoryNotFoundException($"Could not find a library item with the name '{libraryItemName}'");
                }
                return libraryItem.FullPath;
            }
            else
            {
                return directoryOrLibrary;
            }
        }

        private class ConsoleContext
        {
            public IFileSystem FileSystem { get; set; } = default!;
            public IRomLibrary RomLibrary { get; set; } = default!;

            public RtdxRom? Rom { get; set; }
            public string? RomDirectory { get; set; }
            public SkyEditorScriptContext? ScriptContext { get; set; }
        }
    }
}
