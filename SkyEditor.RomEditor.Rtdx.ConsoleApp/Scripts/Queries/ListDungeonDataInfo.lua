if rom == nil then
    print("Script must be run in the context of Sky Editor")
    do return end
end

luanet.load_assembly("SkyEditor.RomEditor.Rtdx")
DungeonFeature = luanet.import_type("SkyEditor.RomEditor.Rtdx.Domain.Structures.DungeonDataInfo").DungeonDataInfoEntry.Feature
Convert = luanet.import_type("System.Convert")

local commonStrings = rom:GetCommonStrings()

-- Formatters ------------------------------

local function enumToInt(value)
    return Convert.ChangeType(value, value:GetTypeCode())
end

local function getPokemonName(id)
    return commonStrings.Pokemon:ContainsKey(id) and commonStrings.Pokemon[id] or ("(Unknown :" .. id .. ")")
end

local function int2bin(n)
    return Convert.ToString(enumToInt(n), 2)
end

local function formatFloor(dungeon, floor)
    local prefix = ""
    if dungeon.Data.Features:HasFlag(DungeonFeature.FloorDirection) then
        prefix = ""
    else
        prefix = "B"
    end
    return prefix .. floor .. "F"
end

local function formatFloors(dungeon)
    return formatFloor(dungeon, dungeon.Extra and dungeon.Extra.Floors or "--")
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

--print("#    Index   Dungeon                    Floors")
print("#    Index   Dungeon                    Floors   Teammates   Items   Level reset   Recruitable   Features                       210FEDCBA9876543210   0x08   0x0A   d_balance   0x13   0x17   0x18   0x19")
local fixedPokemon = rom:GetFixedPokemon().Entries
local dungeons = rom:GetDungeons().Dungeons
for i = 1,dungeons.Length-1,1
do
    local dungeon = dungeons[i]
    local data = dungeon.Data
    local extra = dungeon.Extra
    local balance = dungeon.Balance
    local floorInfos = balance.FloorInfos

    -- Basic dungeon info
    --[[print(string.format("%-4d %5d   %-28s %4s",
            i,
            data.Index,
            dungeon.DungeonName,
            formatFloors(dungeon)
        )
    )]]--

    -- Complete dungeon info
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

    -- Info per floor
    --[[for j = 0,floorInfos.Length-1,1
    do
        local info = floorInfos[j]
        print(string.format("   %5d  %5d  %5d  %5d  %5d  %5d  %3d  %3d  %3d  %3d  %5d  %5d  %3d  %3d  %3d  %s  %s",
                info.Index,
                info.InvitationIndex,
                info.Short02,
                info.Short24,
                info.Short26,
                info.Short28,
                info.Short2A,
                info.Byte2C,
                info.Byte2D,
                info.Byte2E,
                info.Byte2F,
                info.Short30,
                info.Short32,
                info.Byte34,
                info.Byte35,
                info.Byte36,
                info.Bytes37to61AsString,
                info.Event
            )
        )
    end]]--

    -- Fainted Pokemon
    local faintedPokemon = {}
    for j = 0,fixedPokemon.Count-1,1
    do
        local entry = fixedPokemon[j]
        if enumToInt(entry.DungeonIndex) == data.Index then
            table.insert(faintedPokemon, getPokemonName(entry.PokemonId))
        end
    end
    if #faintedPokemon > 0 then print(string.format("  Fainted Pokemon: %s", table.concat(faintedPokemon, ", "))) end

    -- Pokemon in Mystery Houses
    local prevInvitationIndex = -1
    local mysteryHousePokemon = {}
    for j = 0,floorInfos.Length-1,1
    do
        local info = floorInfos[j]
        if info.InvitationIndex ~= 0 and prevInvitationIndex ~= info.InvitationIndex then
            prevInvitationIndex = info.InvitationIndex
            for k = 0,fixedPokemon.Count-1,1
            do
                local entry = fixedPokemon[k]
                if entry.InvitationIndex == info.InvitationIndex then
                    table.insert(mysteryHousePokemon, getPokemonName(entry.PokemonId))
                end
            end
        end
    end
    if #mysteryHousePokemon > 0 then print(string.format("  Mystery House Pokemon: %s", table.concat(mysteryHousePokemon, ", "))) end

    -- Wild Pokemon
    local wildPokemon = balance.WildPokemon
    if wildPokemon ~= nil then
        print("      #   Pokemon         Lvl    HP   Atk   Def   SpA   SpD   Spe    XP Yield")
        print("             Spawn  Recruit")
        print("      Floor   rate   level   0x0B")
        local stats = wildPokemon.Stats
        local floors = wildPokemon.Floors
        for j = 0,stats.Length-1,1
        do
            local r = stats[j]
            local index = r.Index + 1
            local name = getPokemonName(r.CreatureIndex)
            if r.XPYield ~= 0 or r.HitPoints ~= 0 or r.Attack ~= 0 or r.Defense ~= 0 or r.SpecialAttack ~= 0 or r.SpecialDefense ~= 0 or r.Speed ~= 0 or r.Level ~= 0 then
                local strongFoe = ""
                if r.StrongFoe ~= 0 then strongFoe = "Strong Foe" end
                print(string.format("   %4d   %-14s  %3d   %3d   %3d   %3d   %3d   %3d   %3d    %8d   %s",
                        index,
                        name,
                        r.Level,
                        r.HitPoints,
                        r.Attack,
                        r.Defense,
                        r.SpecialAttack,
                        r.SpecialDefense,
                        r.Speed,
                        r.XPYield,
                        strongFoe
                    )
                )

                for k = 0,dungeon.Extra.Floors-1,1
                do
                    local ent = floors[k].Entries[j]
                    if ent.SpawnRate ~= 0 then
                        print(string.format("       %4s    %3d    %3d    %3d",
                                formatFloor(dungeon, k + 1),
                                ent.SpawnRate,
                                ent.RecruitmentLevel,
                                ent.Byte0B
                            )
                        )
                    end
                end
            end
        end
    end

    -- Unknown data from the third SIR0 file in dungeon_balance.bin
    --[[local data3 = balance.Data3
    if data3 ~= nil then
        local recs = data3.Records
        local prevIndex = -1
        local prevShort02s = {}
        local len = dungeon.Extra.Floors  --recs.Length - 1
        for j = 0,len-1,1
        do
            local rec = recs[j]
            local ents = rec.Entries
            local short02s = {}

            for k = 0,ents.Length-1,1
            do
                local ent = ents[k]
                table.insert(short02s, ent.Short02)
            end

            if #prevShort02s == 0 or table.concat(prevShort02s) ~= table.concat(short02s) then
                prevShort02s = short02s
                if prevIndex ~= -1 and prevIndex ~= j then print(".." .. formatFloor(dungeon, j) .. ": *") end
                print("  " .. formatFloor(dungeon, j + 1) .. ": " .. table.concat(short02s, "  "))
                prevIndex = j + 1
            end
        end
        if prevIndex ~= -1 and prevIndex ~= len then print(".." .. formatFloor(dungeon, len) .. ": *") end
    end]]--

    -- Unknown (and mostly uninteresting) data from the fourth SIR0 file in dungeon_balance.bin
    --[[local data4 = balance.Data4
    if data4 ~= nil then
        local recs = data4.Records
        for j = 0,recs.Length-1,1
        do
            local rec = recs[j]
            local ents = rec.Entries
            local short00s = {}
            local short02s = {}
            local int04s = {}

            local has02 = false
            local has04 = false
            for k = 0,ents.Length-1,1
            do
                local ent = ents[k]
                table.insert(short00s, ent.Short00)
                table.insert(short02s, ent.Short02)
                table.insert(int04s, ent.Int04)
                if ent.Short02 ~= 60 then has02 = true end
                if ent.Int04 ~= 0 then has04 = true end
            end

            print("  " .. formatFloor(dungeon, j + 1) .. ": " .. table.concat(short00s, "  "))
            print("     " .. table.concat(short02s, "  "))
            print("     " .. table.concat(int04s, "  "))
        end
    end]]--
end
