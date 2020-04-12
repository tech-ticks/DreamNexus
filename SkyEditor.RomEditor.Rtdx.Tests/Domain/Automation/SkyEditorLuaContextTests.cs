using FluentAssertions;
using SkyEditor.RomEditor.Rtdx.Domain.Automation;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SkyEditor.RomEditor.Rtdx.Tests.Domain.Automation
{
    public class SkyEditorLuaContextTests
    {
        [Fact]
        public void EnumTranslationCreatesExpectedObject()
        {
            // Act
            var luaObject = SkyEditorLuaContext.EnumToLuaObject(typeof(TestEnum));

            // Assert
            luaObject.Should().Be("{Item1=1,Item2=2,Item4=4}");
        }

        private enum TestEnum
        {
            Item1 = 1,
            Item2 = 2,
            Item4 = 4
        }
    }
}
