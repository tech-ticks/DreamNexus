using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Automation.CSharp
{
    /// <summary>
    /// Specifies a custom implementation to convert a value into a C# expression
    /// </summary>
    public class CSharpExpressionGeneratorAttribute : Attribute
    {
        private static readonly Type CSharpExpressionGeneratorType = typeof(ICSharpExpressionGenerator);

        public CSharpExpressionGeneratorAttribute(Type generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }
            if (!CSharpExpressionGeneratorType.IsAssignableFrom(generator))
            {
                throw new ArgumentNullException($"Generator type must implement {CSharpExpressionGeneratorType.Name}", nameof(generator));
            }

            this.Generator = generator;
        }

        public Type Generator { get; }
    }
}
