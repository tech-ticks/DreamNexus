#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

void Print(Sir0StringList stringList)
{
    for (int i = 0; i < stringList.Entries.Count; i++)
    {
        Console.WriteLine($"{i}: {stringList.Entries[i]}");
    }
}

Console.WriteLine("-------- DungeonMapSymbol");
Print(Rom.GetDungeonMapSymbol());

Console.WriteLine();
Console.WriteLine("-------- DungeonBgmSymbol");
Print(Rom.GetDungeonBgmSymbol());

Console.WriteLine();
Console.WriteLine("-------- DungeonSeSymbol");
Print(Rom.GetDungeonSeSymbol());

Console.WriteLine();
Console.WriteLine("-------- EffectSymbol");
Print(Rom.GetEffectSymbol());
