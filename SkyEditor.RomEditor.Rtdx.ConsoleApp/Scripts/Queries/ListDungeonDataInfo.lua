if rom == nil then
    print("Script must be run in the context of Sky Editor")
    do return end
end

-- Helper functions ------------------------

local hex2bin = {
    ['0'] = '0000',
    ['1'] = '0001',
    ['2'] = '0010',
    ['3'] = '0011',
    ['4'] = '0100',
    ['5'] = '0101',
    ['6'] = '0110',
    ['7'] = '0111',
    ['8'] = '1000',
    ['9'] = '1001',
    ['a'] = '1010',
    ['b'] = '1011',
    ['c'] = '1100',
    ['d'] = '1101',
    ['e'] = '1110',
    ['f'] = '1111'
}
local function getHex2bin(a) return hex2bin[a] end
local function int2bin(n)
    local s = string.format('%x', n)
    s = s:gsub('.', getHex2bin)
    return s
end

function bit(p)
    return 2 ^ p
end

-- Typical call:  if hasbit(x, bit(3)) then ...
local function hasbit(x, p)
    return x % (p + p) >= p       
end

-- Formatters ------------------------------

local function formatFloors(dungeon)
    local prefix = ""
    if hasbit(dungeon.Data.Flags, bit(0)) then
        prefix = ""
    else
        prefix = "B"
    end
    return prefix .. (dungeon.Extra and dungeon.Extra.Floors or 0) .. "F"
end

local function formatTeammates(teammates)
    if teammates > 1 then return "yes" else return "no" end
end

local function formatItems(items)
    if items > 0 then return "yes" else return "no" end
end

local function formatRecruitable(flags)
    if hasbit(flags, bit(15)) then return "yes" else return "no" end
end

local function formatFeatures(dungeon)
    local flags = dungeon.Data.Flags
    local features = {}
    if hasbit(flags, bit(17)) then table.insert(features, "Scanning") end
    if hasbit(flags, bit(18)) then table.insert(features, "Radar") end
    return table.concat(features, ", ")
end

--------------------------------------------

print("#    Index   Dungeon                    Floors   Teammates   Items   Recruitable   Features")

local dungeons = rom:GetDungeons().Dungeons
for i = 0,dungeons.Length-1,1
do
    local dungeon = dungeons[i]
    local data = dungeon.Data
    local extra = dungeon.Extra
    print(string.format("%-4d %5d   %-28s %4s      %3s       %3s        %3s       %-30s",
            i,
            data.Index,
            dungeon.DungeonName,
            formatFloors(dungeon),
            formatTeammates(data.MaxTeammates),
            formatItems(data.MaxItems),
            formatRecruitable(data.Flags),
            formatFeatures(dungeon)
        ),
        string.format("%020s", int2bin(data.Flags)),
        data.Short08,
        data.Short0A,
        data.DungeonBalanceIndex,
        string.format("0x%x", data.Byte13),
        string.format("0x%x", data.Byte17),
        data.Byte18,
        data.Byte19
    )
end
