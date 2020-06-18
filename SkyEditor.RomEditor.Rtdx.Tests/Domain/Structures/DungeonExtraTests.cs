using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class DungeonExtraTests
    {
        [Fact]
        public void CanBuildDungeonExtraTests()
        {
            // Arrange
            var db = new DungeonExtra();
          
            // Add elements in random order intentionally, as they should be built in ascending order in the file
            db.Entries[DungeonIndex.D010] = new DungeonExtra.Entry(DungeonIndex.D010)
            {
                Floors = 10,
                DungeonEvents = new DungeonExtra.Entry.DungeonEvent[] {
                    new DungeonExtra.Entry.DungeonEvent
                    {
                        Floor = 10,
                        Name = "@BOSS#0"
                    },
                    new DungeonExtra.Entry.DungeonEvent
                    {
                        Floor = 11,
                        Name = "@END"
                    },
                }
            };
            db.Entries[DungeonIndex.D003] = new DungeonExtra.Entry(DungeonIndex.D003)
            {
                Floors = 5,
                DungeonEvents = new DungeonExtra.Entry.DungeonEvent[] {
                    new DungeonExtra.Entry.DungeonEvent
                    {
                        Floor = 6,
                        Name = "@END"
                    },
                }
            };

            // Act
            var data = db.ToByteArray();

            // Assert
            var rebuiltDb = new DungeonExtra(data);

            // Check modified entries
            rebuiltDb.Entries[DungeonIndex.D003].Index.Should().Be(DungeonIndex.D003);
            rebuiltDb.Entries[DungeonIndex.D003].Floors.Should().Be(5);
            rebuiltDb.Entries[DungeonIndex.D003].DungeonEvents.Length.Should().Be(1);
            rebuiltDb.Entries[DungeonIndex.D003].DungeonEvents[0].Floor.Should().Be(6);
            rebuiltDb.Entries[DungeonIndex.D003].DungeonEvents[0].Name.Should().Be("@END");

            rebuiltDb.Entries[DungeonIndex.D010].Index.Should().Be(DungeonIndex.D010);
            rebuiltDb.Entries[DungeonIndex.D010].Floors.Should().Be(10);
            rebuiltDb.Entries[DungeonIndex.D010].DungeonEvents.Length.Should().Be(2);
            rebuiltDb.Entries[DungeonIndex.D010].DungeonEvents[0].Floor.Should().Be(10);
            rebuiltDb.Entries[DungeonIndex.D010].DungeonEvents[0].Name.Should().Be("@BOSS#0");
            rebuiltDb.Entries[DungeonIndex.D010].DungeonEvents[1].Floor.Should().Be(11);
            rebuiltDb.Entries[DungeonIndex.D010].DungeonEvents[1].Name.Should().Be("@END");

            // Check unmodified entries
            rebuiltDb.Entries[DungeonIndex.D001].Index.Should().Be(DungeonIndex.D001);
            rebuiltDb.Entries[DungeonIndex.D001].Floors.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D001].DungeonEvents.Should().BeEmpty();

            rebuiltDb.Entries[DungeonIndex.D100].Index.Should().Be(DungeonIndex.D100);
            rebuiltDb.Entries[DungeonIndex.D100].Floors.Should().Be(0);
            rebuiltDb.Entries[DungeonIndex.D100].DungeonEvents.Should().BeEmpty();
        }
    }
}
