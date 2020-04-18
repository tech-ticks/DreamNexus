using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Reverse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static SkyEditor.RomEditor.Rtdx.Domain.Structures.DungeonBalance;
using static SkyEditor.RomEditor.Rtdx.Domain.Structures.DungeonDataInfo;
using DungeonIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.dungeon.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Models
{
    public class DungeonCollection
    {
        public DungeonCollection(IRtdxRom rom)
        {
            this.rom = rom ?? throw new ArgumentNullException(nameof(rom));

            this.Dungeons = LoadDungeons();
        }

        protected readonly IRtdxRom rom;

        public DungeonModel[] Dungeons { get; }

        private DungeonModel[] LoadDungeons()
        {
            var commonStrings = rom.GetCommonStrings();
            var dungeonData = rom.GetDungeonDataInfo();
            var dungeonExtra = rom.GetDungeonExtra();
            var dungeonBalance = rom.GetDungeonBalance();

            var dungeons = new List<DungeonModel>();
            foreach (var dungeon in dungeonData.Entries)
            {
                dungeons.Add(new DungeonModel(commonStrings)
                {
                    DungeonId = dungeon.Key,
                    Data = dungeon.Value,
                    Extra = dungeonExtra.Entries.GetValueOrDefault(dungeon.Key),
                    Balance = dungeonBalance.Entries[dungeon.Value.DungeonBalanceIndex]
                });
            }
            dungeons.Sort((d1, d2) => d1.Data.SortKey.CompareTo(d2.Data.SortKey));
            return dungeons.ToArray();
        }

        [DebuggerDisplay("DungeonModel: {DungeonId} -> {DungeonName}")]
        public class DungeonModel
        {
            public DungeonModel(ICommonStrings commonStrings)
            {
                this.commonStrings = commonStrings ?? throw new ArgumentNullException(nameof(commonStrings));
            }

            private readonly ICommonStrings commonStrings;

            public DungeonIndex DungeonId { get; set; }
            public DungeonDataInfoEntry Data { get; set; }  // TODO: copy from the entry instead of referencing it
            public DungeonExtraEntry Extra { get; set; }  // TODO: copy from the entry instead of referencing it
            public DungeonBalanceEntry Balance { get; set; }  // TODO: copy from the entry instead of referencing it
            public string DungeonName => commonStrings.Dungeons.GetValueOrDefault(DungeonId) ?? $"(Unknown: {DungeonId})";
        }
    }
}
