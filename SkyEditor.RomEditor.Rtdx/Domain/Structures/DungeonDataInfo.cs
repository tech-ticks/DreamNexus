using SkyEditor.IO.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DungeonIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.dungeon.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public class DungeonDataInfo
    {
        private const int EntrySize = 0x1C;

        public IDictionary<DungeonIndex, DungeonDataInfoEntry> Entries { get; }

        public DungeonDataInfo(byte[] data)
        {
            IReadOnlyBinaryDataAccessor accessor = new BinaryFile(data);
            var entryCount = checked((int)data.Length / EntrySize);
            var entries = new Dictionary<DungeonIndex, DungeonDataInfoEntry>(entryCount);
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add((DungeonIndex)i, new DungeonDataInfoEntry(accessor.Slice(i * EntrySize, EntrySize)));
            }
            this.Entries = entries;
        }

        [DebuggerDisplay("DungeonDataInfoEntry: {Index}|{Flags}|{Short08}|{Short0A}|{SortKey}|{DungeonBalanceIndex}|{Byte13}|{Byte14}|{Byte15}|{Byte17}|{Byte18}|{Byte19}")]
        public class DungeonDataInfoEntry
        {
            public DungeonDataInfoEntry(IReadOnlyBinaryDataAccessor data)
            {
                Flags = data.ReadInt32(0x00);
                Index = data.ReadInt32(0x04);
                Short08 = data.ReadInt16(0x08);
                Short0A = data.ReadInt16(0x0A);
                SortKey = data.ReadInt32(0x0C);
                DungeonBalanceIndex = data.ReadByte(0x12);
                Byte13 = data.ReadByte(0x13);
                MaxItems = data.ReadByte(0x14);
                MaxTeammates = data.ReadByte(0x15);
                Byte17 = data.ReadByte(0x17);
                Byte18 = data.ReadByte(0x18);
                Byte19 = data.ReadByte(0x19);
            }

            // bit 0: Floor direction (0 = BxxF, 1 = xxF)
            // bit 15: Can recruit wild Pokemon
            // bit 17: Permanent Radar status
            // bit 18: Permanent Scanning status
            public int Flags { get; }
            public int Index { get; }
            public short Short08 { get; }
            public short Short0A { get; }
            public int SortKey { get; }
            public byte DungeonBalanceIndex { get; }
            public byte Byte13 { get; }
            public byte MaxItems { get; }
            public byte MaxTeammates { get; }
            public byte Byte17 { get; }
            public byte Byte18 { get; }
            public byte Byte19 { get; }
        }
    }
}