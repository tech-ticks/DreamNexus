using SkyEditor.RomEditor.Rtdx.Reverse.Const.pokemon;
using System;
using System.Collections.Generic;
using Creature = SkyEditor.RomEditor.Rtdx.Reverse.Const.creature.Index;

namespace SkyEditor.RomEditor.Rtdx.Reverse
{
    public static class PegasusActDatabase
    {
        public class ActorData
        {
            public enum PartyID
            {
                NONE,
                PARTY1,
                PARTY2,
                PARTY3
            }

            public string symbolName;
            public Creature raw_pokemonIndex;
            public FormType raw_formType;
            public bool bIsFemale;
            public PartyID opt_partyId;
            public PokemonWarehouseId opt_warehouseId;
            public TextId opt_specialName;
            public string debug_name;

            public string Name
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public Creature PokemonIndex
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public FormType FormType
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public PokemonWarehouseId WarehouseId
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }

        public class MapData
        {
            public string symbolName;
            public string assetBundleName;
            public string prefabName;
        }

        public class GimmickData
        {
            public string symbolName;
            public string assetBundleName;
            public string prefabName;
        }

        public class EffectData
        {
            public string symbolName;
            public string effectSymbol;
        }

        public static List<ActorData> ActorDataList = new List<ActorData>
        {
            new ActorData
            {
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
                opt_specialName = new TextId(0),
                symbolName = "", // Originally null, but changed to "" to avoid NRE's
                debug_name = "",
                bIsFemale = false,
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO",
                debug_name = "チュンのすけ",
                raw_pokemonIndex = Creature.PIKACHUU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.HERO),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PARTNER",
                debug_name = "ポケさぶろう",
                raw_pokemonIndex = Creature.FUSHIGIDANE,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.PARTNER),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PARTY1",
                debug_name = null,
                raw_pokemonIndex = Creature.RIZAADON,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_partyId = ActorData.PartyID.PARTY1,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PARTY3",
                debug_name = null,
                raw_pokemonIndex = Creature.SANDAA,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_partyId = ActorData.PartyID.PARTY3,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PARTY2",
                debug_name = null,
                raw_pokemonIndex = Creature.FAIYAA,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_partyId = ActorData.PartyID.PARTY2,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "IRAI1",
                debug_name = null,
                raw_pokemonIndex = Creature.PICHUU,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.IRAI1),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "IRAI2",
                debug_name = null,
                raw_pokemonIndex = Creature.RIZAADON,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.IRAI2),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SIZE1",
                debug_name = null,
                raw_pokemonIndex = Creature.KOMORUU,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SIZE2",
                debug_name = null,
                raw_pokemonIndex = Creature.BOOMANDA,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SIZE3",
                debug_name = null,
                raw_pokemonIndex = Creature.HOERUKO,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SIZE4",
                debug_name = null,
                raw_pokemonIndex = Creature.HOERUOO,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SIZE5",
                debug_name = null,
                raw_pokemonIndex = Creature.IWAAKU,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SIZE6",
                debug_name = null,
                raw_pokemonIndex = Creature.HAGANEERU,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "BATAFURII",
                debug_name = "バタフリー",
                raw_pokemonIndex = Creature.BATAFURII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KYATAPII",
                debug_name = "キャタピー",
                raw_pokemonIndex = Creature.KYATAPII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PERIPPAA2",
                debug_name = "ペリッパー",
                raw_pokemonIndex = Creature.PERIPPAA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PERIPPAA3",
                debug_name = "ペリッパー",
                raw_pokemonIndex = Creature.PERIPPAA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PERIPPAA4",
                debug_name = "ペリッパー",
                raw_pokemonIndex = Creature.PERIPPAA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PERIPPAA5",
                debug_name = "ペリッパー",
                raw_pokemonIndex = Creature.PERIPPAA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PERIPPAA",
                debug_name = "ペリッパー",
                raw_pokemonIndex = Creature.PERIPPAA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KOIRU",
                debug_name = "コイル",
                raw_pokemonIndex = Creature.KOIRU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.KOIRU),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KOIRU2",
                debug_name = "コイル",
                raw_pokemonIndex = Creature.KOIRU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KOIRU3",
                debug_name = "コイル",
                raw_pokemonIndex = Creature.KOIRU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KOIRU4",
                debug_name = "コイル",
                raw_pokemonIndex = Creature.KOIRU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "REAKOIRU",
                debug_name = "レアコイル",
                raw_pokemonIndex = Creature.REAKOIRU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "DAGUTORIO",
                debug_name = "ダグトリオ",
                raw_pokemonIndex = Creature.DAGUTORIO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "DIGUDA",
                debug_name = "ディグダ",
                raw_pokemonIndex = Creature.DIGUDA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "EAAMUDO",
                debug_name = "エアームド",
                raw_pokemonIndex = Creature.EAAMUDO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KAKUREON",
                debug_name = "カクレオン",
                raw_pokemonIndex = Creature.KAKUREON,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KAKUREON2",
                debug_name = "カクレオン",
                raw_pokemonIndex = Creature.KAKUREON_AZUKI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PERUSHIAN",
                debug_name = "ペルシアン",
                raw_pokemonIndex = Creature.PERUSHIAN,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PUKURIN",
                debug_name = "プクリン",
                raw_pokemonIndex = Creature.PUKURIN,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "GOKURIN",
                debug_name = "ゴクリン",
                raw_pokemonIndex = Creature.GOKURIN,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "GARUURA",
                debug_name = "ガルーラ",
                raw_pokemonIndex = Creature.GARUURA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "GOKURIN2",
                debug_name = "ゴクリン",
                raw_pokemonIndex = Creature.GOKURIN,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HASUBURERO",
                debug_name = "ハスブレロ",
                raw_pokemonIndex = Creature.HASUBURERO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "WATAKKO",
                debug_name = "ワタッコ",
                raw_pokemonIndex = Creature.WATAKKO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "MADATSUBOMI",
                debug_name = "マダツボミ",
                raw_pokemonIndex = Creature.MADATSUBOMI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "BURUU",
                debug_name = "ブルー",
                raw_pokemonIndex = Creature.BURUU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "GURANBURU",
                debug_name = "グランブル",
                raw_pokemonIndex = Creature.GURANBURU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SAANAITO",
                debug_name = "サーナイト",
                raw_pokemonIndex = Creature.SAANAITO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "ABUSORU",
                debug_name = "アブソル",
                raw_pokemonIndex = Creature.ABUSORU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "MAKUNOSHITA",
                debug_name = "マクノシタ",
                raw_pokemonIndex = Creature.MAKUNOSHITA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "DAATENGU",
                debug_name = "ダーテング",
                raw_pokemonIndex = Creature.DAATENGU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KONOHANA",
                debug_name = "コノハナ",
                raw_pokemonIndex = Creature.KONOHANA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KONOHANA2",
                debug_name = "コノハナ",
                raw_pokemonIndex = Creature.KONOHANA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "FUUDIN",
                debug_name = "フーディン",
                raw_pokemonIndex = Creature.FUUDIN,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "RIZAADON",
                debug_name = "リザードン",
                raw_pokemonIndex = Creature.RIZAADON,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "BANGIRASU",
                debug_name = "バンギラス",
                raw_pokemonIndex = Creature.BANGIRASU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "GENGAA",
                debug_name = "ゲンガー",
                raw_pokemonIndex = Creature.GENGAA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "AABO",
                debug_name = "アーボ",
                raw_pokemonIndex = Creature.AABO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "CHAAREMU",
                debug_name = "チャーレム",
                raw_pokemonIndex = Creature.CHAAREMU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "WATAKKO2",
                debug_name = "ワタッコ",
                raw_pokemonIndex = Creature.WATAKKO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SANDAA",
                debug_name = "サンダー",
                raw_pokemonIndex = Creature.SANDAA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "NEITYIO",
                debug_name = "ネイティオ",
                raw_pokemonIndex = Creature.NEITYIO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "NAMAZUN",
                debug_name = "ナマズン",
                raw_pokemonIndex = Creature.NAMAZUN,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KYUUKON",
                debug_name = "キュウコン",
                raw_pokemonIndex = Creature.KYUUKON,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "FAIYAA",
                debug_name = "ファイヤー",
                raw_pokemonIndex = Creature.FAIYAA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "GURAADON",
                debug_name = "グラードン",
                raw_pokemonIndex = Creature.GURAADON,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KAMEKKUSU",
                debug_name = "カメックス",
                raw_pokemonIndex = Creature.KAMEKKUSU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "OKUTAN",
                debug_name = "オクタン",
                raw_pokemonIndex = Creature.OKUTAN,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "GOROONYA",
                debug_name = "ゴローニャ",
                raw_pokemonIndex = Creature.GOROONYA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "REKKUUZA",
                debug_name = "レックウザ",
                raw_pokemonIndex = Creature.REKKUUZA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SOONANO",
                debug_name = "ソーナノ",
                raw_pokemonIndex = Creature.SOONANO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SOONANSU",
                debug_name = "ソーナンス",
                raw_pokemonIndex = Creature.SOONANSU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "MANKII",
                debug_name = "マンキー",
                raw_pokemonIndex = Creature.MANKII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "MANKII2",
                debug_name = "マンキー",
                raw_pokemonIndex = Creature.MANKII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "MANKII3",
                debug_name = "マンキー",
                raw_pokemonIndex = Creature.MANKII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "MANKII4",
                debug_name = "マンキー",
                raw_pokemonIndex = Creature.MANKII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "PATCHIIRU",
                debug_name = "パッチール",
                raw_pokemonIndex = Creature.PATCHIIRU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "ENTEI",
                debug_name = "エンテイ",
                raw_pokemonIndex = Creature.ENTEI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "RAIKOU",
                debug_name = "ライコウ",
                raw_pokemonIndex = Creature.RAIKOU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SUIKUN",
                debug_name = "スイクン",
                raw_pokemonIndex = Creature.SUIKUN,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HOUOU",
                debug_name = "ホウオウ",
                raw_pokemonIndex = Creature.HOUOU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "MYUUTSUU",
                debug_name = "ミュウツー",
                raw_pokemonIndex = Creature.MYUUTSUU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "RATYIOSU",
                debug_name = "ラティオス",
                raw_pokemonIndex = Creature.RATYIOSU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "RATYIASU",
                debug_name = "ラティアス",
                raw_pokemonIndex = Creature.RATYIASU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "JIRAACHI",
                debug_name = "ジラーチ",
                raw_pokemonIndex = Creature.JIRAACHI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "DOOBURU",
                debug_name = "ドーブル",
                raw_pokemonIndex = Creature.DOOBURU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "DOOBURU2",
                debug_name = "ラティオス",
                raw_pokemonIndex = Creature.DOOBURU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "DOOBURU3",
                debug_name = "ラティオス",
                raw_pokemonIndex = Creature.DOOBURU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "GONBE",
                debug_name = "ゴンベ",
                raw_pokemonIndex = Creature.GONBE,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "MYUU",
                debug_name = "ミュウ",
                raw_pokemonIndex = Creature.MYUU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "REJIROKKU",
                debug_name = "レジロック",
                raw_pokemonIndex = Creature.REJIROKKU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "REJIAISU",
                debug_name = "レジアイス",
                raw_pokemonIndex = Creature.REJIAISU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "REJISUCHIRU",
                debug_name = "レジスチル",
                raw_pokemonIndex = Creature.REJISUCHIRU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KAIOOGA",
                debug_name = "カイオーガ",
                raw_pokemonIndex = Creature.KAIOOGA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "RUGIA",
                debug_name = "ルギア",
                raw_pokemonIndex = Creature.RUGIA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "DEOKISHISU_N",
                debug_name = "デオキシス",
                raw_pokemonIndex = Creature.DEOKISHISU_N,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "RAICHUU",
                debug_name = "ライチュウ",
                raw_pokemonIndex = Creature.RAICHUU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "GORUBATTO",
                debug_name = "ゴルバット",
                raw_pokemonIndex = Creature.GORUBATTO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SAIDON",
                debug_name = "サイドン",
                raw_pokemonIndex = Creature.SAIDON,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "BARIYAADO",
                debug_name = "バリヤード",
                raw_pokemonIndex = Creature.BARIYAADO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SUTORAIKU",
                debug_name = "ストライク",
                raw_pokemonIndex = Creature.SUTORAIKU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "KAIROSU",
                debug_name = "カイロス",
                raw_pokemonIndex = Creature.KAIROSU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "MEGANIUMU",
                debug_name = "メガニウム",
                raw_pokemonIndex = Creature.MEGANIUMU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "EIPAMU",
                debug_name = "エイパム",
                raw_pokemonIndex = Creature.EIPAMU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "GOMAZOU",
                debug_name = "ゴマゾウ",
                raw_pokemonIndex = Creature.GOMAZOU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEREBII",
                debug_name = "セレビィ",
                raw_pokemonIndex = Creature.SEREBII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_HITOKAGE",
                debug_name = null,
                raw_pokemonIndex = Creature.HITOKAGE,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_FUSHIGIDANE",
                debug_name = null,
                raw_pokemonIndex = Creature.FUSHIGIDANE,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_ZENIGAME",
                debug_name = null,
                raw_pokemonIndex = Creature.ZENIGAME,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_PIKACHUU",
                debug_name = null,
                raw_pokemonIndex = Creature.PIKACHUU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_PIKACHUU_F",
                debug_name = null,
                raw_pokemonIndex = Creature.PIKACHUU,
                raw_formType = FormType.HIGH_FEMALE,
                bIsFemale = true,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_CHIKORIITA",
                debug_name = null,
                raw_pokemonIndex = Creature.CHIKORIITA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_WANINOKO",
                debug_name = null,
                raw_pokemonIndex = Creature.WANINOKO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_HINOARASHI",
                debug_name = null,
                raw_pokemonIndex = Creature.HINOARASHI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_ACHAMO",
                debug_name = null,
                raw_pokemonIndex = Creature.ACHAMO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_ACHAMO_F",
                debug_name = null,
                raw_pokemonIndex = Creature.ACHAMO,
                raw_formType = FormType.HIGH_FEMALE,
                bIsFemale = true,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_KIMORI",
                debug_name = null,
                raw_pokemonIndex = Creature.KIMORI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_MIZUGOROU",
                debug_name = null,
                raw_pokemonIndex = Creature.MIZUGOROU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_KODAKKU",
                debug_name = null,
                raw_pokemonIndex = Creature.KODAKKU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_KARAKARA",
                debug_name = null,
                raw_pokemonIndex = Creature.KARAKARA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_NYAASU",
                debug_name = null,
                raw_pokemonIndex = Creature.NYAASU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_WANRIKII",
                debug_name = null,
                raw_pokemonIndex = Creature.WANRIKII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_IIBUI",
                debug_name = null,
                raw_pokemonIndex = Creature.IIBUI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_IIBUI_F",
                debug_name = null,
                raw_pokemonIndex = Creature.IIBUI,
                raw_formType = FormType.HIGH_FEMALE,
                bIsFemale = true,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_ENEKO",
                debug_name = null,
                raw_pokemonIndex = Creature.ENEKO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "SEIKAKU_RIZAADO",
                debug_name = null,
                raw_pokemonIndex = Creature.RIZAADO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_POPPO1",
                debug_name = null,
                raw_pokemonIndex = Creature.POPPO,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_POPPO2",
                debug_name = null,
                raw_pokemonIndex = Creature.POPPO,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_POPPO3",
                debug_name = null,
                raw_pokemonIndex = Creature.POPPO,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_PIJON1",
                debug_name = null,
                raw_pokemonIndex = Creature.PIJON,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_PIJON2",
                debug_name = null,
                raw_pokemonIndex = Creature.PIJON,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_PIJON3",
                debug_name = null,
                raw_pokemonIndex = Creature.PIJON,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_ONISUZUME1",
                debug_name = null,
                raw_pokemonIndex = Creature.ONISUZUME,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_ONISUZUME2",
                debug_name = null,
                raw_pokemonIndex = Creature.ONISUZUME,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_ONISUZUME3",
                debug_name = null,
                raw_pokemonIndex = Creature.ONISUZUME,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_SUBAME1",
                debug_name = null,
                raw_pokemonIndex = Creature.SUBAME,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_SUBAME2",
                debug_name = null,
                raw_pokemonIndex = Creature.SUBAME,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_SUBAME3",
                debug_name = null,
                raw_pokemonIndex = Creature.SUBAME,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_KYAMOME1",
                debug_name = null,
                raw_pokemonIndex = Creature.KYAMOME,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_KYAMOME2",
                debug_name = null,
                raw_pokemonIndex = Creature.KYAMOME,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "LO_KYAMOME3",
                debug_name = null,
                raw_pokemonIndex = Creature.KYAMOME,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "DBG_PARTY_1",
                debug_name = null,
                raw_pokemonIndex = Creature.RIZAADO,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "DBG_PARTY_2",
                debug_name = null,
                raw_pokemonIndex = Creature.RIZAADON,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "DBG_PARTY_3",
                debug_name = null,
                raw_pokemonIndex = Creature.RIZAADON_MEGA_X,
                raw_formType = FormType.NORMAL,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_HITOKAGE",
                debug_name = null,
                raw_pokemonIndex = Creature.HITOKAGE,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_FUSHIGIDANE",
                debug_name = null,
                raw_pokemonIndex = Creature.FUSHIGIDANE,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_ZENIGAME",
                debug_name = null,
                raw_pokemonIndex = Creature.ZENIGAME,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_PIKACHUU",
                debug_name = null,
                raw_pokemonIndex = Creature.PIKACHUU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_CHIKORIITA",
                debug_name = null,
                raw_pokemonIndex = Creature.CHIKORIITA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_WANINOKO",
                debug_name = null,
                raw_pokemonIndex = Creature.WANINOKO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_HINOARASHI",
                debug_name = null,
                raw_pokemonIndex = Creature.HINOARASHI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_ACHAMO",
                debug_name = null,
                raw_pokemonIndex = Creature.ACHAMO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_KIMORI",
                debug_name = null,
                raw_pokemonIndex = Creature.KIMORI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_MIZUGOROU",
                debug_name = null,
                raw_pokemonIndex = Creature.MIZUGOROU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_KODAKKU",
                debug_name = null,
                raw_pokemonIndex = Creature.KODAKKU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_KARAKARA",
                debug_name = null,
                raw_pokemonIndex = Creature.KARAKARA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_NYAASU",
                debug_name = null,
                raw_pokemonIndex = Creature.NYAASU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_WANRIKII",
                debug_name = null,
                raw_pokemonIndex = Creature.WANRIKII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_IIBUI",
                debug_name = null,
                raw_pokemonIndex = Creature.IIBUI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_ENEKO",
                debug_name = null,
                raw_pokemonIndex = Creature.ENEKO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_RIZAADO",
                debug_name = null,
                raw_pokemonIndex = Creature.RIZAADO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_FUSHIGISOU",
                debug_name = null,
                raw_pokemonIndex = Creature.FUSHIGISOU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_KAMEERU",
                debug_name = null,
                raw_pokemonIndex = Creature.KAMEERU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_RAICHUU",
                debug_name = null,
                raw_pokemonIndex = Creature.RAICHUU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_BEIRIIFU",
                debug_name = null,
                raw_pokemonIndex = Creature.BEIRIIFU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_ARIGEITSU",
                debug_name = null,
                raw_pokemonIndex = Creature.ARIGEITSU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_MAGUMARASHI",
                debug_name = null,
                raw_pokemonIndex = Creature.MAGUMARASHI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_WAKASHAMO",
                debug_name = null,
                raw_pokemonIndex = Creature.WAKASHAMO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_JUPUTORU",
                debug_name = null,
                raw_pokemonIndex = Creature.JUPUTORU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_NUMAKUROO",
                debug_name = null,
                raw_pokemonIndex = Creature.NUMAKUROO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_GORUDAKKU",
                debug_name = null,
                raw_pokemonIndex = Creature.GORUDAKKU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_GARAGARA",
                debug_name = null,
                raw_pokemonIndex = Creature.GARAGARA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_PERUSHIAN",
                debug_name = null,
                raw_pokemonIndex = Creature.PERUSHIAN,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_GOORIKII",
                debug_name = null,
                raw_pokemonIndex = Creature.GOORIKII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_SHAWAAZU",
                debug_name = null,
                raw_pokemonIndex = Creature.SHAWAAZU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_SANDAASU",
                debug_name = null,
                raw_pokemonIndex = Creature.SANDAASU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_BUUSUTAA",
                debug_name = null,
                raw_pokemonIndex = Creature.BUUSUTAA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_EEFI",
                debug_name = null,
                raw_pokemonIndex = Creature.EEFI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_BURAKKII",
                debug_name = null,
                raw_pokemonIndex = Creature.BURAKKII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_RIIFIA",
                debug_name = null,
                raw_pokemonIndex = Creature.RIIFIA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_GUREISHIA",
                debug_name = null,
                raw_pokemonIndex = Creature.GUREISHIA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_NINFIA",
                debug_name = null,
                raw_pokemonIndex = Creature.NINFIA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_ENEKORORO",
                debug_name = null,
                raw_pokemonIndex = Creature.ENEKORORO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_RIZAADON",
                debug_name = null,
                raw_pokemonIndex = Creature.RIZAADON,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_FUSHIGIBANA",
                debug_name = null,
                raw_pokemonIndex = Creature.FUSHIGIBANA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_KAMEKKUSU",
                debug_name = null,
                raw_pokemonIndex = Creature.KAMEKKUSU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_MEGANIUMU",
                debug_name = null,
                raw_pokemonIndex = Creature.MEGANIUMU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_OODAIRU",
                debug_name = null,
                raw_pokemonIndex = Creature.OODAIRU,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_BAKUFUUN",
                debug_name = null,
                raw_pokemonIndex = Creature.BAKUFUUN,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_BASHAAMO",
                debug_name = null,
                raw_pokemonIndex = Creature.BASHAAMO,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_JUKAIN",
                debug_name = null,
                raw_pokemonIndex = Creature.JUKAIN,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_RAGURAAJI",
                debug_name = null,
                raw_pokemonIndex = Creature.RAGURAAJI,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_KAIRIKII",
                debug_name = null,
                raw_pokemonIndex = Creature.KAIRIKII,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_FUSHIGIBANA_MEGA",
                debug_name = null,
                raw_pokemonIndex = Creature.FUSHIGIBANA_MEGA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_RIZAADON_MEGA_X",
                debug_name = null,
                raw_pokemonIndex = Creature.RIZAADON_MEGA_X,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_RIZAADON_MEGA_Y",
                debug_name = null,
                raw_pokemonIndex = Creature.RIZAADON_MEGA_Y,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_KAMEKKUSU_MEGA",
                debug_name = null,
                raw_pokemonIndex = Creature.KAMEKKUSU_MEGA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
            new ActorData
            {
                opt_specialName = new TextId(0),
                symbolName = "HERO_TEST_RAGURAAJI_MEGA",
                debug_name = null,
                raw_pokemonIndex = Creature.RAGURAAJI_MEGA,
                raw_formType = FormType.HIGH,
                bIsFemale = false,
                opt_warehouseId = new PokemonWarehouseId(FixedWarehouseId.NULL),
            },
        };

        public static ActorData FindActorData(string symbol)
        {
            return ActorDataList.Find(d => d.symbolName == symbol);
        }

        public static void LoadCharaObject(Creature index, FormType formType, Action<CharacterModel> loadedCb)
        {
            throw new NotImplementedException();
        }

        public static MapData FindMapData(string symbol)
        {
            throw new NotImplementedException();
        }

        public static GimmickData FindGimmick(string symbol)
        {
            throw new NotImplementedException();
        }
        public static EffectData FindEffect(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
