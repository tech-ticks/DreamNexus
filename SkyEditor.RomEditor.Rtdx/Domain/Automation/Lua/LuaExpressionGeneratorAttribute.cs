using System;

namespace SkyEditor.RomEditor.Domain.Automation.Lua
{
    /// <summary>
    /// Specifies a custom implementation to convert a value into a Lua expression
    /// </summary>
    public class LuaExpressionGeneratorAttribute : Attribute
    {
        private static readonly Type LuaExpressionGeneratorType = typeof(ILuaExpressionGenerator);

        public LuaExpressionGeneratorAttribute(Type generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }
            if (!LuaExpressionGeneratorType.IsAssignableFrom(generator))
            {
                throw new ArgumentNullException($"Generator type must implement {LuaExpressionGeneratorType.Name}", nameof(generator));
            }

            this.Generator = generator;
        }

        public Type Generator { get; }
    }
}
