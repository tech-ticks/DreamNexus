using SkyEditor.IO;
using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public class Sir0Builder : IWriteOnlyBinaryDataAccessor
    {
        private const string Magic = "SIR0\0\0\0\0";

        public Sir0Builder(int bufferedLength = 1000)
        {
            Data = new BinaryFile(new byte[bufferedLength]);
            this.Write(new byte[0x20]); // Placeholder for magic and header pointers
            PointerOffsets = new List<long>();
        }

        public Sir0Builder() : this(1000)
        {
        }

        private BinaryFile Data { get; }
        private List<long> PointerOffsets { get; }
        public int Length { get; private set; }
        public int SubHeaderOffset { get; set; }

        public void SetLength(int length)
        {
            if (length > Data.Length)
            {
                Data.SetLength(Data.Length * 2);
            }
            this.Length = length;
        }

        private void EnsureLengthIsLargeEnough(int length)
        {
            if (length > this.Length)
            {
                SetLength(length);
            }
        }

        public void WritePointer(long index, long pointer)
        {
            EnsureLengthIsLargeEnough((int)index + 8);
            Data.WriteInt64(index, pointer);
            PointerOffsets.Add(index);
        }

        public async Task WritePointerAsync(long index, long pointer)
        {
            EnsureLengthIsLargeEnough((int)index + 8);
            await Data.WriteInt64Async(index, pointer).ConfigureAwait(false);
            PointerOffsets.Add(index);
        }

        public Sir0 Build()
        {
            this.WriteString(0, Encoding.ASCII, Magic);
            this.WritePointer(8, SubHeaderOffset);
            var footerOffset = this.Length + (0x10 - (this.Length % 0x10));
            this.WritePointer(16, footerOffset);
            WriteFooter(footerOffset);

            var newData = new byte[this.Length];
            Array.Copy(Data.ReadArray(), newData, this.Length);
            return new Sir0(newData);
        }

        private void WriteFooter(int footerOffset)
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
            this.Write(footerOffset, new byte[paddingLength]);
        }

        #region IWriteOnlyBinaryDataAccessor
        public void Write(byte[] value)
        {
            EnsureLengthIsLargeEnough(value.Length);
            Data.Write(value);
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
        #endregion
    }
}
