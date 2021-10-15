using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public enum StringType
    {
        Common,
        Dungeon,
        Script
    }

    public class LocalizedStringCollection
    {
        public Dictionary<TextIDHash, string> CommonStringsOriginal { get; } = new Dictionary<TextIDHash, string>();
        public Dictionary<TextIDHash, string> CommonStringsOverride { get; set; } = new Dictionary<TextIDHash, string>();
        public Dictionary<int, string> DungeonStringsOriginal { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> DungeonStringsOverride { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, string> ScriptStringsOriginal { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> ScriptStringsOverride { get; set; } = new Dictionary<int, string>();

        private Farc farc;
        private MessageBinEntry common;
        private MessageBinEntry dungeon;
        private MessageBinEntry script;
        private ICodeTable codeTable;

        private static readonly Dictionary<string, int> TextIdValues = Enum.GetValues(typeof(TextIDHash)).Cast<TextIDHash>().ToDictionary(h => h.ToString("f"), h => (int)h);

        public LocalizedStringCollection(Farc farc, ICodeTable codeTable)
        {
            this.farc = farc;
            this.codeTable = codeTable;

            var commonBin = farc.GetFile("common.bin");
            var dungeonBin = farc.GetFile("dungeon.bin");
            var scriptBin = farc.GetFile("script.bin");

            if (commonBin == null || dungeonBin == null || scriptBin == null)
            {
                throw new NoNullAllowedException("Farc must contain common.bin, dungeon.bin and script.bin");
            }

            this.common = new MessageBinEntry(commonBin);
            this.dungeon = new MessageBinEntry(dungeonBin);
            this.script = new MessageBinEntry(scriptBin);

            foreach (var entry in common.Strings)
            {
                var stringValue = entry.Value.FirstOrDefault();
                if (stringValue == null)
                {
                    continue;
                }
                CommonStringsOriginal[(TextIDHash) entry.Key] = codeTable.UnicodeDecode(stringValue.Value!)
                    .Replace("[R]", "\n");
            }
            
            foreach (var entry in dungeon.Strings)
            {
                var stringValue = entry.Value.FirstOrDefault();
                if (stringValue == null)
                {
                    continue;
                }
                DungeonStringsOriginal[(int) entry.Key] = codeTable.UnicodeDecode(stringValue.Value!)
                    .Replace("[R]", "\n");
            }

            foreach (var entry in script.Strings)
            {
                var stringValue = entry.Value.FirstOrDefault();
                if (stringValue == null)
                {
                    continue;
                }
                ScriptStringsOriginal[(int) entry.Key] = codeTable.UnicodeDecode(stringValue.Value)
                    .Replace("[R]", "\n");
            }
        }

        public IEnumerable<(int hash, string value, bool isOverrideString)> GetStrings(StringType type)
        {
            var originalDict = type switch
            {
                StringType.Common => CommonStringsOriginal.ToDictionary(kv => (int) kv.Key, kv => kv.Value),
                StringType.Dungeon => DungeonStringsOriginal,
                StringType.Script => ScriptStringsOriginal,
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
            var overrideDict = type switch
            {
                StringType.Common => CommonStringsOverride.ToDictionary(kv => (int) kv.Key, kv => kv.Value),
                StringType.Dungeon => DungeonStringsOverride,
                StringType.Script => ScriptStringsOverride,
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };

            Dictionary<int, (int hash, string value, bool isOverrideString)> newDict = originalDict
                .ToDictionary(kv => kv.Key, kv => (kv.Key, kv.Value, false));

            foreach (var item in overrideDict)
            {
                if (newDict.ContainsKey(item.Key))
                {
                    newDict[item.Key] = (item.Key, item.Value, true);
                }
                else
                {
                    newDict.Add(item.Key, (item.Key, item.Value, true));
                }
            }

            return newDict.Values;
        }

        public string? GetString(StringType type, int hash, out bool isOverrideString)
        {
            if (type == StringType.Common)
            {
                if (CommonStringsOverride.TryGetValue((TextIDHash) hash, out var value))
                {
                    isOverrideString = true;
                    return value;
                }
                isOverrideString = false;
                if (CommonStringsOriginal.TryGetValue((TextIDHash) hash, out string? originalValue))
                {
                    return originalValue;
                }
                return null;
            }
            else if (type == StringType.Dungeon)
            {
                if (DungeonStringsOverride.TryGetValue(hash, out var value))
                {
                    isOverrideString = true;
                    return value;
                }
                isOverrideString = false;
                if (DungeonStringsOriginal.TryGetValue(hash, out string? originalValue))
                {
                    return originalValue;
                }
                return null;
            }
            else if (type == StringType.Script)
            {
                if (ScriptStringsOverride.TryGetValue(hash, out var value))
                {
                    isOverrideString = true;
                    return value;
                }
                isOverrideString = false;
                if (ScriptStringsOriginal.TryGetValue(hash, out string? originalValue))
                {
                    return originalValue;
                }
                return null;
            }
            else
            {
                throw new ArgumentException("Invalid type", nameof(type));
            }
        }

        public string? GetString(StringType type, int hash)
        {
            return GetString(type, hash, out var _);
        }

        public string? GetString(StringType type, string key)
        {
            return GetString(type, (int) Crc32Hasher.Crc32Hash(key), out var _);
        }

        public string? GetCommonString(TextIDHash hash)
        {
            return GetString(StringType.Common, (int) hash);
        }

        public string? GetCommonString(int hash)
        {
            return GetString(StringType.Common, hash);
        }

        public string? GetCommonString(string key)
        {
            return GetString(StringType.Common, (int) Crc32Hasher.Crc32Hash(key));
        }

        public void SetString(StringType type, int hash, string value)
        {
            if (type == StringType.Common)
            {
                if (!CommonStringsOverride.ContainsKey((TextIDHash) hash))
                {
                    CommonStringsOverride.Add((TextIDHash) hash, value);
                }
                else
                {
                    CommonStringsOverride[(TextIDHash) hash] = value;
                }
            }
            else if (type == StringType.Dungeon)
            {
                if (!DungeonStringsOverride.ContainsKey(hash))
                {
                    DungeonStringsOverride.Add(hash, value);
                }
                else
                {
                    DungeonStringsOverride[hash] = value;
                }
            }
            else if (type == StringType.Script)
            {
                if (!ScriptStringsOverride.ContainsKey(hash))
                {
                    ScriptStringsOverride.Add(hash, value);
                }
                else
                {
                    ScriptStringsOverride[hash] = value;
                }
            }
            else
            {
                throw new ArgumentException("Invalid type", nameof(type));
            }
        }

        public void SetString(StringType type, string key, string value)
        {
            SetString(type, (int) Crc32Hasher.Crc32Hash(key), value);
        }

        public void SetCommonString(int hash, string value)
        {
            SetString(StringType.Common, (int) hash, value);
        }

        public void SetCommonString(TextIDHash hash, string value)
        {
            SetString(StringType.Common, (int) hash, value);
        }

        public void SetCommonString(string key, string value)
        {
            SetString(StringType.Common, (int) Crc32Hasher.Crc32Hash(key), value);
        }

        public int GetDungeonNameHash(string internalName)
        {
            return TextIdValues.GetValueOrDefault("DUNGEON_NAME__DUNGEON_" + internalName.ToUpper());
        }

        public int GetDungeonNameHash(DungeonIndex index)
        {
            return GetDungeonNameHash(index.ToString("f"));
        }

        public string? GetDungeonNameByInternalName(string internalName)
        {
            return GetCommonString(GetDungeonNameHash(internalName));
        }

        public string? GetDungeonName(DungeonIndex index)
        {
            return GetDungeonNameByInternalName(index.ToString("f"));
        }

        public int GetPokemonNameHash(string internalName)
        {
            return TextIdValues.GetValueOrDefault("POKEMON_NAME__POKEMON_" + internalName.ToUpper());
        }

        public int GetPokemonNameHash(CreatureIndex index)
        {
            return GetPokemonNameHash(index.ToString("f"));
        }

        /// <summary>
        /// Gets the name of a Pokemon by the internal Japanese name.
        /// </summary>
        /// <param name="internalName">Internal Japanese name such as "FUSHIGIDANE"</param>
        /// <returns>User-facing name such as "Bulbasaur", or null if the internal name could not be found</returns>
        public string? GetPokemonNameByInternalName(string internalName)
        {
            return GetCommonString(GetPokemonNameHash(internalName));
        }

        public string? GetPokemonName(CreatureIndex index)
        {
            return GetPokemonNameByInternalName(index.ToString("f"));
        }

        public string? GetItemName(ItemIndex index, bool plural = false)
        {
            return GetCommonString(GetItemNameHash(index, plural));
        }

        public int GetItemNameHash(string internalName, bool plural = false)
        {
            return TextIdValues.GetValueOrDefault(plural
                ? "ITEM_NAME_PLURAL__ITEM_PLURAL_" + internalName.ToUpper()
                : "ITEM_NAME__ITEM_" + internalName.ToUpper());
        }

        public int GetItemNameHash(ItemIndex index, bool plural = false)
        {
            var internalName = index.ToString("f");
            return GetItemNameHash(internalName, plural);
        }

        public string? GetItemNameByInternalName(string internalName, bool plural = false)
        {
            return GetCommonString(GetItemNameHash(internalName, plural));
        }

        public string? GetItemDescription(ItemIndex index)
        {
            return GetCommonString(GetItemDescriptionHash(index));
        }

        public int GetItemDescriptionHash(string internalName)
        {
            return TextIdValues.GetValueOrDefault("ITEM_EXPLANATION__EXPLAIN_" + internalName.ToUpper());
        }

        public int GetItemDescriptionHash(ItemIndex index)
        {
            var internalName = index.ToString("f");
            return GetItemDescriptionHash(internalName);
        }

        public string? GetItemDescriptionByInternalName(string internalName)
        {
            return GetCommonString(GetItemDescriptionHash(internalName));
        }

        public int GetMoveNameHash(string internalName)
        {
            return TextIdValues.GetValueOrDefault("WAZA_NAME__WAZA_" + internalName.ToUpper());
        }

        public int GetMoveNameHash(WazaIndex index)
        {
            return GetMoveNameHash(index.ToString("f"));
        }

        public string? GetMoveNameByInternalName(string internalName)
        {
            return GetCommonString(GetMoveNameHash(internalName));
        }

        public string? GetMoveName(WazaIndex index)
        {
            return GetMoveNameByInternalName(index.ToString("f"));
        }

        public string? GetMoveDescription(WazaIndex index)
        {
            return GetMoveDescriptionByInternalName(index.ToString("f"));
        }

        public int GetMoveDescriptionHash(string internalName)
        {
            return TextIdValues.GetValueOrDefault("WAZA_EXPLANATION__EXPLAIN_" + internalName.ToUpper());
        }

        public int GetMoveDescriptionHash(WazaIndex index)
        {
            return GetMoveDescriptionHash(index.ToString("f"));
        }

        public string? GetMoveDescriptionByInternalName(string internalName)
        {
            return GetCommonString(GetMoveDescriptionHash(internalName));
        }

        public int GetPokemonTaxonomyHash(int taxonId)
        {
            // It's stored in pokemon_data_info 1 higher than the internal id
            return TextIdValues.GetValueOrDefault("POKEMON_TAXIS__SPECIES_"
                + (taxonId - 1).ToString().PadLeft(3, '0'));
        }

        public string? GetPokemonTaxonomy(int taxonId)
        {
            return GetCommonString(GetPokemonTaxonomyHash(taxonId));
        }

        public string? GetPokemonTypeNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("TYPE_NAME__TYPE_" + internalName.ToUpper());
            return GetCommonString(nameHash);
        }

        public string? GetPokemonTypeName(PokemonType pokemonType)
        {
            return GetPokemonTypeNameByInternalName(pokemonType.ToString("f"));
        }

        public string? GetAbilityNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("ABILITY_NAME__" + internalName.ToUpper());
            return GetCommonString(nameHash);
        }

        public string? GetAbilityName(AbilityIndex abilityIndex)
        {
            return GetAbilityNameByInternalName(abilityIndex.ToString("f"));
        }

        public string? GetDungeonStatusNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("DUNGEON_STATUS_NAME__DUNGEON_STATUS_" + internalName.ToUpper());
            return GetCommonString(nameHash);
        }

        public string? GetDungeonStatusName(DungeonStatusIndex statusIndex)
        {
            return GetDungeonStatusNameByInternalName(statusIndex.ToString("f"));
        }

        public string? GetStatusNameByInternalName(string internalName)
        {
            var nameHash = TextIdValues.GetValueOrDefault("STATUS_NAME__STATUS_" + internalName.ToUpper());
            return GetCommonString(nameHash);
        }

        public string? GetStatusName(StatusIndex statusIndex)
        {
            return GetStatusNameByInternalName(statusIndex.ToString("f"));
        }

        public async Task Flush()
        {
            foreach (var overrideString in CommonStringsOverride)
            {
                common.SetString((int) overrideString.Key, codeTable.UnicodeEncode(overrideString.Value));
            }
            foreach (var overrideString in DungeonStringsOverride)
            {
                dungeon.SetString(overrideString.Key, codeTable.UnicodeEncode(overrideString.Value));
            }
            foreach (var overrideString in ScriptStringsOverride)
            {
                script.SetString(overrideString.Key, codeTable.UnicodeEncode(overrideString.Value));
            }

            var commonTask = Task.Run(() => common.ToByteArray());
            var dungeonTask = Task.Run(() => dungeon.ToByteArray());
            var scriptTask = Task.Run(() => script.ToByteArray());

            await Task.WhenAll(commonTask, dungeonTask, scriptTask);

            farc.SetFile("common.bin", commonTask.Result);
            farc.SetFile("dungeon.bin", dungeonTask.Result);
            farc.SetFile("script.bin", scriptTask.Result);
        }
    }

    public interface IStringCollection
    {
        LocalizedStringCollection GetStringsForLanguage(LanguageType language);
        IReadOnlyDictionary<LanguageType, LocalizedStringCollection> LoadedLanguages { get; }
        void Flush();

        LocalizedStringCollection Japanese { get; }
        LocalizedStringCollection English { get; }
        LocalizedStringCollection French { get; }
        LocalizedStringCollection German { get; }
        LocalizedStringCollection Italian { get; }
        LocalizedStringCollection Spanish { get; }
    }

    public class StringCollection : IStringCollection
    {
        private IRtdxRom rom;

        private Dictionary<LanguageType, LocalizedStringCollection> loadedLanguages =
            new Dictionary<LanguageType, LocalizedStringCollection>();

        public IReadOnlyDictionary<LanguageType, LocalizedStringCollection> LoadedLanguages => loadedLanguages;

        public StringCollection(IRtdxRom rom)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }
            this.rom = rom;
        }

        public LocalizedStringCollection GetStringsForLanguage(LanguageType language)
        {
            if (loadedLanguages.TryGetValue(language, out var collection))
            {
                return collection;
            }

            var newCollection = new LocalizedStringCollection(rom.GetMessageBin(language), rom.GetCodeTable());
            loadedLanguages.Add(language, newCollection);
            return newCollection;
        }

        public void Flush()
        {
            var tasks = loadedLanguages.Values.Select(lang => lang.Flush()).ToArray();
            Task.WaitAll(tasks);
        }

        public LocalizedStringCollection Japanese => GetStringsForLanguage(LanguageType.JP);
        public LocalizedStringCollection English => GetStringsForLanguage(LanguageType.EN);
        public LocalizedStringCollection French => GetStringsForLanguage(LanguageType.FR);
        public LocalizedStringCollection German => GetStringsForLanguage(LanguageType.GE);
        public LocalizedStringCollection Italian => GetStringsForLanguage(LanguageType.IT);
        public LocalizedStringCollection Spanish => GetStringsForLanguage(LanguageType.SP);
    }
}
