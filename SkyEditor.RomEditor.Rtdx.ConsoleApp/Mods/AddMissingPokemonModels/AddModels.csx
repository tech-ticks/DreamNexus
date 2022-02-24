#load "../../Stubs/RTDX.csx"

using System;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

if (Mod == null)
{
    throw new InvalidOperationException("Script must be run in the context of a mod");
}

var pokemon = Rom.GetPokemon();
var graphics = Rom.GetPokemonGraphics();

PokemonGraphicsModel GetGraphicsModelByCreatureAndForm(CreatureIndex creatureIndex, PokemonFormType formType)
{
    int graphicsDatabaseId = pokemon.GetPokemonById(creatureIndex).PokemonGraphicsDatabaseEntryIds[(int) formType];
    return graphics.GetEntryById(graphicsDatabaseId);
}

PokemonGraphicsModel graphicsEntry;

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NAETORU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "naetoru_00";
graphicsEntry.AnimationName = "4leg_naetoru_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/naetoru_00.ab", Mod.ReadResourceArray("Resources/naetoru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HAYASHIGAME, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hayashigame_00";
graphicsEntry.AnimationName = "4leg_doragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hayashigame_00.ab", Mod.ReadResourceArray("Resources/hayashigame_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DODAITOSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dodaitosu_00";
graphicsEntry.AnimationName = "4leg_doragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dodaitosu_00.ab", Mod.ReadResourceArray("Resources/dodaitosu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HIKOZARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hikozaru_00";
graphicsEntry.AnimationName = "2leg_hikozaru_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hikozaru_00.ab", Mod.ReadResourceArray("Resources/hikozaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MOUKAZARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "moukazaru_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/moukazaru_00.ab", Mod.ReadResourceArray("Resources/moukazaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GOUKAZARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "goukazaru_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/goukazaru_00.ab", Mod.ReadResourceArray("Resources/goukazaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.POTCHAMA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "potchama_00";
graphicsEntry.AnimationName = "2leg_potchama_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/potchama_00.ab", Mod.ReadResourceArray("Resources/potchama_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.POTTAISHI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "pottaishi_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/pottaishi_00.ab", Mod.ReadResourceArray("Resources/pottaishi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ENPERUTO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "enperuto_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/enperuto_00.ab", Mod.ReadResourceArray("Resources/enperuto_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MUKKURU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mukkuru_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mukkuru_00.ab", Mod.ReadResourceArray("Resources/mukkuru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MUKUBAADO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mukubaado_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mukubaado_00.ab", Mod.ReadResourceArray("Resources/mukubaado_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MUKUHOOKU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mukuhooku_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mukuhooku_00.ab", Mod.ReadResourceArray("Resources/mukuhooku_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BIPPA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "bippa_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/bippa_00.ab", Mod.ReadResourceArray("Resources/bippa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BIIDARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "biidaru_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/biidaru_00.ab", Mod.ReadResourceArray("Resources/biidaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOROBOOSHI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "korobooshi_00";
graphicsEntry.AnimationName = "2leg_beast_up_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/korobooshi_00.ab", Mod.ReadResourceArray("Resources/korobooshi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOROTOKKU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "korotokku_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/korotokku_00.ab", Mod.ReadResourceArray("Resources/korotokku_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KORINKU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "korinku_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/korinku_00.ab", Mod.ReadResourceArray("Resources/korinku_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.RUKUSHIO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "rukushio_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/rukushio_00.ab", Mod.ReadResourceArray("Resources/rukushio_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.RENTORAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "rentoraa_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/rentoraa_00.ab", Mod.ReadResourceArray("Resources/rentoraa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ZUGAIDOSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "zugaidosu_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/zugaidosu_00.ab", Mod.ReadResourceArray("Resources/zugaidosu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.RAMUPARUDO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "ramuparudo_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/ramuparudo_00.ab", Mod.ReadResourceArray("Resources/ramuparudo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TATETOPUSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "tatetopusu_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/tatetopusu_00.ab", Mod.ReadResourceArray("Resources/tatetopusu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TORIDEPUSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "toridepusu_00";
graphicsEntry.AnimationName = "4leg_doragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/toridepusu_00.ab", Mod.ReadResourceArray("Resources/toridepusu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MINOMUTCHI_GRASS, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "minomutchi_g_00";
graphicsEntry.AnimationName = "body_minomutchi_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/minomutchi_g_00.ab", Mod.ReadResourceArray("Resources/minomutchi_g_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MINOMUTCHI_SAND, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "minomutchi_s_00";
graphicsEntry.AnimationName = "body_minomutchi_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/minomutchi_s_00.ab", Mod.ReadResourceArray("Resources/minomutchi_s_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MINOMUTCHI_DUST, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "minomutchi_d_00";
graphicsEntry.AnimationName = "body_minomutchi_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/minomutchi_d_00.ab", Mod.ReadResourceArray("Resources/minomutchi_d_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MINOMADAMU_GRASS, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "minomadamu_g_00";
graphicsEntry.AnimationName = "body_minomutchi_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/minomadamu_g_00.ab", Mod.ReadResourceArray("Resources/minomadamu_g_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MINOMADAMU_SAND, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "minomadamu_s_00";
graphicsEntry.AnimationName = "body_minomutchi_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/minomadamu_s_00.ab", Mod.ReadResourceArray("Resources/minomadamu_s_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MINOMADAMU_DUST, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "minomadamu_d_00";
graphicsEntry.AnimationName = "body_minomutchi_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/minomadamu_d_00.ab", Mod.ReadResourceArray("Resources/minomadamu_d_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GAAMEIRU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gaameiru_00";
graphicsEntry.AnimationName = "fly_bibiyon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gaameiru_00.ab", Mod.ReadResourceArray("Resources/gaameiru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MITSUHANII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mitsuhanii_00";
graphicsEntry.AnimationName = "fly_mitsuhanii_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mitsuhanii_00.ab", Mod.ReadResourceArray("Resources/mitsuhanii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MITSUHANII, PokemonFormType.FEMALE);
graphicsEntry.ModelName = "mitsuhanii_f_00";
graphicsEntry.AnimationName = "fly_mitsuhanii_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mitsuhanii_f_00.ab", Mod.ReadResourceArray("Resources/mitsuhanii_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BIIKUIN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "biikuin_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/biikuin_00.ab", Mod.ReadResourceArray("Resources/biikuin_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PACHIRISU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "pachirisu_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/pachirisu_00.ab", Mod.ReadResourceArray("Resources/pachirisu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BUIZERU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "buizeru_00";
graphicsEntry.AnimationName = "2leg_buizeru_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/buizeru_00.ab", Mod.ReadResourceArray("Resources/buizeru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BUIZERU, PokemonFormType.FEMALE);
graphicsEntry.ModelName = "buizeru_f_00";
graphicsEntry.AnimationName = "2leg_buizeru_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/buizeru_f_00.ab", Mod.ReadResourceArray("Resources/buizeru_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FUROOZERU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "furoozeru_00";
graphicsEntry.AnimationName = "2leg_furoozeru_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/furoozeru_00.ab", Mod.ReadResourceArray("Resources/furoozeru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FUROOZERU, PokemonFormType.FEMALE);
graphicsEntry.ModelName = "furoozeru_f_00";
graphicsEntry.AnimationName = "2leg_furoozeru_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/furoozeru_f_00.ab", Mod.ReadResourceArray("Resources/furoozeru_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.CHERINBO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "cherinbo_00";
graphicsEntry.AnimationName = "2leg_bound_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/cherinbo_00.ab", Mod.ReadResourceArray("Resources/cherinbo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.CHERIMU_N, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "cherimu_n_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/cherimu_n_00.ab", Mod.ReadResourceArray("Resources/cherimu_n_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.CHERIMU_P, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "cherimu_p_00";
graphicsEntry.AnimationName = "2leg_beast_up_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/cherimu_p_00.ab", Mod.ReadResourceArray("Resources/cherimu_p_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KARANAKUSHI_E, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "karanakushi_e_00";
graphicsEntry.AnimationName = "6leg_insect_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/karanakushi_e_00.ab", Mod.ReadResourceArray("Resources/karanakushi_e_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KARANAKUSHI_W, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "karanakushi_w_00";
graphicsEntry.AnimationName = "6leg_insect_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/karanakushi_w_00.ab", Mod.ReadResourceArray("Resources/karanakushi_w_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TORITODON_E, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "toritodon_e_00";
graphicsEntry.AnimationName = "6leg_insect_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/toritodon_e_00.ab", Mod.ReadResourceArray("Resources/toritodon_e_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TORITODON_W, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "toritodon_w_00";
graphicsEntry.AnimationName = "6leg_insect_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/toritodon_w_00.ab", Mod.ReadResourceArray("Resources/toritodon_w_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FUWANTE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "fuwante_00";
graphicsEntry.AnimationName = "float_body_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/fuwante_00.ab", Mod.ReadResourceArray("Resources/fuwante_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FUWARAIDO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "fuwaraido_00";
graphicsEntry.AnimationName = "fly_dokukurage_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/fuwaraido_00.ab", Mod.ReadResourceArray("Resources/fuwaraido_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MIMIRORU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mimiroru_00";
graphicsEntry.AnimationName = "2leg_beast_up_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mimiroru_00.ab", Mod.ReadResourceArray("Resources/mimiroru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MIMIROPPU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mimiroppu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mimiroppu_00.ab", Mod.ReadResourceArray("Resources/mimiroppu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NYARUMAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "nyarumaa_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/nyarumaa_00.ab", Mod.ReadResourceArray("Resources/nyarumaa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BUNYATTO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "bunyatto_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/bunyatto_00.ab", Mod.ReadResourceArray("Resources/bunyatto_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SUKANPUU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "sukanpuu_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/sukanpuu_00.ab", Mod.ReadResourceArray("Resources/sukanpuu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SUKATANKU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "sukatanku_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/sukatanku_00.ab", Mod.ReadResourceArray("Resources/sukatanku_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DOOMIRAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "doomiraa_00";
graphicsEntry.AnimationName = "float_body_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/doomiraa_00.ab", Mod.ReadResourceArray("Resources/doomiraa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DOOTAKUN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dootakun_00";
graphicsEntry.AnimationName = "float_body_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dootakun_00.ab", Mod.ReadResourceArray("Resources/dootakun_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PERAPPU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "perappu_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/perappu_00.ab", Mod.ReadResourceArray("Resources/perappu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MIKARUGE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mikaruge_00";
graphicsEntry.AnimationName = "bound_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mikaruge_00.ab", Mod.ReadResourceArray("Resources/mikaruge_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FUKAMARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "fukamaru_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/fukamaru_00.ab", Mod.ReadResourceArray("Resources/fukamaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GABAITO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gabaito_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gabaito_00.ab", Mod.ReadResourceArray("Resources/gabaito_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GABURIASU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gaburiasu_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gaburiasu_00.ab", Mod.ReadResourceArray("Resources/gaburiasu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GABURIASU_MEGA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gaburiasu_mega_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gaburiasu_mega_00.ab", Mod.ReadResourceArray("Resources/gaburiasu_mega_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HIPOPOTASU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hipopotasu_00";
graphicsEntry.AnimationName = "4leg_hipopotasu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hipopotasu_00.ab", Mod.ReadResourceArray("Resources/hipopotasu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HIPOPOTASU, PokemonFormType.FEMALE);
graphicsEntry.ModelName = "hipopotasu_f_00";
graphicsEntry.AnimationName = "4leg_hipopotasu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hipopotasu_f_00.ab", Mod.ReadResourceArray("Resources/hipopotasu_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KABARUDON, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kabarudon_00";
graphicsEntry.AnimationName = "4leg_hipopotasu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kabarudon_00.ab", Mod.ReadResourceArray("Resources/kabarudon_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KABARUDON, PokemonFormType.FEMALE);
graphicsEntry.ModelName = "kabarudon_f_00";
graphicsEntry.AnimationName = "4leg_hipopotasu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kabarudon_f_00.ab", Mod.ReadResourceArray("Resources/kabarudon_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SUKORUPI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "sukorupi_00";
graphicsEntry.AnimationName = "4leg_insect_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/sukorupi_00.ab", Mod.ReadResourceArray("Resources/sukorupi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DORAPION, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dorapion_00";
graphicsEntry.AnimationName = "4leg_doragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dorapion_00.ab", Mod.ReadResourceArray("Resources/dorapion_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GUREGGURU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "guregguru_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/guregguru_00.ab", Mod.ReadResourceArray("Resources/guregguru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DOKUROGGU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dokuroggu_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dokuroggu_00.ab", Mod.ReadResourceArray("Resources/dokuroggu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MASUKIPPA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "masukippa_00";
graphicsEntry.AnimationName = "fly_masukippa_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/masukippa_00.ab", Mod.ReadResourceArray("Resources/masukippa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KEIKOUO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "keikouo_00";
graphicsEntry.AnimationName = "fly_hinbasu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/keikouo_00.ab", Mod.ReadResourceArray("Resources/keikouo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NEORANTO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "neoranto_00";
graphicsEntry.AnimationName = "fly_hinbasu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/neoranto_00.ab", Mod.ReadResourceArray("Resources/neoranto_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YUKIKABURI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "yukikaburi_00";
graphicsEntry.AnimationName = "2leg_yukikaburi_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yukikaburi_00.ab", Mod.ReadResourceArray("Resources/yukikaburi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YUKIKABURI, PokemonFormType.FEMALE);
graphicsEntry.ModelName = "yukikaburi_f_00";
graphicsEntry.AnimationName = "2leg_yukikaburi_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yukikaburi_f_00.ab", Mod.ReadResourceArray("Resources/yukikaburi_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YUKINOOO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "yukinooo_00";
graphicsEntry.AnimationName = "2leg_beast_up_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yukinooo_00.ab", Mod.ReadResourceArray("Resources/yukinooo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YUKINOOO_MEGA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "yukinooo_mega_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yukinooo_mega_00.ab", Mod.ReadResourceArray("Resources/yukinooo_mega_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ROTOMU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "rotomu_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/rotomu_00.ab", Mod.ReadResourceArray("Resources/rotomu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ROTOMU_HEAT, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "rotomu_h_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/rotomu_h_00.ab", Mod.ReadResourceArray("Resources/rotomu_h_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ROTOMU_SPIN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "rotomu_s_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/rotomu_s_00.ab", Mod.ReadResourceArray("Resources/rotomu_s_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ROTOMU_FROST, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "rotomu_f_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/rotomu_f_00.ab", Mod.ReadResourceArray("Resources/rotomu_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ROTOMU_WASH, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "rotomu_w_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/rotomu_w_00.ab", Mod.ReadResourceArray("Resources/rotomu_w_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ROTOMU_CUT, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "rotomu_c_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/rotomu_c_00.ab", Mod.ReadResourceArray("Resources/rotomu_c_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YUKUSHII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "yukushii_00";
graphicsEntry.AnimationName = "fly_yukushii_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yukushii_00.ab", Mod.ReadResourceArray("Resources/yukushii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.EMURITTO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "emuritto_00";
graphicsEntry.AnimationName = "fly_yukushii_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/emuritto_00.ab", Mod.ReadResourceArray("Resources/emuritto_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.AGUNOMU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "agunomu_00";
graphicsEntry.AnimationName = "fly_yukushii_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/agunomu_00.ab", Mod.ReadResourceArray("Resources/agunomu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DIARUGA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "diaruga_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/diaruga_00.ab", Mod.ReadResourceArray("Resources/diaruga_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PARUKIA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "parukia_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/parukia_00.ab", Mod.ReadResourceArray("Resources/parukia_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HIIDORAN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hiidoran_00";
graphicsEntry.AnimationName = "4leg_doragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hiidoran_00.ab", Mod.ReadResourceArray("Resources/hiidoran_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.REJIGIGASU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "rejigigasu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/rejigigasu_00.ab", Mod.ReadResourceArray("Resources/rejigigasu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GIRATYINA_A, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "giratyina_a_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/giratyina_a_00.ab", Mod.ReadResourceArray("Resources/giratyina_a_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GIRATYINA_O, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "giratyina_o_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/giratyina_o_00.ab", Mod.ReadResourceArray("Resources/giratyina_o_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KURESERIA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kureseria_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kureseria_00.ab", Mod.ReadResourceArray("Resources/kureseria_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FIONE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "fione_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/fione_00.ab", Mod.ReadResourceArray("Resources/fione_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MANAFI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "manafi_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/manafi_00.ab", Mod.ReadResourceArray("Resources/manafi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DAAKURAI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "daakurai_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/daakurai_00.ab", Mod.ReadResourceArray("Resources/daakurai_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHEIMI_L, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "sheimi_l_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/sheimi_l_00.ab", Mod.ReadResourceArray("Resources/sheimi_l_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHEIMI_S, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "sheimi_s_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/sheimi_s_00.ab", Mod.ReadResourceArray("Resources/sheimi_s_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_NORMAL, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_normal_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_normal_00.ab", Mod.ReadResourceArray("Resources/aruseusu_normal_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_GRASS, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_grass_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_grass_00.ab", Mod.ReadResourceArray("Resources/aruseusu_grass_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_FIRE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_fire_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_fire_00.ab", Mod.ReadResourceArray("Resources/aruseusu_fire_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_WATER, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_water_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_water_00.ab", Mod.ReadResourceArray("Resources/aruseusu_water_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_ELECTRIC, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_electric_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_electric_00.ab", Mod.ReadResourceArray("Resources/aruseusu_electric_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_ICE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_ice_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_ice_00.ab", Mod.ReadResourceArray("Resources/aruseusu_ice_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_FIGHTING, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_fighting_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_fighting_00.ab", Mod.ReadResourceArray("Resources/aruseusu_fighting_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_POISON, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_poison_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_poison_00.ab", Mod.ReadResourceArray("Resources/aruseusu_poison_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_GROUND, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_ground_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_ground_00.ab", Mod.ReadResourceArray("Resources/aruseusu_ground_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_FLYING, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_flying_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_flying_00.ab", Mod.ReadResourceArray("Resources/aruseusu_flying_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_PSYCHIC, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_psychic_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_psychic_00.ab", Mod.ReadResourceArray("Resources/aruseusu_psychic_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_BUG, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_bug_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_bug_00.ab", Mod.ReadResourceArray("Resources/aruseusu_bug_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_ROCK, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_rock_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_rock_00.ab", Mod.ReadResourceArray("Resources/aruseusu_rock_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_GHOST, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_ghost_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_ghost_00.ab", Mod.ReadResourceArray("Resources/aruseusu_ghost_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_DRAGON, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_dragon_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_dragon_00.ab", Mod.ReadResourceArray("Resources/aruseusu_dragon_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_DARK, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_dark_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_dark_00.ab", Mod.ReadResourceArray("Resources/aruseusu_dark_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_STEEL, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_steel_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_steel_00.ab", Mod.ReadResourceArray("Resources/aruseusu_steel_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ARUSEUSU_FAIRY, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aruseusu_fairy_00";
graphicsEntry.AnimationName = "4leg_aruseusu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aruseusu_fairy_00.ab", Mod.ReadResourceArray("Resources/aruseusu_fairy_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BIKUTYINI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "bikutyini_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/bikutyini_00.ab", Mod.ReadResourceArray("Resources/bikutyini_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TSUTAAJA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "tsutaaja_00";
graphicsEntry.AnimationName = "2leg_tsutaaja_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/tsutaaja_00.ab", Mod.ReadResourceArray("Resources/tsutaaja_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.JANOBII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "janobii_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/janobii_00.ab", Mod.ReadResourceArray("Resources/janobii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.JAROODA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "jarooda_00";
graphicsEntry.AnimationName = "1leg_snake_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/jarooda_00.ab", Mod.ReadResourceArray("Resources/jarooda_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.POKABU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "pokabu_00";
graphicsEntry.AnimationName = "4leg_pokabu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/pokabu_00.ab", Mod.ReadResourceArray("Resources/pokabu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.CHAOBUU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "chaobuu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/chaobuu_00.ab", Mod.ReadResourceArray("Resources/chaobuu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ENBUOO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "enbuoo_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/enbuoo_00.ab", Mod.ReadResourceArray("Resources/enbuoo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MIJUMARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mijumaru_00";
graphicsEntry.AnimationName = "2leg_mijumaru_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mijumaru_00.ab", Mod.ReadResourceArray("Resources/mijumaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FUTACHIMARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "futachimaru_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/futachimaru_00.ab", Mod.ReadResourceArray("Resources/futachimaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DAIKENKI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "daikenki_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/daikenki_00.ab", Mod.ReadResourceArray("Resources/daikenki_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MINEZUMI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "minezumi_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/minezumi_00.ab", Mod.ReadResourceArray("Resources/minezumi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MIRUHOGGU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "miruhoggu_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/miruhoggu_00.ab", Mod.ReadResourceArray("Resources/miruhoggu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YOOTERII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "yooterii_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yooterii_00.ab", Mod.ReadResourceArray("Resources/yooterii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HAADERIA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "haaderia_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/haaderia_00.ab", Mod.ReadResourceArray("Resources/haaderia_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MUURANDO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "muurando_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/muurando_00.ab", Mod.ReadResourceArray("Resources/muurando_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.CHORONEKO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "choroneko_00";
graphicsEntry.AnimationName = "2leg_beast_up_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/choroneko_00.ab", Mod.ReadResourceArray("Resources/choroneko_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.REPARUDASU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "reparudasu_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/reparudasu_00.ab", Mod.ReadResourceArray("Resources/reparudasu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YANAPPU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "yanappu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yanappu_00.ab", Mod.ReadResourceArray("Resources/yanappu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YANAKKII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "yanakkii_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yanakkii_00.ab", Mod.ReadResourceArray("Resources/yanakkii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BAOPPU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "baoppu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/baoppu_00.ab", Mod.ReadResourceArray("Resources/baoppu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BAOKKII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "baokkii_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/baokkii_00.ab", Mod.ReadResourceArray("Resources/baokkii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HIYAPPU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hiyappu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hiyappu_00.ab", Mod.ReadResourceArray("Resources/hiyappu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HIYAKKII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hiyakkii_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hiyakkii_00.ab", Mod.ReadResourceArray("Resources/hiyakkii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MUNNA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "munna_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/munna_00.ab", Mod.ReadResourceArray("Resources/munna_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MUSHAANA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mushaana_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mushaana_00.ab", Mod.ReadResourceArray("Resources/mushaana_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MAMEPATO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mamepato_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mamepato_00.ab", Mod.ReadResourceArray("Resources/mamepato_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HATOOBOOO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hatoobooo_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hatoobooo_00.ab", Mod.ReadResourceArray("Resources/hatoobooo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KENHOROU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kenhorou_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kenhorou_00.ab", Mod.ReadResourceArray("Resources/kenhorou_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KENHOROU, PokemonFormType.FEMALE);
graphicsEntry.ModelName = "kenhorou_f_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kenhorou_f_00.ab", Mod.ReadResourceArray("Resources/kenhorou_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHIMAMA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "shimama_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/shimama_00.ab", Mod.ReadResourceArray("Resources/shimama_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ZEBURAIKA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "zeburaika_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/zeburaika_00.ab", Mod.ReadResourceArray("Resources/zeburaika_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DANGORO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dangoro_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dangoro_00.ab", Mod.ReadResourceArray("Resources/dangoro_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GANTORU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gantoru_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gantoru_00.ab", Mod.ReadResourceArray("Resources/gantoru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GIGAIASU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gigaiasu_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gigaiasu_00.ab", Mod.ReadResourceArray("Resources/gigaiasu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOROMORI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "koromori_00";
graphicsEntry.AnimationName = "fly_bat_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/koromori_00.ab", Mod.ReadResourceArray("Resources/koromori_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOKOROMORI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kokoromori_00";
graphicsEntry.AnimationName = "fly_bat_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kokoromori_00.ab", Mod.ReadResourceArray("Resources/kokoromori_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MOGURYUU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "moguryuu_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/moguryuu_00.ab", Mod.ReadResourceArray("Resources/moguryuu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DORYUUZU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "doryuuzu_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/doryuuzu_00.ab", Mod.ReadResourceArray("Resources/doryuuzu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TABUNNE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "tabunne_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/tabunne_00.ab", Mod.ReadResourceArray("Resources/tabunne_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DOKKORAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dokkoraa_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dokkoraa_00.ab", Mod.ReadResourceArray("Resources/dokkoraa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DOTEKKOTSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dotekkotsu_00";
graphicsEntry.AnimationName = "2leg_dotekkotsu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dotekkotsu_00.ab", Mod.ReadResourceArray("Resources/dotekkotsu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ROOBUSHIN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "roobushin_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/roobushin_00.ab", Mod.ReadResourceArray("Resources/roobushin_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.OTAMARO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "otamaro_00";
graphicsEntry.AnimationName = "body_bound_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/otamaro_00.ab", Mod.ReadResourceArray("Resources/otamaro_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GAMAGARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gamagaru_00";
graphicsEntry.AnimationName = "2leg_bound_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gamagaru_00.ab", Mod.ReadResourceArray("Resources/gamagaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GAMAGEROGE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gamageroge_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gamageroge_00.ab", Mod.ReadResourceArray("Resources/gamageroge_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NAGEKI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "nageki_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/nageki_00.ab", Mod.ReadResourceArray("Resources/nageki_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DAGEKI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dageki_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dageki_00.ab", Mod.ReadResourceArray("Resources/dageki_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KURUMIRU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kurumiru_00";
graphicsEntry.AnimationName = "6leg_insect_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kurumiru_00.ab", Mod.ReadResourceArray("Resources/kurumiru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KURUMAYU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kurumayu_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kurumayu_00.ab", Mod.ReadResourceArray("Resources/kurumayu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HAHAKOMORI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hahakomori_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hahakomori_00.ab", Mod.ReadResourceArray("Resources/hahakomori_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FUSHIDE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "fushide_00";
graphicsEntry.AnimationName = "6leg_insect_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/fushide_00.ab", Mod.ReadResourceArray("Resources/fushide_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HOIIGA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hoiiga_00";
graphicsEntry.AnimationName = "body_bound_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hoiiga_00.ab", Mod.ReadResourceArray("Resources/hoiiga_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PENDORAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "pendoraa_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/pendoraa_00.ab", Mod.ReadResourceArray("Resources/pendoraa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MONMEN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "monmen_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/monmen_00.ab", Mod.ReadResourceArray("Resources/monmen_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ERUFUUN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "erufuun_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/erufuun_00.ab", Mod.ReadResourceArray("Resources/erufuun_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.CHURINE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "churine_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/churine_00.ab", Mod.ReadResourceArray("Resources/churine_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DOREDYIA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "doredyia_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/doredyia_00.ab", Mod.ReadResourceArray("Resources/doredyia_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BASURAO_R, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "basurao_r_00";
graphicsEntry.AnimationName = "fly_hinbasu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/basurao_r_00.ab", Mod.ReadResourceArray("Resources/basurao_r_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BASURAO_B, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "basurao_b_00";
graphicsEntry.AnimationName = "fly_hinbasu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/basurao_b_00.ab", Mod.ReadResourceArray("Resources/basurao_b_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MEGUROKO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "meguroko_00";
graphicsEntry.AnimationName = "4leg_doragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/meguroko_00.ab", Mod.ReadResourceArray("Resources/meguroko_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.WARUBIRU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "warubiru_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/warubiru_00.ab", Mod.ReadResourceArray("Resources/warubiru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.WARUBIARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "warubiaru_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/warubiaru_00.ab", Mod.ReadResourceArray("Resources/warubiaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DARUMAKKA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "darumakka_00";
graphicsEntry.AnimationName = "2leg_beast_up_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/darumakka_00.ab", Mod.ReadResourceArray("Resources/darumakka_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HIHIDARUMA_N, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hihidaruma_n_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hihidaruma_n_00.ab", Mod.ReadResourceArray("Resources/hihidaruma_n_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HIHIDARUMA_D, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hihidaruma_d_00";
graphicsEntry.AnimationName = "body_bound_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hihidaruma_d_00.ab", Mod.ReadResourceArray("Resources/hihidaruma_d_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MARAKATCHI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "marakatchi_00";
graphicsEntry.AnimationName = "bound_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/marakatchi_00.ab", Mod.ReadResourceArray("Resources/marakatchi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ISHIZUMAI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "ishizumai_00";
graphicsEntry.AnimationName = "6leg_crab_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/ishizumai_00.ab", Mod.ReadResourceArray("Resources/ishizumai_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.IWAPARESU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "iwaparesu_00";
graphicsEntry.AnimationName = "6leg_crab_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/iwaparesu_00.ab", Mod.ReadResourceArray("Resources/iwaparesu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ZURUGGU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "zuruggu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/zuruggu_00.ab", Mod.ReadResourceArray("Resources/zuruggu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ZURUZUKIN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "zuruzukin_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/zuruzukin_00.ab", Mod.ReadResourceArray("Resources/zuruzukin_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHINBORAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "shinboraa_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/shinboraa_00.ab", Mod.ReadResourceArray("Resources/shinboraa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DESUMASU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "desumasu_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/desumasu_00.ab", Mod.ReadResourceArray("Resources/desumasu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DESUKAAN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "desukaan_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/desukaan_00.ab", Mod.ReadResourceArray("Resources/desukaan_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PUROTOOGA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "purotooga_00";
graphicsEntry.AnimationName = "4leg_tortoise_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/purotooga_00.ab", Mod.ReadResourceArray("Resources/purotooga_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ABAGOORA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "abagoora_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/abagoora_00.ab", Mod.ReadResourceArray("Resources/abagoora_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.AAKEN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aaken_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aaken_00.ab", Mod.ReadResourceArray("Resources/aaken_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.AAKEOSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aakeosu_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aakeosu_00.ab", Mod.ReadResourceArray("Resources/aakeosu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YABUKURON, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "yabukuron_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yabukuron_00.ab", Mod.ReadResourceArray("Resources/yabukuron_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DASUTODASU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dasutodasu_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dasutodasu_00.ab", Mod.ReadResourceArray("Resources/dasutodasu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ZOROA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "zoroa_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/zoroa_00.ab", Mod.ReadResourceArray("Resources/zoroa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ZOROAAKU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "zoroaaku_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/zoroaaku_00.ab", Mod.ReadResourceArray("Resources/zoroaaku_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.CHIRAAMII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "chiraamii_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/chiraamii_00.ab", Mod.ReadResourceArray("Resources/chiraamii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.CHIRACHIINO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "chirachiino_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/chirachiino_00.ab", Mod.ReadResourceArray("Resources/chirachiino_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GOCHIMU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gochimu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gochimu_00.ab", Mod.ReadResourceArray("Resources/gochimu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GOCHIMIRU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gochimiru_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gochimiru_00.ab", Mod.ReadResourceArray("Resources/gochimiru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GOCHIRUZERU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gochiruzeru_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gochiruzeru_00.ab", Mod.ReadResourceArray("Resources/gochiruzeru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YUNIRAN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "yuniran_00";
graphicsEntry.AnimationName = "float_body_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yuniran_00.ab", Mod.ReadResourceArray("Resources/yuniran_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DABURAN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "daburan_00";
graphicsEntry.AnimationName = "float_body_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/daburan_00.ab", Mod.ReadResourceArray("Resources/daburan_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.RANKURUSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "rankurusu_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/rankurusu_00.ab", Mod.ReadResourceArray("Resources/rankurusu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOARUHII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "koaruhii_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/koaruhii_00.ab", Mod.ReadResourceArray("Resources/koaruhii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SUWANNA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "suwanna_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/suwanna_00.ab", Mod.ReadResourceArray("Resources/suwanna_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BANIPUTCHI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "baniputchi_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/baniputchi_00.ab", Mod.ReadResourceArray("Resources/baniputchi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BANIRITCHI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "baniritchi_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/baniritchi_00.ab", Mod.ReadResourceArray("Resources/baniritchi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BAIBANIRA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "baibanira_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/baibanira_00.ab", Mod.ReadResourceArray("Resources/baibanira_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHIKIJIKA_SPRING, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "shikijika_spring_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/shikijika_spring_00.ab", Mod.ReadResourceArray("Resources/shikijika_spring_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MEBUKIJIKA_SPRING, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mebukijika_spring_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mebukijika_spring_00.ab", Mod.ReadResourceArray("Resources/mebukijika_spring_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.EMONGA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "emonga_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/emonga_00.ab", Mod.ReadResourceArray("Resources/emonga_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KABURUMO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kaburumo_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kaburumo_00.ab", Mod.ReadResourceArray("Resources/kaburumo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHUBARUGO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "shubarugo_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/shubarugo_00.ab", Mod.ReadResourceArray("Resources/shubarugo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TAMAGETAKE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "tamagetake_00";
graphicsEntry.AnimationName = "bound_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/tamagetake_00.ab", Mod.ReadResourceArray("Resources/tamagetake_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MOROBARERU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "morobareru_00";
graphicsEntry.AnimationName = "bound_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/morobareru_00.ab", Mod.ReadResourceArray("Resources/morobareru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PURURIRU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "pururiru_00";
graphicsEntry.AnimationName = "fly_masukippa_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/pururiru_00.ab", Mod.ReadResourceArray("Resources/pururiru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PURURIRU, PokemonFormType.FEMALE);
graphicsEntry.ModelName = "pururiru_f_00";
graphicsEntry.AnimationName = "fly_masukippa_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/pururiru_f_00.ab", Mod.ReadResourceArray("Resources/pururiru_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BURUNGERU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "burungeru_00";
graphicsEntry.AnimationName = "fly_masukippa_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/burungeru_00.ab", Mod.ReadResourceArray("Resources/burungeru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BURUNGERU, PokemonFormType.FEMALE);
graphicsEntry.ModelName = "burungeru_f_00";
graphicsEntry.AnimationName = "fly_masukippa_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/burungeru_f_00.ab", Mod.ReadResourceArray("Resources/burungeru_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MAMANBOU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mamanbou_00";
graphicsEntry.AnimationName = "fly_tosakinto_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mamanbou_00.ab", Mod.ReadResourceArray("Resources/mamanbou_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BACHURU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "bachuru_00";
graphicsEntry.AnimationName = "4leg_insect_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/bachuru_00.ab", Mod.ReadResourceArray("Resources/bachuru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DENCHURA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "denchura_00";
graphicsEntry.AnimationName = "4leg_insect_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/denchura_00.ab", Mod.ReadResourceArray("Resources/denchura_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TESSHIIDO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "tesshiido_00";
graphicsEntry.AnimationName = "body_bound_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/tesshiido_00.ab", Mod.ReadResourceArray("Resources/tesshiido_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NATTOREI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "nattorei_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/nattorei_00.ab", Mod.ReadResourceArray("Resources/nattorei_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GIARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "giaru_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/giaru_00.ab", Mod.ReadResourceArray("Resources/giaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GIGIARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gigiaru_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gigiaru_00.ab", Mod.ReadResourceArray("Resources/gigiaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GIGIGIARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gigigiaru_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gigigiaru_00.ab", Mod.ReadResourceArray("Resources/gigigiaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHIBISHIRASU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "shibishirasu_00";
graphicsEntry.AnimationName = "fly_tosakinto_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/shibishirasu_00.ab", Mod.ReadResourceArray("Resources/shibishirasu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHIBIBIIRU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "shibibiiru_00";
graphicsEntry.AnimationName = "1leg_snake_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/shibibiiru_00.ab", Mod.ReadResourceArray("Resources/shibibiiru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHIBIRUDON, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "shibirudon_00";
graphicsEntry.AnimationName = "1leg_snake_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/shibirudon_00.ab", Mod.ReadResourceArray("Resources/shibirudon_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.RIGUREE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "riguree_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/riguree_00.ab", Mod.ReadResourceArray("Resources/riguree_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.OOBEMU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "oobemu_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/oobemu_00.ab", Mod.ReadResourceArray("Resources/oobemu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HITOMOSHI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hitomoshi_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hitomoshi_00.ab", Mod.ReadResourceArray("Resources/hitomoshi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.RANPURAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "ranpuraa_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/ranpuraa_00.ab", Mod.ReadResourceArray("Resources/ranpuraa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHANDERA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "shandera_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/shandera_00.ab", Mod.ReadResourceArray("Resources/shandera_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KIBAGO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kibago_00";
graphicsEntry.AnimationName = "2leg_kibago_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kibago_00.ab", Mod.ReadResourceArray("Resources/kibago_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ONONDO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "onondo_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/onondo_00.ab", Mod.ReadResourceArray("Resources/onondo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ONONOKUSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "ononokusu_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/ononokusu_00.ab", Mod.ReadResourceArray("Resources/ononokusu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KUMASHUN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kumashun_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kumashun_00.ab", Mod.ReadResourceArray("Resources/kumashun_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TSUNBEAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "tsunbeaa_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/tsunbeaa_00.ab", Mod.ReadResourceArray("Resources/tsunbeaa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FURIIJIO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "furiijio_00";
graphicsEntry.AnimationName = "float_body_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/furiijio_00.ab", Mod.ReadResourceArray("Resources/furiijio_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.CHOBOMAKI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "chobomaki_00";
graphicsEntry.AnimationName = "2leg_bound_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/chobomaki_00.ab", Mod.ReadResourceArray("Resources/chobomaki_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.AGIRUDAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "agirudaa_00";
graphicsEntry.AnimationName = "bound_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/agirudaa_00.ab", Mod.ReadResourceArray("Resources/agirudaa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MAGGYO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "maggyo_00";
graphicsEntry.AnimationName = "body_bound_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/maggyo_00.ab", Mod.ReadResourceArray("Resources/maggyo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOJOFUU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kojofuu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kojofuu_00.ab", Mod.ReadResourceArray("Resources/kojofuu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOJONDO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kojondo_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kojondo_00.ab", Mod.ReadResourceArray("Resources/kojondo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KURIMUGAN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kurimugan_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kurimugan_00.ab", Mod.ReadResourceArray("Resources/kurimugan_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GOBITTO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gobitto_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gobitto_00.ab", Mod.ReadResourceArray("Resources/gobitto_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GORUUGU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "goruugu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/goruugu_00.ab", Mod.ReadResourceArray("Resources/goruugu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOMATANA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "komatana_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/komatana_00.ab", Mod.ReadResourceArray("Resources/komatana_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KIRIKIZAN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kirikizan_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kirikizan_00.ab", Mod.ReadResourceArray("Resources/kirikizan_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BAFFURON, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "baffuron_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/baffuron_00.ab", Mod.ReadResourceArray("Resources/baffuron_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.WASHIBON, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "washibon_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/washibon_00.ab", Mod.ReadResourceArray("Resources/washibon_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.WOOGURU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "wooguru_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/wooguru_00.ab", Mod.ReadResourceArray("Resources/wooguru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BARUCHAI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "baruchai_00";
graphicsEntry.AnimationName = "2leg_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/baruchai_00.ab", Mod.ReadResourceArray("Resources/baruchai_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BARUJIINA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "barujiina_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/barujiina_00.ab", Mod.ReadResourceArray("Resources/barujiina_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KUITARAN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kuitaran_00";
graphicsEntry.AnimationName = "2leg_beast_up_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kuitaran_00.ab", Mod.ReadResourceArray("Resources/kuitaran_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.AIANTO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "aianto_00";
graphicsEntry.AnimationName = "6leg_crab_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/aianto_00.ab", Mod.ReadResourceArray("Resources/aianto_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MONOZU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "monozu_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/monozu_00.ab", Mod.ReadResourceArray("Resources/monozu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.JIHEDDO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "jiheddo_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/jiheddo_00.ab", Mod.ReadResourceArray("Resources/jiheddo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SAZANDORA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "sazandora_00";
graphicsEntry.AnimationName = "fly_sazandora_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/sazandora_00.ab", Mod.ReadResourceArray("Resources/sazandora_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MERARUBA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "meraruba_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/meraruba_00.ab", Mod.ReadResourceArray("Resources/meraruba_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.URUGAMOSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "urugamosu_00";
graphicsEntry.AnimationName = "fly_butterfly_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/urugamosu_00.ab", Mod.ReadResourceArray("Resources/urugamosu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOBARUON, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kobaruon_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kobaruon_00.ab", Mod.ReadResourceArray("Resources/kobaruon_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TERAKION, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "terakion_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/terakion_00.ab", Mod.ReadResourceArray("Resources/terakion_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BIRIJION, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "birijion_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/birijion_00.ab", Mod.ReadResourceArray("Resources/birijion_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TORUNEROSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "torunerosu_00";
graphicsEntry.AnimationName = "float_borutorosu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/torunerosu_00.ab", Mod.ReadResourceArray("Resources/torunerosu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TORUNEROSU_R, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "torunerosu_r_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/torunerosu_r_00.ab", Mod.ReadResourceArray("Resources/torunerosu_r_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BORUTOROSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "borutorosu_00";
graphicsEntry.AnimationName = "float_borutorosu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/borutorosu_00.ab", Mod.ReadResourceArray("Resources/borutorosu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BORUTOROSU_R, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "borutorosu_r_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/borutorosu_r_00.ab", Mod.ReadResourceArray("Resources/borutorosu_r_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.RESHIRAMU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "reshiramu_10";
graphicsEntry.AnimationName = "2leg_reshiramu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/reshiramu_10.ab", Mod.ReadResourceArray("Resources/reshiramu_10.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ZEKUROMU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "zekuromu_10";
graphicsEntry.AnimationName = "2leg_zekuromu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/zekuromu_10.ab", Mod.ReadResourceArray("Resources/zekuromu_10.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.RANDOROSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "randorosu_00";
graphicsEntry.AnimationName = "float_borutorosu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/randorosu_00.ab", Mod.ReadResourceArray("Resources/randorosu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.RANDOROSU_R, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "randorosu_r_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/randorosu_r_00.ab", Mod.ReadResourceArray("Resources/randorosu_r_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KYUREMU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kyuremu_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kyuremu_00.ab", Mod.ReadResourceArray("Resources/kyuremu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KYUREMU_BLACK, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kyuremu_40";
graphicsEntry.AnimationName = "2leg_kyuremu_30";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kyuremu_40.ab", Mod.ReadResourceArray("Resources/kyuremu_40.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KYUREMU_WHITE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kyuremu_20";
graphicsEntry.AnimationName = "2leg_kyuremu_10";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kyuremu_20.ab", Mod.ReadResourceArray("Resources/kyuremu_20.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KERUDYIO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kerudyio_00";
graphicsEntry.AnimationName = "4leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kerudyio_00.ab", Mod.ReadResourceArray("Resources/kerudyio_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KERUDYIO_K, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kerudyio_k_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kerudyio_k_00.ab", Mod.ReadResourceArray("Resources/kerudyio_k_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MEROETTA_V, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "meroetta_v_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/meroetta_v_00.ab", Mod.ReadResourceArray("Resources/meroetta_v_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MEROETTA_S, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "meroetta_s_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/meroetta_s_00.ab", Mod.ReadResourceArray("Resources/meroetta_s_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GENOSEKUTO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "genosekuto_00";
graphicsEntry.AnimationName = "2leg_genosekuto_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/genosekuto_00.ab", Mod.ReadResourceArray("Resources/genosekuto_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GENOSEKUTO_AQUA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "genosekuto_a_00";
graphicsEntry.AnimationName = "2leg_genosekuto_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/genosekuto_a_00.ab", Mod.ReadResourceArray("Resources/genosekuto_a_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GENOSEKUTO_INAZUMA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "genosekuto_i_00";
graphicsEntry.AnimationName = "2leg_genosekuto_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/genosekuto_i_00.ab", Mod.ReadResourceArray("Resources/genosekuto_i_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GENOSEKUTO_BLAZE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "genosekuto_b_00";
graphicsEntry.AnimationName = "2leg_genosekuto_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/genosekuto_b_00.ab", Mod.ReadResourceArray("Resources/genosekuto_b_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GENOSEKUTO_FREEZE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "genosekuto_f_00";
graphicsEntry.AnimationName = "2leg_genosekuto_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/genosekuto_f_00.ab", Mod.ReadResourceArray("Resources/genosekuto_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HARIMARON, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "harimaron_00";
graphicsEntry.AnimationName = "2leg_harimaron_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/harimaron_00.ab", Mod.ReadResourceArray("Resources/harimaron_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HARIBOOGU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hariboogu_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hariboogu_00.ab", Mod.ReadResourceArray("Resources/hariboogu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BURIGARON, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "burigaron_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/burigaron_00.ab", Mod.ReadResourceArray("Resources/burigaron_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FOKKO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "fokko_00";
graphicsEntry.AnimationName = "4leg_fokko_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/fokko_00.ab", Mod.ReadResourceArray("Resources/fokko_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TEERUNAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "teerunaa_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/teerunaa_00.ab", Mod.ReadResourceArray("Resources/teerunaa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MAFOKUSHII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mafokushii_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mafokushii_00.ab", Mod.ReadResourceArray("Resources/mafokushii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KEROMATSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "keromatsu_00";
graphicsEntry.AnimationName = "2leg_keromatsu_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/keromatsu_00.ab", Mod.ReadResourceArray("Resources/keromatsu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GEKOGASHIRA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gekogashira_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gekogashira_00.ab", Mod.ReadResourceArray("Resources/gekogashira_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GEKKOUGA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gekkouga_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gekkouga_00.ab", Mod.ReadResourceArray("Resources/gekkouga_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HORUBII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "horubii_00";
graphicsEntry.AnimationName = "2leg_beast_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/horubii_00.ab", Mod.ReadResourceArray("Resources/horubii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HORUUDO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "horuudo_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/horuudo_00.ab", Mod.ReadResourceArray("Resources/horuudo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YAYAKOMA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "yayakoma_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yayakoma_00.ab", Mod.ReadResourceArray("Resources/yayakoma_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HINOYAKOMA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hinoyakoma_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hinoyakoma_00.ab", Mod.ReadResourceArray("Resources/hinoyakoma_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FAIAROO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "faiaroo_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/faiaroo_00.ab", Mod.ReadResourceArray("Resources/faiaroo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOFUKIMUSHI_HANAZONO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kofukimushi_00";
graphicsEntry.AnimationName = "6leg_insect_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kofukimushi_00.ab", Mod.ReadResourceArray("Resources/kofukimushi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOFUKIMUSHI_MIYABI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kofukimushi_00";
graphicsEntry.AnimationName = "6leg_insect_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kofukimushi_00.ab", Mod.ReadResourceArray("Resources/kofukimushi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOFUURAI_HANAZONO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kofuurai_00";
graphicsEntry.AnimationName = "bound_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kofuurai_00.ab", Mod.ReadResourceArray("Resources/kofuurai_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KOFUURAI_MIYABI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kofuurai_00";
graphicsEntry.AnimationName = "bound_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kofuurai_00.ab", Mod.ReadResourceArray("Resources/kofuurai_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BIBIYON_HANAZONO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "bibiyon_hanazono_00";
graphicsEntry.AnimationName = "fly_bibiyon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/bibiyon_hanazono_00.ab", Mod.ReadResourceArray("Resources/bibiyon_hanazono_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BIBIYON_MIYABI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "bibiyon_miyabi_00";
graphicsEntry.AnimationName = "fly_bibiyon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/bibiyon_miyabi_00.ab", Mod.ReadResourceArray("Resources/bibiyon_miyabi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHISHIKO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "shishiko_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/shishiko_00.ab", Mod.ReadResourceArray("Resources/shishiko_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KAENJISHI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kaenjishi_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kaenjishi_00.ab", Mod.ReadResourceArray("Resources/kaenjishi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KAENJISHI, PokemonFormType.FEMALE);
graphicsEntry.ModelName = "kaenjishi_f_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kaenjishi_f_00.ab", Mod.ReadResourceArray("Resources/kaenjishi_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FURABEBE_RED, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "furabebe_red_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/furabebe_red_00.ab", Mod.ReadResourceArray("Resources/furabebe_red_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FURAETTE_RED, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "furaette_red_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/furaette_red_00.ab", Mod.ReadResourceArray("Resources/furaette_red_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FURAAJESU_RED, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "furaajesu_red_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/furaajesu_red_00.ab", Mod.ReadResourceArray("Resources/furaajesu_red_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MEEKURU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "meekuru_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/meekuru_00.ab", Mod.ReadResourceArray("Resources/meekuru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GOOGOOTO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "googooto_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/googooto_00.ab", Mod.ReadResourceArray("Resources/googooto_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YANCHAMU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "yanchamu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/yanchamu_00.ab", Mod.ReadResourceArray("Resources/yanchamu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GORONDA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "goronda_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/goronda_00.ab", Mod.ReadResourceArray("Resources/goronda_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.TORIMIAN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "torimian_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/torimian_00.ab", Mod.ReadResourceArray("Resources/torimian_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NYASUPAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "nyasupaa_00";
graphicsEntry.AnimationName = "2leg_beast_up_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/nyasupaa_00.ab", Mod.ReadResourceArray("Resources/nyasupaa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NYAONIKUSU_M, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "nyaonikusu_m_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/nyaonikusu_m_00.ab", Mod.ReadResourceArray("Resources/nyaonikusu_m_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NYAONIKUSU_F, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "nyaonikusu_f_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/nyaonikusu_f_00.ab", Mod.ReadResourceArray("Resources/nyaonikusu_f_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.HITOTSUKI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "hitotsuki_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/hitotsuki_00.ab", Mod.ReadResourceArray("Resources/hitotsuki_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NIDANGIRU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "nidangiru_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/nidangiru_00.ab", Mod.ReadResourceArray("Resources/nidangiru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GIRUGARUDO_S, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "girugarudo_s_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/girugarudo_s_00.ab", Mod.ReadResourceArray("Resources/girugarudo_s_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GIRUGARUDO_B, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "girugarudo_b_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/girugarudo_b_00.ab", Mod.ReadResourceArray("Resources/girugarudo_b_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.SHUSHUPU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "shushupu_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/shushupu_00.ab", Mod.ReadResourceArray("Resources/shushupu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FUREFUWAN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "furefuwan_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/furefuwan_00.ab", Mod.ReadResourceArray("Resources/furefuwan_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PEROPPAFU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "peroppafu_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/peroppafu_00.ab", Mod.ReadResourceArray("Resources/peroppafu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PERORIIMU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "peroriimu_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/peroriimu_00.ab", Mod.ReadResourceArray("Resources/peroriimu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MAAIIKA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "maaiika_00";
graphicsEntry.AnimationName = "fly_masukippa_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/maaiika_00.ab", Mod.ReadResourceArray("Resources/maaiika_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KARAMANERO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "karamanero_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/karamanero_00.ab", Mod.ReadResourceArray("Resources/karamanero_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KAMETETE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kametete_00";
graphicsEntry.AnimationName = "bound_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kametete_00.ab", Mod.ReadResourceArray("Resources/kametete_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GAMENODESU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gamenodesu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gamenodesu_00.ab", Mod.ReadResourceArray("Resources/gamenodesu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KUZUMOO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kuzumoo_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kuzumoo_00.ab", Mod.ReadResourceArray("Resources/kuzumoo_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DORAMIDORO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "doramidoro_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/doramidoro_00.ab", Mod.ReadResourceArray("Resources/doramidoro_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.UDEPPOU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "udeppou_00";
graphicsEntry.AnimationName = "6leg_crab_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/udeppou_00.ab", Mod.ReadResourceArray("Resources/udeppou_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BUROSUTAA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "burosutaa_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/burosutaa_00.ab", Mod.ReadResourceArray("Resources/burosutaa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ERIKITERU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "erikiteru_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/erikiteru_00.ab", Mod.ReadResourceArray("Resources/erikiteru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.EREZAADO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "erezaado_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/erezaado_00.ab", Mod.ReadResourceArray("Resources/erezaado_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.CHIGORASU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "chigorasu_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/chigorasu_00.ab", Mod.ReadResourceArray("Resources/chigorasu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.GACHIGORASU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "gachigorasu_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/gachigorasu_00.ab", Mod.ReadResourceArray("Resources/gachigorasu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.AMARUSU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "amarusu_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/amarusu_00.ab", Mod.ReadResourceArray("Resources/amarusu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.AMARURUGA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "amaruruga_00";
graphicsEntry.AnimationName = "4leg_beast_p2_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/amaruruga_00.ab", Mod.ReadResourceArray("Resources/amaruruga_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.RUCHABURU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "ruchaburu_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/ruchaburu_00.ab", Mod.ReadResourceArray("Resources/ruchaburu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DEDENNE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dedenne_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dedenne_00.ab", Mod.ReadResourceArray("Resources/dedenne_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.MERESHII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "mereshii_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/mereshii_00.ab", Mod.ReadResourceArray("Resources/mereshii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NUMERA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "numera_00";
graphicsEntry.AnimationName = "bound_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/numera_00.ab", Mod.ReadResourceArray("Resources/numera_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NUMEIRU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "numeiru_00";
graphicsEntry.AnimationName = "bound_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/numeiru_00.ab", Mod.ReadResourceArray("Resources/numeiru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.NUMERUGON, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "numerugon_00";
graphicsEntry.AnimationName = "2leg_dragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/numerugon_00.ab", Mod.ReadResourceArray("Resources/numerugon_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KUREFFI, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kureffi_00";
graphicsEntry.AnimationName = "float_small_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kureffi_00.ab", Mod.ReadResourceArray("Resources/kureffi_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BOKUREE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "bokuree_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/bokuree_00.ab", Mod.ReadResourceArray("Resources/bokuree_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.OOROTTO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "oorotto_00";
graphicsEntry.AnimationName = "6leg_crab_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/oorotto_00.ab", Mod.ReadResourceArray("Resources/oorotto_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BAKETCHA_M, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "baketcha_m_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/baketcha_m_00.ab", Mod.ReadResourceArray("Resources/baketcha_m_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BAKETCHA_S, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "baketcha_m_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/baketcha_m_00.ab", Mod.ReadResourceArray("Resources/baketcha_m_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BAKETCHA_L, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "baketcha_m_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/baketcha_m_00.ab", Mod.ReadResourceArray("Resources/baketcha_m_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.BAKETCHA_LL, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "baketcha_m_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/baketcha_m_00.ab", Mod.ReadResourceArray("Resources/baketcha_m_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PANPUJIN_M, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "panpujin_m_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/panpujin_m_00.ab", Mod.ReadResourceArray("Resources/panpujin_m_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PANPUJIN_S, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "panpujin_m_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/panpujin_m_00.ab", Mod.ReadResourceArray("Resources/panpujin_m_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PANPUJIN_L, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "panpujin_m_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/panpujin_m_00.ab", Mod.ReadResourceArray("Resources/panpujin_m_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.PANPUJIN_LL, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "panpujin_m_00";
graphicsEntry.AnimationName = "2leg_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/panpujin_m_00.ab", Mod.ReadResourceArray("Resources/panpujin_m_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KACHIKOORU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kachikooru_00";
graphicsEntry.AnimationName = "4leg_doragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kachikooru_00.ab", Mod.ReadResourceArray("Resources/kachikooru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.KUREBEESU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "kurebeesu_00";
graphicsEntry.AnimationName = "4leg_doragon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/kurebeesu_00.ab", Mod.ReadResourceArray("Resources/kurebeesu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ONBATTO, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "onbatto_00";
graphicsEntry.AnimationName = "fly_bat_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/onbatto_00.ab", Mod.ReadResourceArray("Resources/onbatto_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ONBAAN, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "onbaan_00";
graphicsEntry.AnimationName = "fly_bird_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/onbaan_00.ab", Mod.ReadResourceArray("Resources/onbaan_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ZERUNEASU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "zeruneasu_00";
graphicsEntry.AnimationName = "4leg_zeruneasu_r_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/zeruneasu_00.ab", Mod.ReadResourceArray("Resources/zeruneasu_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.ZERUNEASU_R, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "zeruneasu_r_00";
graphicsEntry.AnimationName = "4leg_zeruneasu_r_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/zeruneasu_r_00.ab", Mod.ReadResourceArray("Resources/zeruneasu_r_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.IBERUTARU, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "iberutaru_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/iberutaru_00.ab", Mod.ReadResourceArray("Resources/iberutaru_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.JIGARUDE, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "jigarude_00";
graphicsEntry.AnimationName = "dummypokemon_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/jigarude_00.ab", Mod.ReadResourceArray("Resources/jigarude_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DIANSHII, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dianshii_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dianshii_00.ab", Mod.ReadResourceArray("Resources/dianshii_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.DIANSHII_MEGA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "dianshii_mega_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/dianshii_mega_00.ab", Mod.ReadResourceArray("Resources/dianshii_mega_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FUUPA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "fuupa_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/fuupa_00.ab", Mod.ReadResourceArray("Resources/fuupa_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.FUUPA_MEGA, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "fuupa_mega_00";
graphicsEntry.AnimationName = "float_human_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/fuupa_mega_00.ab", Mod.ReadResourceArray("Resources/fuupa_mega_00.ab"));

graphicsEntry = GetGraphicsModelByCreatureAndForm(CreatureIndex.YOBI_26, PokemonFormType.NORMAL);
graphicsEntry.ModelName = "migawari_00";
graphicsEntry.AnimationName = "migawari_00";
Rom.WriteFile("romfs/Data/StreamingAssets/ab/migawari_00.ab", Mod.ReadResourceArray("Resources/migawari_00.ab"));

