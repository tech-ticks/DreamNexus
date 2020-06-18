using Avalonia;
using Avalonia.Controls;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Avalonia.Infrastructure;
using SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.MenuItems
{
    public class OpenDirectoryMenuItem : MenuItem
    {
        public OpenDirectoryMenuItem(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
        }

        private readonly MainWindowViewModel mainWindowViewModel;

        public async Task<RtdxRomViewModel?> OpenFile()
        {
            var dialog = new OpenFolderDialog();
            var path = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            if (!string.IsNullOrEmpty(path))
            {
                return new RtdxRomViewModel(new RtdxRom(path, PhysicalFileSystem.Instance));
            }

            return null;
        }

        protected override async Task Execute()
        {
            var rom = await OpenFile();
            if (rom != null)
            {
                mainWindowViewModel.OpenFile(rom, false);
            }
        }
    }
}
