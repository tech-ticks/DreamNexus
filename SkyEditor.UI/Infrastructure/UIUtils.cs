using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Gtk;

namespace SkyEditorUI.Infrastructure
{
    public static class UIUtils
    {
        public static ResponseType ShowErrorDialog(Window? parent, string title, string text)
        {
            return ShowDialog(parent, title, text, MessageType.Error);
        }

        public static ResponseType ShowWarningDialog(Window? parent, string title, string text)
        {
            return ShowDialog(parent, title, text, MessageType.Warning);
        }

        public static ResponseType ShowInfoDialog(Window? parent, string title, string text)
        {
            return ShowDialog(parent, title, text, MessageType.Info);
        }

        public static ResponseType ShowDialog(Window? parent, string title, string text, MessageType type)
        {
             var dialog = new MessageDialog(parent, DialogFlags.Modal, type, ButtonsType.Ok, false, text);

            dialog.Title = title;
            var response = (ResponseType) dialog.Run();
            dialog.Destroy();

            return response;
        }

        public static void OpenInFileBrowser(string path)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        public static void OpenInVSCode(string path, Window parent, string? gotoFile = null, int line = 0, int column = 0)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "code",
                    UseShellExecute = false,
                };
                startInfo.ArgumentList.Add(path);

                if (gotoFile != null)
                {
                    startInfo.ArgumentList.Add("--goto");
                    startInfo.ArgumentList.Add($"{gotoFile}:{line}:{column}");
                }

                Process.Start(startInfo);
            }
            catch
            {
                ShowErrorDialog(parent, "Error",
                    "Couldn't open path in VS Code. Please ensure that VS Code is installed and added to the PATH environment variable.");
            }
        }

        public static void ConsumePendingEvents()
        {
            while (Application.EventsPending())
            {
                Application.RunIteration();
            }
        }

        private static bool readVersion = false;
        private static string? cachedVersion;

        public static string GetUIVersion()
        {
            if (!readVersion)
            {
                try
                {
                    // Nightly builds have version.txt file containing the current version and commit
                    cachedVersion = File.ReadAllText("version.txt");
                }
                catch
                {
                    cachedVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
                }
            }
            readVersion = true;
            return cachedVersion ?? "dev";
        }
    }
}
