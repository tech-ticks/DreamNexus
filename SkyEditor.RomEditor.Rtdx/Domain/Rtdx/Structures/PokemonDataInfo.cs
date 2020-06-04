using SkyEditor.IO.Binary;
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

            public long Unknown00 { get; set; }
            public long Unknown08 { get; set; }

            public IReadOnlyList<LevelUpMove> LevelupLearnset { get; }

            public short Unknown5E { get; }
            public int Unknown60 { get; }

            public short PokedexNumber { get; set; }

            public short Unknown66 { get; set; }

            public short Taxon { get; set; }

            public short Unknown6A { get; set; }
            public int Unknown6C { get; set; }
            public int Unknown70 { get; }

            public short BaseHitPoints { get; set; }
            public short BaseAttack { get; set; }
            public short BaseDefense { get; set; }
            public short BaseSpecialAttack { get; set; }
            public short BaseSpecialDefense { get; set; }
            public short BaseSpeed { get; set; }

            public int Unknown80 { get; }

            public int ExperienceEntry { get; set; }

            public long Unknown88 { get; }

            public AbilityIndex Ability1 { get; set; }
            public AbilityIndex Ability2 { get; set; }
            public AbilityIndex HiddenAbility { get; set; }
            public PokemonType Type1 { get; set; }
            public PokemonType Type2 { get; set; }

            public byte Unknown95 { get; set; }
            public byte Unknown96 { get; set; }
            public byte Unknown97 { get; set; }
            public byte Unknown98 { get; set; }
            public byte Unknown99 { get; set; }
            public byte Unknown9A { get; set; }

            public string RecruitPrereq { get; set; }

            public PokemonDataInfoEntry(CreatureIndex id, IReadOnlyBinaryDataAccessor data)
            {
                this.Id = id;

                Unknown00 = data.ReadInt64(0);
                Unknown08 = data.ReadInt64(8);

                var levelupLearnset = new List<LevelUpMove>(26);
                for (int i = 0; i < 26; i++)
                {
                    WazaIndex move = (WazaIndex)data.ReadInt16(0x10 + i * sizeof(short));
                    byte level = data.ReadByte(0x44 + i);
                    levelupLearnset.Add(new LevelUpMove(level, move));
                }
                this.LevelupLearnset = levelupLearnset;

                Unknown5E = data.ReadInt16(0x5E);
                Unknown60 = data.ReadInt32(0x60);

                PokedexNumber = data.ReadInt16(0x64);
                Unknown66 = data.ReadInt16(0x66);
                Taxon = data.ReadInt16(0x68);

                Unknown6A = data.ReadInt16(0x6A);
                Unknown6C = data.ReadInt32(0x6C);
                Unknown70 = data.ReadInt32(0x70);

                BaseHitPoints = data.ReadInt16(0x74);
                BaseAttack = data.ReadInt16(0x76);
                BaseDefense = data.ReadInt16(0x78);
                BaseSpecialAttack = data.ReadInt16(0x7A);
                BaseSpecialDefense = data.ReadInt16(0x7C);
                BaseSpeed = data.ReadInt16(0x7E);

                Unknown80 = data.ReadInt32(0x80);

                ExperienceEntry = data.ReadInt32(0x84);

                Unknown88 = data.ReadInt64(0x88);

                Ability1 = (AbilityIndex)data.ReadByte(0x90);
                Ability2 = (AbilityIndex)data.ReadByte(0x91);
                HiddenAbility = (AbilityIndex)data.ReadByte(0x92);
                Type1 = (PokemonType)data.ReadByte(0x93);
                Type2 = (PokemonType)data.ReadByte(0x94);

                Unknown95 = data.ReadByte(0x95);
                Unknown96 = data.ReadByte(0x96);
                Unknown97 = data.ReadByte(0x97);
                Unknown98 = data.ReadByte(0x98);
                Unknown99 = data.ReadByte(0x99);
                Unknown9A = data.ReadByte(0x9A);

                RecruitPrereq = data.ReadString(0x9B, 69, Encoding.ASCII);
            }

            public void Write(Span<byte> buffer)
            {
                BinaryPrimitives.WriteInt64LittleEndian(buffer, Unknown00);
                BinaryPrimitives.WriteInt64LittleEndian(buffer.Slice(8), Unknown08);
                for (int i = 0; i < 26; i++)
                {
                    var move = LevelupLearnset[i];
                    BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x10 + i * sizeof(short)), (short)move.Move);
                    buffer[0x44 + i] = move.Level;
                }
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x5E), Unknown5E);
                BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(0x60), Unknown60);

                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x64), PokedexNumber);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x66), Unknown66);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x68), Taxon);

                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x6A), Unknown6A);
                BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(0x6C), Unknown6C);
                BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(0x70), Unknown70);

                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x74), BaseHitPoints);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x76), BaseAttack);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x78), BaseDefense);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x7A), BaseSpecialAttack);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x7C), BaseSpecialDefense);
                BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(0x7E), BaseSpeed);

                BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(0x80), Unknown80);

                BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(0x84), ExperienceEntry);

                BinaryPrimitives.WriteInt64LittleEndian(buffer.Slice(0x88), Unknown88);

                buffer[0x90] = (byte)Ability1;
                buffer[0x91] = (byte)Ability2;
                buffer[0x92] = (byte)HiddenAbility;
                buffer[0x93] = (byte)Type1;
                buffer[0x94] = (byte)Type2;

                buffer[0x95] = Unknown95;
                buffer[0x96] = Unknown96;
                buffer[0x97] = Unknown97;
                buffer[0x98] = Unknown98;
                buffer[0x99] = Unknown99;
                buffer[0x9A] = Unknown9A;

#if NETSTANDARD2_0
                Encoding.ASCII.GetBytes(RecruitPrereq).CopyTo(buffer.Slice(0x9B, 69));
#else
                Encoding.ASCII.GetBytes(RecruitPrereq, buffer.Slice(0x9B, 69));
#endif
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
}