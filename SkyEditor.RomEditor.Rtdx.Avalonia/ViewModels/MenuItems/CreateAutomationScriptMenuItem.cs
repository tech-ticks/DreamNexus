using Avalonia;
using Avalonia.Controls;
using SkyEditor.RomEditor.Avalonia.Infrastructure;
using SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.MenuItems
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
                    new FileDialogFilter { Name = Properties.Resources.FileDialogFilter_CsFiles, Extensions = new List<string> { "csx" }},
                    new FileDialogFilter { Name = Properties.Resources.FileDialogFilter_LuaFiles, Extensions = new List<string> { "lua" }},
                }
            };

            var path = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            if (!string.IsNullOrEmpty(path))
            {
                string script;
                if (path.EndsWith("lua"))
                {
                    script = viewModel.Model.GenerateLuaChangeScript();
                }
                else
                {
                    script = viewModel.Model.GenerateCSharpChangeScript();
                }
                File.WriteAllText(path, script);
            }
        }
    }
}
