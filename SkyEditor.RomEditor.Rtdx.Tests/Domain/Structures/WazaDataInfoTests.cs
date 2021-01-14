using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class WazaDataInfoTests
    {
        [Fact]
        public void CanBuildWazaDataInfo()
        {
            // Arrange
            var dataInfo = new WazaDataInfo();
            var entry = new WazaDataInfo.Entry();
            dataInfo.Entries.Add(entry);
            entry.Short00 = 1;
            entry.Short02 = 2;
            entry.Short04 = 3;
            entry.Short0E = 4;
            entry.Byte10 = 5;
            entry.Byte11 = 6;
            entry.ActIndex = 7;

            // Act
            var data = dataInfo.ToByteArray();
            var rebuiltDataInfo = new WazaDataInfo(data);

            // Assert
            rebuiltDataInfo.Entries.Should().HaveCount(1);
            rebuiltDataInfo.Entries[0].Short00.Should().Be(1);
            rebuiltDataInfo.Entries[0].Short02.Should().Be(2);
            rebuiltDataInfo.Entries[0].Short04.Should().Be(3);
            rebuiltDataInfo.Entries[0].Short0E.Should().Be(4);
            rebuiltDataInfo.Entries[0].Byte10.Should().Be(5);
            rebuiltDataInfo.Entries[0].Byte11.Should().Be(6);
            rebuiltDataInfo.Entries[0].ActIndex.Should().Be(7);
        }
    }
}
