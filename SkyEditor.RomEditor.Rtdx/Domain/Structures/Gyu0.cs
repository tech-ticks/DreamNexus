using SkyEditor.IO.Binary;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
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
                    case Opcode.Copy:
                        {
                            var bytes = data.ReadSpan(dataIndex, arg + 1);
                            dataIndex += bytes.Length;
                            dest.Write(destIndex, bytes);
                            destIndex += bytes.Length;
                            break;
                        }

                    case Opcode.SplitCopy:
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

                    case Opcode.Fill:
                        {
                            var fill = data.ReadByte(dataIndex++);
                            for (int i = 0; i < arg + 2; i++)
                                dest.Write(destIndex++, fill);
                            break;
                        }

                    case Opcode.Skip:
                        {
                            var count = arg < 0x1F ? arg + 1 : 0x20 + data.ReadByte(dataIndex++);
                            destIndex += count;
                            break;
                        }
                }
            }
        }
    }
}
