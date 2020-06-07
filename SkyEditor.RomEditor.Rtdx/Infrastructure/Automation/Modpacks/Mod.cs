using SkyEditor.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Infrastructure.Automation.Modpacks
{
    public class Mod : IScriptModAccessor
    {
        public Mod(ModMetadata metadata, string directory, IReadOnlyFileSystem fileSystem)
        {
            this.directory = directory ?? throw new ArgumentNullException(nameof(directory));
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

            this.Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            this.Enabled = metadata.Enabled;

            var scripts = new List<Script>();
            foreach (var scriptRelativePath in metadata.Scripts ?? Enumerable.Empty<string>())
            {
                ScriptType scriptType;
                var extension = Path.GetExtension(scriptRelativePath);
                if (string.Equals(extension, ".csx", StringComparison.OrdinalIgnoreCase))
                {
                    scriptType = ScriptType.CSharp;
                }
                else if (string.Equals(extension, ".lua", StringComparison.OrdinalIgnoreCase))
                {
                    scriptType = ScriptType.Lua;
                }
                else
                {
                    throw new UnsupportedScriptTypeException(extension);
                }

                scripts.Add(new Script(scriptType, ReadResourceText(scriptRelativePath)));
            }

            this.Scripts = scripts;
        }

        private readonly string directory;
        private readonly IReadOnlyFileSystem fileSystem;

        public ModMetadata Metadata { get; }
        public IReadOnlyList<Script> Scripts { get; }

        public bool Enabled { get; set; }

        public async Task Apply(IScriptHost context)
        {
            foreach (var script in Scripts)
            {
                switch (script.Type) 
                {
                    case ScriptType.CSharp:
                        await context.ExecuteCSharp(script.Value).ConfigureAwait(false);
                        break;
                    case ScriptType.Lua:
                        context.ExecuteLua(script.Value);
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported script type: " + script.Type.ToString("f"));
                }
            }
        }

        /// <summary>
        /// Copies the contents of the mod to the given directory on the target fileSystem
        /// </summary>
        /// <param name="targetDirectory">Directory into which the contents of the mod should be copied</param>
        /// <param name="fileSystem">File system into which the contents of the mod should be copied</param>
        public async Task CopyToDirectory(string targetDirectory, IFileSystem targetFileSystem)
        {
            foreach (var filename in this.fileSystem.GetFiles(this.directory, "*", false))
            {
                var destFilename = Path.Combine(targetDirectory, FileSystemExtensions.GetRelativePath(GetBaseDirectory(), filename));
                var destDirectory = Path.GetDirectoryName(destFilename);
                if (!string.IsNullOrEmpty(destDirectory) && !targetFileSystem.DirectoryExists(destDirectory))
                {
                    targetFileSystem.CreateDirectory(destDirectory);
                }

                using var sourceFile = this.fileSystem.OpenFileReadOnly(filename);
                using var destFile = targetFileSystem.OpenFileWriteOnly(destFilename);
                await sourceFile.CopyToAsync(destFile).ConfigureAwait(false);
            }
        }

        private string GetBaseDirectory()
        {
            if (!string.IsNullOrEmpty(Metadata.BaseDirectory))
            {
                return Path.Combine(directory, Metadata.BaseDirectory);
            }
            else
            {
                return directory;
            }
        }

        /// <summary>
        /// Reads a file from the mod
        /// </summary>
        /// <param name="resourcePath">Path of the resource file, relative to the directory in which the mod.json or the modpack.json is located.</param>
        /// <returns>A stream allowing access to the resource data</returns>
        public Stream ReadResourceStream(string resourcePath)
        {
            var absolutePath = Path.Combine(GetBaseDirectory(), resourcePath);
            return fileSystem.OpenFileReadOnly(absolutePath);
        }

        /// <summary>
        /// Reads a file from the mod
        /// </summary>
        /// <param name="resourcePath">Path of the resource file, relative to the directory in which the mod.json or the modpack.json is located.</param>
        /// <returns>An array of byte containing the resource data</returns>
        public byte[] ReadResourceArray(string resourcePath)
        {
            var absolutePath = Path.Combine(GetBaseDirectory(), resourcePath);
            return fileSystem.ReadAllBytes(absolutePath);
        }

        /// <summary>
        /// Reads a file from the mod
        /// </summary>
        /// <param name="resourcePath">Path of the resource file, relative to the directory in which the mod.json or the modpack.json is located.</param>
        /// <returns>A string containing the resource data</returns>
        public string ReadResourceText(string resourcePath)
        {
            var absolutePath = Path.Combine(GetBaseDirectory(), resourcePath);
            return fileSystem.ReadAllText(absolutePath);
        }
    }
}
