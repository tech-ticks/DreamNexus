using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Gtk;
using Settings = Gtk.Settings;
using SkyEditorUI.Controllers;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (OperatingSystem.IsMacOS())
            {
                FixMacOSWorkingDirectory();
            }

            Application.Init();
            UpdateTheme();

            // Setup CSS
            const int StyleProviderPriorityApplication = 600;
            var cssProvider = new CssProvider();

            var cssStream = Assembly.GetCallingAssembly().GetManifestResourceStream("skytemple.css");
            using (var streamReader = new StreamReader(cssStream!))
            {
                cssProvider.LoadFromData(streamReader.ReadToEnd());
            }

            StyleContext.AddProviderForScreen(Gdk.Screen.Default, cssProvider, StyleProviderPriorityApplication);

            // Setup icons
            IconTheme.Default.AppendSearchPath("Assets/Icons");
            IconTheme.Default.AppendSearchPath("Assets/External/skytemple-icons/skytemple_icons");
            IconTheme.Default.RescanIfNeeded();

            // Setup application and main window
            var app = new Application("SkyEditor.UI", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            GLib.ExceptionManager.UnhandledException += (args) => 
            {
                Console.WriteLine(args.ExceptionObject?.ToString());
                UIUtils.ShowErrorDialog(win, "An error occured", args.ExceptionObject?.ToString() ?? "Unknown exception");
                args.ExitApplication = false;
            };

            // Update the theme every eight seconds
            GLib.Timeout.AddSeconds(8, UpdateTheme);

            win.Show();
            Application.Run();
        }

        public static event Action<bool>? OnThemeUpdated; 

        public static bool UpdateTheme()
        {
            // Setup theme
            if (!OperatingSystem.IsLinux())
            {
                bool dark = IsDarkTheme();
                Settings.Default.ThemeName = dark ? "Arc-Dark" : "Arc";
                OnThemeUpdated?.Invoke(dark);
            }
            return true; // Continue the timer
        }

        public static bool IsDarkTheme()
        {
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    return true;
                }
                else if (OperatingSystem.IsMacOS())
                {
                    var proc = Process.Start(new ProcessStartInfo
                    {
                        FileName = "defaults",
                        Arguments = "read -g AppleInterfaceStyle",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    });

                    if (proc == null)
                    {
                        return true;
                    }

                    var theme = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    return theme.Trim() == "Dark";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to get system theme: " + e);
            }

            return true;
        }

        private static void FixMacOSWorkingDirectory()
        {
            // The working directory isn't set automatically if we're running in a mac .app
            if (Directory.GetCurrentDirectory() == "/")
            {
                var executablePath = Process.GetCurrentProcess()?.MainModule?.FileName;
                if (executablePath != null)
                {
                    var executableDirectory = Path.GetDirectoryName(executablePath);
                    if (executableDirectory != null)
                    {
                        Directory.SetCurrentDirectory(executableDirectory);
                    }
                }
            }
        }
    }
}
