using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Rtdx.Infrastructure;
using System.Diagnostics;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public class PokemonGraphicsDatabase : Sir0
    {
        const int entrySize = 0xB0;

        public PokemonGraphicsDatabase(byte[] data) : base(data)
        {
            var indexOffset = BitConverter.ToInt32(data, (int)SubHeaderOffset + 8);
            var entryCount = BitConverter.ToInt32(data, (int)SubHeaderOffset + 16);
            var entries = new List<PokemonGraphicsDatabaseEntry>();
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add(new PokemonGraphicsDatabaseEntry(data, indexOffset + (i * entrySize)));
            }
            this.Entries = entries;
        }

        public IReadOnlyList<PokemonGraphicsDatabaseEntry> Entries { get; }

        [DebuggerDisplay("PokemonGraphicsDatabaseEntry: {String1}|{String2}|{String3}|{String4}|{String5}|{String6}")]
        public class PokemonGraphicsDatabaseEntry
        {
            public PokemonGraphicsDatabaseEntry(byte[] data, int index)
            {
                this.Data = new byte[entrySize];
                Array.Copy(data, index, this.Data, 0, entrySize);

                var accessor = new BinaryFile(data);
                String1 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 0));
                String2 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 8));
                String3 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 16));
                String4 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 24));
                String5 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 32));
                String6 = accessor.ReadNullTerminatedUtf16String(BitConverter.ToInt32(data, index + 40));
            }

            public byte[] Data { get; }

            public string String1 { get; set; }
            public string String2 { get; set; }
            public string String3 { get; set; }
            public string String4 { get; set; }
            public string String5 { get; set; }
            public string String6 { get; set; }
        }
    }
}
