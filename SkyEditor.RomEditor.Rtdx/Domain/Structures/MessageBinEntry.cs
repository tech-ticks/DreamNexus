using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Rtdx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public class MessageBinEntry : Sir0
    {
        public MessageBinEntry(byte[] data, int offset) : base(data, offset)
        {
            var entryCount1 = BitConverter.ToInt32(data, offset + (int)SubHeaderOffset);
            var entryCount2 = BitConverter.ToInt32(data, offset + (int)SubHeaderOffset + 4);
            var entriesOffset = offset + BitConverter.ToInt32(data, offset + (int)SubHeaderOffset + 8);

            var accessor = new BinaryFile(data);
            var strings = new Dictionary<int, string>();
            var hashes = new Dictionary<int, int>();
            for (int i = 0; i < entryCount1; i++)
            {
                var entryOffset = entriesOffset + (i * 0x10);
                var stringOffset = offset + BitConverter.ToInt32(data, entryOffset);
                var hash = BitConverter.ToInt32(data, entryOffset + 8);
                var unknown = BitConverter.ToInt32(data, entryOffset + 0xC);
                strings.Add(hash, accessor.ReadNullTerminatedUtf16String(stringOffset));
                hashes.Add(stringOffset, hash);
            }
            Strings = strings;
            Hashes = hashes.OrderBy(h => h.Key).Select(h => h.Value).ToArray();
        }

        public IReadOnlyDictionary<int, string> Strings { get; }
        public IReadOnlyList<int> Hashes { get; }
    }
}
