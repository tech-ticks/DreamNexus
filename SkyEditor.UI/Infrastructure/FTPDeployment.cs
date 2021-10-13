using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using FluentFTP;
using FluentFTP.Helpers;
using FluentFTP.Rules;

namespace SkyEditorUI.Infrastructure
{
    public static class FTPDeployment
    {
        public static void Deploy(Settings settings, string buildPath, Action<string> onProgress)
        {
            onProgress($"Connecting to ${settings.SwitchIp}...");

            var client = new FtpClient(settings.SwitchIp, int.Parse(settings.SwitchFtpPort ?? "3000"),
                settings.SwitchFtpUser, settings.SwitchFtpPassword);
            client.RetryAttempts = 3;

            FtpTrace.LogToConsole = true;
            FtpTrace.LogUserName = false;
            FtpTrace.LogPassword = false;

            onProgress($"Preparing upload...");

            var exefsDir = "atmosphere/contents/01003D200BAA2000/exefs";
            var romfsDir = "atmosphere/contents/01003D200BAA2000/romfs";
            var patchDir = "atmosphere/exefs_patches/hyperbeam_patch_102";
            client.CreateDirectory(exefsDir);
            client.CreateDirectory(romfsDir);
            client.CreateDirectory(patchDir);

            var localExefsDir = Path.Combine(buildPath, exefsDir);
            var localRomfsDir = Path.Combine(buildPath, romfsDir);
            var localPatchDir = Path.Combine(buildPath, patchDir);

            onProgress($"Uploading exefs...");
            client.UploadDirectory(localExefsDir, exefsDir, FtpFolderSyncMode.Update, FtpRemoteExists.Overwrite,
                FtpVerify.None, null);
            onProgress($"Uploading romfs...");
            client.UploadDirectory(localRomfsDir, romfsDir, FtpFolderSyncMode.Update, FtpRemoteExists.Overwrite,
                FtpVerify.None, null, progress => onProgress("Uploading romfs...\n"
                    + $"Uploading file {progress.FileIndex + 1} of {progress.FileCount} ({(int) progress.Progress}%)"));
            onProgress($"Uploading patches...");
            client.UploadDirectory(localPatchDir, patchDir, FtpFolderSyncMode.Update, FtpRemoteExists.Overwrite,
                FtpVerify.None, null);
        }
    }
}
