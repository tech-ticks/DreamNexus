using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx.Tutorial
{
    public class ModpackViewModel : OpenFileViewModel
    {
        public ModpackViewModel(MainWindowViewModel mainWindow, RtdxRomViewModel viewModel)
        {
            this.mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            CustomizeRomCommand = ReactiveCommand.Create(CustomizeRom);
        }

        private readonly MainWindowViewModel mainWindow;

        public override string Name => Properties.Resources.ViewModels_Rtdx_Tutorial_ModpackViewModel_Name;

        public ReactiveCommand<Unit, Task> CustomizeRomCommand { get; }

        private async Task CustomizeRom()
        {
            throw new NotImplementedException();
        }
    }
}
