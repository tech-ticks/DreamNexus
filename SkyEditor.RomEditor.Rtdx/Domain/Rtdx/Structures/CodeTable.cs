using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class RtdxCodeTable : CodeTable
    {
        private static readonly Dictionary<string, Type> StaticConstantReplacementTable = new Dictionary<string, Type>()
        {
            { "kind:", typeof(CreatureIndex) },
            { "kind_p:", typeof(CreatureIndex) },
            { "item:", typeof(ItemIndex) },
            { "item_w:", typeof(ItemIndex) },
            { "dungeon:", typeof(DungeonIndex) },
            { "dungeon_status_w:", typeof(DungeonStatusIndex) },
            { "ability:", typeof(AbilityIndex) },
            { "waza:", typeof(WazaIndex) },
            { "status:", typeof(StatusIndex) },
            { "status_a:", typeof(StatusIndex) },
            { "sugowaza_w:", typeof(SugowazaIndex) },
            { "type:", typeof(PokemonType) },
            { "weather:", typeof(DungeonStatusIndex) },
        };

        protected override Dictionary<string, Type> ConstantReplacementTable => StaticConstantReplacementTable;

        public RtdxCodeTable()
        {
        }

        public RtdxCodeTable(byte[] data) : base(data)
        {
        }
    }
}
