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

        public Rank(byte[] data)
        {
            var sir0 = new Sir0(data);
            long entriesOffset = sir0.SubHeader.ReadInt64(0x0);
            long entryCount = sir0.SubHeader.ReadInt64(0x8);
            for (int i = 0; i < entryCount; i++)
            {
                Entries.Add((RankIndex)i, new Entry(sir0, sir0.Data.Slice(entriesOffset + i * EntrySize, EntrySize)));
            }
        }

        public Sir0 ToSir0()
        {
            var sir0 = new Sir0Builder(8);
            var stringPointers = new List<long>(Entries.Count);
            foreach (var entry in Entries)
            {
                stringPointers.Add(sir0.Length);
                if (string.IsNullOrEmpty(entry.Value.RewardStatue))
                {
                    sir0.WritePadding(sir0.Length, 0x10);
                }
                else
                {
                    sir0.WriteString(sir0.Length, Encoding.ASCII, entry.Value.RewardStatue);
                    sir0.Align(0x10); // All strings have a length of 0x10
                }
            }

            sir0.Align(0x10);

            long entriesOffset = sir0.Length;
            for (int i = 0; i < Entries.Count; i++)
            {
                var entry = Entries[(RankIndex) i];
                entry.WriteTo(sir0, stringPointers[i]);
            }

            sir0.SubHeaderOffset = sir0.Length;
            sir0.WriteInt64(sir0.Length, entriesOffset);
            sir0.WriteInt64(sir0.Length, Entries.Count);

            return sir0.Build(alignFooter: false);
        }

        public byte[] ToByteArray() => ToSir0().Data.ReadArray();
        
        public class Entry
        {
            public string RewardStatue { get; set; }
            public int MinPoints { get; set; }
            public short Short0C { get; set; }
            public short ToolboxSize { get; set; }
            public short CampCapacity { get; set; }
            public short TeamPresets { get; set; }
            public short JobLimit { get; set; }
            public short Short16 { get; set; }
            public short Short18 { get; set; }

            public Entry(Sir0 sir0, IReadOnlyBinaryDataAccessor data)
            {
                int rewardStatueStringOffset = checked((int)data.ReadInt64(0));
                RewardStatue = sir0.Data.ReadString(rewardStatueStringOffset, 0x10, Encoding.ASCII).TrimEnd('\0');
                MinPoints = data.ReadInt32(0x8);
                Short0C = data.ReadInt16(0xC);
                ToolboxSize = data.ReadInt16(0xE);
                CampCapacity = data.ReadInt16(0x10);
                TeamPresets = data.ReadInt16(0x12);
                JobLimit = data.ReadInt16(0x14);
                Short16 = data.ReadInt16(0x16);
                Short18 = data.ReadInt16(0x18);
            }

            public void WriteTo(Sir0Builder sir0, long stringPointer)
            {
                sir0.WritePointer(sir0.Length, stringPointer);
                sir0.WriteInt32(sir0.Length, MinPoints);
                sir0.WriteInt16(sir0.Length, Short0C);
                sir0.WriteInt16(sir0.Length, ToolboxSize);
                sir0.WriteInt16(sir0.Length, CampCapacity);
                sir0.WriteInt16(sir0.Length, TeamPresets);
                sir0.WriteInt16(sir0.Length, JobLimit);
                sir0.WriteInt16(sir0.Length, Short16);
                sir0.WriteInt16(sir0.Length, Short18);
                sir0.WritePadding(sir0.Length, 0x6);
            }
        }
    }
}
