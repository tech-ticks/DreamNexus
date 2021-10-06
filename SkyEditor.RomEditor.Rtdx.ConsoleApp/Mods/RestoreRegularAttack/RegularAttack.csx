#load "../../Stubs/Rtdx.csx"

using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

// R+A usually triggers the move "Trap Finder" (WazaIndex.WANA_SAGASHI).
// We can change it to the regular attack by simply changing the action of Trap Finder
// to the action of the unused regular attack (WazaIndex.TSUUJOU_KOUGEKI).
var moves = Rom.GetMoves();
var trapFinder = moves.GetMoveById(WazaIndex.WANA_SAGASHI);
var unusedRegularAttack = moves.GetMoveById(WazaIndex.TSUUJOU_KOUGEKI);

trapFinder.ActIndex = unusedRegularAttack.ActIndex;

// Also change the move name to make it appear correctly
var strings = Rom.GetStrings().English;
strings.SetCommonString(TextIDHash.WAZA_NAME__WAZA_WANA_SAGASHI, "Attack");
