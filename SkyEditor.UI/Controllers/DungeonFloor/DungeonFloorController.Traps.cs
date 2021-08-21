using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditorUI.Infrastructure;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System.Linq;
using System.Globalization;

namespace SkyEditorUI.Controllers
{
    partial class DungeonFloorController : Widget
    {
        public static int TrapItemIndexColumn = 0;

        [UI] private ListStore? trapSpawnsStore;

        private void LoadTrapsTab()
        {
            RefreshTraps();
        }

        private void RefreshTraps()
        {
            if (floor.TrapWeights == null)
            {
                return;
            }

            var commonStrings = rom.GetCommonStrings();

            trapSpawnsStore!.Clear();
            int weightSum = floor.TrapWeights.Values.Sum(weight => weight);
            foreach (var trapWeight in floor.TrapWeights)
            {
                float percentage = ((float) trapWeight.Value / weightSum) * 100f;
                string? itemName = commonStrings.GetItemName(trapWeight.Key);
                string displayName = !string.IsNullOrEmpty(itemName) ? itemName : $"({trapWeight.Key.ToString()})";
                trapSpawnsStore.AppendValues((int) trapWeight.Key, displayName,
                    (int) trapWeight.Value, $"{percentage:F2}%");
            }
        }

        private void OnTrapWeightEdited(object sender, EditedArgs args)
        {
            if (floor.TrapWeights == null)
            {
                return;
            }

            var path = new TreePath(args.Path);
            if (trapSpawnsStore!.GetIter(out var iter, path))
            {
                var index = (ItemIndex) trapSpawnsStore.GetValue(iter, TrapItemIndexColumn);
                if (short.TryParse(args.NewText, out short value))
                {
                    floor.TrapWeights[index] = value;
                    RefreshTraps();
                }
            }
        }
    }
}
