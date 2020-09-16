using System.Collections.Generic;
using System.Text;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class CodeTable
    {
        public List<Entry> Entries { get; } = new List<Entry>();
        
        public CodeTable(byte[] data)
        {            
            var sir0 = new Sir0(data);

            for (int i = 3; i < sir0.PointerOffsets.Count - 2; i++)
            {
                long pointerOffset = sir0.PointerOffsets[i];
                long stringOffset = sir0.Data.ReadInt64(pointerOffset);
                string codeString = sir0.Data.ReadNullTerminatedUnicodeString(stringOffset);
                int unicodeValue = sir0.Data.ReadInt32(pointerOffset + 8);
                int unknown = sir0.Data.ReadInt32(pointerOffset + 12);

                Entries.Add(new Entry
                {
                    CodeString = codeString,
                    UnicodeValue = unicodeValue,
                    Unknown = unknown,
                });
            }
        }

        public class Entry
        {
            public string CodeString { get; set; } = "";
            public int UnicodeValue { get; set; }
            public int Unknown { get; set; }
        }
    }
}
