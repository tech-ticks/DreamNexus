using SkyEditor.RomEditor.Rtdx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using CreatureIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;
using NatureType = SkyEditor.RomEditor.Rtdx.Reverse.NDConverterSharedData.NatureType;
using WazaIndex = SkyEditor.RomEditor.Rtdx.Reverse.Const.waza.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Models
{
    public interface IStarterModel
    {
        CreatureIndex PokemonId { get; set; }
        string PokemonName { get; }
        string? NatureDiagnosisMaleModelSymbol { get; set; }
        string? NatureDiagnosisFemaleModelSymbol { get; set; }
        WazaIndex Move1 { get; set; }
        string Move1Name { get; }
        WazaIndex Move2 { get; set; }
        string Move2Name { get; }
        WazaIndex Move3 { get; set; }
        string Move3Name { get; }
        WazaIndex Move4 { get; set; }
        string Move4Name { get; }

        NatureType? MaleNature { get; set; }
        NatureType? FemaleNature { get; set; }

        IStarterModel Clone();        
    }

    [DebuggerDisplay("StarterModel: {PokemonName}")]
    public class StarterModel : IStarterModel
    {
        public StarterModel(ICommonStrings commonStrings)
        {
            this.commonStrings = commonStrings ?? throw new ArgumentNullException(nameof(commonStrings));
        }

        private readonly ICommonStrings commonStrings;

        public CreatureIndex PokemonId { get; set; }
        public string PokemonName => commonStrings.Pokemon.GetValueOrDefault(PokemonId) ?? $"(Unknown: {PokemonId})";
        public string? NatureDiagnosisMaleModelSymbol { get; set; }
        public string? NatureDiagnosisFemaleModelSymbol { get; set; }
        public WazaIndex Move1 { get; set; }
        public string Move1Name => commonStrings.Moves.GetValueOrDefault(Move1) ?? $"(Unknown: {Move1})";
        public WazaIndex Move2 { get; set; }
        public string Move2Name => commonStrings.Moves.GetValueOrDefault(Move2) ?? $"(Unknown: {Move2})";
        public WazaIndex Move3 { get; set; }
        public string Move3Name => commonStrings.Moves.GetValueOrDefault(Move3) ?? $"(Unknown: {Move3})";
        public WazaIndex Move4 { get; set; }
        public string Move4Name => commonStrings.Moves.GetValueOrDefault(Move4) ?? $"(Unknown: {Move4})";

        public NatureType? MaleNature { get; set; }
        public NatureType? FemaleNature { get; set; }

        public IStarterModel Clone()
        {
            return new StarterModel(commonStrings)
            {
                PokemonId = this.PokemonId,
                NatureDiagnosisMaleModelSymbol = this.NatureDiagnosisMaleModelSymbol,
                NatureDiagnosisFemaleModelSymbol = this.NatureDiagnosisFemaleModelSymbol,
                Move1 = this.Move1,
                Move2 = this.Move2,
                Move3 = this.Move3,
                Move4 = this.Move4,
                MaleNature = this.MaleNature,
                FemaleNature = this.FemaleNature
            };
        }
    }
}
