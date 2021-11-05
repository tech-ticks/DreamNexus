using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditorUI.Infrastructure;
using System.Linq;
using SkyEditor.IO.FileSystem;
using IOPath = System.IO.Path;
using AssetStudio;
using SkyEditorUI.Infrastructure.AssetFormats;
using Cairo;
using System.Threading;
using System.IO;

namespace SkyEditorUI.Controllers
{
    partial class PokemonController : Widget
    {
        [UI] private Label? labelIdName;
        [UI] private Entry? entryPokedexNum;
        [UI] private ComboBox? cbTypePrimary;
        [UI] private ComboBox? cbTypeSecondary;
        [UI] private ListStore? creatureCompletionStore;
        [UI] private ListStore? itemCompletionStore;
        [UI] private ListStore? tmCompletionStore;
        [UI] private ListStore? moveCompletionStore;
        [UI] private ListStore? typesStore;
        [UI] private DrawingArea? drawAreaPortrait;

        private LocalizedStringCollection englishStrings;
        private LocalizedStringCollection japaneseStrings;
        private LocalizedStringCollection frenchStrings;
        private LocalizedStringCollection germanStrings;
        private LocalizedStringCollection italianStrings;
        private LocalizedStringCollection spanishStrings;

        private PokemonModel pokemon;
        private IRtdxRom rom;
        private Modpack modpack;
        private Builder builder;
        private ImageSurface? portraitSurface;
        private double portraitZoomFactor = 1;
        private bool nearestNeighborFiltering = false;

        public PokemonController(IRtdxRom rom, Modpack modpack, ControllerContext context)
            : this(new Builder("Pokemon.glade"), rom, modpack, context)
        {
        }

        private PokemonController(Builder builder, IRtdxRom rom, Modpack modpack, ControllerContext context) 
            : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);
            
            var pokemonId = (context as PokemonControllerContext)!.Index;
            this.pokemon = rom.GetPokemon().GetPokemonById(pokemonId)
                ?? throw new ArgumentException("Pokemon from context ID is null", nameof(context));
            this.rom = rom;
            this.modpack = modpack;
            this.builder = builder;

            this.englishStrings = rom.GetStrings().English;
            this.japaneseStrings = rom.GetStrings().Japanese;
            this.frenchStrings = rom.GetStrings().French;
            this.germanStrings = rom.GetStrings().German;
            this.italianStrings = rom.GetStrings().Italian;
            this.spanishStrings = rom.GetStrings().Spanish;

            for (PokemonType i = 0; i <= PokemonType.NASHI; i++)
            {
                typesStore!.AppendValues((int) i, englishStrings.GetPokemonTypeName(i) ?? $"({i.ToString()})");
            }

            creatureCompletionStore!.AppendAll(AutocompleteHelpers.GetPokemon(rom));
            itemCompletionStore!.AppendAll(AutocompleteHelpers.GetItems(rom));
            tmCompletionStore!.AppendAll(Enumerable.Range(
                    (int) ItemIndexConstants.BROKENMACHINE_MIN,
                    (int) (ItemIndexConstants.BROKENMACHINE_MAX - ItemIndexConstants.BROKENMACHINE_MIN))
                .Select(id => AutocompleteHelpers.FormatItem(rom, (ItemIndex) id)));
            moveCompletionStore!.AppendAll(AutocompleteHelpers.GetMoves(rom));

            string formattedId = ((int) pokemonId).ToString("0000");
            string? name = englishStrings.GetPokemonName(pokemonId);
            labelIdName!.Text = $"#{formattedId}: {name} ({pokemonId.ToString()})";
            entryPokedexNum!.Text = pokemon.PokedexNumber.ToString();
            cbTypePrimary!.Active = (int) pokemon.Type1;
            cbTypeSecondary!.Active = (int) pokemon.Type2;

            LoadPortrait();

            LoadNameTab();
            LoadGeneralTab();
            LoadBaseStatsTab();
            LoadEvolutionTab();
            LoadOtherTab();

            LoadStatsTab();
            LoadLevelUpMovesTab();
            LoadTmsTab();
        }

        private void OnPokedexNumChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryPokedexNum!.Text, out short value))
            {
                pokemon.PokedexNumber = value;
            }
            else if (!string.IsNullOrEmpty(entryPokedexNum!.Text))
            {
                entryPokedexNum!.Text = pokemon.PokedexNumber.ToString();
            }
        }

        private void OnPrimaryTypeChanged(object sender, EventArgs args)
        {
            pokemon.Type1 = (PokemonType) cbTypePrimary!.Active;
        }

        private void OnSecondaryTypeChanged(object sender, EventArgs args)
        {
            pokemon.Type2 = (PokemonType) cbTypeSecondary!.Active;
        }

        private void OnImportClicked(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        private void OnExportClicked(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        private void LoadPortrait()
        {
            string portraitSheetName = "dummy";
            if (pokemon.Id > 0)
            {
                var forms = rom.GetPokemonFormDatabase();
                var graphicsDatabase = rom.GetPokemonGraphicsDatabase();
                int graphicsDatabaseIndex = forms.GetGraphicsDatabaseIndex(pokemon.Id, PokemonFormType.NORMAL);
                if (graphicsDatabaseIndex > 0)
                {
                    portraitSheetName = graphicsDatabase.Entries[graphicsDatabaseIndex - 1].PortraitSheetName;
                }
            }

            var assetBundles = rom.GetAssetBundles(false);

            new Thread(() =>
            {
                var relativePortraitPath = IOPath.Combine("ab", $"{portraitSheetName}.ab");
                string? assetBundlePath = null;
                foreach (var mod in modpack!.Mods ?? Enumerable.Empty<Mod>())
                {
                    var bundlePathInMod = IOPath.Combine(mod.GetAssetsDirectory(), relativePortraitPath);
                    if (File.Exists(bundlePathInMod))
                    {
                        assetBundlePath = bundlePathInMod;
                        break;
                    }
                }
                if (assetBundlePath == null)
                {
                    // Load from the ROM if it's not overwritten in any mods
                    assetBundlePath = IOPath.Combine(rom.RomDirectory, "romfs/Data/StreamingAssets/", relativePortraitPath);
                }
                assetBundles.LoadFiles(PhysicalFileSystem.Instance, assetBundlePath);

                System.Console.WriteLine($"Loading {assetBundlePath}");
                var file = assetBundles.assetsFileList[0];
                var texture = file.Objects.OfType<Texture2D>().FirstOrDefault();
                if (texture == null)
                {
                    Console.WriteLine($"Couldn't find Texture2D in portrait AssetBundle");
                    assetBundles.Clear();
                    return;
                }
                
                var encodedData = texture.image_data.GetData();
                assetBundles.Clear();

                byte[] decoded;
                if (texture.m_TextureFormat == TextureFormat.ASTC_RGBA_4x4)
                {
                    decoded = AstcDecoder.DecodeASTC(encodedData, texture.m_Width, texture.m_Height, 4, 4);
                }
                else if (texture.m_TextureFormat == TextureFormat.DXT1)
                {
                    decoded = new byte[texture.m_Width * texture.m_Height * 4]; // RGBA
                    DxtDecoder.DecompressDXT1(encodedData, texture.m_Width, texture.m_Height, decoded);
                }
                else if (texture.m_TextureFormat == TextureFormat.RGB24)
                {
                    decoded = new byte[texture.m_Width * texture.m_Height * 4]; // RGBA
                    RgbConverter.RGB24ToBGRA32(encodedData, texture.m_Width, texture.m_Height, decoded);
                }
                else if (texture.m_TextureFormat == TextureFormat.RGBA32)
                {
                    decoded = new byte[texture.m_Width * texture.m_Height * 4]; // RGBA
                    RgbConverter.RGBA32ToBGRA32(encodedData, texture.m_Width, texture.m_Height, decoded);
                }
                else
                {
                    Console.WriteLine($"Unexpected texture format: {texture.m_TextureFormat}");
                    return;
                }

                if (portraitSurface != null)
                {
                    portraitSurface.Dispose();
                }
                portraitSurface = new ImageSurface(decoded, Format.ARGB32, texture.m_Width, texture.m_Height,
                    4 * texture.m_Width);

                // The expected size is 1024x1024, but portraits can be bigger or smaller
                portraitZoomFactor = 1024 / texture.m_Width;
                System.Console.WriteLine(portraitZoomFactor);
                System.Console.WriteLine(texture.m_Width);
                nearestNeighborFiltering = texture.m_TextureSettings.m_FilterMode == 0;

                GLib.Idle.Add(() => 
                {
                    drawAreaPortrait!.QueueDraw();
                    return false;
                });
            }).Start();
        }

        protected override void OnDestroyed()
        {
            portraitSurface?.Dispose();
        }

        private void OnDrawPortrait(object sender, DrawnArgs args)
        {
            if (portraitSurface == null)
            {
                return;
            }

            args.Cr.Save();
            args.Cr.Scale(0.5, 0.5);
            args.Cr.Scale(portraitZoomFactor, portraitZoomFactor);

            // The image is flipped vertically
            args.Cr.Translate(0, portraitSurface.Height);
            args.Cr.Scale(1, -1);

            args.Cr.SetSourceSurface(portraitSurface, 0, 0);
            using var pattern = args.Cr.GetSource() as SurfacePattern;
            if (pattern != null)
            {
                pattern.Filter = nearestNeighborFiltering ? Filter.Nearest : Filter.Good;
            }
            args.Cr.Paint();
            args.Cr.Restore();

            drawAreaPortrait!.SetSizeRequest(80, 80); // Clips the image
        }
    }
}
