using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IActionStatModifierCollection
    {
        List<ActionStatModifierModel> StatModifiers { get; }
        void Flush(IRtdxRom rom);
    }

    public class ActionStatModifierCollection : IActionStatModifierCollection
    {
        public ActionStatModifierCollection()
        {
            StatModifiers = new List<ActionStatModifierModel>();
        }

        public ActionStatModifierCollection(IRtdxRom rom)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }
            
            var statusTable = rom.GetActStatusTableDataInfo();

            var modifiers = new List<ActionStatModifierModel>();

            for (int i = 0; i < statusTable.Entries.Count; i++)
            {
                var entry = statusTable.Entries[i];

                modifiers.Add(new ActionStatModifierModel
                {
                    AttackMod = entry.AttackMod,
                    SpecialAttackMod = entry.SpecialAttackMod,
                    DefenseMod = entry.DefenseMod,
                    SpecialDefenseMod = entry.SpecialDefenseMod,
                    SpeedMod = entry.SpeedMod,
                    AccuracyMod = entry.AccuracyMod,
                    EvasionMod = entry.EvasionMod,
                });
            }

            this.StatModifiers = modifiers;
        }

        public List<ActionStatModifierModel> StatModifiers { get; set; }

        public void Flush(IRtdxRom rom)
        {
            var statusTable = rom.GetActStatusTableDataInfo();

            statusTable.Entries.Clear();

            for (int i = 0; i < StatModifiers.Count; i++)
            {
                var modifier = StatModifiers[i];

                statusTable.Entries.Add(new ActStatusTableDataInfo.Entry(i)
                {
                    AttackMod = modifier.AttackMod,
                    SpecialAttackMod = modifier.SpecialAttackMod,
                    DefenseMod = modifier.DefenseMod,
                    SpecialDefenseMod = modifier.SpecialDefenseMod,
                    SpeedMod = modifier.SpeedMod,
                    AccuracyMod = modifier.AccuracyMod,
                    EvasionMod = modifier.EvasionMod,
                });
            }
        }
    }
}
