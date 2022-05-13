using System.Linq;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditorUI.Infrastructure;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.FixedMap.FixedMapCreature;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.FixedMap.FixedMapItem;
using System;
using Cairo;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.FixedMap.FixedMapTile;
using System.Threading;
using System.Collections.Generic;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.FixedMap;

namespace SkyEditorUI.Controllers
{
    class FixedMapController : Widget
    {
        [UI] private ListStore? creaturesStore;
        [UI] private ListStore? itemsStore;
        [UI] private TreeView? creaturesTree;
        [UI] private TreeView? itemsTree;
        [UI] private DrawingArea? drawAreaFixedMap;

        private FixedMapModel fixedMap;
        private IFixedPokemonCollection fixedPokemon;
        private IFixedItemCollection fixedItems;
        private IRtdxRom rom;
        private Modpack modpack;
        private Thread loaderThread;

        private Dictionary<CreatureIndex, PortraitSheet>? portraits;

        private const int IndexColumn = 0;
        private const int ItemColumn = 1;
        private const int QuantityColumn = 2;
        private const int Short04Column = 3;
        private const int Byte06Column = 4;

        private double zoomFactor = 24;

        public FixedMapController(IRtdxRom rom, Modpack modpack, ControllerContext context)
            : this(new Builder("FixedMap.glade"), rom, modpack, context)
        {
        }

        private FixedMapController(Builder builder, IRtdxRom rom, Modpack modpack, ControllerContext context)
            : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            int fixedMapIndex = (context as FixedMapControllerContext)!.Index;
            this.rom = rom;
            this.modpack = modpack;
            fixedMap = rom.GetFixedMapCollection().GetEntryById(fixedMapIndex)!;
            fixedPokemon = rom.GetFixedPokemonCollection();
            fixedItems = rom.GetFixedItemCollection();

            var distinctCreatureIndexes = new HashSet<CreatureIndex>();
            foreach (var creature in fixedMap.Creatures)
            {
                var fixedPokemonModel = fixedPokemon.Entries.ElementAtOrDefault((int) creature.Index);
                AddCreature(creature, fixedPokemonModel);
                if (fixedPokemonModel != null)
                {
                    // TODO: properly show player, partner and other members
                    distinctCreatureIndexes.Add(fixedPokemonModel.PokemonId > 0 
                        ? fixedPokemonModel.PokemonId : CreatureIndex.FUSHIGIDANE);
                }
            }
            foreach (var item in fixedMap.Items)
            {
                AddItem(item);
            }

            ResizeDrawArea();

            // Load Pokémon portraits
            loaderThread = new Thread(() =>
            {
                var portraitSheetsByPokemonIndex = new Dictionary<CreatureIndex, PortraitSheet>();
                var pokemon = rom.GetPokemon();
                var graphics = rom.GetPokemonGraphics();

                foreach (var creatureIndex in distinctCreatureIndexes)
                {
                    var graphicsDbId = pokemon.GetPokemonById(creatureIndex, false)?
                        .PokemonGraphicsDatabaseEntryIds?.FirstOrDefault();
                    if (graphicsDbId != null)
                    {
                        var portraitSheetName = graphics.GetEntryById(graphicsDbId.Value, false)?.PortraitSheetName;
                        if (!string.IsNullOrEmpty(portraitSheetName))
                        {
                            var portraitSheet = PortraitSheet.LoadFromLayeredFs(portraitSheetName, rom, modpack);
                            portraitSheetsByPokemonIndex.Add(creatureIndex, portraitSheet);
                        }
                    };
                }
                Gtk.Application.Invoke((sender, evt) =>
                {
                    this.portraits = portraitSheetsByPokemonIndex;
                    drawAreaFixedMap!.QueueDraw();
                });
            });
            loaderThread.Start();
        }

        protected override void OnDestroyed()
        {
            loaderThread.Join();
            foreach (var portrait in portraits?.Values ?? Enumerable.Empty<PortraitSheet>())
            {
                portrait?.Dispose();
            }
        }

        private void AddCreature(FixedMapCreatureModel model, FixedPokemonModel? fixedPokemon)
        {
            var fixedCreatureName = ((FixedCreatureIndex) model.Index).ToString();
            var formattedId = $"#{((int) model.Index).ToString("000")}";
            var displayName = $"{formattedId} {fixedCreatureName}";
            
            if (fixedPokemon != null)
            {
                displayName += $" ({AutocompleteHelpers.FormatPokemon(rom, fixedPokemon.PokemonId)})";
            }

            creaturesStore!.AppendValues(
                (int) model.Index,
                displayName,
                (int) model.XPos,
                (int) model.YPos,
                model.Direction.ToString(),
                CreatureFactionToString(model.Faction),
                (int) model.Byte02,
                (int) model.Byte03,
                (int) model.Byte07
            );
        }

        private void AddItem(FixedMapItemModel model)
        {
            var displayName = $"#{(model.FixedItemIndex).ToString("000")}";

            var fixedItemModel = fixedItems.Entries.ElementAtOrDefault((int) model.FixedItemIndex);
            if (fixedItemModel != null) {
                displayName += $" ({AutocompleteHelpers.FormatItem(rom, fixedItemModel.Index)})";
            }

            itemsStore!.AppendValues(
                (int) model.FixedItemIndex,
                displayName,
                (int) model.XPos,
                (int) model.YPos,
                ItemVariationToString(model.Variation),
                (int) model.Byte02,
                (int) model.Byte03,
                (int) model.Byte07
            );
        }

        private void OnZoomInClicked(object sender, EventArgs args)
        {
            zoomFactor *= 1.5;
            if (zoomFactor > 128) {
                zoomFactor = 128;
            }
            ResizeDrawArea();
            drawAreaFixedMap!.QueueDraw();
        }

        private void OnZoomOutClicked(object sender, EventArgs args)
        {
            zoomFactor /= 1.5;
            if (zoomFactor < 8) {
                zoomFactor = 8;
            }
            ResizeDrawArea();
            drawAreaFixedMap!.QueueDraw();
        }

        private void OnDrawAreaButtonPressed(object sender, ButtonPressEventArgs args)
        {
            
        }

        private void OnDrawAreaButtonReleased(object sender, ButtonReleaseEventArgs args)
        {
            
        }

        private void OnDrawAreaMotion(object sender, MotionNotifyEventArgs args)
        {
            
        }

        private void OnDrawFixedMap(object sender, DrawnArgs args)
        {
            var cr = args.Cr;
            cr.Scale(zoomFactor, zoomFactor);
            cr.SetSourceColor(new Color(60, 60, 60));
            cr.Paint();
            DrawTiles(cr);
            DrawCreatures(cr);
            DrawGrid(cr);
        }

        private void DrawTiles(Cairo.Context cr)
        {
            for (int y = 0; y < fixedMap.Height; y++)
            {
                for (int x = 0; x < fixedMap.Width; x++)
                {
                    var tile = fixedMap.GetTile(x, y);

                    string? text = null;

                    var color = tile.Type switch
                    {
                        TileType.Wall => (0, 0, 0, 1.0),
                        TileType.Floor => (1, 1, 1, 1.0),
                        TileType.MaybeSecondaryTerrain => (0, 0.7, 1, 1.0),
                        TileType.Chasm => (0.6, 0.6, 0.6, 1.0),
                        TileType.MysteryHouseDoor => (0.1, 1, 0.5, 1.0),
                        _ => (1, 0, 1, 1.0),
                    };
                    if (!Enum.IsDefined(typeof(TileType), tile.Type))
                    {
                        text = ((int) tile.Type).ToString();
                    }

                    var item = fixedMap.GetItem(x, y);
                    if (item != null)
                    {
                        text = "I";
                        color = (1, 0, 0, 0.3);
                    }

                    cr.SetSourceRGBA(color.Item1, color.Item2, color.Item3, color.Item4);
                    cr.Rectangle(x, y, 1, 1);
                    cr.Fill();

                    if (text != null)
                    {
                        cr.Save();
                        cr.MoveTo(x + 0.5, y + 0.5);
                        cr.SetSourceRGB(0, 0, 0);
                        cr.SetFontSize(1);

                        var extents = cr.TextExtents(text);
                        cr.RelMoveTo(-extents.Width / 2, extents.Height / 2);
                        cr.ShowText(text);
                        cr.Restore();
                    }
                }
            }
        }

        private void DrawCreatures(Cairo.Context cr)
        {
            foreach (var creature in fixedMap.Creatures)
            {
                cr.SetSourceRGBA(1, 0, 0, 0.5);
                cr.Rectangle(creature.XPos, creature.YPos, 1, 1);
                cr.Fill();

                cr.Save();
                cr.MoveTo(creature.XPos + 0.5, creature.YPos + 0.5);
                cr.SetSourceRGB(0, 0, 0);
                cr.SetFontSize(1);

                var extents = cr.TextExtents("P");
                cr.RelMoveTo(-extents.Width / 2, extents.Height / 2);
                cr.ShowText("P");
                cr.Restore();

                TryDrawPortrait(cr, creature);
            }
        }

        private void DrawGrid(Cairo.Context cr)
        {
            cr.SetSourceRGBA(0.5, 0.5, 0.5, 0.5);
            cr.LineWidth = 1/zoomFactor;
            for (int y = 0; y <= fixedMap.Height; y++)
            {
                cr.MoveTo(0, y);
                cr.LineTo(fixedMap.Width, y);

                for (int x = 0; x <= fixedMap.Width; x++)
                {
                    cr.MoveTo(x, 0);
                    cr.LineTo(x, fixedMap.Height);
                }
            }
            cr.Stroke();
        }

        private bool TryDrawPortrait(Cairo.Context cr, FixedMapCreatureModel creature)
        {
            lock (this)
            {
                if (portraits == null)
                {
                    return false;
                }

                var fixedPokemonModel = fixedPokemon.Entries.ElementAtOrDefault((int) creature.Index);
                if (fixedPokemonModel == null)
                {
                    return false;
                }

                // TODO: properly show player, partner and other members
                var creatureIndex = fixedPokemonModel.PokemonId > 0 
                        ? fixedPokemonModel.PokemonId : CreatureIndex.FUSHIGIDANE;
                
                if (!portraits.TryGetValue(creatureIndex, out var portraitSheet))
                {
                    return false;
                }

                cr.Save();
                cr.Translate(creature.XPos, creature.YPos);
                cr.Scale(1.0 / 160.0, 1.0 / 160.0);
                portraitSheet.DrawDefaultPortrait(cr, EntityDirectionToRadians(creature.Direction));
                cr.Restore();

                return true;
            }
        }

        private void ResizeDrawArea()
        {
            drawAreaFixedMap!.WidthRequest = (int) (fixedMap.Width * zoomFactor);
            drawAreaFixedMap!.HeightRequest = (int) (fixedMap.Height * zoomFactor);
        }

        private string CreatureFactionToString(CreatureFaction faction)
        {
            return faction switch
            {
                (CreatureFaction) 0 => "(None)",
                CreatureFaction.Player => "Player",
                CreatureFaction.Ally => "Ally",
                CreatureFaction.MysteryHousePokemon => "Mystery House Pokémon",
                (CreatureFaction) 4 => "Unknown 4",
                CreatureFaction.Enemy => "Enemy",
                _ => "Unknown",
            };
        }

        private string ItemVariationToString(ItemVariation variation)
        {
            return variation switch
            {
                (ItemVariation) 0 => "(None)",
                ItemVariation.Item => "Item",
                ItemVariation.Trap => "Trap",
                ItemVariation.StairsDown => "Stairs (Down)",
                ItemVariation.StairsUp => "Stairs (Up)",
                _ => "Unknown",
            };
        }

        private double EntityDirectionToRadians(EntityDirection direction)
        {
            return direction switch
            {
                EntityDirection.Down => 0,
                EntityDirection.Right => 3 * (Math.PI / 2),
                EntityDirection.Up => Math.PI,
                EntityDirection.Left => Math.PI / 2,
                _ => 0
            };
        }
    }
}
