using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SkyEditor.IO.Binary;

namespace SkyEditor.RomEditor.Domain.Common.Structures
{
    public interface ICodeTable
    {
        void AddEntry(CodeTable.Entry entry);

        /// <summary>
        /// Replaces special text tokens with Unicode symbols for message.bin
        /// </summary>
        /// <param name="text">User-friendly text</param>
        string UnicodeEncode(string text);

        /// <summary>
        /// Replaces Unicode symbols from message.bin with human readable text tokens
        /// </summary>
        /// <param name="text">Encoded text</param>
        string UnicodeDecode(string text);
    }

    /// <summary>
    /// This class allows encoding and decoding strings between human-readable text directives like [hero]
    /// and an internal representation with special Unicode characters.
    /// </summary>
    /// <remarks>
    /// Note that the code_table.bin file is used, which doesn't always match the directives that are used
    /// in the Japanese text inside the .lua files. It seems like the game uses either a combination of hard-coded
    /// regular expressions and code_table.bin to parse these or even hardcodes everything and ignores code_table.bin
    /// completely. However, this should work for assembling custom text with the most common directives
    /// like [hero], [partner] etc.
    /// </remarks>
    // Thanks to Blue and EddyK for figuring out the encoding details.
    public class CodeTable : ICodeTable
    {
        private static readonly Regex StringTokenRegex = new Regex("\\[(.+?:?)(\\w+)*\\]", RegexOptions.Compiled);

        public CodeTable()
        {
        }

        public CodeTable(byte[] data)
        {
            var sir0 = new Sir0(data);

            for (int i = 3; i < sir0.PointerOffsets.Count - 2; i++)
            {
                long pointerOffset = sir0.PointerOffsets[i];
                long stringOffset = sir0.Data.ReadInt64(pointerOffset);
                string codeString = sir0.Data.ReadNullTerminatedUnicodeString(stringOffset);
                ushort unicodeValue = sir0.Data.ReadUInt16(pointerOffset + 8);
                ushort flags = sir0.Data.ReadUInt16(pointerOffset + 10);
                short length = sir0.Data.ReadInt16(pointerOffset + 12);
                short unknown = sir0.Data.ReadInt16(pointerOffset + 14);

                AddEntry(new Entry
                {
                    CodeString = codeString,
                    UnicodeValue = unicodeValue,
                    Flags = flags,
                    Length = length,
                    Unknown = unknown,
                });
            }
        }

        public Dictionary<ushort, Entry> EntriesByUnicode { get; } = new Dictionary<ushort, Entry>();
        public Dictionary<string, Entry> EntriesByCodeString { get; } = new Dictionary<string, Entry>();

        public void AddEntry(Entry entry)
        {
            if (!EntriesByUnicode.ContainsKey(entry.UnicodeValue))
            {
                // There are duplicate unicode values in the table
                // TODO: more robust handling (maybe consider the Unknown as well)
                EntriesByUnicode.Add(entry.UnicodeValue, entry);
            }
            EntriesByCodeString.Add(entry.CodeString, entry);
        }

        /// <summary>
        /// Replaces special text tokens with Unicode symbols for message.bin
        /// </summary>
        /// <param name="text">User-friendly text</param>
        public string UnicodeEncode(string text)
        {
            var sb = new StringBuilder();

            var match = StringTokenRegex.Match(text);
            int lastStringPos = 0;

            while (match.Success)
            {
                sb.Append(text.Substring(lastStringPos, match.Index - lastStringPos));
                string directive = match.Groups[1].Value;
                string? valueString = match.Groups.Count > 1 ? match.Groups[2].Value : null;

                if (!string.IsNullOrEmpty(valueString) && EntriesByCodeString.TryGetValue(directive + valueString, out var entry))
                {
                    // Sometimes the directive and value are both included (e.g. [M:B01])
                    sb.Append((char)entry.UnicodeValue);
                }
                else if (EntriesByCodeString.TryGetValue(directive, out entry))
                {
                    ushort unicodeValue = entry.UnicodeValue;
                    if (entry.DigitFlag && valueString != null)
                    {
                        uint value;
                        if (ConstantReplacementTable.TryGetValue(entry.CodeString, out var enumType))
                        {
                            value = Convert.ToUInt32(Enum.Parse(enumType, valueString));
                        }
                        else
                        {
                            value = uint.Parse(valueString);
                        }

                        // Encode the value into the two low bytes.
                        // It seems like one of the bytes inside the value is encoded redundantly.
                        unicodeValue = (ushort)((unicodeValue & 0xFF00) | (byte)value);
                        sb.Append((char)unicodeValue);

                        if (entry.Length > 0)
                        {
                            if (entry.Length > 2)
                            {
                                throw new Exception("Invalid entry length.");
                            }

                            byte[] bytes = BitConverter.GetBytes(value + 1);
                            sb.Append(Encoding.Unicode.GetString(bytes, 0, entry.Length * 2));
                        }
                    }
                    else
                    {
                        sb.Append((char)unicodeValue);
                    }
                }
                else
                {
                    sb.Append(match.Value);
                }
                lastStringPos = match.Index + match.Length;
                match = match.NextMatch();
            }

            sb.Append(text.Substring(lastStringPos, text.Length - lastStringPos));
            return sb.ToString();
        }

        /// <summary>
        /// Replaces Unicode symbols from message.bin with human readable text tokens
        /// </summary>
        /// <param name="text">Encoded text</param>
        public string UnicodeDecode(string text)
        {
            var sb = new StringBuilder();
            var textBytes = Encoding.Unicode.GetBytes(text);
            for (int i = 0; i < textBytes.Length; i += 2)
            {
                ushort shortCharacter = BitConverter.ToUInt16(textBytes, i);
                if (EntriesByUnicode.TryGetValue(shortCharacter, out var entry))
                {
                    // A direct match for the character was found
                    sb.Append($"[{entry.CodeString}]");
                }
                else if (EntriesByUnicode.TryGetValue((ushort)(shortCharacter & 0xFF00), out entry))
                {
                    // A match for the low bytes was found, which means that it encodes some other value.
                    // The amount of words to decode is found in the Length.
                    uint encodedValue = shortCharacter & 0xFFu;
                    if (entry.Length > 0)
                    {
                        encodedValue = 0;
                        for (int j = 0; j < entry.Length; j++)
                        {
                            i += (j + 1) * 2;

                            uint partialValue = BitConverter.ToUInt16(textBytes, i);
                            encodedValue |= partialValue << (j * 2);
                        }
                        encodedValue--;
                    }

                    string encodedValueString;
                    if (ConstantReplacementTable.TryGetValue(entry.CodeString, out var enumType))
                    {
                        encodedValueString = Enum.GetName(enumType, encodedValue) ?? encodedValue.ToString();
                    }
                    else
                    {
                        encodedValueString = encodedValue.ToString();
                    }
                    sb.Append($"[{entry.CodeString}{encodedValueString}]");
                }
                else
                {
                    sb.Append(Encoding.Unicode.GetString(textBytes, i, 2));
                }
            }
            return sb.ToString();
        }
        protected virtual Dictionary<string, Type> ConstantReplacementTable { get; } = new Dictionary<string, Type>();

        public class Entry
        {
            public string CodeString { get; set; } = "";
            public ushort UnicodeValue { get; set; }
            public ushort Flags { get; set; }
            public short Length { get; set; }
            public short Unknown { get; set; }

            // If the flag bit 0 is set, the next X words will be replaced with the number
            // after ":" in the string where X is specified in the "Length" field
            public bool DigitFlag => (Flags & 1) != 0;
        }
    }
}
