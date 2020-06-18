using ReactiveUI;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.Rtdx
{
    public class RtdxStarterCollectionViewModel : ViewModelBase
    {
        public RtdxStarterCollectionViewModel(IStarterCollection starterCollection, ICommonStrings commonStrings)
        {
            if (starterCollection == null)
            {
                throw new ArgumentNullException(nameof(starterCollection));
            }

            Starters = starterCollection.Starters.Select(s => new RtdxStarterViewModel(s, commonStrings)).ToList();
            _selectedStarter = Starters.First();
        }

        public IReadOnlyList<RtdxStarterViewModel> Starters { get; }

        public RtdxStarterViewModel SelectedStarter
        {
            get => _selectedStarter;
            set
            {
                _selectedStarter = value;
                this.RaisePropertyChanged(nameof(SelectedStarter));
            }
        }
        private RtdxStarterViewModel _selectedStarter;

        /// <summary>
        /// Signals that properties on the model were changed and the view model should emit property changed events where appropriate
        /// </summary>
        public void ReloadFromModel()
        {
            foreach (var starter in Starters)
            {
                starter.ReloadFromModel();
            }
            SelectedStarter.ReloadFromModel();
        }
    }
}
