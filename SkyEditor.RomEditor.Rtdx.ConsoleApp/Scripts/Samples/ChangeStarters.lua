if Rom == nil then
    error("Script must be run in the context of Sky Editor")
end

local starters = Rom:GetStarters()

local starter5 = starters:GetStarterById(Const.creature.Index.HITOKAGE --[[Charmander]])
if starter5 ~= nil then
    starter5.PokemonId = Const.creature.Index.IWAAKU --[[Onix]]
    starter5.Move1 = Const.waza.Index.TAIATARI --[[Tackle]]
    starter5.Move2 = Const.waza.Index.KATAKUNARU --[[Harden]]
    starter5.Move3 = Const.waza.Index.SHIMETSUKERU --[[Bind]]
    starter5.Move4 = Const.waza.Index.IWAOTOSHI --[[Rock Throw]]
else
    error("Could not find starter 'Charmander' with ID 5. This ROM may have already been modified.")
end

local starter1 = starters:GetStarterById(Const.creature.Index.FUSHIGIDANE --[[Bulbasaur]])
if starter1 ~= nil then
    starter1.PokemonId = Const.creature.Index.MYUU --[[Mew]]
    starter1.Move1 = Const.waza.Index.HATAKU --[[Pound]]
    starter1.Move2 = Const.waza.Index.TELEPORT --[[Teleport]]
    starter1.Move3 = Const.waza.Index.HENSHIN --[[Transform]]
    starter1.Move4 = Const.waza.Index.IYASHINOSUZU --[[Heal Bell]]
else
    error("Could not find starter 'Bulbasaur' with ID 1. This ROM may have already been modified.")
end

local starter10 = starters:GetStarterById(Const.creature.Index.ZENIGAME --[[Squirtle]])
if starter10 ~= nil then
    starter10.PokemonId = Const.creature.Index.BETOBETAA --[[Grimer]]
    starter10.Move1 = Const.waza.Index.HATAKU --[[Pound]]
    starter10.Move2 = Const.waza.Index.DOKUGAS --[[Poison Gas]]
    starter10.Move3 = Const.waza.Index.KATAKUNARU --[[Harden]]
    starter10.Move4 = Const.waza.Index.OIUCHI --[[Pursuit]]
else
    error("Could not find starter 'Squirtle' with ID 10. This ROM may have already been modified.")
end

local starter32 = starters:GetStarterById(Const.creature.Index.PIKACHUU --[[Pikachu]])
if starter32 ~= nil then
    starter32.PokemonId = Const.creature.Index.GAADI --[[Growlithe]]
    starter32.Move1 = Const.waza.Index.KAMITSUKU --[[Bite]]
    starter32.Move2 = Const.waza.Index.OTAKEBI --[[Noble Roar]]
    starter32.Move3 = Const.waza.Index.HINOKO --[[Ember]]
else
    error("Could not find starter 'Pikachu' with ID 32. This ROM may have already been modified.")
end

local starter204 = starters:GetStarterById(Const.creature.Index.HINOARASHI --[[Cyndaquil]])
if starter204 ~= nil then
    starter204.PokemonId = Const.creature.Index.DONMERU --[[Numel]]
    starter204.Move1 = Const.waza.Index.NAKIGOE --[[Growl]]
    starter204.Move2 = Const.waza.Index.TAIATARI --[[Tackle]]
    starter204.Move3 = Const.waza.Index.HINOKO --[[Ember]]
    starter204.Move4 = Const.waza.Index.KOROGARU --[[Rollout]]
else
    error("Could not find starter 'Cyndaquil' with ID 204. This ROM may have already been modified.")
end

local starter201 = starters:GetStarterById(Const.creature.Index.CHIKORIITA --[[Chikorita]])
if starter201 ~= nil then
    starter201.PokemonId = Const.creature.Index.POCHIENA --[[Poochyena]]
    starter201.Move1 = Const.waza.Index.TAIATARI --[[Tackle]]
    starter201.Move2 = Const.waza.Index.TOOBOE --[[Howl]]
    starter201.Move3 = Const.waza.Index.KAMITSUKU --[[Bite]]
    starter201.Move4 = Const.waza.Index.IRONTAIL --[[Iron Tail]]
else
    error("Could not find starter 'Chikorita' with ID 201. This ROM may have already been modified.")
end

local starter330 = starters:GetStarterById(Const.creature.Index.ACHAMO --[[Torchic]])
if starter330 ~= nil then
    starter330.PokemonId = Const.creature.Index.RIORU --[[Riolu]]
    starter330.Move2 = Const.waza.Index.SHINKUUHA --[[Vacuum Wave]]
    starter330.Move3 = Const.waza.Index.MUSHIKUI --[[Bug Bite]]
    starter330.Move4 = Const.waza.Index.KORAERU --[[Endure]]
else
    error("Could not find starter 'Torchic' with ID 330. This ROM may have already been modified.")
end

local starter65 = starters:GetStarterById(Const.creature.Index.KODAKKU --[[Psyduck]])
if starter65 ~= nil then
    starter65.PokemonId = Const.creature.Index.PONIITA --[[Ponyta]]
    starter65.Move1 = Const.waza.Index.TAIATARI --[[Tackle]]
    starter65.Move2 = Const.waza.Index.NAKIGOE --[[Growl]]
    starter65.Move3 = Const.waza.Index.SHIPPOWOFURU --[[Tail Whip]]
    starter65.Move4 = Const.waza.Index.HINOKO --[[Ember]]
else
    error("Could not find starter 'Psyduck' with ID 65. This ROM may have already been modified.")
end

local starter123 = starters:GetStarterById(Const.creature.Index.KARAKARA --[[Cubone]])
if starter123 ~= nil then
    starter123.PokemonId = Const.creature.Index.JIGUZAGUMA --[[Zigzagoon]]
    starter123.Move1 = Const.waza.Index.TAIATARI --[[Tackle]]
    starter123.Move2 = Const.waza.Index.NAKIGOE --[[Growl]]
    starter123.Move3 = Const.waza.Index.SHIPPOWOFURU --[[Tail Whip]]
    starter123.Move4 = Const.waza.Index.OIUCHI --[[Pursuit]]
else
    error("Could not find starter 'Cubone' with ID 123. This ROM may have already been modified.")
end

local starter63 = starters:GetStarterById(Const.creature.Index.NYAASU --[[Meowth]])
if starter63 ~= nil then
    starter63.PokemonId = Const.creature.Index.RARUTOSU --[[Ralts]]
    starter63.Move1 = Const.waza.Index.NENRIKI --[[Confusion]]
    starter63.Move2 = Const.waza.Index.NAKIGOE --[[Growl]]
    starter63.Move3 = Const.waza.Index.KAGEUCHI --[[Shadow Sneak]]
    starter63.Move4 = Const.waza.Index.KAGEBUNSHIN --[[Double Team]]
else
    error("Could not find starter 'Meowth' with ID 63. This ROM may have already been modified.")
end

local starter79 = starters:GetStarterById(Const.creature.Index.WANRIKII --[[Machop]])
if starter79 ~= nil then
    starter79.PokemonId = Const.creature.Index.KEESHII --[[Abra]]
    starter79.Move1 = Const.waza.Index.TELEPORT --[[Teleport]]
    starter79.Move2 = Const.waza.Index.PSYCHOSHOCK --[[Psyshock]]
    starter79.Move3 = Const.waza.Index.IRONTAIL --[[Iron Tail]]
    starter79.Move4 = Const.waza.Index.KORAERU --[[Endure]]
else
    error("Could not find starter 'Machop' with ID 79. This ROM may have already been modified.")
end

