using FluentAssertions;
using SkyEditor.IO;
using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SkyEditor.RomEditor.Rtdx.Tests.Domain.Structures
{
    public class Gyu0Tests
    {
        [Fact]
        public void CanCompressData()
        {
            // Arrange
            var testData = new byte[] { 0, 11, 0, 22, 0, 33, 0, 0, 0, 0, 44, 44, 44, 44, 55, 66, 77 };

            // Act
            var compressed = Gyu0.Compress(new BinaryFile(testData));
            var decompressed = Gyu0.Decompress(compressed);

            // Assert
            var decompressedArray = decompressed.ReadArray();
            decompressedArray.Should().Equal(testData);
        }
    }
}
