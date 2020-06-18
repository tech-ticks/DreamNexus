using Avalonia;
using Avalonia.Controls;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Avalonia.Infrastructure;
using SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.MenuItems
{
    public class ApplyModMenuItem : MenuItem<RtdxRomViewModel>
    {
        public ApplyModMenuItem(MainWindowViewModel mainWindowViewModel) : base(mainWindowViewModel)
        {
        }

        protected override async Task Execute(RtdxRomViewModel viewModel)
        {
            var dialog = new OpenFileDialog
            {
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = Properties.Resources.FileDialogFilter_ModpackFiles, Extensions = new List<string> { "zip" }},
                    new FileDialogFilter { Name = Properties.Resources.FileDialogFilter_CsFiles, Extensions = new List<string> { "csx" }},
                    new FileDialogFilter { Name = Properties.Resources.FileDialogFilter_LuaFiles, Extensions = new List<string> { "lua" }},
                }
            };

            var paths = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            var firstPath = paths.FirstOrDefault();
            if (!string.IsNullOrEmpty(firstPath))
            {
                var modpack = new Modpack(firstPath, PhysicalFileSystem.Instance);
                await modpack.Apply<IRtdxRom>(viewModel.Model);
                viewModel.ReloadFromModel();
            }
        }
    }
}
