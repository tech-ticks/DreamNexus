if rom == nil then
    print("Script must be run in the context of Sky Editor")
    do return end
end

local starters = rom:QueryStarters()
for i = 0,starters.Length-1,1
do
    local starter = starters[i]
    print(i, starter.PokemonName)
end
