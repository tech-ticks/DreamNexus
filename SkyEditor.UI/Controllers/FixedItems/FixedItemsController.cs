using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI.Controllers
{
    class FixedItemsController : Widget
    {
        [UI] private ListStore? fixedItemsStore;
        [UI] private TreeView? fixedItemsTree;

        [UI] private ListStore? itemsStore;
        [UI] private EntryCompletion? itemsCompletion;

        private IFixedItemCollection fixedItems;
        private IRtdxRom rom;

        private const int IndexColumn = 0;
        private const int ItemColumn = 1;
        private const int QuantityColumn = 2;
        private const int Short04Column = 3;
        private const int Byte06Column = 4;

        public FixedItemsController(IRtdxRom rom) : this(new Builder("FixedItems.glade"), rom)
        {
        }

        private FixedItemsController(Builder builder, IRtdxRom rom) : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            this.rom = rom;
            this.fixedItems = rom.GetFixedItemCollection();

            itemsStore!.AppendAll(AutocompleteHelpers.GetItems(rom));

            for (int i = 0; i < fixedItems.Entries.Count; i++)
            {
                AddToStore(fixedItems.Entries[i], i);
            }
        }

        private void AddToStore(FixedItemModel model, int index)
        {
            fixedItemsStore!.AppendValues(
                index,
                AutocompleteHelpers.FormatItem(rom, model.Index),
                (int) model.Quantity,
                (int) model.Short04,
                (int) model.Byte06
            );
        }

        private void OnItemEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedItemsStore!.GetIter(out var iter, path))
            {
                var itemIndex = AutocompleteHelpers.ExtractItem(args.NewText);
                if (itemIndex.HasValue)
                {
                    fixedItemsStore.SetValue(iter, ItemColumn,
                        AutocompleteHelpers.FormatItem(rom!, itemIndex.Value));
                    fixedItems.Entries[path.Indices[0]].Index = itemIndex.Value;
                }
            }
        }

        private void OnItemEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = itemsCompletion;
        }

        private void OnQuantityEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedItemsStore!.GetIter(out var iter, path))
            {
                var entry = fixedItems.Entries[path.Indices[0]];
                if (ushort.TryParse(args.NewText, out ushort value))
                {
                    entry.Quantity = value;
                }
                fixedItemsStore.SetValue(iter, QuantityColumn, (int) entry.Quantity);
            }
        }

        private void OnShort04Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedItemsStore!.GetIter(out var iter, path))
            {
                var entry = fixedItems.Entries[path.Indices[0]];
                if (ushort.TryParse(args.NewText, out ushort value))
                {
                    entry.Short04 = value;
                }
                fixedItemsStore.SetValue(iter, Short04Column, (int) entry.Short04);
            }
        }

        private void OnByte06Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (fixedItemsStore!.GetIter(out var iter, path))
            {
                var entry = fixedItems.Entries[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    entry.Byte06 = value;
                }
                fixedItemsStore.SetValue(iter, Byte06Column, (int) entry.Byte06);
            }
        }

        private void OnAddClicked(object sender, EventArgs args)
        {
            var entry = new FixedItemModel();
            fixedItems.Entries.Add(entry);
            AddToStore(entry, fixedItems.Entries.Count - 1);
        }

        private void OnRemoveClicked(object sender, EventArgs args)
        {
            if (fixedItemsTree!.Selection.GetSelected(out var model, out var iter))
            {
                var path = model.GetPath(iter);
                int index = path.Indices[0];
                fixedItems.Entries.RemoveAt(index);
                var store = (ListStore) model;
                store.Remove(ref iter);
                store.FixIndices(IndexColumn);
            }
        }
    }
}
