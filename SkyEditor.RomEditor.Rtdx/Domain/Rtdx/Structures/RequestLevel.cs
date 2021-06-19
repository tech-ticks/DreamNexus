using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System.Collections.Generic;
using System.IO;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class RequestLevel
    {
        public RequestLevel(byte[] data)
        {
            IReadOnlyBinaryDataAccessor file = new BinaryFile(data);
            var sir0 = new Sir0(data);
            var count = sir0.SubHeader.ReadNextInt64();
            Entries = new Dictionary<DungeonIndex, Entry>();

            for (int i = 0; i < count; i++)
            {
                var mainEntryPointer = sir0.SubHeader.ReadNextInt64();
                var pointer2 = sir0.SubHeader.ReadNextInt64();

                // pointer2 points to another location before its own address
                var data2Count = sir0.Data.ReadInt64(pointer2);
                var data2Pointer = sir0.Data.ReadInt64(pointer2 + 8);

                var entry1Data = sir0.Data.Slice(mainEntryPointer, sir0.Data.Length - mainEntryPointer);
                var entry2Data = sir0.Data.Slice(data2Pointer, data2Pointer * data2Count * sizeof(long));

                var unkEntry2 = new List<long>();
                for (int j = 0; j < data2Count; j++)
                {
                    unkEntry2.Add(entry2Data.ReadInt64(j * sizeof(long)));
                }

                Entries[(DungeonIndex) i] = new Entry
                {
                    MainEntry = new MainEntry(entry1Data),
                    UnkEntry2 = unkEntry2,
                };
            }
        }

        public RequestLevel()
        {
            Entries = new Dictionary<DungeonIndex, Entry>();
            for (var index = DungeonIndex.D001; index <= DungeonIndex.D100; index++)
            {
                Entries[index] = new Entry
                {
                    MainEntry = new MainEntry(index),
                    UnkEntry2 = new List<long>()
                };
            }
        }

        public Dictionary<DungeonIndex, Entry> Entries { get; }

        public Sir0 ToSir0()
        {
            var sir0 = new Sir0Builder(8);
            var mainEntryPointers = new List<long>();
            var entry2Pointers = new List<long>();

            // Write main entries
            foreach (var entry in Entries.Values)
            {
                sir0.Align(16);
                mainEntryPointers.Add(sir0.Length);
                entry.MainEntry!.Write(sir0);
            }

            sir0.Align(8);

            // Write unknown entry 2
            foreach (var entry in Entries.Values)
            {
                sir0.Align(8);
                long subPointer = sir0.Length;
                foreach (var unkValue in entry.UnkEntry2!)
                {
                    sir0.WriteInt64(sir0.Length, unkValue); // or writepointer?
                }

                sir0.Align(16);
                entry2Pointers.Add(sir0.Length);
                if (entry.UnkEntry2!.Count > 0)
                {
                    sir0.WriteInt64(sir0.Length, entry.UnkEntry2!.Count);
                    sir0.WritePointer(sir0.Length, subPointer);
                }
                else
                {
                    sir0.WriteInt64(sir0.Length, 0);
                }
            }

            sir0.SubHeaderOffset = sir0.Length;
            sir0.WriteInt64(sir0.Length, Entries.Count);

            for (int i = 0; i < Entries.Count; i++)
            {
                sir0.WritePointer(sir0.Length, mainEntryPointers[i]);
                sir0.WritePointer(sir0.Length, entry2Pointers[i]);
            }

            return sir0.Build();
        }

        public byte[] ToByteArray() => ToSir0().Data.ReadArray();

        public class Entry
        {
            public MainEntry? MainEntry { get; set; }
            public List<long>? UnkEntry2 { get; set; }
        }

        public class MainEntry
        {
            public const int FloorDataSize = 16;

            public short DungeonIndex { get; set; }

            /// Number of "real" floors that can be explored.
            /// Same as DungeonExtra.Floors.
            public short AccessibleFloorCount { get; set; }

            /// Always TotalFloorCount - 3 on valid entries
            public short Unk1 { get; set; }

            /// Matches the number of floors in DungeonBalance, including "invalid" floors
            public short TotalFloorCount { get; set; }
            public List<FloorData> FloorData { get; set; } = new List<FloorData>();

            public MainEntry(DungeonIndex index)
            {
                DungeonIndex = (short) index;
            }

            public MainEntry(IReadOnlyBinaryDataAccessor accessor)
            {
                DungeonIndex = accessor.ReadInt16(0);
                AccessibleFloorCount = accessor.ReadInt16(2);
                Unk1 = accessor.ReadInt16(4);
                TotalFloorCount = accessor.ReadInt16(6);

                accessor.Position = 8;
                for (int i = 0; i < TotalFloorCount; i++)
                {
                    FloorData.Add(new FloorData
                    {
                        NameID = accessor.ReadNextInt16(),
                        FloorIndex = accessor.ReadNextInt16(),
                        Short4 = accessor.ReadNextInt16(),
                        Short6 = accessor.ReadNextInt16(),
                        Short8 = accessor.ReadNextInt16(),
                        IsBossFloor = accessor.ReadNextInt16(),
                    });

                    // Remaining four bytes are zero padding
                    accessor.ReadNextInt32();
                }
            }

            public void Write(Sir0Builder sir0)
            {
                sir0.WriteInt16(sir0.Length, DungeonIndex);
                sir0.WriteInt16(sir0.Length, AccessibleFloorCount);
                sir0.WriteInt16(sir0.Length, Unk1);
                sir0.WriteInt16(sir0.Length, TotalFloorCount);

                foreach (var floorData in FloorData)
                {
                    sir0.WriteInt16(sir0.Length, floorData.NameID);
                    sir0.WriteInt16(sir0.Length, floorData.FloorIndex);
                    sir0.WriteInt16(sir0.Length, floorData.Short4);
                    sir0.WriteInt16(sir0.Length, floorData.Short6);
                    sir0.WriteInt16(sir0.Length, floorData.Short8);
                    sir0.WriteInt16(sir0.Length, floorData.IsBossFloor);
                    sir0.WritePadding(sir0.Length, 4);
                }
            }
        }

        public class FloorData
        {
            // Same as DungeonBalance NameID
            public short NameID { get; set; }

            /// Same as DungeonBalance index
            public short FloorIndex { get; set; }

            /// Same as DungeonBalance Short02
            public short Short4 { get; set; }

            /// Same as DungeonBalance Short26
            public short Short6 { get; set; }

            /// Same as DungeonBalance Short28
            public short Short8 { get; set; }
            public short IsBossFloor { get; set; }
        }
    }
}
