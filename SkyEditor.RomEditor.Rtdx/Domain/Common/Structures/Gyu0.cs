using SkyEditor.IO.Binary;
using System;
using System.IO;
using System.Runtime.CompilerServices;
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
            var compPrevState = new CompressPreviousState();

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
                TryCompress(inputData, dataOffset, output, ref compPrevState, ref compressionResult);
                if (!compressionResult.Valid)
                {
                    var copyOffset = dataOffset;
                    var copyCommandOffset = output.Position;
                    output.Position++;
                    while (!compressionResult.Valid && copyOffset - dataOffset < 31 && copyOffset < inputData.LongLength)
                    {
                        output.WriteByte(inputData[copyOffset]);
                        copyOffset++;
                        TryCompress(inputData, copyOffset, output, ref compPrevState, ref compressionResult);
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

        private static void TryCompress(byte[] data, long offset, MemoryStream output, ref CompressPreviousState compPrevState, ref CompressionResult result)
        {
            var outputPos = output.Position;
            result.InputByteCount = 0;
            TryCompressSplitCopy(data, offset, output, ref result);
            output.Position = outputPos;

            TryCompressFill(data, offset, output, ref result);
            output.Position = outputPos;

            TryCompressSkip(data, offset, output, ref result);
            output.Position = outputPos;

            TryCompressPrevious(data, offset, output, ref compPrevState, ref result);
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

        class CompressPreviousState
        {
            private readonly long[] head = new long[0x10000];
            private readonly long[] prev = new long[0x400];
            private long currOffset = 0;
            private ushort currHash = 0;

            public CompressPreviousState()
            {
                for (int i = 0; i < head.Length; i++)
                {
                    head[i] = -1;
                }
            }

            // Search output up to a given number of bytes backwards for the longest subsequence that matches the bytes starting at data[offset].
            // The common substring must be between 2 and 33 bytes long. The longer, the better.
            public void Search(byte[] data, long offset, out int matchPos, out int matchLength)
            {
                // Catch up with the given offset
                while (currOffset < offset)
                {
                    currHash = (ushort)((currHash << 8) | data[currOffset]);
                    prev[currOffset & 0x3FF] = head[currHash];
                    head[currHash] = currOffset;
                    currOffset++;
                }

                matchPos = -1;
                matchLength = 0;

                var maxLookbehindDistance = Math.Min(0x400, (int)offset);
                if (maxLookbehindDistance < 2) return;
                var maxLength = Math.Min(33, (int)Math.Min(maxLookbehindDistance, data.Length - offset));
                if (maxLength < 2) return;
                var maxLookbehindOffset = offset - maxLookbehindDistance;

                // Check if the first two bytes at the current offset have been found somewhere within the lookbehind window
                long pos = head[Hash(data, offset)];
                if (pos < maxLookbehindOffset) return;

                // At this point we know we have matched at least two bytes
                int longestMatchLength = 2;

                // In many cases, the lookahead string starts with a repeated series of bytes for which additional optimizations can be applied
                byte repeatChar = data[offset];
                int repeatLength = 1;
                while (repeatLength < maxLength && data[offset + repeatLength] == repeatChar)
                {
                    repeatLength++;
                }

                // Expand the search for the longest match into the rest of the window.
                // The first match is not necessarily the best, but it is the closest to the cursor.
                if (repeatLength <= 2)
                {
                    // Do a regular search when the sequence of repeated bytes is too short for optimizations
                    long currPos = pos;

                    while (currPos != -1 && currPos >= maxLookbehindOffset)
                    {
                        // Expand the match length
                        int currMatchLength = 2;
                        while (currMatchLength < maxLength &&
                            currPos + currMatchLength - 1 < offset &&
                            data[offset + currMatchLength] == data[currPos + currMatchLength - 1])
                        {
                            currMatchLength++;
                        }

                        // Update match position if that's a longer match
                        if (currMatchLength > longestMatchLength)
                        {
                            longestMatchLength = currMatchLength;
                            pos = currPos;
                        }

                        // Go to the previous position in the stream
                        if (prev[currPos & 0x3FF] > currPos) break;
                        currPos = prev[currPos & 0x3FF];
                    }
                }
                else
                {
                    // Do an optimized search, taking advantage of repeated bytes at the start of the lookahead string
                    long currPos = pos;
                    int currMatchLength = 2;

                    // Move the current position backwards up to repeatLength bytes as long as the previous bytes match the repeated byte
                    for (int i = 2; i < repeatLength; i++)
                    {
                        if (currPos - 1 > maxLookbehindOffset && data[currPos - 2] == repeatChar)
                        {
                            currPos--;
                            currMatchLength++;
                        }
                    }

                    while (currPos != -1 && currPos >= maxLookbehindOffset)
                    {
                        // Expand the match length
                        while (currMatchLength < maxLength &&
                            currPos + currMatchLength - 1 < offset &&
                            data[offset + currMatchLength] == data[currPos + currMatchLength - 1])
                        {
                            currMatchLength++;
                        }

                        // Update match position if that's a longer match
                        if (currMatchLength > longestMatchLength)
                        {
                            longestMatchLength = currMatchLength;
                            pos = currPos;
                        }

                        // Go to the previous position in the stream and adjust it to the repeated byte sequence
                        if (prev[currPos & 0x3FF] > currPos) break;
                        currPos = prev[currPos & 0x3FF];
                        currMatchLength = 2;
                        for (int i = 2; i < repeatLength; i++)
                        {
                            if (currPos - 1 > maxLookbehindOffset && data[currPos - 2] == repeatChar)
                            {
                                currPos--;
                                currMatchLength++;
                            }
                        }
                    }
                }
                matchPos = (int)(pos - 1 - offset);
                matchLength = longestMatchLength;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static ushort Hash(byte[] data, long offset)
            {
                return (ushort)((data[offset] << 8) | data[offset + 1]);
            }
        }

        private static void TryCompressPrevious(byte[] data, long offset, MemoryStream output, ref CompressPreviousState state, ref CompressionResult result)
        {
            try
            {
                state.Search(data, offset, out int matchPos, out int matchLength);

                // A match length of zero means we haven't found any matches in the buffer, so bail out
                if (matchLength == 0) return;

                var compressionRatio = matchLength * 0.5f;
                if (result.IsCompressionRatioImproved(matchLength, compressionRatio))
                {
                    result.InputByteCount = matchLength;
                    result.OutputByteCount = 2;
                    output.WriteByte((byte)((byte)((matchLength - 2) << 2) | (byte)((matchPos >> 8) & 3)));
                    output.WriteByte((byte)(matchPos & 0xFF));
                }
            }
            catch (IndexOutOfRangeException)
            {
                // EOF means failure
            }
        }
    }
}
