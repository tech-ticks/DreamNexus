using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

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
                Features = (Feature)data.ReadInt32(0x00);
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

            // TODO: things to check:
            // - whether some of the bits correspond to blocking certain options in the menus
            // - whether they have any relation with the story progression
            // - possible relation with the "give up" option following up with the "start over" question
            [Flags]
            public enum Feature
            {
                FloorDirection = (1 << 0),           // Floor direction (0 = BxxF, 1 = xxF)
                _Bit1 = (1 << 1),                    //   ??  (always 0)
                _Bit2 = (1 << 2),                    //   ??  (always 1)
                _Bit3 = (1 << 3),                    //   ??  (always 0)
                LevelReset = (1 << 4),               // Level reset to 5 upon entry
                _Bit5 = (1 << 5),                    //   ??  (0 on Purity Forest, Wish Cave, Joyous Tower, Illusory Grotto, Makuhita Dojo and the dojo mazes)
                _Bit6 = (1 << 6),                    //   ??  (always 1)
                _Bit7 = (1 << 7),                    //   ??  (always 1)
                _Bit8 = (1 << 8),                    //   ??  (always 0)
                AutoRevive = (1 << 9),               // (possibly) Auto-revive  (0 on Tiny Woods (index 49) and Thunderwave Cave (index 50), Makuhita Dojo and the dojo mazes)
                _Bit10 = (1 << 10),                  //   ??  (0 on Oddity Cave, Fiery/Lightning/Northwind Field and Mt. Faraway)
                _Bit11 = (1 << 11),                  //   ??  (always 1)
                _Bit12 = (1 << 12),                  //   ??  (always 0)
                _Bit13 = (1 << 13),                  //   ??  (always 0)
                _Bit14 = (1 << 14),                  //   ??  (always 0)
                WildPokemonRecruitable = (1 << 15),  // Leader can recruit wild Pokemon
                _Bit16 = (1 << 16),                  //   ??  (0 on both versions of Tiny Woods and Thunderwave Cave, Lapis Cave, Rock/Snow Path, Mt. Blaze, Frosty Forest and Mt. Freeze)
                                                     //   Since Tiny Woods/Thunderwave Cave are early dungeons and Lapis Cave through Mt. Freeze are visited during the fugitive arc,
                                                     //   I suspect this bit is related to not being able to go back to the rescue team base after fainting/giving up.
                Scanning = (1 << 17),                // Permanent Scanning status
                Radar = (1 << 18)                    // Permanent Radar status
                // bits 19+ are always 0
            }

            public Feature Features { get; set; }
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
