using SkyEditor.RomEditor.Rtdx.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public class FixedPokemon : Sir0
    {
        private const int EntrySize = 0x30;
        public FixedPokemon(byte[] data) : base(data)
        {
            var entryCount = BitConverter.ToInt32(data, (int)SubHeaderOffset);
            var entries = new List<FixedPokemonEntry>();
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add(new FixedPokemonEntry(i, data, BitConverter.ToInt32(data, (int)SubHeaderOffset + ((i + 1) * 8))));
            }
            this.Entries = entries;
        }

        public IReadOnlyList<FixedPokemonEntry> Entries { get; }

        [DebuggerDisplay("{FixedCreatureId} -> {PokemonId} : {Move1}|{Move2}|{Move3}|{Move4}")]
        public class FixedPokemonEntry
        {
            public FixedPokemonEntry(int index, byte[] data, int offset)
            {
                FixedCreatureId = (FixedCreature)index;
                PokemonId = (Creature)BitConverter.ToInt16(data, offset + 0);
                Move1 = (Waza)BitConverter.ToInt16(data, offset + 8);
                Move2 = (Waza)BitConverter.ToInt16(data, offset + 0xA);
                Move3 = (Waza)BitConverter.ToInt16(data, offset + 0xC);
                Move4 = (Waza)BitConverter.ToInt16(data, offset + 0xE);
                Level = data[0x16];
                AttackBoost = data[0x17];
                SpAttackBoost = data[0x18];
                DefenseBoost = data[0x19];
                SpDefenseBoost = data[0x1A];
                SpeedBoost = data[0x1B];
            }

            public FixedCreature FixedCreatureId { get; }
            public Creature PokemonId { get; }
            public Waza Move1 { get; }
            public Waza Move2 { get; }
            public Waza Move3 { get; }
            public Waza Move4 { get; }
            public byte Level { get; }
            public byte AttackBoost { get; }
            public byte SpAttackBoost { get; }
            public byte DefenseBoost { get; }
            public byte SpDefenseBoost { get; }
            public byte SpeedBoost { get; }
        }
    }
}
