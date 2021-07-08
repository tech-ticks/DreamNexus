using System.IO;
using IOPath = System.IO.Path;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using System.Linq;
using System;
using System.Threading.Tasks;
using SkyEditor.RomEditor.Domain.Rtdx;

namespace SkyEditorUI.Infrastructure
{

    public class SourceFile
    {

        public string OriginalPath { get; }
        public string? OverridePath { get; set; }
        public bool External { get; }
        public bool IsDirty { get; set; }
        public string Path => OverridePath ?? OriginalPath;
        public bool InProject => !External || OverridePath != null;

        public string Contents
        {
            get
            {
                if (contents == null)
                {
                    Refresh();
                }
                return contents!;
            }
            set
            {
                contents = value;
                IsDirty = true;
            }
        }
        private string? contents;

        public SourceFile(string originalPath, bool external)
        {
            OriginalPath = originalPath;
            External = external;
        }

        public void Refresh()
        {
            contents = File.ReadAllText(Path);
        }

        public void OverrideFromModpackIfExists(IRtdxRom rom, Modpack modpack)
        {
            if (!External)
            {
                throw new Exception("Can only override external files");
            }

            var modpackPath = RomPathToModpackPath(rom, modpack);
            if (File.Exists(modpackPath))
            {
                OverridePath = modpackPath;
            }
        }

        public void CopyToModpack(IRtdxRom rom, Modpack modpack)
        {
            var newPath = RomPathToModpackPath(rom, modpack);
            var directory = IOPath.GetDirectoryName(newPath);
            if (directory != null)
            {
                Directory.CreateDirectory(directory);
            }
            File.Copy(OriginalPath, newPath);

            OverridePath = newPath;
            Refresh();
        }

        public void RemoveFromModpack(Modpack modpack)
        {
            if (OverridePath != null && File.Exists(OverridePath))
            {
                File.Delete(OverridePath);
            }

            OverridePath = null;
            Refresh();
            IsDirty = false;
        }

        public async Task Save()
        {
            if (External && OverridePath == null)
            {
                throw new InvalidOperationException("Cannot save file that is not part of the project.");
            }
            await File.WriteAllTextAsync(External ? OverridePath! : OriginalPath, contents);
            IsDirty = false;
            contents = null; // Free memory of the contents string
        }

        private string RomPathToModpackPath(IRtdxRom rom, Modpack modpack)
        {
            var streamingAssetsPath = IOPath.Combine(rom.RomDirectory, "romfs", "Data", "StreamingAssets");
            var relativePath = IOPath.GetRelativePath(streamingAssetsPath, OriginalPath);

            var relativeAssetsDir = modpack.Mods?.FirstOrDefault()?.GetAssetsDirectory();
            if (relativeAssetsDir == null)
            {
                throw new InvalidOperationException("Couldn't retrieve assets directory.");
            }
            var modAssetDir = IOPath.Combine(modpack.Directory!, relativeAssetsDir);
            return IOPath.Combine(modAssetDir, relativePath);
        }
    }
}
