using SkyEditor.RomEditor.Rtdx.Reverse.Const.pokemon;
using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SkyEditor.RomEditor.Rtdx.Reverse;
using Creature = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public partial class PegasusActDatabase
    {
        public class ActorData
        {
            public enum PartyID
            {
                NONE,
                PARTY1,
                PARTY2,
                PARTY3
            }

            public string symbolName = default!;
            internal Creature raw_pokemonIndex;
            public FormType raw_formType;
            public bool bIsFemale;
            public PartyID opt_partyId;
            public PokemonWarehouseId opt_warehouseId = default!;
            public TextId opt_specialName = default!;
            public string? debug_name;
            public int pokemonIndexOffset;
            public bool pokemonIndexEditable;

            public int AbsolutePokemonIndexOffset => pokemonIndexOffset + TextOffset + FirstCreatureIdOffset;

            internal string Name // Should be public when implemented
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public Creature PokemonIndex // Should be public when implemented
            {
                get => raw_pokemonIndex;
                set
                {
                    if (!pokemonIndexEditable)
                    {
                        throw new InvalidOperationException("This entry is not editable.");
                    }
                    raw_pokemonIndex = value;
                }
            }

            internal FormType FormType // Should be public when implemented
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            internal PokemonWarehouseId WarehouseId // Should be public when implemented
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }

        public class MapData
        {
            public string? symbolName;
            public string? assetBundleName;
            public string? prefabName;
        }

        public class GimmickData
        {
            public string? symbolName;
            public string? assetBundleName;
            public string? prefabName;
        }

        public class EffectData
        {
            public string? symbolName;
            public string? effectSymbol;
        }
        
        /// <summary>
        /// Offset of .text, must be added to the other values
        /// </summary>
        private const int TextOffset = 0x788;

        /// <summary>
        /// Offset of the first creature ID ("HERO") relative to the .text offset. This value works in Version 1.0
        /// </summary>
        private const int FirstCreatureIdOffset = 0xB61290;

        private byte[] elfData;

        public PegasusActDatabase(byte[] elfData)
        {
            this.elfData = elfData;

            Read();
        }

        public void Read()
        {
            int absoluteFirstOffset = ActorDataList.First().AbsolutePokemonIndexOffset;
            var firstOffsetInstruction = new ArmInstruction(BitConverter.ToUInt32(elfData, absoluteFirstOffset));
            if (!firstOffsetInstruction.IsSupported)
            {
                throw new InvalidOperationException("Cannot read Actor database - maybe an incompatible version was used?");
            }

            foreach (var actorData in ActorDataList.Where(actorData => actorData.pokemonIndexEditable))
            {
                var instruction = new ArmInstruction(BitConverter.ToUInt32(elfData, actorData.AbsolutePokemonIndexOffset));
                actorData.raw_pokemonIndex = (Creature) instruction.GetValue();
            }
        }

        public void Write()
        {
            foreach (var actorData in ActorDataList.Where(actorData => actorData.pokemonIndexEditable))
            {
                var instruction = new ArmInstruction(BitConverter.ToUInt32(elfData, actorData.AbsolutePokemonIndexOffset));
                instruction.PatchValue((ushort) actorData.raw_pokemonIndex);
                BitConverter.GetBytes(instruction.RawInstruction).CopyTo(elfData, actorData.AbsolutePokemonIndexOffset);
            }
        }

        public ActorData? FindActorData(string symbol)
        {
            return ActorDataList.Find(d => d.symbolName == symbol);
        }

        public void LoadCharaObject(Creature index, FormType formType, Action<CharacterModel> loadedCb)
        {
            throw new NotImplementedException();
        }

        public MapData FindMapData(string symbol)
        {
            throw new NotImplementedException();
        }

        public GimmickData FindGimmick(string symbol)
        {
            throw new NotImplementedException();
        }
        public EffectData FindEffect(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}