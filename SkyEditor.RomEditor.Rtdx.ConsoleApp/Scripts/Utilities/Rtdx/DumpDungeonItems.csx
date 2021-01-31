// This script dumps all dungeon items into individual CSV files per dungeon in the current working directory.
#load "../../../Stubs/RTDX.csx"

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

var commonStrings = Rom.GetCommonStrings();
var dungeons = Rom.GetDungeons();
var itemDataInfo = Rom.GetItemDataInfo();

void PrintItemSet(StringBuilder sb, ItemArrange.Entry.ItemSet itemSet)
{
  sb.AppendLine("Item Kind;Weight;Probability");
  
  int itemKindWeightSum = itemSet.ItemKindWeights.Sum(weight => weight);
  for (int i = 0; i < itemSet.ItemKindWeights.Length; i++)
  {
    ushort weight = itemSet.ItemKindWeights[i];
    float probability = ((float) weight / itemKindWeightSum) * 100f;

    // Percentage sign prevents Excel from formatting the probability as a date
    sb.AppendLine($"{((ItemKind) i).GetFriendlyName()};{weight};{probability:0.00}%");
  }

  sb.AppendLine();
  sb.AppendLine("Item ID;Item Name;Group;Weight;Probability in group");

  // For a correct probability, we need to calculate the sum of all items of the same kind
  var perKindProbabilitySums = new Dictionary<ItemKind, int>();
  for (int i = 0; i < (int) ItemKind.MAX; i++)
  {
    var itemKind = (ItemKind) i;
    perKindProbabilitySums.Add(itemKind, itemSet.ItemWeights
      .Where(otherItem => itemDataInfo.Entries[(int) otherItem.Index].ItemKind == itemKind)
      .Sum(otherItem => otherItem.Weight));
  }

  // Print the item probabilities
  for (int i = 0; i < itemSet.ItemWeights.Count; i++)
  {
    var entry = itemSet.ItemWeights[i];
    var itemKind = itemDataInfo.Entries[(int) entry.Index].ItemKind;

    float probabilityInGroup = ((float) entry.Weight / perKindProbabilitySums[itemKind]) * 100f;
    string itemName = commonStrings.GetItemName(entry.Index);
    sb.AppendLine($"{entry.Index};{itemName};{itemKind.GetFriendlyName()};{entry.Weight};{probabilityInGroup:0.00}%");
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

  sb.AppendLine($"Item set count;{itemSets.Count}");

  for (int i = 0; i < itemSets.Count; i++)
  {
    sb.AppendLine();
    sb.AppendLine($"Item set;{i}");
    sb.AppendLine();

    PrintItemSet(sb, itemSets[i]);
  }

  string fileName = $"{dungeon.Id}_{dungeon.DungeonName}.csv";
  File.WriteAllText(fileName, sb.ToString());
}
