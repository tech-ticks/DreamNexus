using Gtk;
using System;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

namespace SkyEditorUI.Controllers
{
    partial class DungeonController : Widget
    {
        private DungeonModel dungeon;
        private IRtdxRom rom;
        private Modpack modpack;
        private Builder builder;

        public DungeonController(IRtdxRom rom, Modpack modpack, ControllerContext context)
            : this(new Builder("Dungeon.glade"), rom, modpack, context) {
        }

        private DungeonController(Builder builder, IRtdxRom rom, Modpack modpack, ControllerContext context) 
            : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);
            
            var dungeonId = (context as DungeonControllerContext)!.Index;
            this.dungeon = rom.GetDungeons().GetDungeonById(dungeonId)
                ?? throw new ArgumentException("Dungeon from context ID is null", nameof(context));
            this.rom = rom;
            this.modpack = modpack;
            this.builder = builder;

            LoadGeneralTab();
            LoadStatsTab();
            LoadItemsTab();
        }
    }
}
