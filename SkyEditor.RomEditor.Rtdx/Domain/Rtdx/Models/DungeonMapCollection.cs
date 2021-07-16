using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IDungeonMapCollection
    {
        List<DungeonMapModel> Maps { get; }
        void Flush(IRtdxRom rom);
    }

    public class DungeonMapCollection : IDungeonMapCollection
    {
        public DungeonMapCollection()
        {
            Maps = new List<DungeonMapModel>();
        }

        public DungeonMapCollection(IRtdxRom rom)
        {
            if (rom == null)
            {
                throw new ArgumentNullException(nameof(rom));
            }
            
            var dungeonMapDataInfo = rom.GetDungeonMapDataInfo();
            var dungeonMapSymbol = rom.GetDungeonMapSymbol();

            var maps = new List<DungeonMapModel>();

            for (int i = 0; i < dungeonMapDataInfo.Entries.Count; i++)
            {
                var mapDataInfo = dungeonMapDataInfo.Entries[i];
                var mapSymbol = dungeonMapSymbol.Entries[i];

                maps.Add(new DungeonMapModel
                {
                    Symbol = mapSymbol,
                    FixedMapIndex = mapDataInfo.FixedMapIndex,
                    Byte06 = mapDataInfo.Byte06,
                    Byte07 = mapDataInfo.Byte07,
                    DungeonBgmSymbolIndex = mapDataInfo.DungeonBgmSymbolIndex,
                    Byte09 = mapDataInfo.Byte09,
                    Byte0A = mapDataInfo.Byte0A,
                    Byte0B = mapDataInfo.Byte0B,
                });
            }

            this.Maps = maps;
        }

        public List<DungeonMapModel> Maps { get; set; }

        public void Flush(IRtdxRom rom)
        {
            var dungeonMapDataInfo = rom.GetDungeonMapDataInfo();
            var dungeonMapSymbol = rom.GetDungeonMapSymbol();

            dungeonMapDataInfo.Entries.Clear();
            dungeonMapSymbol.Entries.Clear();

            for (int i = 0; i < Maps.Count; i++)
            {
                var map = Maps[i];
                dungeonMapSymbol.Entries.Add(map.Symbol);

                dungeonMapDataInfo.Entries.Add(new DungeonMapDataInfo.Entry
                {
                    Index = (ushort) i,
                    FixedMapIndex = map.FixedMapIndex,
                    Byte06 = map.Byte06,
                    Byte07 = map.Byte07,
                    DungeonBgmSymbolIndex = map.DungeonBgmSymbolIndex,
                    Byte09 = map.Byte09,
                    Byte0A = map.Byte0A,
                    Byte0B = map.Byte0B,
                });
            }
        }
    }    
}
