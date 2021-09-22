using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI.Controllers
{
    class ActionStatModifiersController : Widget
    {
        [UI] private ListStore? statModifiersStore;
        [UI] private TreeView? statModifiersTree;

        private IActionStatModifierCollection statModifiers;

        private const int IndexColumn = 0;
        private const int AttackColumn = 1;
        private const int DefenseColumn = 2;
        private const int SpecialAttackColumn = 3;
        private const int SpecialDefenseColumn = 4;
        private const int SpeedColumn = 5;
        private const int AccuracyColumn = 6;
        private const int EvasionColumn = 7;

        public ActionStatModifiersController(IRtdxRom rom) : this(new Builder("ActionStatModifiers.glade"), rom)
        {
        }

        private ActionStatModifiersController(Builder builder, IRtdxRom rom) : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            this.statModifiers = rom.GetActionStatModifiers();

            for (int i = 0; i < statModifiers.StatModifiers.Count; i++)
            {
                AddToStore(statModifiers.StatModifiers[i], i);
            }
        }

        private void AddToStore(ActionStatModifierModel modifier, int index)
        {
            statModifiersStore!.AppendValues(
                index,
                (int) modifier.AttackMod,
                (int) modifier.DefenseMod,
                (int) modifier.SpecialAttackMod,
                (int) modifier.SpecialDefenseMod,
                (int) modifier.SpeedMod,
                (int) modifier.AccuracyMod,
                (int) modifier.EvasionMod
            );
        }

        private void OnAttackEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (statModifiersStore!.GetIter(out var iter, path))
            {
                var modifier = statModifiers.StatModifiers[path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    modifier.AttackMod = value;
                }
                statModifiersStore.SetValue(iter, AttackColumn, (int) modifier.AttackMod);
            }
        }

        private void OnDefenseEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (statModifiersStore!.GetIter(out var iter, path))
            {
                var modifier = statModifiers.StatModifiers[path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    modifier.DefenseMod = value;
                }
                statModifiersStore.SetValue(iter, DefenseColumn, (int) modifier.DefenseMod);
            }
        }

        private void OnSpecialAttackEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (statModifiersStore!.GetIter(out var iter, path))
            {
                var modifier = statModifiers.StatModifiers[path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    modifier.SpecialAttackMod = value;
                }
                statModifiersStore.SetValue(iter, SpecialAttackColumn, (int) modifier.SpecialAttackMod);
            }
        }

        private void OnSpecialDefenseEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (statModifiersStore!.GetIter(out var iter, path))
            {
                var modifier = statModifiers.StatModifiers[path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    modifier.SpecialDefenseMod = value;
                }
                statModifiersStore.SetValue(iter, SpecialDefenseColumn, (int) modifier.SpecialDefenseMod);
            }
        }

        private void OnSpeedEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (statModifiersStore!.GetIter(out var iter, path))
            {
                var modifier = statModifiers.StatModifiers[path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    modifier.SpeedMod = value;
                }
                statModifiersStore.SetValue(iter, SpeedColumn, (int) modifier.SpeedMod);
            }
        }

        private void OnAccuracyEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (statModifiersStore!.GetIter(out var iter, path))
            {
                var modifier = statModifiers.StatModifiers[path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    modifier.AccuracyMod = value;
                }
                statModifiersStore.SetValue(iter, AccuracyColumn, (int) modifier.AccuracyMod);
            }
        }

        private void OnEvasionEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (statModifiersStore!.GetIter(out var iter, path))
            {
                var modifier = statModifiers.StatModifiers[path.Indices[0]];
                if (short.TryParse(args.NewText, out short value))
                {
                    modifier.EvasionMod = value;
                }
                statModifiersStore.SetValue(iter, EvasionColumn, (int) modifier.EvasionMod);
            }
        }

        private void OnAddClicked(object sender, EventArgs args)
        {
            var modifier = new ActionStatModifierModel();
            statModifiers.StatModifiers.Add(modifier);
            AddToStore(modifier, statModifiers.StatModifiers.Count - 1);
        }

        private void OnRemoveClicked(object sender, EventArgs args)
        {
            if (statModifiersTree!.Selection.GetSelected(out var model, out var iter))
            {
                var path = model.GetPath(iter);
                int index = path.Indices[0];
                statModifiers.StatModifiers.RemoveAt(index);
                var store = (ListStore) model;
                store.Remove(ref iter);
                store.FixIndices(IndexColumn);
            }
        }
    }
}
