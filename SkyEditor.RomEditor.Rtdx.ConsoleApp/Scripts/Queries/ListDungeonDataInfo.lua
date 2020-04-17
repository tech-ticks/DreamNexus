if rom == nil then
    print("Script must be run in the context of Sky Editor")
    do return end
end

luanet.load_assembly("SkyEditor.RomEditor.Rtdx")
DungeonFeature = luanet.import_type("SkyEditor.RomEditor.Rtdx.Domain.Structures.DungeonDataInfo").DungeonDataInfoEntry.Feature
Convert = luanet.import_type("System.Convert")

-- Formatters ------------------------------

local function int2bin(n)
    return Convert.ToString(Convert.ChangeType(n, n:GetTypeCode()), 2)
end

local function formatFloors(dungeon)
    local prefix = ""
    if dungeon.Data.Features:HasFlag(DungeonFeature.FloorDirection) then
        prefix = ""
    else
        prefix = "B"
    end
    return prefix .. (dungeon.Extra and dungeon.Extra.Floors or "--") .. "F"
end

local function formatTeammates(teammates)
    if teammates > 1 then return "yes" else return "no" end
end

local function formatItems(items)
    if items > 0 then return "yes" else return "no" end
end

local function formatFeature(features, feature)
    if features:HasFlag(feature) then return "yes" else return "no" end
end

local function formatFeatures(features)
    local featureNames = {}
    if not features:HasFlag(DungeonFeature.AutoRevive) then table.insert(featureNames, "Auto-revive") end
    if features:HasFlag(DungeonFeature.Scanning) then table.insert(featureNames, "Scanning") end
    if features:HasFlag(DungeonFeature.Radar) then table.insert(featureNames, "Radar") end
    return table.concat(featureNames, ", ")
end

local function formatFeaturesBits(features)
    return string.gsub(string.gsub(string.format("%019s", int2bin(features)), "0", "-"), "1", "#")
end

--------------------------------------------

print("#    Index   Dungeon                    Floors   Teammates   Items   Level reset   Recruitable   Features                       210FEDCBA9876543210   0x08   0x0A   d_balance   0x13   0x17   0x18   0x19")

local dungeons = rom:GetDungeons().Dungeons
for i = 1,dungeons.Length-1,1
do
    local dungeon = dungeons[i]
    local data = dungeon.Data
    local extra = dungeon.Extra
    print(string.format("%-4d %5d   %-28s %4s      %3s       %3s        %3s           %3s       %-30s %s    %3d    %3d      %3d       %3d    %3d    %3d    %3d",
            i,
            data.Index,
            dungeon.DungeonName,
            formatFloors(dungeon),
            formatTeammates(data.MaxTeammates),
            formatItems(data.MaxItems),
            formatFeature(data.Features, DungeonFeature.LevelReset),
            formatFeature(data.Features, DungeonFeature.WildPokemonRecruitable),
            formatFeatures(data.Features),
            formatFeaturesBits(data.Features),
            data.Short08,
            data.Short0A,
            data.DungeonBalanceIndex,
            data.Byte13,
            data.Byte17,
            data.Byte18,
            data.Byte19
        )
    )
end
