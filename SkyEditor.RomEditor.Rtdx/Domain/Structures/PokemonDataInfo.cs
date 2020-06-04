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

        public List<PokemonDataInfoEntry> Entries { get; }

        public PokemonDataInfo(IReadOnlyBinaryDataAccessor data)
        {
            var entryCount = checked((int)data.Length / EntrySize);
            var entries = new List<PokemonDataInfoEntry>(entryCount);
            for (int i = 0; i < entryCount; i++)
            {
                entries.Add(new PokemonDataInfoEntry((CreatureIndex)i, data.Slice(i * EntrySize, EntrySize)));
            }
            this.Entries = entries;
        }

        public PokemonDataInfoEntry GetByPokemonId(CreatureIndex creatureIndex)
        {
            return Entries[(int)creatureIndex];
        }

        public class PokemonDataInfoEntry
        {
            public CreatureIndex Id { get; }

            public IReadOnlyList<LevelUpMove> LevelupLearnset { get; }
            public short PokedexEntry { get; }

            public short Taxon { get; }
            public short BaseHitPoints { get; }
            public short BaseAttack { get; }
            public short BaseDefense { get; }
            public short BaseSpecialAttack { get; }
            public short BaseSpecialDefense { get; }
            public short BaseSpeed { get; }
            public byte ExperienceEntry { get; }
            public AbilityIndex Ability1 { get; }
            public AbilityIndex Ability2 { get; }
            public AbilityIndex HiddenAbility { get; }
            public PokemonType Type1 { get; }
            public PokemonType Type2 { get; }
            public string RecruitPrereq { get; }

            public PokemonDataInfoEntry(CreatureIndex id, IReadOnlyBinaryDataAccessor data)
            {
                this.Id = id;

                var levelupLearnset = new List<LevelUpMove>(26);
                for (int i = 0; i < 26; i++)
                {
                    WazaIndex move = (WazaIndex)data.ReadInt16(0x10 + i * sizeof(short));
                    byte level = data.ReadByte(0x44 + i);
                    levelupLearnset.Add(new LevelUpMove(level, move));
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
                Ability1 = (AbilityIndex)data.ReadByte(0x90);
                Ability2 = (AbilityIndex)data.ReadByte(0x91);
                HiddenAbility = (AbilityIndex)data.ReadByte(0x92);
                Type1 = (PokemonType)data.ReadByte(0x93);
                Type2 = (PokemonType)data.ReadByte(0x94);
                RecruitPrereq = data.ReadString(0x9B, 69, Encoding.ASCII);
            }

            public struct LevelUpMove
            {
                public LevelUpMove(byte level, WazaIndex move)
                {
                    this.Level = level;
                    this.Move = move;
                }

                public byte Level { get; }
                public WazaIndex Move { get; }
            }
        }
    }
}