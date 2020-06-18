using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Psmd.Structures
{
    public class PokemonGraphicsDatabase
    {
        const int entrySize = 0x54;

        public PokemonGraphicsDatabase(byte[] data)
        {
            var sir0 = new Sir0(data);
            var indexOffset = BitConverter.ToInt32(data, (int)sir0.SubHeaderOffset + 4);
            var entryCount = BitConverter.ToInt32(data, (int)sir0.SubHeaderOffset + 8);
            var entries = new List<PokemonGraphicsDatabaseEntry>();
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add(new PokemonGraphicsDatabaseEntry(sir0.Data.Slice(indexOffset + (i * entrySize), entrySize), sir0.Data));
            }
            this.Entries = entries;
        }

        public PokemonGraphicsDatabase()
        {
            Entries = new List<PokemonGraphicsDatabaseEntry>();
        }

        public List<PokemonGraphicsDatabaseEntry> Entries { get; }

        [DebuggerDisplay("PokemonGraphicsDatabaseEntry: {PrimaryBgrsFilename}|{SecondaryBgrsFilename}|{ActorName}")]
        public class PokemonGraphicsDatabaseEntry
        {
            public PokemonGraphicsDatabaseEntry(IReadOnlyBinaryDataAccessor entryAccessor, IReadOnlyBinaryDataAccessor rawDataAccessor)
            {
                BgrsFilenamePointer = entryAccessor.ReadInt32(0);
                SecondaryBgrsFilenamePointer = entryAccessor.ReadInt32(4);
                PortraitNamePointer = entryAccessor.ReadInt32(8);

                BgrsFilename = rawDataAccessor.ReadNullTerminatedUnicodeString(BgrsFilenamePointer);
                SecondaryBgrsFilename = rawDataAccessor.ReadNullTerminatedUnicodeString(SecondaryBgrsFilenamePointer);
                PortraitName = rawDataAccessor.ReadNullTerminatedUnicodeString(PortraitNamePointer);

                UnknownData = entryAccessor.ReadArray(0xC, entrySize - 0xC);
            }

            public int BgrsFilenamePointer { get; set; }
            public int SecondaryBgrsFilenamePointer { get; set; }
            public int PortraitNamePointer { get; set; }

            public string BgrsFilename { get; set; }
            public string SecondaryBgrsFilename { get; set; }
            public string PortraitName { get; set; }

            private byte[] UnknownData { get; set; }
        }
    }
}
