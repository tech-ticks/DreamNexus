using SkyEditor.RomEditor.Rtdx.Constants;
using System;
using System.Linq;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures.Executable
{
    public partial class PegasusActDatabase
    { 
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

            foreach (var actorData in ActorDataList.Where(actorData => actorData.PokemonIndexEditable))
            {
                var instruction = new ArmInstruction(BitConverter.ToUInt32(elfData, actorData.AbsolutePokemonIndexOffset));
                actorData.PokemonIndex = (CreatureIndex)instruction.GetValue();
            }
        }

        public void Write()
        {
            foreach (var actorData in ActorDataList.Where(actorData => actorData.PokemonIndexEditable))
            {
                var instruction = new ArmInstruction(BitConverter.ToUInt32(elfData, actorData.AbsolutePokemonIndexOffset));
                instruction.PatchValue((ushort)actorData.PokemonIndex);
                BitConverter.GetBytes(instruction.RawInstruction).CopyTo(elfData, actorData.AbsolutePokemonIndexOffset);
            }
        }
    }
}