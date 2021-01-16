using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyEditor.RomEditor.Domain.Rtdx
{
    public interface ICommonStrings
    {
        Dictionary<CreatureIndex, string> Pokemon { get; }
        Dictionary<WazaIndex, string> Moves { get; }
        Dictionary<SugowazaIndex, string> RareQualities { get; }
        Dictionary<DungeonIndex, string> Dungeons { get; }
        Dictionary<ItemIndex, string> Items { get; }
        Dictionary<DungeonStatusIndex, string> DungeonStatuses { get; }
        Dictionary<StatusIndex, string> Statuses { get; }
        Dictionary<PokemonType, string> PokemonTypes { get; }

        /// <summary>
        /// Gets the name of a Pokemon by the internal Japanese name.
        /// </summary>
        /// <param name="internalName">Internal Japanese name such as "FUSHIGIDANE"</param>
        /// <returns>User-facing name such as "Bulbasaur", or null if the internal name could not be found</returns>
        string? GetPokemonNameByInternalName(string internalName);

        string? GetPokemonTaxonomy(int speciesId);

        /// <summary>
        /// Gets the name of a move by the internal Japanese name.
        /// </summary>
        string? GetMoveNameByInternalName(string internalName);

        string? GetMoveName(WazaIndex wazaIndex);

        string? GetRareQualityNameByInternalName(string internalName);

        string? GetRareQualityName(SugowazaIndex sugowazaIndex);

        string? GetAbilityNameByInternalName(string internalName);

        string? GetAbilityName(AbilityIndex abilityIndex);

        string? GetDungeonNameByInternalName(string internalName);

        string? GetDungeonName(DungeonIndex dungeonIndex);

        string? GetItemNameByInternalName(string internalName);

        string? GetItemName(ItemIndex itemIndex);

        string? GetDungeonStatusNameByInternalName(string internalName);

        string? GetDungeonStatusName(DungeonStatusIndex dungeonStatusIndex);

        string? GetStatusNameByInternalName(string internalName);

        string? GetStatusName(StatusIndex statusIndex);

        string? GetPokemonTypeNameByInternalName(string internalName);

        string? GetPokemonTypeName(PokemonType pokemonTypeIndex);
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
                var name = common.GetStringByHash(nameHash);
                Moves.Add(waza, name ?? "");
            }

            RareQualities = new Dictionary<SugowazaIndex, string>();
            var rareQualities = Enum.GetValues(typeof(SugowazaIndex)).Cast<SugowazaIndex>().ToArray();
            foreach (SugowazaIndex sugowaza in rareQualities)
            {
                if (sugowaza == default)
                {
                    continue;
                }

                var nameHash = TextIdValues.GetValueOrDefault("SUGOWAZA_NAME__" + sugowaza.ToString("f"));
                var name = common.GetStringByHash(nameHash);
                RareQualities.Add(sugowaza, name ?? "");
            }

            Dungeons = new Dictionary<DungeonIndex, string>();
            var dungeons = Enum.GetValues(typeof(DungeonIndex)).Cast<DungeonIndex>().ToArray();
            foreach (DungeonIndex dungeon in dungeons)
            {
                if (dungeon == default)
                {
                    continue;
                }

                var name = GetDungeonNameByInternalName(dungeon.ToString("f"));
                Dungeons.Add(dungeon, name ?? "");
            }

            // We need to handle items in a convoluted way since there are multiple entries with the same value
            // e.g. ARROW_MIN and ARROW_WOOD are the same
            Items = new Dictionary<ItemIndex, string>();
            var itemNames = Enum.GetNames(typeof(ItemIndex));
            var items = Enum.GetValues(typeof(ItemIndex)).Cast<ItemIndex>().ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                string itemName = itemNames[i];
                var item = items[i];
                if (item == default || itemName.EndsWith("_MIN") || itemName.EndsWith("_MAX"))
                {
                    continue;
                }

                var name = GetItemNameByInternalName(itemName);
                Items.Add(item, name ?? "");
            }

            DungeonStatuses = new Dictionary<DungeonStatusIndex, string>();
            var dungeonStatuses = Enum.GetValues(typeof(DungeonStatusIndex)).Cast<DungeonStatusIndex>().ToArray();
            foreach (DungeonStatusIndex status in dungeonStatuses)
            {
                if (status == default)
                {
                    continue;
                }

                var name = GetDungeonStatusNameByInternalName(status.ToString("f"));
                DungeonStatuses.Add(status, name ?? "");
            }

            Statuses = new Dictionary<StatusIndex, string>();
            var statuses = Enum.GetValues(typeof(StatusIndex)).Cast<StatusIndex>().ToArray();
            foreach (StatusIndex status in statuses)
            {
                if (status == default)
                {
                    continue;
                }

                var name = GetStatusNameByInternalName(status.ToString("f"));
                Statuses.Add(status, name ?? "");
            }

            PokemonTypes = new Dictionary<PokemonType, string>();
            var pokemonTypes = Enum.GetValues(typeof(PokemonType)).Cast<PokemonType>().ToArray();
            foreach (PokemonType pokemonType in pokemonTypes)
            {
                if (pokemonType == default)
                {
                    continue;
                }

                var name = GetPokemonTypeNameByInternalName(pokemonType.ToString("f"));
                PokemonTypes.Add(pokemonType, name ?? "");
            }
        }

        private readonly MessageBinEntry common;

        public Dictionary<CreatureIndex, string> Pokemon { get; }
        public Dictionary<WazaIndex, string> Moves { get; }
        public Dictionary<SugowazaIndex, string> RareQualities { get; }
        public Dictionary<DungeonIndex, string> Dungeons { get; }
        public Dictionary<ItemIndex, string> Items { get; }
        public Dictionary<DungeonStatusIndex, string> DungeonStatuses { get; }
        public Dictionary<StatusIndex, string> Statuses { get; }
        public Dictionary<PokemonType, string> PokemonTypes { get; }

        /// <summary>
        /// Gets the name of a Pokemon by the internal Japanese name.
        /// </summary>
        /// <param name="internalName">Internal Japanese name such as "FUSHIGIDANE"</param>
        /// <returns>User-facing name such as "Bulbasaur", or null if the internal name could not be found</returns>
        public string? GetPokemonNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("POKEMON_NAME__POKEMON_" + internalName.ToUpper());
            return common.GetStringByHash(nameHash);
        }

        public string? GetPokemonTaxonomy(int taxonId)
        {
            taxonId -= 1; // It's stored in pokemon_data_info 1 higher than the internal id
            var nameHash = TextIdValues.GetValueOrDefault("POKEMON_TAXIS__SPECIES_" + taxonId.ToString().PadLeft(3, '0'));
            return common.GetStringByHash(nameHash);
        }

        /// <summary>
        /// Gets the name of a move by the internal Japanese name.
        /// </summary>
        public string? GetMoveNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("WAZA_NAME__WAZA_" + internalName.ToUpper());
            return common.GetStringByHash(nameHash);
        }

        public string? GetMoveName(WazaIndex wazaIndex)
        {
            return GetMoveNameByInternalName(wazaIndex.ToString("f"));
        }

        public string? GetRareQualityNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("SUGOWAZA_NAME__" + internalName.ToUpper());
            return common.GetStringByHash(nameHash);
        }

        public string? GetRareQualityName(SugowazaIndex sugowazaIndex)
        {
            return GetRareQualityNameByInternalName(sugowazaIndex.ToString("f"));
        }

        public string? GetAbilityNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("ABILITY_NAME__" + internalName.ToUpper());
            return common.GetStringByHash(nameHash);
        }

        public string? GetAbilityName(AbilityIndex abilityIndex)
        {
            return GetAbilityNameByInternalName(abilityIndex.ToString("f"));
        }

        public string? GetDungeonNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("DUNGEON_NAME__DUNGEON_" + internalName.ToUpper());
            return common.GetStringByHash(nameHash);
        }

        public string? GetDungeonName(DungeonIndex dungeonIndex)
        {
            return GetDungeonNameByInternalName(dungeonIndex.ToString("f"));
        }

        public string? GetItemNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("ITEM_NAME__ITEM_" + internalName.ToUpper());
            return common.GetStringByHash(nameHash);
        }

        public string? GetItemName(ItemIndex itemIndex)
        {
            return GetItemNameByInternalName(itemIndex.ToString("f"));
        }

        public string? GetDungeonStatusNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("DUNGEON_STATUS_NAME__DUNGEON_STATUS_" + internalName.ToUpper());
            return common.GetStringByHash(nameHash);
        }

        public string? GetDungeonStatusName(DungeonStatusIndex statusIndex)
        {
            return GetDungeonStatusNameByInternalName(statusIndex.ToString("f"));
        }

        public string? GetStatusNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("STATUS_NAME__STATUS_" + internalName.ToUpper());
            return common.GetStringByHash(nameHash);
        }

        public string? GetStatusName(StatusIndex statusIndex)
        {
            return GetStatusNameByInternalName(statusIndex.ToString("f"));
        }

        public string? GetPokemonTypeNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("TYPE_NAME__TYPE_" + internalName.ToUpper());
            return common.GetStringByHash(nameHash);
        }

        public string? GetPokemonTypeName(PokemonType pokemonType)
        {
            return GetPokemonTypeNameByInternalName(pokemonType.ToString("f"));
        }
    }
}
