using FluentAssertions;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Common.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class Gyu0Tests
    {
        [Fact]
        public void CanCompressData()
        {
            // Arrange
            var testData = new byte[] { 55, 66, 55, 66, 77, 88, 0, 11, 0, 22, 0, 33, 0, 22, 0, 11, 0, 0, 0, 0, 0, 0, 0, 44, 44, 44, 44, 44, 44, 44, 55, 66, 77, 88, 99 };

            // Act
            var compressed = Gyu0.Compress(new BinaryFile(testData));
            var decompressed = Gyu0.Decompress(compressed);

            // Assert
            var decompressedArray = decompressed.ReadArray();
            decompressedArray.Should().Equal(testData);
        }
    }
}
