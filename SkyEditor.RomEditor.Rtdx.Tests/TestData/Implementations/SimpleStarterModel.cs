using SkyEditor.RomEditor.Rtdx.Constants;
using SkyEditor.RomEditor.Rtdx.Domain.Models;

namespace SkyEditor.RomEditor.Rtdx.Tests.TestData.Implementations
{
    public class SimpleStarterModel : IStarterModel
    {
        public CreatureIndex PokemonId { get; set; }

        public string PokemonName { get; set; } = default!;

        public string? NatureDiagnosisMaleModelSymbol { get; set; }
        public string? NatureDiagnosisFemaleModelSymbol { get; set; }
        public WazaIndex Move1 { get; set; }

        public string Move1Name { get; set; } = default!;

        public WazaIndex Move2 { get; set; }

        public string Move2Name { get; set; } = default!;

        public WazaIndex Move3 { get; set; }

        public string Move3Name { get; set; } = default!;

        public WazaIndex Move4 { get; set; }

        public string Move4Name { get; set; } = default!;

        public NatureDiagnosisNatureType? MaleNature { get; set; }
        public NatureDiagnosisNatureType? FemaleNature { get; set; }

        public IStarterModel Clone()
        {
            return (IStarterModel)MemberwiseClone();
        }
    }
}
