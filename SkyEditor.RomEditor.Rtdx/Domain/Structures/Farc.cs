using Force.Crc32;
using SkyEditor.IO;
using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public class Farc
    {        
        public Farc(byte[] data)
        {
            using var binaryFile = new BinaryFile(data);
            IReadOnlyBinaryDataAccessor accessor = binaryFile;
            Magic = accessor.ReadInt32(0);
            UnknownHeaderData = accessor.ReadArray(4, 0x1C);
            FarcVersion = accessor.ReadInt32(0x20);
            if (FarcVersion != 5)
            {
                throw new NotSupportedException("Only FARC version 5 is supported");
            }

            var fatOffset = accessor.ReadInt32(0x24);
            var fatLength = accessor.ReadInt32(0x28);
            var dataOffset = accessor.ReadInt32(0x2C);
            var dataLength = accessor.ReadInt32(0x30);

            var fat = new FarcFat(data, fatOffset, fatLength);

            var files = new Dictionary<uint, byte[]>();
            foreach (var file in fat.Entries)
            {
                files.Add(file.Hash, accessor.ReadArray(dataOffset + file.DataOffset, file.DataLength));
            }
            Files = files;
        }

        private int Magic { get; }
        private byte[] UnknownHeaderData { get; }
        private int FarcVersion { get; }

        public IReadOnlyDictionary<uint, byte[]> Files { get; }

        public byte[]? GetFile(string filename)
        {
            var hash = PmdHashing.Crc32Hash(filename);
            return Files.GetValueOrDefault(hash);
        }

        private class FarcFat
        {
            public FarcFat(byte[] data, long offset, long length)
            {
                var sir0 = new Sir0(data, offset, length);
                var dataOffset = sir0.SubHeader.ReadInt32(0);
                var entryCount = sir0.SubHeader.ReadInt32(8);
                var useHashesInsteadOfFilenames = sir0.SubHeader.ReadInt32(0xC);
                if (useHashesInsteadOfFilenames != 1)
                {
                    throw new NotSupportedException("Only FARC files with hashes instead of filenames are supported");
                }

                var entries = new List<FarcFatEntry>();
                for (int i = 0; i < entryCount; i++)
                {
                    entries.Add(new FarcFatEntry(sir0.Data.ReadArray(dataOffset + (i * 16), 0x10)));
                }
                this.Entries = entries;
            }

            public IReadOnlyList<FarcFatEntry> Entries { get; }

            public class FarcFatEntry
            {
                public FarcFatEntry(byte[] data)
                {
                    Hash = BitConverter.ToUInt32(data, 0);
                    Unknown = BitConverter.ToInt32(data, 4);
                    DataOffset = BitConverter.ToInt32(data, 8);
                    DataLength = BitConverter.ToInt32(data, 0xC);
                }

                public uint Hash { get; }
                public int Unknown { get; }
                public int DataOffset { get; }
                public int DataLength { get; }
            }
        }

        public static class PmdHashing
        {
            private static Crc32Algorithm Crc32 { get; } = new Crc32Algorithm();
            private static object Crc32Lock { get; } = new object();
            public static uint Crc32Hash(string filename)
            {
                lock (Crc32Lock)
                {
                    return BitConverter.ToUInt32(Crc32.ComputeHash(Encoding.Unicode.GetBytes(filename)).Reverse().ToArray(), 0);
                }
            }
        }
    }
}
