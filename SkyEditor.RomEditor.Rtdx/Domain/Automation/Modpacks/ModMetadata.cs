using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Automation.Modpacks
{
    public class ModMetadata
    {
        /// <summary>
        /// Name of the mod
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of the mod
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Whether the mod should be applied by default. The user can override this value.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Ordered list of paths of scripts to run, relative to the modpack.json file
        /// </summary>
        public List<string>? Scripts { get; set; }
    }
}
