using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;

namespace SkyEditorUI.Controllers
{
    partial class DungeonController : Widget
    {
        // Flags
        [UI] private ComboBox? cbDirection;
        [UI] private Switch? switchFlag1;
        [UI] private Switch? switchFlag2;
        [UI] private Switch? switchFlag3;
        [UI] private Switch? switchLevelReset;
        [UI] private Switch? switchFlag7;
        [UI] private Switch? switchFlag8;
        [UI] private Switch? switchAutoRevive;
        [UI] private Switch? switchFlag10;
        [UI] private Switch? switchFlag11;
        [UI] private Switch? switchFlag12;
        [UI] private Switch? switchFlag13;
        [UI] private Switch? switchFlag14;
        [UI] private Switch? switchRecruitingEnabled;
        [UI] private Switch? switchFlag16;
        [UI] private Switch? switchScanner;
        [UI] private Switch? switchRadar;

        // Values
        [UI] private Entry? entrySortKey;
        [UI] private Entry? entryMaxItems;
        [UI] private Entry? entryMaxPartyMembers;
        [UI] private Entry? entryShort0A;
        [UI] private Entry? entryByte13;
        [UI] private Entry? entryByte17;
        [UI] private Entry? entryByte18;
        [UI] private Entry? entryByte19;
        [UI] private Entry? entryAccessibleFloorCount;
        [UI] private Entry? entryUnknownFloorCount;
        [UI] private Entry? entryTotalFloorCount;

        // Names
        [UI] private Entry? entryMainEnglish;

        private void LoadGeneralTab()
        {
            // Flags
            cbDirection!.Active = dungeon.Features.HasFlag(DungeonFeature.FloorDirectionUp) ? 1 : 0;
            switchFlag1!.Active = dungeon.Features.HasFlag(DungeonFeature._Bit1);
            switchFlag2!.Active = dungeon.Features.HasFlag(DungeonFeature._Bit2);
            switchFlag3!.Active = dungeon.Features.HasFlag(DungeonFeature._Bit3);
            switchLevelReset!.Active = dungeon.Features.HasFlag(DungeonFeature.LevelReset);
            switchFlag7!.Active = dungeon.Features.HasFlag(DungeonFeature._Bit7);
            switchFlag8!.Active = dungeon.Features.HasFlag(DungeonFeature._Bit8);
            switchAutoRevive!.Active = dungeon.Features.HasFlag(DungeonFeature.AutoRevive);
            switchFlag10!.Active = dungeon.Features.HasFlag(DungeonFeature._Bit10);
            switchFlag11!.Active = dungeon.Features.HasFlag(DungeonFeature._Bit11);
            switchFlag12!.Active = dungeon.Features.HasFlag(DungeonFeature._Bit12);
            switchFlag13!.Active = dungeon.Features.HasFlag(DungeonFeature._Bit13);
            switchFlag14!.Active = dungeon.Features.HasFlag(DungeonFeature._Bit14);
            switchRecruitingEnabled!.Active = dungeon.Features.HasFlag(DungeonFeature.WildPokemonRecruitable);
            switchFlag16!.Active = dungeon.Features.HasFlag(DungeonFeature._Bit16);
            switchScanner!.Active = dungeon.Features.HasFlag(DungeonFeature.Scanner);
            switchRadar!.Active = dungeon.Features.HasFlag(DungeonFeature.Radar);

            // Values
            entrySortKey!.Text = dungeon.SortKey.ToString();
            entryMaxItems!.Text = dungeon.MaxItems.ToString();
            entryMaxPartyMembers!.Text = dungeon.MaxTeammates.ToString();
            entryShort0A!.Text = dungeon.DataInfoShort0A.ToString();
            entryByte13!.Text = dungeon.DataInfoByte13.ToString();
            entryByte17!.Text = dungeon.DataInfoByte17.ToString();
            entryByte18!.Text = dungeon.DataInfoByte18.ToString();
            entryByte19!.Text = dungeon.DataInfoByte19.ToString();
            entryAccessibleFloorCount!.Text = dungeon.AccessibleFloorCount.ToString();
            entryUnknownFloorCount!.Text = dungeon.UnknownFloorCount.ToString();
            entryTotalFloorCount!.Text = dungeon.TotalFloorCount.ToString();

            // Names
            entryMainEnglish!.Text = dungeon.DungeonName;
        }

        #region Flags
        private void OnDirectionChanged(object sender, EventArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature.FloorDirectionUp, cbDirection!.Active == 1);
        }

        private void OnFlag1StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit1, args.State);
        }

        private void OnFlag2StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit2, args.State);
        }

        private void OnFlag3StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit3, args.State);
        }

        private void OnLevelResetStateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature.LevelReset, args.State);
        }

        private void OnFlag5StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit5, args.State);
        }

        private void OnFlag6StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit6, args.State);
        }

        private void OnFlag7StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit7, args.State);
        }

        private void OnFlag8StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit8, args.State);
        }

        private void OnAutoReviveStateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature.AutoRevive, args.State);
        }

        private void OnFlag10StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit10, args.State);
        }

        private void OnFlag11StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit11, args.State);
        }

        private void OnFlag12StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit12, args.State);
        }

        private void OnFlag13StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit13, args.State);
        }

        private void OnFlag14StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit14, args.State);
        }

        private void OnRecruitingEnabledStateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature.WildPokemonRecruitable, args.State);
        }

        private void OnFlag16StateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature._Bit16, args.State);
        }

        private void OnScannerStateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature.Scanner, args.State);
        }

        private void OnRadarStateSet(object sender, StateSetArgs args)
        {
            dungeon.Features.SetFlag(DungeonFeature.Radar, args.State);
        }
        #endregion

        #region Values

        private void OnSortKeyChanged(object sender, EventArgs args)
        {
            if (int.TryParse(entrySortKey!.Text, out int value))
            {
                dungeon.SortKey = value;
            }
            else if (!string.IsNullOrEmpty(entrySortKey!.Text))
            {
                entrySortKey!.Text = dungeon.SortKey.ToString();
            }
        }

        private void OnMaxItemsChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryMaxItems!.Text, out byte value))
            {
                dungeon.MaxItems = value;
            }
            else if (!string.IsNullOrEmpty(entryMaxItems!.Text))
            {
                entryMaxItems!.Text = dungeon.MaxItems.ToString();
            }
        }

        private void OnMaxPartyMembersChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryMaxPartyMembers!.Text, out byte value))
            {
                dungeon.MaxTeammates = value;
            }
            else if (!string.IsNullOrEmpty(entryMaxPartyMembers!.Text))
            {
                entryMaxPartyMembers!.Text = dungeon.MaxTeammates.ToString();
            }
        }

        private void OnShort0AChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort0A!.Text, out short value))
            {
                dungeon.DataInfoShort0A = value;
            }
            else if (!string.IsNullOrEmpty(entryShort0A!.Text))
            {
                entryShort0A!.Text = dungeon.DataInfoShort0A.ToString();
            }
        }

        private void OnByte13Changed(object sender, EventArgs args)
        {
            if (byte.TryParse(entryByte13!.Text, out byte value))
            {
                dungeon.DataInfoByte13 = value;
            }
            else if (!string.IsNullOrEmpty(entryByte13!.Text))
            {
                entryByte13!.Text = dungeon.DataInfoByte13.ToString();
            }
        }

        private void OnByte17Changed(object sender, EventArgs args)
        {
            if (byte.TryParse(entryByte17!.Text, out byte value))
            {
                dungeon.DataInfoByte17 = value;
            }
            else if (!string.IsNullOrEmpty(entryByte17!.Text))
            {
                entryByte17!.Text = dungeon.DataInfoByte17.ToString();
            }
        }

        private void OnByte18Changed(object sender, EventArgs args)
        {
            if (byte.TryParse(entryByte18!.Text, out byte value))
            {
                dungeon.DataInfoByte18 = value;
            }
            else if (!string.IsNullOrEmpty(entryByte18!.Text))
            {
                entryByte18!.Text = dungeon.DataInfoByte18.ToString();
            }
        }

        private void OnByte19Changed(object sender, EventArgs args)
        {
            if (byte.TryParse(entryByte19!.Text, out byte value))
            {
                dungeon.DataInfoByte19 = value;
            }
            else if (!string.IsNullOrEmpty(entryByte19!.Text))
            {
                entryByte19!.Text = dungeon.DataInfoByte19.ToString();
            }
        }

        private void OnAccessibleFloorCountChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryAccessibleFloorCount!.Text, out short value))
            {
                dungeon.AccessibleFloorCount = value;
            }
            else if (!string.IsNullOrEmpty(entryAccessibleFloorCount!.Text))
            {
                entryAccessibleFloorCount!.Text = dungeon.AccessibleFloorCount.ToString();
            }
        }

        private void OnUnknownFloorCountChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryUnknownFloorCount!.Text, out short value))
            {
                dungeon.UnknownFloorCount = value;
            }
            else if (!string.IsNullOrEmpty(entryUnknownFloorCount!.Text))
            {
                entryUnknownFloorCount!.Text = dungeon.UnknownFloorCount.ToString();
            }
        }

        private void OnTotalFloorCountChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryTotalFloorCount!.Text, out short value))
            {
                dungeon.TotalFloorCount = value;
            }
            else if (!string.IsNullOrEmpty(entryTotalFloorCount!.Text))
            {
                entryTotalFloorCount!.Text = dungeon.TotalFloorCount.ToString();
            }
        }

        #endregion

        #region Names

        private void OnEntryMainEnglishChanged(object sender, EventArgs args)
        {
            dungeon.DungeonName = entryMainEnglish!.Text;
        }

        private void OnEntryMainJapaneseChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryMainFrenchChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryMainGermanChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryMainItalianChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryMainSpanishChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName1EnglishChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName1JapaneseChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName1FrenchChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName1GermanChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName1ItalianChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName1SpanishChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName2EnglishChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName2JapaneseChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName2FrenchChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName2GermanChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName2ItalianChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName2SpanishChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName3EnglishChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName3JapaneseChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName3FrenchChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName3GermanChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName3ItalianChanged(object sender, EventArgs args)
        {
        }

        private void OnEntryUnkName3SpanishChanged(object sender, EventArgs args)
        {
        }

        #endregion
    }
}
