using SkyEditor.IO.FileSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using IYamlSerializer = YamlDotNet.Serialization.ISerializer;
using IYamlDeserializer = YamlDotNet.Serialization.IDeserializer;
using YamlSerializerBuilder = YamlDotNet.Serialization.SerializerBuilder;
using YamlDeserializerBuilder = YamlDotNet.Serialization.DeserializerBuilder;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.Utilities;
using System.Text.RegularExpressions;

namespace SkyEditor.RomEditor.Infrastructure.Automation.Modpacks
{
    public abstract class Modpack : IDisposable
    {
        public Modpack(string path, IFileSystem fileSystem)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.fileSystem = fileSystem;
            if (fileSystem.DirectoryExists(path))
            {
                directory = path;
                LoadFromFileSystem(fileSystem, directory);
            }
            else if (fileSystem.FileExists(path))
            {
                readOnly = true;

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
                    readOnly = false;
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
                                    Infrastructure.FileSystemExtensions.GetRelativePath(directory!, path)
                                }
                            }
                        }
                    };
                    mods = metadata.Mods.Select(m => new Mod(m, directory!, fileSystem)).ToList();
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

        protected static async Task SaveMetadata(ModpackMetadata metadata, string directory, IFileSystem fileSystem)
        {
            await fileSystem.WriteAllTextAsync(Path.Combine(directory, "modpack.yaml"),
                YamlSerializer.Serialize(metadata));
            
            var modpackJson = Path.Combine(directory, "modpack.json");
            if (fileSystem.FileExists(modpackJson))
            {
                // Remove modpack.json if it exists
                fileSystem.DeleteFile(modpackJson);
            }
        }

        private void LoadFromFileSystem(IReadOnlyFileSystem fileSystem, string directory)
        {
            var modpackJsonFilename = Path.Combine(directory, "modpack.json");
            var modpackYamlFilename = Path.Combine(directory, "modpack.yaml");
            var modFilename = Path.Combine(directory, "mod.json");
            if (fileSystem.FileExists(modpackYamlFilename))
            {
                metadata = YamlDeserializer.Deserialize<ModpackMetadata>(fileSystem.ReadAllText(modpackYamlFilename));
                readOnly = false;
            }
            else if (fileSystem.FileExists(modpackJsonFilename))
            {
                metadata = JsonConvert.DeserializeObject<ModpackMetadata>(fileSystem.ReadAllText(modpackJsonFilename));
                readOnly = false;
            }
            else if (fileSystem.FileExists(modFilename))
            {
                var modMetadata = JsonConvert.DeserializeObject<ModMetadata>(fileSystem.ReadAllText(modFilename));
                modMetadata.Enabled = true;
                metadata = new ModpackMetadata
                {
                    Name = modMetadata.Name,
                    Description = modMetadata.Description,
                    Mods = new List<ModMetadata>
                    {
                        modMetadata
                    }
                };

                // Mods are read-only
                readOnly = true;
            }
            else
            {
                throw new FileNotFoundException("Could not find a modpack.yaml, modpack.json or mod.json file in the given directory");
            }
            mods = metadata.Mods?.Select(m => new Mod(m, directory, fileSystem)).ToList() ?? new List<Mod>();

            if (!IsValidId(metadata.Id ?? "") && !string.IsNullOrWhiteSpace(metadata.Name))
            {
                // Attempt to generate a valid modpack ID from the name
                var generatedId = GenerateId(metadata.Name, metadata.Author);
                if (generatedId != null)
                {
                    metadata.Id = generatedId;
                }
            }
        }

        private readonly Stream? zipStream;
        private readonly ZipArchive? zipArchive;

        private ModpackMetadata metadata = new ModpackMetadata();
        private List<Mod>? mods;
        private string? directory;
        private bool readOnly;
        private IFileSystem fileSystem = PhysicalFileSystem.Instance;

        private static readonly Regex idRegex = new Regex("^[a-z0-9]+\\.[a-z0-9]+[a-z0-9.]*$");

        public static IYamlSerializer YamlSerializer { get; } = new YamlSerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(YamlDotNet.Serialization.DefaultValuesHandling.OmitNull)
            .Build();

        public static IYamlDeserializer YamlDeserializer { get; } = new YamlDeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        public ModpackMetadata Metadata => metadata;
        public string? Directory => this.directory;
        public bool ReadOnly => readOnly;

        public List<Mod>? Mods => this.mods;

        public async Task Apply<TTarget>(TTarget target) where TTarget : IModTarget
        {
            if (mods == null)
            {
                throw new InvalidOperationException("Failed to initialize mods prior to applying them");
            }

            foreach (var mod in mods)
            {
                if (mod.Enabled)
                {
                    var host = new ScriptHost<TTarget>(target, mod);
                    await mod.Apply(host).ConfigureAwait(false);
                }
            }

            await ApplyModels(target);
        }

        public async Task Save(IModTarget target)
        {
            if (directory == null) 
            {
                throw new InvalidOperationException("Can't save modpack without directory.");
            }
            if (readOnly) 
            {
                throw new InvalidOperationException("This modpack is read-only.");
            }

            // TODO: save to temp directory, then copy to avoid data corruption
            await Task.WhenAll(SaveModels(target), SaveMetadata(metadata, directory, fileSystem));
        }

        public async Task SaveTo(IModTarget target, string directory)
        {
            if (directory == this.directory && readOnly)
            {
                throw new InvalidOperationException("Cannot save to read-only modpack directory. Please select a different folder.");
            }

            this.directory = directory;
            this.readOnly = false;

            await Save(target);
        }

        public void Dispose()
        {
            zipArchive?.Dispose();
            zipStream?.Dispose();
        }

        protected abstract Task ApplyModels(IModTarget target);
        protected abstract Task SaveModels(IModTarget target);

        public static bool IsValidId(string id)
        {
            return idRegex.IsMatch(id);
        }

        public static string? GenerateId(string? name, string? author)
        {
            name = name ?? "unknownmod";
            author = author ?? "unknownauthor";

            var charRegex = new Regex(@"[^a-z0-9.]+");
            string sanitize(string input) =>
                charRegex!.Replace(input.Trim().Replace("é", "e").ToLower(), "").Trim('.');

            string sanitizedName = sanitize(name);
            string sanitizedAuthor = sanitize(author);

            var id = $"{sanitizedAuthor}.{sanitizedName}";
            return IsValidId(id) ? id : null;
        }
    }
}
