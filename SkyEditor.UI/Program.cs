using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Gtk;
using GtkSource;
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
            Application.Init();

            // Setup theme
            if (!OperatingSystem.IsLinux())
            {
                Settings.Default.ThemeName = IsDarkTheme() ? "Arc-Dark" : "Arc";
            }

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
            var app = new Application("DreamNexus", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            GLib.ExceptionManager.UnhandledException += (args) => 
            {
                Console.WriteLine(args.ExceptionObject?.ToString());
                UIUtils.ShowErrorDialog(win, "An error occured", args.ExceptionObject?.ToString() ?? "Unknown exception");
                args.ExitApplication = false;
            };

            win.Show();
            Application.Run();
        }
        public static bool IsDarkTheme()
        {
            // TODO: re-enable once the code editor's dark theme has been fixed
            return false;
            /*try
            {
                if (OperatingSystem.IsWindows())
                {
                    // TODO: figure out how to do this on Windows
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
                        return false;
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

            return false;*/
        }
    }
}
