using SkyEditor.IO.Binary;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class CampHabitat
    {
        // Credit to AntyMew
        public IDictionary<CreatureIndex, CampIndex> Entries { get; } = new Dictionary<CreatureIndex, CampIndex>();

        public CampHabitat(byte[] data) : this(new BinaryFile(data))
        {
        }

        public CampHabitat(IReadOnlyBinaryDataAccessor data)
        {
            var entryCount = checked((int)data.Length / sizeof(int));
            for (int i = 0; i < entryCount; i++)
                Entries.Add((CreatureIndex)i, (CampIndex)data.ReadInt32(i * sizeof(int)));
        }
    }
}
