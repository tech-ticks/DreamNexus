using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;

namespace SkyEditor.RomEditor.Avalonia.Infrastructure
{
    public static class ApplicationExtensions
    {
        public static Window? GetMainWindow(this Application application)
        {           
            return (application.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        }

        public static Window GetMainWindowOrThrow(this Application application)
        {
            return GetMainWindow(application) ?? throw new InvalidOperationException("The use of windows requires a classic desktop style application lifetime");
        }
    }
}
