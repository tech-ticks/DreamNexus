using SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.MenuItems
{
    public class SaveMenuItem : MenuItem<RtdxRomViewModel>
    {
        public SaveMenuItem(MainWindowViewModel mainWindowViewModel) : base(mainWindowViewModel)
        {
        }

        protected override Task Execute(RtdxRomViewModel viewModel)
        {            
            viewModel.Save();
            return Task.CompletedTask;
        }
    }
}
