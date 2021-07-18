using Force.Crc32;
using SkyEditor.IO.Binary;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Infrastructure;
using SkyEditor.Utilities.AsyncFor;
using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class Farc
    {
        /// <summary>
        /// Creates a new FARC file from the given directory
        /// </summary>
        /// <param name="directory">Directory in which files reside.</param>
        /// <param name="fileSystem">File system in which files reside.</param>
        /// <returns>A new <see cref="Farc"/> instance containing the given files</returns>
        public static Farc FromDirectory(string directory, IFileSystem fileSystem)
        {
            if (!fileSystem.DirectoryExists(directory))
            {
                throw new DirectoryNotFoundException("Could not find the given directory in the given file system");
            }

            var farc = new Farc();
            foreach (var file in fileSystem.GetFiles(directory, "*", topDirectoryOnly: true))
            {
                farc.SetFile(Path.GetFileName(file), fileSystem.ReadAllBytes(file));
            }
            return farc;
        }

        public Farc()
        {
            Magic = 0x43524146; // FARC
            FarcVersion = 5;
            this.Files = new ConcurrentDictionary<uint, byte[]>();
        }

        public Farc(byte[] data)
        {
            using var accessor = new BinaryFile(data);
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

            var files = new ConcurrentDictionary<uint, byte[]>();
            foreach (var file in fat.Entries)
            {
                files[file.Hash] = accessor.ReadArray(dataOffset + file.DataOffset, file.DataLength);
            }
            Files = files;
        }

        private int Magic { get; }
        private byte[]? UnknownHeaderData { get; }
        private int FarcVersion { get; }

        private ConcurrentDictionary<uint, byte[]> Files { get; }

        /// <summary>
        /// Gets all hashes for files defined in the archive
        /// </summary>
        public IEnumerable<uint> GetHashes()
        {
            return Files.Keys;
        }

        /// <summary>
        /// Attempts to get file names for files defined in the archive, using the hex representation of the hash if the actual filename could not be found
        /// </summary>
        public IEnumerable<string> GetFilenames()
        {
            // Getting actual file names is not yet implemented and would only be a best-effort guess when it was
            return GetHashes().Select(h => $"0x{h:X}");
        }

        /// <summary>
        /// Gets the data for the file with the given hash, or null if no file has that hash
        /// </summary>
        /// <param name="hash">CRC32 hash of the file name</param>
        public byte[]? GetFile(uint hash)
        {
            return Files.GetValueOrDefault(hash);
        }

        /// <summary>
        /// Gets the data for the file with the given name, or null if no file has that name
        /// </summary>
        /// <param name="filename">Name of the file</param>
        public byte[]? GetFile(string filename)
        {
            var hash = GetFilenameHash(filename);
            return GetFile(hash);
        }

        /// <summary>
        /// Sets the data for the file with the given hash
        /// </summary>
        /// <param name="hash">CRC32 hash of the file name</param>
        /// <param name="data">Raw data of the file</param>
        public void SetFile(uint hash, byte[] data)
        {
            Files[hash] = data;
        }

        /// <summary>
        /// Sets the data for the file with the given file name
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <param name="data">Raw data of the file</param>
        public void SetFile(string filename, byte[] data)
        {
            var hash = GetFilenameHash(filename);
            SetFile(hash, data);
        }

        /// <summary>
        /// Removes the file with the given hash
        /// </summary>
        /// <param name="hash">CRC32 hash of the file name</param>
        public void RemoveFile(uint hash)
        {
            Files.TryRemove(hash, out _);
        }

        /// <summary>
        /// Removes the file with the given name
        /// </summary>
        /// <param name="filename">Name of the file</param>
        public void RemoveFile(string filename)
        {
            var hash = GetFilenameHash(filename);
            RemoveFile(hash);
        }

        private uint GetFilenameHash(string filename)
        {
            if (filename.StartsWith("0x"))
            {
                // Filename was made by this class and is simply a representation of the hash itself
                return Convert.ToUInt32(filename.Substring(2), fromBase: 16);
            }

            return Crc32Hasher.Crc32Hash(filename);
        }

        /// <summary>
        /// Extracts all files to the given directory
        /// </summary>
        /// <param name="outputDirectory">Directory to which the files should be saved. This will be created if it does not exist.</param>
        /// <param name="fileSystem">File system to which to save the files</param>
        public async Task Extract(string outputDirectory, IFileSystem fileSystem, ProgressReportToken? progressReportToken = null)
        {
            if (!fileSystem.DirectoryExists(outputDirectory))
            {
                fileSystem.CreateDirectory(outputDirectory);
            }

            var filenames = GetFilenames();
            await AsyncFor.ForEach(filenames, filename =>
            {
                var fileData = GetFile(filename);
                if (fileData == null)
                {
                    return;
                }
                fileSystem.WriteAllBytes(Path.Combine(outputDirectory, filename), fileData);
            }, progressReportToken: progressReportToken).ConfigureAwait(false);
        }

        public byte[] ToByteArray()
        {
            static int getPaddingLength(int length)
            {
                var padding = 16 - (length % 16);
                return padding == 16 ? 0 : padding;
            }

            // To-do: Analyze data to identify duplicate entries (i.e. make sure files with the same data are not added multiple times, instead having multiple references to the same data)
            // See here for a sample implementation: https://github.com/evandixon/SkyEditor.ROMEditor/blob/master/SkyEditor.ROMEditor/MysteryDungeon/PSMD/Farc.vb#L404

            var dataSection = new List<byte>(Files.Values.Select(v => v.Length + getPaddingLength(v.Length)).Sum());
            var fat = new FarcFat();
            foreach (var hash in GetHashes().OrderBy(h => h))
            {
                var fileData = Files[hash];
                var entry = new FarcFat.FarcFatEntry
                {
                    Hash = hash,
                    DataLength = fileData.Length,
                    DataOffset = dataSection.Count
                };
                fat.Entries.Add(entry);
                dataSection.AddRange(fileData);

                var paddingLength = getPaddingLength(dataSection.Count);
                dataSection.AddRange(Enumerable.Repeat<byte>(0, paddingLength));
            }

            var fatData = fat.ToByteArray();

            var farcHeader = new byte[0x80];
            var farcSpan = farcHeader.AsSpan();
            BinaryPrimitives.WriteInt32LittleEndian(farcSpan, Magic);
            if (UnknownHeaderData?.Length == 0x1C)
            {
                UnknownHeaderData.CopyTo(farcSpan.Slice(4));
            }
            else
            {
                BinaryPrimitives.WriteInt32LittleEndian(farcSpan.Slice(0x4), 0);
                BinaryPrimitives.WriteInt32LittleEndian(farcSpan.Slice(0x8), 0);
                BinaryPrimitives.WriteInt32LittleEndian(farcSpan.Slice(0xC), 2);
                BinaryPrimitives.WriteInt32LittleEndian(farcSpan.Slice(0x10), 0);
                BinaryPrimitives.WriteInt32LittleEndian(farcSpan.Slice(0x14), 0);
                BinaryPrimitives.WriteInt32LittleEndian(farcSpan.Slice(0x18), 7);
                BinaryPrimitives.WriteInt32LittleEndian(farcSpan.Slice(0x1C), 0x77EA3CA4);
            }

            BinaryPrimitives.WriteInt32LittleEndian(farcSpan.Slice(0x20), FarcVersion);
            farcHeader[0x24] = 0x80;// Start of FAT
            BinaryPrimitives.WriteInt32LittleEndian(farcSpan.Slice(0x28), fatData.Length);  // Length of FAT
            BinaryPrimitives.WriteInt32LittleEndian(farcSpan.Slice(0x2C), fatData.Length + 0x80); // Start of data
            BinaryPrimitives.WriteInt32LittleEndian(farcSpan.Slice(0x30), dataSection.Count); // Length of data

            var data = new List<byte>(farcHeader.Length + fatData.Length + dataSection.Count);
            data.AddRange(farcHeader);
            data.AddRange(fatData);
            data.AddRange(dataSection);
            return data.ToArray();
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
                    entries.Add(new FarcFatEntry(sir0.Data.ReadSpan(dataOffset + (i * FarcFatEntry.Length), 0x10)));
                }
                this.Entries = entries;
            }

            public FarcFat()
            {
                this.Entries = new List<FarcFatEntry>();
            }

            public List<FarcFatEntry> Entries { get; }

            public byte[] ToByteArray()
            {
                var sir0 = new Sir0Builder(8);
                var sortedEntries = Entries.OrderBy(e => e.Hash).ToList();
                Span<byte> entry = stackalloc byte[16];
                for (int i = 0; i < sortedEntries.Count; i++)
                {
                    entry.Clear();
                    sortedEntries[i].Write(entry);
                    sir0.Write(sir0.Length, entry);
                }
                sir0.SubHeaderOffset = sir0.Length;
                sir0.WritePointer(sir0.Length, 0x20); // Points to the first entry, which is always 0x20 (it makes sense when you remember this will later be translated to an address in RAM)
                sir0.WriteInt32(sir0.Length, sortedEntries.Count);
                sir0.WriteInt32(sir0.Length, 1); // Use hashes instead of actual filenames
                return sir0.ToByteArray();
            }

            public class FarcFatEntry
            {
                public const int Length = 16;

                public FarcFatEntry()
                {
                }

                public FarcFatEntry(ReadOnlySpan<byte> data)
                {
                    Hash = BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(0));
                    Unknown = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(4));
                    DataOffset = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(8));
                    DataLength = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(0xC));
                }

                public void Write(Span<byte> buffer)
                {
                    BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(0), Hash);
                    BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(4), Unknown);
                    BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(8), DataOffset);
                    BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(0xC), DataLength);
                }

                public uint Hash { get; set; }
                public int Unknown { get; set; }
                public int DataOffset { get; set; }
                public int DataLength { get; set; }
            }
        }
    }
}
