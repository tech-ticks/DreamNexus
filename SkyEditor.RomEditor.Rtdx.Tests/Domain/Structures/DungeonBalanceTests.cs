using FluentAssertions;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

using CreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;
using DungeonIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.dungeon.Index;

namespace SkyEditor.RomEditor.Rtdx.Tests.Domain.Structures
{
    public class DungeonBalanceTests
    {
        [Fact]
        public void CanBuildDungeonBalanceTests()
        {
            // Arrange
            var db = new DungeonBalance();

            var d001 = db.Entries[(int)DungeonIndex.D001] = new DungeonBalance.DungeonBalanceEntry(2);
            d001.FloorInfos[0].Short02 = 0x02;
            d001.FloorInfos[0].Event = "@BOSS#0";
            d001.FloorInfos[0].Short24 = 0x24;
            d001.FloorInfos[0].Short26 = 0x26;
            d001.FloorInfos[0].Short28 = 0x28;
            d001.FloorInfos[0].Short2A = 0x2A;
            d001.FloorInfos[0].Byte2C = 0x2C;
            d001.FloorInfos[0].Byte2D = 0x2D;
            d001.FloorInfos[0].Byte2E = 0x2E;
            d001.FloorInfos[0].Byte2F = 0x2F;
            d001.FloorInfos[0].Short30 = 0x30;
            d001.FloorInfos[0].Short32 = 0x32;
            d001.FloorInfos[0].Byte34 = 0x34;
            d001.FloorInfos[0].Byte35 = 0x35;
            d001.FloorInfos[0].Byte36 = 0x36;
            d001.FloorInfos[0].InvitationIndex = 0x54;

            d001.FloorInfos[1].Short02 = 0x202;
            d001.FloorInfos[1].Event = "@END";
            d001.FloorInfos[1].Short24 = 0x224;
            d001.FloorInfos[1].Short26 = 0x226;
            d001.FloorInfos[1].Short28 = 0x228;
            d001.FloorInfos[1].Short2A = 0x22A;
            d001.FloorInfos[1].Byte2C = 0x3C;
            d001.FloorInfos[1].Byte2D = 0x3D;
            d001.FloorInfos[1].Byte2E = 0x3E;
            d001.FloorInfos[1].Byte2F = 0x3F;
            d001.FloorInfos[1].Short30 = 0x230;
            d001.FloorInfos[1].Short32 = 0x232;
            d001.FloorInfos[1].Byte34 = 0x44;
            d001.FloorInfos[1].Byte35 = 0x45;
            d001.FloorInfos[1].Byte36 = 0x46;
            d001.FloorInfos[1].InvitationIndex = 0x64;

            d001.WildPokemon = new DungeonBalance.WildPokemonInfo();
            var d001Bulbasaur = d001.WildPokemon.Stats[(int)CreatureIndex.FUSHIGIDANE];
            d001Bulbasaur.Index = (int)CreatureIndex.FUSHIGIDANE;
            d001Bulbasaur.XPYield = 10;
            d001Bulbasaur.HitPoints = 33;
            d001Bulbasaur.Attack = 15;
            d001Bulbasaur.Defense = 10;
            d001Bulbasaur.SpecialAttack = 17;
            d001Bulbasaur.SpecialDefense = 15;
            d001Bulbasaur.Speed = 12;
            d001Bulbasaur.StrongFoe = 0;
            d001Bulbasaur.Level = 5;

            var d001Charmander = d001.WildPokemon.Stats[(int)CreatureIndex.HITOKAGE];
            d001Charmander.Index = (int)CreatureIndex.HITOKAGE;
            d001Charmander.XPYield = 11;
            d001Charmander.HitPoints = 28;
            d001Charmander.Attack = 13;
            d001Charmander.Defense = 12;
            d001Charmander.SpecialAttack = 18;
            d001Charmander.SpecialDefense = 14;
            d001Charmander.Speed = 12;
            d001Charmander.StrongFoe = 0;
            d001Charmander.Level = 5;

            var d001Floor1 = d001.WildPokemon.Floors[0];
            d001Floor1.Entries[(int)CreatureIndex.FUSHIGIDANE].PokemonIndex = (int)CreatureIndex.FUSHIGIDANE;
            d001Floor1.Entries[(int)CreatureIndex.FUSHIGIDANE].SpawnRate = 100;
            d001Floor1.Entries[(int)CreatureIndex.FUSHIGIDANE].RecruitmentLevel = 5;
            d001Floor1.Entries[(int)CreatureIndex.FUSHIGIDANE].Byte0B = 0;

            d001Floor1.Entries[(int)CreatureIndex.HITOKAGE].PokemonIndex = (int)CreatureIndex.HITOKAGE;
            d001Floor1.Entries[(int)CreatureIndex.HITOKAGE].SpawnRate = 100;
            d001Floor1.Entries[(int)CreatureIndex.HITOKAGE].RecruitmentLevel = 5;
            d001Floor1.Entries[(int)CreatureIndex.HITOKAGE].Byte0B = 0;

            var d001Data3 = d001.Data3 = new DungeonBalance.DungeonBalanceDataEntry3();
            d001Data3.Records[0].Entries[0].Index = 0;
            d001Data3.Records[0].Entries[0].Short02 = 0x02;
            d001Data3.Records[0].Entries[0].Int04 = 0x04;
            d001Data3.Records[1].Entries[1].Index = 1;
            d001Data3.Records[1].Entries[1].Short02 = 0x102;
            d001Data3.Records[1].Entries[1].Int04 = 0x104;

            var d001Data4 = d001.Data4 = new DungeonBalance.DungeonBalanceDataEntry4();
            d001Data4.Records[0].Entries[0].Short00 = 0x00;
            d001Data4.Records[0].Entries[0].Short02 = 0x02;
            d001Data4.Records[0].Entries[0].Int04 = 0x04;
            d001Data4.Records[1].Entries[1].Short00 = 0x100;
            d001Data4.Records[1].Entries[1].Short02 = 0x102;
            d001Data4.Records[1].Entries[1].Int04 = 0x104;

            var d002 = db.Entries[(int)DungeonIndex.D002] = new DungeonBalance.DungeonBalanceEntry(1);
            d002.FloorInfos[0].Short02 = 0x302;
            d002.FloorInfos[0].Event = "@END";
            d002.FloorInfos[0].Short24 = 0x324;
            d002.FloorInfos[0].Short26 = 0x326;
            d002.FloorInfos[0].Short28 = 0x328;
            d002.FloorInfos[0].Short2A = 0x32A;
            d002.FloorInfos[0].Byte2C = 0x4C;
            d002.FloorInfos[0].Byte2D = 0x4D;
            d002.FloorInfos[0].Byte2E = 0x4E;
            d002.FloorInfos[0].Byte2F = 0x4F;
            d002.FloorInfos[0].Short30 = 0x330;
            d002.FloorInfos[0].Short32 = 0x332;
            d002.FloorInfos[0].Byte34 = 0x54;
            d002.FloorInfos[0].Byte35 = 0x55;
            d002.FloorInfos[0].Byte36 = 0x56;
            d002.FloorInfos[0].InvitationIndex = 0x74;

            // Act
            var (bin, ent) = db.Build();

            // Assert
            var rebuiltDb = new DungeonBalance(bin, ent);

            var rebuiltD001 = rebuiltDb.Entries[(int)DungeonIndex.D001];
            rebuiltD001.FloorInfos.Should().HaveCount(2);
            rebuiltD001.FloorInfos[0].Short02.Should().Be(0x02);
            rebuiltD001.FloorInfos[0].Event.Should().Be("@BOSS#0");
            rebuiltD001.FloorInfos[0].Short24.Should().Be(0x24);
            rebuiltD001.FloorInfos[0].Short26.Should().Be(0x26);
            rebuiltD001.FloorInfos[0].Short28.Should().Be(0x28);
            rebuiltD001.FloorInfos[0].Short2A.Should().Be(0x2A);
            rebuiltD001.FloorInfos[0].Byte2C.Should().Be(0x2C);
            rebuiltD001.FloorInfos[0].Byte2D.Should().Be(0x2D);
            rebuiltD001.FloorInfos[0].Byte2E.Should().Be(0x2E);
            rebuiltD001.FloorInfos[0].Byte2F.Should().Be(0x2F);
            rebuiltD001.FloorInfos[0].Short30.Should().Be(0x30);
            rebuiltD001.FloorInfos[0].Short32.Should().Be(0x32);
            rebuiltD001.FloorInfos[0].Byte34.Should().Be(0x34);
            rebuiltD001.FloorInfos[0].Byte35.Should().Be(0x35);
            rebuiltD001.FloorInfos[0].Byte36.Should().Be(0x36);
            rebuiltD001.FloorInfos[0].InvitationIndex.Should().Be(0x54);

            rebuiltD001.WildPokemon.Should().NotBeNull();
            var rebuiltD001Bulbasaur = rebuiltD001.WildPokemon.Stats[(int)CreatureIndex.FUSHIGIDANE];
            rebuiltD001Bulbasaur.Index.Should().Be((int)CreatureIndex.FUSHIGIDANE);
            rebuiltD001Bulbasaur.XPYield.Should().Be(10);
            rebuiltD001Bulbasaur.HitPoints.Should().Be(33);
            rebuiltD001Bulbasaur.Attack.Should().Be(15);
            rebuiltD001Bulbasaur.Defense.Should().Be(10);
            rebuiltD001Bulbasaur.SpecialAttack.Should().Be(17);
            rebuiltD001Bulbasaur.SpecialDefense.Should().Be(15);
            rebuiltD001Bulbasaur.Speed.Should().Be(12);
            rebuiltD001Bulbasaur.StrongFoe.Should().Be(0);
            rebuiltD001Bulbasaur.Level.Should().Be(5);

            var rebuiltD001Charmander = rebuiltD001.WildPokemon.Stats[(int)CreatureIndex.HITOKAGE];
            rebuiltD001Charmander.Index.Should().Be((int)CreatureIndex.HITOKAGE);
            rebuiltD001Charmander.XPYield.Should().Be(11);
            rebuiltD001Charmander.HitPoints.Should().Be(28);
            rebuiltD001Charmander.Attack.Should().Be(13);
            rebuiltD001Charmander.Defense.Should().Be(12);
            rebuiltD001Charmander.SpecialAttack.Should().Be(18);
            rebuiltD001Charmander.SpecialDefense.Should().Be(14);
            rebuiltD001Charmander.Speed.Should().Be(12);
            rebuiltD001Charmander.StrongFoe.Should().Be(0);
            rebuiltD001Charmander.Level.Should().Be(5);

            var rebuiltD001Floor1 = rebuiltD001.WildPokemon.Floors[0];
            rebuiltD001Floor1.Entries[(int)CreatureIndex.FUSHIGIDANE].PokemonIndex.Should().Be((int)CreatureIndex.FUSHIGIDANE);
            rebuiltD001Floor1.Entries[(int)CreatureIndex.FUSHIGIDANE].SpawnRate.Should().Be(100);
            rebuiltD001Floor1.Entries[(int)CreatureIndex.FUSHIGIDANE].RecruitmentLevel.Should().Be(5);
            rebuiltD001Floor1.Entries[(int)CreatureIndex.FUSHIGIDANE].Byte0B.Should().Be(0);

            rebuiltD001Floor1.Entries[(int)CreatureIndex.HITOKAGE].PokemonIndex.Should().Be((int)CreatureIndex.HITOKAGE);
            rebuiltD001Floor1.Entries[(int)CreatureIndex.HITOKAGE].SpawnRate.Should().Be(100);
            rebuiltD001Floor1.Entries[(int)CreatureIndex.HITOKAGE].RecruitmentLevel.Should().Be(5);
            rebuiltD001Floor1.Entries[(int)CreatureIndex.HITOKAGE].Byte0B.Should().Be(0);

            var rebuiltD001Data3 = rebuiltD001.Data3;
            rebuiltD001Data3.Should().NotBeNull();
            rebuiltD001Data3.Records[0].Entries[0].Index.Should().Be(0);
            rebuiltD001Data3.Records[0].Entries[0].Short02.Should().Be(0x02);
            rebuiltD001Data3.Records[0].Entries[0].Int04.Should().Be(0x04);
            rebuiltD001Data3.Records[1].Entries[1].Index.Should().Be(1);
            rebuiltD001Data3.Records[1].Entries[1].Short02.Should().Be(0x102);
            rebuiltD001Data3.Records[1].Entries[1].Int04.Should().Be(0x104);

            var rebuiltD001Data4 = rebuiltD001.Data4;
            rebuiltD001Data3.Should().NotBeNull();
            rebuiltD001Data4.Records[0].Entries[0].Short00.Should().Be(0x00);
            rebuiltD001Data4.Records[0].Entries[0].Short02.Should().Be(0x02);
            rebuiltD001Data4.Records[0].Entries[0].Int04.Should().Be(0x04);
            rebuiltD001Data4.Records[1].Entries[1].Short00.Should().Be(0x100);
            rebuiltD001Data4.Records[1].Entries[1].Short02.Should().Be(0x102);
            rebuiltD001Data4.Records[1].Entries[1].Int04.Should().Be(0x104);

            var rebuiltD002 = rebuiltDb.Entries[(int)DungeonIndex.D002];
            rebuiltD002.FloorInfos.Should().HaveCount(1);
            rebuiltD002.FloorInfos[0].Short02.Should().Be(0x302);
            rebuiltD002.FloorInfos[0].Event.Should().Be("@END");
            rebuiltD002.FloorInfos[0].Short24.Should().Be(0x324);
            rebuiltD002.FloorInfos[0].Short26.Should().Be(0x326);
            rebuiltD002.FloorInfos[0].Short28.Should().Be(0x328);
            rebuiltD002.FloorInfos[0].Short2A.Should().Be(0x32A);
            rebuiltD002.FloorInfos[0].Byte2C.Should().Be(0x4C);
            rebuiltD002.FloorInfos[0].Byte2D.Should().Be(0x4D);
            rebuiltD002.FloorInfos[0].Byte2E.Should().Be(0x4E);
            rebuiltD002.FloorInfos[0].Byte2F.Should().Be(0x4F);
            rebuiltD002.FloorInfos[0].Short30.Should().Be(0x330);
            rebuiltD002.FloorInfos[0].Short32.Should().Be(0x332);
            rebuiltD002.FloorInfos[0].Byte34.Should().Be(0x54);
            rebuiltD002.FloorInfos[0].Byte35.Should().Be(0x55);
            rebuiltD002.FloorInfos[0].Byte36.Should().Be(0x56);
            rebuiltD002.FloorInfos[0].InvitationIndex.Should().Be(0x74);
        }
    }
}
