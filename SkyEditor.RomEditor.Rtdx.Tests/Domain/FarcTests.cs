using FluentAssertions;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SkyEditor.RomEditor.Rtdx.Tests.Domain
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
