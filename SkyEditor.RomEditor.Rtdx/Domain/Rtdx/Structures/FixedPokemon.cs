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
        IList<FixedPokemonEntry> Entries { get; }
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

        public IList<FixedPokemonEntry> Entries { get; }

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
        public FixedPokemonEntry(int index)
        {
            FixedCreatureId = (FixedCreatureIndex)index;
            Data = new byte[FixedPokemon.EntrySize];
        }

        public FixedPokemonEntry(int index, byte[] data, int offset)
        {
            FixedCreatureId = (FixedCreatureIndex)index;
            PokemonId = (CreatureIndex)BitConverter.ToInt16(data, offset + 0);
            HitPoints = BitConverter.ToInt16(data, offset + 0x2);
            Short04 = BitConverter.ToInt16(data, offset + 0x4);
            Short06 = BitConverter.ToInt16(data, offset + 0x6);
            Move1 = (WazaIndex)BitConverter.ToInt16(data, offset + 0x8);
            Move2 = (WazaIndex)BitConverter.ToInt16(data, offset + 0xA);
            Move3 = (WazaIndex)BitConverter.ToInt16(data, offset + 0xC);
            Move4 = (WazaIndex)BitConverter.ToInt16(data, offset + 0xE);
            DungeonIndex = (DungeonIndex)BitConverter.ToInt16(data, offset + 0x10);
            Short12 = BitConverter.ToInt16(data, offset + 0x12);
            Byte14 = data[offset + 0x14];
            Byte15 = data[offset + 0x15];
            Byte16 = data[offset + 0x16];
            Byte17 = data[offset + 0x17];
            Byte18 = data[offset + 0x18];
            Byte19 = data[offset + 0x19];
            Level = data[offset + 0x1A];
            AttackBoost = data[offset + 0x1B];
            SpAttackBoost = data[offset + 0x1C];
            DefenseBoost = data[offset + 0x1D];
            SpDefenseBoost = data[offset + 0x1E];
            SpeedBoost = data[offset + 0x1F];
            Byte20 = data[offset + 0x20];
            Byte21 = data[offset + 0x21];
            Byte22 = data[offset + 0x22];
            Byte23 = data[offset + 0x23];
            Byte24 = data[offset + 0x24];
            Byte25 = data[offset + 0x25];
            Byte26 = data[offset + 0x26];
            InvitationIndex = data[offset + 0x27];
            Byte28 = data[offset + 0x28];
            Byte29 = data[offset + 0x29];
            Byte2A = data[offset + 0x2A];
            Byte2B = data[offset + 0x2B];
            Byte2C = data[offset + 0x2C];
            Byte2D = data[offset + 0x2D];
            Byte2E = data[offset + 0x2E];
            Byte2F = data[offset + 0x2F];

            Data = new byte[FixedPokemon.EntrySize];
            Array.Copy(data, offset, Data, 0, FixedPokemon.EntrySize);
        }

        public byte[] ToByteArray()
        {
            BitConverter.GetBytes((short)PokemonId).CopyTo(Data, 0);
            BitConverter.GetBytes(HitPoints).CopyTo(Data, 0x2);
            BitConverter.GetBytes(Short04).CopyTo(Data, 0x4);
            BitConverter.GetBytes(Short06).CopyTo(Data, 0x6);
            BitConverter.GetBytes((short)Move1).CopyTo(Data, 0x8);
            BitConverter.GetBytes((short)Move2).CopyTo(Data, 0xA);
            BitConverter.GetBytes((short)Move3).CopyTo(Data, 0xC);
            BitConverter.GetBytes((short)Move4).CopyTo(Data, 0xE);
            BitConverter.GetBytes((short)DungeonIndex).CopyTo(Data, 0x10);
            BitConverter.GetBytes(Short12).CopyTo(Data, 0x12);
            Data[0x14] = Byte14;
            Data[0x15] = Byte15;
            Data[0x16] = Byte16;
            Data[0x17] = Byte17;
            Data[0x18] = Byte18;
            Data[0x19] = Byte19;
            Data[0x1A] = Level;
            Data[0x1B] = AttackBoost;
            Data[0x1C] = SpAttackBoost;
            Data[0x1D] = DefenseBoost;
            Data[0x1E] = SpDefenseBoost;
            Data[0x1F] = SpeedBoost;
            Data[0x20] = Byte20;
            Data[0x21] = Byte21;
            Data[0x22] = Byte22;
            Data[0x23] = Byte23;
            Data[0x24] = Byte24;
            Data[0x25] = Byte25;
            Data[0x26] = Byte26;
            Data[0x27] = InvitationIndex;
            Data[0x28] = Byte28;
            Data[0x29] = Byte29;
            Data[0x2A] = Byte2A;
            Data[0x2B] = Byte2B;
            Data[0x2C] = Byte2C;
            Data[0x2D] = Byte2D;
            Data[0x2E] = Byte2E;
            Data[0x2F] = Byte2F;

            return Data;
        }

        private byte[] Data { get; }
        public FixedCreatureIndex FixedCreatureId { get; }
        public CreatureIndex PokemonId { get; set; }
        public short Short04 { get; set; }
        public short Short06 { get; set; }
        public WazaIndex Move1 { get; set; }
        public WazaIndex Move2 { get; set; }
        public WazaIndex Move3 { get; set; }
        public WazaIndex Move4 { get; set; }
        public DungeonIndex DungeonIndex { get; set; }
        public short Short12 { get; set; }
        public byte Byte14 { get; set; }
        public byte Byte15 { get; set; }
        public byte Byte16 { get; set; }
        public byte Byte17 { get; set; }
        public byte Byte18 { get; set; }
        public byte Byte19 { get; set; }
        public byte Level { get; set; }
        public short HitPoints { get; set; }
        public byte AttackBoost { get; set; }
        public byte SpAttackBoost { get; set; }
        public byte DefenseBoost { get; set; }
        public byte SpDefenseBoost { get; set; }
        public byte SpeedBoost { get; set; }
        public byte Byte20 { get; set; }
        public byte Byte21 { get; set; }
        public byte Byte22 { get; set; }
        public byte Byte23 { get; set; }
        public byte Byte24 { get; set; }
        public byte Byte25 { get; set; }
        public byte Byte26 { get; set; }
        public byte InvitationIndex { get; set; }
        public byte Byte28 { get; set; }
        public byte Byte29 { get; set; }
        public byte Byte2A { get; set; }
        public byte Byte2B { get; set; }
        public byte Byte2C { get; set; }
        public byte Byte2D { get; set; }
        public byte Byte2E { get; set; }
        public byte Byte2F { get; set; }
    }
}
