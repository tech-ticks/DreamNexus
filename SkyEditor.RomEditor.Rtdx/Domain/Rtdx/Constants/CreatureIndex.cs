using SkyEditor.RomEditor.Domain.Automation.CSharp;
using SkyEditor.RomEditor.Domain.Automation.Lua;
using SkyEditor.RomEditor.Infrastructure;
using System;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public class CreatureIndexLuaExpressionGenerator : ILuaExpressionGenerator
    {
        public CreatureIndexLuaExpressionGenerator(ICommonStrings? commonStrings = null)
        {
            this.commonStrings = commonStrings;
        }

        private readonly ICommonStrings? commonStrings;

        public string Generate(object? obj)
        {
            if (!(obj is CreatureIndex index))
            {
                throw new ArgumentException("Unsupported value type");
            }

            string? friendlyName = commonStrings?.Pokemon?.GetValueOrDefault(index);
            if (!string.IsNullOrEmpty(friendlyName))
            {
                return $"Const.creature.Index.{obj:f} --[[{friendlyName}]]";
            }
            else
            {
                return $"Const.creature.Index.{obj:f}";
            }
        }
    }
    [LuaExpressionGenerator(typeof(CreatureIndexLuaExpressionGenerator))]
    [CSharpExpressionGenerator(typeof(CreatureIndexCSharpExpressionGenerator))]
    public enum CreatureIndex
    {
        NONE,

        /// <summary>
        /// Bulbasaur
        /// </summary>
        FUSHIGIDANE,

        /// <summary>
        /// Ivysaur
        /// </summary>
        FUSHIGISOU,

        /// <summary>
        /// Venusaur
        /// </summary>
        FUSHIGIBANA,

        /// <summary>
        /// Venusaur
        /// </summary>
        FUSHIGIBANA_MEGA,

        /// <summary>
        /// Charmander
        /// </summary>
        HITOKAGE,

        /// <summary>
        /// Charmeleon
        /// </summary>
        RIZAADO,

        /// <summary>
        /// Charizard
        /// </summary>
        RIZAADON,

        /// <summary>
        /// Charizard
        /// </summary>
        RIZAADON_MEGA_X,

        /// <summary>
        /// Charizard
        /// </summary>
        RIZAADON_MEGA_Y,

        /// <summary>
        /// Squirtle
        /// </summary>
        ZENIGAME,

        /// <summary>
        /// Wartortle
        /// </summary>
        KAMEERU,

        /// <summary>
        /// Blastoise
        /// </summary>
        KAMEKKUSU,

        /// <summary>
        /// Blastoise
        /// </summary>
        KAMEKKUSU_MEGA,

        /// <summary>
        /// Caterpie
        /// </summary>
        KYATAPII,

        /// <summary>
        /// Metapod
        /// </summary>
        TORANSERU,

        /// <summary>
        /// Butterfree
        /// </summary>
        BATAFURII,

        /// <summary>
        /// Weedle
        /// </summary>
        BIIDORU,

        /// <summary>
        /// Kakuna
        /// </summary>
        KOKUUN,

        /// <summary>
        /// Beedrill
        /// </summary>
        SUPIAA,

        /// <summary>
        /// Beedrill
        /// </summary>
        SUPIAA_MEGA,

        /// <summary>
        /// Pidgey
        /// </summary>
        POPPO,

        /// <summary>
        /// Pidgeotto
        /// </summary>
        PIJON,

        /// <summary>
        /// Pidgeot
        /// </summary>
        PIJOTTO,

        /// <summary>
        /// Pidgeot
        /// </summary>
        PIJOTTO_MEGA,

        /// <summary>
        /// Rattata
        /// </summary>
        KORATTA,

        /// <summary>
        /// Raticate
        /// </summary>
        RATTA,

        /// <summary>
        /// Spearow
        /// </summary>
        ONISUZUME,

        /// <summary>
        /// Fearow
        /// </summary>
        ONIDORIRU,

        /// <summary>
        /// Ekans
        /// </summary>
        AABO,

        /// <summary>
        /// Arbok
        /// </summary>
        AABOKKU,

        /// <summary>
        /// Pichu
        /// </summary>
        PICHUU,

        /// <summary>
        /// Pikachu
        /// </summary>
        PIKACHUU,

        /// <summary>
        /// Raichu
        /// </summary>
        RAICHUU,

        /// <summary>
        /// Sandshrew
        /// </summary>
        SANDO,

        /// <summary>
        /// Sandslash
        /// </summary>
        SANDOPAN,

        /// <summary>
        /// Nidoran♀
        /// </summary>
        NIDORAN_F,

        /// <summary>
        /// Nidorina
        /// </summary>
        NIDORIINA,

        /// <summary>
        /// Nidoqueen
        /// </summary>
        NIDOKUIN,

        /// <summary>
        /// Nidoran♂
        /// </summary>
        NIDORAN_M,

        /// <summary>
        /// Nidorino
        /// </summary>
        NIDORIINO,

        /// <summary>
        /// Nidoking
        /// </summary>
        NIDOKINGU,

        /// <summary>
        /// Cleffa
        /// </summary>
        PII,

        /// <summary>
        /// Clefairy
        /// </summary>
        PIPPI,

        /// <summary>
        /// Clefable
        /// </summary>
        PIKUSHII,

        /// <summary>
        /// Vulpix
        /// </summary>
        ROKON,

        /// <summary>
        /// Ninetales
        /// </summary>
        KYUUKON,

        /// <summary>
        /// Igglybuff
        /// </summary>
        PUPURIN,

        /// <summary>
        /// Jigglypuff
        /// </summary>
        PURIN,

        /// <summary>
        /// Wigglytuff
        /// </summary>
        PUKURIN,

        /// <summary>
        /// Zubat
        /// </summary>
        ZUBATTO,

        /// <summary>
        /// Golbat
        /// </summary>
        GORUBATTO,

        /// <summary>
        /// Crobat
        /// </summary>
        KUROBATTO,

        /// <summary>
        /// Oddish
        /// </summary>
        NAZONOKUSA,

        /// <summary>
        /// Gloom
        /// </summary>
        KUSAIHANA,

        /// <summary>
        /// Vileplume
        /// </summary>
        RAFURESHIA,

        /// <summary>
        /// Bellossom
        /// </summary>
        KIREIHANA,

        /// <summary>
        /// Paras
        /// </summary>
        PARASU,

        /// <summary>
        /// Parasect
        /// </summary>
        PARASEKUTO,

        /// <summary>
        /// Venonat
        /// </summary>
        KONPAN,

        /// <summary>
        /// Venomoth
        /// </summary>
        MORUFON,

        /// <summary>
        /// Diglett
        /// </summary>
        DIGUDA,

        /// <summary>
        /// Dugtrio
        /// </summary>
        DAGUTORIO,

        /// <summary>
        /// Meowth
        /// </summary>
        NYAASU,

        /// <summary>
        /// Persian
        /// </summary>
        PERUSHIAN,

        /// <summary>
        /// Psyduck
        /// </summary>
        KODAKKU,

        /// <summary>
        /// Golduck
        /// </summary>
        GORUDAKKU,

        /// <summary>
        /// Mankey
        /// </summary>
        MANKII,

        /// <summary>
        /// Primeape
        /// </summary>
        OKORIZARU,

        /// <summary>
        /// Growlithe
        /// </summary>
        GAADI,

        /// <summary>
        /// Arcanine
        /// </summary>
        UINDI,

        /// <summary>
        /// Poliwag
        /// </summary>
        NYOROMO,

        /// <summary>
        /// Poliwhirl
        /// </summary>
        NYOROZO,

        /// <summary>
        /// Poliwrath
        /// </summary>
        NYOROBON,

        /// <summary>
        /// Politoed
        /// </summary>
        NYOROTONO,

        /// <summary>
        /// Abra
        /// </summary>
        KEESHII,

        /// <summary>
        /// Kadabra
        /// </summary>
        YUNGERAA,

        /// <summary>
        /// Alakazam
        /// </summary>
        FUUDIN,

        /// <summary>
        /// Alakazam
        /// </summary>
        FUUDIN_MEGA,

        /// <summary>
        /// Machop
        /// </summary>
        WANRIKII,

        /// <summary>
        /// Machoke
        /// </summary>
        GOORIKII,

        /// <summary>
        /// Machamp
        /// </summary>
        KAIRIKII,

        /// <summary>
        /// Bellsprout
        /// </summary>
        MADATSUBOMI,

        /// <summary>
        /// Weepinbell
        /// </summary>
        UTSUDON,

        /// <summary>
        /// Victreebel
        /// </summary>
        UTSUBOTTO,

        /// <summary>
        /// Tentacool
        /// </summary>
        MENOKURAGE,

        /// <summary>
        /// Tentacruel
        /// </summary>
        DOKUKURAGE,

        /// <summary>
        /// Geodude
        /// </summary>
        ISHITSUBUTE,

        /// <summary>
        /// Graveler
        /// </summary>
        GOROON,

        /// <summary>
        /// Golem
        /// </summary>
        GOROONYA,

        /// <summary>
        /// Ponyta
        /// </summary>
        PONIITA,

        /// <summary>
        /// Rapidash
        /// </summary>
        GYAROPPU,

        /// <summary>
        /// Slowpoke
        /// </summary>
        YADON,

        /// <summary>
        /// Slowbro
        /// </summary>
        YADORAN,

        /// <summary>
        /// Slowbro
        /// </summary>
        YADORAN_MEGA,

        /// <summary>
        /// Slowking
        /// </summary>
        YADOKINGU,

        /// <summary>
        /// Magnemite
        /// </summary>
        KOIRU,

        /// <summary>
        /// Magneton
        /// </summary>
        REAKOIRU,

        /// <summary>
        /// Magnezone
        /// </summary>
        JIBAKOIRU,

        /// <summary>
        /// Farfetch'd
        /// </summary>
        KAMONEGI,

        /// <summary>
        /// Doduo
        /// </summary>
        DOODOO,

        /// <summary>
        /// Dodrio
        /// </summary>
        DOODORIO,

        /// <summary>
        /// Seel
        /// </summary>
        PAUWAU,

        /// <summary>
        /// Dewgong
        /// </summary>
        JUGON,

        /// <summary>
        /// Grimer
        /// </summary>
        BETOBETAA,

        /// <summary>
        /// Muk
        /// </summary>
        BETOBETON,

        /// <summary>
        /// Shellder
        /// </summary>
        SHERUDAA,

        /// <summary>
        /// Cloyster
        /// </summary>
        PARUSHEN,

        /// <summary>
        /// Gastly
        /// </summary>
        GOOSU,

        /// <summary>
        /// Haunter
        /// </summary>
        GOOSUTO,

        /// <summary>
        /// Gengar
        /// </summary>
        GENGAA,

        /// <summary>
        /// Gengar
        /// </summary>
        GENGAA_MEGA,

        /// <summary>
        /// Onix
        /// </summary>
        IWAAKU,

        /// <summary>
        /// Steelix
        /// </summary>
        HAGANEERU,

        /// <summary>
        /// Steelix
        /// </summary>
        HAGANEERU_MEGA,

        /// <summary>
        /// Drowzee
        /// </summary>
        SURIIPU,

        /// <summary>
        /// Hypno
        /// </summary>
        SURIIPAA,

        /// <summary>
        /// Krabby
        /// </summary>
        KURABU,

        /// <summary>
        /// Kingler
        /// </summary>
        KINGURAA,

        /// <summary>
        /// Voltorb
        /// </summary>
        BIRIRIDAMA,

        /// <summary>
        /// Electrode
        /// </summary>
        MARUMAIN,

        /// <summary>
        /// Exeggcute
        /// </summary>
        TAMATAMA,

        /// <summary>
        /// Exeggutor
        /// </summary>
        NASSHII,

        /// <summary>
        /// Cubone
        /// </summary>
        KARAKARA,

        /// <summary>
        /// Marowak
        /// </summary>
        GARAGARA,

        /// <summary>
        /// Tyrogue
        /// </summary>
        BARUKII,

        /// <summary>
        /// Hitmonlee
        /// </summary>
        SAWAMURAA,

        /// <summary>
        /// Hitmonchan
        /// </summary>
        EBIWARAA,

        /// <summary>
        /// Hitmontop
        /// </summary>
        KAPOERAA,

        /// <summary>
        /// Lickitung
        /// </summary>
        BERORINGA,

        /// <summary>
        /// Lickilicky
        /// </summary>
        BEROBERUTO,

        /// <summary>
        /// Koffing
        /// </summary>
        DOGAASU,

        /// <summary>
        /// Weezing
        /// </summary>
        MATADOGASU,

        /// <summary>
        /// Rhyhorn
        /// </summary>
        SAIHOON,

        /// <summary>
        /// Rhydon
        /// </summary>
        SAIDON,

        /// <summary>
        /// Rhyperior
        /// </summary>
        DOSAIDON,

        /// <summary>
        /// Happiny
        /// </summary>
        PINPUKU,

        /// <summary>
        /// Chansey
        /// </summary>
        RAKKII,

        /// <summary>
        /// Blissey
        /// </summary>
        HAPINASU,

        /// <summary>
        /// Tangela
        /// </summary>
        MONJARA,

        /// <summary>
        /// Tangrowth
        /// </summary>
        MOJANBO,

        /// <summary>
        /// Kangaskhan
        /// </summary>
        GARUURA,

        /// <summary>
        /// Kangaskhan
        /// </summary>
        GARUURA_MEGA,

        /// <summary>
        /// Horsea
        /// </summary>
        TATTSUU,

        /// <summary>
        /// Seadra
        /// </summary>
        SHIIDORA,

        /// <summary>
        /// Kingdra
        /// </summary>
        KINGUDORA,

        /// <summary>
        /// Goldeen
        /// </summary>
        TOSAKINTO,

        /// <summary>
        /// Seaking
        /// </summary>
        AZUMAOU,

        /// <summary>
        /// Staryu
        /// </summary>
        HITODEMAN,

        /// <summary>
        /// Starmie
        /// </summary>
        SUTAAMII,

        /// <summary>
        /// Mime Jr.
        /// </summary>
        MANENE,

        /// <summary>
        /// Mr. Mime
        /// </summary>
        BARIYAADO,

        /// <summary>
        /// Scyther
        /// </summary>
        SUTORAIKU,

        /// <summary>
        /// Scizor
        /// </summary>
        HASSAMU,

        /// <summary>
        /// Scizor
        /// </summary>
        HASSAMU_MEGA,

        /// <summary>
        /// Smoochum
        /// </summary>
        MUCHUURU,

        /// <summary>
        /// Jynx
        /// </summary>
        RUUJURA,

        /// <summary>
        /// Elekid
        /// </summary>
        EREKIDDO,

        /// <summary>
        /// Electabuzz
        /// </summary>
        EREBUU,

        /// <summary>
        /// Electivire
        /// </summary>
        EREKIBURU,

        /// <summary>
        /// Magby
        /// </summary>
        BUBII,

        /// <summary>
        /// Magmar
        /// </summary>
        BUUBAA,

        /// <summary>
        /// Magmortar
        /// </summary>
        BUUBAAN,

        /// <summary>
        /// Pinsir
        /// </summary>
        KAIROSU,

        /// <summary>
        /// Pinsir
        /// </summary>
        KAIROSU_MEGA,

        /// <summary>
        /// Tauros
        /// </summary>
        KENTAROSU,

        /// <summary>
        /// Magikarp
        /// </summary>
        KOIKINGU,

        /// <summary>
        /// Gyarados
        /// </summary>
        GYARADOSU,

        /// <summary>
        /// Gyarados
        /// </summary>
        GYARADOSU_MEGA,

        /// <summary>
        /// Lapras
        /// </summary>
        RAPURASU,

        /// <summary>
        /// Ditto
        /// </summary>
        METAMON,

        /// <summary>
        /// Eevee
        /// </summary>
        IIBUI,

        /// <summary>
        /// Vaporeon
        /// </summary>
        SHAWAAZU,

        /// <summary>
        /// Jolteon
        /// </summary>
        SANDAASU,

        /// <summary>
        /// Flareon
        /// </summary>
        BUUSUTAA,

        /// <summary>
        /// Espeon
        /// </summary>
        EEFI,

        /// <summary>
        /// Umbreon
        /// </summary>
        BURAKKII,

        /// <summary>
        /// Leafeon
        /// </summary>
        RIIFIA,

        /// <summary>
        /// Glaceon
        /// </summary>
        GUREISHIA,

        /// <summary>
        /// Sylveon
        /// </summary>
        NINFIA,

        /// <summary>
        /// Porygon
        /// </summary>
        PORIGON,

        /// <summary>
        /// Porygon2
        /// </summary>
        PORIGON2,

        /// <summary>
        /// Porygon-Z
        /// </summary>
        PORIGONZ,

        /// <summary>
        /// Omanyte
        /// </summary>
        OMUNAITO,

        /// <summary>
        /// Omastar
        /// </summary>
        OMUSUTAA,

        /// <summary>
        /// Kabuto
        /// </summary>
        KABUTO,

        /// <summary>
        /// Kabutops
        /// </summary>
        KABUTOPUSU,

        /// <summary>
        /// Aerodactyl
        /// </summary>
        PUTERA,

        /// <summary>
        /// Aerodactyl
        /// </summary>
        PUTERA_MEGA,

        /// <summary>
        /// Munchlax
        /// </summary>
        GONBE,

        /// <summary>
        /// Snorlax
        /// </summary>
        KABIGON,

        /// <summary>
        /// Articuno
        /// </summary>
        FURIIZAA,

        /// <summary>
        /// Zapdos
        /// </summary>
        SANDAA,

        /// <summary>
        /// Moltres
        /// </summary>
        FAIYAA,

        /// <summary>
        /// Dratini
        /// </summary>
        MINIRYUU,

        /// <summary>
        /// Dragonair
        /// </summary>
        HAKURYUU,

        /// <summary>
        /// Dragonite
        /// </summary>
        KAIRYUU,

        /// <summary>
        /// Mewtwo
        /// </summary>
        MYUUTSUU,

        /// <summary>
        /// Mewtwo
        /// </summary>
        MYUUTSUU_MEGA_X,

        /// <summary>
        /// Mewtwo
        /// </summary>
        MYUUTSUU_MEGA_Y,

        /// <summary>
        /// Mew
        /// </summary>
        MYUU,

        /// <summary>
        /// Chikorita
        /// </summary>
        CHIKORIITA,

        /// <summary>
        /// Bayleef
        /// </summary>
        BEIRIIFU,

        /// <summary>
        /// Meganium
        /// </summary>
        MEGANIUMU,

        /// <summary>
        /// Cyndaquil
        /// </summary>
        HINOARASHI,

        /// <summary>
        /// Quilava
        /// </summary>
        MAGUMARASHI,

        /// <summary>
        /// Typhlosion
        /// </summary>
        BAKUFUUN,

        /// <summary>
        /// Totodile
        /// </summary>
        WANINOKO,

        /// <summary>
        /// Croconaw
        /// </summary>
        ARIGEITSU,

        /// <summary>
        /// Feraligatr
        /// </summary>
        OODAIRU,

        /// <summary>
        /// Sentret
        /// </summary>
        OTACHI,

        /// <summary>
        /// Furret
        /// </summary>
        OOTACHI,

        /// <summary>
        /// Hoothoot
        /// </summary>
        HOOHOO,

        /// <summary>
        /// Noctowl
        /// </summary>
        YORUNOZUKU,

        /// <summary>
        /// Ledyba
        /// </summary>
        REDIBA,

        /// <summary>
        /// Ledian
        /// </summary>
        REDIAN,

        /// <summary>
        /// Spinarak
        /// </summary>
        ITOMARU,

        /// <summary>
        /// Ariados
        /// </summary>
        ARIADOSU,

        /// <summary>
        /// Chinchou
        /// </summary>
        CHONCHII,

        /// <summary>
        /// Lanturn
        /// </summary>
        RANTAAN,

        /// <summary>
        /// Togepi
        /// </summary>
        TOGEPII,

        /// <summary>
        /// Togetic
        /// </summary>
        TOGECHIKKU,

        /// <summary>
        /// Togekiss
        /// </summary>
        TOGEKISSU,

        /// <summary>
        /// Natu
        /// </summary>
        NEITYI,

        /// <summary>
        /// Xatu
        /// </summary>
        NEITYIO,

        /// <summary>
        /// Mareep
        /// </summary>
        MERIIPU,

        /// <summary>
        /// Flaaffy
        /// </summary>
        MOKOKO,

        /// <summary>
        /// Ampharos
        /// </summary>
        DENRYUU,

        /// <summary>
        /// Ampharos
        /// </summary>
        DENRYUU_MEGA,

        /// <summary>
        /// Azurill
        /// </summary>
        RURIRI,

        /// <summary>
        /// Marill
        /// </summary>
        MARIRU,

        /// <summary>
        /// Azumarill
        /// </summary>
        MARIRURI,

        /// <summary>
        /// Bonsly
        /// </summary>
        USOHACHI,

        /// <summary>
        /// Sudowoodo
        /// </summary>
        USOKKII,

        /// <summary>
        /// Hoppip
        /// </summary>
        HANEKKO,

        /// <summary>
        /// Skiploom
        /// </summary>
        POPOKKO,

        /// <summary>
        /// Jumpluff
        /// </summary>
        WATAKKO,

        /// <summary>
        /// Aipom
        /// </summary>
        EIPAMU,

        /// <summary>
        /// Ambipom
        /// </summary>
        ETEBOOSU,

        /// <summary>
        /// Sunkern
        /// </summary>
        HIMANATTSU,

        /// <summary>
        /// Sunflora
        /// </summary>
        KIMAWARI,

        /// <summary>
        /// Yanma
        /// </summary>
        YANYANMA,

        /// <summary>
        /// Yanmega
        /// </summary>
        MEGAYANMA,

        /// <summary>
        /// Wooper
        /// </summary>
        UPAA,

        /// <summary>
        /// Quagsire
        /// </summary>
        NUOO,

        /// <summary>
        /// Murkrow
        /// </summary>
        YAMIKARASU,

        /// <summary>
        /// Honchkrow
        /// </summary>
        DONKARASU,

        /// <summary>
        /// Misdreavus
        /// </summary>
        MUUMA,

        /// <summary>
        /// Mismagius
        /// </summary>
        MUUMAAJI,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_A,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_B,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_C,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_D,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_E,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_F,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_G,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_H,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_I,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_J,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_K,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_L,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_M,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_N,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_O,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_P,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_Q,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_R,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_S,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_T,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_U,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_V,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_W,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_X,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_Y,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_Z,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_EXC,

        /// <summary>
        /// Unown
        /// </summary>
        ANNOON_QUE,

        /// <summary>
        /// Wynaut
        /// </summary>
        SOONANO,

        /// <summary>
        /// Wobbuffet
        /// </summary>
        SOONANSU,

        /// <summary>
        /// Girafarig
        /// </summary>
        KIRINRIKI,

        /// <summary>
        /// Pineco
        /// </summary>
        KUNUGIDAMA,

        /// <summary>
        /// Forretress
        /// </summary>
        FORETOSU,

        /// <summary>
        /// Dunsparce
        /// </summary>
        NOKOTCHI,

        /// <summary>
        /// Gligar
        /// </summary>
        GURAIGAA,

        /// <summary>
        /// Gliscor
        /// </summary>
        GURAION,

        /// <summary>
        /// Snubbull
        /// </summary>
        BURUU,

        /// <summary>
        /// Granbull
        /// </summary>
        GURANBURU,

        /// <summary>
        /// Qwilfish
        /// </summary>
        HARIISEN,

        /// <summary>
        /// Shuckle
        /// </summary>
        TSUBOTSUBO,

        /// <summary>
        /// Heracross
        /// </summary>
        HERAKUROSU,

        /// <summary>
        /// Heracross
        /// </summary>
        HERAKUROSU_MEGA,

        /// <summary>
        /// Sneasel
        /// </summary>
        NYUURA,

        /// <summary>
        /// Weavile
        /// </summary>
        MANYUURA,

        /// <summary>
        /// Teddiursa
        /// </summary>
        HIMEGUMA,

        /// <summary>
        /// Ursaring
        /// </summary>
        RINGUMA,

        /// <summary>
        /// Slugma
        /// </summary>
        MAGUMAGGU,

        /// <summary>
        /// Magcargo
        /// </summary>
        MAGUKARUGO,

        /// <summary>
        /// Swinub
        /// </summary>
        URIMUU,

        /// <summary>
        /// Piloswine
        /// </summary>
        INOMUU,

        /// <summary>
        /// Mamoswine
        /// </summary>
        MANMUU,

        /// <summary>
        /// Corsola
        /// </summary>
        SANIIGO,

        /// <summary>
        /// Remoraid
        /// </summary>
        TEPPOUO,

        /// <summary>
        /// Octillery
        /// </summary>
        OKUTAN,

        /// <summary>
        /// Delibird
        /// </summary>
        DERIBAADO,

        /// <summary>
        /// Mantyke
        /// </summary>
        TAMANTA,

        /// <summary>
        /// Mantine
        /// </summary>
        MANTAIN,

        /// <summary>
        /// Skarmory
        /// </summary>
        EAAMUDO,

        /// <summary>
        /// Houndour
        /// </summary>
        DERUBIRU,

        /// <summary>
        /// Houndoom
        /// </summary>
        HERUGAA,

        /// <summary>
        /// Houndoom
        /// </summary>
        HERUGAA_MEGA,

        /// <summary>
        /// Phanpy
        /// </summary>
        GOMAZOU,

        /// <summary>
        /// Donphan
        /// </summary>
        DONFAN,

        /// <summary>
        /// Stantler
        /// </summary>
        ODOSHISHI,

        /// <summary>
        /// Smeargle
        /// </summary>
        DOOBURU,

        /// <summary>
        /// Miltank
        /// </summary>
        MIRUTANKU,

        /// <summary>
        /// Raikou
        /// </summary>
        RAIKOU,

        /// <summary>
        /// Entei
        /// </summary>
        ENTEI,

        /// <summary>
        /// Suicune
        /// </summary>
        SUIKUN,

        /// <summary>
        /// Larvitar
        /// </summary>
        YOOGIRASU,

        /// <summary>
        /// Pupitar
        /// </summary>
        SANAGIRASU,

        /// <summary>
        /// Tyranitar
        /// </summary>
        BANGIRASU,

        /// <summary>
        /// Tyranitar
        /// </summary>
        BANGIRASU_MEGA,

        /// <summary>
        /// Lugia
        /// </summary>
        RUGIA,

        /// <summary>
        /// Ho-Oh
        /// </summary>
        HOUOU,

        /// <summary>
        /// Celebi
        /// </summary>
        SEREBII,

        /// <summary>
        /// Celebi
        /// </summary>
        SEREBII_P,

        /// <summary>
        /// Treecko
        /// </summary>
        KIMORI,

        /// <summary>
        /// Grovyle
        /// </summary>
        JUPUTORU,

        /// <summary>
        /// Sceptile
        /// </summary>
        JUKAIN,

        /// <summary>
        /// Sceptile
        /// </summary>
        JUKAIN_MEGA,

        /// <summary>
        /// Torchic
        /// </summary>
        ACHAMO,

        /// <summary>
        /// Combusken
        /// </summary>
        WAKASHAMO,

        /// <summary>
        /// Blaziken
        /// </summary>
        BASHAAMO,

        /// <summary>
        /// Blaziken
        /// </summary>
        BASHAAMO_MEGA,

        /// <summary>
        /// Mudkip
        /// </summary>
        MIZUGOROU,

        /// <summary>
        /// Marshtomp
        /// </summary>
        NUMAKUROO,

        /// <summary>
        /// Swampert
        /// </summary>
        RAGURAAJI,

        /// <summary>
        /// Swampert
        /// </summary>
        RAGURAAJI_MEGA,

        /// <summary>
        /// Poochyena
        /// </summary>
        POCHIENA,

        /// <summary>
        /// Mightyena
        /// </summary>
        GURAENA,

        /// <summary>
        /// Zigzagoon
        /// </summary>
        JIGUZAGUMA,

        /// <summary>
        /// Linoone
        /// </summary>
        MASSUGUMA,

        /// <summary>
        /// Wurmple
        /// </summary>
        KEMUSSO_KARA,

        /// <summary>
        /// Wurmple
        /// </summary>
        KEMUSSO_MAYU,

        /// <summary>
        /// Silcoon
        /// </summary>
        KARASARISU,

        /// <summary>
        /// Beautifly
        /// </summary>
        AGEHANTO,

        /// <summary>
        /// Cascoon
        /// </summary>
        MAYURUDO,

        /// <summary>
        /// Dustox
        /// </summary>
        DOKUKEIRU,

        /// <summary>
        /// Lotad
        /// </summary>
        HASUBOO,

        /// <summary>
        /// Lombre
        /// </summary>
        HASUBURERO,

        /// <summary>
        /// Ludicolo
        /// </summary>
        RUNPAPPA,

        /// <summary>
        /// Seedot
        /// </summary>
        TANEBOO,

        /// <summary>
        /// Nuzleaf
        /// </summary>
        KONOHANA,

        /// <summary>
        /// Shiftry
        /// </summary>
        DAATENGU,

        /// <summary>
        /// Taillow
        /// </summary>
        SUBAME,

        /// <summary>
        /// Swellow
        /// </summary>
        OOSUBAME,

        /// <summary>
        /// Wingull
        /// </summary>
        KYAMOME,

        /// <summary>
        /// Pelipper
        /// </summary>
        PERIPPAA,

        /// <summary>
        /// Ralts
        /// </summary>
        RARUTOSU,

        /// <summary>
        /// Kirlia
        /// </summary>
        KIRURIA,

        /// <summary>
        /// Gardevoir
        /// </summary>
        SAANAITO,

        /// <summary>
        /// Gardevoir
        /// </summary>
        SAANAITO_MEGA,

        /// <summary>
        /// Gallade
        /// </summary>
        ERUREIDO,

        /// <summary>
        /// Gallade
        /// </summary>
        ERUREIDO_MEGA,

        /// <summary>
        /// Surskit
        /// </summary>
        AMETAMA,

        /// <summary>
        /// Masquerain
        /// </summary>
        AMEMOOSU,

        /// <summary>
        /// Shroomish
        /// </summary>
        KINOKOKO,

        /// <summary>
        /// Breloom
        /// </summary>
        KINOGASSA,

        /// <summary>
        /// Slakoth
        /// </summary>
        NAMAKERO,

        /// <summary>
        /// Vigoroth
        /// </summary>
        YARUKIMONO,

        /// <summary>
        /// Slaking
        /// </summary>
        KEKKINGU,

        /// <summary>
        /// Nincada
        /// </summary>
        TSUCHININ,

        /// <summary>
        /// Ninjask
        /// </summary>
        TEKKANIN,

        /// <summary>
        /// Shedinja
        /// </summary>
        NUKENIN,

        /// <summary>
        /// Whismur
        /// </summary>
        GONYONYO,

        /// <summary>
        /// Loudred
        /// </summary>
        DOGOOMU,

        /// <summary>
        /// Exploud
        /// </summary>
        BAKUONGU,

        /// <summary>
        /// Makuhita
        /// </summary>
        MAKUNOSHITA,

        /// <summary>
        /// Hariyama
        /// </summary>
        HARITEYAMA,

        /// <summary>
        /// Nosepass
        /// </summary>
        NOZUPASU,

        /// <summary>
        /// Probopass
        /// </summary>
        DAINOOZU,

        /// <summary>
        /// Skitty
        /// </summary>
        ENEKO,

        /// <summary>
        /// Delcatty
        /// </summary>
        ENEKORORO,

        /// <summary>
        /// Sableye
        /// </summary>
        YAMIRAMI,

        /// <summary>
        /// Sableye
        /// </summary>
        YAMIRAMI_MEGA,

        /// <summary>
        /// Mawile
        /// </summary>
        KUCHIITO,

        /// <summary>
        /// Mawile
        /// </summary>
        KUCHIITO_MEGA,

        /// <summary>
        /// Aron
        /// </summary>
        KOKODORA,

        /// <summary>
        /// Lairon
        /// </summary>
        KODORA,

        /// <summary>
        /// Aggron
        /// </summary>
        BOSUGODORA,

        /// <summary>
        /// Aggron
        /// </summary>
        BOSUGODORA_MEGA,

        /// <summary>
        /// Meditite
        /// </summary>
        ASANAN,

        /// <summary>
        /// Medicham
        /// </summary>
        CHAAREMU,

        /// <summary>
        /// Medicham
        /// </summary>
        CHAAREMU_MEGA,

        /// <summary>
        /// Electrike
        /// </summary>
        RAKURAI,

        /// <summary>
        /// Manectric
        /// </summary>
        RAIBORUTO,

        /// <summary>
        /// Manectric
        /// </summary>
        RAIBORUTO_MEGA,

        /// <summary>
        /// Plusle
        /// </summary>
        PURASURU,

        /// <summary>
        /// Minun
        /// </summary>
        MAINAN,

        /// <summary>
        /// Volbeat
        /// </summary>
        BARUBIITO,

        /// <summary>
        /// Illumise
        /// </summary>
        IRUMIIZE,

        /// <summary>
        /// Budew
        /// </summary>
        SUBOMII,

        /// <summary>
        /// Roselia
        /// </summary>
        ROZERIA,

        /// <summary>
        /// Roserade
        /// </summary>
        ROZUREIDO,

        /// <summary>
        /// Gulpin
        /// </summary>
        GOKURIN,

        /// <summary>
        /// Swalot
        /// </summary>
        MARUNOOMU,

        /// <summary>
        /// Carvanha
        /// </summary>
        KIBANIA,

        /// <summary>
        /// Sharpedo
        /// </summary>
        SAMEHADAA,

        /// <summary>
        /// Sharpedo
        /// </summary>
        SAMEHADAA_MEGA,

        /// <summary>
        /// Wailmer
        /// </summary>
        HOERUKO,

        /// <summary>
        /// Wailord
        /// </summary>
        HOERUOO,

        /// <summary>
        /// Numel
        /// </summary>
        DONMERU,

        /// <summary>
        /// Camerupt
        /// </summary>
        BAKUUDA,

        /// <summary>
        /// Camerupt
        /// </summary>
        BAKUUDA_MEGA,

        /// <summary>
        /// Torkoal
        /// </summary>
        KOOTASU,

        /// <summary>
        /// Spoink
        /// </summary>
        BANEBUU,

        /// <summary>
        /// Grumpig
        /// </summary>
        BUUPIGGU,

        /// <summary>
        /// Spinda
        /// </summary>
        PATCHIIRU,

        /// <summary>
        /// Trapinch
        /// </summary>
        NAKKURAA,

        /// <summary>
        /// Vibrava
        /// </summary>
        BIBURAABA,

        /// <summary>
        /// Flygon
        /// </summary>
        FURAIGON,

        /// <summary>
        /// Cacnea
        /// </summary>
        SABONEA,

        /// <summary>
        /// Cacturne
        /// </summary>
        NOKUTASU,

        /// <summary>
        /// Swablu
        /// </summary>
        CHIRUTTO,

        /// <summary>
        /// Altaria
        /// </summary>
        CHIRUTARISU,

        /// <summary>
        /// Altaria
        /// </summary>
        CHIRUTARISU_MEGA,

        /// <summary>
        /// Zangoose
        /// </summary>
        ZANGUUSU,

        /// <summary>
        /// Seviper
        /// </summary>
        HABUNEEKU,

        /// <summary>
        /// Lunatone
        /// </summary>
        RUNATOON,

        /// <summary>
        /// Solrock
        /// </summary>
        SORUROKKU,

        /// <summary>
        /// Barboach
        /// </summary>
        DOJOTCHI,

        /// <summary>
        /// Whiscash
        /// </summary>
        NAMAZUN,

        /// <summary>
        /// Corphish
        /// </summary>
        HEIGANI,

        /// <summary>
        /// Crawdaunt
        /// </summary>
        SHIZARIGAA,

        /// <summary>
        /// Baltoy
        /// </summary>
        YAJIRON,

        /// <summary>
        /// Claydol
        /// </summary>
        NENDOORU,

        /// <summary>
        /// Lileep
        /// </summary>
        RIRIIRA,

        /// <summary>
        /// Cradily
        /// </summary>
        YUREIDORU,

        /// <summary>
        /// Anorith
        /// </summary>
        ANOPUSU,

        /// <summary>
        /// Armaldo
        /// </summary>
        AAMARUDO,

        /// <summary>
        /// Feebas
        /// </summary>
        HINBASU,

        /// <summary>
        /// Milotic
        /// </summary>
        MIROKAROSU,

        /// <summary>
        /// Castform
        /// </summary>
        POWARUN_NORMAL,

        /// <summary>
        /// Castform
        /// </summary>
        POWARUN_WATER,

        /// <summary>
        /// Castform
        /// </summary>
        POWARUN_FIRE,

        /// <summary>
        /// Castform
        /// </summary>
        POWARUN_ICE,

        /// <summary>
        /// Kecleon
        /// </summary>
        KAKUREON,

        /// <summary>
        /// Shuppet
        /// </summary>
        KAGEBOUZU,

        /// <summary>
        /// Banette
        /// </summary>
        JUPETTA,

        /// <summary>
        /// Banette
        /// </summary>
        JUPETTA_MEGA,

        /// <summary>
        /// Duskull
        /// </summary>
        YOMAWARU,

        /// <summary>
        /// Dusclops
        /// </summary>
        SAMAYOORU,

        /// <summary>
        /// Dusknoir
        /// </summary>
        YONOWAARU,

        /// <summary>
        /// Tropius
        /// </summary>
        TOROPIUSU,

        /// <summary>
        /// Chingling
        /// </summary>
        RIISHAN,

        /// <summary>
        /// Chimecho
        /// </summary>
        CHIRIIN,

        /// <summary>
        /// Absol
        /// </summary>
        ABUSORU,

        /// <summary>
        /// Absol
        /// </summary>
        ABUSORU_MEGA,

        /// <summary>
        /// Snorunt
        /// </summary>
        YUKIWARASHI,

        /// <summary>
        /// Glalie
        /// </summary>
        ONIGOORI,

        /// <summary>
        /// Glalie
        /// </summary>
        ONIGOORI_MEGA,

        /// <summary>
        /// Froslass
        /// </summary>
        YUKIMENOKO,

        /// <summary>
        /// Spheal
        /// </summary>
        TAMAZARASHI,

        /// <summary>
        /// Sealeo
        /// </summary>
        TODOGURAA,

        /// <summary>
        /// Walrein
        /// </summary>
        TODOZERUGA,

        /// <summary>
        /// Clamperl
        /// </summary>
        PAARURU,

        /// <summary>
        /// Huntail
        /// </summary>
        HANTEERU,

        /// <summary>
        /// Gorebyss
        /// </summary>
        SAKURABISU,

        /// <summary>
        /// Relicanth
        /// </summary>
        JIIRANSU,

        /// <summary>
        /// Luvdisc
        /// </summary>
        RABUKASU,

        /// <summary>
        /// Bagon
        /// </summary>
        TATSUBEI,

        /// <summary>
        /// Shelgon
        /// </summary>
        KOMORUU,

        /// <summary>
        /// Salamence
        /// </summary>
        BOOMANDA,

        /// <summary>
        /// Salamence
        /// </summary>
        BOOMANDA_MEGA,

        /// <summary>
        /// Beldum
        /// </summary>
        DANBARU,

        /// <summary>
        /// Metang
        /// </summary>
        METANGU,

        /// <summary>
        /// Metagross
        /// </summary>
        METAGUROSU,

        /// <summary>
        /// Metagross
        /// </summary>
        METAGUROSU_MEGA,

        /// <summary>
        /// Regirock
        /// </summary>
        REJIROKKU,

        /// <summary>
        /// Regice
        /// </summary>
        REJIAISU,

        /// <summary>
        /// Registeel
        /// </summary>
        REJISUCHIRU,

        /// <summary>
        /// Latias
        /// </summary>
        RATYIASU,

        /// <summary>
        /// Latias
        /// </summary>
        RATYIASU_MEGA,

        /// <summary>
        /// Latios
        /// </summary>
        RATYIOSU,

        /// <summary>
        /// Latios
        /// </summary>
        RATYIOSU_MEGA,

        /// <summary>
        /// Kyogre
        /// </summary>
        KAIOOGA,

        /// <summary>
        /// Kyogre
        /// </summary>
        KAIOOGA_SP,

        /// <summary>
        /// Groudon
        /// </summary>
        GURAADON,

        /// <summary>
        /// Groudon
        /// </summary>
        GURAADON_SP,

        /// <summary>
        /// Rayquaza
        /// </summary>
        REKKUUZA,

        /// <summary>
        /// Rayquaza
        /// </summary>
        REKKUUZA_MEGA,

        /// <summary>
        /// Jirachi
        /// </summary>
        JIRAACHI,

        /// <summary>
        /// Deoxys
        /// </summary>
        DEOKISHISU_N,

        /// <summary>
        /// Deoxys
        /// </summary>
        DEOKISHISU_D,

        /// <summary>
        /// Deoxys
        /// </summary>
        DEOKISHISU_A,

        /// <summary>
        /// Deoxys
        /// </summary>
        DEOKISHISU_S,

        /// <summary>
        /// Turtwig
        /// </summary>
        NAETORU,

        /// <summary>
        /// Grotle
        /// </summary>
        HAYASHIGAME,

        /// <summary>
        /// Torterra
        /// </summary>
        DODAITOSU,

        /// <summary>
        /// Chimchar
        /// </summary>
        HIKOZARU,

        /// <summary>
        /// Monferno
        /// </summary>
        MOUKAZARU,

        /// <summary>
        /// Infernape
        /// </summary>
        GOUKAZARU,

        /// <summary>
        /// Piplup
        /// </summary>
        POTCHAMA,

        /// <summary>
        /// Prinplup
        /// </summary>
        POTTAISHI,

        /// <summary>
        /// Empoleon
        /// </summary>
        ENPERUTO,

        /// <summary>
        /// Starly
        /// </summary>
        MUKKURU,

        /// <summary>
        /// Staravia
        /// </summary>
        MUKUBAADO,

        /// <summary>
        /// Staraptor
        /// </summary>
        MUKUHOOKU,

        /// <summary>
        /// Bidoof
        /// </summary>
        BIPPA,

        /// <summary>
        /// Bibarel
        /// </summary>
        BIIDARU,

        /// <summary>
        /// Kricketot
        /// </summary>
        KOROBOOSHI,

        /// <summary>
        /// Kricketune
        /// </summary>
        KOROTOKKU,

        /// <summary>
        /// Shinx
        /// </summary>
        KORINKU,

        /// <summary>
        /// Luxio
        /// </summary>
        RUKUSHIO,

        /// <summary>
        /// Luxray
        /// </summary>
        RENTORAA,

        /// <summary>
        /// Cranidos
        /// </summary>
        ZUGAIDOSU,

        /// <summary>
        /// Rampardos
        /// </summary>
        RAMUPARUDO,

        /// <summary>
        /// Shieldon
        /// </summary>
        TATETOPUSU,

        /// <summary>
        /// Bastiodon
        /// </summary>
        TORIDEPUSU,

        /// <summary>
        /// Burmy
        /// </summary>
        MINOMUTCHI_GRASS,

        /// <summary>
        /// Burmy
        /// </summary>
        MINOMUTCHI_SAND,

        /// <summary>
        /// Burmy
        /// </summary>
        MINOMUTCHI_DUST,

        /// <summary>
        /// Wormadam
        /// </summary>
        MINOMADAMU_GRASS,

        /// <summary>
        /// Wormadam
        /// </summary>
        MINOMADAMU_SAND,

        /// <summary>
        /// Wormadam
        /// </summary>
        MINOMADAMU_DUST,

        /// <summary>
        /// Mothim
        /// </summary>
        GAAMEIRU,

        /// <summary>
        /// Combee
        /// </summary>
        MITSUHANII,

        /// <summary>
        /// Vespiquen
        /// </summary>
        BIIKUIN,

        /// <summary>
        /// Pachirisu
        /// </summary>
        PACHIRISU,

        /// <summary>
        /// Buizel
        /// </summary>
        BUIZERU,

        /// <summary>
        /// Floatzel
        /// </summary>
        FUROOZERU,

        /// <summary>
        /// Cherubi
        /// </summary>
        CHERINBO,

        /// <summary>
        /// Cherrim
        /// </summary>
        CHERIMU_N,

        /// <summary>
        /// Cherrim
        /// </summary>
        CHERIMU_P,

        /// <summary>
        /// Shellos
        /// </summary>
        KARANAKUSHI_E,

        /// <summary>
        /// Shellos
        /// </summary>
        KARANAKUSHI_W,

        /// <summary>
        /// Gastrodon
        /// </summary>
        TORITODON_E,

        /// <summary>
        /// Gastrodon
        /// </summary>
        TORITODON_W,

        /// <summary>
        /// Drifloon
        /// </summary>
        FUWANTE,

        /// <summary>
        /// Drifblim
        /// </summary>
        FUWARAIDO,

        /// <summary>
        /// Buneary
        /// </summary>
        MIMIRORU,

        /// <summary>
        /// Lopunny
        /// </summary>
        MIMIROPPU,

        /// <summary>
        /// Lopunny
        /// </summary>
        MIMIROPPU_MEGA,

        /// <summary>
        /// Glameow
        /// </summary>
        NYARUMAA,

        /// <summary>
        /// Purugly
        /// </summary>
        BUNYATTO,

        /// <summary>
        /// Stunky
        /// </summary>
        SUKANPUU,

        /// <summary>
        /// Skuntank
        /// </summary>
        SUKATANKU,

        /// <summary>
        /// Bronzor
        /// </summary>
        DOOMIRAA,

        /// <summary>
        /// Bronzong
        /// </summary>
        DOOTAKUN,

        /// <summary>
        /// Chatot
        /// </summary>
        PERAPPU,

        /// <summary>
        /// Spiritomb
        /// </summary>
        MIKARUGE,

        /// <summary>
        /// Gible
        /// </summary>
        FUKAMARU,

        /// <summary>
        /// Gabite
        /// </summary>
        GABAITO,

        /// <summary>
        /// Garchomp
        /// </summary>
        GABURIASU,

        /// <summary>
        /// Garchomp
        /// </summary>
        GABURIASU_MEGA,

        /// <summary>
        /// Riolu
        /// </summary>
        RIORU,

        /// <summary>
        /// Lucario
        /// </summary>
        RUKARIO,

        /// <summary>
        /// Lucario
        /// </summary>
        RUKARIO_MEGA,

        /// <summary>
        /// Hippopotas
        /// </summary>
        HIPOPOTASU,

        /// <summary>
        /// Hippowdon
        /// </summary>
        KABARUDON,

        /// <summary>
        /// Skorupi
        /// </summary>
        SUKORUPI,

        /// <summary>
        /// Drapion
        /// </summary>
        DORAPION,

        /// <summary>
        /// Croagunk
        /// </summary>
        GUREGGURU,

        /// <summary>
        /// Toxicroak
        /// </summary>
        DOKUROGGU,

        /// <summary>
        /// Carnivine
        /// </summary>
        MASUKIPPA,

        /// <summary>
        /// Finneon
        /// </summary>
        KEIKOUO,

        /// <summary>
        /// Lumineon
        /// </summary>
        NEORANTO,

        /// <summary>
        /// Snover
        /// </summary>
        YUKIKABURI,

        /// <summary>
        /// Abomasnow
        /// </summary>
        YUKINOOO,

        /// <summary>
        /// Abomasnow
        /// </summary>
        YUKINOOO_MEGA,

        /// <summary>
        /// Rotom
        /// </summary>
        ROTOMU,

        /// <summary>
        /// Rotom
        /// </summary>
        ROTOMU_HEAT,

        /// <summary>
        /// Rotom
        /// </summary>
        ROTOMU_SPIN,

        /// <summary>
        /// Rotom
        /// </summary>
        ROTOMU_FROST,

        /// <summary>
        /// Rotom
        /// </summary>
        ROTOMU_WASH,

        /// <summary>
        /// Rotom
        /// </summary>
        ROTOMU_CUT,

        /// <summary>
        /// Uxie
        /// </summary>
        YUKUSHII,

        /// <summary>
        /// Mesprit
        /// </summary>
        EMURITTO,

        /// <summary>
        /// Azelf
        /// </summary>
        AGUNOMU,

        /// <summary>
        /// Dialga
        /// </summary>
        DIARUGA,

        /// <summary>
        /// Palkia
        /// </summary>
        PARUKIA,

        /// <summary>
        /// Heatran
        /// </summary>
        HIIDORAN,

        /// <summary>
        /// Regigigas
        /// </summary>
        REJIGIGASU,

        /// <summary>
        /// Giratina
        /// </summary>
        GIRATYINA_A,

        /// <summary>
        /// Giratina
        /// </summary>
        GIRATYINA_O,

        /// <summary>
        /// Cresselia
        /// </summary>
        KURESERIA,

        /// <summary>
        /// Phione
        /// </summary>
        FIONE,

        /// <summary>
        /// Manaphy
        /// </summary>
        MANAFI,

        /// <summary>
        /// Darkrai
        /// </summary>
        DAAKURAI,

        /// <summary>
        /// Shaymin
        /// </summary>
        SHEIMI_L,

        /// <summary>
        /// Shaymin
        /// </summary>
        SHEIMI_S,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_NORMAL,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_GRASS,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_FIRE,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_WATER,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_ELECTRIC,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_ICE,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_FIGHTING,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_POISON,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_GROUND,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_FLYING,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_PSYCHIC,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_BUG,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_ROCK,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_GHOST,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_DRAGON,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_DARK,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_STEEL,

        /// <summary>
        /// Arceus
        /// </summary>
        ARUSEUSU_FAIRY,

        /// <summary>
        /// Victini
        /// </summary>
        BIKUTYINI,

        /// <summary>
        /// Snivy
        /// </summary>
        TSUTAAJA,

        /// <summary>
        /// Servine
        /// </summary>
        JANOBII,

        /// <summary>
        /// Serperior
        /// </summary>
        JAROODA,

        /// <summary>
        /// Tepig
        /// </summary>
        POKABU,

        /// <summary>
        /// Pignite
        /// </summary>
        CHAOBUU,

        /// <summary>
        /// Emboar
        /// </summary>
        ENBUOO,

        /// <summary>
        /// Oshawott
        /// </summary>
        MIJUMARU,

        /// <summary>
        /// Dewott
        /// </summary>
        FUTACHIMARU,

        /// <summary>
        /// Samurott
        /// </summary>
        DAIKENKI,

        /// <summary>
        /// Patrat
        /// </summary>
        MINEZUMI,

        /// <summary>
        /// Watchog
        /// </summary>
        MIRUHOGGU,

        /// <summary>
        /// Lillipup
        /// </summary>
        YOOTERII,

        /// <summary>
        /// Herdier
        /// </summary>
        HAADERIA,

        /// <summary>
        /// Stoutland
        /// </summary>
        MUURANDO,

        /// <summary>
        /// Purrloin
        /// </summary>
        CHORONEKO,

        /// <summary>
        /// Liepard
        /// </summary>
        REPARUDASU,

        /// <summary>
        /// Pansage
        /// </summary>
        YANAPPU,

        /// <summary>
        /// Simisage
        /// </summary>
        YANAKKII,

        /// <summary>
        /// Pansear
        /// </summary>
        BAOPPU,

        /// <summary>
        /// Simisear
        /// </summary>
        BAOKKII,

        /// <summary>
        /// Panpour
        /// </summary>
        HIYAPPU,

        /// <summary>
        /// Simipour
        /// </summary>
        HIYAKKII,

        /// <summary>
        /// Munna
        /// </summary>
        MUNNA,

        /// <summary>
        /// Musharna
        /// </summary>
        MUSHAANA,

        /// <summary>
        /// Pidove
        /// </summary>
        MAMEPATO,

        /// <summary>
        /// Tranquill
        /// </summary>
        HATOOBOOO,

        /// <summary>
        /// Unfezant
        /// </summary>
        KENHOROU,

        /// <summary>
        /// Blitzle
        /// </summary>
        SHIMAMA,

        /// <summary>
        /// Zebstrika
        /// </summary>
        ZEBURAIKA,

        /// <summary>
        /// Roggenrola
        /// </summary>
        DANGORO,

        /// <summary>
        /// Boldore
        /// </summary>
        GANTORU,

        /// <summary>
        /// Gigalith
        /// </summary>
        GIGAIASU,

        /// <summary>
        /// Woobat
        /// </summary>
        KOROMORI,

        /// <summary>
        /// Swoobat
        /// </summary>
        KOKOROMORI,

        /// <summary>
        /// Drilbur
        /// </summary>
        MOGURYUU,

        /// <summary>
        /// Excadrill
        /// </summary>
        DORYUUZU,

        /// <summary>
        /// Audino
        /// </summary>
        TABUNNE,

        /// <summary>
        /// Audino
        /// </summary>
        TABUNNE_MEGA,

        /// <summary>
        /// Timburr
        /// </summary>
        DOKKORAA,

        /// <summary>
        /// Gurdurr
        /// </summary>
        DOTEKKOTSU,

        /// <summary>
        /// Conkeldurr
        /// </summary>
        ROOBUSHIN,

        /// <summary>
        /// Tympole
        /// </summary>
        OTAMARO,

        /// <summary>
        /// Palpitoad
        /// </summary>
        GAMAGARU,

        /// <summary>
        /// Seismitoad
        /// </summary>
        GAMAGEROGE,

        /// <summary>
        /// Throh
        /// </summary>
        NAGEKI,

        /// <summary>
        /// Sawk
        /// </summary>
        DAGEKI,

        /// <summary>
        /// Sewaddle
        /// </summary>
        KURUMIRU,

        /// <summary>
        /// Swadloon
        /// </summary>
        KURUMAYU,

        /// <summary>
        /// Leavanny
        /// </summary>
        HAHAKOMORI,

        /// <summary>
        /// Venipede
        /// </summary>
        FUSHIDE,

        /// <summary>
        /// Whirlipede
        /// </summary>
        HOIIGA,

        /// <summary>
        /// Scolipede
        /// </summary>
        PENDORAA,

        /// <summary>
        /// Cottonee
        /// </summary>
        MONMEN,

        /// <summary>
        /// Whimsicott
        /// </summary>
        ERUFUUN,

        /// <summary>
        /// Petilil
        /// </summary>
        CHURINE,

        /// <summary>
        /// Lilligant
        /// </summary>
        DOREDYIA,

        /// <summary>
        /// Basculin
        /// </summary>
        BASURAO_R,

        /// <summary>
        /// Basculin
        /// </summary>
        BASURAO_B,

        /// <summary>
        /// Sandile
        /// </summary>
        MEGUROKO,

        /// <summary>
        /// Krokorok
        /// </summary>
        WARUBIRU,

        /// <summary>
        /// Krookodile
        /// </summary>
        WARUBIARU,

        /// <summary>
        /// Darumaka
        /// </summary>
        DARUMAKKA,

        /// <summary>
        /// Darmanitan
        /// </summary>
        HIHIDARUMA_N,

        /// <summary>
        /// Darmanitan
        /// </summary>
        HIHIDARUMA_D,

        /// <summary>
        /// Maractus
        /// </summary>
        MARAKATCHI,

        /// <summary>
        /// Dwebble
        /// </summary>
        ISHIZUMAI,

        /// <summary>
        /// Crustle
        /// </summary>
        IWAPARESU,

        /// <summary>
        /// Scraggy
        /// </summary>
        ZURUGGU,

        /// <summary>
        /// Scrafty
        /// </summary>
        ZURUZUKIN,

        /// <summary>
        /// Sigilyph
        /// </summary>
        SHINBORAA,

        /// <summary>
        /// Yamask
        /// </summary>
        DESUMASU,

        /// <summary>
        /// Cofagrigus
        /// </summary>
        DESUKAAN,

        /// <summary>
        /// Tirtouga
        /// </summary>
        PUROTOOGA,

        /// <summary>
        /// Carracosta
        /// </summary>
        ABAGOORA,

        /// <summary>
        /// Archen
        /// </summary>
        AAKEN,

        /// <summary>
        /// Archeops
        /// </summary>
        AAKEOSU,

        /// <summary>
        /// Trubbish
        /// </summary>
        YABUKURON,

        /// <summary>
        /// Garbodor
        /// </summary>
        DASUTODASU,

        /// <summary>
        /// Zorua
        /// </summary>
        ZOROA,

        /// <summary>
        /// Zoroark
        /// </summary>
        ZOROAAKU,

        /// <summary>
        /// Minccino
        /// </summary>
        CHIRAAMII,

        /// <summary>
        /// Cinccino
        /// </summary>
        CHIRACHIINO,

        /// <summary>
        /// Gothita
        /// </summary>
        GOCHIMU,

        /// <summary>
        /// Gothorita
        /// </summary>
        GOCHIMIRU,

        /// <summary>
        /// Gothitelle
        /// </summary>
        GOCHIRUZERU,

        /// <summary>
        /// Solosis
        /// </summary>
        YUNIRAN,

        /// <summary>
        /// Duosion
        /// </summary>
        DABURAN,

        /// <summary>
        /// Reuniclus
        /// </summary>
        RANKURUSU,

        /// <summary>
        /// Ducklett
        /// </summary>
        KOARUHII,

        /// <summary>
        /// Swanna
        /// </summary>
        SUWANNA,

        /// <summary>
        /// Vanillite
        /// </summary>
        BANIPUTCHI,

        /// <summary>
        /// Vanillish
        /// </summary>
        BANIRITCHI,

        /// <summary>
        /// Vanilluxe
        /// </summary>
        BAIBANIRA,

        /// <summary>
        /// Deerling
        /// </summary>
        SHIKIJIKA_SPRING,

        /// <summary>
        /// Deerling
        /// </summary>
        SHIKIJIKA_SUMMER,

        /// <summary>
        /// Deerling
        /// </summary>
        SHIKIJIKA_AUTUMN,

        /// <summary>
        /// Deerling
        /// </summary>
        SHIKIJIKA_WINTER,

        /// <summary>
        /// Sawsbuck
        /// </summary>
        MEBUKIJIKA_SPRING,

        /// <summary>
        /// Sawsbuck
        /// </summary>
        MEBUKIJIKA_SUMMER,

        /// <summary>
        /// Sawsbuck
        /// </summary>
        MEBUKIJIKA_AUTUMN,

        /// <summary>
        /// Sawsbuck
        /// </summary>
        MEBUKIJIKA_WINTER,

        /// <summary>
        /// Emolga
        /// </summary>
        EMONGA,

        /// <summary>
        /// Karrablast
        /// </summary>
        KABURUMO,

        /// <summary>
        /// Escavalier
        /// </summary>
        SHUBARUGO,

        /// <summary>
        /// Foongus
        /// </summary>
        TAMAGETAKE,

        /// <summary>
        /// Amoonguss
        /// </summary>
        MOROBARERU,

        /// <summary>
        /// Frillish
        /// </summary>
        PURURIRU,

        /// <summary>
        /// Jellicent
        /// </summary>
        BURUNGERU,

        /// <summary>
        /// Alomomola
        /// </summary>
        MAMANBOU,

        /// <summary>
        /// Joltik
        /// </summary>
        BACHURU,

        /// <summary>
        /// Galvantula
        /// </summary>
        DENCHURA,

        /// <summary>
        /// Ferroseed
        /// </summary>
        TESSHIIDO,

        /// <summary>
        /// Ferrothorn
        /// </summary>
        NATTOREI,

        /// <summary>
        /// Klink
        /// </summary>
        GIARU,

        /// <summary>
        /// Klang
        /// </summary>
        GIGIARU,

        /// <summary>
        /// Klinklang
        /// </summary>
        GIGIGIARU,

        /// <summary>
        /// Tynamo
        /// </summary>
        SHIBISHIRASU,

        /// <summary>
        /// Eelektrik
        /// </summary>
        SHIBIBIIRU,

        /// <summary>
        /// Eelektross
        /// </summary>
        SHIBIRUDON,

        /// <summary>
        /// Elgyem
        /// </summary>
        RIGUREE,

        /// <summary>
        /// Beheeyem
        /// </summary>
        OOBEMU,

        /// <summary>
        /// Litwick
        /// </summary>
        HITOMOSHI,

        /// <summary>
        /// Lampent
        /// </summary>
        RANPURAA,

        /// <summary>
        /// Chandelure
        /// </summary>
        SHANDERA,

        /// <summary>
        /// Axew
        /// </summary>
        KIBAGO,

        /// <summary>
        /// Fraxure
        /// </summary>
        ONONDO,

        /// <summary>
        /// Haxorus
        /// </summary>
        ONONOKUSU,

        /// <summary>
        /// Cubchoo
        /// </summary>
        KUMASHUN,

        /// <summary>
        /// Beartic
        /// </summary>
        TSUNBEAA,

        /// <summary>
        /// Cryogonal
        /// </summary>
        FURIIJIO,

        /// <summary>
        /// Shelmet
        /// </summary>
        CHOBOMAKI,

        /// <summary>
        /// Accelgor
        /// </summary>
        AGIRUDAA,

        /// <summary>
        /// Stunfisk
        /// </summary>
        MAGGYO,

        /// <summary>
        /// Mienfoo
        /// </summary>
        KOJOFUU,

        /// <summary>
        /// Mienshao
        /// </summary>
        KOJONDO,

        /// <summary>
        /// Druddigon
        /// </summary>
        KURIMUGAN,

        /// <summary>
        /// Golett
        /// </summary>
        GOBITTO,

        /// <summary>
        /// Golurk
        /// </summary>
        GORUUGU,

        /// <summary>
        /// Pawniard
        /// </summary>
        KOMATANA,

        /// <summary>
        /// Bisharp
        /// </summary>
        KIRIKIZAN,

        /// <summary>
        /// Bouffalant
        /// </summary>
        BAFFURON,

        /// <summary>
        /// Rufflet
        /// </summary>
        WASHIBON,

        /// <summary>
        /// Braviary
        /// </summary>
        WOOGURU,

        /// <summary>
        /// Vullaby
        /// </summary>
        BARUCHAI,

        /// <summary>
        /// Mandibuzz
        /// </summary>
        BARUJIINA,

        /// <summary>
        /// Heatmor
        /// </summary>
        KUITARAN,

        /// <summary>
        /// Durant
        /// </summary>
        AIANTO,

        /// <summary>
        /// Deino
        /// </summary>
        MONOZU,

        /// <summary>
        /// Zweilous
        /// </summary>
        JIHEDDO,

        /// <summary>
        /// Hydreigon
        /// </summary>
        SAZANDORA,

        /// <summary>
        /// Larvesta
        /// </summary>
        MERARUBA,

        /// <summary>
        /// Volcarona
        /// </summary>
        URUGAMOSU,

        /// <summary>
        /// Cobalion
        /// </summary>
        KOBARUON,

        /// <summary>
        /// Terrakion
        /// </summary>
        TERAKION,

        /// <summary>
        /// Virizion
        /// </summary>
        BIRIJION,

        /// <summary>
        /// Tornadus
        /// </summary>
        TORUNEROSU,

        /// <summary>
        /// Tornadus
        /// </summary>
        TORUNEROSU_R,

        /// <summary>
        /// Thundurus
        /// </summary>
        BORUTOROSU,

        /// <summary>
        /// Thundurus
        /// </summary>
        BORUTOROSU_R,

        /// <summary>
        /// Reshiram
        /// </summary>
        RESHIRAMU,

        /// <summary>
        /// Reshiram
        /// </summary>
        RESHIRAMU_N,

        /// <summary>
        /// Zekrom
        /// </summary>
        ZEKUROMU,

        /// <summary>
        /// Zekrom
        /// </summary>
        ZEKUROMU_N,

        /// <summary>
        /// Landorus
        /// </summary>
        RANDOROSU,

        /// <summary>
        /// Landorus
        /// </summary>
        RANDOROSU_R,

        /// <summary>
        /// Kyurem
        /// </summary>
        KYUREMU,

        /// <summary>
        /// Kyurem
        /// </summary>
        KYUREMU_BLACK,

        /// <summary>
        /// Kyurem
        /// </summary>
        KYUREMU_WHITE,

        /// <summary>
        /// Keldeo
        /// </summary>
        KERUDYIO,

        /// <summary>
        /// Keldeo
        /// </summary>
        KERUDYIO_K,

        /// <summary>
        /// Meloetta
        /// </summary>
        MEROETTA_V,

        /// <summary>
        /// Meloetta
        /// </summary>
        MEROETTA_S,

        /// <summary>
        /// Genesect
        /// </summary>
        GENOSEKUTO,

        /// <summary>
        /// Genesect
        /// </summary>
        GENOSEKUTO_AQUA,

        /// <summary>
        /// Genesect
        /// </summary>
        GENOSEKUTO_INAZUMA,

        /// <summary>
        /// Genesect
        /// </summary>
        GENOSEKUTO_BLAZE,

        /// <summary>
        /// Genesect
        /// </summary>
        GENOSEKUTO_FREEZE,

        /// <summary>
        /// Chespin
        /// </summary>
        HARIMARON,

        /// <summary>
        /// Quilladin
        /// </summary>
        HARIBOOGU,

        /// <summary>
        /// Chesnaught
        /// </summary>
        BURIGARON,

        /// <summary>
        /// Fennekin
        /// </summary>
        FOKKO,

        /// <summary>
        /// Braixen
        /// </summary>
        TEERUNAA,

        /// <summary>
        /// Delphox
        /// </summary>
        MAFOKUSHII,

        /// <summary>
        /// Froakie
        /// </summary>
        KEROMATSU,

        /// <summary>
        /// Frogadier
        /// </summary>
        GEKOGASHIRA,

        /// <summary>
        /// Greninja
        /// </summary>
        GEKKOUGA,

        /// <summary>
        /// Bunnelby
        /// </summary>
        HORUBII,

        /// <summary>
        /// Diggersby
        /// </summary>
        HORUUDO,

        /// <summary>
        /// Fletchling
        /// </summary>
        YAYAKOMA,

        /// <summary>
        /// Fletchinder
        /// </summary>
        HINOYAKOMA,

        /// <summary>
        /// Talonflame
        /// </summary>
        FAIAROO,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_HANAZONO,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_MIYABI,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_HYOUSETSU,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_YUKIGUNI,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_SETSUGEN,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_TAIRIKU,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_TEIEN,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_MODERN,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_MARIN,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_GUNTOU,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_KOUYA,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_SAJIN,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_TAIGA,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_SQUALL,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_SAVANNA,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_TAIYOU,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_OCEAN,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_JUNGLE,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_FANCY,

        /// <summary>
        /// Scatterbug
        /// </summary>
        KOFUKIMUSHI_BALL,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_HANAZONO,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_MIYABI,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_HYOUSETSU,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_YUKIGUNI,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_SETSUGEN,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_TAIRIKU,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_TEIEN,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_MODERN,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_MARIN,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_GUNTOU,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_KOUYA,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_SAJIN,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_TAIGA,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_SQUALL,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_SAVANNA,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_TAIYOU,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_OCEAN,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_JUNGLE,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_FANCY,

        /// <summary>
        /// Spewpa
        /// </summary>
        KOFUURAI_BALL,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_HANAZONO,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_MIYABI,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_HYOUSETSU,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_YUKIGUNI,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_SETSUGEN,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_TAIRIKU,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_TEIEN,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_MODERN,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_MARIN,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_GUNTOU,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_KOUYA,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_SAJIN,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_TAIGA,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_SQUALL,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_SAVANNA,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_TAIYOU,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_OCEAN,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_JUNGLE,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_FANCY,

        /// <summary>
        /// Vivillon
        /// </summary>
        BIBIYON_BALL,

        /// <summary>
        /// Litleo
        /// </summary>
        SHISHIKO,

        /// <summary>
        /// Pyroar
        /// </summary>
        KAENJISHI,

        /// <summary>
        /// Flabébé
        /// </summary>
        FURABEBE_RED,

        /// <summary>
        /// Flabébé
        /// </summary>
        FURABEBE_YELLOW,

        /// <summary>
        /// Flabébé
        /// </summary>
        FURABEBE_ORANGE,

        /// <summary>
        /// Flabébé
        /// </summary>
        FURABEBE_BLUE,

        /// <summary>
        /// Flabébé
        /// </summary>
        FURABEBE_WHITE,

        /// <summary>
        /// Flabébé
        /// </summary>
        FURABEBE_6,

        /// <summary>
        /// Floette
        /// </summary>
        FURAETTE_RED,

        /// <summary>
        /// Floette
        /// </summary>
        FURAETTE_YELLOW,

        /// <summary>
        /// Floette
        /// </summary>
        FURAETTE_ORANGE,

        /// <summary>
        /// Floette
        /// </summary>
        FURAETTE_BLUE,

        /// <summary>
        /// Floette
        /// </summary>
        FURAETTE_WHITE,

        /// <summary>
        /// Floette
        /// </summary>
        FURAETTE_6,

        /// <summary>
        /// Florges
        /// </summary>
        FURAAJESU_RED,

        /// <summary>
        /// Florges
        /// </summary>
        FURAAJESU_YELLOW,

        /// <summary>
        /// Florges
        /// </summary>
        FURAAJESU_ORANGE,

        /// <summary>
        /// Florges
        /// </summary>
        FURAAJESU_BLUE,

        /// <summary>
        /// Florges
        /// </summary>
        FURAAJESU_WHITE,

        /// <summary>
        /// Florges
        /// </summary>
        FURAAJESU_6,

        /// <summary>
        /// Skiddo
        /// </summary>
        MEEKURU,

        /// <summary>
        /// Gogoat
        /// </summary>
        GOOGOOTO,

        /// <summary>
        /// Pancham
        /// </summary>
        YANCHAMU,

        /// <summary>
        /// Pangoro
        /// </summary>
        GORONDA,

        /// <summary>
        /// Furfrou
        /// </summary>
        TORIMIAN,

        /// <summary>
        /// Furfrou
        /// </summary>
        TORIMIAN_HEART,

        /// <summary>
        /// Furfrou
        /// </summary>
        TORIMIAN_STAR,

        /// <summary>
        /// Furfrou
        /// </summary>
        TORIMIAN_DIA,

        /// <summary>
        /// Furfrou
        /// </summary>
        TORIMIAN_LADY,

        /// <summary>
        /// Furfrou
        /// </summary>
        TORIMIAN_MADAM,

        /// <summary>
        /// Furfrou
        /// </summary>
        TORIMIAN_GENTLE,

        /// <summary>
        /// Furfrou
        /// </summary>
        TORIMIAN_QUEEN,

        /// <summary>
        /// Furfrou
        /// </summary>
        TORIMIAN_KABUKI,

        /// <summary>
        /// Furfrou
        /// </summary>
        TORIMIAN_KINGDOM,

        /// <summary>
        /// Espurr
        /// </summary>
        NYASUPAA,

        /// <summary>
        /// Meowstic
        /// </summary>
        NYAONIKUSU_M,

        /// <summary>
        /// Meowstic
        /// </summary>
        NYAONIKUSU_F,

        /// <summary>
        /// Honedge
        /// </summary>
        HITOTSUKI,

        /// <summary>
        /// Doublade
        /// </summary>
        NIDANGIRU,

        /// <summary>
        /// Aegislash
        /// </summary>
        GIRUGARUDO_S,

        /// <summary>
        /// Aegislash
        /// </summary>
        GIRUGARUDO_B,

        /// <summary>
        /// Spritzee
        /// </summary>
        SHUSHUPU,

        /// <summary>
        /// Aromatisse
        /// </summary>
        FUREFUWAN,

        /// <summary>
        /// Swirlix
        /// </summary>
        PEROPPAFU,

        /// <summary>
        /// Slurpuff
        /// </summary>
        PERORIIMU,

        /// <summary>
        /// Inkay
        /// </summary>
        MAAIIKA,

        /// <summary>
        /// Malamar
        /// </summary>
        KARAMANERO,

        /// <summary>
        /// Binacle
        /// </summary>
        KAMETETE,

        /// <summary>
        /// Barbaracle
        /// </summary>
        GAMENODESU,

        /// <summary>
        /// Skrelp
        /// </summary>
        KUZUMOO,

        /// <summary>
        /// Dragalge
        /// </summary>
        DORAMIDORO,

        /// <summary>
        /// Clauncher
        /// </summary>
        UDEPPOU,

        /// <summary>
        /// Clawitzer
        /// </summary>
        BUROSUTAA,

        /// <summary>
        /// Helioptile
        /// </summary>
        ERIKITERU,

        /// <summary>
        /// Heliolisk
        /// </summary>
        EREZAADO,

        /// <summary>
        /// Tyrunt
        /// </summary>
        CHIGORASU,

        /// <summary>
        /// Tyrantrum
        /// </summary>
        GACHIGORASU,

        /// <summary>
        /// Amaura
        /// </summary>
        AMARUSU,

        /// <summary>
        /// Aurorus
        /// </summary>
        AMARURUGA,

        /// <summary>
        /// Hawlucha
        /// </summary>
        RUCHABURU,

        /// <summary>
        /// Dedenne
        /// </summary>
        DEDENNE,

        /// <summary>
        /// Carbink
        /// </summary>
        MERESHII,

        /// <summary>
        /// Goomy
        /// </summary>
        NUMERA,

        /// <summary>
        /// Sliggoo
        /// </summary>
        NUMEIRU,

        /// <summary>
        /// Goodra
        /// </summary>
        NUMERUGON,

        /// <summary>
        /// Klefki
        /// </summary>
        KUREFFI,

        /// <summary>
        /// Phantump
        /// </summary>
        BOKUREE,

        /// <summary>
        /// Trevenant
        /// </summary>
        OOROTTO,

        /// <summary>
        /// Pumpkaboo
        /// </summary>
        BAKETCHA_M,

        /// <summary>
        /// Pumpkaboo
        /// </summary>
        BAKETCHA_S,

        /// <summary>
        /// Pumpkaboo
        /// </summary>
        BAKETCHA_L,

        /// <summary>
        /// Pumpkaboo
        /// </summary>
        BAKETCHA_LL,

        /// <summary>
        /// Gourgeist
        /// </summary>
        PANPUJIN_M,

        /// <summary>
        /// Gourgeist
        /// </summary>
        PANPUJIN_S,

        /// <summary>
        /// Gourgeist
        /// </summary>
        PANPUJIN_L,

        /// <summary>
        /// Gourgeist
        /// </summary>
        PANPUJIN_LL,

        /// <summary>
        /// Bergmite
        /// </summary>
        KACHIKOORU,

        /// <summary>
        /// Avalugg
        /// </summary>
        KUREBEESU,

        /// <summary>
        /// Noibat
        /// </summary>
        ONBATTO,

        /// <summary>
        /// Noivern
        /// </summary>
        ONBAAN,

        /// <summary>
        /// Xerneas
        /// </summary>
        ZERUNEASU,

        /// <summary>
        /// Xerneas
        /// </summary>
        ZERUNEASU_R,

        /// <summary>
        /// Yveltal
        /// </summary>
        IBERUTARU,

        /// <summary>
        /// Zygarde
        /// </summary>
        JIGARUDE,

        /// <summary>
        /// Diancie
        /// </summary>
        DIANSHII,

        /// <summary>
        /// Diancie
        /// </summary>
        DIANSHII_MEGA,

        /// <summary>
        /// Hoopa
        /// </summary>
        FUUPA,

        /// <summary>
        /// Hoopa
        /// </summary>
        FUUPA_MEGA,

        /// <summary>
        /// Butterfree
        /// </summary>
        YOBI_01,

        /// <summary>
        /// Ninetales
        /// </summary>
        YOBI_02,

        /// <summary>
        /// Rapidash
        /// </summary>
        YOBI_03,

        /// <summary>
        /// Electrode
        /// </summary>
        YOBI_04,

        /// <summary>
        /// Exeggcute
        /// </summary>
        YOBI_05,

        /// <summary>
        /// Exeggutor
        /// </summary>
        YOBI_06,

        /// <summary>
        /// Lickilicky
        /// </summary>
        YOBI_07,

        /// <summary>
        /// Starmie
        /// </summary>
        YOBI_08,

        /// <summary>
        /// Magikarp
        /// </summary>
        YOBI_09,

        /// <summary>
        /// Gyarados
        /// </summary>
        YOBI_10,

        /// <summary>
        /// Gyarados
        /// </summary>
        YOBI_11,

        /// <summary>
        /// Ditto
        /// </summary>
        YOBI_12,

        /// <summary>
        /// Espeon
        /// </summary>
        YOBI_13,

        /// <summary>
        /// Dragonite
        /// </summary>
        YOBI_14,

        /// <summary>
        /// Hoothoot
        /// </summary>
        YOBI_15,

        /// <summary>
        /// Noctowl
        /// </summary>
        YOBI_16,

        /// <summary>
        /// Ampharos
        /// </summary>
        YOBI_17,

        /// <summary>
        /// Ampharos
        /// </summary>
        YOBI_18,

        /// <summary>
        /// Azumarill
        /// </summary>
        YOBI_19,

        /// <summary>
        /// Sudowoodo
        /// </summary>
        YOBI_20,

        /// <summary>
        /// Aipom
        /// </summary>
        YOBI_21,

        /// <summary>
        /// Ambipom
        /// </summary>
        YOBI_22,

        /// <summary>
        /// Wobbuffet
        /// </summary>
        YOBI_23,

        /// <summary>
        /// Shuckle
        /// </summary>
        YOBI_24,

        /// <summary>
        /// Corsola
        /// </summary>
        YOBI_25,

        /// <summary>
        /// Ho-Oh
        /// </summary>
        YOBI_26,

        /// <summary>
        /// Celebi
        /// </summary>
        YOBI_27,

        /// <summary>
        /// Spinda
        /// </summary>
        YOBI_28,

        /// <summary>
        /// Altaria
        /// </summary>
        YOBI_29,

        /// <summary>
        /// Altaria
        /// </summary>
        YOBI_30,

        /// <summary>
        /// Zangoose
        /// </summary>
        YOBI_31,

        /// <summary>
        /// Metagross
        /// </summary>
        YOBI_32,

        /// <summary>
        /// Metagross
        /// </summary>
        YOBI_33,

        YOBI_34,
        YOBI_35,
        YOBI_36,
        YOBI_37,
        YOBI_38,
        YOBI_39,
        YOBI_40,
        YOBI_41,
        YOBI_42,
        YOBI_43,
        YOBI_44,
        YOBI_45,
        YOBI_46,
        YOBI_47,
        YOBI_48,
        YOBI_49,

        /// <summary>
        /// Rayquaza
        /// </summary>
        YOBI_50,

        /// <summary>
        /// Rayquaza
        /// </summary>
        REKKUUZA_BOSS,

        /// <summary>
        /// Kecleon
        /// </summary>
        KAKUREON_AZUKI,

        /// <summary>
        /// Kangaskhan
        /// </summary>
        GARUURA_CHILD,

        /// <summary>
        /// Substitute
        /// </summary>
        MIGAWARI,

        /// <summary>
        /// Outlaw Level
        /// </summary>
        WANTED_LV,

        /// <summary>
        /// Outlaw Bottom Up HP
        /// </summary>
        WANTED_HP,

        /// <summary>
        /// Black Kyurem
        /// </summary>
        KYUREMU_BN,

        /// <summary>
        /// White Kyurem
        /// </summary>
        KYUREMU_WN,

        END
    }
}
