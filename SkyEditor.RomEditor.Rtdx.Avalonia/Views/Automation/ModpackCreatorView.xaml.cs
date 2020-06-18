using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SkyEditor.RomEditor.Avalonia.Views.Automation
{
    public class ModpackCreatorView : UserControl
    {
        public ModpackCreatorView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
