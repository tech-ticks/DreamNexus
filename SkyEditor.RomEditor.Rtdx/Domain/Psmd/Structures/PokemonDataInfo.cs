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

            Entries = new List<PokemonInfoEntry>();
            for (var i = 0; i < entryCount; i++)
            {
                Entries.Add(new PokemonInfoEntry(data.Slice(i * EntrySize, EntrySize)));
            }
        }

        public List<PokemonInfoEntry> Entries { get; set; }

        //public byte[] ToByteArray()
        //{
        //    var buffer = new byte[Entries.Count * EntrySize];
        //    for (int i = 0; i < Entries.Count; i++)
        //    {
        //        Entries[i].Write(buffer.AsSpan().Slice(i * EntrySize, EntrySize));
        //    }
        //    return buffer;
        //}

        public class PokemonInfoEntry
        {
            public byte[] Unk1toF { get; set; } // 16 bytes
            public short[] Moves { get; set; }
            public byte[] MoveLevels { get; set; }
            public short Unk5E { get; set; } // 0 for all Pokemon
            public int Unk60 { get; set; }
            public short DexNumber { get; set; }
            public short Unk66 { get; set; }
            public short Category { get; set; }
            public short ListNumber1 { get; set; }
            public short ListNumber2 { get; set; }
            public short Unk6E { get; set; }
            public short BaseHP { get; set; }
            public short BaseAttack { get; set; }
            public short BaseSpAttack { get; set; }
            public short BaseDefense { get; set; }
            public short BaseSpDefense { get; set; }
            public short BaseSpeed { get; set; }
            public short EntryNumber { get; set; }
            public short EvolvesFromEntry { get; set; }
            public short ExpTableNumber { get; set; }
            public byte[] Unk82 { get; set; } // Length: 10
            public byte Ability1 { get; set; }
            public byte Ability2 { get; set; }
            public byte AbilityHidden { get; set; }
            public byte Type1 { get; set; }
            public byte Type2 { get; set; }
            public byte[] Unk91 { get; set; } // Length: 3
            public byte IsMegaEvolution { get; set; } // Maybe
            public byte MinEvolveLevel { get; set; }
            public short Unk96 { get; set; }

            public PokemonInfoEntry(ReadOnlySpan<byte> data)
            {
                // The unknown data
                byte[] Unk1toF = new byte[16];
                for (var count = 0; count <= 0xF; count++)
                {
                    Unk1toF[count] = data[count + 0];
                }
                this.Unk1toF = Unk1toF;

                // The 26 moves
                var moves = new short[26];
                for (var i = 0; i <= 25; i++)
                {
                    moves[i] = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(i * 2 + 0x10));
                }
                this.Moves = moves;

                // The 26 levels corresponding to the above moves
                byte[] moveLevels = new byte[26];
                for (var count = 0; count <= 25; count++)
                {
                    moveLevels[count] = data[count + 0x44];
                }
                this.MoveLevels = moveLevels;

                Unk5E = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x5E));
                Unk60 = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(0x60));
                DexNumber = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x64));
                Unk66 = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x66));
                Category = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x68));
                ListNumber1 = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x6A));
                ListNumber2 = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x6));
                Unk6E = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x6E));
                BaseHP = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x70));
                BaseAttack = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x72));
                BaseSpAttack = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x74));
                BaseDefense = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x76));
                BaseSpDefense = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x78));
                BaseSpeed = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x7A));
                EntryNumber = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x7C));
                EvolvesFromEntry = BinaryPrimitives.ReadInt16LittleEndian(data.Slice( 0x7E));
                ExpTableNumber = BinaryPrimitives.ReadInt16LittleEndian(data.Slice(0x80));

                // The unknown data
                byte[] Unk82 = new byte[11];
                for (var i = 0; i <= 0xA; i++)
                {
                    Unk82[i] = data[i + 0x82];
                }
                this.Unk82 = Unk82;

                Ability1 = data[0x8];
                Ability2 = data[0x8D];
                AbilityHidden = data[0x8E];
                Type1 = data[0x8F];
                Type2 = data[0x90];

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
            //    @out.AddRange(BitConverter.GetBytes(DexNumber));
            //    @out.AddRange(BitConverter.GetBytes(Unk66));
            //    @out.AddRange(BitConverter.GetBytes(Category));
            //    @out.AddRange(BitConverter.GetBytes(ListNumber1));
            //    @out.AddRange(BitConverter.GetBytes(ListNumber2));
            //    @out.AddRange(BitConverter.GetBytes(Unk6E));
            //    @out.AddRange(BitConverter.GetBytes(BaseHP));
            //    @out.AddRange(BitConverter.GetBytes(BaseAttack));
            //    @out.AddRange(BitConverter.GetBytes(BaseSpAttack));
            //    @out.AddRange(BitConverter.GetBytes(BaseDefense));
            //    @out.AddRange(BitConverter.GetBytes(BaseSpDefense));
            //    @out.AddRange(BitConverter.GetBytes(BaseSpeed));
            //    @out.AddRange(BitConverter.GetBytes(EntryNumber));
            //    @out.AddRange(BitConverter.GetBytes(EvolvesFromEntry));
            //    @out.AddRange(BitConverter.GetBytes(ExpTableNumber));

            //    // Unknown data
            //    for (var count = 0; count <= 0xA; count++)
            //        @out.Add(Unk82[count]);

            //    // More properties
            //    @out.Add(Ability1);
            //    @out.Add(Ability2);
            //    @out.Add(AbilityHidden);
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
    }
}
