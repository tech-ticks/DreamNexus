using System;
using System.IO;
using System.Reactive;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Serilog;
using Serilog.Filters;

namespace SkyEditor.RomEditor.Avalonia
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            RxApp.DefaultExceptionHandler = Observer.Create<Exception>(HandleReactiveUiException);

            return AppBuilder.Configure<App>()
                  .UsePlatformDetect()
                  .LogToDebug()
                  .UseReactiveUI();
        }

        private static void HandleReactiveUiException(Exception e)
        {
            File.AppendAllLines("error.log", new[] { $"{DateTime.Now} [ReactiveUI] {e}" });
            // To-do: Show an error window
        }
    }
}
