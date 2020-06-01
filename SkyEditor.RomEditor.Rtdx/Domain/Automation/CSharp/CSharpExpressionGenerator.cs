using SkyEditor.RomEditor.Rtdx.Constants;
using SkyEditor.RomEditor.Rtdx.Infrastructure;
using System;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Rtdx.Domain.Automation.CSharp
{
    public interface ICSharpExpressionGenerator : IScriptExpressionGenerator
    {
    }

    public class CreatureIndexCSharpExpressionGenerator : ICSharpExpressionGenerator
    {
        public CreatureIndexCSharpExpressionGenerator(ICommonStrings? commonStrings = null)
        {
            this.commonStrings = commonStrings;
        }

        private readonly ICommonStrings? commonStrings;

        public string Generate(object? obj)
        {
            if (!(obj is CreatureIndex index))
            {
                throw new ArgumentException("Unsupported value type");
            }

            string? friendlyName = commonStrings?.Pokemon?.GetValueOrDefault(index);
            if (!string.IsNullOrEmpty(friendlyName))
            {
                return $"CreatureIndex.{obj:f} /* {friendlyName} */";
            }
            else
            {
                return $"CreatureIndex.{obj:f}";
            }
        }
    }

    public class WazaIndexCSharpExpressionGenerator : ICSharpExpressionGenerator
    {
        public WazaIndexCSharpExpressionGenerator(ICommonStrings? commonStrings = null)
        {
            this.commonStrings = commonStrings;
        }

        private readonly ICommonStrings? commonStrings;

        public string Generate(object? obj)
        {
            if (!(obj is WazaIndex index))
            {
                throw new ArgumentException("Unsupported value type");
            }

            string? friendlyName = commonStrings?.Moves?.GetValueOrDefault(index);
            if (!string.IsNullOrEmpty(friendlyName))
            {
                return $"WazaIndex.{obj:f} /* {friendlyName} */";
            }
            else
            {
                return $"WazaIndex.{obj:f}";
            }
        }
    }
}
