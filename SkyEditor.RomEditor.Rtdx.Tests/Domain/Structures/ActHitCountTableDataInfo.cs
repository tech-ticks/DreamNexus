using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class ActHitCountTableDataInfoTests
    {
        [Fact]
        public void CanBuildActHitCountTableDataInfoTests()
        {
            // Arrange
            var table = new ActHitCountTableDataInfo();

            var entry = new ActHitCountTableDataInfo.Entry(0);
            table.Entries.Add(entry);
            entry.StopOnMiss = 1;
            entry.MinHits = 2;
            entry.MaxHits = 5;
            entry.Weights[0] = 50;
            entry.Weights[1] = 25;
            entry.Weights[2] = 15;
            entry.Weights[3] = 10;

            // Act
            var bin = table.Build();

            // Assert
            var rebuiltTable = new ActHitCountTableDataInfo(bin);

            var rebuiltEntry = rebuiltTable.Entries[0];
            rebuiltEntry.StopOnMiss.Should().Be(1);
            rebuiltEntry.MinHits.Should().Be(2);
            rebuiltEntry.MaxHits.Should().Be(5);
            rebuiltEntry.Weights[0].Should().Be(50);
            rebuiltEntry.Weights[1].Should().Be(25);
            rebuiltEntry.Weights[2].Should().Be(15);
            rebuiltEntry.Weights[3].Should().Be(10);
        }
    }
}
