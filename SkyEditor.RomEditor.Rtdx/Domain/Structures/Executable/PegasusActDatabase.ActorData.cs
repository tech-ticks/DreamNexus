using SkyEditor.RomEditor.Rtdx.Constants;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures.Executable
{
    public partial class PegasusActDatabase
    {
        public class ActorData
        {
            public string SymbolName { get; set; } = default!;
            public CreatureIndex PokemonIndex { get; set; }
            public PokemonFormType FormType { get; set; }
            public bool IsFemale { get; set; }
            public PegasusActorDataPartyId PartyId { get; set; }
            public PokemonFixedWarehouseId WarehouseId { get; set; } = default!;
            public TextIDHash SpecialName { get; set; } = default!;
            public string? DebugName { get; set; }
            public int PokemonIndexOffset { get; set; }
            public bool PokemonIndexEditable { get; set; }
        }
    }
}
