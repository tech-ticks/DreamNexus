using System;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx
{
    public static class DungeonHelpers
    {
        public static bool IsDojoDungeon(DungeonIndex index)
        {
            return index >= DungeonIndex.D051 && index <= DungeonIndex.D068;
        }
    }
}
