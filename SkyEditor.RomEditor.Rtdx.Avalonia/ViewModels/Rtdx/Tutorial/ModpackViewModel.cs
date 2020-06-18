using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Avalonia.Infrastructure;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using System;
using System.Collections.Generic;
using System.Linq;
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
            this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            OpenModpackCommand = ReactiveCommand.Create(OpenModpack);
            CustomizeRomCommand = ReactiveCommand.Create(CustomizeRom);
        }

        private readonly MainWindowViewModel mainWindow;
        private readonly RtdxRomViewModel viewModel;

        public override string Name => Properties.Resources.ViewModels_Rtdx_Tutorial_ModpackViewModel_Name;

        public ReactiveCommand<Unit, Task> OpenModpackCommand { get; }
        public ReactiveCommand<Unit, Task> CustomizeRomCommand { get; }

        private async Task OpenModpack()
        {
            var dialog = new OpenFileDialog
            {
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "Mod Files", Extensions = new List<string> { "zip" }},
                    new FileDialogFilter { Name = "C# Files", Extensions = new List<string> { "csx" }},
                    new FileDialogFilter { Name = "Lua Files", Extensions = new List<string> { "lua" }},
                }
            };

            var paths = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            var firstPath = paths.FirstOrDefault();
            if (!string.IsNullOrEmpty(firstPath))
            {
                var modpack = new Modpack(firstPath, PhysicalFileSystem.Instance);
                await modpack.Apply<IRtdxRom>(viewModel.Model);
                viewModel.ReloadFromModel();
                await CustomizeRom();
            }
        }

        private Task CustomizeRom()
        {
            mainWindow.OpenFile(new CustomizeRomViewModel(this.mainWindow, viewModel), true);
            return Task.CompletedTask;
        }
    }
}
