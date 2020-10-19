#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;

var codeTable = Rom.GetCodeTable();
var messageBin = new MessageBinEntry(Rom.GetUSMessageBin().GetFile("script.bin"));

string encodedText = messageBin.GetStringByHash((int) Crc32Hasher.Crc32Hash("SCRIPT__PEGID_HANYOU_OHAYOUKURINOMI__8384"));
Console.WriteLine(encodedText);

string decodedText = codeTable.UnicodeDecode(encodedText);
Console.WriteLine(decodedText);
Console.WriteLine(codeTable.UnicodeEncode(decodedText));

encodedText = codeTable.UnicodeEncode("Hello [hero] and [partner], press the [M:B03] button to evolve with a [item:226]. This should stay intact: [test]");
Console.WriteLine(encodedText);
Console.WriteLine(codeTable.UnicodeDecode(encodedText));
