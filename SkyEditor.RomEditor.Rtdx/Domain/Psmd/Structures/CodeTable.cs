using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Common.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures
{
    public class PsmdCodeTable : CodeTable
    {
        private static readonly Dictionary<string, Type> StaticConstantReplacementTable = new Dictionary<string, Type>()
        {
            { "kind:", typeof(CreatureIndex) },
            { "ability:", typeof(AbilityIndex) },
            { "waza:", typeof(WazaIndex) },
        };

        protected override Dictionary<string, Type> ConstantReplacementTable => StaticConstantReplacementTable;

        public PsmdCodeTable()
        {
        }

        public PsmdCodeTable(byte[] data) : base(data)
        {
        }
    }
}
