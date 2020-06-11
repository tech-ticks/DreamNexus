using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx.Tutorial
{
    public class IntroViewModel : OpenFileViewModel
    {
        public IntroViewModel(MainWindowViewModel mainWindow)
        {
            this.mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            OpenRomCommand = ReactiveCommand.Create(OpenRom);
        }

        private readonly MainWindowViewModel mainWindow;

        public override string Name => Properties.Resources.ViewModels_Rtdx_Tutorial_IntroViewModel_Name;

        public ReactiveCommand<Unit, Task> OpenRomCommand { get; }

        private async Task OpenRom()
        {
            var rom = await mainWindow.OpenDirectoryMenuItem.OpenFile();
            if (rom != null)
            {
                mainWindow.OpenFile(new ModpackViewModel(this.mainWindow, rom), true);
            }
        }
    }
}
