﻿using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
using System.Collections.Generic;
using System.Linq;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class MessageBinEntry
    {
        public const int EntryLength = 0x10;

        public MessageBinEntry(IReadOnlyBinaryDataAccessor data)
        {
            var sir0 = new Sir0(data);
            var entryCount1 = sir0.SubHeader.ReadInt32(0);
            var entryCount2 = sir0.SubHeader.ReadInt32(4);
            var entriesOffset = sir0.SubHeader.ReadInt32(8);

            var strings = new Dictionary<int, string>();
            var hashes = new Dictionary<long, int>();
            for (int i = 0; i < entryCount1; i++)
            {
                var entryOffset = entriesOffset + (i * EntryLength);
                var stringOffset = sir0.Data.ReadInt64(entryOffset);
                var hash = sir0.Data.ReadInt32(entryOffset + 8);
                var unknown = sir0.Data.ReadInt32(entryOffset + 0xC);
                if (!strings.ContainsKey(hash)) // script.bin contains duplicate hashes
                {
                    strings.Add(hash, sir0.Data.ReadNullTerminatedUnicodeString(stringOffset));
                    hashes.Add(stringOffset, hash);
                }
            }
            Strings = strings;
            OrderedHashes = hashes.OrderBy(h => h.Key).Select(h => h.Value).ToArray();
        }

        public MessageBinEntry(byte[] data) : this(new BinaryFile(data))
        {
        }

        public IReadOnlyDictionary<int, string> Strings { get; }
        public IReadOnlyList<int> OrderedHashes { get; }
    }
}
