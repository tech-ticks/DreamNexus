using Avalonia;
using Avalonia.Controls;
using SkyEditor.RomEditor.Rtdx.Avalonia.Infrastructure;
using SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.Rtdx;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.MenuItems
{
    public class CreateAutomationScriptMenuItem : MenuItem<RtdxRomViewModel>
    {
        public CreateAutomationScriptMenuItem(MainWindowViewModel mainWindowViewModel) : base(mainWindowViewModel)
        {
        }

        protected override async Task Execute(RtdxRomViewModel viewModel)
        {
            var dialog = new SaveFileDialog
            {
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "Lua Files", Extensions = new List<string> { "lua" }}
                }
            };

            var path = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            if (!string.IsNullOrEmpty(path))
            {
                var script = viewModel.Model.GenerateLuaChangeScript();
                File.WriteAllText(path, script);
            }
        }
    }
}
