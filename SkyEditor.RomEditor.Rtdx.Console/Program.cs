using Newtonsoft.Json;
using SkyEditor.RomEditor.Rtdx.Domain.Constants;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SkyEditor.RomEditor.Rtdx.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var natureDiagnosis = JsonConvert.DeserializeObject<NatureDiagnosis>(File.ReadAllText(@"D:\01003D200BAA2000\romfs\Data\StreamingAssets\data\nature_diagnosis\diagnosis.json"));
            //var actorDataInfoPath = @"D:\01003D200BAA2000\romfs\Data\StreamingAssets\native_data\pokemon\pokemon_actor_data_info.bin";
            //var actorDataInfo = new PokemonActorDataInfo(File.ReadAllBytes(actorDataInfoPath));

            //var graphicsDatabasePath = @"D:\01003D200BAA2000\romfs\Data\StreamingAssets\native_data\pokemon_graphics_database.bin";
            //var graphicsDatabase = new PokemonGraphicsDatabase(File.ReadAllBytes(graphicsDatabasePath));

            //var nsoPath = @"D:\01003D200BAA2000\exefs\main";
            //var nso = MainExecutable.LoadFromNso(nsoPath);

            //var fixedPokemonPath = @"D:\01003D200BAA2000\romfs\Data\StreamingAssets\native_data\dungeon\fixed_pokemon.bin";
            //var fixedPokemon = new FixedPokemon(File.ReadAllBytes(fixedPokemonPath));

            var messageBinPath = @"D:\01003D200BAA2000\romfs\Data\StreamingAssets\native_data\message_us.bin";
            var messageBin = new Farc(File.ReadAllBytes(messageBinPath));
            var common = new MessageBinEntry(messageBin.GetFile("common.bin"), 0);

            var textIdValues = Enum.GetValues(typeof(TextIDHash)).Cast<TextIDHash>().ToDictionary(h => h.ToString("f"), h => (int)h);

            var creatures = Enum.GetValues(typeof(Creature)).Cast<Creature>().ToArray();
            foreach (Creature creature in creatures)
            {
                if (creature == default)
                {
                    continue;
                }

                var nameHash = textIdValues.GetValueOrDefault("POKEMON_NAME__POKEMON_" + creature.ToString("f"));

                // The tagline (e.g. "The Seed Pokémon") has its own ID that doesn't correspond to Pokemon ID or Pokedex ID.
                var descriptionHash = 0;// textIdValues.GetValueOrDefault("POKEMON_TAXIS__SPECIES_" + unknownId.ToString("D").PadLeft(3, '0'));

                Console.WriteLine($"{creature:d} {creature:f} - {common.Strings.GetValueOrDefault(nameHash)} ({common.Strings.GetValueOrDefault(descriptionHash)})");
            }

            var moves = Enum.GetValues(typeof(Waza)).Cast<Waza>().ToArray();
            foreach (Waza waza in moves)
            {
                if (waza == default)
                {
                    continue;
                }

                var nameHash = textIdValues.GetValueOrDefault("WAZA_NAME__WAZA_" + waza.ToString("f"));
                var descriptionHash = textIdValues.GetValueOrDefault("WAZA_EXPLANATION__EXPLAIN_" + waza.ToString("f"));
                Console.WriteLine($"{waza:d} {waza:f} - {common.Strings.GetValueOrDefault(nameHash)} ({common.Strings.GetValueOrDefault(descriptionHash)})");
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
