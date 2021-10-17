using FluentAssertions;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class ActDataInfoTests
    {
        [Fact]
        public void CanRebuildBytePerfectEntry()
        {
            // Arrange
            var data = new byte[0xA0];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)i;
            }
            var entry = new ActDataInfo.Entry(new BinaryFile(data));

            // Act
            var bin = entry.ToBytes().ToArray();

            // Assert
            bin.Should().BeEquivalentTo(data);
        }
    }
}
