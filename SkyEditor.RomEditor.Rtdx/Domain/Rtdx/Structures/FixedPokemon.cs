using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IFixedPokemon
    {
        IReadOnlyList<FixedPokemonEntry> Entries { get; }
        Sir0 Build();
    }

    public class FixedPokemon : IFixedPokemon
    {
        public const int EntrySize = 0x30;
        public FixedPokemon(byte[] data)
        {
            var sir0 = new Sir0(data);
            var entryCount = sir0.SubHeader.ReadInt32(0);
            var entries = new List<FixedPokemonEntry>();
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add(new FixedPokemonEntry(i, data, sir0.SubHeader.ReadInt32((i + 1) * 8)));
            }
            this.Entries = entries;
        }

        public IReadOnlyList<FixedPokemonEntry> Entries { get; }

        public Sir0 Build()
        {
            var builder = new Sir0Builder(0x20 + Entries.Count * EntrySize + (Entries.Count + 1) * 8 + Entries.Count + 0x20);
            var pointers = new List<long>();
            foreach (var entry in Entries)
            {
                pointers.Add(builder.Length);
                builder.Write(builder.Length, entry.ToByteArray());
            }
            builder.SubHeaderOffset = builder.Length;
            builder.WriteInt64(builder.Length, Entries.Count);
            foreach (var pointer in pointers)
            {
                builder.WritePointer(builder.Length, pointer);
            }
            return builder.Build();
        }
    }

    [DebuggerDisplay("{FixedCreatureId} -> {PokemonId} : {Move1}|{Move2}|{Move3}|{Move4}")]
    public class FixedPokemonEntry
    {
        public FixedPokemonEntry(int index, byte[] data, int offset)
        {
            FixedCreatureId = (FixedCreatureIndex)index;
            PokemonId = (CreatureIndex)BitConverter.ToInt16(data, offset + 0);
            Move1 = (WazaIndex)BitConverter.ToInt16(data, offset + 8);
            Move2 = (WazaIndex)BitConverter.ToInt16(data, offset + 0xA);
            Move3 = (WazaIndex)BitConverter.ToInt16(data, offset + 0xC);
            Move4 = (WazaIndex)BitConverter.ToInt16(data, offset + 0xE);
            Level = data[0x16];
            AttackBoost = data[0x17];
            SpAttackBoost = data[0x18];
            DefenseBoost = data[0x19];
            SpDefenseBoost = data[0x1A];
            SpeedBoost = data[0x1B];

            Data = new byte[FixedPokemon.EntrySize];
            Array.Copy(data, offset, Data, 0, FixedPokemon.EntrySize);
        }

        public byte[] ToByteArray()
        {
            BitConverter.GetBytes((short)PokemonId).CopyTo(Data, 0);
            BitConverter.GetBytes((short)Move1).CopyTo(Data, 8);
            BitConverter.GetBytes((short)Move2).CopyTo(Data, 0xA);
            BitConverter.GetBytes((short)Move3).CopyTo(Data, 0xC);
            BitConverter.GetBytes((short)Move4).CopyTo(Data, 0xE);
            Data[0x16] = Level;
            Data[0x17] = AttackBoost;
            Data[0x18] = SpAttackBoost;
            Data[0x19] = DefenseBoost;
            Data[0x1A] = SpDefenseBoost;
            Data[0x1B] = SpeedBoost;

            return Data;
        }

        private byte[] Data { get; }
        public FixedCreatureIndex FixedCreatureId { get; }
        public CreatureIndex PokemonId { get; set; }
        public WazaIndex Move1 { get; set; }
        public WazaIndex Move2 { get; set; }
        public WazaIndex Move3 { get; set; }
        public WazaIndex Move4 { get; set; }
        public byte Level { get; set; }
        public byte AttackBoost { get; set; }
        public byte SpAttackBoost { get; set; }
        public byte DefenseBoost { get; set; }
        public byte SpDefenseBoost { get; set; }
        public byte SpeedBoost { get; set; }
    }
}
