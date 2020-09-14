using FluentAssertions;
using SkyEditor.RomEditor.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Infrastructure
{
    public class Crc32HasherTests
    {
        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { "common.bin", 0x14989DE1 };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void HashesCorrectly(string testString, int expectedHash)
        {
            var expectedHashUnsigned = BitConverter.ToUInt32(BitConverter.GetBytes(expectedHash), 0);
            Crc32Hasher.Crc32Hash(testString).Should().Be(expectedHashUnsigned);
        }
    }
}
