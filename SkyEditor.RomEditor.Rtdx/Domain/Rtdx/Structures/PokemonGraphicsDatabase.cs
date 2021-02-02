using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class PokemonGraphicsDatabase
    {
        const int entrySize = 0xB0;

        public PokemonGraphicsDatabase(byte[] data)
        {
            var sir0 = new Sir0(data);
            var indexOffset = BitConverter.ToInt32(data, (int)sir0.SubHeaderOffset + 8);
            var entryCount = BitConverter.ToInt32(data, (int)sir0.SubHeaderOffset + 16);
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

        public Sir0 ToSir0()
        {
            var sir0 = new Sir0Builder(8);

            // Write the strings
            foreach (var entry in Entries)
            {
                sir0.Align(8);
                entry.ModelNamePointer = sir0.Length;
                sir0.WriteNullTerminatedString(sir0.Length, Encoding.Unicode, entry.ModelName);

                sir0.Align(8);
                entry.AnimationNamePointer = sir0.Length;
                sir0.WriteNullTerminatedString(sir0.Length, Encoding.Unicode, entry.AnimationName);

                sir0.Align(8);
                entry.BaseFormModelNamePointer = sir0.Length;
                sir0.WriteNullTerminatedString(sir0.Length, Encoding.Unicode, entry.BaseFormModelName);

                sir0.Align(8);
                entry.PortraitSheetNamePointer = sir0.Length;
                sir0.WriteNullTerminatedString(sir0.Length, Encoding.Unicode, entry.PortraitSheetName);

                sir0.Align(8);
                entry.RescueCampSheetNamePointer = sir0.Length;
                sir0.WriteNullTerminatedString(sir0.Length, Encoding.Unicode, entry.RescueCampSheetName);

                sir0.Align(8);
                entry.RescueCampSheetReverseNamePointer = sir0.Length;
                sir0.WriteNullTerminatedString(sir0.Length, Encoding.Unicode, entry.RescueCampSheetReverseName);
            }

            // Write the data
            sir0.Align(8);
            var entriesSectionStart = sir0.Length;
            foreach (var entry in Entries)
            {
                var entryOffset = sir0.Length;

                sir0.Write(sir0.Length, entry.ToByteArray());

                sir0.MarkPointer(entryOffset + 0);
                sir0.MarkPointer(entryOffset + 8);
                sir0.MarkPointer(entryOffset + 16);
                sir0.MarkPointer(entryOffset + 24);
                sir0.MarkPointer(entryOffset + 32);
                sir0.MarkPointer(entryOffset + 40);
            }

            // Write the content header
            sir0.SubHeaderOffset = sir0.Length;
            sir0.WriteString(sir0.Length, Encoding.ASCII, "PGDB");
            sir0.Align(8);
            sir0.WritePointer(sir0.Length, entriesSectionStart);
            sir0.WriteInt64(sir0.Length, Entries.Count);
            return sir0.Build();
        }

        public byte[] ToByteArray() => ToSir0().Data.ReadArray();

        [DebuggerDisplay("PokemonGraphicsDatabaseEntry: {ModelName}")]
        public class PokemonGraphicsDatabaseEntry
        {
            public PokemonGraphicsDatabaseEntry(IReadOnlyBinaryDataAccessor entryAccessor, IReadOnlyBinaryDataAccessor rawDataAccessor)
            {
                ModelNamePointer = entryAccessor.ReadInt64(0);
                AnimationNamePointer = entryAccessor.ReadInt64(8);
                BaseFormModelNamePointer = entryAccessor.ReadInt64(16);
                PortraitSheetNamePointer = entryAccessor.ReadInt64(24);
                RescueCampSheetNamePointer = entryAccessor.ReadInt64(32);
                RescueCampSheetReverseNamePointer = entryAccessor.ReadInt64(40);

                ModelName = rawDataAccessor.ReadNullTerminatedUnicodeString(ModelNamePointer);
                AnimationName = rawDataAccessor.ReadNullTerminatedUnicodeString(AnimationNamePointer);
                BaseFormModelName = rawDataAccessor.ReadNullTerminatedUnicodeString(BaseFormModelNamePointer);
                PortraitSheetName = rawDataAccessor.ReadNullTerminatedUnicodeString(PortraitSheetNamePointer);
                RescueCampSheetName = rawDataAccessor.ReadNullTerminatedUnicodeString(RescueCampSheetNamePointer);
                RescueCampSheetReverseName = rawDataAccessor.ReadNullTerminatedUnicodeString(RescueCampSheetReverseNamePointer);

                UnkX30 = entryAccessor.ReadSingle(0x30);
                UnkX34 = entryAccessor.ReadSingle(0x34);
                UnkX38 = entryAccessor.ReadSingle(0x38);
                UnkX3C = entryAccessor.ReadSingle(0x3C);

                UnkX40 = entryAccessor.ReadSingle(0x40);
                UnkX44 = entryAccessor.ReadSingle(0x44);
                UnkX48 = entryAccessor.ReadSingle(0x48);
                WalkSpeedDistance = entryAccessor.ReadSingle(0x4C); // Referenced by PokemonDatabase_GetWalkSpeed()

                UnkX50 = entryAccessor.ReadSingle(0x50);
                RunSpeedRatioGround = entryAccessor.ReadSingle(0x54); // Referenced by PokemonDatabase_GetRunRateGround()
                UnkX58 = entryAccessor.ReadSingle(0x58);
                UnkX5C = entryAccessor.ReadSingle(0x5C);

                UnkX60 = entryAccessor.ReadSingle(0x60);
                UnkX64 = entryAccessor.ReadSingle(0x64);
                UnknownBodyType1 = (GraphicsBodySizeType)entryAccessor.ReadInt32(0x68);
                UnknownBodyType2 = (GraphicsBodySizeType)entryAccessor.ReadInt32(0x6C);

                Flags = (PokemonGraphicsDatabaseEntryFlags)entryAccessor.ReadInt32(0x70);
                EnabledPortraits = (EnabledPortraitsFlags)entryAccessor.ReadUInt32(0x74); // Bitmask of enabled portraits
                UnkX78 = entryAccessor.ReadInt32(0x78);
                UnkX7C = entryAccessor.ReadInt32(0x7C);

                UnkX80 = entryAccessor.ReadInt32(0x80);
                UnkX84 = entryAccessor.ReadSingle(0x84);
                UnkX88 = entryAccessor.ReadSingle(0x88);
                UnkX8C = entryAccessor.ReadSingle(0x8C);

                UnkX90 = entryAccessor.ReadSingle(0x90);
                UnkX94 = entryAccessor.ReadSingle(0x94);
                UnkX98 = entryAccessor.ReadSingle(0x98);
                UnkX9C = entryAccessor.ReadSingle(0x9C);

                UnkXA0 = entryAccessor.ReadSingle(0xA0);
                Padding1 = entryAccessor.ReadInt32(0xA4);
                Padding2 = entryAccessor.ReadInt32(0xA8);
                Padding3 = entryAccessor.ReadInt32(0xAC);
            }

            public PokemonGraphicsDatabaseEntry()
            {
                ModelName = "";
                AnimationName = "";
                BaseFormModelName = "";
                PortraitSheetName = "";
                RescueCampSheetName = "";
                RescueCampSheetReverseName = "";
            }

            public byte[] ToByteArray()
            {
                var data = new byte[entrySize];

                using var accessor = new BinaryFile(data);
                accessor.WriteInt64(0, ModelNamePointer);
                accessor.WriteInt64(8, AnimationNamePointer);
                accessor.WriteInt64(16, BaseFormModelNamePointer);
                accessor.WriteInt64(24, PortraitSheetNamePointer);
                accessor.WriteInt64(32, RescueCampSheetNamePointer);
                accessor.WriteInt64(40, RescueCampSheetReverseNamePointer);

                accessor.WriteSingle(0x30, UnkX30);
                accessor.WriteSingle(0x34, UnkX34);
                accessor.WriteSingle(0x38, UnkX38);
                accessor.WriteSingle(0x3C, UnkX3C);

                accessor.WriteSingle(0x40, UnkX40);
                accessor.WriteSingle(0x44, UnkX44);
                accessor.WriteSingle(0x48, UnkX48);
                accessor.WriteSingle(0x4C, WalkSpeedDistance);

                accessor.WriteSingle(0x50, UnkX50);
                accessor.WriteSingle(0x54, RunSpeedRatioGround);
                accessor.WriteSingle(0x58, UnkX58);
                accessor.WriteSingle(0x5C, UnkX5C);

                accessor.WriteSingle(0x60, UnkX60);
                accessor.WriteSingle(0x64, UnkX64);
                accessor.WriteInt32(0x68, (int)UnknownBodyType1);
                accessor.WriteInt32(0x6C, (int)UnknownBodyType2);

                accessor.WriteInt32(0x70, (int)Flags);
                accessor.WriteInt32(0x74, (int)EnabledPortraits);
                accessor.WriteInt32(0x78, UnkX78);
                accessor.WriteInt32(0x7C, UnkX7C);

                accessor.WriteInt32(0x80, UnkX80);
                accessor.WriteSingle(0x84, UnkX84);
                accessor.WriteSingle(0x88, UnkX88);
                accessor.WriteSingle(0x8C, UnkX8C);

                accessor.WriteSingle(0x90, UnkX90);
                accessor.WriteSingle(0x94, UnkX94);
                accessor.WriteSingle(0x98, UnkX98);
                accessor.WriteSingle(0x9C, UnkX9C);

                accessor.WriteSingle(0xA0, UnkXA0);
                accessor.WriteInt32(0xA4, Padding1);
                accessor.WriteInt32(0xA8, Padding2);
                accessor.WriteInt32(0xAC, Padding3);

                return data;
            }

            public long ModelNamePointer { get; set; }
            public long AnimationNamePointer { get; set; }
            public long BaseFormModelNamePointer { get; set; }
            public long PortraitSheetNamePointer { get; set; }
            public long RescueCampSheetNamePointer { get; set; }
            public long RescueCampSheetReverseNamePointer { get; set; }

            public string ModelName { get; set; }
            public string AnimationName { get; set; }
            public string BaseFormModelName { get; set; }
            public string PortraitSheetName { get; set; }
            public string RescueCampSheetName { get; set; }
            public string RescueCampSheetReverseName { get; set; }

            public float UnkX30 { get; set; } // Possibly BaseScale
            public float UnkX34 { get; set; } // Possibly DungeonBaseScale
            public float UnkX38 { get; set; } // Possibly DotOffsetX
            public float UnkX3C { get; set; } // Possibly DotOffsetY

            public float UnkX40 { get; set; }
            public float UnkX44 { get; set; }
            public float UnkX48 { get; set; }
            public float WalkSpeedDistance { get; set; }

            public float UnkX50 { get; set; } // Possibly WalkSpeedDistanceGround
            public float RunSpeedRatioGround { get; set; }
            public float UnkX58 { get; set; }
            public float UnkX5C { get; set; }

            public float UnkX60 { get; set; }
            public float UnkX64 { get; set; }
            public GraphicsBodySizeType UnknownBodyType1 { get; set; }
            public GraphicsBodySizeType UnknownBodyType2 { get; set; }

            public PokemonGraphicsDatabaseEntryFlags Flags { get; set; }
            public EnabledPortraitsFlags EnabledPortraits { get; set; }
            public int UnkX78 { get; set; }
            public int UnkX7C { get; set; }

            public int UnkX80 { get; set; }
            public float UnkX84 { get; set; }
            public float UnkX88 { get; set; }
            public float UnkX8C { get; set; }

            public float UnkX90 { get; set; }
            public float UnkX94 { get; set; }
            public float UnkX98 { get; set; }
            public float UnkX9C { get; set; }


            public float UnkXA0 { get; set; }
            public int Padding1 { get; set; }
            public int Padding2 { get; set; }
            public int Padding3 { get; set; }

            [Flags]
            public enum PokemonGraphicsDatabaseEntryFlags
            {
                IsBig = 1, // Bit 0
                Bit1 = 2,
                Bit2 = 4,
                Bit3 = 8,
                Bit4 = 16,
                Bit5 = 32,
                Bit6 = 64,
                Bit7 = 128,
                Bit8 = 256,
                Bit9 = 512,
                Bit10 = 1024,
                Bit11 = 2048,
                IsIgnoreBaisokuMotion = 4096, // Bit 12
            }

            [Flags]
            public enum EnabledPortraitsFlags : uint
            {
                Normal = 1,
                Happy = 2,
                Pain = 4,
                Angry = 8,
                Think = 16,
                Sad = 32,
                Weep = 64,
                Shout = 128,
                Tears = 256,
                Decide = 512,
                Gladness = 1024,
                Emotion = 2048,
                Surprise = 4096,
                Faint = 8192,
                Relief = 16384,
                CatchBreath = 32768,
                Special01 = 65536,
                Special02 = 131072,
                Special03 = 262144,
                Special04 = 524288,
                Special05 = 1048576,
                Special06 = 2097152,
                Special07 = 4194304,
                Special08 = 8388608,
                Special09 = 16777216,
                Special10 = 33554432,
                Special11 = 67108864,
                Special12 = 134217728,
                Special13 = 268435456,
                Special14 = 536870912,
                Special15 = 1073741824,
                Special16 = 2147483648
            }
        }
    }
}
