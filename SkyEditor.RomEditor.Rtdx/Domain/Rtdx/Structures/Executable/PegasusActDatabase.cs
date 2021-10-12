using SkyEditor.IO.Binary;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures.Executable
{
    public partial class PegasusActDatabase
    {
        /// <summary>
        /// Offset of the first creature ID ("HERO") relative to the start of the PegasusActDatabase constructor.
        /// </summary>
        private const ulong RelativeFirstCreatureIdOffset = 0x1b0;

        public PegasusActDatabase(IMainExecutable executable)
        {
            this.executable = executable;

#if !NETSTANDARD2_0
            firstCreatureIdOffset = executable.CodeSectionOffset
                + executable.GetIlConstructorOffset("PegasusActDatabase", new string[] {})
                + RelativeFirstCreatureIdOffset;

            Read();
#endif
        }

        private IMainExecutable executable;

#if !NETSTANDARD2_0
        private readonly ulong firstCreatureIdOffset = 0;

        private void Read()
        {
            ulong absoluteFirstOffset = AbsolutePokemonIndexOffset(ActorDataList.First());

            // TODO: get rid of the ArmInstruction class and add encoding and decoding functions for
            // the instructions used here to CodeGenerationHelper instead.
            var firstOffsetInstruction = new ArmInstruction(BitConverter.ToUInt32(executable.Data, (int) absoluteFirstOffset));
            if (!firstOffsetInstruction.IsSupported)
            {
                throw new InvalidOperationException("Cannot read Actor database - maybe an incompatible version was used?");
            }

            foreach (var actorData in ActorDataList.Where(actorData => actorData.PokemonIndexEditable))
            {
                var instruction =
                    new ArmInstruction(BitConverter.ToUInt32(executable.Data, (int) AbsolutePokemonIndexOffset(actorData)));

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
            // TODO: get rid of the ArmInstruction class and add encoding and decoding functions for
            // the instructions used here to CodeGenerationHelper instead.
            foreach (var actorData in ActorDataList.Where(actorData => actorData.PokemonIndexEditable))
            {
                var instruction = new ArmInstruction(BitConverter.ToUInt32(executable.Data,
                    (int) AbsolutePokemonIndexOffset(actorData)));
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

                BitConverter.GetBytes(instruction.RawInstruction).CopyTo(executable.Data,
                    (int) AbsolutePokemonIndexOffset(actorData));
            }
        }

        private ulong AbsolutePokemonIndexOffset(ActorData actorData)
        {
            return firstCreatureIdOffset + actorData.PokemonIndexOffset;
        }

#else
        public void Write()
        {
            throw new Exception("Not supported on .NET Standard 2.0");
        }
#endif

        public byte[] ToByteArray()
        {
            using var file = new BinaryFile(new MemoryStream());

            void WriteFixedLengthString(string str, int length)
            {
                int byteCount = Encoding.Unicode.GetByteCount(str);
                if (byteCount + 1 > length)
                {
                    throw new Exception($"Input string '{str}' too long, must be shorter than {length}");
                }
                file!.WriteNullTerminatedString(file.Position, Encoding.Unicode, str);
                for (int i = byteCount + 2; i < length; i++) {
                    file.Write(file.Position, 0);
                }
            }

            // The dummy entry at the beginning is usually ignored, but must be included here.
            var actorList = ActorDataList.Prepend(new ActorData
            {
                WarehouseId = PokemonFixedWarehouseId.NONE,
                SymbolName = "",
                DebugName = ""
            }).ToList();

            file.WriteUInt32(file.Position, (uint) actorList.Count);
            foreach (var actorData in actorList)
            {
                WriteFixedLengthString(actorData.SymbolName, 64);
                WriteFixedLengthString(actorData.DebugName ?? "", 64);
                file.WriteUInt16(file.Position, (ushort) actorData.PokemonIndex);
                file.WriteUInt16(file.Position, (ushort) actorData.FormType);
                file.Write(file.Position, actorData.IsFemale ? (byte) 1 : (byte) 0);
                file.Write(file.Position, (byte) actorData.PartyId);
                file.WriteUInt16(file.Position, (ushort) actorData.WarehouseId);
                file.WriteInt32(file.Position, (int) actorData.SpecialName);
            }

            return file.ReadArray();
        }

    }
}
