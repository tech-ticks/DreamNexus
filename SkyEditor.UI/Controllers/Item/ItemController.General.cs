using System;
using Gtk;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditorUI.Infrastructure;
using UI = Gtk.Builder.ObjectAttribute;

namespace SkyEditorUI.Controllers
{
    partial class ItemController : Widget
    {
        [UI] private Entry? entryTmMove;
        [UI] private Entry? entryPrimaryActionId;
        [UI] private Entry? entryReviveActionId;
        [UI] private Entry? entryThrowActionId;
        [UI] private ComboBox? cbCommandType;
        [UI] private Entry? entryShort0C;
        [UI] private Entry? entryByte17;
        [UI] private Entry? entryByte18;
        [UI] private Entry? entryByte1A;
        [UI] private Entry? entryByte1B;
        [UI] private Entry? entryByte1C;
        [UI] private Entry? entryByte1D;
        [UI] private Entry? entryByte1F;
        [UI] private Entry? entryByte20;
        [UI] private Entry? entryByte21;

        [UI] private ListStore? commandTypeStore;
        [UI] private ListStore? moveCompletionStore;

        private void LoadGeneralTab()
        {
            moveCompletionStore!.AppendAll(AutocompleteHelpers.GetMoves(rom));
            commandTypeStore!.AppendAll(Enum.GetNames<ItemCommandType>());

            entryTmMove!.Text = AutocompleteHelpers.FormatMove(rom, item.TaughtMove);
            entryPrimaryActionId!.Text = item.PrimaryActIndex.ToString();
            entryReviveActionId!.Text = item.ReviveActIndex.ToString();
            entryThrowActionId!.Text = item.ThrowActIndex.ToString();
            cbCommandType!.Active = (int) item.CommandType;
            entryShort0C!.Text = item.Short0C.ToString();
            entryByte17!.Text = item.Byte17.ToString();
            entryByte18!.Text = item.Byte18.ToString();
            entryByte1A!.Text = item.Byte1A.ToString();
            entryByte1B!.Text = item.Byte1B.ToString();
            entryByte1C!.Text = item.Byte1C.ToString();
            entryByte1D!.Text = item.Byte1D.ToString();
            entryByte1F!.Text = item.Byte1F.ToString();
            entryByte20!.Text = item.Byte20.ToString();
            entryByte21!.Text = item.Byte21.ToString();
        }

        private void OnTmMoveChanged(object sender, EventArgs args)
        {
            var moveIndex = AutocompleteHelpers.ExtractMove(entryTmMove!.Text);
            if (moveIndex.HasValue)
            {
                item.TaughtMove = moveIndex.Value;
                entryTmMove!.Text = AutocompleteHelpers.FormatMove(rom, moveIndex.Value);
            }
        }

        private void OnPrimaryActionIdChanged(object sender, EventArgs args)
        {
            item.PrimaryActIndex = entryPrimaryActionId!.ParseUShort(item.PrimaryActIndex);
        }

        private void OnReviveActionIdChanged(object sender, EventArgs args)
        {
            item.ReviveActIndex = entryReviveActionId!.ParseUShort(item.ReviveActIndex);
        }

        private void OnThrowActionIdChanged(object sender, EventArgs args)
        {
            item.ThrowActIndex = entryThrowActionId!.ParseUShort(item.ThrowActIndex);
        }

        private void OnCommandTypeChanged(object sender, EventArgs args)
        {
            item.CommandType = (ItemCommandType) cbCommandType!.Active;
        }

        private void OnShort0CChanged(object sender, EventArgs args)
        {
            item.Short0C = entryShort0C!.ParseUShort(item.Short0C);
        }

        private void OnByte17Changed(object sender, EventArgs args)
        {
            item.Byte17 = entryByte17!.ParseByte(item.Byte17);
        }

        private void OnByte18Changed(object sender, EventArgs args)
        {
            item.Byte18 = entryByte18!.ParseByte(item.Byte18);
        }

        private void OnByte1AChanged(object sender, EventArgs args)
        {
            item.Byte1A = entryByte1A!.ParseByte(item.Byte1A);
        }

        private void OnByte1BChanged(object sender, EventArgs args)
        {
            item.Byte1B = entryByte1B!.ParseByte(item.Byte1B);
        }

        private void OnByte1CChanged(object sender, EventArgs args)
        {
            item.Byte1C = entryByte1C!.ParseByte(item.Byte1C);
        }

        private void OnByte1DChanged(object sender, EventArgs args)
        {
            item.Byte1D = entryByte1A!.ParseByte(item.Byte1D);
        }

        private void OnByte1FChanged(object sender, EventArgs args)
        {
            item.Byte1F = entryByte1A!.ParseByte(item.Byte1F);
        }

        private void OnByte20Changed(object sender, EventArgs args)
        {
            item.Byte20 = entryByte20!.ParseByte(item.Byte20);
        }

        private void OnByte21Changed(object sender, EventArgs args)
        {
            item.Byte21 = entryByte21!.ParseByte(item.Byte21);
        }

        private void OnHelpPrimaryActionIdClicked(object sender, EventArgs args)
        {
            var dialog = new MessageDialog(MainWindow.Instance, DialogFlags.DestroyWithParent, MessageType.Info,
                            ButtonsType.Ok, "Action taken when used (items) or stepped on (traps).\n"
                                + "Open the entry with this ID in 'Actions' to view or edit the action's effects.");
            dialog.Title = "Primary action";
            dialog.Run();
            dialog.Destroy();
        }

        private void OnHelpReviveActionIdClicked(object sender, EventArgs args)
        {
            var dialog = new MessageDialog(MainWindow.Instance, DialogFlags.DestroyWithParent, MessageType.Info,
                            ButtonsType.Ok, "Action taken while reviving (only Tiny Reviver Seeds and Reviver Seeds).\n"
                                + "Open the entry with this ID in 'Actions' to view or edit the action's effects.");
            dialog.Title = "Revive action";
            dialog.Run();
            dialog.Destroy();
        }

        private void OnHelpThrowActionIdClicked(object sender, EventArgs args)
        {
            var dialog = new MessageDialog(MainWindow.Instance, DialogFlags.DestroyWithParent, MessageType.Info,
                            ButtonsType.Ok, "Action taken when the item is thrown.\n"
                                + "Open the entry with this ID in 'Actions' to view or edit the action's effects.");
            dialog.Title = "Throw action";
            dialog.Run();
            dialog.Destroy();
        }

        private void OnHelpCommandTypeClicked(object sender, EventArgs args)
        {
            var dialog = new MessageDialog(MainWindow.Instance, DialogFlags.DestroyWithParent, MessageType.Info,
                ButtonsType.Ok, "Action name displayed in dungeon menus (Use, Eat, Ingest, Equip...).\n"
                    + "See 'Strings' for actual values. Whether this setting has other effects is unknown.");
            dialog.Title = "Command Type";
            dialog.Run();
            dialog.Destroy();
        }
    }
}
