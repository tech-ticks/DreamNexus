using SkyEditor.RomEditor.Rtdx.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.Rtdx
{
    public class RtdxRomViewModel : OpenFileViewModel
    {
        public RtdxRomViewModel(RtdxRom model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            StartersCollection = new RtdxStarterCollectionViewModel(model.GetStarters(), model.GetCommonStrings());
        }

        protected readonly RtdxRom model;

        public override string Name => Path.GetFileName(model.RomDirectory);

        public RtdxStarterCollectionViewModel StartersCollection { get; }
    }
}
