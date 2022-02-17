using System;
using System.IO;
using System.Linq;
using LibHac.Common;
using LibHac.Common.Keys;
using LibHac.Fs;
using LibHac.FsSystem;
using LibHac.Tools.Fs;
using LibHac.Tools.FsSystem;
using LibHac.Tools.FsSystem.NcaUtils;
using Path = System.IO.Path;

namespace SkyEditorUI.Infrastructure
{
    public static class RomExtraction
    {

        public static string UnpackRom(string romPath, string keysPath, Action<string> onProgress)
        {
            bool isNsp = romPath.EndsWith(".nsp");
            if (!isNsp && !romPath.EndsWith(".xci"))
            {
                throw new Exception("Unsupported file type, only .nsp or .xci is supported.");
            }

            var keySet = ExternalKeyReader.ReadKeyFile(keysPath);

            onProgress("Reading ROM...");
            using (var romFile = new LocalStorage(romPath, FileAccess.Read))
            {
                PartitionFileSystem partition;
                if (isNsp)
                {
                    partition = new PartitionFileSystem(romFile);
                }
                else
                {
                    var xci = new Xci(keySet, romFile);
                    partition = xci.OpenPartition(XciPartitionType.Secure);
                }

                var biggestNcaFileEntry = partition.Files.OrderByDescending(file => file.Size).FirstOrDefault();
                if (biggestNcaFileEntry == null)
                {
                    throw new InvalidOperationException("The secure partition doesn't contain any NCAs.");
                }

                using var ncaFile = partition.OpenFile(biggestNcaFileEntry, OpenMode.Read).AsStorage();
                var nca = new Nca(keySet, ncaFile);

                var targetDirectory = romPath.Replace(".xci", "").Replace(".nsp", "");
                Directory.CreateDirectory(targetDirectory);

                using (var exefs = nca.OpenFileSystem(NcaSectionType.Code, IntegrityCheckLevel.ErrorOnInvalid))
                {
                    onProgress("Extracting exefs...");
                    var exefsOutDirectory = Path.Combine(targetDirectory, "exefs");
                    Directory.CreateDirectory(exefsOutDirectory);
                    exefs.Extract(exefsOutDirectory);
                }
                using (var romfs = nca.OpenFileSystem(NcaSectionType.Data, IntegrityCheckLevel.ErrorOnInvalid))
                {
                    onProgress("Extracting romfs...");
                    var romfsOutDirectory = Path.Combine(targetDirectory, "romfs");
                    Directory.CreateDirectory(romfsOutDirectory);
                    int entryCount = romfs.GetEntryCountRecursive("/", LibHac.Fs.Fsa.OpenDirectoryMode.File);
                    if (entryCount == 0)
                    {
                        throw new Exception("The romfs is empty!");
                    }
                    romfs.Extract(romfsOutDirectory, new ProgressReporter(onProgress, entryCount));
                } 

                return targetDirectory;
            }
        }

        private class ProgressReporter : IProgressReport
        {
            private int fileCount = 0;
            private int currentFileNum = 0;

            private Action<string> onProgress;

            public ProgressReporter(Action<string> onProgress, int fileCount)
            {
                this.onProgress = onProgress;
                this.fileCount = fileCount;
            }

            public void SetTotal(long value)
            {
                // Called whenever the library starts to extract a new file, the message is the file name
                int percentage = (int) ((float) currentFileNum / fileCount * 100);
                onProgress($"Extracting romfs ({percentage}%)...");
                currentFileNum++;
            }

            public void LogMessage(string message)
            {
            }

            public void Report(long value)
            {
            }

            public void ReportAdd(long value)
            {
            }
        }
    }
}
