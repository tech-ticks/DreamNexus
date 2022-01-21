using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using System;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Infrastructure.Interfaces
{
    public enum OutputStructureType
    {
        Atmosphere,
        Ryujinx
    }

    public class RomBuildSettings
    {
        public CompressionType CompressionType { get; set; }
        public OutputStructureType OutputStructureType { get; set; }
        public ModpackMetadata? ModpackMetadata { get; set; }
        public bool GenerateManifest { get; set; }
    }

    public interface ISaveableToDirectory
    {
        Task<BuildManifest?> Save(string directory, IFileSystem fileSystem, RomBuildSettings settings, Action<string>? onProgress = null);
    }
}
