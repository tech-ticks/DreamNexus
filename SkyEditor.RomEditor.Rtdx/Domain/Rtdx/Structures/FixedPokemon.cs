using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
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
            var builder = new Sir0Builder(8, 0x20 + Entries.Count * EntrySize + (Entries.Count + 1) * 8 + Entries.Count + 0x20);
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
            HitPoints = BitConverter.ToInt16(data, offset + 2);
            Move1 = (WazaIndex)BitConverter.ToInt16(data, offset + 8);
            Move2 = (WazaIndex)BitConverter.ToInt16(data, offset + 0xA);
            Move3 = (WazaIndex)BitConverter.ToInt16(data, offset + 0xC);
            Move4 = (WazaIndex)BitConverter.ToInt16(data, offset + 0xE);
            DungeonIndex = (DungeonIndex)data[offset + 0x10];
            Level = data[offset + 0x1A];
            AttackBoost = data[offset + 0x1B];
            SpAttackBoost = data[offset + 0x1C];
            DefenseBoost = data[offset + 0x1D];
            SpDefenseBoost = data[offset + 0x1E];
            SpeedBoost = data[offset + 0x1F];
            InvitationIndex = data[offset + 0x27];

            Data = new byte[FixedPokemon.EntrySize];
            Array.Copy(data, offset, Data, 0, FixedPokemon.EntrySize);
        }

        public byte[] ToByteArray()
        {
            BitConverter.GetBytes((short)PokemonId).CopyTo(Data, 0);
            BitConverter.GetBytes(HitPoints).CopyTo(Data, 2);
            BitConverter.GetBytes((short)Move1).CopyTo(Data, 8);
            BitConverter.GetBytes((short)Move2).CopyTo(Data, 0xA);
            BitConverter.GetBytes((short)Move3).CopyTo(Data, 0xC);
            BitConverter.GetBytes((short)Move4).CopyTo(Data, 0xE);
            Data[0x10] = (byte)DungeonIndex;
            Data[0x1A] = Level;
            Data[0x1B] = AttackBoost;
            Data[0x1C] = SpAttackBoost;
            Data[0x1D] = DefenseBoost;
            Data[0x1E] = SpDefenseBoost;
            Data[0x1F] = SpeedBoost;
            Data[0x27] = InvitationIndex;

            return Data;
        }

        private byte[] Data { get; }
        public FixedCreatureIndex FixedCreatureId { get; }
        public CreatureIndex PokemonId { get; set; }
        public WazaIndex Move1 { get; set; }
        public WazaIndex Move2 { get; set; }
        public WazaIndex Move3 { get; set; }
        public WazaIndex Move4 { get; set; }
        public DungeonIndex DungeonIndex { get; set; }
        public byte Level { get; set; }
        public short HitPoints { get; set; }
        public byte AttackBoost { get; set; }
        public byte SpAttackBoost { get; set; }
        public byte DefenseBoost { get; set; }
        public byte SpDefenseBoost { get; set; }
        public byte SpeedBoost { get; set; }
        public byte InvitationIndex { get; set; }
    }
}
