using System;
using IOPath = System.IO.Path;
using System.Linq;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using Cairo;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditorUI.Infrastructure;
using SkyEditorUI.Infrastructure.AssetFormats;
using System.IO;

public class PortraitSheet : IDisposable
{
    public int Width { get; set; }
    public int Height { get; set; }
    public FilterMode FilterMode { get; set; }
    public ImageSurface Surface { get; set; }

    public PortraitSheet(ImageSurface surface, int width, int height, FilterMode filterMode)
    {
        Surface = surface;
        Width = width;
        Height = height;
        FilterMode = filterMode;
    }

    public static PortraitSheet LoadFromLayeredFs(string name, IRtdxRom rom, Modpack modpack)
    {
        var relativePortraitPath = IOPath.Combine("ab", $"{name}.ab");
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
        
        var bundle = manager.LoadBundleFile(assetBundlePath);
        var texture = AssetBundleHelpers.LoadFirstTextureFromBundle(manager, bundle);

        if (texture == null)
        {
            manager.UnloadAll(true);
            throw new Exception("Failed to load texture from bundle.");
        }
        var encodedData = texture.GetTextureDataRaw(bundle.file);
        if (encodedData == null)
        {
            manager.UnloadAll(true);
            throw new Exception("Failed to load texture file");
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
            throw new Exception($"Unexpected texture format: {texture.m_TextureFormat}");
        }

        var surface = new ImageSurface(decoded, Format.ARGB32, texture.m_Width, texture.m_Height,
            4 * texture.m_Width);

        manager.UnloadAll(true);
        return new PortraitSheet(surface, texture.m_Width, texture.m_Height,
            (FilterMode) texture.m_TextureSettings.m_FilterMode);
    }

    public void DrawSheet(Context cr)
    {
        cr.Save();

        // The expected size is 1024x1024, but portraits can be bigger or smaller
        double zoomFactor = 1024.0 / (double) Width;
        cr.Scale(zoomFactor, zoomFactor);

        // The image is flipped vertically
        cr.Translate(0, Height);
        cr.Scale(1, -1);

        cr.SetSourceSurface(Surface, 0, 0);
        using var pattern = cr.GetSource() as SurfacePattern;
        if (pattern != null)
        {
            pattern.Filter = FilterMode == FilterMode.Point ? Filter.Nearest : Filter.Good;
        }
        cr.Paint();
        cr.Restore();
    }

    public void DrawDefaultPortrait(Context cr, double rotation)
    {
        cr.Save();

        // The expected size is 1024x1024, but portraits can be bigger or smaller
        double zoomFactor = 1024.0 / (double) Width;
        cr.Scale(zoomFactor, zoomFactor);

        // The image is flipped vertically
        cr.Rectangle(0, 0, 160, 160);
        cr.Clip();
        cr.Translate((160 * zoomFactor) / 2, (160 * zoomFactor) / 2);
        cr.Rotate(rotation);
        cr.Translate((-160 * zoomFactor) / 2, (-160 * zoomFactor) / 2);
        cr.Translate(0, Height);
        cr.Scale(1, -1);

        cr.SetSourceSurface(Surface, 0, 0);
        cr.Operator = Operator.Over;
        using var pattern = cr.GetSource() as SurfacePattern;
        if (pattern != null)
        {
            pattern.Filter = FilterMode == FilterMode.Point ? Filter.Nearest : Filter.Good;
        }
        cr.Paint();
        cr.Restore();
    }

    public void Dispose()
    {
        Surface.Dispose();
    }
}

public enum FilterMode
{
    Point,
    Bilinear,
    Trilinear
}
