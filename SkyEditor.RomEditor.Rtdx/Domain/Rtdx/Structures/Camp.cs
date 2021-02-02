using SkyEditor.IO.Binary;
using System.Collections.Generic;
using System.Text;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class Camp
    {
        // Credit to AntyMew
        private const int EntrySize = 0x114;

        public IDictionary<CampIndex, Entry> Entries { get; } = new Dictionary<CampIndex, Entry>();

        public Camp(byte[] data) : this(new BinaryFile(data))
        {
        }

        public Camp(IReadOnlyBinaryDataAccessor data)
        {
            var entryCount = checked((int)data.Length / EntrySize);
            for (int i = 0; i < entryCount; i++)
                Entries.Add((CampIndex)i, new Entry(data.Slice(i * EntrySize, EntrySize)));
        }

        public class Entry
        {
            public string Lineup { get; set; }
            public string UnlockCondition { get; set; }
            public int Price { get; set; }
            public string BackgroundTexture { get; set; }
            public string BackgroundMusic { get; set; }

            public Entry(IReadOnlyBinaryDataAccessor data)
            {
                Lineup = data.ReadString(2, 0x40, Encoding.ASCII);
                UnlockCondition = data.ReadString(0x42, 0x40, Encoding.ASCII).TrimEnd('\0');
                Price = data.ReadInt32(0x84);
                BackgroundTexture = data.ReadString(0x94, 0x40, Encoding.ASCII).TrimEnd('\0');
                BackgroundMusic = data.ReadString(0xD4, 0x40, Encoding.ASCII).TrimEnd('\0');
            }
        }
    }
}
