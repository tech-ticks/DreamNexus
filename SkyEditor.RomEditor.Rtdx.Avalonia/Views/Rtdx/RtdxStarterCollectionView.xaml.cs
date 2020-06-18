using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SkyEditor.RomEditor.Avalonia.Views.Rtdx
{
    public class RtdxStarterCollectionView : UserControl
    {
        public RtdxStarterCollectionView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
