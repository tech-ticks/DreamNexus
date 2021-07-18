using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace SkyEditorUI.Infrastructure
{
  public enum BuildFileStructureType
  {
    Atmosphere,
    Emulator
  }

  public class Settings
  {

    public static string DataPath =>
      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SkyEditorDX");

    public static string SettingsFilePath => Path.Combine(DataPath, "settings.json");

    public string? SwitchIp { get; set; }
    public string? SwitchFtpPort { get; set; }
    public string? SwitchFtpUser { get; set; }
    public string? SwitchFtpPassword { get; set; }

    public string? RtdxRomPath { get; set; }
    public BuildFileStructureType BuildFileStructure { get; set; } = BuildFileStructureType.Atmosphere;

    public List<(string nameOrId, string path)> RecentModpacks { get; set; } = new List<(string nameOrId, string path)>();

    public static Settings Load()
    {
      try
      {
        return TryLoad();
      }
      catch (Exception e)
      {
        Console.WriteLine($"Failed to load settings: {e}");
        return new Settings();
      }
    }

    public static Settings TryLoad()
    {
      if (!File.Exists(SettingsFilePath))
      {
        return new Settings();
      }

      var settingsJson = File.ReadAllText(SettingsFilePath);
      var deserializedSettings = JsonConvert.DeserializeObject<Settings>(settingsJson);
      if (deserializedSettings == null)
      {
        Console.Error.WriteLine("Deserialized settings were null, this should never happen! Creating new Settings.");
        return new Settings();
      }

      return deserializedSettings;
    }

    public void Save()
    {
      if (!Directory.Exists(DataPath))
      {
        Directory.CreateDirectory(DataPath);
      }

      var serialized = JsonConvert.SerializeObject(this, Formatting.Indented);
      if (serialized != null)
      {
        File.WriteAllText(SettingsFilePath, serialized);
      }
    }

    public void AddRecentModpack(string nameOrId, string path)
    {
      if (!RecentModpacks.Any(modpack => modpack.path == path))
      {
        RecentModpacks.Insert(0, (nameOrId, path));
      }
    }

    public bool FtpSettingsComplete() => !string.IsNullOrWhiteSpace(SwitchIp)
      && !string.IsNullOrWhiteSpace(SwitchFtpPort)
      && !string.IsNullOrWhiteSpace(SwitchFtpUser)
      && !string.IsNullOrWhiteSpace(SwitchFtpPassword);
  }
}
