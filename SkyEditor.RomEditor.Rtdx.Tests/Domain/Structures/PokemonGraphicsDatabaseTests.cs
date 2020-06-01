using FluentAssertions;
using SkyEditor.RomEditor.Rtdx.Constants;
using SkyEditor.RomEditor.Rtdx.Domain.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Rtdx.Tests.Domain.Structures
{
    public class PokemonGraphicsDatabaseTests
    {
        [Fact]
        public void CanBuildPokemonGraphicsDatabaseTests()
        {
            // Arrange
            var db = new PokemonGraphicsDatabase();
            db.Entries.Add(new PokemonGraphicsDatabase.PokemonGraphicsDatabaseEntry
            {
                ModelName = "modelName",
                AnimationName = "animationName",
                BaseFormModelName = "baseFormModelName",
                PortraitSheetName = "portraitSheetName",
                RescueCampSheetName = "rescueCampSheetName",
                RescueCampSheetReverseName = "rescueCampSheetNameReverse",
                WalkSpeedDistance = 0.5f,
                RunSpeedRatioGround = 0.5f,
                UnknownBodyType1 = GraphicsBodySizeType.G_BODY_SIZE_L,
                UnknownBodyType2 = GraphicsBodySizeType.G_BODY_SIZE_M
            });
            db.Entries.Add(new PokemonGraphicsDatabase.PokemonGraphicsDatabaseEntry
            {
                ModelName = "modelName2",
                AnimationName = "animationName2",
                BaseFormModelName = "",
                PortraitSheetName = "portraitSheetName2",
                RescueCampSheetName = "",
                RescueCampSheetReverseName = "rescueCampSheetNameReverse2",
                WalkSpeedDistance = 1.5f,
                RunSpeedRatioGround = 1.5f,
                UnknownBodyType1 = GraphicsBodySizeType.G_BODY_SIZE_SS,
                UnknownBodyType2 = GraphicsBodySizeType.G_BODY_SIZE_XL
            });

            // Act
            var data = db.ToByteArray();

            // Assert
            var rebuiltDb = new PokemonGraphicsDatabase(data);
            rebuiltDb.Entries[0].ModelName.Should().Be("modelName");
            rebuiltDb.Entries[0].AnimationName.Should().Be("animationName");
            rebuiltDb.Entries[0].BaseFormModelName.Should().Be("baseFormModelName");
            rebuiltDb.Entries[0].PortraitSheetName.Should().Be("portraitSheetName");
            rebuiltDb.Entries[0].RescueCampSheetName.Should().Be("rescueCampSheetName");
            rebuiltDb.Entries[0].RescueCampSheetReverseName.Should().Be("rescueCampSheetNameReverse");
            rebuiltDb.Entries[0].WalkSpeedDistance.Should().Be(0.5f);
            rebuiltDb.Entries[0].RunSpeedRatioGround.Should().Be(0.5f);
            rebuiltDb.Entries[0].UnknownBodyType1.Should().Be(GraphicsBodySizeType.G_BODY_SIZE_L);
            rebuiltDb.Entries[0].UnknownBodyType2.Should().Be(GraphicsBodySizeType.G_BODY_SIZE_M);

            rebuiltDb.Entries[1].ModelName.Should().Be("modelName2");
            rebuiltDb.Entries[1].AnimationName.Should().Be("animationName2");
            rebuiltDb.Entries[1].BaseFormModelName.Should().Be("");
            rebuiltDb.Entries[1].PortraitSheetName.Should().Be("portraitSheetName2");
            rebuiltDb.Entries[1].RescueCampSheetName.Should().Be("");
            rebuiltDb.Entries[1].RescueCampSheetReverseName.Should().Be("rescueCampSheetNameReverse2");
            rebuiltDb.Entries[1].WalkSpeedDistance.Should().Be(1.5f);
            rebuiltDb.Entries[1].RunSpeedRatioGround.Should().Be(1.5f);
            rebuiltDb.Entries[1].UnknownBodyType1.Should().Be(GraphicsBodySizeType.G_BODY_SIZE_SS);
            rebuiltDb.Entries[1].UnknownBodyType2.Should().Be(GraphicsBodySizeType.G_BODY_SIZE_XL);
        }
    }
}
