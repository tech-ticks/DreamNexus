using FluentAssertions;
using Xunit;

namespace SkyEditor.RomEditor.Tests.Infrastructure
{
    public class CsvParserTests
    {
        [Fact]
        public void ParsesCsv()
        {
            // Since we need to escape this string too, duplicate double quotes are actually single characters
            var parser = new CsvParser(@"foo,bar
Bulbasaur,""Charmander""
""Squirtle"",""Some, """"other"""" name,"",""baz""
");

            var expected = new[] {
                new[] { "foo", "bar" },
                new[] { "Bulbasaur", "Charmander" },
                new[] { "Squirtle", "Some, \"other\" name,", "baz" },
            };

            parser.GetLines().Should().BeEquivalentTo(expected);
        }
    }
}
