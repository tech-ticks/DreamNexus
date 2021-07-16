using Gtk;
using System;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using System.Linq;

namespace SkyEditorUI.Controllers
{
    partial class DungeonFloorController : Widget
    {
        private DungeonModel dungeon;
        private DungeonFloorModel floor;
        private IRtdxRom rom;
        private Modpack modpack;
        private Builder builder;

        public DungeonFloorController(IRtdxRom rom, Modpack modpack, ControllerContext context)
            : this(new Builder("DungeonFloor.glade"), rom, modpack, context)
        {
        }

        private DungeonFloorController(Builder builder, IRtdxRom rom, Modpack modpack, ControllerContext context) 
            : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);
            
            var floorContext = (context as DungeonFloorControllerContext);
            this.dungeon = rom.GetDungeons().GetDungeonById(floorContext!.DungeonIndex)
                ?? throw new ArgumentException("Dungeon from context ID is null", nameof(context));
            this.floor = dungeon.Floors.First(floor => floor.Index == floorContext.FloorIndex);
            this.rom = rom;
            this.modpack = modpack;
            this.builder = builder;

            LoadGeneralTab();
            LoadSpawnsTab();
            LoadTrapsTab();
        }
    }
}
