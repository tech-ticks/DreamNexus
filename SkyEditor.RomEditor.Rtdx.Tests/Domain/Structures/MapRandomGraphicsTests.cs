using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class MapRandomGraphicsTests
    {
        [Fact]
        public void CanBuildMapRandomGraphics()
        {
            // Arrange
            var db = new MapRandomGraphics();
            
            db.Entries.Add(new MapRandomGraphics.Entry
            {
                Symbol = "DN01_CAVE_01",
                AssetBundleName = "dn01_cave_01",
                ExtraFileName = "dn01_cave",
                Unk1 = 10,
                Unk2 = 20,
                Unk3 = 30,
            });
            db.Entries.Add(new MapRandomGraphics.Entry
            {
                Symbol = "DN02_MAGMA_01",
                AssetBundleName = "dn02_magma_01",
                ExtraFileName = "dn02_magma",
                Unk1 = 100000000,
                Unk2 = 12,
                Unk3 = 13,
            });

            // Act
            var data = db.ToByteArray();
            var rebuiltDb = new MapRandomGraphics(data);

            // Assert
            rebuiltDb.Entries.Count.Should().Be(2);

            rebuiltDb.Entries[0].Symbol.Should().Be("DN01_CAVE_01");
            rebuiltDb.Entries[0].AssetBundleName.Should().Be("dn01_cave_01");
            rebuiltDb.Entries[0].ExtraFileName.Should().Be("dn01_cave");
            rebuiltDb.Entries[0].Unk1.Should().Be(10);
            rebuiltDb.Entries[0].Unk2.Should().Be(20);
            rebuiltDb.Entries[0].Unk3.Should().Be(30);

            rebuiltDb.Entries[1].Symbol.Should().Be("DN02_MAGMA_01");
            rebuiltDb.Entries[1].AssetBundleName.Should().Be("dn02_magma_01");
            rebuiltDb.Entries[1].ExtraFileName.Should().Be("dn02_magma");
            rebuiltDb.Entries[1].Unk1.Should().Be(100000000);
            rebuiltDb.Entries[1].Unk2.Should().Be(12);
            rebuiltDb.Entries[1].Unk3.Should().Be(13);
        }
    }
}
