using System.Text;
using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class Sir0StringListTests
    {
        [Fact]
        public void CanBuildStringLists()
        {
            // Arrange
            var db = new Sir0StringList(Encoding.Unicode);
            db.Entries.Add("Red");
            db.Entries.Add("Blue");
            db.Entries.Add("Time");
            db.Entries.Add("Darkness");
            db.Entries.Add("Sky");

            // Act
            var data = db.ToByteArray();

            // Assert
            var rebuiltDb = new Sir0StringList(data);
            rebuiltDb.Entries.Should().Equal("Red", "Blue", "Time", "Darkness", "Sky");
        }
    }
}
