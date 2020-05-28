using Autofac.Features.Metadata;
using Newtonsoft.Json;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Rtdx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Automation.Modpacks
{
    public class Modpack
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
                var metadataFilename = Path.Combine(path, "modpack.json");
                if (!File.Exists(metadataFilename))
                {
                    throw new FileNotFoundException("Could not find a modpack.json file in the given directory", metadataFilename);
                }

                metadata = JsonConvert.DeserializeObject<ModpackMetadata>(fileSystem.ReadAllText(metadataFilename));
            }
            else if (fileSystem.FileExists(path))
            {
                directory = Path.GetDirectoryName(path);
                var extension = Path.GetExtension(path);
                if (string.Equals(extension, ".zip", StringComparison.OrdinalIgnoreCase))
                {
                    throw new NotImplementedException("Reading modpacks from zip files is not yet implemented");
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
                                    FileSystemExtensions.GetRelativePath(directory, path)
                                }
                            }
                        }
                    };
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

            mods = metadata.Mods.Select(m => new Mod(m, directory, fileSystem)).ToList();
        }

        private readonly ModpackMetadata metadata;
        private readonly List<Mod> mods;

        public async Task Apply(SkyEditorScriptContext context)
        {
            foreach (var mod in mods)
            {
                if (mod.Enabled)
                {
                    await mod.Apply(context).ConfigureAwait(false);
                }
            }
        }
    }
}
