using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using SkyEditor.RomEditor.Avalonia.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx.Tutorial
{
    public class CustomizeRomViewModel : OpenFileViewModel
    {
        public CustomizeRomViewModel(MainWindowViewModel mainWindow, RtdxRomViewModel viewModel)
        {
            this.mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            this.RomViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            SaveAtmosphereLayeredFsCommand = ReactiveCommand.CreateFromTask(() => SaveAtmosphereLayeredFs());
        }

        private readonly MainWindowViewModel mainWindow;

        public RtdxRomViewModel RomViewModel { get; }

        public ReactiveCommand<Unit, Unit> SaveAtmosphereLayeredFsCommand { get; }

        public override string Name => Properties.Resources.ViewModels_Rtdx_Tutorial_CustomizeRomViewModel_Name;

        private async Task SaveAtmosphereLayeredFs()
        {
            var dialog = new OpenFolderDialog
            {
                Title = Properties.Resources.ViewModels_Rtdx_Tutorial_CustomizeRomViewModel_LayeredFsOpenFolderDialogTitle
            };

            var path = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            if (!string.IsNullOrEmpty(path))
            {
                var fullPath = Path.Combine(path, "atmosphere", "contents", "01003D200BAA2000");
                RomViewModel.Save(fullPath);
                mainWindow.OpenFile(new FinishedViewModel(this.mainWindow), true);
            }
        }
    }
}
