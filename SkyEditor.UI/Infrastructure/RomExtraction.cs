using SkyEditor.UI.Infrastructure;
using System;
using System.IO;
using System.Linq;

namespace SkyEditorUI.Infrastructure
{
    public static class RomExtraction
    {
        private static string ExtractXci(string romPath, string keysPath, string outPath)
        {
            return Hactool.RunHactool("-k", keysPath, "-t", "xci", "--securedir", outPath, romPath);
        }

        private static string ExtractNsp(string romPath, string keysPath, string outPath)
        {
            return Hactool.RunHactool("-k", keysPath, "-t", "pfs0", "--pfs0dir", outPath, romPath);
        }

        private static string ExtractNca(string ncaFilePath, string keysPath, string outPath)
        {
            string exefsDir = Path.Combine(outPath, "exefs");
            string romfsDir = Path.Combine(outPath, "romfs");
            return Hactool.RunHactool("-k", keysPath, "-t", "nca", "--exefsdir", exefsDir, "--romfsdir", romfsDir, ncaFilePath);
        }

        public static string UnpackRom(string romPath, string keysPath, Action<string> onProgress)
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            Console.WriteLine($"Extracting to {tempDir}");

            string outPath = Path.Combine(tempDir, "out");
            var secureDir = Directory.CreateDirectory(outPath);

            bool isNsp = romPath.EndsWith(".nsp");
            if (!isNsp && !romPath.EndsWith(".xci"))
            {
                throw new Exception("Unsupported file type, only .nsp or .xci is supported.");
            }

            try
            {
                if (isNsp)
                {
                    onProgress("Unpacking NSP (1/2)...");
                    ExtractNsp(romPath, keysPath, outPath);
                }
                else
                {
                    onProgress("Unpacking XCI (1/2)...");
                    ExtractXci(romPath, keysPath, outPath);
                }

                // We assume that the largest file is the NCA we're interested in
                var ncaFile = secureDir.EnumerateFiles()
                    .Where(file => file.Extension == ".nca")
                    .OrderByDescending(file => file.Length)
                    .FirstOrDefault();

                if (ncaFile == null)
                {
                    throw new InvalidOperationException("No NCA file found");
                }

                var targetDirectory = romPath.Replace(".xci", "").Replace(".nsp", "");
                Directory.CreateDirectory(targetDirectory);
                
                onProgress("Unpacking NCA (2/2)...");
                ExtractNca(ncaFile.FullName, keysPath, targetDirectory);

                return targetDirectory;
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}
