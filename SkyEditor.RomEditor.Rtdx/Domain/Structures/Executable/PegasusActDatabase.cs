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
                var instruction = new ArmInstruction(BitConverter.ToUInt32(elfData, AbsolutePokemonIndexOffset(actorData)));
                actorData.PokemonIndex = (CreatureIndex)instruction.GetValue();
            }
        }

        public void Write()
        {
            foreach (var actorData in ActorDataList.Where(actorData => actorData.PokemonIndexEditable))
            {
                var absoluteOffset = AbsolutePokemonIndexOffset(actorData);
                var instruction = new ArmInstruction(BitConverter.ToUInt32(elfData, absoluteOffset));
                instruction.PatchValue((ushort)actorData.PokemonIndex);
                BitConverter.GetBytes(instruction.RawInstruction).CopyTo(elfData, absoluteOffset);
            }
        }


        public int AbsolutePokemonIndexOffset(ActorData actorData) => actorData.PokemonIndexOffset + TextOffset + firstCreatureIdOffset;
    }
}