using SkyEditor.IO.Binary;
using System;
using System.IO;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Common.Structures
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
                    case Opcode.Copy: // 80..9F
                        {
                            var bytes = data.ReadSpan(dataIndex, arg + 1);
                            dataIndex += bytes.Length;
                            dest.Write(destIndex, bytes);
                            destIndex += bytes.Length;
                            break;
                        }

                    case Opcode.SplitCopy: // A0..BF
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

                    case Opcode.Fill: // C0..DF
                        {
                            var fill = data.ReadByte(dataIndex++);
                            for (int i = 0; i < arg + 2; i++)
                                dest.Write(destIndex++, fill);
                            break;
                        }

                    case Opcode.Skip: // E0..FF
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
            var output = new MemoryStream((int)input.Length / 2);
            var inputData = input.ReadArray();

            void writeArray(byte[] array)
            {
                output.Write(array, 0, array.Length);
            };

            writeArray(Encoding.ASCII.GetBytes("GYU0"));
            writeArray(BitConverter.GetBytes(inputData.Length));

            long dataOffset = 0;
            var compressionResult = new CompressionResult();
            while (dataOffset < inputData.LongLength)
            {
                // Try each of the compression algorithms without copying data first.
                // If we get a result, write that to the output right away.
                // Otherwise, try copying the least amount of data followed by one of the algorithms.
                TryCompress(inputData, dataOffset, output, ref compressionResult);
                if (!compressionResult.Valid)
                {
                    var copyOffset = dataOffset;
                    var copyCommandOffset = output.Position;
                    output.Position++;
                    while (!compressionResult.Valid && copyOffset - dataOffset < 31 && copyOffset < inputData.LongLength)
                    {
                        output.WriteByte(inputData[copyOffset]);
                        copyOffset++;
                        TryCompress(inputData, copyOffset, output, ref compressionResult);
                    }
                    var currPos = output.Position;
                    output.Position = copyCommandOffset;
                    output.WriteByte((byte)(0x80 + copyOffset - dataOffset - 1));
                    output.Position = currPos;
                    dataOffset = copyOffset;
                }
                if (compressionResult.Valid)
                {
                    dataOffset += compressionResult.InputByteCount;
                }
            }

            // Write EOF marker
            output.WriteByte(0x7F);
            output.WriteByte(0xFF);
            // Trim any excess bytes that may have been written by the TryCompress* methods
            output.SetLength(output.Position);
            return new BinaryFile(output.ToArray());
        }

        private class CompressionResult
        {
            public long InputByteCount { get; set; }
            public long OutputByteCount { get; set; }
            public float CompressionRatio => Valid ? ((float)InputByteCount / OutputByteCount) : 0;
            public bool Valid => (InputByteCount != 0);

            public bool IsCompressionRatioImproved(int newInputByteCount, float newCompressionRatio)
            {
                if (newInputByteCount > InputByteCount) return true;
                if (newInputByteCount < InputByteCount) return false;
                return newCompressionRatio > CompressionRatio;
            }
        }

        private static void TryCompress(byte[] data, long offset, MemoryStream output, ref CompressionResult result)
        {
            var outputPos = output.Position;
            result.InputByteCount = 0;
            TryCompressSplitCopy(data, offset, output, ref result);
            output.Position = outputPos;

            TryCompressFill(data, offset, output, ref result);
            output.Position = outputPos;

            TryCompressSkip(data, offset, output, ref result);
            output.Position = outputPos;

            TryCompressPrevious(data, offset, output, ref result);
            output.Position = outputPos;

            if (result.Valid) output.Position += result.OutputByteCount;
        }

        private static void TryCompressSplitCopy(byte[] data, long offset, MemoryStream output, ref CompressionResult result)
        {
            try
            {
                var sep = data[offset];
                var count = 1;
                while (data[offset + count * 2] == sep && data[offset + count * 2 + 1] != sep && count < 0x21)
                {
                    count++;
                }

                if (count >= 2)
                {
                    var compressionRatio = count * 2.0f / (2.0f + count);
                    if (result.IsCompressionRatioImproved(count, compressionRatio))
                    {
                        result.InputByteCount = count * 2;
                        result.OutputByteCount = 2 + count;
                        output.WriteByte((byte)(0xA0 + count - 2));
                        output.WriteByte(sep);
                        for (int i = 0; i < count; i++)
                        {
                            output.WriteByte(data[offset + i * 2 + 1]);
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // EOF means failure
            }
        }

        private static void TryCompressFill(byte[] data, long offset, MemoryStream output, ref CompressionResult result)
        {
            try
            {
                var fill = data[offset];
                var count = 1;
                while (data[offset + count] == fill && count < 0x21)
                {
                    count++;
                }

                if (count >= 2)
                {
                    var compressionRatio = count * 0.5f;
                    if (result.IsCompressionRatioImproved(count, compressionRatio))
                    {
                        result.InputByteCount = count;
                        result.OutputByteCount = 2;
                        output.WriteByte((byte)(0xC0 + count - 2));
                        output.WriteByte(fill);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // EOF means failure
            }
        }

        private static void TryCompressSkip(byte[] data, long offset, MemoryStream output, ref CompressionResult result)
        {
            try
            {
                var count = 0;
                while (data[offset + count] == 0 && count < 0x11F)
                {
                    count++;
                }
                if (count > 0)
                {
                    if (count < 0x1F)
                    {
                        var compressionRatio = count;
                        if (result.IsCompressionRatioImproved(count, compressionRatio))
                        {
                            result.InputByteCount = count;
                            result.OutputByteCount = 1;
                            output.WriteByte((byte)(0xE0 + count - 1));
                        }
                    }
                    else
                    {
                        var compressionRatio = count * 0.5f;
                        if (result.IsCompressionRatioImproved(count, compressionRatio))
                        {
                            result.InputByteCount = count;
                            result.OutputByteCount = 2;
                            output.WriteByte(0xFF);
                            output.WriteByte((byte)(count - 0x20));
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // EOF means failure
            }
        }

        private static void TryCompressPrevious(byte[] data, long offset, MemoryStream output, ref CompressionResult result)
        {
            // Don't waste time trying to look behind if there's nothing written yet
            if (offset == 0) return;

            try
            {
                // TODO: figure out if it's possible to reuse state from previous iterations

                // Search output up to a given number of bytes backwards for the longest subsequence that matches the bytes starting at data[offset].
                // A smaller maxLookbehindDistance increases compressed size but decreases time
                // The common substring must be between 2 and 33 bytes long. The longer, the better.
                var maxLookbehindDistance = Math.Min(0x400, (int)offset);
                if (maxLookbehindDistance < 2) return;
                var maxLength = Math.Min(33, (int)Math.Min(maxLookbehindDistance, data.Length - offset));
                if (maxLength < 2) return;

                var lookbehindData = new Span<byte>(data, (int)(offset - maxLookbehindDistance), maxLookbehindDistance);
                var lookaheadData = new Span<byte>(data, (int)offset, maxLength);

                int matchLength = 0;
                int matchPos = -1; // relative to the start of the lookbehind span

                // Look for the first matching sequence of 2 bytes backwards into the lookbehind buffer
                for (int i = 0; i <= maxLookbehindDistance - 2; i++)
                {
                    int bufOffset = maxLookbehindDistance - 2 - i;
                    if (lookbehindData[bufOffset] == lookaheadData[0] &&
                        lookbehindData[bufOffset + 1] == lookaheadData[1])
                    {
                        matchLength = 2;
                        matchPos = bufOffset;
                        break;
                    }
                }

                // A match length of zero means we haven't found any matches in the buffer, so bail out
                if (matchLength == 0) return;

                // Search for longer matches from there
                while (matchLength < maxLength)
                {
                    if (matchLength + matchPos < maxLookbehindDistance && lookbehindData[matchPos + matchLength] == lookaheadData[matchLength])
                    {
                        // Expand match length at the current position
                        matchLength++;
                    }
                    else
                    {
                        // Search backwards for another match of the same length
                        // Stop the search if no more matches are available
                        int tentativeMatchPos = matchPos - 1;
                        int tentativeMatchLength = matchLength + 1;
                        var lookaheadSlice = lookaheadData.Slice(0, tentativeMatchLength);
                        while (tentativeMatchPos >= 0)
                        {
                            if (lookbehindData.Slice(tentativeMatchPos, tentativeMatchLength).SequenceEqual(lookaheadSlice))
                            {
                                matchPos = tentativeMatchPos;
                                matchLength = tentativeMatchLength;
                                break;
                            }
                            tentativeMatchPos--;
                        }
                        if (tentativeMatchPos < 0) break;
                    }
                }

                var compressionRatio = matchLength * 0.5f;
                if (result.IsCompressionRatioImproved(matchLength, compressionRatio))
                {
                    var matchOffset = matchPos - maxLookbehindDistance;

                    result.InputByteCount = matchLength;
                    result.OutputByteCount = 2;
                    output.WriteByte((byte)((byte)((matchLength - 2) << 2) | (byte)((matchOffset >> 8) & 3)));
                    output.WriteByte((byte)(matchOffset & 0xFF));
                }
            }
            catch (IndexOutOfRangeException)
            {
                // EOF means failure
            }
        }
    }
}
