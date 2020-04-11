using Newtonsoft.Json;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Rtdx.Domain;
using SkyEditor.RomEditor.Rtdx.Domain.Automation;
using SkyEditor.RomEditor.Rtdx.Domain.Commands;
using SkyEditor.RomEditor.Rtdx.Domain.Handlers;
using SkyEditor.RomEditor.Rtdx.Domain.Queries;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Reverse;
using System;
using System.IO;
using System.Linq;

namespace SkyEditor.RomEditor.Rtdx.ConsoleApp
{
    class Program
    {
        private static void ChangeStarters()
        {
            var rom = new RtdxRom(@"E:\01003D200BAA2000-Edited", PhysicalFileSystem.Instance);

            var handler = new ReplaceStarterHandler(rom);

            handler.Handle(new ReplaceStarterCommand
            {
                OldPokemonId = Reverse.Const.creature.Index.FUSHIGIDANE,
                NewPokemonId = Reverse.Const.creature.Index.MYUU,
                Move1 = Reverse.Const.waza.Index.HATAKU,
                Move2 = Reverse.Const.waza.Index.TELEPORT,
                Move3 = Reverse.Const.waza.Index.HENSHIN,
                Move4 = Reverse.Const.waza.Index.IYASHINOSUZU
            });
            handler.Handle(new ReplaceStarterCommand
            {
                OldPokemonId = Reverse.Const.creature.Index.ACHAMO,
                NewPokemonId = Reverse.Const.creature.Index.RIORU
            });
            handler.Handle(new ReplaceStarterCommand
            {
                OldPokemonId = Reverse.Const.creature.Index.HITOKAGE,
                NewPokemonId = Reverse.Const.creature.Index.IWAAKU
            });
            handler.Handle(new ReplaceStarterCommand
            {
                OldPokemonId = Reverse.Const.creature.Index.CHIKORIITA,
                NewPokemonId = Reverse.Const.creature.Index.POCHIENA
            });

            rom.Save();
        }
        static void Main(string[] args)
        {
            //ChangeStarters();
            //return;

            var rom = new RtdxRom(@"E:\01003D200BAA2000-Edited", PhysicalFileSystem.Instance);
            var luaContext = new SkyEditorLuaContext(rom);
            luaContext.Execute(@"
                local starters = rom:QueryStarters()
                for i = 0,starters.Length-1,1
                do
                    local starter = starters[i]
                    print(i, starter.PokemonName)
                end
            ");
            return;
        }
    }
}
