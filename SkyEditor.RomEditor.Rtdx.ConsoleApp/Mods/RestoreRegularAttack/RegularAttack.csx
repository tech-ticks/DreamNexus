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
  call DungeonPlayerCommand.UseWaza(WazaIndex.TSUUJOU_KOUGEKI, 0).
  WazaIndex.TSUUJOU_KOUGEKI is the index of the regular attack move.

  Original code:
  .text:0000000000DCD304   MOV   X1, X21
  .text:0000000000DCD308   MOV   X2, XZR
  .text:0000000000DCD30C   BL    DungeonPlayerCommand_SetNative ; Void SetNative(PlayerCommand, IStructDefParameter)

  Changed to:
  .text:0000000000DCD304   MOV   X0, #0x2b1 (0x2b1 == WazaIndex.TSUUJOU_KOUGEKI)
  .text:0000000000DCD308   MOV   X1, XZR
  .text:0000000000DCD30C   BL    DungeonPlayerCommand_UseWaza ;

  Encoded as 205680D2E1031FAA99C7F797
 */

codeHelper.SetOffsetToMethod("UIWazaButtonSet", "_doItemThrow", new string[] {});
codeHelper.Offset += 0x184;
codeHelper.WriteCode("205680D2E1031FAA");
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
  Don't show the registered item when holding ZL to avoid confusing players.
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
    message.Value = "Regular attack";
  }
  else if (message.Value.StartsWith("Register (use with"))
  {
    // Change the text in the item menu to communicate that registered items can't be used with ZL+ZR anymore
    message.Value = "Register for teammates";
  }
  else if (message.Value.Contains("Select the item, then")) {
    // Also change the tutorial text
    // TODO: Check if the game displays an image for this tutorial. If it does, make a cheap Photoshop edit
    string newTutorialText = "You can set [M:I0302]stones or [M:I0202]spikes to throw by[R]opening the [M:B03] menu and going "
      + "to your [CS:6][item_bag][CR].[R]Select the item, then [M:IDL61][CS:6]register[CR] it. Your teammates[R] will "
      + "then be able to throw the item.";
    message.Value = codeTable.UnicodeEncode(newTutorialText);
  }
}

// Save the modified strings
Rom.GetUSMessageBin().SetFile("common.bin", messageBin.ToByteArray());
