using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Reverse
{
    public interface IBinaryDataByteStream
    {
        bool ReadBool();
        int ReadInt32();
        uint ReadUInt32();
        byte ReadByte();
        short ReadInt16();
        ushort ReadUInt16();
        long ReadInt64();
        ulong ReadUInt64();
        float ReadFloat();
        string ReadStringASCII();
        string ReadStringUnicode();
        void WriteBool(bool v);
        void WriteInt32(int v);
        void WriteUInt32(uint v);
        void WriteInt16(int v);
        void WriteUInt16(uint v);
        void WriteInt64(long v);
        void WriteUInt64(ulong v);
        void WriteByte(byte v);
        void WriteFloat(float v);
        void WriteStringASCII(string v);
        void WriteStringUnicode(string v);
    }
}
