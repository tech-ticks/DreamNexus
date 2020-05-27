using Avalonia;
using Avalonia.Controls;
using SkyEditor.RomEditor.Rtdx.Avalonia.Infrastructure;
using SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.Rtdx;
using SkyEditor.RomEditor.Rtdx.Domain.Automation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.MenuItems
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
                var context = new SkyEditorScriptContext(viewModel.Model);
                context.ExecuteLua(script);
                viewModel.ReloadFromModel();
            }
        }
    }
}
