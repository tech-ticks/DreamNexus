using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using System;
using System.Collections.Generic;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain
{
    public class FarcTests
    {
        public class HashingTests
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
                Farc.PmdHashing.Crc32Hash(testString).Should().Be(expectedHashUnsigned);
            }
        }
    }
}
