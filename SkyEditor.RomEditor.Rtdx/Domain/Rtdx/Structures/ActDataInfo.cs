using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public interface IActDataInfo
    {
        public IList<ActDataInfo.Entry> Entries { get; }

        public byte[] ToByteArray();
    }

    public class ActDataInfo : IActDataInfo
    {
        public const int EntrySize = 0xA0;

        public ActDataInfo()
        {
            this.Entries = new List<Entry>();
        }

        public ActDataInfo(byte[] data) : this(new BinaryFile(data))
        {
        }

        public ActDataInfo(IReadOnlyBinaryDataAccessor data)
        {
            var entries = new List<Entry>();
            for (int i = 0; i < data.Length / EntrySize; i++)
            {
                entries.Add(new Entry(data.Slice(i * EntrySize, EntrySize)));
            }
            this.Entries = entries;
        }

        public byte[] ToByteArray()
        {
            IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize * Entries.Count]);
            int currentIndex = 0;
            foreach (var entry in Entries)
            {
                data.Write(currentIndex, entry.ToBytes());
                currentIndex += EntrySize;
            }
            return data.ReadArray();
        }

        public IList<Entry> Entries { get; }

        // Some notes about actions:
        // - Action table includes all actions from moves, items and traps.
        // - Moves that take two turns are split into two actions.
        //   - Example: Solar Beam
        //     - Charge-up action index: 76
        //     - Fire action index: 668

        [DebuggerDisplay("{Name}")]
        public class Entry
        {
            public Entry() { }

            public Entry(IReadOnlyBinaryDataAccessor data)
            {
                Long00 = data.ReadUInt64(0x00);
                Text08 = (TextIDHash)data.ReadInt32(0x08);
                Text0C = (TextIDHash)data.ReadInt32(0x0C);

                for (var i = 0; i < 4; i++)
                {
                    Actions[i] = new Action(data, i);
                }

                Short58 = data.ReadUInt16(0x58);
                Short5A = data.ReadUInt16(0x5A);

                Byte7C = data.ReadByte(0x7C);
                MoveType = (PokemonType)data.ReadByte(0x7D);
                MoveCategory = (MoveCategory)data.ReadByte(0x7E);
                Byte7F = data.ReadByte(0x7F);
                Byte80 = data.ReadByte(0x80);
                Byte81 = data.ReadByte(0x81);
                Byte82 = data.ReadByte(0x82);
                Byte83 = data.ReadByte(0x83);
                Byte84 = data.ReadByte(0x84);
                Byte85 = data.ReadByte(0x85);
                Byte86 = data.ReadByte(0x86);
                Byte87 = data.ReadByte(0x87);
                Range = data.ReadByte(0x88);
                Byte89 = data.ReadByte(0x89);
                Byte8A = data.ReadByte(0x8A);
                Byte8B = data.ReadByte(0x8B);
                Target = (ActionTarget)data.ReadByte(0x8C);
                Byte8D = data.ReadByte(0x8D);
                Byte8E = data.ReadByte(0x8E);
                Byte8F = data.ReadByte(0x8F);
                Byte90 = data.ReadByte(0x90);
                Byte91 = data.ReadByte(0x91);
                Byte92 = data.ReadByte(0x92);
                ActHitCountIndex = data.ReadByte(0x93);
                Byte94 = data.ReadByte(0x94);
                Byte95 = data.ReadByte(0x95);
                Byte96 = data.ReadByte(0x96);
                Byte97 = data.ReadByte(0x97);
                Byte98 = data.ReadByte(0x98);
                Byte99 = data.ReadByte(0x99);
                Byte9A = data.ReadByte(0x9A);
                Byte9B = data.ReadByte(0x9B);
                Byte9C = data.ReadByte(0x9C);
                Byte9D = data.ReadByte(0x9D);
                Byte9E = data.ReadByte(0x9E);
                Byte9F = data.ReadByte(0x9F);
            }

            public ReadOnlySpan<byte> ToBytes()
            {
                IBinaryDataAccessor data = new BinaryFile(new byte[EntrySize]);
                data.WriteUInt64(0x00, Long00);
                data.WriteInt32(0x08, (int)Text08);
                data.WriteInt32(0x0C, (int)Text0C);

                for (var i = 0; i < 4; i++)
                {
                    Actions[i].WriteTo(data, i);
                }

                data.WriteUInt16(0x58, Short58);
                data.WriteUInt16(0x5A, Short5A);

                data.Write(0x7C, Byte7C);
                data.Write(0x7D, (byte)MoveType);
                data.Write(0x7E, (byte)MoveCategory);
                data.Write(0x7F, Byte7F);
                data.Write(0x80, Byte80);
                data.Write(0x81, Byte81);
                data.Write(0x82, Byte82);
                data.Write(0x83, Byte83);
                data.Write(0x84, Byte84);
                data.Write(0x85, Byte85);
                data.Write(0x86, Byte86);
                data.Write(0x87, Byte87);
                data.Write(0x88, Range);
                data.Write(0x89, Byte89);
                data.Write(0x8A, Byte8A);
                data.Write(0x8B, Byte8B);
                data.Write(0x8C, (byte)Target);
                data.Write(0x8D, Byte8D);
                data.Write(0x8E, Byte8E);
                data.Write(0x8F, Byte8F);
                data.Write(0x90, Byte90);
                data.Write(0x91, Byte91);
                data.Write(0x92, Byte92);
                data.Write(0x93, ActHitCountIndex);
                data.Write(0x94, Byte94);
                data.Write(0x95, Byte95);
                data.Write(0x96, Byte96);
                data.Write(0x97, Byte97);
                data.Write(0x98, Byte98);
                data.Write(0x99, Byte99);
                data.Write(0x9A, Byte9A);
                data.Write(0x9B, Byte9B);
                data.Write(0x9C, Byte9C);
                data.Write(0x9D, Byte9D);
                data.Write(0x9E, Byte9E);
                data.Write(0x9F, Byte9F);
                return data.ReadSpan();
            }

            // Contains several flags
            //   Bit 10: Target of stat changes: 0 = target, 1 = self
            public ulong Long00 { get; set; }
            public TextIDHash Text08 { get; set; }
            public TextIDHash Text0C { get; set; }

            public class Action
            {
                public Action(ActionType type)
                {
                    Type = type;
                }

                public Action(IReadOnlyBinaryDataAccessor data, int index)
                {
                    Type = (ActionType)data.ReadUInt16(0x10 + index * 2);
                    for (var i = 0; i < 8; i++)
                    {
                        Params[i] = data.ReadUInt16(0x18 + i * 2 + index * 0x10);
                        ExtraParams[i] = data.ReadByte(0x5C + i + index * 0x8);
                    }
                }

                public void WriteTo(IBinaryDataAccessor data, int index)
                {
                    data.WriteUInt16(0x10 + index * 2, (ushort)Type);
                    for (var i = 0; i < 8; i++)
                    {
                        data.WriteUInt16(0x18 + i * 2 + index * 0x10, Params[i]);
                        data.Write(0x5C + i * 2 + index * 0x8, ExtraParams[i]);
                    }
                }

                public ActionType Type { get; set; }
                public ushort[] Params { get; } = new ushort[8];
                public byte[] ExtraParams { get; } = new byte[8];
            }

            public Action[] Actions { get; } = new Action[4];

            public ushort Short58 { get; set; }
            public ushort Short5A { get; set; }
            public byte Byte7C { get; set; }
            public PokemonType MoveType { get; set; }
            public MoveCategory MoveCategory { get; set; }
            public byte Byte7F { get; set; }
            public byte Byte80 { get; set; }
            public byte Byte81 { get; set; }
            public byte Byte82 { get; set; }
            public byte Byte83 { get; set; }
            public byte Byte84 { get; set; }
            public byte Byte85 { get; set; }
            public byte Byte86 { get; set; }
            public byte Byte87 { get; set; }

            // Maximum distance for moves: 2 tiles, 4 tiles, 10 tiles, etc.
            // For charged moves, fhe first turn has a range of 0, while the second turn has the correct range.
            public byte Range { get; set; }
            public byte Byte89 { get; set; }
            public byte Byte8A { get; set; }
            public byte Byte8B { get; set; }

            public ActionTarget Target { get; set; }
            public byte Byte8D { get; set; }
            public byte Byte8E { get; set; }
            public byte Byte8F { get; set; }
            public byte Byte90 { get; set; }
            public byte Byte91 { get; set; }
            public byte Byte92 { get; set; }
            public byte ActHitCountIndex { get; set; }
            public byte Byte94 { get; set; }
            public byte Byte95 { get; set; }
            public byte Byte96 { get; set; }
            public byte Byte97 { get; set; }
            public byte Byte98 { get; set; }
            public byte Byte99 { get; set; }
            public byte Byte9A { get; set; }
            public byte Byte9B { get; set; }
            public byte Byte9C { get; set; }
            public byte Byte9D { get; set; }
            public byte Byte9E { get; set; }
            public byte Byte9F { get; set; }
        }

        public enum ActionTarget : byte
        {
            Self = 1,            // Self
            Front = 2,           // Directly in front
            FrontFan = 3,        // Fan pattern in front
            Around = 4,          // All tiles around user
            Room = 5,            // Entire room
            Floor = 6,          // Entire floor
            Unknown7 = 7,        // unknown
            RangedSingle = 8,    // Ranged (single target)
            RangedPiercing = 9,  // Ranged (piercing)
            Other = 10,          // Special cases (e.g. Spikes, Sleep Talk, Dragon Rage, Metronome, Assist)
        }
    }
}
