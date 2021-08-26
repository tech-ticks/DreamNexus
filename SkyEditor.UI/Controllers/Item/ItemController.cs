using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI.Controllers
{
    partial class ItemController : Widget
    {
        [UI] private Label? labelIdName;
        [UI] private ComboBox? cbCategory;
        [UI] private Entry? entryGraphicsId;
        [UI] private Entry? entryIconId;
        [UI] private Entry? entryImageName;
        [UI] private Entry? entryBuyPrice;
        [UI] private Entry? entrySellPrice;

        [UI] private ListStore? itemCategoriesStore;

        private ItemDataInfo.Entry item;
        private IRtdxRom rom;
        private Modpack modpack;
        private string internalName;

        public ItemController(IRtdxRom rom, Modpack modpack, ControllerContext context)
            : this(new Builder("Item.glade"), rom, modpack, context)
        {
        }

        private ItemController(Builder builder, IRtdxRom rom, Modpack modpack, ControllerContext context) 
            : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);
            
            var itemContext = (ItemControllerContext) context;
            var itemId = itemContext.Index;
            internalName = itemContext.InternalName;

            this.item = rom.GetItems().GetItemById(itemId)
                ?? throw new ArgumentException("Item from context ID is null", nameof(context));
            this.rom = rom;
            this.modpack = modpack;

            var englishStrings = rom.GetStrings().English;

            for (ItemKind i = ItemKind.NONE; i < ItemKind.MAX; i++)
            {
                itemCategoriesStore!.AppendValues((int) i, i.GetFriendlyName());
            }

            string formattedId = ((int) itemId).ToString("0000");
            string? name = englishStrings.GetItemNameByInternalName(internalName);
            labelIdName!.Text = $"#{formattedId}: {name} ({itemId.ToString()})";

            cbCategory!.Active = (int) item.ItemKind;
            entryGraphicsId!.Text = item.ItemGraphicsKey.ToString();
            entryIconId!.Text = item.IconIndex.ToString();
            entryImageName!.Text = item.Symbol;
            entryBuyPrice!.Text = item.BuyPrice.ToString();
            entrySellPrice!.Text = item.SellPrice.ToString();

            LoadTextTab();
            LoadGeneralTab();
            LoadFlagsTab();
        }

        private void OnCategoryChanged(object sender, EventArgs args)
        {
            item.ItemKind = (ItemKind) cbCategory!.Active;
        }

        private void OnGraphicsIdChanged(object sender, EventArgs args)
        {
            item.ItemGraphicsKey = entryGraphicsId!.ParseInt(item.ItemGraphicsKey);
        }

        private void OnIconIdChanged(object sender, EventArgs args)
        {
            item.IconIndex = entryIconId!.ParseByte(item.IconIndex);
        }

        private void OnImageNameChanged(object sender, EventArgs args)
        {
            item.Symbol = entryImageName!.Text;
        }

        private void OnBuyPriceChanged(object sender, EventArgs args)
        {
            item.BuyPrice = entryBuyPrice!.ParseUShort(item.BuyPrice);
        }

        private void OnSellPriceChanged(object sender, EventArgs args)
        {
            item.SellPrice = entryBuyPrice!.ParseUShort(item.SellPrice);
        }
    }
}
