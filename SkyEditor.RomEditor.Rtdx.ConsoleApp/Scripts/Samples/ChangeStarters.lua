if rom == nil then
    print("Script must be run in the context of Sky Editor")
    do return end
end

local starters = rom:GetStarters()

-- Note: Pokemon types are based on the model being displayed, not the actual Pokemon type
-- Mew has a high-res model, so Psychic type is used
-- Riolu doesn't have a high-res model, so the Torchic model is used here, so the type is Fire, making Riolu unable to be used with other Fire type starters

local bulbasaur = starters:GetStarterById(Const.creature.Index.FUSHIGIDANE)
if bulbasaur ~= null then
    bulbasaur.PokemonId = Const.creature.Index.MYUU --Mew
    bulbasaur.Move1 = Const.waza.Index.HATAKU --Pound
    bulbasaur.Move2 = Const.waza.Index.TELEPORT --Teleport (obviously)
    bulbasaur.Move3 = Const.waza.Index.HENSHIN --Transform
    bulbasaur.Move4 = Const.waza.Index.IYASHINOSUZU --Heal bell
else
    print("Bulbasaur is not a starter. This ROM could already have had its starters replaced.")
end

local torchic = starters:GetStarterById(Const.creature.Index.ACHAMO)
if torchic ~= null then
    torchic.PokemonId = Const.creature.Index.RIORU --Riolu
else
    print("Torchic is not a starter. This ROM could already have had its starters replaced.")
end

local charmander = starters:GetStarterById(Const.creature.Index.HITOKAGE)
if charmander ~= null then
    charmander.PokemonId = Const.creature.Index.IWAAKU --Onix
else
    print("Charmander is not a starter. This ROM could already have had its starters replaced.")
end

local chikorita = starters:GetStarterById(Const.creature.Index.CHIKORIITA)
if chikorita ~= null then
    chikorita.PokemonId = Const.creature.Index.POCHIENA --Poochyena
else
    print("Chikorita is not a starter. This ROM could already have had its starters replaced.")
end
