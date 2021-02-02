using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Domain.Common.Structures
{
    public class Sir0Builder : IWriteOnlyBinaryDataAccessor
    {
        private const string Magic = "SIR0\0\0\0\0";

        public Sir0Builder(int pointerSize, int bufferedLength = 1000)
        {
            this.pointerSize = pointerSize;

            DataFile = new BinaryFile(new byte[bufferedLength]);
            if (pointerSize == 4)
            {
                this.Write(new byte[0x10]); // Placeholder for magic and header pointers
            }
            else if (pointerSize == 8)
            {
                this.Write(new byte[0x20]); // Placeholder for magic and header pointers
            }
            else
            {
                throw new ArgumentException("Pointer size must be 4 or 8");
            }

            PointerOffsets = new List<long>();
        }

        public Sir0Builder(int pointerSize) : this(pointerSize, 1000)
        {
        }

        private readonly int pointerSize;

        private BinaryFile DataFile { get; }
        private IBinaryDataAccessor Data => DataFile;
        private List<long> PointerOffsets { get; }
        public int Length { get; private set; }
        public int SubHeaderOffset { get; set; }

        public void SetLength(int length)
        {
            if (length > Data.Length)
            {
                DataFile.SetLength((length + Data.Length) * 2);
            }
            this.Length = length;
        }

        public void Align(int length)
        {
            var paddingLength = length - (Length % length);
            if (paddingLength != length)
            {
                WritePadding(Length, paddingLength);
            }
        }

        private void EnsureLengthIsLargeEnough(int length)
        {
            if (length > this.Length)
            {
                SetLength(length);
            }
        }

        /// <summary>
        /// Registers the data at the given index as a pointer
        /// </summary>
        /// <param name="index">Index of the pointer</param>
        public void MarkPointer(long index)
        {
            PointerOffsets.Add(index);
        }

        /// <summary>
        /// Writes the given data and registers it as a pointer
        /// </summary>
        /// <param name="index">Target location to which the pointer should be written, which will be registered as storing a pointer</param>
        /// <param name="pointer">The pointer to write</param>
        public void WritePointer(long index, long pointer)
        {
            EnsureLengthIsLargeEnough((int)index + pointerSize);
            if (pointerSize == 8)
            {
                Data.WriteInt64(index, pointer);
            }
            else if (pointerSize == 4)
            {
                Data.WriteInt32(index, (int)pointer);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported pointer size: {pointerSize}");
            }
            MarkPointer(index);
        }

        /// <summary>
        /// Writes the given data and registers it as a pointer
        /// </summary>
        /// <param name="index">Target location to which the pointer should be written, which will be registered as storing a pointer</param>
        /// <param name="pointer">The pointer to write</param>
        public async Task WritePointerAsync(long index, long pointer)
        {
            EnsureLengthIsLargeEnough((int)index + pointerSize);
            if (pointerSize == 8)
            {
                await Data.WriteInt64Async(index, pointer).ConfigureAwait(false);
            }
            else if (pointerSize == 4)
            {
                await Data.WriteInt32Async(index, (int)pointer).ConfigureAwait(false);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported pointer size: {pointerSize}");
            }
            MarkPointer(index);
        }

        public void WritePadding(long index, int paddingLength, byte paddingCharacter = 0)
        {
            if (paddingLength == 0)
            {
                return;
            }

            if (paddingLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paddingLength), "Padding length must not be negative");
            }

            EnsureLengthIsLargeEnough((int)index + paddingLength);
            Span<byte> padding = stackalloc byte[paddingLength];
            if (paddingCharacter != 0)
            {
                for (int i = 0; i < paddingLength; i++)
                {
                    padding[i] = paddingCharacter;
                }
            }
            this.Write(index, paddingLength, padding);
        }

        public async Task WritePaddingAsync(long index, int paddingLength, byte paddingCharacter = 0)
        {
            if (paddingLength == 0)
            {
                return;
            }

            if (paddingLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paddingLength), "Padding length must not be negative");
            }

            EnsureLengthIsLargeEnough((int)index + paddingLength);
            byte[] padding = new byte[paddingLength];
            if (paddingCharacter != 0)
            {
                for (int i = 0; i < paddingLength; i++)
                {
                    padding[i] = paddingCharacter;
                }
            }
            await this.WriteAsync(index, paddingLength, padding);
        }

        public byte[] ToByteArray(bool alignFooter = true)
        {
            this.WriteString(0, Encoding.ASCII, Magic);
            this.WritePointer(8, SubHeaderOffset);
            var footerOffset = this.Length;
            if (alignFooter)
            {
                footerOffset += 0x10 - (this.Length % 0x10);
            }
            this.WritePointer(16, footerOffset);
            WriteFooter(footerOffset, alignFooter);

            var newData = new byte[this.Length];
            Array.Copy(Data.ReadArray(), newData, this.Length);
            return newData;
        }

        public Sir0 Build(bool alignFooter = true)
        {            
            return new Sir0(ToByteArray(alignFooter));
        }

        private void WriteFooter(int footerOffset, bool alignFooter)
        {
            long lastPointer = 0;
            PointerOffsets.Sort();
            foreach (var pointer in PointerOffsets)
            {
                var pointerRelativeIndex = pointer - lastPointer;
                lastPointer = pointer;
                if (pointerRelativeIndex < 128)
                {
                    this.Write(footerOffset++, (byte)pointerRelativeIndex);
                }
                else
                {
                    var workingBytes = new List<byte>();
                    var workingItem = pointerRelativeIndex;

                    workingBytes.Add((byte)(workingItem & 0x7F));
                    workingItem >>= 7;

                    while (workingItem > 0)
                    {
                        workingBytes.Add((byte)((workingItem & 0x7F) | 0x80));
                        workingItem >>= 7;
                    }

                    for (var i = workingBytes.Count - 1; i >= 0; i--)
                    {
                        this.Write(footerOffset++, workingBytes[i]);
                    }
                }
            }
            this.Write(footerOffset++, 0); // Marks the end of the pointers

            // Align to 16 bytes
            var paddingLength = 0x10 - (this.Length % 0x10);
            if (paddingLength != 0x10)
            {
                this.WritePadding(footerOffset, paddingLength);
            }
        }

        #region IWriteOnlyBinaryDataAccessor Implementation
        public void Write(byte[] value)
        {
            EnsureLengthIsLargeEnough(value.Length);
            Data.Write(value);
        }

        public void Write(long offset, byte[] value)
        {
            EnsureLengthIsLargeEnough((int)offset + value.Length);
            Data.Write(offset, value);
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            EnsureLengthIsLargeEnough(value.Length);
            Data.Write(value);
        }

        public void Write(long index, byte value)
        {
            EnsureLengthIsLargeEnough((int)index + 1);
            Data.Write(index, value);
        }

        public void Write(long index, int length, byte[] value)
        {
            EnsureLengthIsLargeEnough((int)index + length);
            Data.Write(index, length, value);
        }

        public void Write(long index, int length, ReadOnlySpan<byte> value)
        {
            EnsureLengthIsLargeEnough((int)index + length);
            Data.Write(index, length, value);
        }

        public async Task WriteAsync(byte[] value)
        {
            EnsureLengthIsLargeEnough(value.Length);
            await Data.WriteAsync(value).ConfigureAwait(false);
        }

        public async Task WriteAsync(ReadOnlyMemory<byte> value)
        {
            EnsureLengthIsLargeEnough(value.Length);
            await Data.WriteAsync(value).ConfigureAwait(false);
        }

        public async Task WriteAsync(long index, byte value)
        {
            EnsureLengthIsLargeEnough((int)index + 1);
            await Data.WriteAsync(index, value).ConfigureAwait(false);
        }

        public async Task WriteAsync(long index, int length, byte[] value)
        {
            EnsureLengthIsLargeEnough((int)index + length);
            await Data.WriteAsync(index, length, value).ConfigureAwait(false);
        }

        public async Task WriteAsync(long index, int length, ReadOnlyMemory<byte> value)
        {
            EnsureLengthIsLargeEnough((int)index + length);
            await Data.WriteAsync(index, length, value).ConfigureAwait(false);
        }

        public void WriteString(long index, Encoding e, string value)
        {
            var bytes = e.GetBytes(value);
            this.Write(index, bytes);
        }

        public void WriteInt32(long offset, int value)
        {
            EnsureLengthIsLargeEnough((int)offset + 4);
            Data.WriteInt32(offset, value);
        }

        public void WriteInt64(long offset, long value)
        {
            EnsureLengthIsLargeEnough((int)offset + 8);
            Data.WriteInt64(offset, value);
        }
        #endregion
    }
}
