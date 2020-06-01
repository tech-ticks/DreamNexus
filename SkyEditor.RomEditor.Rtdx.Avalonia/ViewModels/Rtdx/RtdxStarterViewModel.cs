using ReactiveUI;
using SkyEditor.RomEditor.Rtdx.Domain;
using SkyEditor.RomEditor.Rtdx.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static SkyEditor.RomEditor.Rtdx.Domain.Models.StarterCollection;
using CreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;
using NatureType = SkyEditor.RomEditor.Rtdx.Reverse.NDConverterSharedData.NatureType;
using WazaIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.waza.WazaIndex;

namespace SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels.Rtdx
{
    public class RtdxStarterViewModel : ViewModelBase
    {
        public RtdxStarterViewModel(IStarterModel model, ICommonStrings commonStrings)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));

            if (commonStrings == null)
            {
                throw new ArgumentNullException(nameof(commonStrings));
            }
            this.PokemonOptions = commonStrings.Pokemon.Select(kv => new ListItem<CreatureIndex>(kv.Value, kv.Key)).OrderBy(li => li.DisplayName).ThenBy(li => li.Value).ToList();
            this.MoveOptions = commonStrings.Moves.Select(kv => new ListItem<WazaIndex>(kv.Value, kv.Key)).OrderBy(li => li.DisplayName).ThenBy(li => li.Value).ToList();
            this.PokemonOptionsByValue = PokemonOptions.ToDictionary(li => li.Value, li => li);
            this.MoveOptionsByValue = MoveOptions.ToDictionary(li => li.Value, li => li);
        }

        private readonly IStarterModel model;

        public List<ListItem<CreatureIndex>> PokemonOptions { get; }
        public List<ListItem<WazaIndex>> MoveOptions { get; }
        public Dictionary<CreatureIndex, ListItem<CreatureIndex>> PokemonOptionsByValue { get; }
        public Dictionary<WazaIndex, ListItem<WazaIndex>> MoveOptionsByValue { get; }

        public ListItem<CreatureIndex> Pokemon
        {
            get => PokemonOptionsByValue[model.PokemonId];
            set
            {
                if (model.PokemonId != value.Value)
                {
                    model.PokemonId = value.Value;
                    this.RaisePropertyChanged(nameof(Pokemon));
                }
            }
        }

        public string? NatureDiagnosisMaleModelSymbol
        {
            get => model.NatureDiagnosisMaleModelSymbol;
            set
            {
                if (model.NatureDiagnosisMaleModelSymbol != value)
                {
                    model.NatureDiagnosisMaleModelSymbol = value;
                    this.RaisePropertyChanged(nameof(NatureDiagnosisMaleModelSymbol));
                }
            }
        }
        public string? NatureDiagnosisFemaleModelSymbol
        {
            get => model.NatureDiagnosisFemaleModelSymbol;
            set
            {
                if (model.NatureDiagnosisFemaleModelSymbol != value)
                {
                    model.NatureDiagnosisFemaleModelSymbol = value;
                    this.RaisePropertyChanged(nameof(NatureDiagnosisFemaleModelSymbol));
                }
            }
        }

        public ListItem<WazaIndex> Move1
        {
            get => MoveOptionsByValue[model.Move1];
            set
            {
                if (model.Move1 != value.Value)
                {
                    model.Move1 = value.Value;
                    this.RaisePropertyChanged(nameof(Move1));
                }
            }
        }
        public ListItem<WazaIndex> Move2
        {
            get => MoveOptionsByValue[model.Move2];
            set
            {
                if (model.Move2 != value.Value)
                {
                    model.Move2 = value.Value;
                    this.RaisePropertyChanged(nameof(Move2));
                }
            }
        }
        public ListItem<WazaIndex> Move3
        {
            get => MoveOptionsByValue[model.Move3];
            set
            {
                if (model.Move3 != value.Value)
                {
                    model.Move3 = value.Value;
                    this.RaisePropertyChanged(nameof(Move3));
                }
            }
        }
        public ListItem<WazaIndex> Move4
        {
            get => MoveOptionsByValue[model.Move4];
            set
            {
                if (model.Move4 != value.Value)
                {
                    model.Move4 = value.Value;
                    this.RaisePropertyChanged(nameof(Move4));
                }
            }
        }

        public NatureType? MaleNature
        {
            get => model.MaleNature;
            set
            {
                if (model.MaleNature != value)
                {
                    model.MaleNature = value;
                    this.RaisePropertyChanged(nameof(MaleNature));
                }
            }
        }
        public NatureType? FemaleNature
        {
            get => model.FemaleNature;
            set
            {
                if (model.FemaleNature != value)
                {
                    model.FemaleNature = value;
                    this.RaisePropertyChanged(nameof(FemaleNature));
                }
            }
        }

        /// <summary>
        /// Signals that properties on the model were changed and the view model should emit property changed events where appropriate
        /// </summary>
        public void ReloadFromModel()
        {
            this.RaisePropertyChanged(nameof(Pokemon));
            this.RaisePropertyChanged(nameof(NatureDiagnosisMaleModelSymbol));
            this.RaisePropertyChanged(nameof(NatureDiagnosisFemaleModelSymbol));
            this.RaisePropertyChanged(nameof(Move1));
            this.RaisePropertyChanged(nameof(Move2));
            this.RaisePropertyChanged(nameof(Move3));
            this.RaisePropertyChanged(nameof(Move4));
            this.RaisePropertyChanged(nameof(MaleNature));
            this.RaisePropertyChanged(nameof(FemaleNature));
        }
    }
}
