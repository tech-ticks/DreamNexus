using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Domain.Rtdx;
using System;
using System.IO;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx
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

        /// <summary>
        /// Saves the model to the given directory
        /// </summary>
        public void Save(string targetDirectory)
        {
            Model.Save(targetDirectory, PhysicalFileSystem.Instance);
        }

        /// <summary>
        /// Saves the model to the source directory
        /// </summary>
        public void Save()
        {
            Model.Save();
        }
    }
}
