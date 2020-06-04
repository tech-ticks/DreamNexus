using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SkyEditor.RomEditor.Avalonia.Views.Rtdx
{
    public class RtdxStarterView : UserControl
    {
        public RtdxStarterView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
