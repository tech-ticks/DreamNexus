using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static SkyEditor.RomEditor.Rtdx.Domain.Structures.DungeonBalance;
using static SkyEditor.RomEditor.Rtdx.Domain.Structures.DungeonExtra;
using static SkyEditor.RomEditor.Rtdx.Domain.Structures.DungeonDataInfo;
using DungeonIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.dungeon.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Models
{
    public interface IDungeonModel
    {
        DungeonIndex DungeonId { get; }
        DungeonDataInfoEntry Data { get; }
        DungeonExtraEntry? Extra { get; }
        DungeonBalanceEntry? Balance { get; }
        string DungeonName { get; }
    }

    [DebuggerDisplay("DungeonModel: {DungeonId} -> {DungeonName}")]
    public class DungeonModel : IDungeonModel
    {
        public DungeonModel(ICommonStrings commonStrings, DungeonDataInfoEntry data)
        {
            this.commonStrings = commonStrings ?? throw new ArgumentNullException(nameof(commonStrings));
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        private readonly ICommonStrings commonStrings;

        public DungeonIndex DungeonId { get; set; }
        public DungeonDataInfoEntry Data { get; set; }  // TODO: copy from the entry instead of referencing it
        public DungeonExtraEntry? Extra { get; set; }  // TODO: copy from the entry instead of referencing it
        public DungeonBalanceEntry? Balance { get; set; }  // TODO: copy from the entry instead of referencing it
        public string DungeonName => commonStrings.Dungeons.GetValueOrDefault(DungeonId) ?? $"(Unknown: {DungeonId})";
    }
}
