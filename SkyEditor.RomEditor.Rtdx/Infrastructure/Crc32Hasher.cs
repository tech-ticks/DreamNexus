using Force.Crc32;
using System;
using System.Buffers.Binary;
using System.Text;

namespace SkyEditor.RomEditor.Infrastructure
{ 
    /// <summary>
    /// Creates CRC32 hashes compatible with PSMD and RTDX 
    /// </summary>
    public static class Crc32Hasher
    {
        private static Crc32Algorithm Crc32 { get; } = new Crc32Algorithm();
        private static object Crc32Lock { get; } = new object();

        public static uint Crc32Hash(string filename)
        {
            lock (Crc32Lock)
            {
                return BinaryPrimitives.ReadUInt32BigEndian(Crc32.ComputeHash(Encoding.Unicode.GetBytes(filename)).AsSpan());
            }
        }
    }
}
