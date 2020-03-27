using SkyEditor.IO;
using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public sealed class Sir0 : IReadOnlyBinaryDataAccessor
    {
        public Sir0(IReadOnlyBinaryDataAccessor data)
        {
            DataAccessor = data;
            Init();
        }

        public Sir0(byte[] data, long offset, long length)
        {
            var binaryFile = new BinaryFile(data);
            DataAccessor = new ReadOnlyBinaryDataAccessorReference(binaryFile, offset, length);
            Init();
        }

        public Sir0(byte[] data)
        {
            DataAccessor = new BinaryFile(data);
            Init();
        }

        private void Init()
        {
            Magic = DataAccessor.ReadInt32(0);
            SubHeaderOffset = DataAccessor.ReadInt32(8);
            FooterOffset = DataAccessor.ReadInt32(16);
            SubHeader = DataAccessor.GetReadOnlyDataReference(SubHeaderOffset, FooterOffset - SubHeaderOffset);

            PointerOffsets = new List<long>();
            long pointerIndex = 0;
            var currentFooterOffset = FooterOffset;
            var rawByte = this.ReadByte(currentFooterOffset++);
            while (rawByte != 0)
            {
                if (rawByte < 0x80)
                {
                    pointerIndex += rawByte;
                    PointerOffsets.Add(pointerIndex);
                    rawByte = this.ReadByte(currentFooterOffset++);
                }
                else
                {
                    long workingPointer;
                    do
                    {
                        workingPointer = rawByte & 0x7F;
                        workingPointer <<= 7;
                        rawByte = this.ReadByte(currentFooterOffset++);
                    } while (rawByte >= 0x80);
                    pointerIndex += workingPointer;
                    PointerOffsets.Add(pointerIndex);
                }
            }
        }

        private IReadOnlyBinaryDataAccessor DataAccessor { get; }
        public int Magic { get; private set; }
        public long SubHeaderOffset { get; private set; }
        public long FooterOffset { get; private set; }

        public IReadOnlyBinaryDataAccessor SubHeader { get; private set; } = default!;
        public List<long> PointerOffsets { get; private set; } = default!;

        #region IReadOnlyBinaryDataAccessor Implementation
        public long Length => DataAccessor.Length;
        public byte[] ReadArray() => DataAccessor.ReadArray();
        public byte[] ReadArray(long index, int length) => DataAccessor.ReadArray(index, length);
        public Task<byte[]> ReadArrayAsync() => DataAccessor.ReadArrayAsync();
        public Task<byte[]> ReadArrayAsync(long index, int length) => DataAccessor.ReadArrayAsync(index, length);
        public byte ReadByte(long index) => DataAccessor.ReadByte(index);
        public Task<byte> ReadByteAsync(long index) => DataAccessor.ReadByteAsync(index);
        public Task<ReadOnlyMemory<byte>> ReadMemoryAsync() => DataAccessor.ReadMemoryAsync();
        public Task<ReadOnlyMemory<byte>> ReadMemoryAsync(long index, int length) => DataAccessor.ReadMemoryAsync(index, length);
        public ReadOnlySpan<byte> ReadSpan() => DataAccessor.ReadSpan();
        public ReadOnlySpan<byte> ReadSpan(long index, int length) => DataAccessor.ReadSpan(index, length);
        #endregion
    }
}
