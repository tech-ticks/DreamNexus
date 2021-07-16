using System.Linq;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;

namespace SkyEditorUI.Infrastructure
{
    public static class ModpackExtensions
    {
        public static Mod? GetDefaultMod(this Modpack modpack)
        {
            if (modpack == null || modpack.Mods == null) 
            {
                return null;
            }

            var defaultMod = modpack.Mods
                .FirstOrDefault(mod => mod.Metadata.Id == $"{modpack.Metadata.Id}.default");
            if (defaultMod == null && modpack.Mods.Count == 1)
            {
                defaultMod = modpack.Mods.FirstOrDefault();
            }

            return defaultMod;
        }
    }
}
