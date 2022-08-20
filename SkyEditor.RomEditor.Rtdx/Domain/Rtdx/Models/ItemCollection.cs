using System;
using System.Collections.Generic;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IItemCollection
    {
        IDictionary<ItemIndex, ItemDataInfo.Entry> LoadedItems { get; }
        int Count { get; }
        void SetItem(ItemIndex id, ItemDataInfo.Entry model);
        bool IsItemDirty(ItemIndex id);
        ItemDataInfo.Entry? GetItemById(ItemIndex id, bool markAsDirty = true);
        void Flush(IRtdxRom rom);
    }

    public class ItemCollection : IItemCollection
    {
        public IDictionary<ItemIndex, ItemDataInfo.Entry> LoadedItems { get; } = new Dictionary<ItemIndex, ItemDataInfo.Entry>();
        public HashSet<ItemIndex> DirtyItems { get; } = new HashSet<ItemIndex>();
        public int Count { get; private set; }

        private IRtdxRom rom;

        public ItemCollection(IRtdxRom rom)
        {
            this.rom = rom;
            Count = rom.GetItemDataInfo().Entries.Count;
        }

        private ItemDataInfo.Entry LoadItem(ItemIndex index)
        {
            var data = rom.GetItemDataInfo().Entries[(int) index];
            return data.Clone();
        }

        public ItemDataInfo.Entry GetItemById(ItemIndex id, bool markAsDirty = true)
        {
            if (!LoadedItems.ContainsKey(id))
            {
                LoadedItems.Add(id, LoadItem(id));
            }
            if (markAsDirty)
            {
                DirtyItems.Add(id);
            }
            return LoadedItems[id];
        }

        public void SetItem(ItemIndex id, ItemDataInfo.Entry model)
        {
            LoadedItems[id] = model;
        }

        public bool IsItemDirty(ItemIndex id)
        {
            return DirtyItems.Contains(id);
        }

        public void Flush(IRtdxRom rom)
        {
            var romItems = rom.GetItemDataInfo().Entries;
            foreach (var item in LoadedItems)
            {
                romItems[(int) item.Key] = item.Value;
            }
        }
    }    
}
