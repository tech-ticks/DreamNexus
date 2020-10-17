using FluentAssertions;
using SkyEditor.RomEditor.Domain.Common.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class CodeTableTests
    {
        [Fact]
        public void CanEncodeAndDecode()
        {
            // Arrange
            var codeTable = new CodeTable();
            codeTable.AddEntry(new CodeTable.Entry
            {
                CodeString = "hero",
                UnicodeValue = 0xD100,
            });
            codeTable.AddEntry(new CodeTable.Entry
            {
                CodeString = "partner",
                UnicodeValue = 0xD200,
            });
            codeTable.AddEntry(new CodeTable.Entry
            {
                CodeString = "M:B01",
                UnicodeValue = 0xA08A,
            });
            codeTable.AddEntry(new CodeTable.Entry
            {
                CodeString = "kind_p:",
                UnicodeValue = 0xE400,
                Flags = 1 // Flag bit 0 means that a value can be encoded
            });
            string originalString = "Hello [hero] and [partner], press the [M:B01] button to evolve to [kind_p:25] and [kind_p:90]";

            // Act
            var encodedString = codeTable.UnicodeEncode(originalString);
            var decodedString = codeTable.UnicodeDecode(encodedString);

            // Assert
            char unicode(ushort value)
            {
                return (char) value;
            }
            encodedString.Should().Be($"Hello {unicode(0xD100)} and {unicode(0xD200)}, "
                + $"press the {unicode(0xA08A)} button to evolve to {unicode(0xE400 + 25)} and {unicode(0xE400 + 90)}");
            decodedString.Should().Be(originalString);
        }
    }
}
