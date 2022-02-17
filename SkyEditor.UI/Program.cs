using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Gtk;
using Settings = Gtk.Settings;
using SkyEditorUI.Controllers;
using SkyEditorUI.Infrastructure;
using Microsoft.Win32;

namespace SkyEditorUI
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;

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
            IconTheme.Default.AppendSearchPath(Path.Combine(AppContext.BaseDirectory, "Assets/Icons"));
            IconTheme.Default.AppendSearchPath(Path.Combine(AppContext.BaseDirectory, "Assets/External/skytemple-icons/skytemple_icons"));
            IconTheme.Default.RescanIfNeeded();

            // Setup application and main window
            var app = new Application("SkyEditor.UI", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            GLib.ExceptionManager.UnhandledException += (args) => 
            {
                Console.WriteLine(args.ExceptionObject?.ToString());
                UIUtils.ShowExceptionDialog(win, "An error occured", args.ExceptionObject?.ToString() ?? "Unknown exception");
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
                    var subkey = Registry.CurrentUser.OpenSubKey(
                        @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                    var theme = subkey?.GetValue("AppsUseLightTheme", 0) as int?;

                    if (theme != null)
                    {
                        return theme == 0;
                    }
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

        private static void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var dataDirectory = SkyEditorUI.Infrastructure.Settings.DataPath;
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            string message;
            if (e.ExceptionObject is Exception ex)
            {
                message = $"{ex.Message}\nStack trace: {ex.StackTrace}\n";
            }
            else
            {
                message = e.ExceptionObject?.ToString() ?? "No exception object";
            }
            File.WriteAllText(Path.Combine(dataDirectory, $"AppDomainUnhandledException-{DateTime.Now}.txt"), message);
        }
    }
}
