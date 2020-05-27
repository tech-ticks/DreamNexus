using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Infrastructure;
using SkyEditor.RomEditor.Rtdx.Reverse.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using CreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;
using WazaIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.waza.Index;
using DungeonIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.dungeon.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain
{
    public interface ICommonStrings
    {
        Dictionary<CreatureIndex, string> Pokemon { get; }
        Dictionary<WazaIndex, string> Moves { get; }
        Dictionary<DungeonIndex, string> Dungeons { get; }

        /// <summary>
        /// Gets the name of a Pokemon by the internal Japanese name.
        /// </summary>
        /// <param name="internalName">Internal Japanese name such as "FUSHIGIDANE"</param>
        /// <returns>User-facing name such as "Bulbasaur", or null if the internal name could not be found</returns>
        string? GetPokemonNameByInternalName(string internalName);

        /// <summary>
        /// Gets the name of a move by the internal Japanese name.
        /// </summary>
        string? GetMoveNameByInternalName(string internalName);
    }

    public class CommonStrings : ICommonStrings
    {
        private static readonly Dictionary<string, int> TextIdValues = Enum.GetValues(typeof(TextIDHash)).Cast<TextIDHash>().ToDictionary(h => h.ToString("f"), h => (int)h);

        public CommonStrings(MessageBinEntry common)
        {
            this.common = common ?? throw new ArgumentNullException(nameof(common));

            Pokemon = new Dictionary<CreatureIndex, string>();
            var creatures = Enum.GetValues(typeof(CreatureIndex)).Cast<CreatureIndex>().ToArray();
            foreach (CreatureIndex creature in creatures)
            {
                if (creature == default)
                {
                    continue;
                }

                var name = GetPokemonNameByInternalName(creature.ToString("f"));
                Pokemon.Add(creature, name ?? "");
            }

            Moves = new Dictionary<WazaIndex, string>();
            var moves = Enum.GetValues(typeof(WazaIndex)).Cast<WazaIndex>().ToArray();
            foreach (WazaIndex waza in moves)
            {
                if (waza == default)
                {
                    continue;
                }

                var nameHash = TextIdValues.GetValueOrDefault("WAZA_NAME__WAZA_" + waza.ToString("f"));
                var name = common.Strings.GetValueOrDefault(nameHash);
                Moves.Add(waza, name ?? "");
            }

            Dungeons = new Dictionary<DungeonIndex, string>();
            var dungeons = Enum.GetValues(typeof(DungeonIndex)).Cast<DungeonIndex>().ToArray();
            foreach (DungeonIndex dungeon in dungeons)
            {
                /*if (dungeon == default)
                {
                    continue;
                }*/

                var name = GetDungeonNameByInternalName(dungeon.ToString("f"));
                Dungeons.Add(dungeon, name ?? "");
            }
        }

        private readonly MessageBinEntry common;

        public Dictionary<CreatureIndex, string> Pokemon { get;  }
        public Dictionary<WazaIndex, string> Moves { get; }
        public Dictionary<DungeonIndex, string> Dungeons { get; }

        /// <summary>
        /// Gets the name of a Pokemon by the internal Japanese name.
        /// </summary>
        /// <param name="internalName">Internal Japanese name such as "FUSHIGIDANE"</param>
        /// <returns>User-facing name such as "Bulbasaur", or null if the internal name could not be found</returns>
        public string? GetPokemonNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("POKEMON_NAME__POKEMON_" + internalName.ToUpper());
            return common.Strings.GetValueOrDefault(nameHash);
        }

        /// <summary>
        /// Gets the name of a dungeon by the internal Japanese name.
        /// </summary>
        public string? GetDungeonNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("DUNGEON_NAME__DUNGEON_" + internalName.ToUpper());
            return common.Strings.GetValueOrDefault(nameHash);
        }
		
        /// <summary>
        /// Gets the name of a move by the internal Japanese name.
        /// </summary>
        public string? GetMoveNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("WAZA_NAME__WAZA_" + internalName.ToUpper());
            return common.Strings.GetValueOrDefault(nameHash);
        }
    }
}
