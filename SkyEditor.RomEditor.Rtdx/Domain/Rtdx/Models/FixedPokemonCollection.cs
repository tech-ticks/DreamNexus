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
                    Short04 = romEntry.Short04,
                    Short06 = romEntry.Short06,
                    Move1 = romEntry.Move1,
                    Move2 = romEntry.Move2,
                    Move3 = romEntry.Move3,
                    Move4 = romEntry.Move4,
                    DungeonIndex = romEntry.DungeonIndex,
                    Short12 = romEntry.Short12,
                    Byte14 = romEntry.Byte14,
                    Byte15 = romEntry.Byte15,
                    Byte16 = romEntry.Byte16,
                    Byte17 = romEntry.Byte17,
                    Byte18 = romEntry.Byte18,
                    Byte19 = romEntry.Byte19,
                    Level = romEntry.Level,
                    HitPoints = romEntry.HitPoints,
                    AttackBoost = romEntry.AttackBoost,
                    SpAttackBoost = romEntry.SpAttackBoost,
                    DefenseBoost = romEntry.DefenseBoost,
                    SpDefenseBoost = romEntry.SpDefenseBoost,
                    SpeedBoost = romEntry.SpeedBoost,
                    Byte20 = romEntry.Byte20,
                    Byte21 = romEntry.Byte21,
                    Byte22 = romEntry.Byte22,
                    Byte23 = romEntry.Byte23,
                    Byte24 = romEntry.Byte24,
                    Byte25 = romEntry.Byte25,
                    Byte26 = romEntry.Byte26,
                    InvitationIndex = romEntry.InvitationIndex,
                    Byte28 = romEntry.Byte28,
                    Byte29 = romEntry.Byte29,
                    Byte2A = romEntry.Byte2A,
                    Byte2B = romEntry.Byte2B,
                    Byte2C = romEntry.Byte2C,
                    Byte2D = romEntry.Byte2D,
                    Byte2E = romEntry.Byte2E,
                    Byte2F = romEntry.Byte2F
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
                    Short04 = entry.Short04,
                    Short06 = entry.Short06,
                    Move1 = entry.Move1,
                    Move2 = entry.Move2,
                    Move3 = entry.Move3,
                    Move4 = entry.Move4,
                    DungeonIndex = entry.DungeonIndex,
                    Short12 = entry.Short12,
                    Byte14 = entry.Byte14,
                    Byte15 = entry.Byte15,
                    Byte16 = entry.Byte16,
                    Byte17 = entry.Byte17,
                    Byte18 = entry.Byte18,
                    Byte19 = entry.Byte19,
                    Level = entry.Level,
                    HitPoints = entry.HitPoints,
                    AttackBoost = entry.AttackBoost,
                    SpAttackBoost = entry.SpAttackBoost,
                    DefenseBoost = entry.DefenseBoost,
                    SpDefenseBoost = entry.SpDefenseBoost,
                    SpeedBoost = entry.SpeedBoost,
                    Byte20 = entry.Byte20,
                    Byte21 = entry.Byte21,
                    Byte22 = entry.Byte22,
                    Byte23 = entry.Byte23,
                    Byte24 = entry.Byte24,
                    Byte25 = entry.Byte25,
                    Byte26 = entry.Byte26,
                    InvitationIndex = entry.InvitationIndex,
                    Byte28 = entry.Byte28,
                    Byte29 = entry.Byte29,
                    Byte2A = entry.Byte2A,
                    Byte2B = entry.Byte2B,
                    Byte2C = entry.Byte2C,
                    Byte2D = entry.Byte2D,
                    Byte2E = entry.Byte2E,
                    Byte2F = entry.Byte2F
                });
            }
        }
    }
}
