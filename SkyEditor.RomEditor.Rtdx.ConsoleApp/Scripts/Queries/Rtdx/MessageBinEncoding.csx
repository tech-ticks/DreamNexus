#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;

var codeTable = Rom.GetCodeTable();
var messageBin = new MessageBinEntry(Rom.GetUSMessageBin().GetFile("script.bin"));

string encodedText = messageBin.GetStringByHash((int) Crc32Hasher.Crc32Hash("SCRIPT__B01P01A_M01E01A_3_02_0360"));
Console.WriteLine(encodedText);

string decodedText = codeTable.UnicodeDecode(encodedText);
Console.WriteLine(decodedText);
Console.WriteLine(codeTable.UnicodeEncode(decodedText));

encodedText = codeTable.UnicodeEncode("Hello [hero] and [partner], press the [M:B03] button to evolve. This should stay intact: [test]");
Console.WriteLine(encodedText);
Console.WriteLine(codeTable.UnicodeDecode(encodedText));
