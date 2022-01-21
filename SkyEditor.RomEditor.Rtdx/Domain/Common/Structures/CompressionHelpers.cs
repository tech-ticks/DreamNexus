using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using SkyEditor.IO.Binary;

namespace SkyEditor.RomEditor.Domain.Common.Structures
{
    public enum CompressionType
    {
        Gyu0,
        Deflate
    }

    public static class CompressionHelpers
    {
        public static IBinaryDataAccessor Compress(IReadOnlyBinaryDataAccessor data, CompressionType type)
        {
            switch (type)
            {
                case CompressionType.Gyu0:
                    return Gyu0.Compress(data);
                case CompressionType.Deflate:
                    return CompressDeflate(data);
                default:
                    throw new ArgumentException("Invalid compression type", nameof(type));
            }
        }

        public static IBinaryDataAccessor CompressDeflate(IReadOnlyBinaryDataAccessor data)
        {
            using (var outStream = new MemoryStream())
            {
                using (var deflateStream = new DeflateStream(outStream, CompressionMode.Compress))
                {
                    deflateStream.Write(data.ReadArray(), 0, (int) data.Length);
                }

                var compressed = outStream.ToArray();

                var file = new BinaryFile(new MemoryStream((int) data.Length + 12));
                file.WriteString(0, Encoding.ASCII, "DEFL");
                file.WriteUInt32(4, (uint) data.Length); // Compressed size
                file.WriteUInt32(8, (uint) compressed.Length); // Uncompressed size
                file.Write(12, (int) compressed.Length, compressed);
                return file;
            }
        }
    }
}
