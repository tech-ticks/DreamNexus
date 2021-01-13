using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    // Source: https://pastebin.com/84cBGfrc
    // Credit to AntyMew
    public class PokemonDataInfo
    {
        private const int EntrySize = 0xE0;

        public List<PokemonDataInfoEntry> Entries { get; }

        public PokemonDataInfo(IReadOnlyBinaryDataAccessor data)
        {
            var entryCount = checked((int)data.Length / EntrySize);
            var entries = new List<PokemonDataInfoEntry>(entryCount);
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add(new PokemonDataInfoEntry((CreatureIndex)i, data.Slice(i * EntrySize, EntrySize)));
            }
            this.Entries = entries;
        }

        public byte[] ToByteArray()
        {
            var buffer = new byte[EntrySize * Entries.Count];
            for (int i = 0; i < Entries.Count; i++)
            {
                Entries[i].Write(buffer.AsSpan().Slice(i * EntrySize, EntrySize));
            }
            return buffer;
        }

        public PokemonDataInfoEntry GetByPokemonId(CreatureIndex creatureIndex)
        {
            return Entries[(int)creatureIndex];
        }

        public class PokemonDataInfoEntry
        {
            public CreatureIndex Id { get; }

            public byte[] LearnableTMs { get; set; }

            public List<LevelUpMove> LevelupLearnset { get; set; }

            public FeatureFlags Features;

            public ushort Unknown62 { get; set; }

            public short PokedexNumber { get; set; }

            public short Unknown66 { get; set; }

            public short Taxon { get; set; }

            public short Unknown6A { get; set; }
            public short Unknown6C { get; set; }
            public short Unknown6E { get; set; }
            public short Unknown70 { get; set; }
            public short Unknown72 { get; set; }

            public short BaseHitPoints { get; set; }
            public short BaseAttack { get; set; }
            public short BaseSpecialAttack { get; set; }
            public short BaseDefense { get; set; }
            public short BaseSpecialDefense { get; set; }
            public short BaseSpeed { get; set; }

            public short Unknown80 { get; set; }
            public short EvolvesFrom { get; set; }

            public byte ExperienceEntry { get; set; }

            // -2, -2, 0 and 0 (respectively) for the Kangaskhan Child (index 1001, GARUURA_CHILD)
            // 2, 2, 1 and 1 (respectively) for everyone
            public sbyte Unknown8B { get; set; }
            public sbyte Unknown8C { get; set; }
            public sbyte Unknown8D { get; set; }
            public sbyte Unknown8E { get; set; }

            public AbilityIndex Ability1 { get; set; }
            public AbilityIndex Ability2 { get; set; }
            public AbilityIndex HiddenAbility { get; set; }
            public PokemonType Type1 { get; set; }
            public PokemonType Type2 { get; set; }

            // 2 for the Deoxys Speed form (index 495, DEOKISHISU_S)
            // 1 for everyone else
            public byte Unknown95 { get; set; }

            // 100 for Snorlax and Sudowoodo (all variants)
            //   0 for Mega Mewtwo Y
            //  10 for everyone else
            public byte Unknown96 { get; set; }

            public byte Size { get; set; }

            // Set to 1 on several Megas, except:
            //   Beedrill, Pidgeot, Steelix, Slowbro, Kangaskhan, Sceptile,
            //   Swampert, Gallade, Sharpedo, Camerupt, Altaria, Glalie,
            //   Salamence, Metagross, Kyogre, Groudon, Rayquaza
            public byte MegaRelatedProperty { get; set; }

            // Level range for ???
            // - Wild spawns?
            // - Recruits?
            public byte SomeMinimumLevel { get; set; }
            public byte SomeMaximumLevel { get; set; }

            public string RecruitPrereq { get; set; }

            public PokemonDataInfoEntry(CreatureIndex id, IReadOnlyBinaryDataAccessor data)
            {
                this.Id = id;

                LearnableTMs = data.ReadArray(0, 16);

                var levelupLearnset = new List<LevelUpMove>(26);
                for (int i = 0; i < 26; i++)
                {
                    WazaIndex move = (WazaIndex)data.ReadInt16(0x10 + i * sizeof(short));
                    byte level = data.ReadByte(0x44 + i);
                    levelupLearnset.Add(new LevelUpMove(level, move));
                }
                this.LevelupLearnset = levelupLearnset;

                Features = (FeatureFlags)data.ReadUInt16(0x60);
                Unknown62 = data.ReadUInt16(0x62);

                PokedexNumber = data.ReadInt16(0x64);
                Unknown66 = data.ReadInt16(0x66);
                Taxon = data.ReadInt16(0x68);

                Unknown6A = data.ReadInt16(0x6A);
                Unknown6C = data.ReadInt16(0x6C);
                Unknown6E = data.ReadInt16(0x6E);
                Unknown70 = data.ReadInt16(0x70);
                Unknown72 = data.ReadInt16(0x72);

                BaseHitPoints = data.ReadInt16(0x74);
                BaseAttack = data.ReadInt16(0x76);
                BaseSpecialAttack = data.ReadInt16(0x78);
                BaseDefense = data.ReadInt16(0x7A);
                BaseSpecialDefense = data.ReadInt16(0x7C);
                BaseSpeed = data.ReadInt16(0x7E);

                Unknown80 = data.ReadInt16(0x80);
                EvolvesFrom = data.ReadInt16(0x82);

                ExperienceEntry = data.ReadByte(0x84);

                Unknown8B = (sbyte)data.ReadByte(0x8B);
                Unknown8C = (sbyte)data.ReadByte(0x8C);
                Unknown8D = (sbyte)data.ReadByte(0x8D);
                Unknown8E = (sbyte)data.ReadByte(0x8E);

                Ability1 = (AbilityIndex)data.ReadByte(0x90);
                Ability2 = (AbilityIndex)data.ReadByte(0x91);
                HiddenAbility = (AbilityIndex)data.ReadByte(0x92);
                Type1 = (PokemonType)data.ReadByte(0x93);
                Type2 = (PokemonType)data.ReadByte(0x94);

                Unknown95 = data.ReadByte(0x95);
                Unknown96 = data.ReadByte(0x96);
                Size = data.ReadByte(0x97);
                MegaRelatedProperty = data.ReadByte(0x98);
                SomeMinimumLevel = data.ReadByte(0x99);
                SomeMaximumLevel = data.ReadByte(0x9A);

                RecruitPrereq = data.ReadString(0x9B, 69, Encoding.ASCII).TrimEnd('\0');
            }

            public void Write(Span<byte> buffer)
            {
                LearnableTMs.CopyTo(buffer.Slice(0));
                for (int i = 0; i < 26; i++)
                {
                    var move = LevelupLearnset[i];
                    BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x10 + i * sizeof(short)), (short)move.Move);
                    buffer[0x44 + i] = move.Level;
                }
                BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(0x60), (ushort)Features);
                BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(0x62), Unknown62);

                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x64), PokedexNumber);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x66), Unknown66);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x68), Taxon);

                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x6A), Unknown6A);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x6C), Unknown6C);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x6E), Unknown6E);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x70), Unknown70);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x72), Unknown72);

                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x74), BaseHitPoints);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x76), BaseAttack);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x78), BaseSpecialAttack);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x7A), BaseDefense);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x7C), BaseSpecialDefense);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x7E), BaseSpeed);

                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x80), Unknown80);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x82), EvolvesFrom);

                buffer[0x84] = ExperienceEntry;

                buffer[0x8B] = (byte)Unknown8B;
                buffer[0x8C] = (byte)Unknown8C;
                buffer[0x8D] = (byte)Unknown8D;
                buffer[0x8E] = (byte)Unknown8E;

                buffer[0x90] = (byte)Ability1;
                buffer[0x91] = (byte)Ability2;
                buffer[0x92] = (byte)HiddenAbility;
                buffer[0x93] = (byte)Type1;
                buffer[0x94] = (byte)Type2;

                buffer[0x95] = Unknown95;
                buffer[0x96] = Unknown96;
                buffer[0x97] = Size;
                buffer[0x98] = MegaRelatedProperty;
                buffer[0x99] = SomeMinimumLevel;
                buffer[0x9A] = SomeMaximumLevel;

#if NETSTANDARD2_0
                Encoding.ASCII.GetBytes(RecruitPrereq).CopyTo(buffer.Slice(0x9B, 69));
#else
                Encoding.ASCII.GetBytes(RecruitPrereq, buffer.Slice(0x9B, 69));
#endif
            }

            [Flags]
            public enum FeatureFlags : ushort
            {
                Male = 1 << 0,
                Female = 1 << 1,
                NoGender = 1 << 2,  // unsure
                CanWalkOnWater = 1 << 3,
                CanWalkOnMagma = 1 << 4,
                CanPhaseThroughWalls = 1 << 5,
                CanLevitate = 1 << 6,  // Levitate, fly, float, etc.
                CannotMove = 1 << 7,  // Wild Pokémon only
                // Bits 8, 9 and 10 are always zero
                Starter = 1 << 11,
                BaseForm = 1 << 12,  // Not set for Megas or alternate forms
                Unknown13 = 1 << 13,
                // Bit 14 is always zero
                Active = 1 << 15,  // Set to 1 for Pokémon used in RTDX
            }
        }

        public class LevelUpMove
        {
            public LevelUpMove(byte level, WazaIndex move)
            {
                this.Level = level;
                this.Move = move;
            }

            public byte Level { get; set; }
            public WazaIndex Move { get; set; }
        }
    }
}