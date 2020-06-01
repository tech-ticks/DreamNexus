using System;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    /// <summary>
    /// Utility for reading and patching ARM instruction values
    /// </summary>
    public class ArmInstruction
    {
        public enum InstructionCode : uint
        {
            // http://shell-storm.org/armv8-a/ISA_v85A_A64_xml_00bet8/xhtml/mov_movz.html
            MovToXRegister = 0xD2,
            MovToXRegister64Bit = 0x52,

            // http://shell-storm.org/armv8-a/ISA_v85A_A64_xml_00bet8/xhtml/mov_orr_log_imm.html
            // Currently unsupported because it uses a really weird encoding. 
            // It's only used for a few Actors most of which shouldn't be edited anyway.
            MovBitmaskImmediate = 0x32
        }

        private uint instruction;

        public ArmInstruction(uint instruction)
        {
            this.instruction = instruction;
        }

        public uint RawInstruction => instruction;

        // There's a lot more to ARM instruction codes but this is currently
        // good enough for comparisons
        public InstructionCode Code => (InstructionCode) (instruction >> 24);

        public bool IsSupported => Code == InstructionCode.MovToXRegister
                                   || Code == InstructionCode.MovToXRegister64Bit;

        public ushort GetValue()
        {
            switch (Code)
            {
                case InstructionCode.MovToXRegister:
                case InstructionCode.MovToXRegister64Bit:
                    return (ushort) ((instruction >> 5) & 0x7FFF);
                default:
                    throw new UnsupportedInstructionException(instruction);
            }
        }

        public void PatchValue(ushort newValue)
        {
            if (!IsSupported)
            {
                throw new UnsupportedInstructionException(instruction);
            }

            uint instructionWithMaskedValue = instruction & 0xFFE0001F;
            instruction = instructionWithMaskedValue | (uint) (newValue << 5);
        }

        public class UnsupportedInstructionException : Exception
        {
            public UnsupportedInstructionException(uint instruction)
                : base("Unsupported instruction: 0x" + instruction.ToString("x"))
            {}
        }
    }
}