using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using YamlDotNet.Serialization;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    [DebuggerDisplay("DungeonModel: {DungeonId} -> {DungeonName}")]
    public class DungeonModel
    {
        public static int MaxNameId = 130;

        public DungeonModel()
        {
        }

        [Obsolete]
        public DungeonModel(DungeonDataInfo.Entry data)
        {
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public DungeonIndex Id { get; set; }

        [Obsolete, YamlIgnore]
        public string DungeonName { get; set; } = "(unknown dungeon)";

        // From dungeon_data_info.bin
        public DungeonFeature Features { get; set; }
        public short DataInfoShort0A { get; set; }
        public int SortKey { get; set; }
        public byte DataInfoByte13 { get; set; }
        public byte MaxItems { get; set; }
        public byte MaxTeammates { get; set; }
        public byte DataInfoByte17 { get; set; }
        public byte DataInfoByte18 { get; set; }
        public byte DataInfoByte19 { get; set; }

        // From request_level.bin
        public short AccessibleFloorCount { get; set; } // Same as DungeonExtra.Floors
        public short UnknownFloorCount { get; set; }
        public short TotalFloorCount { get; set; }
        public short NameId { get; set; }

        public List<DungeonPokemonStatsModel>? PokemonStats { get; set; }

        [YamlIgnore] // Saved in separate files
        public List<ItemSetModel> ItemSets { get; set; } = new List<ItemSetModel>();

        [YamlIgnore] // Saved in separate files
        public List<DungeonFloorModel>? Floors { get; set; } = new List<DungeonFloorModel>();

        // Deprecated properties
        [Obsolete, YamlIgnore]
        public DungeonDataInfo.Entry? Data { get; set; }

        [Obsolete, YamlIgnore]
        public DungeonExtra.Entry? Extra { get; set; }

        [Obsolete, YamlIgnore]
        public DungeonBalance.Entry? Balance { get; set; }

        [Obsolete, YamlIgnore]
        public ItemArrange.Entry? ItemArrange { get; set; }

        [Obsolete, YamlIgnore]
        public RequestLevel.Entry? RequestLevel { get; set; }
    }
}
