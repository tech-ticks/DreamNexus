using SkyEditor.RomEditor.Avalonia.ViewModels.Automation;
using System;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.MenuItems
{
    public class CreateModpackMenuItem : MenuItem
    {
        public CreateModpackMenuItem(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
        }
        private readonly MainWindowViewModel mainWindowViewModel;

        protected override Task Execute()
        {
            mainWindowViewModel.OpenFile(new ModpackCreatorViewModel(), false);
            return Task.CompletedTask;
        }
    }
}
