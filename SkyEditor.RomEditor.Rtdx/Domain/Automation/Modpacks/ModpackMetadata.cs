using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Automation.Modpacks
{
    class ModpackMetadata
    {
        /// <summary>
        /// Name of the modpack
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of the modpack
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Mods within the modpack
        /// </summary>
        public List<ModMetadata>? Mods { get; set; }
    }
}
