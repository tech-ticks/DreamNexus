using AssetStudio;
using Newtonsoft.Json.Schema;
using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public static class Gyu0
    {
        private enum Opcode
        {
            Copy,
            SplitCopy,
            Fill,
            Skip
        }

        public static IBinaryDataAccessor Decompress(IReadOnlyBinaryDataAccessor data)
        {
            var Magic = data.ReadInt32(0);
            var Length = data.ReadInt32(4);

            IBinaryDataAccessor dest = new BinaryFile(new byte[Length]);

            var dataIndex = 8;
            var destIndex = 0;

            while (true)
            {
                byte cur = data.ReadByte(dataIndex++);

                if (cur < 0x80)
                {
                    var next = data.ReadByte(dataIndex++);

                    // EOF marker
                    if (cur == 0x7F && next == 0xFF)
                        return dest;

                    var offset = 0x400 - ((cur & 3) * 0x100 + next);
                    var bytes = dest.ReadSpan(destIndex - offset, (cur >> 2) + 2);
                    dest.Write(destIndex, bytes);
                    destIndex += bytes.Length;
                    continue;
                }

                var op = (Opcode)((cur >> 5) - 4);
                var arg = cur & 0x1F;

                switch (op)
                {
                    case Opcode.Copy:
                        {
                            var bytes = data.ReadSpan(dataIndex, arg + 1);
                            dataIndex += bytes.Length;
                            dest.Write(destIndex, bytes);
                            destIndex += bytes.Length;
                            break;
                        }

                    case Opcode.SplitCopy:
                        {
                            var sep = data.ReadByte(dataIndex++);
                            var bytes = data.ReadSpan(dataIndex, arg + 2);
                            dataIndex += bytes.Length;
                            foreach (byte x in bytes)
                            {
                                dest.Write(destIndex++, sep);
                                dest.Write(destIndex++, x);
                            }
                            break;
                        }

                    case Opcode.Fill:
                        {
                            var fill = data.ReadByte(dataIndex++);
                            for (int i = 0; i < arg + 2; i++)
                                dest.Write(destIndex++, fill);
                            break;
                        }

                    case Opcode.Skip:
                        {
                            var count = arg < 0x1F ? arg + 1 : 0x20 + data.ReadByte(dataIndex++);
                            destIndex += count;
                            break;
                        }
                }
            }
        }

        public static IBinaryDataAccessor Compress(IReadOnlyBinaryDataAccessor input)
        {
            var output = new BinaryFile(new byte[input.Length / 2]);

            var outPos = 0;
            void writeOut(byte[] data, bool incPos)
            {
                if (data.Length + outPos >= output.Length)
                {
                    output.SetLength((data.Length + outPos) * 2);
                }
                output.Write(outPos, data);
                if (incPos) outPos += data.Length;
            }

            writeOut(Encoding.ASCII.GetBytes("GYU0"), true);
            writeOut(BitConverter.GetBytes((int)input.Length), true);

            long dataOffset = 0;
            while (dataOffset < input.Length)
            {
                // Try each of the compression algorithms without copying data first.
                // If we get a result, write that to the output right away.
                // Otherwise, try copying the least amount of data followed by one of the algorithms.
                var nextOffset = dataOffset;
                var shortest = TryCompress(input, dataOffset);
                var length = 1;
                var copy = new byte[0];
                while (shortest == null && length < 0x20 && dataOffset + length <= input.Length)
                {
                    copy = CompressCopy(input, dataOffset, length);
                    writeOut(copy, false);
                    nextOffset = dataOffset + length;
                    shortest = TryCompress(input, nextOffset);
                    length++;
                }
                dataOffset = nextOffset;
                outPos += copy.Length;
                if (shortest != null)
                {
                    writeOut(shortest.Output, true);
                    dataOffset += shortest.InputByteCount;
                }
            }

            // Write EOF marker
            writeOut(new byte[] { 0x7F, 0xFF }, true);
            output.SetLength(outPos);
            return output;
        }

        private class CompressionResult
        {
            /// <summary>
            /// Constructs a compression result indicating failure to compress data.
            /// </summary>
            public CompressionResult()
            {
                InputByteCount = 0;
                Output = new byte[0];
            }

            /// <summary>
            /// Constructs a compression result indicating success.
            /// </summary>
            /// <param name="inputByteCount">The number of bytes compressed</param>
            /// <param name="output">The compressed output</param>
            public CompressionResult(long inputByteCount, byte[] output)
            {
                InputByteCount = inputByteCount;
                Output = output;
            }

            public readonly long InputByteCount;
            public readonly byte[] Output;
            public float CompressionRatio => Valid ? (InputByteCount / Output.Length) : 0;
            public bool Valid => (InputByteCount != 0);
        }

        private static byte[] CompressCopy(IReadOnlyBinaryDataAccessor data, long offset, int count)
        {
            if (count < 1 || count > 0x20) throw new ArgumentOutOfRangeException(nameof(count));
            return new byte[] { (byte)(0x80 + count - 1) }.Concat(data.ReadArray(offset, count)).ToArray();
        }

        private static CompressionResult TryCompress(IReadOnlyBinaryDataAccessor data, long offset)
        {
            // Choose the option with the best compression ratio, or failure if none worked
            CompressionResult[] options = {
                TryCompressSplitCopy(data, offset),
                TryCompressFill(data, offset),
                TryCompressSkip(data, offset),
                TryCompressPrevious(data, offset)
            };
            return options.Where(x => x.Valid).OrderByDescending(x => x?.CompressionRatio).FirstOrDefault();
        }

        private static CompressionResult TryCompressSplitCopy(IReadOnlyBinaryDataAccessor data, long offset)
        {
            try
            {
                var sep = data.ReadByte(offset);
                var count = 1;
                var bytes = new List<byte> { data.ReadByte(offset + 1) };
                while (data.ReadByte(offset + count * 2) == sep && data.ReadByte(offset + count * 2 + 1) != sep && count < 0x21)
                {
                    bytes.Add(data.ReadByte(offset + count * 2 + 1));
                    count++;
                }
                if (count < 2) return new CompressionResult();
                return new CompressionResult(count * 2, new byte[] { (byte)(0xA0 + count - 2), sep }.Concat(bytes).ToArray());
            }
            catch (IndexOutOfRangeException)
            {
                // EOF means failure
                return new CompressionResult();
            }
        }

        private static CompressionResult TryCompressFill(IReadOnlyBinaryDataAccessor data, long offset)
        {
            try
            {
                var fill = data.ReadByte(offset);
                var count = 1;
                while (data.ReadByte(offset + count) == fill && count < 0x21)
                {
                    count++;
                }
                if (count < 2) return new CompressionResult();
                return new CompressionResult(count, new byte[] { (byte)(0xC0 + count - 2), fill });
            }
            catch (IndexOutOfRangeException)
            {
                // EOF means failure
                return new CompressionResult();
            }
        }

        private static CompressionResult TryCompressSkip(IReadOnlyBinaryDataAccessor data, long offset)
        {
            try
            {
                var count = 0;
                while (data.ReadByte(offset + count) == 0 && count < 0x11F)
                {
                    count++;
                }
                if (count == 0) return new CompressionResult();
                if (count < 0x1F) return new CompressionResult(count, new byte[] { (byte)(0xE0 + count - 1) });
                return new CompressionResult(count, new byte[] { 0xFF, (byte)(count - 0x20) });
            }
            catch (IndexOutOfRangeException)
            {
                // EOF means failure
                return new CompressionResult();
            }
        }

        private static CompressionResult TryCompressPrevious(IReadOnlyBinaryDataAccessor data, long offset)
        {
            // Don't waste time trying to look behind if there's nothing written yet
            if (offset == 0) return new CompressionResult();

            try
            {
                // Search output up to 0x400 bytes behind for the longest subsequence of bytes found in data starting at offset.
                // The common substring must be between 2 and 33 bytes long.
                var maxLookbehindDistance = Math.Min(0x400, (int)offset);
                if (maxLookbehindDistance < 2) return new CompressionResult();
                var maxLength = Math.Min(33, (int)Math.Min(maxLookbehindDistance, data.Length - offset));

                var lookbehindData = data.Slice(offset - maxLookbehindDistance, maxLookbehindDistance);
                var lookaheadData = data.Slice(offset, maxLength);

                int matchLength = 0;
                int matchPos = -1;

                int[,] longestCommonSuffixes = new int[lookbehindData.Length + 1, lookaheadData.Length + 1];
                for (int i = 0; i <= lookbehindData.Length; i++)
                {
                    for (int j = 0; j <= lookaheadData.Length; j++)
                    {
                        if (i == 0 || j == 0)
                        {
                            longestCommonSuffixes[i, j] = 0;
                        }
                        else if (lookbehindData.ReadByte(i - 1) == lookaheadData.ReadByte(j - 1))
                        {
                            longestCommonSuffixes[i, j] = longestCommonSuffixes[i - 1, j - 1] + 1;
                            if (longestCommonSuffixes[i, j] > matchLength && longestCommonSuffixes[i, j] == j)
                            {
                                matchLength = longestCommonSuffixes[i, j];
                                matchPos = i - matchLength;
                            }
                        }
                        else
                        {
                            longestCommonSuffixes[i, j] = 0;
                        }
                    }
                }

                if (matchLength < 2) return new CompressionResult();
                var matchOffset = matchPos - offset;

                return new CompressionResult(matchLength, new byte[] {
                    (byte)((byte)((matchLength - 2) << 2) | (byte)((matchOffset >> 8) & 3)),
                    (byte)(matchOffset & 0xFF)
                });
            }
            catch (IndexOutOfRangeException)
            {
                // EOF means failure
                return new CompressionResult();
            }
        }
    }
}
