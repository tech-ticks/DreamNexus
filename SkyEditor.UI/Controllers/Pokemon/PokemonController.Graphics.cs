using System;
using System.Threading;
using System.Linq;
using System.IO;
using Gtk;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using UI = Gtk.Builder.ObjectAttribute;
using IOPath = System.IO.Path;
using AssetsTools.NET;
using SkyEditorUI.Infrastructure.AssetFormats;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using Cairo;
using Flags = SkyEditor.RomEditor.Domain.Rtdx.Structures.PokemonGraphicsDatabase.PokemonGraphicsDatabaseEntry.PokemonGraphicsDatabaseEntryFlags;
using SkyEditor.RomEditor.Infrastructure;
using SkyEditorUI.Infrastructure;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using AssetsTools.NET.Extra;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Runtime.InteropServices;

namespace SkyEditorUI.Controllers
{
    partial class PokemonController : Widget
    {
        [UI] private ComboBoxText? cbFormType;
        [UI] private SpinButton? sbGraphicsDatabaseId;
        [UI] private Notebook? notebookGraphicsSettings;

        [UI] private Entry? entryModelBundleName;
        [UI] private Entry? entryBaseFormModelName;
        [UI] private Entry? entryAnimationBundleName;
        [UI] private Entry? entryPortraitSheetName;
        [UI] private Entry? entryCampImageName;
        [UI] private Entry? entryCampImageNameReverse;

        [UI] private Entry? entryGroundBaseScale;
        [UI] private Entry? entryDungeonBaseScale;
        [UI] private Entry? entryGraphicsFloat38;
        [UI] private Entry? entryGraphicsFloat3C;
        [UI] private Entry? entryGraphicsFloat40;
        [UI] private Entry? entryGraphicsFloat44;
        [UI] private Entry? entryYOffset;
        [UI] private Entry? entryWalkAnimationSpeed;
        [UI] private Entry? entryGraphicsFloat50;
        [UI] private Entry? entryRunSpeedRatio;
        [UI] private Entry? entryGraphicsFloat58;
        [UI] private Entry? entryGraphicsFloat5C;
        [UI] private Entry? entryGraphicsFloat60;
        [UI] private Entry? entryGraphicsFloat64;
        [UI] private Entry? entryGraphicsInt78;
        [UI] private Entry? entryGraphicsInt7C;
        [UI] private Entry? entryGraphicsInt80;
        [UI] private Entry? entryGraphicsFloat84;
        [UI] private Entry? entryGraphicsFloat88;
        [UI] private Entry? entryGraphicsFloat8C;
        [UI] private Entry? entryGraphicsFloat90;
        [UI] private Entry? entryGraphicsFloat94;
        [UI] private Entry? entryGraphicsFloat98;
        [UI] private Entry? entryGraphicsFloat9C;
        [UI] private Entry? entryGraphicsFloatA0;

        [UI] private ComboBoxText? cbUnkSize1;
        [UI] private ComboBoxText? cbUnkSize2;

        private int currentFormType;
        private IPokemonGraphicsCollection? graphicsCollection;
        private PokemonGraphicsModel? graphicsModel = null;
        private string? loadedPortraitBundleName;

        private ImageSurface? portraitSurface;
        private double portraitZoomFactor = 1;
        private bool nearestNeighborFiltering = false;

        private void LoadGraphicsTab()
        {
            graphicsCollection = rom.GetPokemonGraphics();
            sbGraphicsDatabaseId!.Adjustment.Upper = graphicsCollection.Count - 1;

            if (pokemon.PokemonGraphicsDatabaseEntryIds != null)
            {
                cbFormType!.Active = 0;
            }
            else
            {
                cbFormType!.Sensitive = false;
                sbGraphicsDatabaseId!.Sensitive = false;
                notebookGraphicsSettings!.Sensitive = false;
            }
        }

        private void LoadGraphicsModel(int id)
        {
            graphicsModel = id > 0 && id < graphicsCollection!.Count ? graphicsCollection!.GetEntryById(id) : null;
            notebookGraphicsSettings!.Sensitive = graphicsModel != null;

            if (graphicsModel == null)
            {
                return;
            }

            entryModelBundleName!.Text = graphicsModel.ModelName;
            entryBaseFormModelName!.Text = graphicsModel.BaseFormModelName;
            entryAnimationBundleName!.Text = graphicsModel.AnimationName;
            entryPortraitSheetName!.Text = graphicsModel.PortraitSheetName;
            entryCampImageName!.Text = graphicsModel.RescueCampSheetName;
            entryCampImageNameReverse!.Text = graphicsModel.RescueCampSheetReverseName;

            if (!string.IsNullOrWhiteSpace(graphicsModel!.PortraitSheetName))
            {
                LoadPortrait();
            }

            entryGroundBaseScale!.Text = graphicsModel.BaseScale.ToString();
            entryDungeonBaseScale!.Text = graphicsModel.DungeonBaseScale.ToString();
            entryGraphicsFloat38!.Text = graphicsModel.UnkX38.ToString();
            entryGraphicsFloat3C!.Text = graphicsModel.UnkX3C.ToString();
            entryGraphicsFloat40!.Text = graphicsModel.UnkX40.ToString();
            entryGraphicsFloat44!.Text = graphicsModel.UnkX44.ToString();
            entryYOffset!.Text = graphicsModel.YOffset.ToString();
            entryWalkAnimationSpeed!.Text = graphicsModel.WalkSpeedDistance.ToString();
            entryGraphicsFloat50!.Text = graphicsModel.UnkX50.ToString();
            entryRunSpeedRatio!.Text = graphicsModel.RunSpeedRatioGround.ToString();
            entryGraphicsFloat58!.Text = graphicsModel.UnkX58.ToString();
            entryGraphicsFloat5C!.Text = graphicsModel.UnkX5C.ToString();
            entryGraphicsFloat60!.Text = graphicsModel.UnkX60.ToString();
            entryGraphicsFloat64!.Text = graphicsModel.UnkX64.ToString();
            entryGraphicsInt78!.Text = graphicsModel.UnkX78.ToString();
            entryGraphicsInt7C!.Text = graphicsModel.UnkX7C.ToString();
            entryGraphicsInt80!.Text = graphicsModel.UnkX80.ToString();
            entryGraphicsFloat84!.Text = graphicsModel.UnkX84.ToString();
            entryGraphicsFloat88!.Text = graphicsModel.UnkX88.ToString();
            entryGraphicsFloat8C!.Text = graphicsModel.UnkX8C.ToString();
            entryGraphicsFloat90!.Text = graphicsModel.UnkX90.ToString();
            entryGraphicsFloat94!.Text = graphicsModel.UnkX94.ToString();
            entryGraphicsFloat98!.Text = graphicsModel.UnkX98.ToString();
            entryGraphicsFloat9C!.Text = graphicsModel.UnkX9C.ToString();
            entryGraphicsFloatA0!.Text = graphicsModel.UnkXA0.ToString();

            cbUnkSize1!.Active = (int) graphicsModel.UnknownBodyType1;
            cbUnkSize2!.Active = (int) graphicsModel.UnknownBodyType2;

            for (int i = 0; i <= 12; i++)
            {
                var flagSwitch = (Switch) builder.GetObject($"switchGraphicsFlag{i}");
                var flag = (Flags) (1 << i);
                flagSwitch.Active = graphicsModel.Flags.HasFlag(flag);
            }
        }

        private void OnFormTypeChanged(object sender, EventArgs args)
        {
            currentFormType = cbFormType!.Active;
            if (pokemon!.PokemonGraphicsDatabaseEntryIds == null)
            {
                graphicsModel = null;
            }
            else
            {
                int currentGraphicsDatabaseId = pokemon.PokemonGraphicsDatabaseEntryIds[currentFormType];
                sbGraphicsDatabaseId!.Value = currentGraphicsDatabaseId;
            }
        }

        private void OnGraphicsDatabaseIdChanged(object sender, EventArgs args)
        {
            var ids = pokemon!.PokemonGraphicsDatabaseEntryIds;
            if (ids == null)
            {
                return;
            }
            ids[currentFormType] = (short) sbGraphicsDatabaseId!.ValueAsInt;
            LoadGraphicsModel(ids[currentFormType]);
        }

        private void OnModelBundleNameChanged(object sender, EventArgs args)
        {
            graphicsModel!.ModelName = entryModelBundleName!.Text;
        }

        private void OnBaseFormModelNameChanged(object sender, EventArgs args)
        {
            graphicsModel!.BaseFormModelName = entryBaseFormModelName!.Text;
        }

        private void OnAnimationBundleNameChanged(object sender, EventArgs args)
        {
            graphicsModel!.AnimationName = entryAnimationBundleName!.Text;
        }

        private void OnPortraitSheetNameChanged(object sender, EventArgs args)
        {
            graphicsModel!.PortraitSheetName = entryPortraitSheetName!.Text;
        }

        private void OnPortraitSheetNameFocusOut(object sender, FocusOutEventArgs args)
        {
            LoadPortrait();
        }

        private void OnCampImageNameChanged(object sender, EventArgs args)
        {
            graphicsModel!.RescueCampSheetName = entryCampImageName!.Text;
        }

        private void OnCampImageNameReverseChanged(object sender, EventArgs args)
        {
            graphicsModel!.RescueCampSheetReverseName = entryCampImageNameReverse!.Text;
        }

        private void OnGroundBaseScaleChanged(object sender, EventArgs args)
        {
            graphicsModel!.BaseScale = entryGroundBaseScale!.ParseFloat(graphicsModel!.BaseScale);
        }

        private void OnDungeonBaseScaleChanged(object sender, EventArgs args)
        {
            graphicsModel!.DungeonBaseScale = entryDungeonBaseScale!.ParseFloat(graphicsModel!.DungeonBaseScale);
        }

        private void OnGraphicsFloat38Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX38 = entryGraphicsFloat38!.ParseFloat(graphicsModel!.UnkX38);
        }

        private void OnGraphicsFloat3CChanged(object sender, EventArgs args)
        {
            graphicsModel!.UnkX3C = entryGraphicsFloat3C!.ParseFloat(graphicsModel!.UnkX3C);
        }

        private void OnGraphicsFloat40Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX40 = entryGraphicsFloat40!.ParseFloat(graphicsModel!.UnkX40);
        }

        private void OnGraphicsFloat44Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX44 = entryGraphicsFloat44!.ParseFloat(graphicsModel!.UnkX44);
        }

        private void OnYOffsetChanged(object sender, EventArgs args)
        {
            graphicsModel!.YOffset = entryYOffset!.ParseFloat(graphicsModel!.YOffset);
        }

        private void OnWalkAnimationSpeedChanged(object sender, EventArgs args)
        {
            graphicsModel!.WalkSpeedDistance = entryWalkAnimationSpeed!.ParseFloat(graphicsModel!.WalkSpeedDistance);
        }

        private void OnGraphicsFloat50Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX50 = entryGraphicsFloat50!.ParseFloat(graphicsModel!.UnkX50);
        }

        private void OnRunSpeedRatioChanged(object sender, EventArgs args)
        {
            graphicsModel!.RunSpeedRatioGround = entryRunSpeedRatio!.ParseFloat(graphicsModel!.RunSpeedRatioGround);
        }

        private void OnGraphicsFloat58Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX58 = entryGraphicsFloat58!.ParseFloat(graphicsModel!.UnkX58);
        }

        private void OnGraphicsFloat5CChanged(object sender, EventArgs args)
        {
            graphicsModel!.UnkX5C = entryGraphicsFloat5C!.ParseFloat(graphicsModel!.UnkX5C);
        }

        private void OnGraphicsFloat60Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX60 = entryGraphicsFloat60!.ParseFloat(graphicsModel!.UnkX60);
        }

        private void OnGraphicsFloat64Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX64 = entryGraphicsFloat64!.ParseFloat(graphicsModel!.UnkX64);
        }

        private void OnGraphicsInt78Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX78 = entryGraphicsInt78!.ParseInt(graphicsModel!.UnkX78);
        }

        private void OnGraphicsInt7CChanged(object sender, EventArgs args)
        {
            graphicsModel!.UnkX7C = entryGraphicsInt7C!.ParseInt(graphicsModel!.UnkX7C);
        }

        private void OnGraphicsInt80Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX80 = entryGraphicsInt80!.ParseInt(graphicsModel!.UnkX80);
        }

        private void OnGraphicsFloat84Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX84 = entryGraphicsFloat84!.ParseFloat(graphicsModel!.UnkX84);
        }

        private void OnGraphicsFloat88Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX88 = entryGraphicsFloat88!.ParseFloat(graphicsModel!.UnkX88);
        }

        private void OnGraphicsFloat8CChanged(object sender, EventArgs args)
        {
            graphicsModel!.UnkX8C = entryGraphicsFloat8C!.ParseFloat(graphicsModel!.UnkX8C);
        }

        private void OnGraphicsFloat90Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX90 = entryGraphicsFloat90!.ParseFloat(graphicsModel!.UnkX90);
        }

        private void OnGraphicsFloat94Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX94 = entryGraphicsFloat94!.ParseFloat(graphicsModel!.UnkX94);
        }

        private void OnGraphicsFloat98Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkX98 = entryGraphicsFloat98!.ParseFloat(graphicsModel!.UnkX98);
        }

        private void OnGraphicsFloat9CChanged(object sender, EventArgs args)
        {
            graphicsModel!.UnkX9C = entryGraphicsFloat9C!.ParseFloat(graphicsModel!.UnkX9C);
        }

        private void OnGraphicsFloatA0Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnkXA0 = entryGraphicsFloat64!.ParseFloat(graphicsModel!.UnkXA0);
        }

        private void OnUnkSize1Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnknownBodyType1 = (GraphicsBodySizeType) cbUnkSize1!.Active;
        }

        private void OnUnkSize2Changed(object sender, EventArgs args)
        {
            graphicsModel!.UnknownBodyType2 = (GraphicsBodySizeType) cbUnkSize2!.Active;
        }

        [GLib.ConnectBefore]
        private void OnGraphicsFlagSet(object sender, StateSetArgs args)
        {
            var flagSwitch = (Switch) sender;

            // Extract flag from switch name
            var flagIndex = int.Parse(flagSwitch.Name.Replace("switchGraphicsFlag", ""));
            var flag = (Flags) (1 << flagIndex);
            graphicsModel!.Flags = graphicsModel!.Flags.SetFlag(flag, flagSwitch.Active);
        }

        private void OnImportPortraitClicked(object sender, EventArgs args)
        {
            if (modpack.ReadOnly)
            {
                throw new Exception("Not supported with read-only modpacks.");
            }

            var fileDialog = new FileChooserNative("Select a portrait to import (.png)", MainWindow.Instance,
                FileChooserAction.Open, null, null);
            using var filter = new FileFilter();
            filter.AddPattern("*.png");
            fileDialog.AddFilter(filter);

            var response = (ResponseType) fileDialog.Run();

            if (response != ResponseType.Accept)
            {
                fileDialog.Destroy();
                return;
            }

            var path = fileDialog.Filename;
            fileDialog.Destroy();

            string fileName;
            do
            {
                var bundleNameDialog = new MessageDialog(MainWindow.Instance, DialogFlags.Modal, MessageType.Other, ButtonsType.Ok,
                    "Enter a file name for the asset bundle that will be generated.\n"
                    + "Only alphanumeric characters and '_' are allowed.\n"
                    + "Any other asset bundles with this name will be REPLACED, including bundles that don't contain portraits.\n"
                    + "A unique name such as 'portrait_[Pokédex number]_[form]' is recommended.\n"
                    + "To delete the generated bundle, open the modpack in a file browser and delete the file "
                    + "'Assets/ab/[bundle name].ab'.");
                bundleNameDialog.Title = "Portrait import";

                var entry = new Entry();
                entry.Text = $"portrait_{pokemon.PokedexNumber}_form{currentFormType}";
                bundleNameDialog.ContentArea.PackEnd(entry, false, false, 0);

                bundleNameDialog.ShowAll();
                response = (ResponseType) bundleNameDialog.Run();
                fileName = entry.Text;
                bundleNameDialog.Destroy();

                if (response != ResponseType.Ok)
                {
                    return;
                }

                if (Regex.IsMatch(fileName, "^\\w+$"))
                {
                    break;
                }
                else
                {
                    UIUtils.ShowErrorDialog(MainWindow.Instance, "Portrait import",
                        "The file name must only contain alphanumeric characters or '_'.");
                }
            }
            while (true);

            bool created = CreatePortraitBundle(path, fileName);

            if (graphicsModel != null && created)
            {
                graphicsModel.PortraitSheetName = fileName;
                entryPortraitSheetName!.Text = fileName;
                LoadPortrait();
            }
        }

        private void OnExportPortraitClicked(object sender, EventArgs args)
        {
            if (portraitSurface == null)
            {
                UIUtils.ShowErrorDialog(MainWindow.Instance, "Portrait export error", "No portrait loaded.");
                return;
            }

            var dialog = new FileChooserNative("Portrait export (.png)", MainWindow.Instance, FileChooserAction.Save, null, null);
            using var filter = new FileFilter();
            filter.AddPattern("*.png");
            dialog.AddFilter(filter);

            var response = (ResponseType) dialog.Run();

            if (response != ResponseType.Accept)
            {
                dialog.Destroy();
                return;
            }

            var path = dialog.Filename;
            dialog.Destroy();

            lock (this)
            {
                // portraitSurface.SaveAsPng() leads to corrupted transparency for some reason
                using var image = SixLabors.ImageSharp.Image.LoadPixelData<Bgra32>(portraitSurface.Data,
                    portraitSurface.Width, portraitSurface.Height);
                image.Mutate(x => x.Flip(FlipMode.Vertical));
                image.SaveAsPng(path);
            }
        }
        private void LoadPortrait()
        {
            string portraitSheetName = "dummy";
            if (graphicsModel != null)
            {
                portraitSheetName = graphicsModel.PortraitSheetName;
            }

            if (portraitSheetName == loadedPortraitBundleName)
            {
                // Already displayed
                return;
            }

            new Thread(() =>
            {
                lock (this)
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

                    var manager = new AssetsManager();
                    try
                    {
                        var bundle = manager.LoadBundleFile(assetBundlePath);
                        var texture = AssetBundleHelpers.LoadFirstTextureFromBundle(manager, bundle);

                        if (texture == null)
                        {
                            Console.WriteLine($"Failed to load texture from bundle.");
                            manager.UnloadAll(true);
                            HidePortraitsIdle();
                            return;
                        }

                        loadedPortraitBundleName = portraitSheetName;
                        var encodedData = texture.GetTextureDataRaw(bundle.file);
                        if (encodedData == null)
                        {
                            Console.WriteLine($"Failed to load texture file.");
                            manager.UnloadAll(true);
                            HidePortraitsIdle();
                            return;
                        }
                        byte[] decoded;
                        if ((TextureFormat) texture.m_TextureFormat == TextureFormat.ASTC_RGBA_4x4)
                        {
                            decoded = AstcDecoder.DecodeASTC(encodedData, texture.m_Width, texture.m_Height, 4, 4);
                        }
                        else if ((TextureFormat) texture.m_TextureFormat == TextureFormat.DXT1)
                        {
                            decoded = new byte[texture.m_Width * texture.m_Height * 4]; // RGBA
                            DxtDecoder.DecompressDXT1(encodedData, texture.m_Width, texture.m_Height, decoded);
                        }
                        else if ((TextureFormat) texture.m_TextureFormat == TextureFormat.RGB24)
                        {
                            decoded = new byte[texture.m_Width * texture.m_Height * 4]; // RGBA
                            RgbConverter.RGB24ToBGRA32(encodedData, texture.m_Width, texture.m_Height, decoded);
                        }
                        else if ((TextureFormat) texture.m_TextureFormat == TextureFormat.RGBA32)
                        {
                            decoded = new byte[texture.m_Width * texture.m_Height * 4]; // RGBA
                            RgbConverter.RGBA32ToBGRA32(encodedData, texture.m_Width, texture.m_Height, decoded);
                        }
                        else
                        {
                            Console.WriteLine($"Unexpected texture format: {texture.m_TextureFormat}");
                            HidePortraitsIdle();
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
                        nearestNeighborFiltering = texture.m_TextureSettings.m_FilterMode == 0;

                        GLib.Idle.Add(() => 
                        {
                            drawAreaPortrait!.Visible = true;
                            drawAreaPortraitFull!.Visible = true;
                            drawAreaPortrait!.QueueDraw();
                            drawAreaPortraitFull!.QueueDraw();
                            return false;
                        });           
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed to load portrait AssetBundle: {e}");
                        manager.UnloadAll(true);
                        HidePortraitsIdle();
                        return;
                    }
                }
            }).Start();
        }

        private bool CreatePortraitBundle(string imageFilePath, string bundleName)
        {
            var manager = new AssetsManager();

            try
            {
                using var image = SixLabors.ImageSharp.Image.Load<Bgra32>(imageFilePath);

                if (image.Width != image.Height || image.Width <= 0)
                {
                    UIUtils.ShowErrorDialog(MainWindow.Instance, "Portrait import error", "Portrait width and height "
                        + "must be the same. In-game portraits are 1024x1024, but other sizes are also supported.");
                    return false;
                }

                bool isPowerOfTwo(int x)
                {
                    return (x & (x - 1)) == 0;
                }

                if (!isPowerOfTwo(image.Width))
                {
                    UIUtils.ShowErrorDialog(MainWindow.Instance, "Portrait import error", "Portrait size must be a "
                        + "power of two. In-game portraits are 1024x1024, but other sizes are also supported.");
                    return false;
                }

                // Flip the image first
                image.Mutate(x => x.Flip(FlipMode.Vertical));
                using var imageDataStream = new MemoryStream();
                for (int i = 0; i < image.Height; i++) 
                {
                    // Convert to bytes
                    imageDataStream.Write(MemoryMarshal.AsBytes(image.GetPixelRowSpan(i)));
                }

                var pikachuPortraitsPath = IOPath.Combine(rom.GetAssetBundlesPath(), "pikachuu.ab");
                var bundle = manager.LoadBundleFile(pikachuPortraitsPath);
                var assetsFile = manager.LoadAssetsFileFromBundle(bundle, 0);

                var asset = assetsFile.table.GetAssetsOfType((int) AssetClassID.Texture2D).First();
                var textureReplacer = new CustomTextureReplacer(manager, assetsFile.file, asset, bundleName,
                    imageDataStream.GetBuffer(), image.Width, image.Height);

                // The AssetBundle name in the metadata needs to be changed too
                var assetBundleAsset = assetsFile.table.GetAssetsOfType((int) AssetClassID.AssetBundle).First();
                var baseField = manager.GetTypeInstance(assetsFile, assetBundleAsset).GetBaseField();
                baseField["m_Name"].GetValue().Set($"{bundleName}.ab");
                baseField["m_AssetBundleName"].GetValue().Set($"{bundleName}.ab");
                // Replace the file name in the asset path for correct loading
                baseField["m_Container"].children[0]["data"]["first"].GetValue()
                    .Set($"assets/postre/graphic/ui/face_size160/{bundleName}.png");
                var assetBundleAssetBytes = baseField.WriteToByteArray();

                var assetBundleAssetReplacer = new AssetsReplacerFromMemory(0, assetBundleAsset.index,
                    (int) AssetClassID.AssetBundle, 0xffff, baseField.WriteToByteArray());
                
                // Rebuild the main asset bundle file
                var assetsFileBytes = assetsFile.file.Build(textureReplacer, assetBundleAssetReplacer);
                
                // Generate a random file name for the file inside the asset bundle since Unity
                // refuses to load the same file twice
                var randomBytes = new byte[16];
                new Random().NextBytes(randomBytes);
                var bundleReplacer = new BundleReplacerFromMemory(assetsFile.name,
                    "CAB-" + Convert.ToHexString(randomBytes).ToLower(), true, assetsFileBytes, -1);
                
                // The asset bundle normally contains a second ".resS" file which raw data, which is read based on
                // an offset in the main file. However, new portrait is written directly into the main file,
                // so we can get rid of the second file.
                bundle.file.bundleInf6.dirInf = bundle.file.bundleInf6.dirInf.Take(1).ToArray();
                bundle.file.bundleInf6.directoryCount = 1;

                var assetDir = modpack.GetDefaultMod()?.GetAssetsDirectory();
                if (assetDir == null)
                {
                    throw new Exception("Failed to retrieve assets directory.");
                }

                var targetDir = IOPath.Combine(assetDir, "ab");
                Directory.CreateDirectory(targetDir);

                using var outMemoryStream = new MemoryStream();
                using var memoryBundleWriter = new AssetsFileWriter(outMemoryStream);
                bundle.file.Write(memoryBundleWriter, new List<BundleReplacer>() { bundleReplacer });

                // Compress the bundle
                outMemoryStream.Flush();
                outMemoryStream.Position = 0;
                var targetPath = IOPath.Combine(targetDir, $"{bundleName}.ab");
                var newBundle = manager.LoadBundleFile(outMemoryStream, targetPath);
                using var bundleWriter = new AssetsFileWriter(targetPath);
                bundle.file.Pack(newBundle.file.reader, bundleWriter, AssetBundleCompressionType.LZMA);
                return true;
            }
            finally
            {
                manager.UnloadAll(true);
            }
        }

        private void HidePortraitsIdle()
        {
            GLib.Idle.Add(() => 
            {
                drawAreaPortrait!.Visible = false;
                drawAreaPortraitFull!.Visible = false;
                return false;
            });
        }

        private void OnDrawPortraitFull(object sender, DrawnArgs args)
        {
            lock (this)
            {
                if (portraitSurface == null)
                {
                    return;
                }

                DrawPortrait(args.Cr);
                drawAreaPortraitFull!.SetSizeRequest(512, 512);
            }
        }
    }
}
