using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IDungeonModel
    {
        DungeonIndex Id { get; }
        DungeonDataInfo.Entry Data { get; }
        DungeonExtra.Entry? Extra { get; }
        DungeonBalance.Entry? Balance { get; }
        string DungeonName { get; }
    }

    [DebuggerDisplay("DungeonModel: {DungeonId} -> {DungeonName}")]
    public class DungeonModel : IDungeonModel
    {
        public DungeonModel(ICommonStrings commonStrings, DungeonDataInfo.Entry data)
        {
            this.commonStrings = commonStrings ?? throw new ArgumentNullException(nameof(commonStrings));
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        private readonly ICommonStrings commonStrings;

        public DungeonIndex Id { get; set; }
        public DungeonDataInfo.Entry Data { get; set; }
        public DungeonExtra.Entry? Extra { get; set; }
        public DungeonBalance.Entry? Balance { get; set; }
        public string DungeonName => commonStrings.Dungeons.GetValueOrDefault(Id) ?? $"(Unknown: {Id})";
    }
}
