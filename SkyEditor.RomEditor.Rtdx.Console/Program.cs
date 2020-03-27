using Newtonsoft.Json;
using SkyEditor.RomEditor.Rtdx.Domain;
using SkyEditor.RomEditor.Rtdx.Domain.Queries;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using SkyEditor.RomEditor.Rtdx.Reverse;
using System;
using System.IO;

namespace SkyEditor.RomEditor.Rtdx.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = @"D:\01003D200BAA2000";
            var natureDiagnosis = JsonConvert.DeserializeObject<NDConverterSharedData.DataStore>(File.ReadAllText(basePath + @"\romfs\Data\StreamingAssets\data\nature_diagnosis\diagnosis.json"));
            //var actorDataInfoPath = basePath + @"\romfs\Data\StreamingAssets\native_data\pokemon\pokemon_actor_data_info.bin";
            //var actorDataInfo = new PokemonActorDataInfo(File.ReadAllBytes(actorDataInfoPath));

            var graphicsDatabasePath = basePath + @"\romfs\Data\StreamingAssets\native_data\pokemon_graphics_database.bin";
            var graphicsDatabase = new PokemonGraphicsDatabase(File.ReadAllBytes(graphicsDatabasePath));

            var nsoPath = basePath + @"\exefs\main";
            IMainExecutable nso = MainExecutable.LoadFromNso(nsoPath);

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
