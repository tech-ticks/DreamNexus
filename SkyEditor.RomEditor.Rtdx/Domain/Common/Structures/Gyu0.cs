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
            // Implementation notes:
            //
            // This algorithm performs a simple forward search with a few optimizations.
            // The first and simplest one is to skip one or two bytes by checking the first two bytes with a handcrafted conditional sequence.
            // This is made possible by the fact that the command requires at least two bytes.
            //
            // There are a couple of optimizations done if the lookahead buffer starts with a sequence of repeated bytes:
            //
            // 1. Skip bytes when a non-matching byte is found after the full sequence of repeated bytes
            // 
            //   The sequence of repeated bytes can be safely skipped since it is impossible to match a longer sequence at any point within that portion of the sequence.
            //
            //   For example, suppose we have the following buffers:
            //
            //       Lookbehind buffer   AAABAAAACAAABC
            //       Lookahead buffer    AAABC
            //
            //     These are all valid matches:
            //       AAABBBAAAACAAABC   match pos
            //    *  AAAB                  0
            //    -   AA                   1
            //             AAA             6
            //           *  AAA            7
            //           -   AA            8
            //               *  AAABC     11
            //               -   AA       12
            //
            //    The matches marked with an asterisk indicate cases for which this optimization applies.
            //    The dashes beneath the asterisk indicate matches eliminated by the special cases.
            //    Note that, in all of the marked cases, the matched string contains all repeated characters from the beginning of the lookahead buffer and the next byte
            //    in the lookbehind buffer does not match the repeated character.
            //    Under those circumstances we can safely skip ahead the entire repeated block until we find a byte that matches the repeated character.
            //    Also note that we cannot skip the entire matched string since it's possible to have the initial portion of the string match in the middle of that block.
            //    For example, in the string 'AAABCAAADAAAE', the block 'AAA' appears twice. We can safely skip the initial 'AAABC', which would then match 'AAA' from 'AAAD'.
            //
            // 2. Find the furthest match for the repeating sequence
            //
            //    When encountering a sequence of repeated characters in the lookbehind buffer that matches the repeated character in the start of the lookahead buffer,
            //    we can skip several costly scans by advancing the cursor to the furthest point where the sequence fully matches the beginning of the string.
            //    At that point, resume the normal search process to try to match the following characters.
            //
            //    For example, assume we have the following buffers:
            //
            //       Lookbehind buffer   AAABAAAAAABC
            //       Lookahead buffer    AAABC
            //
            //     These are all valid matches:
            //       AAABAAAABC     match pos
            //       AAAB              0
            //        AA               1
            //         - AAA           4
            //         -  AAA          5
            //         -   AAA         6
            //         *    AAABC      7
            //               AA        8
            //
            //   The match marked with an asterisk is the only case in this example where this optimization applies.
            //   That is the furthest point in which the initial sequence of repeating characters from the lookahead buffer matches the byte sequence in the
            //   lookbehind buffer. All previous matches (marked with a dash) can be skipped since they cannot be any longer and are automatically superseded
            //   by the other matches since the algorithm prioritizes matches of the same size closest to the read cursor.

            try
            {
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

                byte repeatChar = lookaheadData[0];
                int repeatLength = 1;
                while (repeatLength < maxLength && lookaheadData[repeatLength] == repeatChar)
                {
                    repeatLength++;
                }

                if (repeatLength == 1)
                {
                    // Do a regular search when the lookahead buffer doesn't have repeating bytes at the start
                    for (int i = 0; i <= maxLookbehindDistance - 2; i++)
                    {
                        // Skip sequences that don't match at least the first two bytes
                        if (lookbehindData[i] != lookaheadData[0] || lookbehindData[i + 1] != lookaheadData[1])
                        {
                            // Optimization: Skip one extra byte if the second byte of the lookbehind buffer doesn't match the first byte in the lookahead buffer
                            if (lookbehindData[i + 1] != lookaheadData[0]) i++;
                            continue;
                        }

                        int currentMatchLength = 2;

                        // Find the longest match from that point on
                        while (currentMatchLength < maxLength &&
                            currentMatchLength + i < maxLookbehindDistance &&
                            lookbehindData[i + currentMatchLength] == lookaheadData[currentMatchLength])
                        {
                            currentMatchLength++;
                        }

                        // Update match.
                        // A match of the same length as the previous match is always superior since we're scanning forward and
                        // want the longest match that is closest to the cursor.
                        if (currentMatchLength >= matchLength)
                        {
                            matchLength = currentMatchLength;
                            matchPos = i;
                        }
                    }
                }
                else
                {
                    // Do an optimized search when the lookahead buffer contains repeating bytes at the start
                    for (int i = 0; i <= maxLookbehindDistance - 2; i++)
                    {
                        // Skip sequences that don't match at least the first two bytes.
                        // Note that the first two bytes in the lookahead buffer are guaranteed to be the same here.
                        if (lookbehindData[i] != repeatChar || lookbehindData[i + 1] != repeatChar)
                        {
                            if (lookbehindData[i + 1] != repeatChar) i++;
                            continue;
                        }

                        int currentMatchLength = 2;

                        // Find the match for the repeating byte sequence first.
                        // Expand the match to its length, then move it as far forward as possible.
                        while (currentMatchLength + i < maxLookbehindDistance &&
                            lookbehindData[i + currentMatchLength] == repeatChar)
                        {
                            if (currentMatchLength < repeatLength)
                            {
                                currentMatchLength++;
                            }
                            else
                            {
                                i++;
                            }
                        }

                        // Find the longest match from that point on, but only if we already matched the sequence of repeating bytes
                        if (currentMatchLength == repeatLength)
                        {
                            while (currentMatchLength < maxLength &&
                            currentMatchLength + i < maxLookbehindDistance &&
                            lookbehindData[i + currentMatchLength] == lookaheadData[currentMatchLength])
                            {
                                currentMatchLength++;
                            }
                        }

                        // Update match.
                        // A match of the same length as the previous match is always superior since we're scanning forward and
                        // want the longest match that is closest to the cursor.
                        if (currentMatchLength >= matchLength)
                        {
                            matchLength = currentMatchLength;
                            matchPos = i;
                        }

                        // Skip additional bytes when a non-matching byte is found after the full sequence of repeated bytes
                        if (currentMatchLength >= repeatLength &&
                            i + currentMatchLength < maxLookbehindDistance &&
                            lookbehindData[i + currentMatchLength] != repeatChar)
                        {
                            i += repeatLength;
                        }
                    }
                }

                // A match length of zero means we haven't found any matches in the buffer, so bail out
                if (matchLength == 0) return;

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
