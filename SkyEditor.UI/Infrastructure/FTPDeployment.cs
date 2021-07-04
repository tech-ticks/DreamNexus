using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Method = System.Net.WebRequestMethods.Ftp;

namespace SkyEditorUI.Infrastructure
{
  public static class FTPDeployment
  {
    public static void Deploy(Settings settings, string buildPath)
    {
      WalkDirectory(settings, buildPath, new DirectoryInfo(buildPath));
    }

    public static void WalkDirectory(Settings settings, string basePath, DirectoryInfo directory)
    {
      var remoteFiles = ListDirectory(settings, Path.GetRelativePath(basePath, directory.FullName));

      foreach (var info in directory.GetFileSystemInfos())
      {
        if (info is FileInfo file)
        {
          UploadFile(settings, Path.GetRelativePath(basePath, file.FullName), File.ReadAllBytes(file.FullName));
        }
        if (info is DirectoryInfo innerDir)
        {
          var relativePath = Path.GetRelativePath(basePath, innerDir.FullName);
          if (!remoteFiles.Contains(relativePath))
          {
            CreateDirectory(settings, relativePath);
            WalkDirectory(settings, basePath, innerDir);
          }
        }
      }
    }

    public static FtpWebRequest CreateRequest(Settings settings, string path, string method)
    {
      path = path.Replace("\\", "/");

      var request = (FtpWebRequest) WebRequest.Create($"ftp://{settings.SwitchIp}:{settings.SwitchFtpPort}/{path}");
      request.Timeout = 20000;
      request.Credentials = new NetworkCredential(settings.SwitchFtpUser, settings.SwitchFtpPassword);
      request.Method = method;
      return request;
    }

    public static void UploadFile(Settings settings, string fileName, byte[] data)
    {
      var request = CreateRequest(settings, fileName, Method.UploadFile);
      request.ContentLength = data.Length;
      using (var requestStream = request.GetRequestStream())
      {
        requestStream.Write(data, 0, data.Length);
      }
      using (var response = (FtpWebResponse) request.GetResponse())
      {
        Console.WriteLine($"Uploaded {fileName} ({response.StatusDescription})");
      }
    }

    public static void CreateDirectory(Settings settings, string path)
    {
      var request = CreateRequest(settings, path, Method.MakeDirectory);
      using (var response = (FtpWebResponse) request.GetResponse())
      {
        Console.WriteLine($"Created directory {path} ({response.StatusDescription})");
      }
    }

    public static string[] ListDirectory(Settings settings, string path)
    {
      var request = CreateRequest(settings, path, Method.ListDirectory);
      using (var response = request.GetResponse())
      {
        var responseStream = response.GetResponseStream();
        using (var reader = new StreamReader(responseStream))
        {
          return reader.ReadToEnd().Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        }
      }
    }
  }
}
