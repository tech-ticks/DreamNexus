using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Avalonia.ViewModels
{
    public abstract class OpenFileViewModel : ViewModelBase
    {
        public abstract string Name { get; }
    }
}
