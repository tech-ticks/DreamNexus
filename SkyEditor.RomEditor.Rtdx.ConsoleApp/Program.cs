using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Domain;
using SkyEditor.RomEditor.Domain.Library;
using SkyEditor.RomEditor.Domain.Psmd;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure.Automation.CSharp;
using SkyEditor.RomEditor.Infrastructure.Automation.Lua;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.ConsoleApp
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
            Console.WriteLine("UnFARC <File.bin> <OutputDirectory> - Extracts a FARC archive file to the given directory");
            Console.WriteLine("ReFARC <InputDirectory> <File.bin> - Creates a new FARC archive file from files in the given directory");
            Console.WriteLine();
            Console.WriteLine("Additional options:");
            Console.WriteLine("--enable-custom-files: Prefer custom file formats over editing the ROM's binary directly (experimental).");
            Console.WriteLine("                       Download a build from https://github.com/tech-ticks/hyperbeam to use custom file formats.");
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
                RomLibrary = new Library("Library", fileSystem),
                VerboseLogging = args.Any(arg => arg == "--verbose")
            };

            while (arguments.TryDequeue(out var arg))
            {
                if (string.Equals(arg, "--save", StringComparison.OrdinalIgnoreCase))
                {
                    if (context.Rom == null)
                    {
                        throw new InvalidOperationException("Argument '--save' must follow a ROM directory argument");
                    }
                    if (!(context.Rom is ISaveable saveableRom))
                    {
                        throw new NotSupportedException($"ROM of type {context.Rom.GetType().Name} does not implement ISaveable");
                    }

                    await saveableRom.Save();
                    if (context.VerboseLogging) Console.WriteLine("Saved");
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

                    if (!(context.Rom is ISaveableToDirectory saveableRom))
                    {
                        throw new NotSupportedException($"ROM of type {context.Rom.GetType().Name} does not implement ISaveableToDirectory");
                    }

                    var targetDirectory = GetRomDirectory(target, context);

                    await saveableRom.Save(targetDirectory, fileSystem);
                    if (context.VerboseLogging) Console.WriteLine("Saved to " + targetDirectory);
                }
                else if (arg.StartsWith("library:"))
                {
                    var libraryItemName = arg.Split(':', 2)[1];
                    var libraryItem = context.RomLibrary.GetItem(libraryItemName);
                    if (libraryItem == null)
                    {
                        throw new DirectoryNotFoundException($"Could not find a library item with the name '{libraryItemName}'");
                    }

                    var rom = await RomLoader.LoadRom(libraryItem.FullPath, fileSystem) ?? throw new ArgumentException($"Unable to determine the type of ROM located at {arg}");
                    
                    if (rom is IRtdxRom rtdxRom)
                    {
                        rtdxRom.EnableCustomFiles = args.Contains("--enable-custom-files");
                    }

                    context.Rom = rom;
                    context.RomPath = libraryItem.FullPath;
                    if (context.VerboseLogging) Console.WriteLine($"Loaded {arg}");
                }
                else if (Directory.Exists(arg))
                {
                    if (File.Exists(Path.Combine(arg, "modpack.json")) || File.Exists(Path.Combine(arg, "mod.json")))
                    {
                        await ApplyMod(arg, context);
                    }
                    else
                    {
                        var rom = await RomLoader.LoadRom(arg, fileSystem);
                        context.Rom = rom ?? throw new ArgumentException($"Unable to determine the type of ROM located at {arg}");
                        context.RomPath = arg;
                        if (context.VerboseLogging) Console.WriteLine($"Loaded {arg}");
                    }
                }
                else if (File.Exists(arg))
                {
                    var rom = await RomLoader.LoadRom(arg, fileSystem);
                    if (rom != null)
                    {
                        context.Rom = rom;
                        context.RomPath = arg;
                        if (context.VerboseLogging) Console.WriteLine($"Loaded {arg}");
                    }
                    else
                    {
                        // Assume it's a mod
                        await ApplyMod(arg, context);
                    }
                }
                else if (Commands.TryGetValue(arg, out var command))
                {
                    await command(arguments, context);
                }
                else if (arg == "--verbose")
                {
                    // Do nothing, verbose logging has already been enabled
                }
                else
                {
                    if (context.VerboseLogging) Console.WriteLine($"Unrecognized argument '{arg}'");
                    return;
                }
            }
        }

        private static async Task ApplyMod(string modPath, ConsoleContext context)
        {
            if (context.Rom == null)
            {
                throw new InvalidOperationException("Mod argument must follow a ROM argument");
            }

            if (context.Rom is IRtdxRom rtdx)
            {
                using var modpack = new RtdxModpack(modPath, context.FileSystem);
                await modpack.Apply<IRtdxRom>(rtdx);
            }
            else if (context.Rom is IPsmdRom psmd)
            {
                // TODO: re-introduce support
                // await modpack.Apply<IPsmdRom>(psmd);
            }
            else
            {
                throw new ArgumentException("Unsupported ROM type: " + context.Rom.GetType().Name);
            }
        }

        #region Commands
        private delegate Task ConsoleCommand(Queue<string> arguments, ConsoleContext context);

        private static readonly Dictionary<string, ConsoleCommand> Commands = new Dictionary<string, ConsoleCommand>(StringComparer.OrdinalIgnoreCase)
        {
            { "Import", Import },
            { "ListLibrary", ListLibrary },
            { "LuaGen", GenerateLuaChangeScript }, { "GenerateLuaChangeScript", GenerateLuaChangeScript },
            { "CSGen", GenerateCSharpChangeScript },
            { "Pack", BuildModpack }, { "BuildModpack", BuildModpack },
            { "Unfarc", Unfarc },
            { "Refarc", Refarc }
        };

        private static async Task Import(Queue<string> arguments, ConsoleContext context)
        {
            if (context.RomPath == null)
            {
                throw new InvalidOperationException("Import must follow a ROM argument");
            }

            if (!arguments.TryDequeue(out var targetName))
            {
                throw new ArgumentException("Argument 'Import' must be followed by a target library item name");
            }

            if (context.VerboseLogging) Console.WriteLine($"Importing '{context.RomPath}' to library:{targetName}");
            if (Directory.Exists(context.RomPath))
            {
                await context.RomLibrary.AddDirectoryAsync(context.RomPath, context.FileSystem, targetName);
            }
            else if (File.Exists(context.RomPath))
            {
                await context.RomLibrary.AddFileAsync(context.RomPath, context.FileSystem, targetName);
            }
            else
            {
                throw new ArgumentException($"Unable to find a file or directory at '{context.RomPath}'");
            }
            if (context.VerboseLogging) Console.WriteLine("Import complete");
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

            if (!(context.Rom is ILuaChangeScriptGenerator changeScriptGenerator))
            {
                throw new NotSupportedException($"ROM of type {context.Rom.GetType().Name} does not implement ILuaChangeScriptGenerator");
            }

            var script = changeScriptGenerator.GenerateLuaChangeScript();

            if (context.VerboseLogging) Console.WriteLine("Change script:");
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

            if (!(context.Rom is ICSharpChangeScriptGenerator changeScriptGenerator))
            {
                throw new NotSupportedException($"ROM of type {context.Rom.GetType().Name} does not implement ICSharpChangeScriptGenerator");
            }

            var script = changeScriptGenerator.GenerateCSharpChangeScript();

            if (context.VerboseLogging) Console.WriteLine("Change script:");
            Console.WriteLine(script);

            if (arguments.TryDequeue(out var targetFileName))
            {
                File.WriteAllText(targetFileName, script);
            }

            return Task.CompletedTask;
        }        

        /// <summary>
        /// Creates a modpack from one or more modpacks, mods, or scripts
        /// </summary>
        /// <remarks>
        /// Sample usage: BuildModpack Mods/Mod1 Mods/Mod2 --disabled Mods/Mod3 --id "SkyEditor.Modpack" --name="My Modpack" --save-to modpack.zip
        /// This will create a modpack called "My Modpack" with 3 mods. Mod2 will be disabled by default.
        /// </remarks>
        private static async Task BuildModpack(Queue<string> arguments, ConsoleContext context)
        {
            var builder = new ModpackBuilder();
            while (arguments.TryDequeue(out var arg))
            {
                var modpackMetadataType = builder.Metadata.GetType();
                if (arg == "--save-to")
                {
                    var filename = arguments.Dequeue();
                    await builder.Build(filename);
                    if (context.VerboseLogging) Console.WriteLine("Saved modpack to " + filename);
                    return;
                }
                else if (arg.StartsWith("--"))
                {
                    var parts = arg.TrimStart('-').Split('=', 2);
                    var propertyName = parts[0];
                    var propertyValue = parts[1];
                    var property = modpackMetadataType.GetProperties().FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
                    if (property == null)
                    {
                        Console.Error.Write($"Warning: Unable to find property '{propertyName}' in modpack metadata. Skipping argument '{arg}'");
                        continue;
                    }
                    property.SetValue(builder.Metadata, Convert.ChangeType(propertyValue, property.PropertyType));
                }
                else if (File.Exists(arg) || Directory.Exists(arg))
                {
                    var enabled = true;
                    if (arguments.TryPeek(out var nextArg) && nextArg == "--disabled")
                    {
                        arguments.Dequeue();
                        enabled = false;
                    }

                    // TODO: PSMD support
                    var modpack = new RtdxModpack(arg, context.FileSystem);
                    foreach (var mod in modpack.Mods ?? Enumerable.Empty<Mod>())
                    {
                        builder.AddMod(mod, enabled);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Unrecognized argument in modpack builder: '{arg}'");
                }
            }
            
            Console.Error.Write("Reached end of arguments without saving modpack.");
        }

        /// <summary>
        /// Extracts an RTDX FARC archive
        /// </summary>
        /// <returns>
        /// Usage: unfarc File.bin OutputDirectory/
        /// </returns>
        private static async Task Unfarc(Queue<string> arguments, ConsoleContext context)
        {
            var filename = arguments.Dequeue();
            var outputDirectory = arguments.Dequeue();

            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("Unable to find FARC at the given path", filename);
            }

            var farc = new Farc(File.ReadAllBytes(filename));
            await farc.Extract(outputDirectory, PhysicalFileSystem.Instance);
        }

        /// <summary>
        /// Creates a new RTDX FARC archive
        /// </summary>
        /// <returns>
        /// Usage: refarc InputDirectory/ File.bin
        /// </returns>
        private static Task Refarc(Queue<string> arguments, ConsoleContext context)
        {
            var inputDiretcory = arguments.Dequeue();
            var outputFile = arguments.Dequeue();

            var farc = Farc.FromDirectory(inputDiretcory, PhysicalFileSystem.Instance);
            File.WriteAllBytes(outputFile, farc.ToByteArray());

            return Task.CompletedTask;
        }

        #endregion

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
            public bool VerboseLogging { get; set; }

            public IFileSystem FileSystem { get; set; } = default!;
            public ILibrary RomLibrary { get; set; } = default!;

            public IModTarget? Rom { get; set; }
            public string? RomPath { get; set; }
        }
    }
}
