using SkyEditor.IO.FileSystem;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Infrastructure
{
    public static class FileSystemExtensions
    {
        public static void CopyDirectory(this IFileSystem sourceFileSystem, string sourceDirectory, IFileSystem targetFileSystem, string targetDirectory)
        {
            if (!sourceFileSystem.DirectoryExists(sourceDirectory))
            {
                throw new DirectoryNotFoundException($"Could not find source directory at '{sourceDirectory}'");
            }

            if (!targetFileSystem.DirectoryExists(targetDirectory))
            {
                targetFileSystem.CreateDirectory(targetDirectory);
            }

            foreach (var file in sourceFileSystem.GetFiles(sourceDirectory, "*", topDirectoryOnly: false))
            {
                var dest = Path.Combine(targetDirectory, GetRelativePath(sourceDirectory, file));

                var destDirectory = Path.GetDirectoryName(dest);
                if (!targetFileSystem.DirectoryExists(destDirectory))
                {
                    targetFileSystem.CreateDirectory(destDirectory);
                }

                sourceFileSystem.CopyFile(file, dest);
            }
        }

        public static async Task CopyDirectoryAsync(this IFileSystem sourceFileSystem, string sourceDirectory, IFileSystem targetFileSystem, string targetDirectory)
        {
            if (!sourceFileSystem.DirectoryExists(sourceDirectory))
            {
                throw new DirectoryNotFoundException($"Could not find source directory at '{sourceDirectory}'");
            }

            if (!targetFileSystem.DirectoryExists(targetDirectory))
            {
                targetFileSystem.CreateDirectory(targetDirectory);
            }

            using var directoryLock = new SemaphoreSlim(1);

            await Task.WhenAll(sourceFileSystem.GetFiles(sourceDirectory, "*", topDirectoryOnly: false).Select(file =>
            {
                return Task.Run(async () =>
                {
                    var dest = Path.Combine(targetDirectory, GetRelativePath(sourceDirectory, file));

                    var destDirectory = Path.GetDirectoryName(dest);
                    if (!targetFileSystem.DirectoryExists(destDirectory))
                    {
                        await directoryLock.WaitAsync().ConfigureAwait(false);
                        if (!targetFileSystem.DirectoryExists(destDirectory))
                        {
                            targetFileSystem.CreateDirectory(destDirectory);
                        }
                        directoryLock.Release();
                    }

                    sourceFileSystem.CopyFile(file, dest);
                });
            })).ConfigureAwait(false);
        }

        public static string GetRelativePath(string relativeTo, string path)
        {
#if NETSTANDARD2_0
            var relativeToUri = new Uri(relativeTo);
            var pathUri = new Uri(path);
            return pathUri.MakeRelativeUri(relativeToUri).AbsolutePath;
#else
            return Path.GetRelativePath(relativeTo, path);
#endif
        }
    }
}
