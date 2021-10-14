using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;
using System.Text;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class MessageBinTests
    {
        [Fact]
        public void CanBuildMessageBin()
        {
            // Arrange
            var messageBin = new MessageBinEntry();
            messageBin.AddString("first", Encoding.Unicode.GetBytes("Grookey"));
            messageBin.AddString("second", Encoding.Unicode.GetBytes("Scorbunny"));
            messageBin.AddString("third", Encoding.Unicode.GetBytes("Sobble"));

            // Act
            var data = messageBin.ToByteArray();

            // Assert
            var rebuiltDb = new MessageBinEntry(data);
            rebuiltDb.GetStringByHash((int) Crc32Hasher.Crc32Hash("first")).Should().Be("Grookey");
            rebuiltDb.GetStringByHash((int) Crc32Hasher.Crc32Hash("second")).Should().Be("Scorbunny");
            rebuiltDb.GetStringByHash((int) Crc32Hasher.Crc32Hash("third")).Should().Be("Sobble");
        }
        
    }
}
