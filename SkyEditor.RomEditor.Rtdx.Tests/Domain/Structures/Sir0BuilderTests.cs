using FluentAssertions;
using SkyEditor.IO;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SkyEditor.RomEditor.Rtdx.Tests.Domain.Structures
{
    public class Sir0BuilderTests
    {
        [Fact]
        public void CanBuildSir0()
        {
            // Arrange
            var builder = new Sir0Builder();
            var testData = new int[] { 1, 2, 3, 7, 11, 13 };

            // Act
            var pointers = new List<long>();
            foreach (var value in testData)
            {
                pointers.Add(builder.Length);
                builder.WriteInt32(builder.Length, value);
            }

            builder.SubHeaderOffset = builder.Length;
            builder.WriteInt32(builder.Length, pointers.Count);
            foreach (var pointer in pointers)
            {
                builder.WritePointer(builder.Length, pointer);
            }

            var sir0 = builder.Build();

            // Assert
            sir0.Data.Length.Should().BeGreaterThan(
                0x20 // Header
                + (testData.Length * 4) // The integers from testData
                + (testData.Length * 8) // Pointers to each integer
                + (testData.Length)); // Minimum footer size. Testing the size of the footer is outside the scope of this test, but is covered by checking each pointer later

            sir0.PointerOffsets[0].Should().Be(8); // Index of the subheader pointer
            sir0.PointerOffsets[1].Should().Be(16); // Index of the footer pointer

            var entryCount = sir0.SubHeader.ReadInt32(0);
            entryCount.Should().Be(testData.Length);
            for (int i = 0; i < entryCount; i++)
            {
                var entryPointerOffset = sir0.SubHeaderOffset + 4 + i * 8;
                sir0.PointerOffsets[i + 2].Should().Be(entryPointerOffset);

                var entryPointer = sir0.SubHeader.ReadInt64(4 + i * 8);
                var entryPointerFromAbsolute = sir0.Data.ReadInt64(sir0.SubHeaderOffset + 4 + i * 8);
                entryPointerFromAbsolute.Should().Be(entryPointer);

                var entry = sir0.Data.ReadInt32(entryPointer);
                entry.Should().Be(testData[i]);
            }
        }
    }
}
