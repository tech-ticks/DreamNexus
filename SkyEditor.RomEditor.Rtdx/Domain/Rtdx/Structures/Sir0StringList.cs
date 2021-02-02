using System.Collections.Generic;
using System.Text;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class Sir0StringList
    {
        public List<string> Entries { get; } = new List<string>();

        private Encoding encoding;

        public Sir0StringList(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public Sir0StringList(byte[] data) : this(data, Encoding.Unicode)
        {
        }
        
        public Sir0StringList(byte[] data, Encoding encoding)
        {
            this.encoding = encoding;
            
            var sir0 = new Sir0(data);

            var entryCount = sir0.SubHeader.ReadInt32(0);
            for (int i = 0; i < entryCount; i++)
            {
                long stringOffset = sir0.SubHeader.ReadInt32(8 + i * sizeof(long));
                string value;
                // For some reason, ReadNullTerminatedString with Encoding.Unicode doesn't read UTF-16 characters properly
                if (encoding == Encoding.Unicode)
                {
                    value = sir0.Data.ReadNullTerminatedUnicodeString(stringOffset);
                }
                else
                {
                    value = sir0.Data.ReadNullTerminatedString(stringOffset, encoding);
                }
                Entries.Add(value);
            }
        }
        
        public Sir0 ToSir0()
        {
            var sir0 = new Sir0Builder(8);
            
            var pointers = new List<int>();
            
            var entriesSectionStart = sir0.Length;

            // Write the strings
            foreach (var entry in Entries)
            {
                pointers.Add(sir0.Length);
                sir0.WriteNullTerminatedString(sir0.Length, encoding, entry);
            }

            // Write the pointers
            sir0.Align(8);
            sir0.SubHeaderOffset = sir0.Length;
            sir0.WriteInt64(sir0.Length, Entries.Count);
            foreach (int pointer in pointers)
            {
                sir0.WritePointer(sir0.Length, pointer);
            }
            return sir0.Build();
        }
        
        public byte[] ToByteArray() => ToSir0().Data.ReadArray();
    }
}
