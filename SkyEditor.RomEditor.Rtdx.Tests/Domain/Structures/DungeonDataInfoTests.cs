using FluentAssertions;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using DungeonIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.dungeon.Index;

namespace SkyEditor.RomEditor.Rtdx.Tests.Domain.Structures
{
    public class DungeonDataInfoTests
    {
        [Fact]
        public void CanBuildDungeonDataInfoTests()
        {
            // -- Arrange
            var db = new DungeonDataInfo();
            
            // Add elements in random order intentionally, as they should be built in ascending order in the file
            db.Entries[DungeonIndex.D022] = new DungeonDataInfo.DungeonDataInfoEntry
            {
                Features = 0,
                Index = 2,
                Short08 = 0x28,
                Short0A = 0x2A,
                SortKey = 2,
                DungeonBalanceIndex = 1,
                Byte13 = 0x23,
                MaxItems = 0,
                MaxTeammates = 1,
                Byte17 = 0x27,
                Byte18 = 0x28,
                Byte19 = 0x29
            };
            db.Entries[DungeonIndex.D001] = new DungeonDataInfo.DungeonDataInfoEntry
            {
                Features = DungeonDataInfo.DungeonDataInfoEntry.Feature.Radar | DungeonDataInfo.DungeonDataInfoEntry.Feature.Scanning,
                Index = 1,
                Short08 = 0x08,
                Short0A = 0x0A,
                SortKey = 1,
                DungeonBalanceIndex = 2,
                Byte13 = 0x13,
                MaxItems = 64,
                MaxTeammates = 3,
                Byte17 = 0x17,
                Byte18 = 0x18,
                Byte19 = 0x19
            };

            // -- Act
            var data = db.ToByteArray();

            // -- Assert
            data.Length.Should().Be((int)DungeonIndex.END * DungeonDataInfo.EntrySize);

            var rebuiltDb = new DungeonDataInfo(data);
            
            // Check modified entries
            rebuiltDb.Entries[DungeonIndex.D001].Features.Should().Be(DungeonDataInfo.DungeonDataInfoEntry.Feature.Radar | DungeonDataInfo.DungeonDataInfoEntry.Feature.Scanning);
            rebuiltDb.Entries[DungeonIndex.D001].Index.Should().Be(1);
            rebuiltDb.Entries[DungeonIndex.D001].Short08.Should().Be(0x08);
            rebuiltDb.Entries[DungeonIndex.D001].Short0A.Should().Be(0x0A);
            rebuiltDb.Entries[DungeonIndex.D001].SortKey.Should().Be(1);
            rebuiltDb.Entries[DungeonIndex.D001].DungeonBalanceIndex.Should().Be(2);
            rebuiltDb.Entries[DungeonIndex.D001].Byte13.Should().Be(0x13);
            rebuiltDb.Entries[DungeonIndex.D001].MaxItems.Should().Be(64);
            rebuiltDb.Entries[DungeonIndex.D001].MaxTeammates.Should().Be(3);
            rebuiltDb.Entries[DungeonIndex.D001].Byte17.Should().Be(0x17);
            rebuiltDb.Entries[DungeonIndex.D001].Byte18.Should().Be(0x18);
            rebuiltDb.Entries[DungeonIndex.D001].Byte19.Should().Be(0x19);

            rebuiltDb.Entries[DungeonIndex.D022].Features.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D022].Index.Should().Be(2);
            rebuiltDb.Entries[DungeonIndex.D022].Short08.Should().Be(0x28);
            rebuiltDb.Entries[DungeonIndex.D022].Short0A.Should().Be(0x2A);
            rebuiltDb.Entries[DungeonIndex.D022].SortKey.Should().Be(2);
            rebuiltDb.Entries[DungeonIndex.D022].DungeonBalanceIndex.Should().Be(1);
            rebuiltDb.Entries[DungeonIndex.D022].Byte13.Should().Be(0x23);
            rebuiltDb.Entries[DungeonIndex.D022].MaxItems.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D022].MaxTeammates.Should().Be(1);
            rebuiltDb.Entries[DungeonIndex.D022].Byte17.Should().Be(0x27);
            rebuiltDb.Entries[DungeonIndex.D022].Byte18.Should().Be(0x28);
            rebuiltDb.Entries[DungeonIndex.D022].Byte19.Should().Be(0x29);

            // Check unmodified entries
            rebuiltDb.Entries[DungeonIndex.NONE].Features.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.NONE].Index.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.NONE].Short08.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.NONE].Short0A.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.NONE].SortKey.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.NONE].DungeonBalanceIndex.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.NONE].Byte13.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.NONE].MaxItems.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.NONE].MaxTeammates.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.NONE].Byte17.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.NONE].Byte18.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.NONE].Byte19.Should().Be(0);

            rebuiltDb.Entries[DungeonIndex.D002].Features.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D002].Index.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D002].Short08.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D002].Short0A.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D002].SortKey.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D002].DungeonBalanceIndex.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D002].Byte13.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D002].MaxItems.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D002].MaxTeammates.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D002].Byte17.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D002].Byte18.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D002].Byte19.Should().Be(0);
        }
    }
}
