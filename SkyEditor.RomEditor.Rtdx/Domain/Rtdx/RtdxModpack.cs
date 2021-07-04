using System;
using System.IO;
using System.Threading.Tasks;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;

namespace SkyEditor.RomEditor.Domain.Rtdx
{
  public class RtdxModpack : Modpack
  {
    public RtdxModpack(string path, IFileSystem fileSystem) : base(path, fileSystem)
    {
    }

    public static Modpack CreateInDirectory(ModpackMetadata metadata, string directory, IFileSystem fileSystem)
    {
      void ensureDirectoryExists(string directory)
      {
        if (!fileSystem.DirectoryExists(directory))
        {
          fileSystem.CreateDirectory(directory);
        }
      }

      string basePath = Path.Combine(directory, metadata.Id ?? "NewModpack");

      if (fileSystem.FileExists(Path.Combine(basePath, "modpack.yaml"))
        || fileSystem.FileExists(Path.Combine(basePath, "modpack.json"))
        || fileSystem.FileExists(Path.Combine(basePath, "mod.json")))
      {
        throw new InvalidOperationException("Cannot create modpack in directory with an existing modpack");
      }

      ensureDirectoryExists(basePath);
      ensureDirectoryExists(Path.Combine(basePath, "Assets"));
      ensureDirectoryExists(Path.Combine(basePath, "Data"));
      ensureDirectoryExists(Path.Combine(basePath, "Scripts"));

      SaveMetadata(metadata, basePath, fileSystem).Wait();

      return new RtdxModpack(basePath, fileSystem);
    }

    protected async override Task ApplyModels(IModTarget target)
    {
      var rom = (IRtdxRom) target;

      if (ModelExists("starters.yaml"))
      {
        rom.SetStarters(await LoadModel<StarterCollection>("starters.yaml"));
      }
    }

    protected async override Task SaveModels(IModTarget target)
    {
      var rom = (IRtdxRom) target;

      if (rom.StartersModified)
      {
        await SaveModel(rom.GetStarters(), "starters.yaml");
      }
    }
  }
}
