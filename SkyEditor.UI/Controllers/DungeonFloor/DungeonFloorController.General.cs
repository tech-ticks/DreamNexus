using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditorUI.Infrastructure;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System.Linq;
using System.Globalization;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

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
        [UI] private Entry? entryBuriedItemSetIndex;
        [UI] private Entry? entryMaxBuriedItems;
        [UI] private Entry? entryMinTrapDensity;
        [UI] private Entry? entryMaxTrapDensity;
        [UI] private Entry? entryMinEnemyDensity;
        [UI] private Entry? entryMaxEnemyDensity;
        [UI] private Entry? entryMysteryHouseChance;
        [UI] private ComboBoxText? cbMysteryHouseSize;
        [UI] private Entry? entryMonsterHouseChance;
        [UI] private Entry? entryKecleonShopChance;
        [UI] private Entry? entryTurnLimit;
        [UI] private Entry? entryMinMoneyStackSize;
        [UI] private Entry? entryMaxMoneyStackSize;
        [UI] private Entry? entryShort30;
        [UI] private Entry? entryShort32;
        [UI] private SpinButton? sbNameId;

        // Names
        [UI] private ComboBoxText? cbNameLanguage;
        [UI] private Entry? entryPlaceName0;
        [UI] private Entry? entryPlaceName1;
        [UI] private Entry? entryPlaceName2;
        [UI] private Entry? entryPlaceName3;

        private LocalizedStringCollection? strings;

        void LoadGeneralTab()
        {
            var strings = rom.GetStrings().English;
            var commonStrings = rom.GetCommonStrings();
            var dungeonStatuses = Enum.GetValues<DungeonStatusIndex>()
                .SkipLast(1)
                .Select(index =>
                {
                    var name = commonStrings.GetDungeonStatusName(index);
                    return !string.IsNullOrEmpty(name) ? name : $"({index.ToString()})";
                });
            weatherStore!.AppendAll(dungeonStatuses);

            labelDungeonName!.Text = strings.GetDungeonName(dungeon.Id);

            string prefix = (dungeon.Features.HasFlag(DungeonFeature.FloorDirectionUp)) ? "" : "B";
            labelFloorNumber!.Text = $"{prefix}{floor.Index}F";

            entryMapDataIndex!.Text = floor.DungeonMapDataInfoIndex.ToString();
            switchBossFloor!.Active = floor.IsBossFloor;
            entryInvitationIndex!.Text = floor.InvitationIndex.ToString();
            entryEvent!.Text = floor.Event;
            cbWeather!.Active = (int)floor.Weather;
            entryShort02!.Text = floor.BalanceFloorInfoShort02.ToString();
            entryUnknownItemSetIndex!.Text = floor.UnknownItemSetIndex.ToString();
            entryBuriedItemSetIndex!.Text = floor.BuriedItemSetIndex.ToString();
            entryMaxBuriedItems!.Text = floor.MaxBuriedItems.ToString();
            entryMinTrapDensity!.Text = floor.MinTrapDensity.ToString();
            entryMaxTrapDensity!.Text = floor.MaxTrapDensity.ToString();
            entryMinEnemyDensity!.Text = floor.MinEnemyDensity.ToString();
            entryMaxEnemyDensity!.Text = floor.MaxEnemyDensity.ToString();
            entryMysteryHouseChance!.Text = floor.MysteryHouseChance.ToString();
            cbMysteryHouseSize!.Active = floor.MysteryHouseSize;
            entryMonsterHouseChance!.Text = floor.MonsterHouseChance.ToString();
            entryKecleonShopChance!.Text = floor.KecleonShopChance.ToString();
            entryTurnLimit!.Text = floor.TurnLimit.ToString();
            entryMinMoneyStackSize!.Text = floor.MinMoneyStackSize.ToString();
            entryMaxMoneyStackSize!.Text = floor.MaxMoneyStackSize.ToString();
            entryShort30!.Text = floor.BalanceFloorInfoShort30.ToString();
            entryShort32!.Text = floor.BalanceFloorInfoShort32.ToString();
            sbNameId!.Value = floor.NameId;

            unknownsStore!.AppendValues("Byte2D", floor.BalanceFloorInfoByte2D);
            unknownsStore.AppendValues("Byte2E", floor.BalanceFloorInfoByte2E);
            unknownsStore.AppendValues("Byte2F", floor.BalanceFloorInfoByte2F);
            unknownsStore.AppendValues("Byte34", floor.BalanceFloorInfoByte34);
            unknownsStore.AppendValues("Byte35", floor.BalanceFloorInfoByte35);
            for (int i = 0x37; i <= 0x43; i++)
            {
                unknownsStore.AppendValues($"Byte{i:X}", floor.BalanceFloorInfoBytes37to43[i - 0x37]);
            }
            unknownsStore.AppendValues("Byte46", floor.BalanceFloorInfoByte46);
            unknownsStore.AppendValues("Byte47", floor.BalanceFloorInfoByte47);
            unknownsStore.AppendValues("Byte49", floor.BalanceFloorInfoByte49);
            unknownsStore.AppendValues("Byte4A", floor.BalanceFloorInfoByte4A);
            unknownsStore.AppendValues("Byte4F", floor.BalanceFloorInfoByte4F);
            unknownsStore.AppendValues("Byte50", floor.BalanceFloorInfoByte50);
            unknownsStore.AppendValues("Byte51", floor.BalanceFloorInfoByte51);
            unknownsStore.AppendValues("Byte56", floor.BalanceFloorInfoByte56);
            unknownsStore.AppendValues("Byte57", floor.BalanceFloorInfoByte57);
            unknownsStore.AppendValues("Byte58", floor.BalanceFloorInfoByte58);

            for (int i = 0x5A; i <= 0x61; i++)
            {
                unknownsStore.AppendValues($"Byte{i:X}", floor.BalanceFloorInfoBytes5Ato61[i - 0x5A]);
            }

            LoadNames(LanguageType.EN);
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
            floor.Weather = (DungeonStatusIndex)cbWeather!.Active;
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

        private void OnBuriedItemSetChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryBuriedItemSetIndex!.Text, out byte value))
            {
                floor.BuriedItemSetIndex = value;
            }
            else if (!string.IsNullOrEmpty(entryBuriedItemSetIndex!.Text))
            {
                entryBuriedItemSetIndex!.Text = floor.BuriedItemSetIndex.ToString();
            }
        }

        private void OnMaxBuriedItemsChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryMaxBuriedItems!.Text, out byte value))
            {
                floor.MaxBuriedItems = value;
            }
            else if (!string.IsNullOrEmpty(entryMaxBuriedItems!.Text))
            {
                entryMaxBuriedItems!.Text = floor.MaxBuriedItems.ToString();
            }
        }

        private void OnMinTrapDensityChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryMinTrapDensity!.Text, out byte value))
            {
                floor.MinTrapDensity = value;
            }
            else if (!string.IsNullOrEmpty(entryMinTrapDensity!.Text))
            {
                entryMinTrapDensity!.Text = floor.MinTrapDensity.ToString();
            }
        }

        private void OnMaxTrapDensityChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryMaxTrapDensity!.Text, out byte value))
            {
                floor.MaxTrapDensity = value;
            }
            else if (!string.IsNullOrEmpty(entryMaxTrapDensity!.Text))
            {
                entryMaxTrapDensity!.Text = floor.MaxTrapDensity.ToString();
            }
        }

        private void OnMinEnemyDensityChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryMinEnemyDensity!.Text, out byte value))
            {
                floor.MinEnemyDensity = value;
            }
            else if (!string.IsNullOrEmpty(entryMinEnemyDensity!.Text))
            {
                entryMinEnemyDensity!.Text = floor.MinEnemyDensity.ToString();
            }
        }

        private void OnMaxEnemyDensityChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryMaxEnemyDensity!.Text, out byte value))
            {
                floor.MaxEnemyDensity = value;
            }
            else if (!string.IsNullOrEmpty(entryMaxEnemyDensity!.Text))
            {
                entryMaxEnemyDensity!.Text = floor.MaxEnemyDensity.ToString();
            }
        }

        private void OnMysteryHouseChanceChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryMysteryHouseChance!.Text, out byte value))
            {
                floor.MysteryHouseChance = value;
            }
            else if (!string.IsNullOrEmpty(entryMysteryHouseChance!.Text))
            {
                entryMysteryHouseChance!.Text = floor.MysteryHouseChance.ToString();
            }
        }

        private void OnMysteryHouseSizeChanged(object sender, EventArgs args)
        {
            floor.MysteryHouseSize = (byte) cbMysteryHouseSize!.Active;
        }

        private void OnMonsterHouseChanceChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryMonsterHouseChance!.Text, out byte value))
            {
                floor.MonsterHouseChance = value;
            }
            else if (!string.IsNullOrEmpty(entryMonsterHouseChance!.Text))
            {
                entryMonsterHouseChance!.Text = floor.MonsterHouseChance.ToString();
            }
        }

        private void OnKecleonShopChanceChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryKecleonShopChance!.Text, out byte value))
            {
                floor.KecleonShopChance = value;
            }
            else if (!string.IsNullOrEmpty(entryKecleonShopChance!.Text))
            {
                entryKecleonShopChance!.Text = floor.KecleonShopChance.ToString();
            }
        }

        private void OnShort24Changed(object sender, EventArgs args)
        {
            if (short.TryParse(entryTurnLimit!.Text, out short value))
            {
                floor.TurnLimit = value;
            }
            else if (!string.IsNullOrEmpty(entryTurnLimit!.Text))
            {
                entryTurnLimit!.Text = floor.TurnLimit.ToString();
            }
        }
        private void OnMinMoneyStackSizeChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryMinMoneyStackSize!.Text, out short value))
            {
                floor.MinMoneyStackSize = value;
            }
            else if (!string.IsNullOrEmpty(entryMinMoneyStackSize!.Text))
            {
                entryMinMoneyStackSize!.Text = floor.MinMoneyStackSize.ToString();
            }
        }
        private void OnMaxMoneyStackSizeChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryMaxMoneyStackSize!.Text, out short value))
            {
                floor.MaxMoneyStackSize = value;
            }
            else if (!string.IsNullOrEmpty(entryMaxMoneyStackSize!.Text))
            {
                entryMaxMoneyStackSize!.Text = floor.MaxMoneyStackSize.ToString();
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
                floor.BalanceFloorInfoShort32 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort32!.Text))
            {
                entryShort32!.Text = floor.BalanceFloorInfoShort32.ToString();
            }
        }

        private void OnNameIdChanged(object sender, EventArgs args)
        {
            floor.NameId = (byte)sbNameId!.ValueAsInt;
            LoadNames((LanguageType)cbNameLanguage!.Active);
        }

        private void OnUnknownValueEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (!unknownsStore!.GetIter(out var iter, path))
            {
                return;
            }

            if (!byte.TryParse(args.NewText, out byte value))
            {
                return;
            }

            var name = (string)unknownsStore!.GetValue(iter, UnknownLocationColumn);
            unknownsStore.SetValue(iter, UnknownValueColumn, value);

            // Strip "Byte" prefix and parse as int
            var location = int.Parse(name.Substring(4), NumberStyles.HexNumber);
            if (location >= 0x37 && location <= 0x43)
            {
                floor.BalanceFloorInfoBytes37to43[location - 0x37] = value;
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
                    case 0x46:
                        floor.BalanceFloorInfoByte46 = value;
                        break;
                    case 0x47:
                        floor.BalanceFloorInfoByte47 = value;
                        break;
                    case 0x49:
                        floor.BalanceFloorInfoByte49 = value;
                        break;
                    case 0x4A:
                        floor.BalanceFloorInfoByte4A = value;
                        break;
                    case 0x4F:
                        floor.BalanceFloorInfoByte4F = value;
                        break;
                    case 0x50:
                        floor.BalanceFloorInfoByte50 = value;
                        break;
                    case 0x51:
                        floor.BalanceFloorInfoByte51 = value;
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

        #region Names

        private void OnPlaceName0Changed(object sender, EventArgs args)
        {
            int hash = executable.GetPlaceName0HashForNameId(floor.NameId);
            strings!.SetCommonString(hash, entryPlaceName0!.Text);
        }

        private void OnPlaceName1Changed(object sender, EventArgs args)
        {
            int hash = executable.GetPlaceName1HashForNameId(floor.NameId);
            strings!.SetCommonString(hash, entryPlaceName1!.Text);
        }

        private void OnPlaceName2Changed(object sender, EventArgs args)
        {
            int hash = executable.GetPlaceName2HashForNameId(floor.NameId);
            strings!.SetCommonString(hash, entryPlaceName2!.Text);
        }

        private void OnPlaceName3Changed(object sender, EventArgs args)
        {
            int hash = executable.GetPlaceName3HashForNameId(floor.NameId);
            strings!.SetCommonString(hash, entryPlaceName3!.Text);
        }

        private void OnNameLanguageChanged(object sender, EventArgs args)
        {
            LoadNames((LanguageType)cbNameLanguage!.Active);
        }

        private void LoadNames(LanguageType language)
        {
            strings = rom.GetStrings().GetStringsForLanguage(language);
            int placeName0Hash = executable.GetPlaceName0HashForNameId(floor.NameId);
            entryPlaceName0!.Text = strings.GetCommonString(placeName0Hash);

            int placeName1Hash = executable.GetPlaceName1HashForNameId(floor.NameId);
            entryPlaceName1!.Text = strings.GetCommonString(placeName1Hash);

            int placeName2Hash = executable.GetPlaceName2HashForNameId(floor.NameId);
            entryPlaceName2!.Text = strings.GetCommonString(placeName2Hash);

            int placeName3Hash = executable.GetPlaceName3HashForNameId(floor.NameId);
            entryPlaceName3!.Text = strings.GetCommonString(placeName3Hash);
        }

        #endregion
    }
}
