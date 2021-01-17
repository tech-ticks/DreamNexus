#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

public void Print(MessageBinEntry messageBin)
{
    foreach (var entry in messageBin.Strings)
    {
        if (entry.Value.Count == 1)
        {
            Console.WriteLine($"{(uint)entry.Key:x8}: \"{entry.Value[0].Value}\"");
        }
        else
        {
            Console.WriteLine($"{(uint)entry.Key:x8}:");
            foreach (var str in entry.Value)
            {
                Console.WriteLine($"  \"{str.Value}\"");
            }
        }
    }
}

Console.WriteLine("-------- Common messages");
Print(Rom.GetCommonBinEntry());

Console.WriteLine();
Console.WriteLine("-------- Dungeon messages");
Print(Rom.GetDungeonBinEntry());

Console.WriteLine();
Console.WriteLine("-------- Script messages");
Print(Rom.GetScriptBinEntry());

Console.WriteLine();
Console.WriteLine("-------- Debug messages");
Print(Rom.GetDebugBinEntry());

Console.WriteLine();
Console.WriteLine("-------- Test messages");
Print(Rom.GetTestBinEntry());
