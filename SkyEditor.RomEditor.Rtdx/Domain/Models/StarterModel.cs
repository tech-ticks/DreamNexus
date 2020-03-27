using SkyEditor.RomEditor.Rtdx.Reverse;
using System.Diagnostics;
using CreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;
using NatureType = SkyEditor.RomEditor.Rtdx.Reverse.NDConverterSharedData.NatureType;
using WazaIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Models
{
    [DebuggerDisplay("StarterModel: {PokemonName}")]
    public class StarterModel
    {
        public CreatureIndex PokemonId { get; set; }
        public string PokemonName { get; set; } = default!;
        public PegasusActDatabase.ActorData? NatureDiagnosisModelMale { get; set; }
        public PegasusActDatabase.ActorData? NatureDiagnosisModelFemale { get; set; }
        public WazaIndex Move1 { get; set; }
        public string Move1Name { get; set; } = default!;
        public WazaIndex Move2 { get; set; }
        public string Move2Name { get; set; } = default!;
        public WazaIndex Move3 { get; set; }
        public string Move3Name { get; set; } = default!;
        public WazaIndex Move4 { get; set; }
        public string Move4Name { get; set; } = default!;

        public NatureType? MaleNature { get; set; }
        public NatureType? FemaleNature { get; set; }
    }
}
