using Newtonsoft.Json;
using SkyEditor.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Infrastructure.Automation.Modpacks
{
    public class ModpackBuilder
    {
        public ModpackBuilder()
        {
            mods = new List<Mod>();
            Metadata = new ModpackMetadata();
        }

        private readonly List<Mod> mods;

        public ModpackMetadata Metadata { get; }

        /// <summary>
        /// Adds a mod to the modpack
        /// </summary>
        /// <param name="mod">Mod to add to the modpack</param>
        /// <param name="enabled">Whether the mod should be enabled</param>
        public ModpackBuilder AddMod(Mod mod, bool enabled)
        {
            if (string.IsNullOrWhiteSpace(mod.Metadata.Id))
            {
                throw new ArgumentException("Mod must have an ID in the metadata", nameof(mod));
            }
            if (mods.Any(m => string.Equals(m.Metadata.Id, mod.Metadata.Id, StringComparison.OrdinalIgnoreCase))) 
            {
                throw new ArgumentException($"Mod with the ID '{Metadata.Id}' is already present in the modpack", nameof(mod));
            }

            mod.Enabled = enabled;
            mods.Add(mod);
            return this;
        }

        /// <summary>
        /// Builds the modpack and saves it as a zip file to the given path
        /// </summary>
        /// <param name="filename"></param>
        public async Task Build(string filename)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), "SkyEditorModpackBuilder-" + Guid.NewGuid().ToString());
            try
            {
                Directory.CreateDirectory(tempDirectory);

                var modMetadata = new List<ModMetadata>();
                var modIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var i = 0;
                foreach (var mod in mods)
                {
                    var modId = mod.Metadata.Id;
                    if (string.IsNullOrWhiteSpace(modId))
                    {
                        throw new InvalidOperationException($"Mod at index {i} does not have an ID");
                    }
                    if (modIds.Contains(modId!))
                    {
                        throw new InvalidOperationException($"Mod with ID '{modId}' already exists, so the mod at index {i} cannot be added");
                    }
                    modIds.Add(modId!);

                    var modDirectory = Path.Combine(tempDirectory, modId);
                    Directory.CreateDirectory(modDirectory);
                    await mod.CopyToDirectory(modDirectory, PhysicalFileSystem.Instance).ConfigureAwait(false);

                    mod.Metadata.BaseDirectory = modId;
                    modMetadata.Add(mod.Metadata);

                    i += 1;
                }

                this.Metadata.Mods = modMetadata;
                File.WriteAllText(Path.Combine(tempDirectory, "modpack.json"), JsonConvert.SerializeObject(this.Metadata));
                ZipFile.CreateFromDirectory(tempDirectory, filename);
            }
            finally
            {
                if (Directory.Exists(tempDirectory))
                {
                    Directory.Delete(tempDirectory, true);
                }
            }
        }
    }
}
