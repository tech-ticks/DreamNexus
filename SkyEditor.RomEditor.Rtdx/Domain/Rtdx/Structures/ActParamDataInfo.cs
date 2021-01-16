using System;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IActParamDataInfo
    {
        byte[] Entries { get; }
        byte[] Build();
    }

    public class ActParamDataInfo : IActParamDataInfo
    {
        public byte[] Entries { get; } = new byte[70];

        public ActParamDataInfo()
        {
        }

        public ActParamDataInfo(byte[] data)
        {
            Array.Copy(data, Entries, Entries.Length);
        }

        public byte[] Build()
        {
            return Entries;
        }
    }
}
