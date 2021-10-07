using System.Collections.Generic;

namespace SkyEditor.RomEditor.Infrastructure.Automation.Modpacks
{
    public class ModpackMetadata
    {
        public const string DefaultCodeInjectionRepository = "tech-ticks/hyperbeam";

        /// <summary>
        /// ID of the modpack. This should remain unchanged across versions, and is required to be unique across all modpacks.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Version of the modpack, preferably a SemVer (e.g. 1.0.0, 2.0.5-hotfix2, etc)
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// Which ROM this mod targets. Supported values: "RTDX" and "PSMD".
        /// </summary>
        public string? Target { get; set; }

        /// <summary>
        /// Name of the modpack
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of the modpack
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Author of the mod
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// The version of DreamNexus this modpack was last edited with
        /// </summary>
        public string EditorVersion { get; set; } = "0.0.0";

        /// <summary>
        /// Mods within the modpack
        /// </summary>
        public List<ModMetadata>? Mods { get; set; }

        /// <summary>
        /// Whether to enable code injection and generate custom file formats for the mod.
        /// This is only used in the GUI. On the CLI, pass the --enable-custom-files flag instead.
        /// </summary>
        public bool EnableCodeInjection { get; set; }

        /// <summary>
        /// The GitHub repository that should be used to download code injection binaries from
        /// </summary>
        public string? CodeInjectionRepository { get; set; } = DefaultCodeInjectionRepository;

        /// <summary>
        /// The version of the code injection binary
        /// </summary>
        public string? CodeInjectionVersion { get; set; }

        /// <summary>
        /// "debug" or "release"
        /// </summary>
        public string? CodeInjectionReleaseType { get; set; } = "debug";
    }
}
