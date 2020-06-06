using System.IO;

namespace SkyEditor.RomEditor.Domain.Automation.Modpacks
{
    /// <summary>
    /// Access layer for scripts to interact with a mod
    /// </summary>
    public interface IScriptModAccessor
    {
        /// <summary>
        /// Reads a file from the mod
        /// </summary>
        /// <param name="resourcePath">Path of the resource file, relative to the directory in which the mod.json or the modpack.json is located.</param>
        /// <returns>A stream allowing access to the resource data</returns>
        Stream ReadResourceStream(string resourcePath);

        /// <summary>
        /// Reads a file from the mod
        /// </summary>
        /// <param name="resourcePath">Path of the resource file, relative to the directory in which the mod.json or the modpack.json is located.</param>
        /// <returns>An array of byte containing the resource data</returns>
        byte[] ReadResourceArray(string resourcePath);

        /// <summary>
        /// Reads a file from the mod
        /// </summary>
        /// <param name="resourcePath">Path of the resource file, relative to the directory in which the mod.json or the modpack.json is located.</param>
        /// <returns>A string containing the resource data</returns>
        string ReadResourceText(string resourcePath);
    }
}
