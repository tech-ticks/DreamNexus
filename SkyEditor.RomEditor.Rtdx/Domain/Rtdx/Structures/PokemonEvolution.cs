using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Common.Structures;
using YamlDotNet.Serialization;

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
            var entryCount = sir0.SubHeader.ReadInt64(8);
            for (int i = 0; i < entryCount; i++)
            {
                Entries.Add((CreatureIndex)i, new Entry(sir0, sir0.Data.Slice(indexOffset + i * EntrySize, EntrySize)));
            }
        }

        public Sir0 ToSir0()
        {
            var sir0 = new Sir0Builder(8);
            
            var branchPointers = new List<long>();
            var emptyEvolutionBranch = new PokemonEvolutionBranch();

            // Write branches
            foreach (var entry in Entries)
            {
                branchPointers.Add(sir0.Length);
                if (entry.Value.Branches.Count > 0)
                {
                    foreach (var branch in entry.Value.Branches)
                    {
                        branch.WriteTo(sir0);
                    }
                }
                else
                {
                    emptyEvolutionBranch.WriteTo(sir0);
                }
            }

            sir0.Align(0x10);
            long entriesOffset = sir0.Length;

            // Write entries
            for (int i = 0; i < Entries.Count; i++)
            {
                var entry = Entries[(CreatureIndex)i];
                entry.WriteTo(sir0, branchPointers[i]);
            }

            sir0.SubHeaderOffset = sir0.Length;
            sir0.WritePointer(sir0.Length, entriesOffset);
            sir0.WriteInt64(sir0.Length, Entries.Count);

            return sir0.Build();
        }

        public byte[] ToByteArray() => ToSir0().Data.ReadArray();

        public class Entry
        {
            public IList<PokemonEvolutionBranch> Branches { get; set; } = new List<PokemonEvolutionBranch>();
            public (CreatureIndex First, CreatureIndex Second) MegaEvos { get; set; }

            public Entry(Sir0 sir0, IReadOnlyBinaryDataAccessor data)
            {
                int branchOffset = checked((int)data.ReadInt64(0x0));
                int branchCount = data.ReadInt32(0x8);
                var branches = new List<PokemonEvolutionBranch>(branchCount);
                for (int i = 0; i < branchCount; i++)
                {
                    branches.Add(new PokemonEvolutionBranch(sir0.Data.Slice(branchOffset + i * BranchEntrySize, BranchEntrySize)));
                }
                this.Branches = branches;
                MegaEvos = (
                    (CreatureIndex)data.ReadInt16(0xC),
                    (CreatureIndex)data.ReadInt16(0xE)
                );
            }

            public void WriteTo(Sir0Builder sir0, long branchPointer)
            {
                sir0.WritePointer(sir0.Length, branchPointer);
                sir0.WriteInt32(sir0.Length, Branches.Count);
                sir0.WriteInt16(sir0.Length, (short)MegaEvos.First);
                sir0.WriteInt16(sir0.Length, (short)MegaEvos.Second);
            }
        }

        [Flags]
        public enum Requirements
        {
            None = 0,
            Level = 0x01,
            Item = 0x02
        }

        public class PokemonEvolutionBranch
        {
            public Requirements RequirementFlags { get; set; }
            public CreatureIndex Evolution { get; set; }

            public ItemIndex EvolutionItem { get; set; }

            public short ItemsRequired { get; set; }

            public byte MinimumLevel { get; set; }

            public int Int00 { get; set; } // Always 0, except for Haunter

            [YamlIgnore]
            public bool HasMinimumLevel => RequirementFlags.HasFlag(Requirements.Level);

            [YamlIgnore]
            public bool RequiresItem => RequirementFlags.HasFlag(Requirements.Item);

            public PokemonEvolutionBranch()
            {
            }

            public PokemonEvolutionBranch(IReadOnlyBinaryDataAccessor accessor)
            {
                Int00 = accessor.ReadInt32(0x0);
                Evolution = (CreatureIndex)accessor.ReadInt16(0x4);
                EvolutionItem = (ItemIndex)accessor.ReadInt16(0x6);
                ItemsRequired = accessor.ReadInt16(0x8);
                MinimumLevel = accessor.ReadByte(0x10);
                RequirementFlags = (Requirements)accessor.ReadByte(0x11);
            }

            public void WriteTo(Sir0Builder sir0)
            {
                sir0.WriteInt32(sir0.Length, Int00);
                sir0.WriteInt16(sir0.Length, (short)Evolution);
                sir0.WriteInt16(sir0.Length, (short)EvolutionItem);
                sir0.WriteInt16(sir0.Length, ItemsRequired);
                sir0.WritePadding(sir0.Length, 0x6);
                sir0.Write(sir0.Length, MinimumLevel);
                sir0.Write(sir0.Length, (byte)RequirementFlags);
                sir0.WritePadding(sir0.Length, 0x2);
            }

            public PokemonEvolutionBranch Clone()
            {
                return new PokemonEvolutionBranch
                {
                    RequirementFlags = RequirementFlags,
                    Evolution = Evolution,
                    EvolutionItem = EvolutionItem,
                    ItemsRequired = ItemsRequired,
                    MinimumLevel = MinimumLevel,
                    Int00 = Int00,
                };
            }
        }
    }
}
