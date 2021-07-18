using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;

namespace SkyEditorUI.Infrastructure
{
  public static class CodeInjectionHelpers
  {
    public static bool HasDownloadedBinaries(string dataDirectory)
    {
      var localDirectory = Path.Combine(dataDirectory, "hyperbeam");
      if (!Directory.Exists(localDirectory))
      {
        return false;
      }

      return Directory.EnumerateDirectories(localDirectory).Any();
    }

    public static string[] GetAvailableVersions(string dataDirectory)
    {
      if (!HasDownloadedBinaries(dataDirectory))
      {
        return new string[] {};
      }

      var localDirectory = Path.Combine(dataDirectory, "hyperbeam");
      return Directory.EnumerateDirectories(localDirectory).Select(path => Path.GetFileName(path)).ToArray();
    }

    public static string? GetDirectoryForVersion(string dataDirectory, string? version, string releaseType)
    {
      if (string.IsNullOrWhiteSpace(version))
      {
        return null;
      }

      var directory = Path.Combine(dataDirectory, "hyperbeam", version, releaseType);
      if (Directory.Exists(directory))
      {
        return directory;
      }

      return null;
    }

    public static void DownloadBinaries(string repository, string dataDirectory)
    {
      using var client = new WebClient();
      client.Headers.Add("User-Agent", "request");

      Console.WriteLine($"Checking releases in {repository}...");
      string releasesString = client.DownloadString($"https://api.github.com/repos/{repository}/releases");
      var releases = JArray.Parse(releasesString);

      var localDirectory = Path.Combine(dataDirectory, "hyperbeam");
      if (!Directory.Exists(localDirectory))
      {
        Directory.CreateDirectory(localDirectory);
      }

      foreach (var release in releases.Select(release => release.Value<JObject>()))
      {
        if (release == null)
        {
          Console.WriteLine("GitHub API returned a null release!");
          continue;
        }
        var tagName = release["tag_name"]?.Value<string>();
        var assetsArray = release["assets"]?.Value<JArray>();

        string? getAssetDownload(string type)
        {
          return assetsArray?
            .FirstOrDefault(asset => asset.Value<JObject>()?["name"]?.Value<string>()?.EndsWith($"{type}.zip") ?? false)?
              ["browser_download_url"]?.Value<string>();
        }
        var debugDownload = getAssetDownload("debug");
        var releaseDownload = getAssetDownload("release");

        if (string.IsNullOrWhiteSpace(tagName) || string.IsNullOrWhiteSpace(debugDownload))
        {
          return;
        }

        string targetFolder = Path.Combine(localDirectory, tagName);
        if (Directory.Exists(targetFolder))
        {
          Directory.Delete(targetFolder, true);
        }

        Directory.CreateDirectory(targetFolder);
        DownloadBinaryVersion(client, debugDownload, targetFolder, "debug");

        if (releaseDownload != null)
        {
          DownloadBinaryVersion(client, releaseDownload, targetFolder, "release");
        }
      }
    }

    public static void DownloadBinaryVersion(WebClient client, string zipUrl, string targetFolder, string releaseType)
    {
      var downloadDir = Path.Combine(targetFolder, releaseType);
      Directory.CreateDirectory(downloadDir);

      var fileName = Path.GetTempFileName();
      try
      {
        Console.WriteLine($"Downloading {zipUrl}...");
        client.DownloadFile(zipUrl, fileName);
        ZipFile.ExtractToDirectory(fileName, downloadDir);
      }
      finally
      {
        File.Delete(fileName);
      }
    }
  }
}
