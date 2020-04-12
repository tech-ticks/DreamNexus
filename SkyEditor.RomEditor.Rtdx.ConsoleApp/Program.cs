using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Rtdx.Domain;
using SkyEditor.RomEditor.Rtdx.Domain.Automation;
using System;
using System.IO;

namespace SkyEditor.RomEditor.Rtdx.ConsoleApp
{
    class Program
    {
        static void PrintUsage()
        {
            Console.WriteLine("SkyEditor.RomEditor.Rtdx.Console <RomDirectory> <Script.lua>");
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                PrintUsage();
                return;
            }

            var romDirectory = args[0];
            var scriptFilename = args[1];

            var rom = new RtdxRom(romDirectory, PhysicalFileSystem.Instance);
            var luaContext = new SkyEditorLuaContext(rom);
            luaContext.Execute(File.ReadAllText(scriptFilename));
        }
    }
}
