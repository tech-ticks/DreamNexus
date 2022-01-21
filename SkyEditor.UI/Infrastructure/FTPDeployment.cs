using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentFTP;
using SkyEditor.RomEditor.Infrastructure;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;

namespace SkyEditorUI.Infrastructure
{
    public static class FTPDeployment
    {
        public static void Deploy(Settings settings, BuildManifest manifest, string buildPath, Action<string> onProgress)
        {
            DeployAsync(settings, manifest, buildPath, onProgress).Wait();
        }

        public static async Task DeployAsync(Settings settings, BuildManifest manifest, string buildPath, Action<string> onProgress)
        {
            onProgress($"Connecting to ${settings.SwitchIp}...");

            var client = new FtpClient(settings.SwitchIp, int.Parse(settings.SwitchFtpPort ?? "3000"),
            settings.SwitchFtpUser, settings.SwitchFtpPassword);
            client.RetryAttempts = 3;

            var localManifestPaths = manifest.HashToFilename.Select(pair => Path.Combine(buildPath, pair.Value));

            onProgress("Preparing upload...");

            byte[] previousManifestBytes = await client.DownloadAsync(".dreamnexus-build.manifest.yaml", CancellationToken.None);
            string? previousManifestString = previousManifestBytes != null
                ? new UTF8Encoding(false).GetString(previousManifestBytes)
                : null;
            var previousManifest = previousManifestString != null
                ? Modpack.YamlDeserializer.Deserialize<BuildManifest>(previousManifestString)
                : null;

            // Figure out which files need to be uploaded by comparing the previous and current build manifest
            var filesToUpload = new List<string>();

            foreach (var pair in manifest.HashToFilename)
            {
                if (previousManifest != null && previousManifest.HashToFilename.TryGetValue(pair.Key, out var name))
                {
                    if (name != pair.Value)
                    {
                        filesToUpload.Add(pair.Value);
                    }
                }
                else
                {
                    filesToUpload.Add(pair.Value);
                }
            }

            var createdDirectories = new HashSet<string>();

            for (int i = 0; i < filesToUpload.Count; i++)
            {
                string? remotePath = filesToUpload[i];
                string buildRelativePath = remotePath.Substring(1); // Remove "/"
                string localPath = Path.Combine(buildPath, buildRelativePath);
                string remoteDirectory = Path.GetDirectoryName(remotePath)!;
                if (!createdDirectories.Contains(remoteDirectory))
                {
                    await client.CreateDirectoryAsync(remoteDirectory, true);
                    createdDirectories.Add(remoteDirectory);
                }

                Console.WriteLine($"Uploading {remotePath}");
                onProgress($"Uploading file {i + 1} of {filesToUpload.Count} (0%)");
                var status = await client.UploadFileAsync(localPath, remotePath, FtpRemoteExists.Overwrite, false, FtpVerify.Retry,
                    new ActionProgress(progress =>
                    {
                        onProgress($"Uploading file {i + 1} of {filesToUpload.Count} ({(int) progress.Progress}%)");
                    }));
                if (status == FtpStatus.Failed)
                {
                    throw new FtpException($"Failed to upload '{remotePath}'");
                }
            }

            if (filesToUpload.Count > 0)
            {
                onProgress($"Uploading build manifest...");
                await client.UploadFileAsync(Path.Combine(buildPath, ".dreamnexus-build.manifest.yaml"), ".dreamnexus-build.manifest.yaml");
            }
        }

        private class ActionProgress : IProgress<FtpProgress>
        {
            private Action<FtpProgress> action;

            public ActionProgress(Action<FtpProgress> action)
            {
                this.action = action;
            }

            public void Report(FtpProgress progress)
            {
                action(progress);
            }
        }
    }
}
