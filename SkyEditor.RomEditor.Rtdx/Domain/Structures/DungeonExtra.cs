using SkyEditor.IO;
using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DungeonIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.dungeon.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public class DungeonExtra
    {
        public const int EntrySize = 0x18;
        public DungeonExtra(byte[] data)
        {
            IReadOnlyBinaryDataAccessor file = new BinaryFile(data);
            var sir0 = new Sir0(data);
            var offset = sir0.SubHeader.ReadInt32(0);
            var count = sir0.SubHeader.ReadInt32(8);
            var entries = new Dictionary<DungeonIndex, DungeonExtraEntry>();
            for (int i = 0; i < count; i++)
            {
                var entry = new DungeonExtraEntry(file, file.Slice(offset + i * EntrySize, EntrySize));
                entries[entry.Index] = entry;
            }
            this.Entries = entries;
        }

        public IReadOnlyDictionary<DungeonIndex, DungeonExtraEntry> Entries { get; }

        /*public Sir0 Build()
        {
            var builder = new Sir0Builder(0x20 + Entries.Count * EntrySize + (Entries.Count + 1) * 8 + Entries.Count + 0x20);
            var pointers = new List<long>();
            foreach (var entry in Entries)
            {
                pointers.Add(builder.Length);
                builder.Write(builder.Length, entry.ToByteArray());
            }
            builder.SubHeaderOffset = builder.Length;
            builder.WriteInt64(builder.Length, Entries.Count);
            foreach (var pointer in pointers)
            {
                builder.WritePointer(builder.Length, pointer);
            }
            return builder.Build();
        }*/
    }

    [DebuggerDisplay("{Index} : {Floors}")]
    public class DungeonExtraEntry
    {
        public DungeonExtraEntry(IReadOnlyBinaryDataAccessor fileAccessor, IReadOnlyBinaryDataAccessor entryAccessor)
        {
            Data = entryAccessor.ReadArray();
            Index = (DungeonIndex) entryAccessor.ReadInt32(0x00);
            Floors = entryAccessor.ReadInt32(0x04);
            DungeonCompletionActions = new List<string>();
            var offset = entryAccessor.ReadInt32(0x08);
            var count = entryAccessor.ReadInt32(0x10);
            var offset2 = fileAccessor.ReadInt32(offset + 0x08);
            for (int i = 0; i < count; i++)
            {
                var str = fileAccessor.ReadNullTerminatedUnicodeString(offset2);
                DungeonCompletionActions.Add(str);
                offset2 += (str.Length + 1) * 2;  // FIXME: get the length of the string in bytes
            }
        }

        public byte[] ToByteArray()
        {
            return Data;
        }

        private byte[] Data { get; }
        public DungeonIndex Index { get; set; }
        public int Floors { get; set; }
        public List<string> DungeonCompletionActions { get; }
    }
}
