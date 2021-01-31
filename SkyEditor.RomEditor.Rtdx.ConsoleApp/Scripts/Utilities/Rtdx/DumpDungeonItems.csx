// This script dumps all dungeon items into individual CSV files per dungeon in the current working directory.
#load "../../../Stubs/RTDX.csx"

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

var commonStrings = Rom.GetCommonStrings();
var dungeons = Rom.GetDungeons();
var itemDataInfo = Rom.GetItemDataInfo();

(short[] validFloors, short[] invalidFloors) GetItemSetFloors(int itemSetIndex, IDungeonModel dungeon)
{
  var allFloors =  dungeon.Balance.FloorInfos
    .Where(floorInfo => floorInfo.Byte36 == itemSetIndex) // All floors with the item set index
    .Select(floorInfo => floorInfo.Index);

  var validFloors = allFloors.Where(index => index > 0) // Floors 0 and -1 are invalid
    .Where(index => index <= dungeon.Extra.Floors) // Floors that are higher than the floor count are invalid
    .ToArray();

  var invalidFloors = allFloors.Except(validFloors).ToArray();

  return (validFloors, invalidFloors);
}

void PrintItemSet(StringBuilder sb, int itemSetIndex, ItemArrange.Entry.ItemSet itemSet, IDungeonModel dungeon)
{
  var (validFloors, invalidFloors) = GetItemSetFloors(itemSetIndex, dungeon);
  if (validFloors.Length == 0 && invalidFloors.Length == 0) {
    // Don't include unused item sets
    return;
  }

  sb.AppendLine();
  sb.AppendLine($"Item set;{itemSetIndex}");
  sb.AppendLine($"Valid floors with item set;{(validFloors.Length == 0 ? "none" : string.Join("; ", validFloors))}");
  sb.AppendLine($"Invalid floors with item set;{(invalidFloors.Length == 0 ? "none" : string.Join("; ", invalidFloors))}");

  sb.AppendLine();

  // For a correct probability, we need to calculate the sum of all items of the same kind
  int itemKindWeightSum = itemSet.ItemKindWeights.Sum(weight => weight);
  var perKindProbabilitySums = new Dictionary<ItemKind, int>();
  for (int i = 0; i < (int) ItemKind.MAX; i++)
  {
    var itemKind = (ItemKind) i;
    perKindProbabilitySums.Add(itemKind, itemSet.ItemWeights
      .Where(otherItem => itemDataInfo.Entries[(int) otherItem.Index].ItemKind == itemKind)
      .Sum(otherItem => otherItem.Weight));
  }

  sb.AppendLine("Item ID;Item Name;Group;Probability");

  // Print the item probabilities
  for (int i = 0; i < itemSet.ItemWeights.Count; i++)
  {
    var entry = itemSet.ItemWeights[i];
    var itemKind = itemDataInfo.Entries[(int) entry.Index].ItemKind;

    float probabilityInGroup = (float) entry.Weight / perKindProbabilitySums[itemKind];
    ushort groupWeight = itemSet.ItemKindWeights[(int) itemKind];
    float groupProbability = (float) groupWeight / itemKindWeightSum;
    float totalProbability = probabilityInGroup * groupProbability * 100f;

    if (totalProbability == 0f)
    {
      // Don't include items with zero probability
      continue;
    }

    string itemName = commonStrings.GetItemName(entry.Index);
    sb.AppendLine($"{entry.Index};{itemName};{itemKind.GetFriendlyName()};{totalProbability:0.00}%");
  }
}

foreach (var dungeon in dungeons.Dungeons)
{
  if (dungeon.Extra == null)
  {
    continue;
  }

  var sb = new StringBuilder();
  var itemSets = dungeon.ItemArrange.ItemSets;

  for (int i = 0; i < itemSets.Count; i++)
  {
    PrintItemSet(sb, i, itemSets[i], dungeon);
  }

  string fileName = $"{dungeon.Id}_{dungeon.DungeonName}.csv";
  File.WriteAllText(fileName, sb.ToString());
}
