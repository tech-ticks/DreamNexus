using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IFixedPokemonCollection
    {
        List<FixedPokemonModel> Entries { get; }
        void Flush(IRtdxRom rom);
    }

    public class FixedPokemonCollection : IFixedPokemonCollection
    {
        public FixedPokemonCollection()
        {
            Entries = new List<FixedPokemonModel>();
        }

        public FixedPokemonCollection(IRtdxRom rom)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }
            
            var entries = new List<FixedPokemonModel>();
            var romEntries = rom.GetFixedPokemon().Entries;

            foreach (var romEntry in romEntries)
            {
                entries.Add(new FixedPokemonModel
                {
                    PokemonId = romEntry.PokemonId,
                    Move1 = romEntry.Move1,
                    Move2 = romEntry.Move2,
                    Move3 = romEntry.Move3,
                    Move4 = romEntry.Move4,
                    DungeonIndex = romEntry.DungeonIndex,
                    Level = romEntry.Level,
                    HitPoints = romEntry.HitPoints,
                    AttackBoost = romEntry.AttackBoost,
                    SpAttackBoost = romEntry.SpAttackBoost,
                    DefenseBoost = romEntry.DefenseBoost,
                    SpDefenseBoost = romEntry.SpDefenseBoost,
                    SpeedBoost = romEntry.SpeedBoost,
                    InvitationIndex = romEntry.InvitationIndex,
                });
            }

            this.Entries = entries;
        }

        public List<FixedPokemonModel> Entries { get; set; }

        public void Flush(IRtdxRom rom)
        {
            var romEntries = rom.GetFixedPokemon().Entries;
            romEntries.Clear();

            for (int i = 0; i < Entries.Count; i++)
            {
                var entry = Entries[i];
                romEntries.Add(new FixedPokemonEntry(i)
                {
                    PokemonId = entry.PokemonId,
                    Move1 = entry.Move1,
                    Move2 = entry.Move2,
                    Move3 = entry.Move3,
                    Move4 = entry.Move4,
                    DungeonIndex = entry.DungeonIndex,
                    Level = entry.Level,
                    HitPoints = entry.HitPoints,
                    AttackBoost = entry.AttackBoost,
                    SpAttackBoost = entry.SpAttackBoost,
                    DefenseBoost = entry.DefenseBoost,
                    SpDefenseBoost = entry.SpDefenseBoost,
                    SpeedBoost = entry.SpeedBoost,
                    InvitationIndex = entry.InvitationIndex,
                });
            }
        }
    }
}
