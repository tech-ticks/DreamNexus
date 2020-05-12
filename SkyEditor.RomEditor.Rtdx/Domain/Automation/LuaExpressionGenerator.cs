using SkyEditor.RomEditor.Rtdx.Infrastructure;
using System;
using System.Collections.Generic;
using CreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;
using WazaIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.waza.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Automation
{
    public interface ILuaExpressionGenerator
    {
        string Generate(object? value);
    }

    public class CreatureIndexLuaExpressionGenerator : ILuaExpressionGenerator
    {
        public CreatureIndexLuaExpressionGenerator(ICommonStrings? commonStrings = null)
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

            string? friendlyName = commonStrings?.Pokemon?.GetValueOrDefault((int)index);
            if (!string.IsNullOrEmpty(friendlyName))
            {
                return $"Const.creature.Index.{obj:f} --[[{friendlyName}]]";
            }
            else
            {
                return $"Const.creature.Index.{obj:f}";
            }
        }
    }

    public class WazaIndexLuaExpressionGenerator : ILuaExpressionGenerator
    {
        public WazaIndexLuaExpressionGenerator(ICommonStrings? commonStrings = null)
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

            string? friendlyName = commonStrings?.Moves?.GetValueOrDefault((int)index);
            if (!string.IsNullOrEmpty(friendlyName))
            {
                return $"Const.waza.Index.{obj:f} --[[{friendlyName}]]";
            }
            else
            {
                return $"Const.waza.Index.{obj:f}";
            }
        }
    }
}
