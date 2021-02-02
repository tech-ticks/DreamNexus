using SkyEditor.IO.Binary;
using System.Collections.Generic;
using System.Text;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Common.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class Rank
    {
        // Credit to AntyMew
        private const int EntrySize = 0x20;

        public IDictionary<RankIndex, Entry> Entries { get; } = new Dictionary<RankIndex, Entry>();

        public Rank(byte[] data) : this(new BinaryFile(data))
        {
        }

        public Rank(IReadOnlyBinaryDataAccessor data)
        {
            var sir0 = new Sir0(data);
            long entriesOffset = sir0.SubHeader.ReadInt64(0x0);
            long entryCount = sir0.SubHeader.ReadInt64(0x8);
            for (int i = 0; i < entryCount; i++)
            {
                Entries.Add((RankIndex)i, new Entry(sir0, sir0.Data.Slice(entriesOffset + i * EntrySize, EntrySize)));
            }
        }

        public class Entry
        {
            public string RewardStatue { get; set; }
            public int MinPoints { get; set; }
            public short Short0C { get; set; }
            public short ToolboxSize { get; set; }
            public short CampCapacity { get; set; }
            public short TeamPresets { get; set; }
            public short JobLimit { get; set; }

            public Entry(Sir0 sir0, IReadOnlyBinaryDataAccessor data)
            {
                int offset = checked((int)data.ReadInt64(0));
                RewardStatue = sir0.Data.ReadString(offset, 0x10, Encoding.ASCII).TrimEnd('\0');
                MinPoints = data.ReadInt32(0x8);
                Short0C = data.ReadInt16(0xC);
                ToolboxSize = data.ReadInt16(0xE);
                CampCapacity = data.ReadInt16(0x10);
                TeamPresets = data.ReadInt16(0x12);
                JobLimit = data.ReadInt16(0x14);
            }
        }
    }
}
