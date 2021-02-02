#load "../../../Stubs/Rtdx.csx"

using System;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

var ranks = Rom.GetRanks().Entries;
var commonStrings = Rom.GetCommonStrings();

Console.WriteLine($"#   {"Name",-12}  {"MinPoints",9}  {"ToolboxSize",11}  {"CampCapacity",11}  {"TeamPresets",11}  {"JobLimit",11} {"Short0C",11}  RewardStatue");
for (int i = 0; i < (int) RankIndex.RANK_END; i++)
{
    var rankIndex = (RankIndex) i;
    var rank = ranks[rankIndex];
    Console.WriteLine($"{i,-2}  "
        + $"{commonStrings.Ranks[rankIndex],-12}  "
        + $"{rank.MinPoints,9}  "
        + $"{rank.ToolboxSize,11}  "
        + $"{rank.CampCapacity,11}  "
        + $"{rank.TeamPresets,11}  "
        + $"{rank.JobLimit,11}  "
        + $"{rank.Short0C,11}  "
        + $"{rank.RewardStatue}  "
    );
}
