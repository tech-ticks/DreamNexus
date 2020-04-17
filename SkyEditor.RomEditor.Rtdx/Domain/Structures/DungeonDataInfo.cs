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

        [DebuggerDisplay("DungeonDataInfoEntry: {Index}|{Flags}|{Short08}|{Short0A}|{SortKey}|{DungeonBalanceIndex}|{Byte13}|{MaxItems}|{MaxTeammates}|{Byte17}|{Byte18}|{Byte19}")]
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

            // TODO: things to check:
            // - whether some of the bits correspond to blocking certain options in the menus
            // - whether they have any relation with the story progression
            // - possible relation with the "give up" option following up with the "start over" question
            //
            // bit  0: Floor direction (0 = BxxF, 1 = xxF)
            // bit  1:   ??  (always 0)
            // bit  2:   ??  (always 1)
            // bit  3:   ??  (always 0)
            // bit  4: Level reset to 5 upon entry
            // bit  5:   ??  (0 on Purity Forest, Wish Cave, Joyous Tower, Illusory Grotto, Makuhita Dojo and the dojo mazes)
            // bit  6:   ??  (always 1)
            // bit  7:   ??  (always 1)
            // bit  8:   ??  (always 0)
            // bit  9: (possibly) Auto-revive  (0 on Tiny Woods (index 49) and Thunderwave Cave (index 50), Makuhita Dojo and the dojo mazes)
            // bit 10:   ??  (0 on Oddity Cave, Fiery/Lightning/Northwind Field and Mt. Faraway)
            // bit 11:   ??  (always 1)
            // bit 12:   ??  (always 0)
            // bit 13:   ??  (always 0)
            // bit 14:   ??  (always 0)
            // bit 15: Can recruit wild Pokemon
            // bit 16:   ??  (0 on both versions of Tiny Woods and Thunderwave Cave, Lapis Cave, Rock/Snow Path, Mt. Blaze, Frosty Forest and Mt. Freeze)
            //             Since Tiny Woods/Thunderwave Cave are early dungeons and Lapis Cave through Mt. Freeze are visited during the fugitive arc,
            //             I suspect this bit is related to not being able to go back to the rescue team base after fainting/giving up.
            // bit 17: Permanent Scanning status
            // bit 18: Permanent Radar status
            // bits 19+:   ??  (always 0)
            public int Flags { get; }
            public int Index { get; }
            public short Short08 { get; }
            public short Short0A { get; }
            public int SortKey { get; }
            public byte DungeonBalanceIndex { get; }

            // This is either 0x64 or 0xff.
            // Dungeons with 0xff:
            // - Tiny Woods (index 49)
            // - Thunderwave Cave (index 50)
            // - Rock/Snow Path
            // - Meteor Cave
            // - Makuhita Dojo
            // - Illusory Grotto
            public byte Byte13 { get; }
            public byte MaxItems { get; }
            public byte MaxTeammates { get; }
            public byte Byte17 { get; }  // always 0x20
            public byte Byte18 { get; }
            public byte Byte19 { get; }
        }
    }
}