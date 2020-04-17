using SkyEditor.IO.Binary;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public partial class Gyu0Stream
    {
        private class EOFCommand : Command
        {
            public EOFCommand()
            {
            }

            public override (byte, int)? ReadByte(IReadOnlyBinaryDataAccessor data, long position) => null;

            public override (int, int)? Read(IReadOnlyBinaryDataAccessor data, long position, byte[] dest, int offset, int count) => null;

            public override ValueTask<(int, int)?> ReadAsync(IReadOnlyBinaryDataAccessor data, long position, byte[] dest, int offset, int count)
                => new ValueTask<(int, int)?>(((int, int)?)null);
        }
    }
}