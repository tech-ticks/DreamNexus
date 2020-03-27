using Newtonsoft.Json;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Rtdx.Domain;
using SkyEditor.RomEditor.Rtdx.Domain.Commands;
using SkyEditor.RomEditor.Rtdx.Domain.Handlers;
using SkyEditor.RomEditor.Rtdx.Domain.Queries;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Reverse;
using System;
using System.IO;

namespace SkyEditor.RomEditor.Rtdx.ConsoleApp
{
    class Program
    {
        private static void ChangeStarters()
        {
            var rom = new PhysicalFileSystem();
            rom.WorkingDirectory = @"F:\atmosphere\contents\01003D200BAA2000";

            var handler = new ReplaceStarterHandler(rom);
            // To-do: make a handler to do all this at once
            // As it is now, each file is loaded/saved each time
            handler.Handle(new ReplaceStarterCommand
            {
                OldPokemonId = Reverse.Const.creature.Index.FUSHIGIDANE,
                NewPokemonId = Reverse.Const.creature.Index.MYUU
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
        }
        static void Main(string[] args)
        {
            var basePath = @"D:\01003D200BAA2000";
            var natureDiagnosis = JsonConvert.DeserializeObject<NDConverterSharedData.DataStore>(File.ReadAllText(basePath + @"\romfs\Data\StreamingAssets\data\nature_diagnosis\diagnosis.json"));
            //var actorDataInfoPath = basePath + @"\romfs\Data\StreamingAssets\native_data\pokemon\pokemon_actor_data_info.bin";
            //var actorDataInfo = new PokemonActorDataInfo(File.ReadAllBytes(actorDataInfoPath));

            var graphicsDatabasePath = basePath + @"\romfs\Data\StreamingAssets\native_data\pokemon_graphics_database.bin";
            var graphicsDatabase = new PokemonGraphicsDatabase(File.ReadAllBytes(graphicsDatabasePath));

            var nsoPath = basePath + @"\exefs\main";
            IMainExecutable nso = MainExecutable.LoadFromNso(File.ReadAllBytes(nsoPath));

            var fixedPokemonPath = basePath + @"\romfs\Data\StreamingAssets\native_data\dungeon\fixed_pokemon.bin";
            IFixedPokemon fixedPokemon = new FixedPokemon(File.ReadAllBytes(fixedPokemonPath));

            var messageBinPath = basePath + @"\romfs\Data\StreamingAssets\native_data\message_us.bin";
            var messageBin = new Farc(File.ReadAllBytes(messageBinPath));
            var common = new MessageBinEntry(messageBin.GetFile("common.bin"));

            ICommonStrings commonStrings = new CommonStrings(common);
            IStarterQueries starterQueries = new StarterQueries(commonStrings, nso, natureDiagnosis, fixedPokemon);

            Console.WriteLine("Starters:");
            var starters = starterQueries.GetStarters();
            foreach (var starter in starters)
            {
                Console.WriteLine(starter.PokemonName);
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
