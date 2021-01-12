using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using Xunit;
using ConnectionFlags = SkyEditor.RomEditor.Domain.Rtdx.Structures.RandomParts.RandomPartsEntry.Entry.ConnectionFlags;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class RandomPartsTests
    {
        [Fact]
        public void CanBuildRandomPartsTests()
        {
            // Arrange
            var rp = new RandomParts();

            var entry = new RandomParts.RandomPartsEntry(0, 9, 9);
            rp.Entries.Add(entry);

            var subentry = new RandomParts.RandomPartsEntry.Entry(0, 9, 9);
            entry.Entries.Add(subentry);
            subentry.Connections = ConnectionFlags.East | ConnectionFlags.West;
            for (var y = 0; y < 9; y++)
            {
                for (var x = 0; x < 9; x++)
                {
                    subentry.Tiles[x, y].Byte0 = (byte)(x | (y << 4));
                    subentry.Tiles[x, y].Byte1 = (byte)~(x | (y << 4));
                }
            }

            // Act
            var (bin, ent) = rp.Build();

            // Assert
            var rebuiltRp = new RandomParts(bin, ent);

            var rebuiltEntry = rebuiltRp.Entries[0];
            rebuiltEntry.Width.Should().Be(9);
            rebuiltEntry.Height.Should().Be(9);
            rebuiltEntry.Entries.Should().HaveCount(1);

            var rebuiltSubentry = rebuiltEntry.Entries[0];
            rebuiltSubentry.Connections.Should().Be(ConnectionFlags.East | ConnectionFlags.West);
            for (var y = 0; y < 9; y++)
            {
                for (var x = 0; x < 9; x++)
                {
                    rebuiltSubentry.Tiles[x, y].Byte0.Should().Be((byte)(x | (y << 4)));
                    rebuiltSubentry.Tiles[x, y].Byte1.Should().Be((byte)~(x | (y << 4)));
                }
            }
        }
    }
}
