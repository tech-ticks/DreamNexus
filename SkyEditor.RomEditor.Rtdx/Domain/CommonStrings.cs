using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Reverse.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using CreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;
using WazaIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.waza.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain
{
    public interface ICommonStrings
    {
        Dictionary<int, string> Pokemon { get; set; }
        Dictionary<int, string> Moves { get; set; }
    }

    public class CommonStrings : ICommonStrings
    {
        public CommonStrings(MessageBinEntry common)
        {
            var textIdValues = Enum.GetValues(typeof(TextIDHash)).Cast<TextIDHash>().ToDictionary(h => h.ToString("f"), h => (int)h);

            Pokemon = new Dictionary<int, string>();
            var creatures = Enum.GetValues(typeof(CreatureIndex)).Cast<CreatureIndex>().ToArray();
            foreach (CreatureIndex creature in creatures)
            {
                if (creature == default)
                {
                    continue;
                }

                var nameHash = textIdValues.GetValueOrDefault("POKEMON_NAME__POKEMON_" + creature.ToString("f"));
                var name = common.Strings.GetValueOrDefault(nameHash);
                Pokemon.Add((int)creature, name ?? "");
            }

            Moves = new Dictionary<int, string>();
            var moves = Enum.GetValues(typeof(WazaIndex)).Cast<WazaIndex>().ToArray();
            foreach (WazaIndex waza in moves)
            {
                if (waza == default)
                {
                    continue;
                }

                var nameHash = textIdValues.GetValueOrDefault("WAZA_NAME__WAZA_" + waza.ToString("f"));
                var name = common.Strings.GetValueOrDefault(nameHash);
                Moves.Add((int)waza, name ?? "");
            }
        }

        public Dictionary<int, string> Pokemon { get; set; }
        public Dictionary<int, string> Moves { get; set; }
    }
}
