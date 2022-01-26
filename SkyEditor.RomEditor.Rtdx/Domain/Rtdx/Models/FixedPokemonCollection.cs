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
            for (int i = 0; i < Entries.Count; i++)
            {
                var viewModel = Entries[i];
                var model = romEntries[i];

                model.PokemonId = viewModel.PokemonId;
                model.Move1 = viewModel.Move1;
                model.Move2 = viewModel.Move2;
                model.Move3 = viewModel.Move3;
                model.Move4 = viewModel.Move4;
                model.DungeonIndex = viewModel.DungeonIndex;
                model.Level = viewModel.Level;
                model.HitPoints = viewModel.HitPoints;
                model.AttackBoost = viewModel.AttackBoost;
                model.SpAttackBoost = viewModel.SpAttackBoost;
                model.DefenseBoost = viewModel.DefenseBoost;
                model.SpDefenseBoost = viewModel.SpDefenseBoost;
                model.SpeedBoost = viewModel.SpeedBoost;
                model.InvitationIndex = viewModel.InvitationIndex;
            }
        }
    }
}
