using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
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
    public class CodeTable
    {
        public Dictionary<ushort, Entry> EntriesByUnicode { get; } = new Dictionary<ushort, Entry>();
        public Dictionary<string, Entry> EntriesByCodeString { get; } = new Dictionary<string, Entry>();

        private static readonly Regex StringTokenRegex = new Regex("\\[(.+?:?)(\\d+)*\\]", RegexOptions.Compiled);

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
                int unknown = sir0.Data.ReadInt32(pointerOffset + 12);
                
                AddEntry(new Entry
                {
                    CodeString = codeString,
                    UnicodeValue = unicodeValue,
                    Flags = flags,
                    Unknown = unknown,
                });
            }
        }

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

        // Replaces special text tokens with Unicode symbols for message.bin
        public string UnicodeEncode(string text)
        {
            var sb = new StringBuilder();

            var match = StringTokenRegex.Match(text);
            int lastStringPos = 0;
            while (match.Success)
            {
                sb.Append(text.Substring(lastStringPos, match.Index - lastStringPos));
                string directive = match.Groups[1].Value;
                string? value = match.Groups.Count > 1 ? match.Groups[2].Value : null;

                if (!string.IsNullOrEmpty(value) && EntriesByCodeString.TryGetValue(directive + value, out var entry))
                {
                    // Sometimes the directive and value are both included (e.g. [M:B01])
                    sb.Append((char) entry.UnicodeValue);
                }
                else if (EntriesByCodeString.TryGetValue(directive, out entry)) 
                {
                    ushort unicodeValue = entry.UnicodeValue;
                    if (entry.DigitFlag && value != null)
                    {
                        // Encode the value into the two low bytes
                        // TODO: figure out how the game encodes values that are bigger than bytes (e.g. Pokédex numbers)
                        unicodeValue = (ushort) ((unicodeValue & 0xFF00) | byte.Parse(value));
                    }
                    sb.Append((char) unicodeValue);
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

        // Replaces Unicode symbols from message.bin with human readable text tokens
        public string UnicodeDecode(string text)
        {
            var sb = new StringBuilder();
            foreach (var character in text)
            {
                var bytes = Encoding.Unicode.GetBytes(new char[] { character });
                ushort shortCharacter = BitConverter.ToUInt16(bytes, 0);
                if (EntriesByUnicode.TryGetValue(shortCharacter, out var entry))
                {
                    // A direct match for the character was found
                    sb.Append($"[{entry.CodeString}]");
                }
                else if (EntriesByUnicode.TryGetValue((ushort) (shortCharacter & 0xFF00), out entry))
                {
                    // A match for the low bytes was found, which means that it encodes some other value
                    sb.Append($"[{entry.CodeString}{shortCharacter & 0x00FF}]");
                }
                else
                {
                    sb.Append(character);
                }
            }
            return sb.ToString();
        }

        public class Entry
        {
            public string CodeString { get; set; } = "";
            public ushort UnicodeValue { get; set; }
            public ushort Flags { get; set; }
            public int Unknown { get; set; }

            // If the flag bit 0 is set, byte 0 of the unicode character
            // will be replaced with the number after ":" in the string
            public bool DigitFlag => (Flags & 1) != 0;
        }
    }
}
