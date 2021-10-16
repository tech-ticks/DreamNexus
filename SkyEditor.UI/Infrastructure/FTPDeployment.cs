using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentFTP;
using FluentFTP.Rules;

namespace SkyEditorUI.Infrastructure
{
    public static class FTPDeployment
    {
        public static void Deploy(Settings settings, string buildPath, Action<string> onProgress)
        {
            DeployAsync(settings, buildPath, onProgress).Wait();
        }

        public static async Task DeployAsync(Settings settings, string buildPath, Action<string> onProgress)
        {
            onProgress($"Connecting to ${settings.SwitchIp}...");

            FtpClient createClient()
            {
                var client = new FtpClient(settings.SwitchIp, int.Parse(settings.SwitchFtpPort ?? "3000"),
                settings.SwitchFtpUser, settings.SwitchFtpPassword);
                client.RetryAttempts = 3;
                return client;
            }

            var exefsDir = "atmosphere/contents/01003D200BAA2000/exefs";
            var romfsDir = "atmosphere/contents/01003D200BAA2000/romfs";
            var patchDir = "atmosphere/exefs_patches/hyperbeam_patch_102";
            var dungeonDir = romfsDir + "/Data/StreamingAssets/native_data/dungeon";
            var dungeonBalancePath = dungeonDir + "/dungeon_balance.bin";

            var localExefsDir = Path.Combine(buildPath, exefsDir);
            var localRomfsDir = Path.Combine(buildPath, romfsDir);
            var localDungeonBalancePath = Path.Combine(buildPath, dungeonBalancePath);
            var localPatchDir = Path.Combine(buildPath, patchDir);

            // Create multiple clients for parallel uploads
            var mainClient = createClient();

            bool dungeonBalanceExists = File.Exists(localDungeonBalancePath);
            var dungeonBalanceClient = dungeonBalanceExists ? createClient() : null;
            
            onProgress($"Preparing upload...");
            
            await mainClient.CreateDirectoryAsync(exefsDir);
            await mainClient.CreateDirectoryAsync(patchDir);
            await mainClient.CreateDirectoryAsync(dungeonDir);

            var tasks = new List<Task>();

            // exefs, patches and romfs without dungeon_balance.bin (uploaded by another task)
            bool romFsUploadFinished = false;
            int romfsFileCount = 0;
            var rules = new List<FtpRule> { new FtpFileNameRule(false, new string[] { "dungeon_balance.bin" }) };
            tasks.Add(Task.Run(async () =>
            {
                onProgress("Uploading exefs...");
                await mainClient.UploadDirectoryAsync(localExefsDir, exefsDir, FtpFolderSyncMode.Update,
                    FtpRemoteExists.Overwrite, FtpVerify.None);
                onProgress("Uploading patches...");
                await mainClient.UploadDirectoryAsync(localPatchDir, patchDir, FtpFolderSyncMode.Update,
                            FtpRemoteExists.Overwrite, FtpVerify.None);
                await mainClient.UploadDirectoryAsync(localRomfsDir, romfsDir, FtpFolderSyncMode.Update,
                    FtpRemoteExists.Overwrite, FtpVerify.None, rules, new ActionProgress(progress =>
                    {
                        int fileCount = dungeonBalanceExists ? progress.FileCount + 1 : progress.FileCount;
                        onProgress($"Uploading romfs...\nUploading file {progress.FileIndex + 1} "
                            + $"of {fileCount} ({(int) progress.Progress}%)");
                        romfsFileCount = progress.FileCount;
                    }));
                romFsUploadFinished = true;
            }));

            if (dungeonBalanceClient != null)
            {
                // Upload dungeon_balance.bin
                tasks.Add(dungeonBalanceClient.UploadFileAsync(localDungeonBalancePath, dungeonBalancePath,
                    FtpRemoteExists.Overwrite, false, FtpVerify.None, new ActionProgress(progress => {
                        // Only show progress after the other uploads have finished to avoid flickering
                        if (romFsUploadFinished)
                        {
                            onProgress($"Uploading romfs...\nUploading file {romfsFileCount + 1} "
                                + $"of {romfsFileCount + 1} ({(int) progress.Progress}%)");
                        }
                    })));
            }

            onProgress($"Uploading exefs, romfs and patches...");
            await Task.WhenAll(tasks);
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
