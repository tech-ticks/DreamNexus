using SkyEditor.IO.Binary;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public partial class Gyu0Stream
    {
        private class FillCommand : Command
        {
            private int byteCount;

            private readonly byte value;

            public FillCommand(int byteCount, byte value)
            {
                this.byteCount = byteCount;
                this.value = value;
            }

            public override (byte, int)? ReadByte(IReadOnlyBinaryDataAccessor data, long position)
            {
                return (byteCount-- <= 0) ? null : ((byte, int)?)(value, 0);
            }

            public override (int, int)? Read(IReadOnlyBinaryDataAccessor data, long position, byte[] dest, int offset, int count)
            {
                if (byteCount <= 0)
                    return null;

                var toWrite = Math.Min(count, byteCount);
                Array.Fill(dest, value, offset, toWrite);
                byteCount -= toWrite;
                return (0, toWrite);
            }

            public override ValueTask<(int, int)?> ReadAsync(IReadOnlyBinaryDataAccessor data, long position, byte[] dest, int offset, int count)
                => new ValueTask<(int, int)?>(Read(data, position, dest, offset, count));
        }
    }
}