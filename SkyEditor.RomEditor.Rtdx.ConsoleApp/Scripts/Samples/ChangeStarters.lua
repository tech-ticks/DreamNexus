if rom == nil then
    error("Script must be run in the context of Sky Editor")
end

local starters = rom:GetStarters()

-- Note: Pokemon types are based on the model being displayed, not the actual Pokemon type
-- Mew has a high-res model, so Psychic type is used
-- Riolu doesn't have a high-res model, so the Torchic model is used here, so the type is Fire, making Riolu unable to be used with other Fire type starters

local starter0 = starters:GetStarterById(Const.creature.Index.HITOKAGE --[[Charmander]])
if starter0 ~= null then
    starter0.PokemonId = Const.creature.Index.IWAAKU --[[Onix]]
else
    error("Could not find starter 'Charmander' with ID 5. This ROM may have already been modified.")
end

local starter1 = starters:GetStarterById(Const.creature.Index.FUSHIGIDANE --[[Bulbasaur]])
if starter1 ~= null then
    starter1.PokemonId = Const.creature.Index.MYUU --[[Mew]]
    starter1.Move1 = Const.waza.Index.HATAKU --[[Pound]]
    starter1.Move2 = Const.waza.Index.TELEPORT --[[Teleport]]
    starter1.Move3 = Const.waza.Index.HENSHIN --[[Transform]]
    starter1.Move4 = Const.waza.Index.IYASHINOSUZU --[[Heal Bell]]
else
    error("Could not find starter 'Bulbasaur' with ID 1. This ROM may have already been modified.")
end

local starter5 = starters:GetStarterById(Const.creature.Index.CHIKORIITA --[[Chikorita]])
if starter5 ~= null then
    starter5.PokemonId = Const.creature.Index.POCHIENA --[[Poochyena]]
else
    error("Could not find starter 'Chikorita' with ID 201. This ROM may have already been modified.")
end

local starter7 = starters:GetStarterById(Const.creature.Index.ACHAMO --[[Torchic]])
if starter7 ~= null then
    starter7.PokemonId = Const.creature.Index.RIORU --[[Riolu]]
else
    error("Could not find starter 'Torchic' with ID 330. This ROM may have already been modified.")
end

