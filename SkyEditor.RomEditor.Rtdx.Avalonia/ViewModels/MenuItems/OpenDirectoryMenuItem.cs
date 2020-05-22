using Avalonia;
using Avalonia.Controls;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Rtdx.Avalonia.Infrastructure;
using SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.Rtdx;
using SkyEditor.RomEditor.Rtdx.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.MenuItems
{
    public class OpenDirectoryMenuItem : MenuItem
    {
        public OpenDirectoryMenuItem(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
        }

        private readonly MainWindowViewModel mainWindowViewModel;

        protected override async Task Execute()
        {
            var dialog = new OpenFolderDialog();
            var path = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            if (!string.IsNullOrEmpty(path))
            {
                if (mainWindowViewModel.OpenFiles.Any())
                {
                    // Let's keep the UI simple for now
                    mainWindowViewModel.CloseAllFiles();
                }

                var rom = new RtdxRom(path, PhysicalFileSystem.Instance);
                mainWindowViewModel.OpenFile(new RtdxRomViewModel(rom));
            }
        }
    }
}
