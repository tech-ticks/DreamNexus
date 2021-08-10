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
        public static int UnknownLocationColumn = 0;
        public static int UnknownValueColumn = 1;

        [UI] private Label? labelDungeonName;
        [UI] private Label? labelFloorNumber;

        [UI] private ListStore? weatherStore;
        [UI] private ListStore? unknownsStore;

        [UI] private Entry? entryMapDataIndex;
        [UI] private Switch? switchBossFloor;
        [UI] private Entry? entryInvitationIndex;
        [UI] private Entry? entryEvent;
        [UI] private ComboBox? cbWeather;
        [UI] private Entry? entryShort02;
        [UI] private Entry? entryUnknownItemSetIndex;
        [UI] private Entry? entryShort24;
        [UI] private Entry? entryShort26;
        [UI] private Entry? entryShort28;
        [UI] private Entry? entryShort30;
        [UI] private Entry? entryShort32;
        [UI] private Entry? entryNameId;

        void LoadGeneralTab()
        {
            var commonStrings = rom.GetCommonStrings();
            var dungeonStatuses = Enum.GetValues<DungeonStatusIndex>()
                .SkipLast(1)
                .Select(index =>
                {
                    var name = commonStrings.GetDungeonStatusName(index);
                    return !string.IsNullOrEmpty(name) ? name : $"({index.ToString()})";
                });
            weatherStore!.AppendAll(dungeonStatuses);

            labelDungeonName!.Text = dungeon.DungeonName;

            string prefix = (dungeon.Features.HasFlag(DungeonFeature.FloorDirectionUp)) ? "" : "B";
            labelFloorNumber!.Text = $"{prefix}{floor.FriendlyIndex}F";

            entryMapDataIndex!.Text = floor.DungeonMapDataInfoIndex.ToString();
            switchBossFloor!.Active = floor.IsBossFloor;
            entryInvitationIndex!.Text = floor.InvitationIndex.ToString();
            entryEvent!.Text = floor.Event;
            cbWeather!.Active = (int) floor.Weather;
            entryShort02!.Text = floor.BalanceFloorInfoShort02.ToString();
            entryUnknownItemSetIndex!.Text = floor.UnknownItemSetIndex.ToString();
            entryShort24!.Text = floor.BalanceFloorInfoShort24.ToString();
            entryShort26!.Text = floor.BalanceFloorInfoShort26.ToString();
            entryShort28!.Text = floor.BalanceFloorInfoShort28.ToString();
            entryShort30!.Text = floor.BalanceFloorInfoShort30.ToString();
            entryShort32!.Text = floor.BalanceFloorInfoShort32.ToString();
            entryNameId!.Text = floor.NameId.ToString();

            unknownsStore!.AppendValues("Byte2D", floor.BalanceFloorInfoByte2D);
            unknownsStore.AppendValues("Byte2E", floor.BalanceFloorInfoByte2E);
            unknownsStore.AppendValues("Byte2F", floor.BalanceFloorInfoByte2F);
            unknownsStore.AppendValues("Byte34", floor.BalanceFloorInfoByte34);
            unknownsStore.AppendValues("Byte35", floor.BalanceFloorInfoByte35);
            for (int i = 0; i < floor.BalanceFloorInfoBytes37to53.Length; i++)
            {
                unknownsStore.AppendValues($"Byte{(i+0x37).ToString("X")}", floor.BalanceFloorInfoBytes37to53[i]);
            }

            unknownsStore.AppendValues("Byte55", floor.BalanceFloorInfoByte55);
            unknownsStore.AppendValues("Byte56", floor.BalanceFloorInfoByte56);
            unknownsStore.AppendValues("Byte57", floor.BalanceFloorInfoByte57);
            unknownsStore.AppendValues("Byte58", floor.BalanceFloorInfoByte58);

            for (int i = 0; i < floor.BalanceFloorInfoBytes5Ato61.Length; i++)
            {
                unknownsStore.AppendValues($"Byte{(i+0x5A).ToString("X")}", floor.BalanceFloorInfoBytes5Ato61[i]);
            }
        }

        private void OnMapDataIndexChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryMapDataIndex!.Text, out short value))
            {
                floor.DungeonMapDataInfoIndex = value;
            }
            else if (!string.IsNullOrEmpty(entryMapDataIndex!.Text))
            {
                entryMapDataIndex!.Text = floor.DungeonMapDataInfoIndex.ToString();
            }
        }

        private void OnInvitationIndexChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryInvitationIndex!.Text, out byte value))
            {
                floor.InvitationIndex = value;
            }
            else if (!string.IsNullOrEmpty(entryInvitationIndex!.Text))
            {
                entryInvitationIndex!.Text = floor.InvitationIndex.ToString();
            }
        }

        private void OnEventChanged(object sender, EventArgs args)
        {
            floor.Event = entryEvent!.Text;
        }

        private void OnSwitchBossFloorStateSet(object sender, StateSetArgs args)
        {
            floor.IsBossFloor = args.State;
        }

        private void OnWeatherChanged(object sender, EventArgs args)
        {
            floor.Weather = (DungeonStatusIndex) cbWeather!.Active;
        }

        private void OnShort02Changed(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort02!.Text, out short value))
            {
                floor.BalanceFloorInfoShort02 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort02!.Text))
            {
                entryShort02!.Text = floor.BalanceFloorInfoShort02.ToString();
            }
        }

        private void OnUnknownItemSetChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryUnknownItemSetIndex!.Text, out byte value))
            {
                floor.UnknownItemSetIndex = value;
            }
            else if (!string.IsNullOrEmpty(entryUnknownItemSetIndex!.Text))
            {
                entryUnknownItemSetIndex!.Text = floor.UnknownItemSetIndex.ToString();
            }
        }

        private void OnShort24Changed(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort24!.Text, out short value))
            {
                floor.BalanceFloorInfoShort24 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort24!.Text))
            {
                entryShort24!.Text = floor.BalanceFloorInfoShort24.ToString();
            }
        }
        private void OnShort26Changed(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort26!.Text, out short value))
            {
                floor.BalanceFloorInfoShort26 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort26!.Text))
            {
                entryShort26!.Text = floor.BalanceFloorInfoShort26.ToString();
            }
        }
        private void OnShort28Changed(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort28!.Text, out short value))
            {
                floor.BalanceFloorInfoShort28 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort28!.Text))
            {
                entryShort28!.Text = floor.BalanceFloorInfoShort28.ToString();
            }
        }
        private void OnShort30Changed(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort30!.Text, out short value))
            {
                floor.BalanceFloorInfoShort30 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort30!.Text))
            {
                entryShort30!.Text = floor.BalanceFloorInfoShort30.ToString();
            }
        }

        private void OnShort32Changed(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort32!.Text, out short value))
            {
                floor.BalanceFloorInfoShort24 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort32!.Text))
            {
                entryShort32!.Text = floor.BalanceFloorInfoShort32.ToString();
            }
        }

        private void OnNameIdChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryNameId!.Text, out byte value))
            {
                floor.NameId = value;
            }
            else if (!string.IsNullOrEmpty(entryNameId!.Text))
            {
                entryNameId!.Text = floor.NameId.ToString();
            }
        }

        private void OnUnknownValueEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (!unknownsStore!.GetIter(out TreeIter iter, path))
            {
                return;
            }

            if (!byte.TryParse(args.NewText, out byte value))
            {
                return;
            }
            
            var name = (string) unknownsStore!.GetValue(iter, UnknownLocationColumn);
            unknownsStore.SetValue(iter, UnknownValueColumn, value);

            // Strip "Byte" prefix and parse as int
            var location = int.Parse(name.Substring(4), NumberStyles.HexNumber);
            if (location >= 0x37 && location <= 0x53)
            {
                floor.BalanceFloorInfoBytes37to53[location - 0x37] = value;
            }
            else if (location >= 0x5A && location <= 0x61)
            {
                floor.BalanceFloorInfoBytes5Ato61[location - 0x5A] = value;
            }
            else
            {
                switch (location)
                {
                    case 0x2D:
                        floor.BalanceFloorInfoByte2D = value;
                        break;
                    case 0x2E:
                        floor.BalanceFloorInfoByte2E = value;
                        break;
                    case 0x2F:
                        floor.BalanceFloorInfoByte2F = value;
                        break;
                    case 0x34:
                        floor.BalanceFloorInfoByte34 = value;
                        break;
                    case 0x35:
                        floor.BalanceFloorInfoByte35 = value;
                        break;
                    case 0x55:
                        floor.BalanceFloorInfoByte55 = value;
                        break;
                    case 0x56:
                        floor.BalanceFloorInfoByte56 = value;
                        break;
                    case 0x57:
                        floor.BalanceFloorInfoByte57 = value;
                        break;
                    case 0x58:
                        floor.BalanceFloorInfoByte58 = value;
                        break;
                }
            }
        }
    }
}
