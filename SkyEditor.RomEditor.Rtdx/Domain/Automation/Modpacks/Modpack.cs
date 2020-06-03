using Newtonsoft.Json;
using SkyEditor.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Automation.Modpacks
{
    public class Modpack : IDisposable
    {
        public Modpack(string path, IFileSystem fileSystem)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            string directory;
            if (fileSystem.DirectoryExists(path))
            {
                directory = path;
                LoadFromFileSystem(fileSystem, directory);
            }
            else if (fileSystem.FileExists(path))
            {
                directory = Path.GetDirectoryName(path);
                var extension = Path.GetExtension(path);
                if (string.Equals(extension, ".zip", StringComparison.OrdinalIgnoreCase))
                {
                    zipStream = fileSystem.OpenFileReadOnly(path);
                    zipArchive = new ZipArchive(zipStream);

                    var zipFileSystem = new ZipFileSystem(zipArchive);
                    LoadFromFileSystem(zipFileSystem, "/");
                }
                else if (string.Equals(extension, ".csx", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(extension, ".lua", StringComparison.OrdinalIgnoreCase))
                {
                    metadata = new ModpackMetadata
                    {
                        Name = Path.GetFileName(path),
                        Description = null,
                        Mods = new List<ModMetadata>
                        {
                            new ModMetadata
                            {
                                Name = Path.GetFileName(path),
                                Description = null,
                                Enabled = true,
                                Scripts = new List<string>
                                {
                                    Infrastructure.FileSystemExtensions.GetRelativePath(directory, path)
                                }
                            }
                        }
                    };
                    mods = metadata.Mods.Select(m => new Mod(m, directory, fileSystem)).ToList();
                }
                else
                {
                    throw new ArgumentException("Unsupported file extension: " + extension, nameof(path));
                }
            }
            else
            {
                throw new DirectoryNotFoundException("Unable to find either a file or a directory at the given path: " + path);
            }
        }

        private void LoadFromFileSystem(IReadOnlyFileSystem fileSystem, string directory)
        {
            var metadataFilename = Path.Combine(directory, "modpack.json");
            if (!fileSystem.FileExists(metadataFilename))
            {
                throw new FileNotFoundException("Could not find a modpack.json file in the given directory", metadataFilename);
            }

            metadata = JsonConvert.DeserializeObject<ModpackMetadata>(fileSystem.ReadAllText(metadataFilename));
            mods = metadata.Mods.Select(m => new Mod(m, directory, fileSystem)).ToList();
        }

        private readonly Stream? zipStream;
        private readonly ZipArchive? zipArchive;

        private ModpackMetadata? metadata;
        private List<Mod>? mods;

        public async Task Apply(SkyEditorScriptContext context)
        {
            if (mods == null)
            {
                throw new InvalidOperationException("Failed to initialize mods prior to applying them");
            }

            foreach (var mod in mods)
            {
                if (mod.Enabled)
                {
                    await mod.Apply(context).ConfigureAwait(false);
                }
            }
        }

        public void Dispose()
        {
            zipArchive?.Dispose();
            zipStream?.Dispose();
        }
    }
}
