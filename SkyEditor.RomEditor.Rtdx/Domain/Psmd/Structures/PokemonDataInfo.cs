using SkyEditor.RomEditor.Domain.Psmd.Constants;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Domain.Psmd.Structures
{
    public class PokemonDataInfo
    {
        private const int EntrySize = 0x98;

        public PokemonDataInfo(ReadOnlySpan<byte> data)
        {
            var entryCount = checked((int)data.Length / EntrySize);

            Entries = new List<PokemonDataInfoEntry>();
            for (var i = 0; i < entryCount; i++)
            {
                Entries.Add(new PokemonDataInfoEntry((CreatureIndex)i, data.Slice(i * EntrySize, EntrySize)));
            }
        }

        public List<PokemonDataInfoEntry> Entries { get; set; }

        //public byte[] ToByteArray()
        //{
        //    var buffer = new byte[Entries.Count * EntrySize];
        //    for (int i = 0; i < Entries.Count; i++)
        //    {
        //        Entries[i].Write(buffer.AsSpan().Slice(i * EntrySize, EntrySize));
        //    }
        //    return buffer;
        //}

        public class PokemonDataInfoEntry
        {
            public CreatureIndex Id { get; }

            public byte[] Unk1toF { get; set; } // 16 bytes
            public IReadOnlyList<LevelUpMove> LevelupLearnset { get; }
            public short Unk5E { get; set; } // 0 for all Pokemon
            public int Unk60 { get; set; }
            public short PokedexNumber { get; set; }
            public short Unk66 { get; set; }
            public short Taxon { get; set; }
            public short ListNumber1 { get; set; }
            public short ListNumber2 { get; set; }
            public short Unk6E { get; set; }
            public short BaseHitPoints { get; set; }
            public short BaseAttack { get; set; }
            public short BaseSpecialAttack { get; set; }
            public short BaseDefense { get; set; }
            public short BaseSpecialDefense { get; set; }
            public short BaseSpeed { get; set; }
            public short EntryNumber { get; set; }
            public short EvolvesFromEntry { get; set; }
            public short ExperienceEntry { get; set; }
            public byte[] Unk82 { get; set; } // Length: 10
            public AbilityIndex Ability1 { get; set; }
            public AbilityIndex Ability2 { get; set; }
            public AbilityIndex HiddenAbility { get; set; }
            public PokemonType Type1 { get; set; }
            public PokemonType Type2 { get; set; }
            public byte[] Unk91 { get; set; } // Length: 3
            public byte IsMegaEvolution { get; set; } // Maybe
            public byte MinEvolveLevel { get; set; }
            public short Unk96 { get; set; }

            public PokemonDataInfoEntry(CreatureIndex id, ReadOnlySpan<byte> data)
            {
                this.Id = id;

                // The unknown data
                byte[] Unk1toF = new byte[16];
                for (var count = 0; count <= 0xF; count++)
                {
                    Unk1toF[count] = data[count + 0];
                }
                this.Unk1toF = Unk1toF;

                var levelupLearnset = new List<LevelUpMove>(26);
                for (int i = 0; i < 26; i++)
                {
                    int move = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x10 + i * sizeof(short)));
                    byte level = data[0x44 + i];
                    levelupLearnset.Add(new LevelUpMove(level, (WazaIndex)move));
                }
                this.LevelupLearnset = levelupLearnset;

                Unk5E = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x5E));
                Unk60 = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(0x60));
                PokedexNumber = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x64));
                Unk66 = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x66));
                Taxon = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x68));
                ListNumber1 = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x6A));
                ListNumber2 = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x6));
                Unk6E = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x6E));
                BaseHitPoints = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x70));
                BaseAttack = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x72));
                BaseDefense = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x74));
                BaseSpecialAttack = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x76));
                BaseSpecialDefense = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x78));
                BaseSpeed = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x7A));
                EntryNumber = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x7C));
                EvolvesFromEntry = BinaryPrimitives.ReadInt16LittleEndian(data.Slice( 0x7E));
                ExperienceEntry = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x80));

                // The unknown data
                byte[] Unk82 = new byte[11];
                for (var i = 0; i <= 0xA; i++)
                {
                    Unk82[i] = data[i + 0x82];
                }
                this.Unk82 = Unk82;

                Ability1 = (AbilityIndex)data[0x8C];
                Ability2 = (AbilityIndex)data[0x8D];
                HiddenAbility = (AbilityIndex)data[0x8E];
                Type1 = (PokemonType)data[0x8F];
                Type2 = (PokemonType)data[0x90];

                byte[] unk91 = new byte[3];
                unk91[0] = data[0x91];
                unk91[1] = data[0x92];
                unk91[2] = data[0x93];
                this.Unk91 = unk91;

                IsMegaEvolution = data[0x94];
                MinEvolveLevel = data[0x95];
                Unk96 = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x96));
            }

            // To-do: Convert to `Write(Span<byte> data)` as done in the RTDX version
            //public byte[] ToBytes()
            //{
            //    List<byte> @out = new List<byte>();
            //    // Unknown 1-F
            //    var unk1 = this.Unk1toF;
            //    for (int i = 0; i <= 0xF; i++)
            //    {
            //        @out.Add(unk1[i]);
            //    }
            //    // The 26 moves
            //    for (int i = 0; i <= 25; i++)
            //    {
            //        @out.AddRange(BitConverter.GetBytes(Moves[i]));
            //    }
            //    // The 26 move levels
            //    for (int i = 0; i <= 25; i++)
            //    {
            //        @out.Add(MoveLevels[i]);
            //    }
            //    // Properties
            //    @out.AddRange(BitConverter.GetBytes(Unk5E));
            //    @out.AddRange(BitConverter.GetBytes(Unk60));
            //    @out.AddRange(BitConverter.GetBytes(PokedexNumber));
            //    @out.AddRange(BitConverter.GetBytes(Unk66));
            //    @out.AddRange(BitConverter.GetBytes(Taxon));
            //    @out.AddRange(BitConverter.GetBytes(ListNumber1));
            //    @out.AddRange(BitConverter.GetBytes(ListNumber2));
            //    @out.AddRange(BitConverter.GetBytes(Unk6E));
            //    @out.AddRange(BitConverter.GetBytes(BaseHitPoints));
            //    @out.AddRange(BitConverter.GetBytes(BaseAttack));
            //    @out.AddRange(BitConverter.GetBytes(BaseDefense));
            //    @out.AddRange(BitConverter.GetBytes(BaseSpecialAttack));
            //    @out.AddRange(BitConverter.GetBytes(BaseSpecialDefense));
            //    @out.AddRange(BitConverter.GetBytes(BaseSpeed));
            //    @out.AddRange(BitConverter.GetBytes(EntryNumber));
            //    @out.AddRange(BitConverter.GetBytes(EvolvesFromEntry));
            //    @out.AddRange(BitConverter.GetBytes(ExperienceEntry));

            //    // Unknown data
            //    for (var count = 0; count <= 0xA; count++)
            //        @out.Add(Unk82[count]);

            //    // More properties
            //    @out.Add(Ability1);
            //    @out.Add(Ability2);
            //    @out.Add(HiddenAbility);
            //    @out.Add(Type1);
            //    @out.Add(Type2);

            //    @out.Add(this.Unk91[0]);
            //    @out.Add(this.Unk91[1]);
            //    @out.Add(this.Unk91[2]);

            //    @out.Add(IsMegaEvolution);
            //    @out.Add(MinEvolveLevel);
            //    @out.AddRange(BitConverter.GetBytes(Unk96));

            //    return @out.ToArray();
            //}
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
