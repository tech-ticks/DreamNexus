using SkyEditor.RomEditor.Rtdx.Constants;
using System;
using System.IO;
using System.Linq;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures.Executable
{
    public partial class PegasusActDatabase
    {
        /// <summary>
        /// Offset of .text, must be added to the other values. This value works in Versions 1.0 and 1.1
        /// </summary>
        private const int TextOffset = 0x788;

        /// <summary>
        /// Offset of the first creature ID ("HERO") relative to the .text offset. This value works in Version 1.0
        /// </summary>
        private const int FirstCreatureIdOffsetOriginal = 0xB61290;

        /// <summary>
        /// Offset of the first creature ID ("HERO") relative to the .text offset. This value works in Version 1.1
        /// </summary>
        private const int FirstCreatureIdOffsetUpdate = 0x6172B0;

        public PegasusActDatabase(byte[] elfData, ExecutableVersion version)
        {
            this.elfData = elfData;

            firstCreatureIdOffset = version switch
            {
                ExecutableVersion.Original => FirstCreatureIdOffsetOriginal,
                ExecutableVersion.Update1 => FirstCreatureIdOffsetUpdate,
                _ => throw new ArgumentOutOfRangeException(nameof(version)),
            };

            Read();
        }

        private readonly byte[] elfData;
        private readonly int firstCreatureIdOffset;

        private void Read()
        {
            int absoluteFirstOffset = AbsolutePokemonIndexOffset(ActorDataList.First());
            var firstOffsetInstruction = new ArmInstruction(BitConverter.ToUInt32(elfData, absoluteFirstOffset));
            if (!firstOffsetInstruction.IsSupported)
            {
                throw new InvalidOperationException("Cannot read Actor database - maybe an incompatible version was used?");
            }

            foreach (var actorData in ActorDataList.Where(actorData => actorData.PokemonIndexEditable))
            {
                var instruction =
                    new ArmInstruction(BitConverter.ToUInt32(elfData, AbsolutePokemonIndexOffset(actorData)));

                if (instruction.IsSupported)
                {
                    // It doesn't matter if the instruction is unsupported here because we can rely
                    // on the hardcoded values PegasusActDatabase.Data.cs and it will be readable after edits.
                    actorData.PokemonIndex = (CreatureIndex) instruction.Value;
                }
            }
        }

        public void Write()
        {
            foreach (var actorData in ActorDataList.Where(actorData => actorData.PokemonIndexEditable))
            {
                var instruction = new ArmInstruction(BitConverter.ToUInt32(elfData, AbsolutePokemonIndexOffset(actorData)));
                if (instruction.IsSupported)
                {
                    instruction.Value = (ushort) actorData.PokemonIndex;
                }
                else if (instruction.Code == ArmInstructionCode.MovBitmaskImmediateToWRegister)
                {
                    // Replace the unsupported instruction with an equivalent one
                    instruction = new ArmInstruction(ArmInstructionCode.MovImmediateToWRegister, instruction.Register,
                        (ushort) actorData.PokemonIndex);
                }

                BitConverter.GetBytes(instruction.RawInstruction).CopyTo(elfData, AbsolutePokemonIndexOffset(actorData));
            }
        }


        public int AbsolutePokemonIndexOffset(ActorData actorData) => actorData.PokemonIndexOffset + TextOffset + firstCreatureIdOffset;
    }
}