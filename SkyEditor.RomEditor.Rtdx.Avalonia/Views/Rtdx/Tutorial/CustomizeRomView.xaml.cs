using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SkyEditor.RomEditor.Avalonia.Views.Rtdx.Tutorial
{
    public class CustomizeRomView : UserControl
    {
        public CustomizeRomView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
