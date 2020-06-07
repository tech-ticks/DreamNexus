using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Infrastructure.Automation.Modpacks
{
    public class ModpackMetadata
    {
        /// <summary>
        /// ID of the mod. This should remain unchanged across versions, and is required to be unique across all mods.
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
        /// Mods within the modpack
        /// </summary>
        public List<ModMetadata>? Mods { get; set; }
    }
}
