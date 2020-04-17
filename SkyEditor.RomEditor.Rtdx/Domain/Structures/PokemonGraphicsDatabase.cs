using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Rtdx.Reverse.Const;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
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
                entries.Add(new PokemonGraphicsDatabaseEntry(data, indexOffset + (i * entrySize)));
            }
            this.Entries = entries;
        }

        public IReadOnlyList<PokemonGraphicsDatabaseEntry> Entries { get; }

        [DebuggerDisplay("PokemonGraphicsDatabaseEntry: {ModelName}")]
        public class PokemonGraphicsDatabaseEntry
        {
            public PokemonGraphicsDatabaseEntry(byte[] data, int index)
            {
                this.Data = new byte[entrySize];
                Array.Copy(data, index, this.Data, 0, entrySize);

                var accessor = new BinaryFile(data);
                ModelName = accessor.ReadNullTerminatedUnicodeString(BitConverter.ToInt32(data, index + 0));
                AnimationName = accessor.ReadNullTerminatedUnicodeString(BitConverter.ToInt32(data, index + 8));
                BaseFormModelName = accessor.ReadNullTerminatedUnicodeString(BitConverter.ToInt32(data, index + 16));
                PortraitSheetName = accessor.ReadNullTerminatedUnicodeString(BitConverter.ToInt32(data, index + 24));
                RescueCampSheetName = accessor.ReadNullTerminatedUnicodeString(BitConverter.ToInt32(data, index + 32));
                RescueCampSheetReverseName = accessor.ReadNullTerminatedUnicodeString(BitConverter.ToInt32(data, index + 40));

                UnkX30 = accessor.ReadSingle(index + 0x30);
                UnkX34 = accessor.ReadSingle(index + 0x34);
                UnkX38 = accessor.ReadSingle(index + 0x38);
                UnkX3C = accessor.ReadSingle(index + 0x3C);

                UnkX40 = accessor.ReadSingle(index + 0x40);
                UnkX44 = accessor.ReadSingle(index + 0x44);
                UnkX48 = accessor.ReadSingle(index + 0x48);
                WalkSpeedDistance = accessor.ReadSingle(index + 0x4C); // Referenced by PokemonDatabase_GetWalkSpeed()

                UnkX50 = accessor.ReadSingle(index + 0x50);
                RunSpeedRatioGround = accessor.ReadSingle(index + 0x54); // Referenced by PokemonDatabase_GetRunRateGround()
                UnkX58 = accessor.ReadInt32(index + 0x58);
                UnkX5C = accessor.ReadInt32(index + 0x5C);

                UnkX60 = accessor.ReadSingle(index + 0x60);
                UnkX64 = accessor.ReadSingle(index + 0x64);
                UnknownBodyType1 = (GraphicsBodySizeType)accessor.ReadInt32(index + 0x68);
                UnknownBodyType2 = (GraphicsBodySizeType)accessor.ReadInt32(index + 0x6C);

                Flags = (PokemonGraphicsDatabaseEntryFlags)accessor.ReadInt32(index + 0x70);
                EnabledPortraits = (EnabledPortraitsFlags)accessor.ReadUInt32(index + 0x74); // Bitmask of enabled portraits
                UnkX78 = accessor.ReadInt32(index + 0x78);
                UnkX7C = accessor.ReadInt32(index + 0x7C);

                UnkX80 = accessor.ReadInt32(index + 0x80);
                UnkX84 = accessor.ReadSingle(index + 0x84);
                UnkX88 = accessor.ReadSingle(index + 0x88);
                UnkX8C = accessor.ReadSingle(index + 0x8C);

                UnkX90 = accessor.ReadSingle(index + 0x90);
                UnkX94 = accessor.ReadSingle(index + 0x94);
                UnkX98 = accessor.ReadSingle(index + 0x98);
                UnkX9C = accessor.ReadSingle(index + 0x9C);

                UnkXA0 = accessor.ReadSingle(index + 0xA0);
                Padding1 = accessor.ReadInt32(index + 0xA4);
                Padding2 = accessor.ReadInt32(index + 0xA8);
                Padding3 = accessor.ReadInt32(index + 0xAC);
            }

            public byte[] Data { get; }

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
            public int UnkX58 { get; set; }
            public int UnkX5C { get; set; }

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
