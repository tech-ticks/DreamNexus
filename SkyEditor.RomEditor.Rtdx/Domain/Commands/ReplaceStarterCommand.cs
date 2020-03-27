using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Commands
{
    public class ReplaceStarterCommand
    {
        public Reverse.Const.creature.Index OldPokemonId { get; set; }
        public Reverse.Const.creature.Index NewPokemonId { get; set; }
        public Reverse.Const.waza.Index Move1 { get; set; }
        public Reverse.Const.waza.Index Move2 { get; set; }
        public Reverse.Const.waza.Index Move3 { get; set; }
        public Reverse.Const.waza.Index Move4 { get; set; }
    }
}
