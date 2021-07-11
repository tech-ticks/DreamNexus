using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System.Diagnostics;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    [DebuggerDisplay("StarterModel: {PokemonName}")]
    public class StarterModel
    {
        public CreatureIndex PokemonId { get; set; }
        public string? NatureDiagnosisMaleModelSymbol { get; set; }
        public string? NatureDiagnosisFemaleModelSymbol { get; set; }
        public WazaIndex Move1 { get; set; }
        public WazaIndex Move2 { get; set; }
        public WazaIndex Move3 { get; set; }
        public WazaIndex Move4 { get; set; }

        public NatureDiagnosisNatureType? MaleNature { get; set; }
        public NatureDiagnosisNatureType? FemaleNature { get; set; }
    }
}
