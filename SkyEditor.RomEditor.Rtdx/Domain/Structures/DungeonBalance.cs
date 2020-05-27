using SkyEditor.IO;
using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using CreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;
using DungeonIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.dungeon.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public class DungeonBalance
    {
        public DungeonBalance(byte[] binData, byte[] entData)
        {
            IReadOnlyBinaryDataAccessor binFile = new BinaryFile(binData);
            IReadOnlyBinaryDataAccessor entFile = new BinaryFile(entData);

            var entCount = entFile.Length / sizeof(uint) - 1;
            Entries = new DungeonBalanceEntry[entCount];
            for (var i = 0; i < entCount; i++)
            {
                var curr = entFile.ReadInt32(i * sizeof(int));
                var next = entFile.ReadInt32((i + 1) * sizeof(int));
                Entries[i] = new DungeonBalanceEntry(binFile.Slice(curr, next - curr));
            }
        }

        public DungeonBalanceEntry[] Entries { get; }

        public class DungeonBalanceEntry
        {
            public DungeonBalanceEntry(IReadOnlyBinaryDataAccessor accessor)
            {
                var buffer = Gyu0.Decompress(accessor);

                // FIXME: this is much slower and triggers GC frequently
                /*Gyu0Stream gyu0Stream = new Gyu0Stream(accessor);
                var buffer = new byte[gyu0Stream.Length];
                int lenRead = gyu0Stream.Read(buffer);
                if (lenRead != gyu0Stream.Length)
                    throw new InvalidDataException();*/

                Sir0 sir0 = new Sir0(buffer);
                var offsetHeader = sir0.SubHeader.ReadInt64(0x00);
                var offset2 = sir0.SubHeader.ReadInt64(0x08);
                var offset3 = sir0.SubHeader.ReadInt64(0x10);
                var offset4 = sir0.SubHeader.ReadInt64(0x18);
                var lenHeader = offset2 - offsetHeader;
                var len2 = offset3 - offset2;
                var len3 = offset4 - offset3;
                var len4 = sir0.SubHeaderOffset - offset4;

                var headerEntrySize = FloorInfoEntry.Size;
                var entryCount = lenHeader / headerEntrySize;
                FloorInfos = new FloorInfoEntry[entryCount];
                for (int i = 0; i < lenHeader / headerEntrySize; i++)
                {
                    FloorInfos[i] = new FloorInfoEntry(sir0.Data.Slice(offsetHeader + i * headerEntrySize, headerEntrySize));
                }

                if (len2 > 0) WildPokemon = new WildPokemonInfo(sir0.Data.Slice(offset2, len2));
                if (len3 > 0) Data3 = new DungeonBalanceDataEntry3(sir0.Data.Slice(offset3, len3));
                if (len4 > 0) Data4 = new DungeonBalanceDataEntry4(sir0.Data.Slice(offset4, len4));
            }

            public FloorInfoEntry[] FloorInfos { get; }
            public WildPokemonInfo? WildPokemon { get; }
            public DungeonBalanceDataEntry3? Data3 { get; }
            public DungeonBalanceDataEntry4? Data4 { get; }
        }

        [DebuggerDisplay("{Index} : {Event}|{Short02}")]
        public class FloorInfoEntry
        {
            internal const int Size = 98;

            public FloorInfoEntry(IReadOnlyBinaryDataAccessor data)
            {
                Index = data.ReadInt16(0x00);
                Short02 = data.ReadInt16(0x02);
                Event = data.ReadString(0x04, 32, Encoding.ASCII).Trim('\0');
                Short24 = data.ReadInt16(0x24);
                Short26 = data.ReadInt16(0x26);
                Short28 = data.ReadInt16(0x28);
                Short2A = data.ReadInt16(0x2A);
                Byte2C = data.ReadByte(0x2C);
                Byte2D = data.ReadByte(0x2D);
                Byte2E = data.ReadByte(0x2E);
                Byte2F = data.ReadByte(0x2F);
                Short30 = data.ReadInt16(0x30);
                Short32 = data.ReadInt16(0x32);
                Byte34 = data.ReadByte(0x34);
                Byte35 = data.ReadByte(0x35);
                Byte36 = data.ReadByte(0x36);
                InvitationIndex = data.ReadByte(0x54);
                Bytes37to53 = data.ReadArray(0x37, 0x53 - 0x37 + 1);
                Bytes55to61 = data.ReadArray(0x55, 0x61 - 0x55 + 1);
            }

            public short Index { get; }
            public short Short02 { get; }
            public string Event { get; }
            public short Short24 { get; }   // possibly the max number of turns?
            public short Short26 { get; }
            public short Short28 { get; }
            public short Short2A { get; }
            public byte Byte2C { get; }
            public byte Byte2D { get; }
            public byte Byte2E { get; }
            public byte Byte2F { get; }
            public short Short30 { get; }
            public short Short32 { get; }
            public byte Byte34 { get; }
            public byte Byte35 { get; }
            public byte Byte36 { get; }
            public byte InvitationIndex { get; }
            public byte[] Bytes37to53 { get; }
            public byte[] Bytes55to61 { get; }
            public string Bytes37to53AsString => string.Join(",", Bytes37to53);
            public string Bytes55to61AsString => string.Join(",", Bytes55to61);
        }

        public class WildPokemonInfo
        {
            public WildPokemonInfo(IReadOnlyBinaryDataAccessor accessor)
            {
                var sir0 = new Sir0(accessor);

                int pokemonStatsCount = sir0.SubHeader.ReadInt32(0x00);
                int pokemonStatsOffset = sir0.SubHeader.ReadInt32(0x08);
                Stats = new StatsEntry[pokemonStatsCount];
                for (int i = 0; i < pokemonStatsCount; i++)
                {
                    var offset = sir0.Data.ReadInt64(pokemonStatsOffset + i * sizeof(long));
                    Stats[i] = new StatsEntry(i, sir0.Data.Slice(offset, 16));
                }

                int count2 = sir0.SubHeader.ReadInt32(0x10);
                Floors = new FloorInfo[count2];
                for (int i = 0; i < count2; i++)
                {
                    Floors[i] = new FloorInfo(pokemonStatsCount);
                    var offset = sir0.SubHeader.ReadInt64(0x18 + i * sizeof(long));
                    for (int j = 0; j < pokemonStatsCount; j++)
                    {
                        Floors[i].Entries[j] = new FloorInfo.Entry(sir0.Data.Slice(offset + j * 16, 16));
                    }
                }
            }

            [DebuggerDisplay("{Index} : {XPYield}|{HitPoints}|{Attack}|{Defense}|{SpecialAttack}|{SpecialDefense}|{Speed}|{Level}")]
            public struct StatsEntry
            {
                public StatsEntry(int index, IReadOnlyBinaryDataAccessor accessor)
                {
                    Index = index;
                    CreatureIndex = (CreatureIndex)(index + 1);
                    XPYield = accessor.ReadInt32(0x00);
                    HitPoints = accessor.ReadInt16(0x04);
                    Attack = accessor.ReadByte(0x06);
                    Defense = accessor.ReadByte(0x07);
                    SpecialAttack = accessor.ReadByte(0x08);
                    SpecialDefense = accessor.ReadByte(0x09);
                    Speed = accessor.ReadByte(0x0A);
                    StrongFoe = accessor.ReadByte(0x0B);
                    Level = accessor.ReadByte(0x0C);
                }

                public int Index { get; }
                public CreatureIndex CreatureIndex { get; }
                public int XPYield { get; }
                public short HitPoints { get; }
                public byte Attack { get; }
                public byte Defense { get; }
                public byte SpecialAttack { get; }
                public byte SpecialDefense { get; }
                public byte Speed { get; }
                public byte StrongFoe { get; }
                public byte Level { get; }
            }

            public struct FloorInfo
            {
                public FloorInfo(int entryCount)
                {
                    Entries = new Entry[entryCount];
                }

                [DebuggerDisplay("{PokemonIndex} : {SpawnRate}|{RecruitmentLevel}|{Byte0B}")]
                public struct Entry
                {
                    public Entry(IReadOnlyBinaryDataAccessor accessor)
                    {
                        PokemonIndex = accessor.ReadInt16(0x00);
                        SpawnRate = accessor.ReadByte(0x02);
                        RecruitmentLevel = accessor.ReadByte(0x0A);
                        Byte0B = accessor.ReadByte(0x0B);
                    }

                    public short PokemonIndex { get; }
                    public byte SpawnRate { get; }
                    public byte RecruitmentLevel { get; }
                    public byte Byte0B { get; }
                }

                public Entry[] Entries { get; }
            }

            public StatsEntry[] Stats { get; } 
            public FloorInfo[] Floors { get; }
        }

        public class DungeonBalanceDataEntry3
        {
            public DungeonBalanceDataEntry3(IReadOnlyBinaryDataAccessor accessor)
            {
                var sir0 = new Sir0(accessor);
                int count = sir0.SubHeader.ReadInt32(0x00);
                Records = new Record[count];
                for (int i = 0; i < count; i++)
                {
                    Records[i] = new Record();
                    var offset = sir0.SubHeader.ReadInt64(0x08 + i * sizeof(long));
                    for (int j = 0; j < Records[i].Entries.Length; j++)
                    {
                        Records[i].Entries[j] = new Record.Entry(sir0.Data.Slice(offset + j * 8, 8));
                    }
                }
            }

            public class Record
            {
                public Record()
                {
                    Entries = new Entry[32];
                }

                [DebuggerDisplay("{Index} : {Short02}|{Int04}")]
                public struct Entry
                {
                    public Entry(IReadOnlyBinaryDataAccessor accessor)
                    {
                        Index = accessor.ReadInt16(0x00);
                        Short02 = accessor.ReadInt16(0x02);
                        Int04 = accessor.ReadInt16(0x04);
                    }

                    public short Index { get; }
                    public short Short02 { get; }
                    public int Int04 { get; }  // all 0s
                }

                public Entry[] Entries { get; }
            }

            public Record[] Records { get; }
        }

        public class DungeonBalanceDataEntry4
        {
            public DungeonBalanceDataEntry4(IReadOnlyBinaryDataAccessor accessor)
            {
                var sir0 = new Sir0(accessor);
                int count = sir0.SubHeader.ReadInt32(0x00);
                Records = new Record[count];
                for (int i = 0; i < count; i++)
                {
                    Records[i] = new Record();
                    var offset = sir0.SubHeader.ReadInt64(0x08 + i * sizeof(long));
                    for (int j = 0; j < Records[i].Entries.Length; j++)
                    {
                        Records[i].Entries[j] = new Record.Entry(sir0.Data.Slice(offset + j * 8, 8));
                    }
                }
            }

            public class Record
            {
                public Record()
                {
                    Entries = new Entry[45];
                }

                [DebuggerDisplay("{Short00}|{Short02}|{Int04}")]
                public struct Entry
                {
                    public Entry(IReadOnlyBinaryDataAccessor accessor)
                    {
                        Short00 = accessor.ReadInt16(0x00);
                        Short02 = accessor.ReadInt16(0x02);
                        Int04 = accessor.ReadInt16(0x04);
                    }

                    public short Short00 { get; }  // 0 through 45, skipping 13
                    public short Short02 { get; }  // all 60s
                    public int Int04 { get; }      // all 0s
                }

                public Entry[] Entries { get; }
            }

            public Record[] Records { get; }
        }
    }
}
