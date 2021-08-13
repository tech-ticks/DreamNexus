using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;

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
                CommonStringsOriginal[(TextIDHash) entry.Key] = codeTable.UnicodeDecode(stringValue.Value)
                    .Replace("[R]", "\n");
            }
            
            foreach (var entry in dungeon.Strings)
            {
                var stringValue = entry.Value.FirstOrDefault();
                if (stringValue == null)
                {
                    continue;
                }
                DungeonStringsOriginal[(int) entry.Key] = codeTable.UnicodeDecode(stringValue.Value)
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

        public void Flush()
        {
            foreach (var overrideString in CommonStringsOverride)
            {
                common.SetString((int) overrideString.Key, codeTable.UnicodeEncode(overrideString.Value)
                    .Replace("\n", "[R]"));
            }
            foreach (var overrideString in DungeonStringsOverride)
            {
                dungeon.SetString(overrideString.Key, codeTable.UnicodeEncode(overrideString.Value)
                    .Replace("\n", "[R]"));
            }
            foreach (var overrideString in ScriptStringsOverride)
            {
                script.SetString(overrideString.Key, codeTable.UnicodeEncode(overrideString.Value)
                    .Replace("\n", "[R]"));
            }

            farc.SetFile("common.bin", common.ToByteArray());
            farc.SetFile("dungeon.bin", dungeon.ToByteArray());
            farc.SetFile("script.bin", script.ToByteArray());
        }
    }

    public interface IStringCollection
    {
        LocalizedStringCollection GetStringsForLanguage(LanguageType language);
        IReadOnlyDictionary<LanguageType, LocalizedStringCollection> LoadedLanguages { get; }
        void Flush();
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
            foreach (var collection in loadedLanguages.Values)
            {
                collection.Flush();
            }
        }
    }
}
