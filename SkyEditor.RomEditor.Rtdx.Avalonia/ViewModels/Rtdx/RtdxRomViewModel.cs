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
            this.Model = model ?? throw new ArgumentNullException(nameof(model));
            StartersCollection = new RtdxStarterCollectionViewModel(model.GetStarters(), model.GetCommonStrings());
        }

        public RtdxRom Model { get; }

        public override string Name => Path.GetFileName(Model.RomDirectory);

        public RtdxStarterCollectionViewModel StartersCollection { get; }

        /// <summary>
        /// Signals that properties on the model were changed and the view model should emit property changed events where appropriate
        /// </summary>
        public void ReloadFromModel()
        {
            StartersCollection.ReloadFromModel();
        }
    }
}
