using System;
using System.IO;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Infrastructure.Interfaces;

namespace SkyEditor.RomEditor.Infrastructure
{
    public class OutputPaths
    {
        public string ModpackRoot { get; set; } = "";
    }

    public static class SwitchBuildHelpers
    {
        public const string TitleId = "01003D200BAA2000";

        public static OutputPaths CreateDirectoryStructure(string buildPath, string modpackId, OutputStructureType structureType, bool useCodeInjection)
        {
            if (!useCodeInjection)
            {
                var root = Path.Combine(buildPath, "atmosphere/contents/01003D200BAA2000");
                Directory.CreateDirectory(root);
                return new OutputPaths
                {
                    ModpackRoot = root
                };
            }

            switch (structureType)
            {
                case OutputStructureType.Atmosphere:
                    return CreateAtmosphereDirectoryStructure(buildPath, modpackId);
                case OutputStructureType.Ryujinx:
                    return CreateRyujinxDirectoryStructure(buildPath, modpackId);
                default:
                    throw new ArgumentException($"unknown file structure type {structureType}", nameof(structureType));
            }
        }

        private static OutputPaths CreateAtmosphereDirectoryStructure(string buildPath, string modpackId)
        {
            var contentsPath = Path.Combine(buildPath, "atmosphere/contents/01003D200BAA2000");
            var exefsPath = Path.Combine(contentsPath, "exefs");
            var romfsPath = Path.Combine(contentsPath, "romfs");

            var modpackRoot = Path.Combine(romfsPath, $"hyperbeam/modpacks/{modpackId}");

            Directory.CreateDirectory(exefsPath);
            Directory.CreateDirectory(modpackRoot);

            return new OutputPaths
            {
                ModpackRoot = modpackRoot
            };
        }

        private static OutputPaths CreateRyujinxDirectoryStructure(string buildPath, string modpackId)
        {
            throw new NotImplementedException();
        }

        public static void CopyCodeInjectionBinaries(string buildPath, OutputStructureType structureType)
        {
            // TODO: implement
        }

        public static void CopyRecursively(string source, string destination)
        {
            DirectoryInfo dir = new DirectoryInfo(source);

            DirectoryInfo[] dirs = dir.GetDirectories();    
            Directory.CreateDirectory(destination);

            FileInfo[] files = dir.GetFiles();
            foreach (var file in files)
            {
                string tempPath = Path.Combine(destination, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            foreach (var subdir in dirs)
            {
                string tempPath = Path.Combine(destination, subdir.Name);
                CopyRecursively(subdir.FullName, tempPath);
            }
        }
    }
}
