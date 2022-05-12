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

        private const int IndexColumn = 0;
        private const int ItemColumn = 1;
        private const int QuantityColumn = 2;
        private const int Short04Column = 3;
        private const int Byte06Column = 4;

        private double zoomFactor = 24;

        public FixedMapController(IRtdxRom rom, ControllerContext context) : this(new Builder("FixedMap.glade"), rom, context)
        {
        }

        private FixedMapController(Builder builder, IRtdxRom rom, ControllerContext context) : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            int fixedMapIndex = (context as FixedMapControllerContext)!.Index;
            this.rom = rom;
            fixedMap = rom.GetFixedMapCollection().GetEntryById(fixedMapIndex)!;
            fixedPokemon = rom.GetFixedPokemonCollection();
            fixedItems = rom.GetFixedItemCollection();

            foreach (var creature in fixedMap.Creatures)
            {
                AddCreature(creature);
            }
            foreach (var item in fixedMap.Items)
            {
                AddItem(item);
            }

            ResizeDrawArea();
            drawAreaFixedMap!.QueueDraw();
        }

        private void AddCreature(FixedMapCreatureModel model)
        {
            var fixedCreatureName = ((FixedCreatureIndex) model.Index).ToString();
            var formattedId = $"#{((int) model.Index).ToString("000")}";
            var displayName = $"{formattedId} {fixedCreatureName}";

            var fixedPokemonModel = fixedPokemon.Entries.ElementAtOrDefault((int) model.Index);
            if (fixedPokemonModel != null) {
                displayName += $" ({AutocompleteHelpers.FormatPokemon(rom, fixedPokemonModel.PokemonId)})";
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
            DrawGrid(cr);
        }

        private void DrawTiles(Cairo.Context cr)
        {
            for (int y = 0; y < fixedMap.Height; y++)
            {
                for (int x = 0; x < fixedMap.Width; x++)
                {
                    var tile = fixedMap.GetTile(x, y);

                    // TODO: draw number of tile type if it's an unknown
                    var color = tile.Type switch
                    {
                        TileType.Wall => new Color(0, 0, 0),
                        TileType.Floor => new Color(255, 255, 255),
                        TileType.MaybeSecondaryTerrain => new Color(0, 229, 255),
                        TileType.Chasm => new Color(131, 145, 137),
                        TileType.MysteryHouseDoor => new Color(55, 230, 134),
                        _ => new Color(229, 52, 235),
                    };
                    cr.SetSourceColor(color);

                    cr.Rectangle(x, y, 1, 1);
                    cr.Fill();
                }
            }
        }

        private void DrawGrid(Cairo.Context cr)
        {
            cr.SetSourceColor(new Color(0, 0, 0));
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

        private void ResizeDrawArea()
        {
            drawAreaFixedMap!.WidthRequest = (int) (fixedMap.Width * zoomFactor);
            drawAreaFixedMap!.HeightRequest = (int) (fixedMap.Height * zoomFactor);
        }

        private string CreatureFactionToString(CreatureFaction faction)
        {
            return faction switch {
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
            return variation switch {
                (ItemVariation) 0 => "(None)",
                ItemVariation.Item => "Item",
                ItemVariation.Trap => "Trap",
                ItemVariation.StairsDown => "Stairs (Down)",
                ItemVariation.StairsUp => "Stairs (Up)",
                _ => "Unknown",
            };
        }
    }
}
