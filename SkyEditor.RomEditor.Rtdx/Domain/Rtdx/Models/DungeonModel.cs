using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using System;
using System.Diagnostics;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IDungeonModel
    {
        DungeonIndex Id { get; }
        DungeonDataInfo.Entry Data { get; }
        DungeonExtra.Entry? Extra { get; }
        DungeonBalance.Entry? Balance { get; }
        ItemArrange.Entry? ItemArrange { get; }
        RequestLevel.Entry? RequestLevel { get; }
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
        public ItemArrange.Entry? ItemArrange { get; set; }
        public RequestLevel.Entry? RequestLevel { get; set; }
        public string DungeonName => commonStrings.Dungeons.ContainsKey(Id) ? commonStrings.Dungeons[Id] : $"(Unknown: {Id})";
    }
}
