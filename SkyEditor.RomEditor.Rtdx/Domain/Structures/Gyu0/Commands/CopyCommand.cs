using SkyEditor.IO.Binary;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public partial class Gyu0Stream
    {
        private class CopyCommand : Command
        {
            private int byteCount;

            public CopyCommand(int byteCount)
            {
                this.byteCount = byteCount;
            }

            public override (byte, int)? ReadByte(IReadOnlyBinaryDataAccessor data, long position)
            {
                return (byteCount-- <= 0) ? null : ((byte, int)?)(data.ReadByte(position), 1);
            }

            public override (int, int)? Read(IReadOnlyBinaryDataAccessor data, long position, byte[] dest, int offset, int count)
            {
                if (byteCount <= 0)
                    return null;

                var bytes = data.ReadArray(position, Math.Min(count, byteCount));
                bytes.CopyTo(dest, offset);
                byteCount -= bytes.Length;
                return (bytes.Length, bytes.Length);
            }

            public override async ValueTask<(int, int)?> ReadAsync(IReadOnlyBinaryDataAccessor data, long position, byte[] dest, int offset, int count)
            {
                if (byteCount <= 0)
                    return null;

                var bytes = await data.ReadArrayAsync(position, Math.Min(count, byteCount));
                bytes.CopyTo(dest, offset);
                byteCount -= bytes.Length;
                return (bytes.Length, bytes.Length);
            }
        }
    }
}