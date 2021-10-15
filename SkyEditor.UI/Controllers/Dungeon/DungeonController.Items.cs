using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditorUI.Infrastructure;
using System;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System.Linq;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditorUI.Controllers
{
    partial class DungeonController : Widget
    {
        private const int ItemCategoryWeightColumn = 2;

        private const int ItemRunningIndexColumn = 0;
        private const int ItemIndexColumn = 1;
        private const int ItemNameColumn = 2;
        private const int ItemWeightColumn = 3;
        private const int ItemChanceColumn = 4;

        [UI] private ListStore? itemCategoriesStore;
        [UI] private ComboBoxText? cbSelectedItemSet;
        [UI] private EntryCompletion? itemsCompletion;
        [UI] private ListStore? itemsStore;

        private ItemSetModel? currentItemSet;
        private static bool loadedItemCache = false;
        private static Dictionary<ItemIndex, ItemKind> itemKindByIndex = new Dictionary<ItemIndex, ItemKind>();
        private static Dictionary<ItemKind, ItemIndex[]> itemsByCategory = new Dictionary<ItemKind, ItemIndex[]>();
        private static Dictionary<ItemKind, string[]> itemStringsByCategory = new Dictionary<ItemKind, string[]>();
        private ILookup<ItemKind, ItemArrange.Entry.ItemWeightEntry>? itemCategoryLookup;

        private void LoadItemsTab()
        {
            var items = rom.GetItemDataInfo().Entries;
            if (!loadedItemCache)
            {
                loadedItemCache = true;
                itemsByCategory = items.GroupBy(item => item.ItemKind)
                    .ToDictionary(items => items.Key, items => items
                        .Select(item => item.Index)
                        .ToArray());
                itemStringsByCategory = items.GroupBy(item => item.ItemKind)
                    .ToDictionary(items => items.Key, items => items
                        .Select(item => AutocompleteHelpers.FormatItem(rom, item.Index))
                        .ToArray());
                itemKindByIndex = items.ToDictionary(item => item.Index, item => item.ItemKind);
            }
            LoadItemSet(0);
        }

        private void LoadItemSet(int index)
        {
            currentItemSet = dungeon.ItemSets[index];
            RefreshCategoryLookup();
            RefreshCategories();
            RefreshItems();
        }

        private void RefreshCategoryLookup()
        {
            itemCategoryLookup = currentItemSet!.ItemWeights.ToLookup(item => itemKindByIndex[item.Index]);
        }

        private void RefreshCategories()
        {
            itemCategoriesStore!.Clear();

            int totalCategoryWeight = currentItemSet!.ItemKindWeights.Values.Sum(weight => weight);
            for (ItemKind i = ItemKind.NONE + 1; i < ItemKind.TRAP; i++) // Don't include traps
            {
                int weight = currentItemSet.ItemKindWeights[i];
                float percentage = ((float) weight / totalCategoryWeight) * 100f;
                itemCategoriesStore.AppendValues((int) i, i.GetFriendlyName(), weight, $"{percentage:F2}%");
            }
        }

        private void RefreshItems()
        {
            for (ItemKind i = ItemKind.NONE + 1; i < ItemKind.TRAP; i++) // Don't include traps
            {
                RefreshItemsInCategory(i);
            }
        }

        private void RefreshItemsInCategory(ItemKind category)
        {
            string treeViewName = $"itemCatTree{(int) category}";
            var treeView = (TreeView) builder.GetObject(treeViewName);
            if (treeView.Model == null)
            {
                InitTreeView(treeView, category);
            }

            var store = (TreeStore) treeView.Model!;
            store.Clear();

            if (itemCategoryLookup == null || !itemCategoryLookup.Contains(category))
            {
                return;
            }

            var itemsInCategory = itemCategoryLookup[category];
            var totalWeight = itemsInCategory.Sum(item => item.Weight);

            int i = 0;
            foreach (var item in itemsInCategory)
            {
                int weight = (int) item.Weight;
                float percentage = ((float) weight / totalWeight) * 100f;
                store.AppendValues(i, (int) item.Index, AutocompleteHelpers.FormatItem(rom, item.Index),
                    weight, $"{percentage:F2}%");
                i++;
            }
        }

        private void OnItemCategoryWeightEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (itemCategoriesStore!.GetIter(out var iter, path))
            {
                var index = (ItemKind) path.Indices[0] + 1;
                var weight = currentItemSet!.ItemKindWeights[index];
                if (ushort.TryParse(args.NewText, out ushort value))
                {
                    currentItemSet.ItemKindWeights[index] = value;
                    RefreshCategories();
                }
            }
        }

        private void OnItemEdited(TreeStore store, EditedArgs args, ItemKind kind)
        {
            var path = new TreePath(args.Path);
            if (store.GetIter(out var iter, path))
            {
                var itemIndex = AutocompleteHelpers.ExtractItem(args.NewText);
                if (itemIndex.HasValue && itemKindByIndex[itemIndex.Value] == kind)
                {
                    var oldIndex = (ItemIndex) store.GetValue(iter, ItemIndexColumn);
                    var weightIndex = currentItemSet!.ItemWeights.FindIndex(weight => weight.Index == oldIndex);
                    if (weightIndex == -1)
                    {
                        return;
                    }
                    if (itemIndex != oldIndex &&
                        currentItemSet!.ItemWeights.Any(weight => weight.Index == itemIndex.Value))
                    {
                        UIUtils.ShowInfoDialog(MainWindow.Instance, "Cannot add item",
                            "The item is already in the list.");
                        return;
                    }
                    var weight = currentItemSet!.ItemWeights[weightIndex];
                    weight.Index = itemIndex.Value;
                    currentItemSet!.ItemWeights[weightIndex] = weight;
                    store.SetValue(iter, ItemIndexColumn, (int) itemIndex.Value);
                    store.SetValue(iter, ItemNameColumn, AutocompleteHelpers.FormatItem(rom, itemIndex.Value));
                    RefreshCategoryLookup();
                    RefreshItemsInCategory(kind);
                }
            }
        }

        private void OnItemWeightEdited(TreeStore store, EditedArgs args, ItemKind kind)
        {
            var path = new TreePath(args.Path);
            if (store.GetIter(out var iter, path))
            {
                var itemIndex = (ItemIndex) store.GetValue(iter, ItemIndexColumn);
                string name = (string) store.GetValue(iter, ItemNameColumn);
                if (ushort.TryParse(args.NewText, out ushort value) && value > 0)
                {
                    Console.WriteLine("Edited " + name + " to weight " + value);
                    var weightIndex = currentItemSet!.ItemWeights.FindIndex(weight => weight.Index == itemIndex);
                    if (weightIndex == -1)
                    {
                        return;
                    }
                    var weight = currentItemSet.ItemWeights[weightIndex];
                    weight.Weight = value;
                    currentItemSet!.ItemWeights[weightIndex] = weight;
                    RefreshCategoryLookup();
                    RefreshItemsInCategory(kind);
                }
            }
        }

        private void OnAddItemClicked(TreeView treeView, ItemKind kind)
        {
            if (currentItemSet != null && itemsByCategory.ContainsKey(kind))
            {
                var index = itemsByCategory[kind]
                    .FirstOrDefault(i => !currentItemSet.ItemWeights.Any(weight => weight.Index == i));

                if (index == default(ItemIndex))
                {
                    UIUtils.ShowInfoDialog(MainWindow.Instance, "Cannot add item",
                        "All items in this category were already added");
                    return;
                }
                var weights = new ItemArrange.Entry.ItemWeightEntry
                {
                    Index = index,
                    Weight = 10,
                };
                currentItemSet.ItemWeights.Add(weights);
                RefreshCategoryLookup();
                RefreshItemsInCategory(kind);
            }
        }

        private void OnRemoveItemClicked(TreeView treeView, ItemKind kind)
        {
            if (currentItemSet != null)
            {
                if (treeView.Selection.GetSelected(out var model, out var iter))
                {
                    var index = (ItemIndex) model.GetValue(iter, ItemIndexColumn);
                    currentItemSet!.ItemWeights.RemoveAll(item => item.Index == index);
                    (model as TreeStore)!.Remove(ref iter);
                    RefreshCategoryLookup();
                    RefreshItemsInCategory(kind);
                }
            }
        }

        private void OnSelectedItemSetChanged(object sender, EventArgs args)
        {
            LoadItemSet(cbSelectedItemSet!.Active);
        }  

        private void InitTreeView(TreeView treeView, ItemKind kind)
        {
            var model = new TreeStore(new Type[] { typeof(int), typeof(int), typeof(string), typeof(int), typeof(string) });
            treeView.Model = model;

            foreach (var column in treeView.Columns)
            {
                treeView.RemoveColumn(column);
            }

            var col1 = new TreeViewColumn();
            col1.Title = "Item";
            var textRenderer1 = new CellRendererText();
            textRenderer1.Editable = true;
            textRenderer1.EditingStarted += (object sender, EditingStartedArgs args) =>
            {
                var editable = (Entry) args.Editable;
                itemsStore!.Clear();
                itemsStore!.AppendAll(itemStringsByCategory[kind]);
                editable.Completion = itemsCompletion;
            };
            textRenderer1.Edited += (object sender, EditedArgs args) =>
            {
                OnItemEdited(model, args, kind);
            };
            col1.PackStart(textRenderer1, true);
            col1.AddAttribute(textRenderer1, "text", ItemNameColumn);

            col1.Resizable = false;
            col1.Expand = false;

            var col2 = new TreeViewColumn();
            col2.Title = "Weight";
            var textRenderer2 = new CellRendererText();
            col2.PackStart(textRenderer2, true);
            col2.AddAttribute(textRenderer2, "text", ItemWeightColumn);
            col2.Resizable = false;
            col2.Expand = false;

            textRenderer2.Editable = true;
            textRenderer2.Edited += (object sender, EditedArgs args) =>
            {
                OnItemWeightEdited(model, args, kind);
            };

            var col3 = new TreeViewColumn();
            col3.Title = "Chance";
            var textRenderer3 = new CellRendererText();
            textRenderer3.Alignment = Pango.Alignment.Left;
            col3.PackStart(textRenderer3, true);
            col3.AddAttribute(textRenderer3, "text", ItemChanceColumn);
            col3.Resizable = false;
            col3.Expand = false;
            col3.FixedWidth = 100;

            treeView.AppendColumn(col1);
            treeView.AppendColumn(col2);
            treeView.AppendColumn(col3);

            var addButton = (Button) builder.GetObject($"itemCatAdd{(int) kind}");
            addButton.Clicked += (object? sender, EventArgs args) =>
            {
                OnAddItemClicked(treeView, kind);
            };

            var removeButton = (Button) builder.GetObject($"itemCatRemove{(int) kind}");
            removeButton.Clicked += (object? sender, EventArgs args) =>
            {
                OnRemoveItemClicked(treeView, kind);
            };
        }
    }
}
