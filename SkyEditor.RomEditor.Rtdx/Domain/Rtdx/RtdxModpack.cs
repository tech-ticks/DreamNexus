using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

      SaveMetadata(metadata, basePath, fileSystem).Wait();

      return new RtdxModpack(basePath, fileSystem);
    }

    protected async override Task ApplyModels(IModTarget target)
    {
      var rom = (IRtdxRom) target;
      foreach (var mod in Mods ?? Enumerable.Empty<Mod>())
      {
        // TODO: create derived RtdxMod class and move this stuff there
        if (mod.ModelExists("starters.yaml"))
        {
          rom.SetStarters(await mod.LoadModel<StarterCollection>("starters.yaml"));
        }
      }
    }

    protected async override Task SaveModels(IModTarget target)
    {
      var rom = (IRtdxRom) target;

      // Models can only be automatically applied to the first mod
      var mod = Mods?.FirstOrDefault();
      if (mod != null)
      {
        var tasks = new List<Task>();

        // TODO: create derived RtdxMod class and move this stuff there
        if (rom.StartersModified)
        {
          tasks.Add(mod.SaveModel(rom.GetStarters(), "starters.yaml"));
        }

        await Task.WhenAll(tasks);
      }
    }
  }
}
