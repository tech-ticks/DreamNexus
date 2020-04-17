if rom == nil then
    print("Script must be run in the context of Sky Editor")
    do return end
end

local entries = rom:GetPokemonGraphicsDatabase().Entries
for i = 0,entries.Count-1,1
do
    local entry = entries[i]
    --[[print(i,
        entry.AvatarAssetName,
        entry.AnimationsAssetName,
        entry.ParentAvatarAssetName,
        entry.PortraitsAssetName,
        entry.RescueCampSpritesLeftAssetName,
        entry.RescueCampSpritesRightAssetName,
        string.format("0x%x", entry.FacialExpressionsMask)
    )]]--
    --print(i, entry.BodySizeType, string.format("0x%x", entry.Int6C), string.format("0x%x", entry.Int70), entry.Int78, string.format("0x%x", entry.FacialExpressionsMask), entry.PortraitsAssetName)
    print(i,
        string.format("%25s", entry.AvatarAssetName),
        --string.format("%25s", entry.AnimationsAssetName),
        --string.format("%25s", entry.ParentAvatarAssetName),
        --string.format("%20s", entry.PortraitsAssetName),
        --string.format("%30s", entry.RescueCampSpritesLeftAssetName),
        --[[string.format("%.02f", entry.Float30),
        string.format("%.02f", entry.Float34),
        string.format("%.02f", entry.Float38),
        string.format("%.02f", entry.Float3C),
        string.format("%.02f", entry.Float40),
        string.format("%.02f", entry.Float44),
        string.format("%.03f", entry.Float48),
        string.format("%.02f", entry.fWalkSpeed),
        string.format("%.02f", entry.Float50),
        string.format("%.02f", entry.fRunRateGround),
        string.format("%.01f", entry.Float58),
        string.format("%.01f", entry.Float5C),
        string.format("%.02f", entry.Float60),
        string.format("%.02f", entry.Float64),
        string.format("%.0f", entry.Float80),
        string.format("%.01f", entry.Float84),
        string.format("%.01f", entry.Float88),
        string.format("%.02f", entry.Float8C),
        string.format("%.02f", entry.Float90),
        string.format("%.02f", entry.Float94),
        string.format("%.02f", entry.Float98),
        string.format("%.02f", entry.Float9C),
        string.format("%.02f", entry.FloatA0),]]--
        entry.BodySizeType,
        entry.Int6C,
        string.format("0x%x", entry.Flags),
        entry.EvolutionCamType
    )
end
