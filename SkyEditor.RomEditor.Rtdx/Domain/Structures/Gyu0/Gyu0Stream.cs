using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Rtdx.Infrastructure;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public partial class Gyu0Stream : Stream
    {
        private Command? currentCommand;
        private readonly RingBuffer<byte> backrefBuffer;
        private readonly IReadOnlyBinaryDataAccessor data;
        public long position; // Position in the input file
        public bool done;

        // Position in the output
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override long Length { get; }

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanSeek => false;

        public Gyu0Stream(IReadOnlyBinaryDataAccessor data)
        {
            this.data = data;
            backrefBuffer = new RingBuffer<byte>(0x400);

            if (data.ReadString(0, 4, Encoding.ASCII) != "GYU0")
                throw new InvalidDataException();

            Length = data.ReadInt32(4);
            position = 8;
        }

        public override int ReadByte()
        {
            if (currentCommand == null)
            {
                (var command, var read) = Command.Decode(data, position, backrefBuffer);
                currentCommand = command;
                position += read;
                if (currentCommand == null)
                    return -1;
            }

            var ret = currentCommand.ReadByte(data, position);

            if (ret == null)
            {
                currentCommand = null;
                return ReadByte();
            }

            position += ret.Value.BytesRead;
            backrefBuffer.Add(ret.Value.Result);
            return ret.Value.Result;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (done)
                return 0;

            int totalWritten = 0;

            while (totalWritten < count)
            {
                if (currentCommand == null)
                {
                    (var command, var read) = Command.Decode(data, position, backrefBuffer);
                    currentCommand = command;
                    position += read;
                    if (currentCommand == null)
                        return totalWritten;
                }

                var result = currentCommand.Read(data, position, buffer, offset + totalWritten, count - totalWritten);

                if (result == null)
                {
                    currentCommand = null;
                }
                else
                {
                    position += result.Value.BytesRead;
                    backrefBuffer.AddRange(buffer, offset + totalWritten, result.Value.BytesWritten);
                    totalWritten += result.Value.BytesWritten;
                }
            }

            return totalWritten;
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            if (done)
                return 0;

            int totalWritten = 0;

            while (totalWritten < count)
            {
                if (currentCommand == null)
                {
                    (var command, var read) = Command.Decode(data, position, backrefBuffer);
                    currentCommand = command;
                    position += read;
                    if (currentCommand is EOFCommand)
                        return totalWritten;
                }

                var result = await currentCommand.ReadAsync(data, position, buffer, offset + totalWritten, count - totalWritten);

                if (result == null)
                {
                    currentCommand = null;
                }
                else
                {
                    position += result.Value.BytesRead;
                    backrefBuffer.AddRange(buffer, offset + totalWritten, result.Value.BytesWritten);
                    totalWritten += result.Value.BytesWritten;
                }
            }

            return totalWritten;
        }

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Flush()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                backrefBuffer.Dispose();
        }
    }
}