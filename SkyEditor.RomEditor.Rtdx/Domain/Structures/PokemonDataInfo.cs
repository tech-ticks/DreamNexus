using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Rtdx.Constants;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    // Source: https://pastebin.com/84cBGfrc
    // Credit to AntyMew
    public class PokemonDataInfo
    {
        private const int EntrySize = 0xE0;

        public IDictionary<CreatureIndex, PokemonDataInfoEntry> Entries { get; }

        public PokemonDataInfo(IReadOnlyBinaryDataAccessor data)
        {
            var entryCount = checked((int)data.Length / EntrySize);
            var entries = new Dictionary<CreatureIndex, PokemonDataInfoEntry>(entryCount);
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add((CreatureIndex)i, new PokemonDataInfoEntry(data.Slice(i * EntrySize, EntrySize)));
            }
            this.Entries = entries;
        }

        public class PokemonDataInfoEntry
        {
            public IReadOnlyList<(byte Level, WazaIndex Move)> LevelupLearnset { get; }
            public short PokedexEntry { get; }

            public short Taxon { get; }
            public short BaseHitPoints { get; }
            public short BaseAttack { get; }
            public short BaseDefense { get; }
            public short BaseSpecialAttack { get; }
            public short BaseSpecialDefense { get; }
            public short BaseSpeed { get; }
            public byte ExperienceEntry { get; }
            public (AbilityIndex First, AbilityIndex Second) Abilities { get; }
            public AbilityIndex HiddenAbility { get; }
            public (PokemonType Primary, PokemonType Secondary) Types { get; }
            public string RecruitPrereq { get; }

            public PokemonDataInfoEntry(IReadOnlyBinaryDataAccessor data)
            {
                var levelupLearnset = new List<(byte, WazaIndex)>(26);
                for (int i = 0; i < 26; i++)
                {
                    WazaIndex move = (WazaIndex)data.ReadInt16(0x10 + i * sizeof(short));
                    byte level = data.ReadByte(0x44 + i);
                    levelupLearnset.Add((level, move));
                }
                this.LevelupLearnset = levelupLearnset;
                PokedexEntry = data.ReadInt16(0x64);
                Taxon = data.ReadInt16(0x68);
                BaseHitPoints = data.ReadInt16(0x74);
                BaseAttack = data.ReadInt16(0x76);
                BaseDefense = data.ReadInt16(0x78);
                BaseSpecialAttack = data.ReadInt16(0x7A);
                BaseSpecialDefense = data.ReadInt16(0x7C);
                BaseSpeed = data.ReadInt16(0x7E);
                ExperienceEntry = data.ReadByte(0x84);
                Abilities = ((AbilityIndex)data.ReadByte(0x90), (AbilityIndex)data.ReadByte(0x91));
                HiddenAbility = (AbilityIndex)data.ReadByte(0x92);
                Types = ((PokemonType)data.ReadByte(0x93), (PokemonType)data.ReadByte(0x94));
                RecruitPrereq = data.ReadString(0x9B, 69, Encoding.ASCII);
            }
        }
    }
}