#load "../../Stubs/Rtdx.csx"

using System;
using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

// R+A usually triggers the move "Trap Finder" (WazaIndex.WANA_SAGASHI).
// We can change it to the regular attack by simply changing the action of Trap Finder
// to the action of the unused regular attack (WazaIndex.TSUUJOU_KOUGEKI).
var wazaDataInfo = Rom.GetWazaDataInfo();
var trapFinder = wazaDataInfo.Entries[(int) WazaIndex.WANA_SAGASHI];
var unusedRegularAttack = wazaDataInfo.Entries[(int) WazaIndex.TSUUJOU_KOUGEKI];

trapFinder.ActIndex = unusedRegularAttack.ActIndex;

// Also change the move name to make it appear correctly
var messageBin = new MessageBinEntry(Rom.GetUSMessageBin().GetFile("common.bin"));
long hash = (long) TextIDHash.WAZA_NAME__WAZA_WANA_SAGASHI;
messageBin.Strings[hash][0].Value = "Attack";
Rom.GetUSMessageBin().SetFile("common.bin", messageBin.ToByteArray());
