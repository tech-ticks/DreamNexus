using Avalonia;
using Avalonia.Controls;
using SkyEditor.RomEditor.Domain.Automation;
using SkyEditor.RomEditor.Avalonia.Infrastructure;
using SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SkyEditor.RomEditor.Domain.Rtdx;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.MenuItems
{
    public class RunAutomationScriptMenuItem : MenuItem<RtdxRomViewModel>
    {
        public RunAutomationScriptMenuItem(MainWindowViewModel mainWindowViewModel) : base(mainWindowViewModel)
        {
        }

        protected override async Task Execute(RtdxRomViewModel viewModel)
        {
            var dialog = new OpenFileDialog
            {
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "Lua Files", Extensions = new List<string> { "lua" }}
                }
            };

            var paths = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            var firstPath = paths.FirstOrDefault();
            if (!string.IsNullOrEmpty(firstPath))
            {
                var script = File.ReadAllText(firstPath);
                var context = new ScriptHost<IRtdxRom>(viewModel.Model);
                context.ExecuteLua(script);
                viewModel.ReloadFromModel();
            }
        }
    }
}
