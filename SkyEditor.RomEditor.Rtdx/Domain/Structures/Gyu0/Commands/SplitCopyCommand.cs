using SkyEditor.IO.Binary;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public partial class Gyu0Stream
    {
        private class SplitCopyCommand : Command
        {
            private int byteCount;

            private readonly byte separator;

            public SplitCopyCommand(int byteCount, byte separator)
            {
                this.byteCount = byteCount;
                this.separator = separator;
            }

            public override (byte, int)? ReadByte(IReadOnlyBinaryDataAccessor data, long position)
            {
                if (byteCount <= 0)
                    return null;
                else if (byteCount-- % 2 == 0)
                    return (separator, 0);
                else
                    return (data.ReadByte(position), 1);
            }

            public override (int, int)? Read(IReadOnlyBinaryDataAccessor data, long position, byte[] dest, int offset, int count)
            {
                if (byteCount <= 0)
                    return null;

                var toWrite = Math.Min(count, byteCount);
                var toRead = Math.Max(toWrite / 2, byteCount % 2);

                var bytes = data.ReadArray(position, toRead);

                for (int i = 0; i < toWrite; i++)
                {
                    if ((byteCount + i) % 2 == 0)
                        dest[offset + i] = separator;
                    else
                        dest[offset + i] = bytes[i / 2];
                }

                byteCount -= toWrite;
                return (toRead, toWrite);
            }

            public override async ValueTask<(int, int)?> ReadAsync(IReadOnlyBinaryDataAccessor data, long position, byte[] dest, int offset, int count)
            {
                if (byteCount <= 0)
                    return null;

                var toWrite = Math.Min(count, byteCount);
                var toRead = Math.Max(toWrite / 2, (byteCount + 1) % 2);

                var bytes = await data.ReadArrayAsync(position, toRead);

                for (int i = 0; i < toWrite; i++)
                {
                    if ((byteCount + i) % 2 == 0)
                        dest[offset + i] = separator;
                    else
                        dest[offset + i] = bytes[i / 2];
                }

                byteCount -= toWrite;
                return (toRead, toWrite);
            }
        }
    }
}