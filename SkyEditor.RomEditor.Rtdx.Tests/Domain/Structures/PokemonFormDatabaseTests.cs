using System;
using FluentAssertions;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Domain.Structures
{
    public class PokemonFormDatabaseTests
    {
        [Fact]
        public void CanBuildPokemonFormDatabaseTests()
        {
            // Arrange
            var entryIds1 = new short[] {10, 11, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            var entryIds2 = new short[] {0, 1, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            
            var db = new PokemonFormDatabase();
            db.Entries.Add(new PokemonFormDatabase.PokemonFormDatabaseEntry
            {
                PokemonGraphicsDatabaseEntryIds = entryIds1
            });
            db.Entries.Add(new PokemonFormDatabase.PokemonFormDatabaseEntry
            {
                PokemonGraphicsDatabaseEntryIds = entryIds2
            });

            // Act
            var data = db.ToByteArray();

            // Assert
            var rebuiltDb = new PokemonFormDatabase(data);
            rebuiltDb.Entries[0].PokemonGraphicsDatabaseEntryIds.Should().Equal(entryIds1);
            rebuiltDb.Entries[1].PokemonGraphicsDatabaseEntryIds.Should().Equal(entryIds2);
        }
        
        public class PokemonFormDatabaseEntryTests
        {
            [Fact]
            public void ThrowsExceptionWithWrongArraySize()
            {
                Action act = () => new PokemonFormDatabase.PokemonFormDatabaseEntry
                {
                    PokemonGraphicsDatabaseEntryIds = new short[] { 10, 11, 12 }
                };
                
                // Assert
                act.Should().Throw<Exception>();
            }
            
            [Fact]
            public void AcceptsRightArraySize()
            {
                // Act
                Action act = () => new PokemonFormDatabase.PokemonFormDatabaseEntry
                {
                    PokemonGraphicsDatabaseEntryIds = new short[PokemonFormDatabase.PokemonFormDatabaseEntry.ExpectedIdCount]
                };
                
                // Assert
                act.Should().NotThrow<Exception>();
            }
        }
    }
}
