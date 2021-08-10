using System.IO;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures.Custom
{
    public class DefaultStarters
    {
        public CreatureIndex HeroCreature { get; set; }
        public PokemonGenderType HeroGender { get; set; }
        public CreatureIndex PartnerCreature { get; set; }
        public PokemonGenderType PartnerGender { get; set; }

        public DefaultStarters()
        {
            HeroCreature = CreatureIndex.PIKACHUU;
            PartnerCreature = CreatureIndex.FUSHIGIDANE;
        }

        public DefaultStarters(byte[] data)
        {
            using var file = new BinaryFile(data);
            HeroCreature = (CreatureIndex) file.ReadUInt16(0x0);
            HeroGender = (PokemonGenderType) file.ReadInt16(0x2);
            PartnerCreature = (CreatureIndex) file.ReadUInt16(0x4);
            PartnerGender = (PokemonGenderType) file.ReadInt16(0x6);
        }

        public byte[] ToByteArray()
        {
            using var file = new BinaryFile(new MemoryStream());
            file.WriteUInt16(file.Length, (ushort) HeroCreature);
            file.WriteInt16(file.Length, (short) HeroGender);
            file.WriteUInt16(file.Length, (ushort) PartnerCreature);
            file.WriteInt16(file.Length, (short) PartnerGender);
            return file.ReadArray();
        }
    }
}
