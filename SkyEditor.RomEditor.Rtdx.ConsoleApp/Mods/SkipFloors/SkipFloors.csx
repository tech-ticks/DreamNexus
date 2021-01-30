#load "../../Stubs/Rtdx.csx"

using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Structures.Executable;
using System.Linq;
using System.Runtime.CompilerServices;

// TODO: test this with the updated version
var executable = Rom.GetMainExecutable();
var codeHelper = new CodeGenerationHelper(executable);

// ---
// Code changes
// ---

/*
  Instead of throwing the registered item with DungeonPlayerCommand.SetNative(),
  call DungeonPlayerCommand.UseWaza(WazaIndex.YOBI_03, 0).
  WazaIndex.YOBI_03 is an unused move which we can edit to act like an unused trap
  that would send the target to the next floor.

  Original code:
  .text:0000000000DCD304   MOV   X1, X21
  .text:0000000000DCD308   MOV   X2, XZR
  .text:0000000000DCD30C   BL    DungeonPlayerCommand_SetNative ; Void SetNative(PlayerCommand, IStructDefParameter)

  Changed to:
  .text:0000000000DCD304   MOV   X0, #685 (685 == WazaIndex.YOBI_03)
  .text:0000000000DCD308   MOV   X1, XZR
  .text:0000000000DCD30C   BL    DungeonPlayerCommand_UseWaza ;

  Encoded as A05580D2 E1031FAA 99C7F797
 */

codeHelper.SetOffsetToMethod("UIWazaButtonSet", "_doItemThrow", new string[] {});
codeHelper.Offset += 0x184;
codeHelper.WriteCode("A05580D2E1031FAA");
codeHelper.WriteMethodCall("DungeonPlayerCommand", "UseWaza", new string[] {"Const.waza.Index", "IItem"});

/*
  Bypass the check whether we've registered an item by jumping to the part that displays the item

  Original code:
  .text:0000000000DCD1DC BL ItemBag_GetCurrentBag ; ItemBag GetCurrentBag()

  Changed to:
  .text:0000000000DCD1DC BL #0x124

  Encoded as 49000094
 */

codeHelper.SetOffsetToMethod("UIWazaButtonSet", "_doItemThrow", new string[] {});
codeHelper.Offset += 0x5c;
codeHelper.WriteRelativeBranchWithLink(0x124);

/*
  Don't show the registered item when holding ZL.
  This is done by stubbing out the check whether the registered item is null.

  Original code:
  .text:0000000000DC8238 CBZ X20, loc_DC8388 (= local offset of #0x150)

  Changed to:
  .text:0000000000DC8238 CBZ XZR, #0x150

  Encoded as 9F0A00B4
 */

codeHelper.SetOffsetToMethod("UIWazaButtonSet", "SetRegistItem", new string[] {});
codeHelper.Offset += 0x178;
codeHelper.WriteCode("9F0A00B4");

executable.Data = codeHelper.Data;

// ---
// Move changes
// ---

var wazaDataInfo = Rom.GetWazaDataInfo();
var yobi03 = wazaDataInfo.Entries[(int) WazaIndex.YOBI_03];
yobi03.ActIndex = 954; // Deal fixed damage and fall to the next floor

// Set damage to zero
var effect = Rom.GetActDataInfo().Entries[954].Effects[0];
int damageParamIndex = Array.FindIndex(effect.ParamTypes, paramType => paramType == EffectParameterType.FixedDamage);
effect.Params[damageParamIndex] = 0;


// ---
// Text changes
// ---

var messageBin = new MessageBinEntry(Rom.GetUSMessageBin().GetFile("common.bin"));
var codeTable = Rom.GetCodeTable();

foreach (var messages in messageBin.Strings.Values)
{
  var message = messages.First();
  if (message.Value == "Registered item" || message.Value == "Throw registered item")
  {
    // Text in the ZL button overlay
    message.Value = "Skip floor";
  }
}

// Save the modified strings
Rom.GetUSMessageBin().SetFile("common.bin", messageBin.ToByteArray());
