using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Rtdx.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public partial class Gyu0Stream
    {
        private abstract class Command
        {
            public enum Opcode
            {
                Copy,
                SplitCopy,
                Fill,
                Skip
            }

            public abstract (byte Result, int BytesRead)? ReadByte(IReadOnlyBinaryDataAccessor data, long offset);

            public abstract (int BytesRead, int BytesWritten)? Read(IReadOnlyBinaryDataAccessor data, long dataOffset, byte[] dest, int destOffset, int count);

            public abstract ValueTask<(int BytesRead, int BytesWritten)?> ReadAsync(IReadOnlyBinaryDataAccessor data, long dataOffset, byte[] dest, int destOffset, int count);

            public static (Command Command, int BytesRead) Decode(IReadOnlyBinaryDataAccessor data, long offset, RingBuffer<byte> backrefBuffer)
            {
                var cur = data.ReadByte(offset);

                if (cur < 0x80)
                {
                    var next = data.ReadByte(offset + 1);

                    if (cur == 0x7F && next == 0xFF)
                        return (new EOFCommand(), 2);

                    return (new BackreferenceCommand((cur >> 2) + 2, 0x400 - ((cur & 3) * 0x100 + next), backrefBuffer), 2);
                }

                var op = (Opcode)((cur >> 5) - 4);
                var arg = cur & 0x1F;

                switch (op)
                {
                    case Opcode.Copy:
                        return (new CopyCommand(arg + 1), 1);

                    case Opcode.SplitCopy:
                    {
                        var separator = data.ReadByte(offset + 1);
                        return (new SplitCopyCommand(2 * (arg + 2), separator), 2);
                    }

                    case Opcode.Fill:
                    {
                        var value = data.ReadByte(offset + 1);
                        return (new FillCommand(arg + 2, value), 2);
                    }

                    case Opcode.Skip:
                    {
                        var byteCount = arg + 1;
                        if (byteCount == 0x20)
                            return (new SkipCommand(byteCount + data.ReadByte(offset + 1)), 2);
                        return (new SkipCommand(byteCount), 1);
                    }

                    default:
                        // Should be unreachable, but we need to throw an exception to please the compiler
                        throw new InvalidDataException();
                }
            }
        }
    }
}