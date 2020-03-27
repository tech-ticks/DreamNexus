using SkyEditor.RomEditor.Rtdx.Reverse.Const.pokemon;
using System;

namespace SkyEditor.RomEditor.Rtdx.Reverse
{
    public interface IPokemonWarehouse
    {
        IDataList<IPokemonWarehouseStatus> GetList(bool bFirst);
        void SetInitializeHero(Index index, GenderType gender);
        IPokemonWarehouseStatus GetHeroStatus();
        void SetInitializePartner(Index index, GenderType gender);
        IPokemonWarehouseStatus GetPartnerStatus();
        IPokemonWarehouseStatus GetFixedMemberStatus(FixedWarehouseId id);
    }
}
