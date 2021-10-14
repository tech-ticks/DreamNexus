using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SkyEditorUI.Infrastructure
{
    public static class Hactool
    {
        private static readonly object logFileLock = new();

        private static void WriteToLogFile(string line)
        {
            lock(logFileLock)
            {
                File.AppendAllLines(Path.Combine(Settings.DataPath, "hactool.log"), new[] { DateTime.Now.ToString() + ": " + line });
            }
        }

        public static string RunHactool(params string[] args)
        {
            var enableFileLogging = Settings.Load().EnableHactoolLogging;

            using var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = OperatingSystem.IsWindows() ? "hactool.exe" : "hactool",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            foreach (var arg in args)
            {
                proc.StartInfo.ArgumentList.Add(arg);
            }

            var output = new StringBuilder();
            var error = new StringBuilder();
            proc.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                output.AppendLine(e.Data);
                Console.WriteLine($"[hactool stdout {proc.Id}] {e.Data}");
                if (enableFileLogging) WriteToLogFile($"[hactool stdout {proc.Id}] {e.Data}");
            };
            proc.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                error.AppendLine(e.Data);
                Console.WriteLine($"[hactool stderr {proc.Id}] {e.Data}");
                if (enableFileLogging) WriteToLogFile($"[hactool stderr {proc.Id}] {e.Data}");
            };

            Console.WriteLine($"Running hactool with '{string.Join(" ", args)}'");
            if (enableFileLogging) WriteToLogFile($"Running hactool with '{string.Join(" ", args)}'");

            try
            {
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                if (enableFileLogging)
                {
                    try
                    {
                        WriteToLogFile("Encountered exception running hactool: " + ex.ToString());
                    }
                    catch (Exception)
                    {
                        // Bubble up the original exception, we don't want errors about logging interfering with errors from hactool
                    }
                }

                throw;
            }

            return output.ToString();
        }
    }
}
