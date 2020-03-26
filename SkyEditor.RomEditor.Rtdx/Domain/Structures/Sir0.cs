using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public abstract class Sir0
    {
        public Sir0(byte[] data, int offset = 0)
        {
            Magic = BitConverter.ToInt64(data, offset + 0);
            SubHeaderOffset = BitConverter.ToInt64(data, offset + 8);
            FooterOffset = BitConverter.ToInt64(data, offset + 16);
        }

        public long Magic { get; }
        public long SubHeaderOffset { get; }
        public long FooterOffset { get; }
    }
}
