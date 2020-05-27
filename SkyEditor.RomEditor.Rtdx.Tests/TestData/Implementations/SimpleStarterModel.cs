using SkyEditor.RomEditor.Rtdx.Domain.Models;
using SkyEditor.RomEditor.Rtdx.Reverse;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Tests.TestData.Implementations
{
    public class SimpleStarterModel : IStarterModel
    {
        public Reverse.Const.creature.Index PokemonId { get; set; }

        public string PokemonName { get; set; } = default!;

        public string? NatureDiagnosisMaleModelSymbol { get; set; }
        public string? NatureDiagnosisFemaleModelSymbol { get; set; }
        public Reverse.Const.waza.Index Move1 { get; set; }

        public string Move1Name { get; set; } = default!;

        public Reverse.Const.waza.Index Move2 { get; set; }

        public string Move2Name { get; set; } = default!;

        public Reverse.Const.waza.Index Move3 { get; set; }

        public string Move3Name { get; set; } = default!;

        public Reverse.Const.waza.Index Move4 { get; set; }

        public string Move4Name { get; set; } = default!;

        public NDConverterSharedData.NatureType? MaleNature { get; set; }
        public NDConverterSharedData.NatureType? FemaleNature { get; set; }

        public IStarterModel Clone()
        {
            return (IStarterModel)MemberwiseClone();
        }
    }
}
