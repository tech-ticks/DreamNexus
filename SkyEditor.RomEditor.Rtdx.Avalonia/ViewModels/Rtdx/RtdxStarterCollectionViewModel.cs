using ReactiveUI;
using SkyEditor.RomEditor.Rtdx.Domain;
using SkyEditor.RomEditor.Rtdx.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.Rtdx
{
    public class RtdxStarterCollectionViewModel : ViewModelBase
    {
        public RtdxStarterCollectionViewModel(StarterCollection starterCollection, ICommonStrings commonStrings)
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

    }
}
