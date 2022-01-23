using AssetStudio;
using SkyEditor.IO.Binary;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IExperience
    {
        IList<Experience.ExperienceEntry> Entries { get; }
        (byte[] bin, byte[] ent) Build();
    }

    public class Experience : IExperience
    {
        public IList<ExperienceEntry> Entries { get; }

        public Experience(byte[] data, byte[] entryList) : this(new BinaryFile(data), new BinaryFile(entryList))
        {

        }

        public Experience(IReadOnlyBinaryDataAccessor data, IReadOnlyBinaryDataAccessor entryList)
        {
            var entryCount = checked((int)entryList.Length / sizeof(int));
            var entries = new List<ExperienceEntry>(entryCount);
            for (int i = 0; i < entryCount - 1; i++)
            {
                var entryOffset = entryList.ReadInt32(i * sizeof(int));
                var entryEnd = entryList.ReadInt32((i + 1) * sizeof(int));
                entries.Add(new ExperienceEntry(data.Slice(entryOffset, entryEnd - entryOffset)));
            }
            this.Entries = entries;
        }

        public (byte[] bin, byte[] ent) Build()
        {
            MemoryStream bin = new MemoryStream();
            var entryPointers = new List<int>();

            // Build the .bin file data
            entryPointers.Add(0);
            foreach (var entry in Entries)
            {
                // Write data to .bin and the pointer to .ent
                // Align data to 16 bytes
                var binData = entry.ToByteArray();
                bin.Write(binData, 0, binData.Length);
                var paddingLength = 16 - (bin.Length % 16);
                if (paddingLength != 16)
                {
                    bin.SetLength(bin.Length + paddingLength);
                    bin.Position = bin.Length;
                }
                entryPointers.Add((int)bin.Position);
            }

            // Build the .ent file data
            var ent = new byte[entryPointers.Count * sizeof(int)];
            for (int i = 0; i < entryPointers.Count; i++)
            {
                BinaryPrimitives.WriteInt32LittleEndian(ent.AsSpan().Slice(i * sizeof(int)), entryPointers[i]);
            }

            return (bin.ToArray(), ent);
        }

        public class ExperienceEntry
        {
            private const int EntrySize = 0x0C;

            public IList<Level> Levels { get; }

            public ExperienceEntry(IReadOnlyBinaryDataAccessor data)
            {
                var levelCount = checked((int)data.Length / EntrySize);
                var levels = new List<Level>(levelCount);
                for (int i = 0; i < levelCount; i++)
                    levels.Add(new Level(data.Slice(i * EntrySize, EntrySize)));
                this.Levels = levels;
            }

            public class Level
            {
                public int MinimumExperience { get; set; }
                public byte HitPointsGained { get; set; }
                public byte AttackGained { get; set; }
                public byte SpecialAttackGained { get; set; }
                public byte DefenseGained { get; set; }
                public byte SpecialDefenseGained { get; set; }
                public byte SpeedGained { get; set; }
                public byte LevelsGained { get; set; }

                public Level()
                {
                }

                public Level(IReadOnlyBinaryDataAccessor data)
                {
                    MinimumExperience = data.ReadInt32(0x0);
                    HitPointsGained = data.ReadByte(0x4);
                    AttackGained = data.ReadByte(0x5);
                    SpecialAttackGained = data.ReadByte(0x6);
                    DefenseGained = data.ReadByte(0x7);
                    SpecialDefenseGained = data.ReadByte(0x8);
                    SpeedGained = data.ReadByte(0x9);
                    LevelsGained = data.ReadByte(0xA);
                }
            }

            public byte[] ToByteArray()
            {
                var data = new byte[EntrySize * Levels.Count];
                for (var i = 0; i < Levels.Count; i++)
                {
                    var level = Levels[i];
                    var slice = data.AsSpan().Slice(i * EntrySize);
                    BinaryPrimitives.WriteInt32LittleEndian(slice.Slice(0x0), level.MinimumExperience);
                    slice[0x4] = level.HitPointsGained;
                    slice[0x5] = level.AttackGained;
                    slice[0x6] = level.SpecialAttackGained;
                    slice[0x7] = level.DefenseGained;
                    slice[0x8] = level.SpecialDefenseGained;
                    slice[0x9] = level.SpeedGained;
                    slice[0xA] = level.LevelsGained;
                }
                return data;
            }
        }
    }
}
