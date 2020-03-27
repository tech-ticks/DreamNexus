using SkyEditor.RomEditor.Rtdx.Reverse.Const.pokemon;
using System;

namespace SkyEditor.RomEditor.Rtdx.Reverse
{
    public class PokemonWarehouseId
    {
        public const int NULL_ID = 65535;
        public const int MAX = 2000;
        private int id;

        public PokemonWarehouseId()
        {
        }

        public PokemonWarehouseId(int _id)
        {
            id = _id;
        }

        public PokemonWarehouseId(FixedWarehouseId _id)
        {
            id = (int)_id;
        }

        public void ReBuild(int _id)
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            return id != default;
        }

        public bool IsHero()
        {
            return id == (int)FixedWarehouseId.HERO;
        }

        public bool IsPartner()
        {
            return id == (int)FixedWarehouseId.PARTNER;
        }

        public bool IsFixedMember()
        {
            throw new NotImplementedException();
        }

        public bool IsGENGAA()
        {
            throw new NotImplementedException();
        }

        public bool IsKYARAGEKI_DEBUG_MEMBER()
        {
            throw new NotImplementedException();
        }

        public int Get()
        {
            throw new NotImplementedException();
        }
    }
}
