using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx
{
    public class RtdxModpack : Modpack
    {
        public RtdxModpack(string path, IFileSystem fileSystem) : base(path, fileSystem)
        {
        }

        public static Modpack CreateInDirectory(ModpackMetadata metadata, string directory, IFileSystem fileSystem)
        {
            void ensureDirectoryExists(string directory)
            {
                if (!fileSystem.DirectoryExists(directory))
                {
                    fileSystem.CreateDirectory(directory);
                }
            }

            string basePath = Path.Combine(directory, metadata.Id ?? "NewModpack");

            if (fileSystem.FileExists(Path.Combine(basePath, "modpack.yaml"))
                || fileSystem.FileExists(Path.Combine(basePath, "modpack.json"))
                || fileSystem.FileExists(Path.Combine(basePath, "mod.json")))
            {
                throw new InvalidOperationException("Cannot create modpack in directory with an existing modpack");
            }

            ensureDirectoryExists(basePath);
            ensureDirectoryExists(Path.Combine(basePath, "Scripts"));

            SaveMetadata(metadata, basePath, fileSystem).Wait();

            return new RtdxModpack(basePath, fileSystem);
        }

        protected async override Task ApplyModels(IModTarget target)
        {
            var rom = (IRtdxRom) target;
            foreach (var mod in Mods ?? Enumerable.Empty<Mod>())
            {
                // TODO: create derived RtdxMod class and move this stuff there
                if (mod.ModelExists("actors.yaml"))
                {
                    rom.SetActors(await mod.LoadModel<ActorCollection>("actors.yaml"));
                }

                if (mod.ModelExists("starters.yaml"))
                {
                    rom.SetStarters(await mod.LoadModel<StarterCollection>("starters.yaml"));
                }

                var pokemon = rom.GetPokemon();
                foreach (var path in mod.GetModelFilesInDirectory("pokemon"))
                {
                    var model = await mod.LoadModel<PokemonModel>(path);
                    pokemon.SetPokemon(model.Id, model);
                }

                var statGrowth = rom.GetStatGrowth();
                foreach (var path in mod.GetModelFilesInDirectory("stat_growth"))
                {
                    var model = await mod.LoadModel<StatGrowthModel>(path);
                    var indexStr = Path.GetFileNameWithoutExtension(path);
                    if (int.TryParse(indexStr, out int index))
                    {
                        statGrowth.SetEntry(index, model);
                    }
                    else
                    {
                        Console.WriteLine($"Ignoring invalid stat growth model file name: {path}");
                    }
                }

                var pokemonGraphics = rom.GetPokemonGraphics();
                foreach (var path in mod.GetModelFilesInDirectory("pokemon_graphics"))
                {
                    var model = await mod.LoadModel<PokemonGraphicsModel>(path);
                    var indexStr = Path.GetFileNameWithoutExtension(path);
                    if (int.TryParse(indexStr, out int index))
                    {
                        pokemonGraphics.SetEntry(index, model);
                    }
                    else
                    {
                        Console.WriteLine($"Ignoring invalid Pokémon graphics model file name: {path}");
                    }
                }

                if (mod.ModelExists("fixed_pokemon.yaml"))
                {
                    rom.SetFixedPokemonCollection(await mod.LoadModel<FixedPokemonCollection>("fixed_pokemon.yaml"));
                }

                if (mod.ModelExists("fixed_items.yaml"))
                {
                    rom.SetFixedPokemonCollection(await mod.LoadModel<FixedPokemonCollection>("fixed_items.yaml"));
                }

                var dungeons = rom.GetDungeons();
                for (int i = 1; i < (int) DungeonIndex.END; i++)
                {
                    await LoadDungeon(mod, rom, (DungeonIndex) i, dungeons);
                }

                var items = rom.GetItems();
                foreach (var path in mod.GetModelFilesInDirectory("item"))
                {
                    var model = await mod.LoadModel<ItemDataInfo.Entry>(path);
                    items.SetItem(model.Index, model);
                }

                var moves = rom.GetMoves();
                foreach (var path in mod.GetModelFilesInDirectory("move"))
                {
                    var model = await mod.LoadModel<WazaDataInfo.Entry>(path);
                    moves.SetMove(model.Index, model);
                }

                if (mod.ModelExists("charged_moves.yaml"))
                {
                    rom.SetChargedMoveCollection(await mod.LoadModel<ChargedMoveCollection>("charged_moves.yaml"));
                }

                if (mod.ModelExists("extra_large_moves.yaml"))
                {
                    rom.SetExtraLargeMoveCollection(await mod.LoadModel<ExtraLargeMoveCollection>("extra_large_moves.yaml"));
                }

                var actions = rom.GetActions();
                foreach (var path in mod.GetModelFilesInDirectory("actions"))
                {
                    var model = await mod.LoadModel<ActionModel>(path);
                    var indexStr = Path.GetFileNameWithoutExtension(path);
                    if (int.TryParse(indexStr, out int index))
                    {
                        actions.SetAction(index, model);
                    }
                    else
                    {
                        Console.WriteLine($"Ignoring invalid action model file name: {path}");
                    }
                }

                if (mod.ModelExists("action_stat_modifiers.yaml"))
                {
                    rom.SetActionStatModifiers(await mod.LoadModel<ActionStatModifierCollection>(
                        "action_stat_modifiers.yaml"));
                }

                if (mod.ModelExists("dungeon_maps.yaml"))
                {
                    rom.SetDungeonMaps(await mod.LoadModel<DungeonMapCollection>("dungeon_maps.yaml"));
                }
                if (mod.ModelExists("dungeon_music.yaml"))
                {
                    rom.SetDungeonMusic(await mod.LoadModel<DungeonMusicCollection>("dungeon_music.yaml"));
                }

                await LoadStrings(mod, rom);
            }
        }

        private async Task LoadDungeon(Mod mod, IModTarget rom, DungeonIndex index, IDungeonCollection dungeons)
        {
            string dungeonFolder = Path.Combine("dungeons", index.ToString());
            string mainDataPath = Path.Combine(dungeonFolder, "dungeon.yaml");
            if (mod.ModelExists(mainDataPath))
            {
                var dungeonModel = await mod.LoadModel<DungeonModel>(mainDataPath);

                var itemSetsPath = Path.Combine(dungeonFolder, "itemsets");
                foreach (var path in mod.GetModelFilesInDirectory(itemSetsPath)
                    .OrderBy(path => Path.GetFileNameWithoutExtension(path)))
                {
                    dungeonModel.ItemSets.Add(await mod.LoadModel<ItemSetModel>(path));
                }

                var floorsPath = Path.Combine(dungeonFolder, "floors");
                var floorModels = new List<DungeonFloorModel>();
                foreach (var path in mod.GetModelFilesInDirectory(floorsPath))
                {
                    var model = await mod.LoadModel<DungeonFloorModel>(path);
                    floorModels.Add(model);
                }

                var sortedFloorModels = floorModels
                    .OrderBy(model => model.Index).ToArray();

                // Floor -1 must be added last
                dungeonModel.Floors.AddRange(sortedFloorModels.Where(model => model.Index > -1));
                dungeonModel.Floors.AddRange(sortedFloorModels.Where(model => model.Index <= -1));

                dungeons.SetDungeon(index, dungeonModel);
            }
        }

        private async Task LoadStrings(Mod mod, IRtdxRom rom)
        {
            for (LanguageType i = (LanguageType) 0; i < LanguageType.MAX; i++)
            {
                string languagePath = Path.Combine("strings", i.ToString().ToLower());
                if (!mod.ModelDirectoryExists(languagePath))
                {
                    continue;
                }
                
                var strings = rom.GetStrings().GetStringsForLanguage(i);

                string commonPath = Path.Combine(languagePath, "common.yaml");
                if (mod.ModelExists(commonPath))
                {
                    strings.CommonStringsOverride = await mod.LoadModel<Dictionary<TextIDHash, string>>(commonPath);
                }
            
                string dungeonPath = Path.Combine(languagePath, "dungeon.yaml");
                if (mod.ModelExists(dungeonPath))
                {
                    strings.DungeonStringsOverride = await mod.LoadModel<Dictionary<int, string>>(dungeonPath);
                }
            
                string scriptPath = Path.Combine(languagePath, "script.yaml");
                if (mod.ModelExists(scriptPath))
                {
                    strings.ScriptStringsOverride = await mod.LoadModel<Dictionary<int, string>>(scriptPath);
                }
            }
        }

        protected async override Task SaveModels(IModTarget target)
        {
            var rom = (IRtdxRom) target;

            // Models can only be automatically applied to the first mod
            var mod = Mods?.FirstOrDefault();
            if (mod != null)
            {
                var tasks = new List<Task>();

                // TODO: create derived RtdxMod class and move this stuff there
                if (rom.ActorsModified)
                {
                    tasks.Add(mod.SaveModel(rom.GetActors(), "actors.yaml"));
                }

                if (rom.StartersModified)
                {
                    tasks.Add(mod.SaveModel(rom.GetStarters(), "starters.yaml"));
                }

                if (rom.PokemonModified)
                {
                    var pokemon = rom.GetPokemon();
                    foreach (var model in pokemon.LoadedPokemon.Where(model => pokemon.IsPokemonDirty(model.Key)))
                    {
                        string path = Path.Combine("pokemon", $"{model.Key.ToString()}.yaml");
                        tasks.Add(mod.SaveModel(model.Value, path));
                    }
                }

                if (rom.StatGrowthModified)
                {
                    var collection = rom.GetStatGrowth();
                    foreach (var model in collection.LoadedEntries.Where(model => collection.IsEntryDirty(model.Key)))
                    {
                        string path = Path.Combine("stat_growth", $"{model.Key.ToString()}.yaml");
                        tasks.Add(mod.SaveModel(model.Value, path));
                    }
                }

                if (rom.PokemonGraphicsModified)
                {
                    var collection = rom.GetPokemonGraphics();
                    foreach (var model in collection.LoadedEntries.Where(model => collection.IsEntryDirty(model.Key)))
                    {
                        string path = Path.Combine("pokemon_graphics", $"{model.Key.ToString()}.yaml");
                        tasks.Add(mod.SaveModel(model.Value, path));
                    }
                }

                if (rom.FixedPokemonModified)
                {
                    tasks.Add(mod.SaveModel(rom.GetFixedPokemonCollection(), "fixed_pokemon.yaml"));
                }

                if (rom.FixedItemsModified)
                {
                    tasks.Add(mod.SaveModel(rom.GetFixedItemCollection(), "fixed_items.yaml"));
                }

                if (rom.ItemsModified)
                {
                    var items = rom.GetItems();
                    foreach (var model in items.LoadedItems.Where(model => items.IsItemDirty(model.Key)))
                    {
                        string path = Path.Combine("item", $"{model.Key.ToString()}.yaml");
                        tasks.Add(mod.SaveModel(model.Value, path));
                    }
                }

                if (rom.MovesModified)
                {
                    var moves = rom.GetMoves();
                    foreach (var model in moves.LoadedMoves.Where(model => moves.IsMoveDirty(model.Key)))
                    {
                        string path = Path.Combine("move", $"{model.Key.ToString()}.yaml");
                        tasks.Add(mod.SaveModel(model.Value, path));
                    }
                }

                if (rom.ChargedMovesModified)
                {
                    tasks.Add(mod.SaveModel(rom.GetChargedMoves(), "charged_moves.yaml"));
                }

                if (rom.ExtraLargeMovesModified)
                {
                    tasks.Add(mod.SaveModel(rom.GetExtraLargeMoveCollection(), "extra_large_moves.yaml"));
                }

                if (rom.ActionsModified)
                {
                    var actions = rom.GetActions();
                    foreach (var model in actions.LoadedActions.Where(model => actions.IsActionDirty(model.Key)))
                    {
                        string path = Path.Combine("actions", $"{model.Key.ToString()}.yaml");
                        tasks.Add(mod.SaveModel(model.Value, path));
                    }
                }

                if (rom.ActionStatModifiersModified)
                {
                    tasks.Add(mod.SaveModel(rom.GetActionStatModifiers(), "action_stat_modifiers.yaml"));
                }

                if (rom.DungeonsModified)
                {
                    var dungeons = rom.GetDungeons();
                    foreach (var dungeon in dungeons.LoadedDungeons.Where(dungeon => dungeons.IsDungeonDirty(dungeon.Key)))
                    {
                        string dungeonFolder = Path.Combine("dungeons", dungeon.Key.ToString());
                        string mainDataPath = Path.Combine(dungeonFolder, "dungeon.yaml");
                        tasks.Add(mod.SaveModel(dungeon.Value, mainDataPath));

                        for (int i = 0; i < dungeon.Value.ItemSets.Count; i++)
                        {
                            var itemSet = dungeon.Value.ItemSets[i];
                            string path = Path.Combine(dungeonFolder, "itemsets", $"{i:D2}.yaml");
                            tasks.Add(mod.SaveModel(itemSet, path));
                        }
                        for (int i = 0; i < dungeon.Value.Floors.Count; i++)
                        {
                            var floor = dungeon.Value.Floors[i];
                            string path = Path.Combine(dungeonFolder, "floors", $"{floor.Index:D2}.yaml");
                            tasks.Add(mod.SaveModel(floor, path));
                        }
                    }
                }

                if (rom.DungeonMapsModified)
                {
                    tasks.Add(mod.SaveModel(rom.GetDungeonMaps(), "dungeon_maps.yaml"));
                }

                if (rom.DungeonMusicModified)
                {
                    tasks.Add(mod.SaveModel(rom.GetDungeonMusic(), "dungeon_music.yaml"));
                }

                if (rom.StringsModified)
                {
                    foreach (var strings in rom.GetStrings().LoadedLanguages)
                    {
                        string languagePath = Path.Combine("strings", strings.Key.ToString().ToLower());
                        
                        string commonPath = Path.Combine(languagePath, "common.yaml");
                        tasks.Add(mod.SaveModel(strings.Value.CommonStringsOverride, commonPath));
                    
                        string dungeonPath = Path.Combine(languagePath, "dungeon.yaml");
                        tasks.Add(mod.SaveModel(strings.Value.DungeonStringsOverride, dungeonPath));
                    
                        string scriptPath = Path.Combine(languagePath, "script.yaml");
                        tasks.Add(mod.SaveModel(strings.Value.ScriptStringsOverride, scriptPath));
                    }
                }

                await Task.WhenAll(tasks);
            }
        }
    }
}
