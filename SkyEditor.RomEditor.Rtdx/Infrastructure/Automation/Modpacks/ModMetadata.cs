using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace SkyEditor.RomEditor.Infrastructure.Automation.Modpacks
{
    public class ModMetadata
    {
        /// <summary>
        /// ID of the mod. This should remain unchanged across versions, and is required to be unique across all mods.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Version of the mod, preferably a SemVer (e.g. 1.0.0, 2.0.5-hotfix2, etc)
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// Which ROM this mod targets. Supported values: "RTDX" and "PSMD".
        /// </summary>
        public string? Target { get; set; }

        /// <summary>
        /// User-friendly name of the mod
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of the mod
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Author of the mod
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// Whether the mod should be applied by default. The user can override this value.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Path relative to the modpack.json or mod.json containing scripts and resources
        /// </summary>
        public string? BaseDirectory { get; set; }

        /// <summary>
        /// Ordered list of paths of scripts to run, relative to the modpack.json file
        /// </summary>
        public List<string>? Scripts { get; set; }
    }
}
