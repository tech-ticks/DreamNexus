using System;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures
{
    public enum ArmInstructionCode : uint
    {
        // http://shell-storm.org/armv8-a/ISA_v85A_A64_xml_00bet8/xhtml/mov_movz.html
        MovImmediateToWRegister = 0x1A5, // 32 Bit
        MovImmediateToXRegister = 0xA5, // 64 Bit

        // http://shell-storm.org/armv8-a/ISA_v85A_A64_xml_00bet8/xhtml/mov_orr_log_imm.html
        // Reading is currently unsupported because it uses a really weird encoding. 
        MovBitmaskImmediateToWRegister = 0x64
    }
    
    /// <summary>
    /// Utility for reading and patching ARM instruction values
    /// </summary>
    public class ArmInstruction
    {
        private uint instruction;

        public ArmInstruction(uint instruction)
        {
            this.instruction = instruction;
        }

        public ArmInstruction(ArmInstructionCode code, uint register, ushort value)
        {
            Code = code;
            Register = register;
            Value = value;
        }

        public uint RawInstruction => instruction;

        // There's a lot more to ARM instruction codes but this is currently
        // good enough for comparisons
        public ArmInstructionCode Code
        {
            get => (ArmInstructionCode) (instruction >> 23);
            set => instruction = (instruction & 0x7FFFFF) | ((uint) value << 23);
        }

        public uint Register
        {
            get => instruction & 0x1F;
            set
            {
                if (value > 0x1F)
                    throw new ArgumentException("Register out of range");

                instruction = (instruction & 0xFFFFFFE0) | value;
            }
        }

        public bool IsSupported => Code == ArmInstructionCode.MovImmediateToWRegister 
                                || Code == ArmInstructionCode.MovImmediateToXRegister;

        public ushort Value
        {
            get
            {
                switch (Code)
                {
                    case ArmInstructionCode.MovImmediateToWRegister:
                    case ArmInstructionCode.MovImmediateToXRegister:
                        return (ushort) ((instruction >> 5) & 0x7FFF);
                    default:
                        throw new UnsupportedInstructionException(instruction);
                }
            }
            set
            {
                switch (Code)
                {
                    case ArmInstructionCode.MovImmediateToWRegister:
                    case ArmInstructionCode.MovImmediateToXRegister:
                        instruction = (instruction & 0xFFE0001F) | (uint) (value << 5);
                        break;
                    default:
                        throw new UnsupportedInstructionException(instruction);
                }
            }
        }

        public class UnsupportedInstructionException : Exception
        {
            public UnsupportedInstructionException(uint instruction)
                : base("Unsupported instruction: 0x" + instruction.ToString("x"))
            {}
        }
    }
}