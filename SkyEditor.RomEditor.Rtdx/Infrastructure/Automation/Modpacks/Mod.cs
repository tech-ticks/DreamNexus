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

            LoadScripts();
        }

        private void LoadScripts()
        {
            var scripts = new List<Script>();
            foreach (var scriptRelativePath in Metadata.Scripts ?? Enumerable.Empty<string>())
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

                scripts.Add(new Script(scriptType, scriptRelativePath, ReadResourceText(scriptRelativePath)));
            }

            this.Scripts = scripts;
        }

        public void AddScript(string path)
        {
            if (!(fileSystem is IFileSystem rwFileSystem))
            {
                throw new InvalidOperationException("Cannot add script to read-only mod.");
            }
            rwFileSystem.WriteAllText(Path.Combine(GetBaseDirectory(), path), "");
            Metadata?.Scripts?.Add(path);
            LoadScripts();
        }

        private readonly string directory;
        private readonly IReadOnlyFileSystem fileSystem;

        public ModMetadata Metadata { get; }
        public IReadOnlyList<Script> Scripts { get; private set; } = new List<Script>();

        public bool Enabled { get; set; }
        public bool ReadOnly => !(fileSystem is IFileSystem);

        public async Task Apply(IScriptHost context)
        {
            await WriteAssets(context);

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

        public async Task WriteAssets(IScriptHost context)
        {
            // TODO: Directly copy the files instead of loading all assets in RAM
            var assetsDir = GetAssetsDirectory();
            if (!fileSystem.DirectoryExists(assetsDir))
            {
                return;
            }

            var tasks = new List<Task>();
            foreach (var assetPath in fileSystem.GetFiles(assetsDir, "*", false))
            {
                var relativePath = FileSystemExtensions.GetRelativePath(assetsDir, assetPath);
                var targetPath = Path.Combine("romfs", "Data", "StreamingAssets", relativePath);
                tasks.Add(WriteAsset(context.Target, assetPath, targetPath));
            }

            await Task.WhenAll(tasks);
        }

#pragma warning disable CS1998
        private async Task WriteAsset(IModTarget target, string assetPath, string targetPath)
        {
#if !NETSTANDARD2_0
            target.WriteFile(targetPath, await File.ReadAllBytesAsync(assetPath));
#else
            target.WriteFile(targetPath, File.ReadAllBytes(assetPath));
#endif
        }
#pragma warning restore CS1998

        public async Task SaveModel(object model, string relativePath)
        {
            var fs = fileSystem as IFileSystem;
            if (fs == null)
            {
                throw new Exception("Attempted to save a read-only mod");
            }

            var fullPath = Path.Combine(GetDataDirectory(), relativePath);
            var directoryPath = Path.GetDirectoryName(fullPath);
            if (!fs.DirectoryExists(directoryPath!))
            {
                fs.CreateDirectory(directoryPath!);
            }

            var yaml = Modpack.YamlSerializer.Serialize(model);
            if (fs.FileExists(fullPath))
            {
                fs.DeleteFile(fullPath);
            }
            await fs.WriteAllTextAsync(fullPath, yaml);
        }

        public async Task<TModel> LoadModel<TModel>(string relativePath)
        {
            var fullPath = Path.Combine(GetDataDirectory(), relativePath);
            var text = await fileSystem.ReadAllTextAsync(fullPath);
            return Modpack.YamlDeserializer.Deserialize<TModel>(text);
        }

        public bool ModelExists(string relativePath)
        {
            var fullPath = Path.Combine(GetDataDirectory(), relativePath);
            return fileSystem.FileExists(fullPath);
        }

        public bool ModelDirectoryExists(string relativePath)
        {
            var fullPath = Path.Combine(GetDataDirectory(), relativePath);
            return fileSystem.DirectoryExists(fullPath);
        }

        public string[] GetModelFilesInDirectory(string relativePath)
        {
            var fullPath = Path.Combine(GetDataDirectory(), relativePath);
            if (!fileSystem.DirectoryExists(fullPath))
            {
                return new string[0];
            }
            return fileSystem.GetFiles(fullPath, "*", true);
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

        public string GetBaseDirectory()
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

        public string GetDataDirectory() => Path.Combine(GetBaseDirectory(), "Data");
        public string GetAssetsDirectory() => Path.Combine(GetBaseDirectory(), "Assets");

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

        public Mod Clone(string newDirectory, IFileSystem fileSystem)
        {
            return new Mod(Metadata.Clone(), newDirectory, fileSystem);
        }
    }
}
