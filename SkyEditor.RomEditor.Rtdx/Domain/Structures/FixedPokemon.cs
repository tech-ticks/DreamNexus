using SkyEditor.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;
using FixedCreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.fixed_creature.Index;
using WazaIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public interface IFixedPokemon
    {
        IReadOnlyList<FixedPokemonEntry> Entries { get; }
    }

    public class FixedPokemon : IFixedPokemon
    {
        private const int EntrySize = 0x30;
        public FixedPokemon(byte[] data)
        {
            var sir0 = new Sir0(data);
            var entryCount = sir0.SubHeader.ReadInt32(0);
            var entries = new List<FixedPokemonEntry>();
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add(new FixedPokemonEntry(i, data, sir0.ReadInt32((i + 1) * 8)));
            }
            this.Entries = entries;
        }

        public IReadOnlyList<FixedPokemonEntry> Entries { get; }
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
        }

        public FixedCreatureIndex FixedCreatureId { get; }
        public CreatureIndex PokemonId { get; }
        public WazaIndex Move1 { get; }
        public WazaIndex Move2 { get; }
        public WazaIndex Move3 { get; }
        public WazaIndex Move4 { get; }
        public byte Level { get; }
        public byte AttackBoost { get; }
        public byte SpAttackBoost { get; }
        public byte DefenseBoost { get; }
        public byte SpDefenseBoost { get; }
        public byte SpeedBoost { get; }
    }
}
