using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IDungeonDataInfo
    {
        IDictionary<DungeonIndex, DungeonDataInfo.Entry> Entries { get; }
        byte[] ToByteArray();
    }

    public class DungeonDataInfo : IDungeonDataInfo
    {
        public const int EntrySize = 0x1C;

        public IDictionary<DungeonIndex, Entry> Entries { get; }

        public DungeonDataInfo(byte[] data)
        {
            IReadOnlyBinaryDataAccessor accessor = new BinaryFile(data);
            var entryCount = checked((int)data.Length / EntrySize);
            var entries = new Dictionary<DungeonIndex, Entry>(entryCount);
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add((DungeonIndex)i, new Entry(accessor.Slice(i * EntrySize, EntrySize)));
            }
            this.Entries = entries;
        }

        public DungeonDataInfo()
        {
            Entries = new Dictionary<DungeonIndex, Entry>();
            for (int i = 0; i < (int)DungeonIndex.END; i++)
            {
                Entries.Add((DungeonIndex)i, new Entry());
            }
        }

        public byte[] ToByteArray()
        {
            var file = new BinaryFile(new byte[Entries.Count * EntrySize]);
            long offset = 0;
            foreach (var entry in Entries.ToImmutableSortedDictionary())
            {
                entry.Value.Write(file.Slice(offset, EntrySize));
                offset += EntrySize;
            }
            return file.ReadArray();
        }

        [DebuggerDisplay("DungeonDataInfoEntry: {Index}|{Features}|{NameID}|{Short0A}|{SortKey}|{DungeonBalanceIndex}|{Byte13}|{MaxItems}|{MaxTeammates}|{Byte17}|{Byte18}|{Byte19}")]
        public class Entry
        {
            public Entry()
            { }

            public Entry(IReadOnlyBinaryDataAccessor data)
            {
                Features = (DungeonFeature)data.ReadInt32(0x00);
                Index = data.ReadInt32(0x04);
                NameID = data.ReadInt16(0x08);
                Short0A = data.ReadInt16(0x0A);
                SortKey = data.ReadInt32(0x0C);
                DungeonBalanceIndex = data.ReadByte(0x12);
                Byte13 = data.ReadByte(0x13);
                MaxItems = data.ReadByte(0x14);
                MaxTeammates = data.ReadByte(0x15);
                Byte17 = data.ReadByte(0x17);
                Byte18 = data.ReadByte(0x18);
                Byte19 = data.ReadByte(0x19);
                // All unread bytes are zero
            }

            public void Write(IBinaryDataAccessor data)
            {
                data.WriteInt32(0x00, (int)Features);
                data.WriteInt32(0x04, Index);
                data.WriteInt16(0x08, NameID);
                data.WriteInt16(0x0A, Short0A);
                data.WriteInt32(0x0C, SortKey);
                data.Write(0x12, DungeonBalanceIndex);
                data.Write(0x13, Byte13);
                data.Write(0x14, MaxItems);
                data.Write(0x15, MaxTeammates);
                data.Write(0x17, Byte17);
                data.Write(0x18, Byte18);
                data.Write(0x19, Byte19);
            }

            public DungeonFeature Features { get; set; }
            public int Index { get; set; }
            public short NameID { get; set; }
            public short Short0A { get; set; }
            public int SortKey { get; set; }
            public byte DungeonBalanceIndex { get; set; }

            // This is either 100 or 255.
            // Dungeons with value = 255:
            // - Tiny Woods (index 49)
            // - Thunderwave Cave (index 50)
            // - Rock/Snow Path
            // - Meteor Cave
            // - Makuhita Dojo
            // - Illusory Grotto
            public byte Byte13 { get; set; }
            public byte MaxItems { get; set; }
            public byte MaxTeammates { get; set; }
            public byte Byte17 { get; set; }  // always 32
            public byte Byte18 { get; set; }  // might be related to dungeon_variation_data_info.bin/ent
            public byte Byte19 { get; set; }
        }
    }
}
