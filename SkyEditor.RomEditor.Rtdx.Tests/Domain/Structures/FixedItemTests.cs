using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class FixedItemTests
    {
        [Fact]
        public void CanBuildFixedItem()
        {
            // Arrange
            var fixedItem = new FixedItem();
            var entry = new FixedItem.Entry(ItemIndex.ARROW_IRON);
            fixedItem.Entries.Add(entry);
            entry.Quantity = 10;
            entry.Short04 = 4;
            entry.Byte06 = 6;

            // Act
            var data = fixedItem.Build();
            var rebuiltFixedItem = new FixedItem(data.Data);

            // Assert
            rebuiltFixedItem.Entries.Should().HaveCount(1);
            rebuiltFixedItem.Entries[0].Index.Should().Be(ItemIndex.ARROW_IRON);
            rebuiltFixedItem.Entries[0].Quantity.Should().Be(10);
            rebuiltFixedItem.Entries[0].Short04.Should().Be(4);
            rebuiltFixedItem.Entries[0].Byte06.Should().Be(6);
        }
    }
}
