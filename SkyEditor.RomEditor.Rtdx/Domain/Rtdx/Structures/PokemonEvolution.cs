using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Common.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class PokemonEvolution
    {
        // Credit to AntyMew
        private const int EntrySize = 0x10;
        private const int BranchEntrySize = 0x14;

        public IDictionary<CreatureIndex, Entry> Entries { get; } = new Dictionary<CreatureIndex, Entry>();

        public PokemonEvolution(byte[] data)
        {
            var sir0 = new Sir0(data);
            var indexOffset = checked((int)sir0.SubHeader.ReadInt64(0));
            var entryCount = sir0.SubHeader.ReadInt32(8);
            for (int i = 0; i < entryCount; i++)
            {
                Entries.Add((CreatureIndex)i, new Entry(sir0, sir0.Data.Slice(indexOffset + i * EntrySize, EntrySize)));
            }
        }

        public byte[] ToByteArray() => throw new NotImplementedException();

        public class Entry
        {
            public IList<PokemonEvolutionBranch> Branches { get; } = new List<PokemonEvolutionBranch>();
            public (CreatureIndex First, CreatureIndex Second) MegaEvos { get; set; }

            public Entry(Sir0 sir0, IReadOnlyBinaryDataAccessor data)
            {
                int branchOffset = checked((int)data.ReadInt64(0x0));
                int branchCount = data.ReadInt32(0x8);
                var branches = new List<PokemonEvolutionBranch>(branchCount);
                for (int i = 0; i < branchCount; i++)
                    branches.Add(new PokemonEvolutionBranch(sir0.Data.Slice(branchOffset + i * BranchEntrySize, BranchEntrySize)));
                this.Branches = branches;
                MegaEvos = (
                    (CreatureIndex)data.ReadInt16(0xC),
                    (CreatureIndex)data.ReadInt16(0xE)
                );
            }
        }

        public class PokemonEvolutionBranch
        {
            private Requirements RequirementFlags { get; set; }

            public CreatureIndex Evolution { get; set; }

            public ItemIndex EvolutionItem { get; set; }

            public short ItemsRequired { get; set; }

            public byte MinimumLevel { get; set; }

            public bool HasMinimumLevel => RequirementFlags.HasFlag(Requirements.Level);

            public bool RequiresItem => RequirementFlags.HasFlag(Requirements.Item);

            [Flags]
            private enum Requirements
            {
                None = 0,
                Level = 0x01,
                Item = 0x02
            }

            public PokemonEvolutionBranch(IReadOnlyBinaryDataAccessor accessor)
            {
                Evolution = (CreatureIndex)accessor.ReadInt16(0x4);
                EvolutionItem = (ItemIndex)accessor.ReadInt16(0x6);
                ItemsRequired = accessor.ReadInt16(0x8);
                MinimumLevel = accessor.ReadByte(0x10);
                RequirementFlags = (Requirements)accessor.ReadByte(0x11);
            }
        }
    }
}
