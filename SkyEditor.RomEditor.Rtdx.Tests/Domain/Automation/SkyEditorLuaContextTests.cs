using AssetStudio;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using NLua.Exceptions;
using SkyEditor.RomEditor.Rtdx.Domain;
using SkyEditor.RomEditor.Rtdx.Domain.Automation;
using SkyEditor.RomEditor.Rtdx.Domain.Models;
using SkyEditor.RomEditor.Rtdx.Tests.TestData.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace SkyEditor.RomEditor.Rtdx.Tests.Domain.Automation
{
    public class SkyEditorLuaContextTests
    {
        [Fact]
        public void ErrorsBubbleUp()
        {
            // Arrange
            var rom = Mock.Of<IRtdxRom>();
            var context = new SkyEditorLuaContext(rom);
            var script = File.ReadAllText("TestData/Scripts/ErrorTest.lua");

            // Act & Assert
            context.Invoking(c => c.Execute(script))
                .Should().Throw<LuaException>()
                .Which.Message.Should().Contain("Error thrown successfully");

            context.LuaState["execution_continued"].Should().NotBeNull();
            context.LuaState["execution_continued"].Should().Be(false);
        }

        [Fact]
        public void CanInteractWithRom()
        {
            // Arrange
            var romMock = new Mock<IRtdxRom>();
            romMock.Setup(r => r.GetStarters()).Returns(new SimpleStarterCollection
            {
                Starters = new SimpleStarterModel[]
                {
                    new SimpleStarterModel
                    {
                        PokemonId = Reverse.Const.creature.Index.RIORU,
                        PokemonName = "Riolu"
                    },new SimpleStarterModel
                    {
                        PokemonId = Reverse.Const.creature.Index.MYUU,
                        PokemonName = "Mew"
                    },
                }
            });

            var context = new SkyEditorLuaContext(romMock.Object);
            var script = File.ReadAllText("TestData/Scripts/RomInteractionTest.lua");

            // Act
            context.Execute(script);

            // Assert
            context.LuaState["passed"].Should().NotBeNull();
            context.LuaState["passed"].Should().Be(true);
        }
    }
}
