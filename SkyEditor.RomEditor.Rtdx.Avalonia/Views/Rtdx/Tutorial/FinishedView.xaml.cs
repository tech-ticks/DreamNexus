using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SkyEditor.RomEditor.Avalonia.Views.Rtdx.Tutorial
{
    public class FinishedView : UserControl
    {
        public FinishedView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
