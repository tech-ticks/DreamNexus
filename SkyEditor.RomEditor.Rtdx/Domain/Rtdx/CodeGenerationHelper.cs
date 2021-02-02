#if !NETSTANDARD2_0
using System;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Structures.Executable;

namespace SkyEditor.RomEditor.Domain.Rtdx
{
  public class CodeGenerationHelper
  {
    public IMainExecutable Executable { get; set; }
    public ulong Offset { get; set; }
    public byte[] Data { get; }
    public ulong GlobalOffset => sectionOffset + Offset;

    private ulong sectionOffset = 0;

    public CodeGenerationHelper(IMainExecutable executable)
    {
      Executable = executable;
      sectionOffset = executable.CodeSectionOffset;
      Data = executable.Data;
    }

    public void WriteCode(string hexString)
    {
      WriteCode(HexStringToByteArray(hexString));
    }

    public void WriteCode(uint data)
    {
      WriteCode(BitConverter.GetBytes(data));
    }

    public void WriteCode(byte[] bytes)
    {
      Array.Copy(bytes, 0, Data, (long) GlobalOffset, bytes.LongLength);
      Offset += (uint) bytes.Length;
    }

    public void WriteMethodCall(string fullTypeName, string methodName, string[] paramTypeNames)
    {
      ulong methodOffset = Executable.GetIlMethodOffset(fullTypeName, methodName, paramTypeNames);
      WriteRelativeBranchWithLink(methodOffset - Offset);
    }

    public void WriteRelativeBranchWithLink(ulong relativeOffset)
    {
      WriteCode(GenerateRelativeBranchWithLink(relativeOffset));
    }

    public void SetOffsetToMethod(string fullTypeName, string methodName, string[] paramTypeNames)
    {
      Offset = Executable.GetIlMethodOffset(fullTypeName, methodName, paramTypeNames);
    }

    public void SetOffsetToConstructor(string fullTypeName, string[] paramTypeNames)
    {
      Offset = Executable.GetIlConstructorOffset(fullTypeName, paramTypeNames);
    }

    public static byte[] HexStringToByteArray(string hexString)
    {
      return Enumerable.Range(0, hexString.Length / 2) 
        .Select(x => Convert.ToByte(hexString.Substring(x * 2, 2), 16))
        .ToArray();
    }

    public static uint GenerateRelativeBranchWithLink(ulong relativeOffset)
    {
      // http://shell-storm.org/armv8-a/ISA_v85A_A64_xml_00bet8_OPT/xhtml/bl.html
      uint relativeAddress = (uint) relativeOffset / 4;
      return 0x94000000 | (relativeAddress & 0x3ffffff);
    }
  }
}
#endif
