using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx.Tutorial
{
    public class FinishedViewModel : OpenFileViewModel
    {
        public FinishedViewModel(MainWindowViewModel mainWindow)
        {
            this.mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            RestartCommand = ReactiveCommand.Create(Restart);
        }

        private readonly MainWindowViewModel mainWindow;

        public override string Name => Properties.Resources.ViewModels_Rtdx_Tutorial_FinishedViewModel_Name;

        public ReactiveCommand<Unit, Task> RestartCommand { get; }

        private Task Restart()
        {            
            mainWindow.OpenFile(new IntroViewModel(this.mainWindow), true);
            return Task.CompletedTask;
        }
    }
}
