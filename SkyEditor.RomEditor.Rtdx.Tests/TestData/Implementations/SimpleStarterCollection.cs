using SkyEditor.RomEditor.Rtdx.Constants;
using SkyEditor.RomEditor.Rtdx.Domain.Models;
using System;
using System.Linq;

namespace SkyEditor.RomEditor.Rtdx.Tests.TestData.Implementations
{
    public class SimpleStarterCollection : IStarterCollection
    {
        public IStarterModel[] Starters { get; set; } = new IStarterModel[0];

        public void Flush()
        {
        }

        public string GenerateLuaChangeScript(int indentLevel = 0)
        {
            throw new NotImplementedException();
        }

        public IStarterModel? GetStarterById(CreatureIndex id)
        {
            return Starters.FirstOrDefault(s => s.PokemonId == id);
        }
    }
}
