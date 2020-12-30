using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class DungeonMapDataInfoTests
    {
        [Fact]
        public void CanBuildDungeonMapDataInfo()
        {
            // Arrange
            var db = new DungeonMapDataInfo();
            db.Entries.Add(new DungeonMapDataInfo.Entry
            {
                Index = 0,
                DungeonBgmSymbolIndex = 30,
                FixedMapIndex = 100,
            });
            db.Entries.Add(new DungeonMapDataInfo.Entry
            {
                Index = 1,
                DungeonBgmSymbolIndex = 20,
                FixedMapIndex = 240,
            });
            
            // Act
            var bin = db.ToByteArray();

            // Assert
            var rebuiltDb = new DungeonMapDataInfo(bin);
            rebuiltDb.Entries.Count.Should().Be(2);
            rebuiltDb.Entries[0].DungeonBgmSymbolIndex.Should().Be(30);
            rebuiltDb.Entries[0].FixedMapIndex.Should().Be(100);
            rebuiltDb.Entries[1].DungeonBgmSymbolIndex.Should().Be(20);
            rebuiltDb.Entries[1].FixedMapIndex.Should().Be(240);
        }
    }
}
