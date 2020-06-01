using AssetStudio;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using NLua.Exceptions;
using SkyEditor.RomEditor.Rtdx.Constants;
using SkyEditor.RomEditor.Rtdx.Domain;
using SkyEditor.RomEditor.Rtdx.Domain.Automation;
using SkyEditor.RomEditor.Rtdx.Domain.Models;
using SkyEditor.RomEditor.Rtdx.Tests.TestData.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SkyEditor.RomEditor.Rtdx.Tests.Domain.Automation
{
    public class SkyEditorScriptContextTests
    {
        public class LuaTests : SkyEditorScriptContextTests
        {
            [Fact]
            public void ErrorsBubbleUp()
            {
                // Arrange
                var rom = Mock.Of<IRtdxRom>();
                var context = new SkyEditorScriptContext(rom);
                var script = File.ReadAllText("TestData/Scripts/Lua/ErrorTest.lua");

                // Act & Assert
                context.Invoking(c => c.ExecuteLua(script))
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
                        PokemonId = CreatureIndex.RIORU,
                        PokemonName = "Riolu"
                    },new SimpleStarterModel
                    {
                        PokemonId = CreatureIndex.MYUU,
                        PokemonName = "Mew"
                    },
                    }
                });

                var context = new SkyEditorScriptContext(romMock.Object);
                var script = File.ReadAllText("TestData/Scripts/Lua/RomInteractionTest.lua");

                // Act
                context.ExecuteLua(script);

                // Assert
                context.LuaState["passed"].Should().NotBeNull();
                context.LuaState["passed"].Should().Be(true);
            }
        }
      
        public class CSharpTests
        {
            [Fact]
            public async Task CanInteractWithRom()
            {
                // Arrange
                var romMock = new Mock<IRtdxRom>();
                romMock.Setup(r => r.GetStarters()).Returns(new SimpleStarterCollection
                {
                    Starters = new SimpleStarterModel[]
                    {
                        new SimpleStarterModel
                        {
                            PokemonId = CreatureIndex.RIORU,
                            PokemonName = "Riolu"
                        },
                        new SimpleStarterModel
                        {
                            PokemonId = CreatureIndex.MYUU,
                            PokemonName = "Mew"
                        }
                    }
                });

                var context = new SkyEditorScriptContext(romMock.Object);
                var script = File.ReadAllText("TestData/Scripts/CSharp/RomInteractionTest.csx");

                // Act
                await context.ExecuteCSharp(script);                
            }
        }
    }
}
