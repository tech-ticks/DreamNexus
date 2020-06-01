using SkyEditor.RomEditor.Rtdx.Reverse.Const.item;

namespace SkyEditor.RomEditor.Rtdx.Reverse
{
    public interface IItem
    {
        uint GetUniqueId();
        Const.item.Index GetIndex();
        Kind GetKind();
        bool IsArrowOrStone();
        int GetCount();
        void SetCount(int value);
        void SetShopGoods(bool bEnable);
        bool IsShopGoods();
        int GetPrice(PriceType type);
        int GetPurePrice(PriceType type);
        string GetName(bool bPlural);
        string GetNameNoCount(bool bPlural);
        void SetBuyFlag();
        void ClearBuyFlag();
        bool IsBuyFlag();
        void SetSellFlag();
        void ClearSellFlag();
        bool IsSellFlag();
        void SetSticky(bool set);
        bool IsSticky();
        bool IsGrouped();
        bool IsPlaced();
        bool IsEquipped();
        bool IsRegistered();
        int GetSortKey();
    }
}
