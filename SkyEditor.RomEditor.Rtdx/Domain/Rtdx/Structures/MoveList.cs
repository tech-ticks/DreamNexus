using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IMoveList
    {
        public IList<WazaIndex> Entries { get; }

        public byte[] ToByteArray();
    }

    public class MoveList : IMoveList
    {
        public const int EntrySize = 0x2;

        public MoveList()
        {
            this.Entries = new List<WazaIndex>();
        }

        public MoveList(byte[] data) : this(new BinaryFile(data))
        {
        }

        public MoveList(IReadOnlyBinaryDataAccessor data)
        {
            var entries = new List<WazaIndex>();
            for (int i = 0; i < data.Length / EntrySize; i++)
            {
                entries.Add((WazaIndex)data.ReadUInt16(i * 2));
            }
            this.Entries = entries;
        }

        public byte[] ToByteArray()
        {
            IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize * Entries.Count]);
            int currentIndex = 0;
            foreach (var entry in Entries)
            {
                data.WriteUInt16(currentIndex, (ushort)entry);
                currentIndex += EntrySize;
            }
            return data.ReadArray();
        }

        public IList<WazaIndex> Entries { get; }
    }
}
