using SkyEditor.IO;
using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DungeonIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.dungeon.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public class DungeonExtra
    {
        public DungeonExtra(byte[] data)
        {
            IReadOnlyBinaryDataAccessor file = new BinaryFile(data);
            var sir0 = new Sir0(data);
            var offset = sir0.SubHeader.ReadInt32(0);
            var count = sir0.SubHeader.ReadInt32(8);
            var entries = new Dictionary<DungeonIndex, DungeonExtraEntry>();
            for (int i = 0; i < count; i++)
            {
                var entry = new DungeonExtraEntry(file, file.Slice(offset + i * DungeonExtraEntry.EntrySize, DungeonExtraEntry.EntrySize));
                entries[entry.Index] = entry;
            }
            this.Entries = entries;
        }

        public DungeonExtra()
        {
            Entries = new Dictionary<DungeonIndex, DungeonExtraEntry>();
            for (var index = DungeonIndex.D001; index <= DungeonIndex.D100; index++)
            {
                Entries[index] = new DungeonExtraEntry(index);
            }
        }

        public Dictionary<DungeonIndex, DungeonExtraEntry> Entries { get; }

        public Sir0 ToSir0()
        {
            var sir0 = new Sir0Builder();

            void align(int length)
            {
                var paddingLength = length - (sir0.Length % length);
                if (paddingLength != length)
                {
                    sir0.WritePadding(sir0.Length, paddingLength);
                }
            }

            var sortedEntries = Entries.ToImmutableSortedDictionary();

            // Write the strings
            foreach (var entry in sortedEntries.Values)
            {
                align(8);
                foreach (var evt in entry.DungeonEvents)
                {
                    evt.NamePointer = sir0.Length;
                    sir0.WriteNullTerminatedString(sir0.Length, Encoding.Unicode, evt.Name);
                }
            }

            // Write the events
            align(8);
            foreach (var entry in sortedEntries.Values)
            {
                entry.DungeonEventsPointer = sir0.Length;
                foreach (var evt in entry.DungeonEvents)
                {
                    sir0.WriteInt32(sir0.Length, evt.Floor);
                    align(8);
                    sir0.MarkPointer(sir0.Length);
                    sir0.WriteInt64(sir0.Length, evt.NamePointer);
                }
            }

            // Write the entries
            align(8);
            var entryListOffset = sir0.Length;
            foreach (var entry in sortedEntries.Values)
            {
                var entryOffset = sir0.Length;
                sir0.Write(sir0.Length, entry.ToByteArray());
                sir0.MarkPointer(entryOffset + 0x08);
            }

            // Write the content header
            sir0.SubHeaderOffset = sir0.Length;
            sir0.WritePointer(sir0.Length, entryListOffset);
            sir0.WriteInt64(sir0.Length, Entries.Count);
            return sir0.Build();
        }

        public byte[] ToByteArray() => ToSir0().Data.ReadArray();

        [DebuggerDisplay("{Index} : {Floors}")]
        public class DungeonExtraEntry
        {
            public const int EntrySize = 0x18;

            public DungeonExtraEntry(DungeonIndex index)
            {
                Index = index;
                DungeonEvents = new DungeonEvent[0];
            }

            public DungeonExtraEntry(IReadOnlyBinaryDataAccessor fileAccessor, IReadOnlyBinaryDataAccessor entryAccessor)
            {
                Index = (DungeonIndex)entryAccessor.ReadInt32(0x00);
                Floors = entryAccessor.ReadInt32(0x04);
                var offset = entryAccessor.ReadInt32(0x08);
                var count = entryAccessor.ReadInt32(0x10);
                DungeonEvents = new DungeonEvent[count];
                for (int i = 0; i < count; i++)
                {
                    var eventNameOffset = fileAccessor.ReadInt32(offset + i * 16 + 0x08);
                    DungeonEvents[i] = new DungeonEvent()
                    {
                        Floor = fileAccessor.ReadInt32(offset + i * 16),
                        Name = fileAccessor.ReadNullTerminatedUnicodeString(eventNameOffset)
                    };
                }
            }

            public byte[] ToByteArray()
            {
                var data = new byte[EntrySize];

                using var accessor = new BinaryFile(data);
                accessor.WriteInt32(0x00, (int)Index);
                accessor.WriteInt32(0x04, Floors);
                accessor.WriteInt64(0x08, DungeonEventsPointer);
                accessor.WriteInt32(0x10, DungeonEvents.Length);

                return data;
            }

            [DebuggerDisplay("{Floor} : {Name}")]
            public class DungeonEvent
            {
                public DungeonEvent()
                {
                    Name = "";
                }

                public int Floor { get; set; }
                public string Name { get; set; }

                public long NamePointer { get; set; }
            }

            public DungeonIndex Index { get; set; }
            public int Floors { get; set; }
            public DungeonEvent[] DungeonEvents { get; set; }

            public long DungeonEventsPointer { get; set; }
        }
    }
}
