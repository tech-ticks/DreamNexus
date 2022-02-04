using System;
using System.IO;
using System.Linq;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace SkyEditorUI.Infrastructure
{
    public static class AssetBundleHelpers
    {
        public static TextureFile? LoadFirstTextureFromBundle(AssetsManager manager, BundleFileInstance bundle)
        {
            var assetFile = manager.LoadAssetsFileFromBundle(bundle, 0);
            var textureAsset = assetFile.table
                .GetAssetsOfType((int) AssetClassID.Texture2D).FirstOrDefault();

            if (textureAsset == null)
            {
                return null;
            }

            var baseField = manager.GetTypeInstance(assetFile, textureAsset).GetBaseField();
            var texture = TextureFile.ReadTextureFile(baseField);
            return texture;
        }

        public static byte[] Build(this AssetsFile file, params AssetsReplacer[] replacers)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new AssetsFileWriter(stream))
                {
                    file.Write(writer, 0, replacers.ToList(), 0);
                    return stream.ToArray();
                }
            }
        }

        public static byte[]? GetTextureDataRaw(this TextureFile texture, AssetBundleFile bundle)
        {
            // From https://github.com/nesrak1/AssetsTools.NET/blob/master/AssetTools.NET/Standard/TextureFileFormat/TextureFile.cs#L290
            if ((texture.pictureData == null || texture.pictureData.Length == 0) && texture.m_StreamData.path != null
                && texture.m_StreamData.path.StartsWith("archive:/") && bundle != null)
            {
                string resourceFileName = texture.m_StreamData.path.Split('/').Last();
                int resourceFileIndex = bundle.GetFileIndex(resourceFileName);
                if (resourceFileIndex >= 0)
                {
                    bundle.GetFileRange(resourceFileIndex, out long resourceFileOffset, out _);
                    texture.pictureData = new byte[texture.m_StreamData.size];
                    bundle.reader.Position = resourceFileOffset + (long) texture.m_StreamData.offset;
                    bundle.reader.Read(texture.pictureData, 0, texture.pictureData.Length);
                }
            }

            return texture.pictureData;
        }

        public static void GetFileRange(this AssetBundleFile bundle, int index, out long offset, out long length)
        {
            // From https://github.com/nesrak1/AssetsTools.NET/blob/master/AssetTools.NET/Standard/AssetsBundleFileFormat/AssetsBundleFile.cs#L636
            // Copied because the method is internal

            if (bundle.bundleHeader3 != null)
            {
                AssetsBundleEntry entry = bundle.assetsLists3.entries[index];
                offset = bundle.bundleHeader3.bundleDataOffs + entry.offset;
                length = entry.length;
            }
            else if (bundle.bundleHeader6 != null)
            {
                AssetBundleDirectoryInfo06 entry = bundle.bundleInf6.dirInf[index];
                offset = bundle.bundleHeader6.GetFileDataOffset() + entry.offset;
                length = entry.decompressedSize;
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }

    public class CustomTextureReplacer : Texture2DAssetReplacer
    {
        private byte[] bgra;
        private int width;
        private int height;
        private string name;

        public CustomTextureReplacer(AssetsManager manager, AssetsFile assetsFile, AssetFileInfoEx asset,
            string name, byte[] bgra, int width, int height) : base(manager, assetsFile, asset)
        {
            this.bgra = bgra;
            this.width = width;
            this.height = height;
            this.name = name;
        }

        protected override void GetNewTextureData(out byte[] bgra, out int width, out int height)
        {
            bgra = this.bgra;
            width = this.width;
            height = this.height;
        }

        protected override void Modify(AssetTypeValueField baseField)
        {
            var texture = TextureFile.ReadTextureFile(baseField);
            texture.m_Name = name;
            texture.m_TextureFormat = (int) TextureFormat.RGBA32;
            GetNewTextureData(out byte[] bgra, out int width, out int height);
            SetTextureData(texture, bgra, width, height);

            texture.WriteTo(baseField);
        }
    }
}
