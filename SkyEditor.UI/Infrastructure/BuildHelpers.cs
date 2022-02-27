using System.IO;
using SkyEditor.IO.FileSystem;

namespace SkyEditorUI.Infrastructure
{
  public static class BuildHelpers
  {
    public const string TitleId = "01003D200BAA2000";

    public struct AtmospherePaths
    {
      public string ContentRoot { get; set; }
      public string ExeFsPatches { get; set; }
    }

    public static AtmospherePaths CreateAtmosphereFolderStructure(Settings settings, string buildPath, IFileSystem fileSystem)
    {
      // TODO: this should eventually also be used in the console app
      string createDir(string path)
      {
        string fullPath = Path.Combine(buildPath, path);
        if (!fileSystem.DirectoryExists(fullPath))
        {
          fileSystem.CreateDirectory(fullPath);
        }
        return fullPath;
      }

      string contentRoot = createDir(Path.Combine("atmosphere", "contents", TitleId));

      return new AtmospherePaths
      {
        ContentRoot = contentRoot,
        ExeFsPatches = Path.Combine(buildPath, "atmosphere", "exefs_patches"),
      };
    }

    public static void CopyCodeInjectionBinariesForAtmosphere(AtmospherePaths paths, string codeInjectionDirectory)
    {
      var exefsPath = Path.Combine(paths.ContentRoot, "exefs");
      CopyCodeInjectionBinaries(codeInjectionDirectory, exefsPath, paths.ExeFsPatches, false);
    }

    public static void CopyCodeInjectionBinariesForEmulator(string buildPath, string codeInjectionDirectory)
    {
      var exefsPath = Path.Combine(buildPath, "exefs");
      CopyCodeInjectionBinaries(codeInjectionDirectory, exefsPath, exefsPath, true);
    }

    private static void CopyCodeInjectionBinaries(string codeInjectionDirectory, string exefsPath,
      string patchTargetPath, bool flattenPatches)
    {
      Directory.CreateDirectory(exefsPath);
      Directory.CreateDirectory(patchTargetPath);

      System.Console.WriteLine(exefsPath);
      System.Console.WriteLine(patchTargetPath);

      var subsdk1Source = Path.Combine(codeInjectionDirectory, "subsdk1");
      File.Copy(subsdk1Source, Path.Combine(exefsPath, "subsdk1"), true);

      foreach (var directory in new DirectoryInfo(codeInjectionDirectory).EnumerateDirectories())
      {
        if (!directory.Name.Contains("_patch_"))
        {
          continue;
        }

        var destDirectory = flattenPatches ? patchTargetPath : Path.Combine(patchTargetPath, directory.Name);
        Directory.CreateDirectory(destDirectory);

        foreach (var file in directory.EnumerateFiles())
        {
          file.CopyTo(Path.Combine(destDirectory, file.Name), true);
        }
      }
    }

    public static void CopyRecursively(string source, string destination)
    {
      DirectoryInfo dir = new DirectoryInfo(source);

      DirectoryInfo[] dirs = dir.GetDirectories();    
      Directory.CreateDirectory(destination);

      FileInfo[] files = dir.GetFiles();
      foreach (var file in files)
      {
        string tempPath = Path.Combine(destination, file.Name);
        file.CopyTo(tempPath, true);
      }

      // If copying subdirectories, copy them and their contents to new location.
      foreach (var subdir in dirs)
      {
        string tempPath = Path.Combine(destination, subdir.Name);
        CopyRecursively(subdir.FullName, tempPath);
      }
    }
  }
}
