using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Rtdx.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public partial class Gyu0Stream
    {
        private class BackreferenceCommand : Command
        {
            private int byteCount;
            private readonly int offset;
            private readonly RingBuffer<byte> buffer;

            public BackreferenceCommand(int byteCount, int offset, RingBuffer<byte> buffer)
            {
                this.byteCount = byteCount;
                this.offset = offset;
                this.buffer = buffer;
            }

            public override (byte, int)? ReadByte(IReadOnlyBinaryDataAccessor data, long position)
            {
                return (byteCount-- <= 0) ? null : ((byte, int)?)(buffer[^offset], 0);
            }

            public override (int, int)? Read(IReadOnlyBinaryDataAccessor data, long position, byte[] dest, int destOffset, int count)
            {
                if (byteCount <= 0)
                    return null;

                var toWrite = Math.Min(count, byteCount);
                buffer.Slice(buffer.Count - offset, toWrite).CopyTo(dest, destOffset);
                byteCount -= toWrite;
                return (0, toWrite);
            }

            public override ValueTask<(int, int)?> ReadAsync(IReadOnlyBinaryDataAccessor data, long position, byte[] dest, int offset, int count)
                => new ValueTask<(int, int)?>(Read(data, position, dest, offset, count));
        }
    }
}