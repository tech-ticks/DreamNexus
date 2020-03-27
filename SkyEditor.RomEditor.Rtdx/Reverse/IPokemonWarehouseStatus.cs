using SkyEditor.RomEditor.Rtdx.Reverse.Const.pokemon;

namespace SkyEditor.RomEditor.Rtdx.Reverse
{
    public interface IPokemonWarehouseStatus
    {
        bool Favorite { get; set; }
        PokemonWarehouseId GetWarehouseId();
        IPokemonStatus GetStatus();
        SallyType GetSallyType();
        void SetSallyType(SallyType sally);
        bool IsSallyOtherMode();
        bool IsVisible();
        void SetVisible(bool bEnable);
        bool IsFavor();
        void SetFavor(bool bFlag);
        int GetFriendSortId();
        void UpdateFriendSortId();
    }
}
