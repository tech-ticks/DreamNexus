using System.Collections.Generic;
using SkyEditor.RomEditor.Rtdx.Constants;

namespace SkyEditor.RomEditor.Rtdx.Domain.Structures.Executable
{
    public partial class PegasusActDatabase
    {
        public List<ActorData> ActorDataList { get; } = new List<ActorData>
        {
            new ActorData {
                SymbolName = "HERO",
                DebugName = "チュンのすけ",
                PokemonIndex = CreatureIndex.PIKACHUU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.HERO,
                PokemonIndexOffset = 0x0,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "PARTNER",
                DebugName = "ポケさぶろう",
                PokemonIndex = CreatureIndex.FUSHIGIDANE,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.PARTNER,
                PokemonIndexOffset = 0xCC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "PARTY1",
                DebugName = null,
                PokemonIndex = CreatureIndex.RIZAADON,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                PartyId = PegasusActorDataPartyId.PARTY1,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x190,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "PARTY3",
                DebugName = null,
                PokemonIndex = CreatureIndex.SANDAA,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                PartyId = PegasusActorDataPartyId.PARTY3,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x238,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "PARTY2",
                DebugName = null,
                PokemonIndex = CreatureIndex.FAIYAA,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                PartyId = PegasusActorDataPartyId.PARTY2,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2DC,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "IRAI1",
                DebugName = null,
                PokemonIndex = CreatureIndex.PICHUU,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.IRAI1,
                PokemonIndexOffset = 0x380,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "IRAI2",
                DebugName = null,
                PokemonIndex = CreatureIndex.RIZAADON,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.IRAI2,
                PokemonIndexOffset = 0x438,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "SIZE1",
                DebugName = null,
                PokemonIndex = CreatureIndex.KOMORUU,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4F0,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "SIZE2",
                DebugName = null,
                PokemonIndex = CreatureIndex.BOOMANDA,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x58C,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "SIZE3",
                DebugName = null,
                PokemonIndex = CreatureIndex.HOERUKO,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x628,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "SIZE4",
                DebugName = null,
                PokemonIndex = CreatureIndex.HOERUOO,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6C4,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData
            {
                SymbolName = "SIZE5",
                DebugName = null,
                PokemonIndex = CreatureIndex.IWAAKU,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x760,
                PokemonIndexEditable = false
            },
            new ActorData
            {
                SymbolName = "SIZE6",
                DebugName = null,
                PokemonIndex = CreatureIndex.HAGANEERU,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7FC,
                PokemonIndexEditable = false
            },
            new ActorData {
                SymbolName = "BATAFURII",
                DebugName = "バタフリー",
                PokemonIndex = CreatureIndex.BATAFURII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x8A8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "KYATAPII",
                DebugName = "キャタピー",
                PokemonIndex = CreatureIndex.KYATAPII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x958,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "PERIPPAA",
                DebugName = "ペリッパー",
                PokemonIndex = CreatureIndex.PERIPPAA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0xA04,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "PERIPPAA2",
                DebugName = "ペリッパー",
                PokemonIndex = CreatureIndex.PERIPPAA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "PERIPPAA3",
                DebugName = "ペリッパー",
                PokemonIndex = CreatureIndex.PERIPPAA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "PERIPPAA4",
                DebugName = "ペリッパー",
                PokemonIndex = CreatureIndex.PERIPPAA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "PERIPPAA5",
                DebugName = "ペリッパー",
                PokemonIndex = CreatureIndex.PERIPPAA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "KOIRU",
                DebugName = "コイル",
                PokemonIndex = CreatureIndex.KOIRU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.KOIRU,
                PokemonIndexOffset = 0xD4C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "KOIRU2",
                DebugName = "コイル",
                PokemonIndex = CreatureIndex.KOIRU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "KOIRU3",
                DebugName = "コイル",
                PokemonIndex = CreatureIndex.KOIRU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "KOIRU4",
                DebugName = "コイル",
                PokemonIndex = CreatureIndex.KOIRU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "REAKOIRU",
                DebugName = "レアコイル",
                PokemonIndex = CreatureIndex.REAKOIRU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1010,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "DAGUTORIO",
                DebugName = "ダグトリオ",
                PokemonIndex = CreatureIndex.DAGUTORIO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x10C0,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "DIGUDA",
                DebugName = "ディグダ",
                PokemonIndex = CreatureIndex.DIGUDA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1170,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "EAAMUDO",
                DebugName = "エアームド",
                PokemonIndex = CreatureIndex.EAAMUDO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1220,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "KAKUREON",
                DebugName = "カクレオン",
                PokemonIndex = CreatureIndex.KAKUREON,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x12D0,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "KAKUREON2",
                DebugName = "カクレオン",
                PokemonIndex = CreatureIndex.KAKUREON_AZUKI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1380,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "PERUSHIAN",
                DebugName = "ペルシアン",
                PokemonIndex = CreatureIndex.PERUSHIAN,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1438,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "PUKURIN",
                DebugName = "プクリン",
                PokemonIndex = CreatureIndex.PUKURIN,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x14E8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "GOKURIN",
                DebugName = "ゴクリン",
                PokemonIndex = CreatureIndex.GOKURIN,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1594,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "GARUURA",
                DebugName = "ガルーラ",
                PokemonIndex = CreatureIndex.GARUURA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1650,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "GOKURIN2",
                DebugName = "ゴクリン",
                PokemonIndex = CreatureIndex.GOKURIN,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "HASUBURERO",
                DebugName = "ハスブレロ",
                PokemonIndex = CreatureIndex.HASUBURERO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x17AC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "WATAKKO",
                DebugName = "ワタッコ",
                PokemonIndex = CreatureIndex.WATAKKO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x185C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "MADATSUBOMI",
                DebugName = "マダツボミ",
                PokemonIndex = CreatureIndex.MADATSUBOMI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x190C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "BURUU",
                DebugName = "ブルー",
                PokemonIndex = CreatureIndex.BURUU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x19BC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "GURANBURU",
                DebugName = "グランブル",
                PokemonIndex = CreatureIndex.GURANBURU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1A6C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SAANAITO",
                DebugName = "サーナイト",
                PokemonIndex = CreatureIndex.SAANAITO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1B1C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "ABUSORU",
                DebugName = "アブソル",
                PokemonIndex = CreatureIndex.ABUSORU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1BCC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "MAKUNOSHITA",
                DebugName = "マクノシタ",
                PokemonIndex = CreatureIndex.MAKUNOSHITA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1C98,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "DAATENGU",
                DebugName = "ダーテング",
                PokemonIndex = CreatureIndex.DAATENGU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1D48,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "KONOHANA",
                DebugName = "コノハナ",
                PokemonIndex = CreatureIndex.KONOHANA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1DF4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "KONOHANA2",
                DebugName = "コノハナ",
                PokemonIndex = CreatureIndex.KONOHANA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "FUUDIN",
                DebugName = "フーディン",
                PokemonIndex = CreatureIndex.FUUDIN,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x1F54,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "RIZAADON",
                DebugName = "リザードン",
                PokemonIndex = CreatureIndex.RIZAADON,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2004,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "BANGIRASU",
                DebugName = "バンギラス",
                PokemonIndex = CreatureIndex.BANGIRASU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x20B4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "GENGAA",
                DebugName = "ゲンガー",
                PokemonIndex = CreatureIndex.GENGAA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2164,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "AABO",
                DebugName = "アーボ",
                PokemonIndex = CreatureIndex.AABO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2214,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "CHAAREMU",
                DebugName = "チャーレム",
                PokemonIndex = CreatureIndex.CHAAREMU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x22C4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "TORANSERU",
                DebugName = null,
                PokemonIndex = CreatureIndex.TORANSERU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2374,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "WATAKKO2",
                DebugName = "ワタッコ",
                PokemonIndex = CreatureIndex.WATAKKO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2424,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SANDAA",
                DebugName = "サンダー",
                PokemonIndex = CreatureIndex.SANDAA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x24D4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "NEITYIO",
                DebugName = "ネイティオ",
                PokemonIndex = CreatureIndex.NEITYIO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x25A0,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "NAMAZUN",
                DebugName = "ナマズン",
                PokemonIndex = CreatureIndex.NAMAZUN,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2650,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "KYUUKON",
                DebugName = "キュウコン",
                PokemonIndex = CreatureIndex.KYUUKON,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2700,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "FAIYAA",
                DebugName = "ファイヤー",
                PokemonIndex = CreatureIndex.FAIYAA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x27B0,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "FURIIZAA",
                DebugName = "ファイヤー",
                PokemonIndex = CreatureIndex.FURIIZAA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x287C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "GURAADON",
                DebugName = "グラードン",
                PokemonIndex = CreatureIndex.GURAADON,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2948,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "KAMEKKUSU",
                DebugName = "カメックス",
                PokemonIndex = CreatureIndex.KAMEKKUSU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x29F8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "OKUTAN",
                DebugName = "オクタン",
                PokemonIndex = CreatureIndex.OKUTAN,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2AA8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "GOROONYA",
                DebugName = "ゴローニャ",
                PokemonIndex = CreatureIndex.GOROONYA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2B58,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "REKKUUZA",
                DebugName = "レックウザ",
                PokemonIndex = CreatureIndex.REKKUUZA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2C08,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SOONANO",
                DebugName = "ソーナノ",
                PokemonIndex = CreatureIndex.SOONANO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2CB8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SOONANSU",
                DebugName = "ソーナンス",
                PokemonIndex = CreatureIndex.SOONANSU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2D68,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "MANKII",
                DebugName = "マンキー",
                PokemonIndex = CreatureIndex.MANKII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x2E14,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "MANKII2",
                DebugName = "マンキー",
                PokemonIndex = CreatureIndex.MANKII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "MANKII3",
                DebugName = "マンキー",
                PokemonIndex = CreatureIndex.MANKII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "MANKII4",
                DebugName = "マンキー",
                PokemonIndex = CreatureIndex.MANKII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "PATCHIIRU",
                DebugName = "パッチール",
                PokemonIndex = CreatureIndex.PATCHIIRU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x30BC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "ENTEI",
                DebugName = "エンテイ",
                PokemonIndex = CreatureIndex.ENTEI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x316C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "RAIKOU",
                DebugName = "ライコウ",
                PokemonIndex = CreatureIndex.RAIKOU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x321C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SUIKUN",
                DebugName = "スイクン",
                PokemonIndex = CreatureIndex.SUIKUN,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x32CC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HOUOU",
                DebugName = "ホウオウ",
                PokemonIndex = CreatureIndex.HOUOU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x337C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "MYUUTSUU",
                DebugName = "ミュウツー",
                PokemonIndex = CreatureIndex.MYUUTSUU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x342C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "RATYIOSU",
                DebugName = "ラティオス",
                PokemonIndex = CreatureIndex.RATYIOSU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x34DC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "RATYIASU",
                DebugName = "ラティアス",
                PokemonIndex = CreatureIndex.RATYIASU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x358C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "JIRAACHI",
                DebugName = "ジラーチ",
                PokemonIndex = CreatureIndex.JIRAACHI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x363C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "DOOBURU",
                DebugName = "ドーブル",
                PokemonIndex = CreatureIndex.DOOBURU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x3704,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "DOOBURU2",
                DebugName = "ラティオス",
                PokemonIndex = CreatureIndex.DOOBURU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "DOOBURU3",
                DebugName = "ラティオス",
                PokemonIndex = CreatureIndex.DOOBURU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "GONBE",
                DebugName = "ゴンベ",
                PokemonIndex = CreatureIndex.GONBE,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x3924,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "MYUU",
                DebugName = "ミュウ",
                PokemonIndex = CreatureIndex.MYUU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x39D4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "REJIROKKU",
                DebugName = "レジロック",
                PokemonIndex = CreatureIndex.REJIROKKU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x3A84,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "REJIAISU",
                DebugName = "レジアイス",
                PokemonIndex = CreatureIndex.REJIAISU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x3B34,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "REJISUCHIRU",
                DebugName = "レジスチル",
                PokemonIndex = CreatureIndex.REJISUCHIRU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x3BE4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "KAIOOGA",
                DebugName = "カイオーガ",
                PokemonIndex = CreatureIndex.KAIOOGA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x3C94,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "RUGIA",
                DebugName = "ルギア",
                PokemonIndex = CreatureIndex.RUGIA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x3D44,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "DEOKISHISU_N",
                DebugName = "デオキシス",
                PokemonIndex = CreatureIndex.DEOKISHISU_N,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x3DF4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "RAICHUU",
                DebugName = "ライチュウ",
                PokemonIndex = CreatureIndex.RAICHUU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x3EA4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "GORUBATTO",
                DebugName = "ゴルバット",
                PokemonIndex = CreatureIndex.GORUBATTO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x3F54,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SAIDON",
                DebugName = "サイドン",
                PokemonIndex = CreatureIndex.SAIDON,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4004,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "BARIYAADO",
                DebugName = "バリヤード",
                PokemonIndex = CreatureIndex.BARIYAADO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x40B4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SUTORAIKU",
                DebugName = "ストライク",
                PokemonIndex = CreatureIndex.SUTORAIKU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4164,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "KAIROSU",
                DebugName = "カイロス",
                PokemonIndex = CreatureIndex.KAIROSU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4214,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "MEGANIUMU",
                DebugName = "メガニウム",
                PokemonIndex = CreatureIndex.MEGANIUMU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x42C4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "EIPAMU",
                DebugName = "エイパム",
                PokemonIndex = CreatureIndex.EIPAMU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4374,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "GOMAZOU",
                DebugName = "ゴマゾウ",
                PokemonIndex = CreatureIndex.GOMAZOU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4424,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEREBII",
                DebugName = "セレビィ",
                PokemonIndex = CreatureIndex.SEREBII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x44D4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_HITOKAGE",
                DebugName = null,
                PokemonIndex = CreatureIndex.HITOKAGE,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4570,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_FUSHIGIDANE",
                DebugName = null,
                PokemonIndex = CreatureIndex.FUSHIGIDANE,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4610,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_ZENIGAME",
                DebugName = null,
                PokemonIndex = CreatureIndex.ZENIGAME,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x46AC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_PIKACHUU",
                DebugName = null,
                PokemonIndex = CreatureIndex.PIKACHUU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x474C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_PIKACHUU_F",
                DebugName = null,
                PokemonIndex = CreatureIndex.PIKACHUU,
                FormType = PokemonFormType.HIGH_FEMALE,
                IsFemale = true,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x47E8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_CHIKORIITA",
                DebugName = null,
                PokemonIndex = CreatureIndex.CHIKORIITA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4884,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_WANINOKO",
                DebugName = null,
                PokemonIndex = CreatureIndex.WANINOKO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4920,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_HINOARASHI",
                DebugName = null,
                PokemonIndex = CreatureIndex.HINOARASHI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x49BC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_ACHAMO",
                DebugName = null,
                PokemonIndex = CreatureIndex.ACHAMO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4A58,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_ACHAMO_F",
                DebugName = null,
                PokemonIndex = CreatureIndex.ACHAMO,
                FormType = PokemonFormType.HIGH_FEMALE,
                IsFemale = true,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4AF4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_KIMORI",
                DebugName = null,
                PokemonIndex = CreatureIndex.KIMORI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4B90,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_MIZUGOROU",
                DebugName = null,
                PokemonIndex = CreatureIndex.MIZUGOROU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4C2C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_KODAKKU",
                DebugName = null,
                PokemonIndex = CreatureIndex.KODAKKU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4CC8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_KARAKARA",
                DebugName = null,
                PokemonIndex = CreatureIndex.KARAKARA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4D64,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_NYAASU",
                DebugName = null,
                PokemonIndex = CreatureIndex.NYAASU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4E00,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_WANRIKII",
                DebugName = null,
                PokemonIndex = CreatureIndex.WANRIKII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4E9C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_IIBUI",
                DebugName = null,
                PokemonIndex = CreatureIndex.IIBUI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4F38,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_IIBUI_F",
                DebugName = null,
                PokemonIndex = CreatureIndex.IIBUI,
                FormType = PokemonFormType.HIGH_FEMALE,
                IsFemale = true,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x4FD4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_ENEKO",
                DebugName = null,
                PokemonIndex = CreatureIndex.ENEKO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5070,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "SEIKAKU_RIZAADO",
                DebugName = null,
                PokemonIndex = CreatureIndex.RIZAADO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x510C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "LO_POPPO1",
                DebugName = null,
                PokemonIndex = CreatureIndex.POPPO,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x51A8,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_POPPO2",
                DebugName = null,
                PokemonIndex = CreatureIndex.POPPO,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5240,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_POPPO3",
                DebugName = null,
                PokemonIndex = CreatureIndex.POPPO,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x52D8,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_PIJON1",
                DebugName = null,
                PokemonIndex = CreatureIndex.PIJON,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5370,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_PIJON2",
                DebugName = null,
                PokemonIndex = CreatureIndex.PIJON,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5408,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_PIJON3",
                DebugName = null,
                PokemonIndex = CreatureIndex.PIJON,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x54A0,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_ONISUZUME1",
                DebugName = null,
                PokemonIndex = CreatureIndex.ONISUZUME,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5538,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_ONISUZUME2",
                DebugName = null,
                PokemonIndex = CreatureIndex.ONISUZUME,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x55D0,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_ONISUZUME3",
                DebugName = null,
                PokemonIndex = CreatureIndex.ONISUZUME,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5668,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_SUBAME1",
                DebugName = null,
                PokemonIndex = CreatureIndex.SUBAME,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5700,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_SUBAME2",
                DebugName = null,
                PokemonIndex = CreatureIndex.SUBAME,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5798,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_SUBAME3",
                DebugName = null,
                PokemonIndex = CreatureIndex.SUBAME,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5830,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_KYAMOME1",
                DebugName = null,
                PokemonIndex = CreatureIndex.KYAMOME,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x58C8,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_KYAMOME2",
                DebugName = null,
                PokemonIndex = CreatureIndex.KYAMOME,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5960,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "LO_KYAMOME3",
                DebugName = null,
                PokemonIndex = CreatureIndex.KYAMOME,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x59F8,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "DBG_PARTY_1",
                DebugName = null,
                PokemonIndex = CreatureIndex.RIZAADO,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5A9C,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "DBG_PARTY_2",
                DebugName = null,
                PokemonIndex = CreatureIndex.RIZAADON,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5B48,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "DBG_PARTY_3",
                DebugName = null,
                PokemonIndex = CreatureIndex.RIZAADON_MEGA_X,
                FormType = PokemonFormType.NORMAL,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5BF4,
                PokemonIndexEditable = false  // Can't edit due to unsupported instruction 
            },
            new ActorData {
                SymbolName = "HERO_TEST_HITOKAGE",
                DebugName = null,
                PokemonIndex = CreatureIndex.HITOKAGE,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5C98,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_FUSHIGIDANE",
                DebugName = null,
                PokemonIndex = CreatureIndex.FUSHIGIDANE,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5D38,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_ZENIGAME",
                DebugName = null,
                PokemonIndex = CreatureIndex.ZENIGAME,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5DD8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_PIKACHUU",
                DebugName = null,
                PokemonIndex = CreatureIndex.PIKACHUU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5E78,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_CHIKORIITA",
                DebugName = null,
                PokemonIndex = CreatureIndex.CHIKORIITA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5F18,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_WANINOKO",
                DebugName = null,
                PokemonIndex = CreatureIndex.WANINOKO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x5FB8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_HINOARASHI",
                DebugName = null,
                PokemonIndex = CreatureIndex.HINOARASHI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6058,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_ACHAMO",
                DebugName = null,
                PokemonIndex = CreatureIndex.ACHAMO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x60F8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_KIMORI",
                DebugName = null,
                PokemonIndex = CreatureIndex.KIMORI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6198,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_MIZUGOROU",
                DebugName = null,
                PokemonIndex = CreatureIndex.MIZUGOROU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6238,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_KODAKKU",
                DebugName = null,
                PokemonIndex = CreatureIndex.KODAKKU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x62D8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_KARAKARA",
                DebugName = null,
                PokemonIndex = CreatureIndex.KARAKARA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6378,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_NYAASU",
                DebugName = null,
                PokemonIndex = CreatureIndex.NYAASU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6418,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_WANRIKII",
                DebugName = null,
                PokemonIndex = CreatureIndex.WANRIKII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x64B8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_IIBUI",
                DebugName = null,
                PokemonIndex = CreatureIndex.IIBUI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6558,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_ENEKO",
                DebugName = null,
                PokemonIndex = CreatureIndex.ENEKO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x65F8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_RIZAADO",
                DebugName = null,
                PokemonIndex = CreatureIndex.RIZAADO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexEditable = false // Can't edit due to copied value 
            },
            new ActorData {
                SymbolName = "HERO_TEST_FUSHIGISOU",
                DebugName = null,
                PokemonIndex = CreatureIndex.FUSHIGISOU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6728,
                
                // TODO: this one doesn't work for some reason although it should
                // pokemonIndexEditable = true  
            },
            new ActorData {
                SymbolName = "HERO_TEST_KAMEERU",
                DebugName = null,
                PokemonIndex = CreatureIndex.KAMEERU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x67C0,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_RAICHUU",
                DebugName = null,
                PokemonIndex = CreatureIndex.RAICHUU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6860,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_BEIRIIFU",
                DebugName = null,
                PokemonIndex = CreatureIndex.BEIRIIFU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x68FC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_ARIGEITSU",
                DebugName = null,
                PokemonIndex = CreatureIndex.ARIGEITSU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6998,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_MAGUMARASHI",
                DebugName = null,
                PokemonIndex = CreatureIndex.MAGUMARASHI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6A34,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_WAKASHAMO",
                DebugName = null,
                PokemonIndex = CreatureIndex.WAKASHAMO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6AD0,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_JUPUTORU",
                DebugName = null,
                PokemonIndex = CreatureIndex.JUPUTORU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6B6C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_NUMAKUROO",
                DebugName = null,
                PokemonIndex = CreatureIndex.NUMAKUROO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6C08,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_GORUDAKKU",
                DebugName = null,
                PokemonIndex = CreatureIndex.GORUDAKKU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6CA4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_GARAGARA",
                DebugName = null,
                PokemonIndex = CreatureIndex.GARAGARA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6D40,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_PERUSHIAN",
                DebugName = null,
                PokemonIndex = CreatureIndex.PERUSHIAN,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6DE0,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_GOORIKII",
                DebugName = null,
                PokemonIndex = CreatureIndex.GOORIKII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6E7C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_SHAWAAZU",
                DebugName = null,
                PokemonIndex = CreatureIndex.SHAWAAZU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6F18,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_SANDAASU",
                DebugName = null,
                PokemonIndex = CreatureIndex.SANDAASU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x6FB4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_BUUSUTAA",
                DebugName = null,
                PokemonIndex = CreatureIndex.BUUSUTAA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7050,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_EEFI",
                DebugName = null,
                PokemonIndex = CreatureIndex.EEFI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x70EC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_BURAKKII",
                DebugName = null,
                PokemonIndex = CreatureIndex.BURAKKII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7188,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_RIIFIA",
                DebugName = null,
                PokemonIndex = CreatureIndex.RIIFIA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7224,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_GUREISHIA",
                DebugName = null,
                PokemonIndex = CreatureIndex.GUREISHIA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x72C0,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_NINFIA",
                DebugName = null,
                PokemonIndex = CreatureIndex.NINFIA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x735C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_ENEKORORO",
                DebugName = null,
                PokemonIndex = CreatureIndex.ENEKORORO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x73F8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_RIZAADON",
                DebugName = null,
                PokemonIndex = CreatureIndex.RIZAADON,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7498,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_FUSHIGIBANA",
                DebugName = null,
                PokemonIndex = CreatureIndex.FUSHIGIBANA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7534,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_KAMEKKUSU",
                DebugName = null,
                PokemonIndex = CreatureIndex.KAMEKKUSU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x75D4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_MEGANIUMU",
                DebugName = null,
                PokemonIndex = CreatureIndex.MEGANIUMU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7674,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_OODAIRU",
                DebugName = null,
                PokemonIndex = CreatureIndex.OODAIRU,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7710,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_BAKUFUUN",
                DebugName = null,
                PokemonIndex = CreatureIndex.BAKUFUUN,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x77AC,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_BASHAAMO",
                DebugName = null,
                PokemonIndex = CreatureIndex.BASHAAMO,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7848,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_JUKAIN",
                DebugName = null,
                PokemonIndex = CreatureIndex.JUKAIN,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x78E4,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_RAGURAAJI",
                DebugName = null,
                PokemonIndex = CreatureIndex.RAGURAAJI,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7980,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_KAIRIKII",
                DebugName = null,
                PokemonIndex = CreatureIndex.KAIRIKII,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7A1C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_FUSHIGIBANA_MEGA",
                DebugName = null,
                PokemonIndex = CreatureIndex.FUSHIGIBANA_MEGA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7AB8,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_RIZAADON_MEGA_X",
                DebugName = null,
                PokemonIndex = CreatureIndex.RIZAADON_MEGA_X,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7B54,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_RIZAADON_MEGA_Y",
                DebugName = null,
                PokemonIndex = CreatureIndex.RIZAADON_MEGA_Y,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7BF0,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_KAMEKKUSU_MEGA",
                DebugName = null,
                PokemonIndex = CreatureIndex.KAMEKKUSU_MEGA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7C8C,
                PokemonIndexEditable = true
            },
            new ActorData {
                SymbolName = "HERO_TEST_RAGURAAJI_MEGA",
                DebugName = null,
                PokemonIndex = CreatureIndex.RAGURAAJI_MEGA,
                FormType = PokemonFormType.HIGH,
                IsFemale = false,
                WarehouseId = PokemonFixedWarehouseId.NULL,
                PokemonIndexOffset = 0x7D28,
                PokemonIndexEditable = true
            },
        };
    }
}