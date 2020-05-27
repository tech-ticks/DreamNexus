if rom == nil then
    error("`rom` is not accessible")
end

local starters = rom:GetStarters().Starters

local riolu = starters[0]
if riolu.PokemonId ~= Const.creature.Index.RIORU then error("Failed to read first starter's PokemonId") end
if riolu.PokemonName ~= "Riolu" then error("Failed to read first starter's PokemonId") end

local mew = starters[1]
if mew.PokemonId ~= Const.creature.Index.MYUU then error("Failed to read second starter's PokemonId") end
if mew.PokemonName ~= "Mew" then error("Failed to read second starter's PokemonId") end

passed = true
