if rom == nil then
    error("Script must be run in the context of Sky Editor")
end

local starters = rom:GetStarters().Starters
for i = 0,starters.Length-1,1
do
    local starter = starters[i]
    print(i, starter.PokemonName)
end
