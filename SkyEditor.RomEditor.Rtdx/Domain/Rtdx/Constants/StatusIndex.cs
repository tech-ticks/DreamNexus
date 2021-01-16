using SkyEditor.RomEditor.Infrastructure.Automation.CSharp;
using SkyEditor.RomEditor.Infrastructure.Automation.Lua;
using SkyEditor.RomEditor.Infrastructure;
using System;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public class StatusIndexLuaExpressionGenerator : ILuaExpressionGenerator
    {
        public StatusIndexLuaExpressionGenerator(ICommonStrings? commonStrings = null)
        {
            this.commonStrings = commonStrings;
        }

        private readonly ICommonStrings? commonStrings;

        public string Generate(object? obj)
        {
            if (!(obj is StatusIndex index))
            {
                throw new ArgumentException("Unsupported value type");
            }

            string? friendlyName = commonStrings?.Statuses?.GetValueOrDefault(index);
            if (!string.IsNullOrEmpty(friendlyName))
            {
                return $"Const.status.Index.{obj:f} --[[{friendlyName}]]";
            }
            else
            {
                return $"Const.status.Index.{obj:f}";
            }
        }
    }
    [LuaExpressionGenerator(typeof(StatusIndexLuaExpressionGenerator))]
    [CSharpExpressionGenerator(typeof(StatusIndexCSharpExpressionGenerator))]
    public enum StatusIndex
    {
        NONE = 0,

        /// <summary>
        /// Nightmare
        /// </summary>
        AKUMU = 1,

        /// <summary>
        /// Sleepless
        /// </summary>
        NEMURAZU = 2,

        /// <summary>
        /// Sleep
        /// </summary>
        SUIMIN = 3,

        /// <summary>
        /// Yawning
        /// </summary>
        AKUBI = 4,

        /// <summary>
        /// Napping
        /// </summary>
        NEMURU = 5,

        /// <summary>
        ///
        /// </summary>
        KAMIN = 6,

        /// <summary>
        /// Burn
        /// </summary>
        YAKEDO = 7,

        /// <summary>
        /// Poisoned
        /// </summary>
        DOKU = 8,

        /// <summary>
        /// Badly Poisoned
        /// </summary>
        MOUDOKU = 9,

        /// <summary>
        /// Paralysis
        /// </summary>
        MAHI = 10,

        /// <summary>
        /// Frozen
        /// </summary>
        KOORI = 11,

        /// <summary>
        /// Stuck
        /// </summary>
        ARUKEZU = 12,

        /// <summary>
        /// Ingrain
        /// </summary>
        NEWOHARI = 13,

        /// <summary>
        /// Petrified
        /// </summary>
        KOUCHOKU = 14,

        /// <summary>
        /// Bound
        /// </summary>
        SHIMETSUKE = 15,

        /// <summary>
        /// Fire Spin
        /// </summary>
        HONOONOUZU = 16,

        /// <summary>
        /// Sand Tomb
        /// </summary>
        SUNAJIGOKU = 17,

        /// <summary>
        /// Clamp
        /// </summary>
        KARADEHASAMU = 18,

        /// <summary>
        /// Whirlpool
        /// </summary>
        UZUSHIO = 19,

        /// <summary>
        /// Magma Storm
        /// </summary>
        MAGMASTORM = 20,

        /// <summary>
        /// Infestation
        /// </summary>
        MATOWARITSUKU = 21,

        /// <summary>
        /// Flinch
        /// </summary>
        HIRUMI = 22,

        /// <summary>
        /// Truant
        /// </summary>
        NAMAKE = 23,

        /// <summary>
        /// Confused
        /// </summary>
        KONRAN = 24,

        /// <summary>
        /// Recoil
        /// </summary>
        YOUSUMI = 25,

        /// <summary>
        /// Lethargic
        /// </summary>
        STOP = 26,

        /// <summary>
        /// Infatuated
        /// </summary>
        MEROMERO = 27,

        /// <summary>
        /// Taunted
        /// </summary>
        CHOUHATSU = 28,

        /// <summary>
        /// Encore
        /// </summary>
        ENCORE = 29,

        /// <summary>
        /// Discord
        /// </summary>
        GIKUSHAKU = 30,

        /// <summary>
        /// Bide
        /// </summary>
        GAMAN = 31,

        /// <summary>
        /// Solar Beam
        /// </summary>
        SOLARBEAM = 32,

        /// <summary>
        /// Focus Punch
        /// </summary>
        KIAIPUNCH = 33,

        /// <summary>
        /// Flying
        /// </summary>
        SORAWOTOBU = 34,

        /// <summary>
        /// Bouncing
        /// </summary>
        TOBIHANE = 35,

        /// <summary>
        /// Digging
        /// </summary>
        ANAWOHORU = 36,

        /// <summary>
        /// Future Sight
        /// </summary>
        MIRAIYOCHI = 37,

        /// <summary>
        /// Sky Drop
        /// </summary>
        FREEFALL = 38,

        /// <summary>
        /// Suspended
        /// </summary>
        TSURESARARE = 39,

        /// <summary>
        /// Charging
        /// </summary>
        JUUDEN = 40,

        /// <summary>
        /// Enraged
        /// </summary>
        IKARI = 41,

        /// <summary>
        /// Round
        /// </summary>
        RINSHOU = 42,

        /// <summary>
        /// Razor Wind
        /// </summary>
        KAMAITACHI = 43,

        /// <summary>
        /// Sky Attack
        /// </summary>
        GODBIRD = 44,

        /// <summary>
        /// Skull Bash
        /// </summary>
        ROCKETZUTSUKI = 45,

        /// <summary>
        /// Doom Desire
        /// </summary>
        HAMETSUNONEGAI = 46,

        /// <summary>
        /// Dive
        /// </summary>
        DIVING = 47,

        /// <summary>
        /// Shadow Force
        /// </summary>
        SHADOWDIVE = 48,

        /// <summary>
        /// Phantom Force
        /// </summary>
        GHOSTDIVE = 49,

        /// <summary>
        /// Ice Burn
        /// </summary>
        COLDFLARE = 50,

        /// <summary>
        /// Freeze Shock
        /// </summary>
        FREEZEVOLT = 51,

        /// <summary>
        /// Geomancy
        /// </summary>
        GEOCONTROL = 52,

        /// <summary>
        /// Reflect
        /// </summary>
        REFLECTOR = 53,

        /// <summary>
        /// Safeguard
        /// </summary>
        SHINPINOMAMORI = 54,

        /// <summary>
        /// Light Screen
        /// </summary>
        HIKARINOKABE = 55,

        /// <summary>
        /// Counter
        /// </summary>
        COUNTER = 56,

        /// <summary>
        /// Protect
        /// </summary>
        MAMORI = 57,

        /// <summary>
        /// Quick Guard
        /// </summary>
        FASTGUARD = 58,

        /// <summary>
        /// Wide Guard
        /// </summary>
        WIDEGUARD = 59,

        /// <summary>
        /// Mat Block
        /// </summary>
        TATAMIGAESHI = 60,

        /// <summary>
        /// Crafty Shield
        /// </summary>
        TRICKGUARD = 61,

        /// <summary>
        /// King's Shield
        /// </summary>
        KINGSHIELD = 62,

        /// <summary>
        /// Spiky Shield
        /// </summary>
        NEEDLEGUARD = 63,

        /// <summary>
        /// Mirror Coat
        /// </summary>
        MIRRORCOAT = 64,

        /// <summary>
        /// Enduring
        /// </summary>
        KORAERU = 65,

        /// <summary>
        /// Mist
        /// </summary>
        SHIROIKIRI = 66,

        /// <summary>
        /// Healthy
        /// </summary>
        KENKOU = 67,

        /// <summary>
        /// Aqua Ring
        /// </summary>
        AQUARING = 68,

        /// <summary>
        /// Lucky Chant
        /// </summary>
        OMAJINAI = 69,

        /// <summary>
        /// Metal Burst
        /// </summary>
        METALBURST = 70,

        /// <summary>
        /// Magic Coat
        /// </summary>
        MAGICCOAT = 71,

        /// <summary>
        /// Snatch
        /// </summary>
        YOKODORI = 72,

        /// <summary>
        /// Cursed
        /// </summary>
        NOROI = 73,

        /// <summary>
        /// Substitute
        /// </summary>
        MIGAWARI = 74,

        /// <summary>
        /// Decoy
        /// </summary>
        CHUUMOKU = 75,

        /// <summary>
        /// Gastro Acid
        /// </summary>
        IEKI = 76,

        /// <summary>
        /// Heal Block
        /// </summary>
        KAIFUKUFUUJI = 77,

        /// <summary>
        /// Embargo
        /// </summary>
        SASHIOSAE = 78,

        /// <summary>
        /// Leech Seed
        /// </summary>
        YADORIGI = 79,

        /// <summary>
        /// Destiny Bond
        /// </summary>
        MICHIZURE = 80,

        /// <summary>
        /// Sure Shot
        /// </summary>
        HITCHUU = 81,

        /// <summary>
        /// Focus Energy
        /// </summary>
        KIAIDAME = 82,

        /// <summary>
        /// Encouraged
        /// </summary>
        HAGEMI = 83,

        /// <summary>
        /// Pierce
        /// </summary>
        KANTSUU = 84,

        /// <summary>
        /// Blinker
        /// </summary>
        METSUBUSHI = 85,

        /// <summary>
        /// Eyedrops
        /// </summary>
        MEGUSURI = 86,

        /// <summary>
        /// Mobile
        /// </summary>
        TSUUKA = 87,

        /// <summary>
        /// Trained
        /// </summary>
        WAZAMIGAKI = 88,

        /// <summary>
        /// Stockpiling
        /// </summary>
        TAKUWAE = 89,

        /// <summary>
        /// Radar
        /// </summary>
        JIGOKUMIMI = 90,

        /// <summary>
        /// Scanning
        /// </summary>
        SENRIGAN = 91,

        /// <summary>
        /// Grudge
        /// </summary>
        ONNEN = 92,

        /// <summary>
        /// Exposed
        /// </summary>
        MIYABURI = 93,

        /// <summary>
        /// Miracle Eye
        /// </summary>
        MIRACLEEYE = 94,

        /// <summary>
        /// Terrified
        /// </summary>
        OBIE = 95,

        /// <summary>
        /// Grounded
        /// </summary>
        TSUIRAKU = 96,

        /// <summary>
        /// Magnet Rise
        /// </summary>
        DENJIFUYUU = 97,

        /// <summary>
        /// Telekinesis
        /// </summary>
        TELEKINESIS = 98,

        /// <summary>
        /// Autotomize
        /// </summary>
        KARUI = 99,

        /// <summary>
        /// Sealed
        /// </summary>
        WAZAFUIN = 100,

        /// <summary>
        /// Perish Song
        /// </summary>
        HOROBINOUTA = 101,

        /// <summary>
        /// Wish
        /// </summary>
        NEGAIGOTO = 102,

        /// <summary>
        /// Transform
        /// </summary>
        HENSHIN = 103,

        /// <summary>
        /// Electrify
        /// </summary>
        SOUDEN = 104,

        /// <summary>
        /// Powder
        /// </summary>
        FUNJIN = 105,

        /// <summary>
        /// Puppet
        /// </summary>
        AYATSURARE = 106,

        /// <summary>
        /// Awakened
        /// </summary>
        MEGA = 107,

        /// <summary>
        /// Berserk
        /// </summary>
        BOUSOU = 108,

        /// <summary>
        ///
        /// </summary>
        TSUEWASURE = 109,

        /// <summary>
        /// Self-Destruct
        /// </summary>
        BAKUDAN = 110,

        /// <summary>
        /// Explosion
        /// </summary>
        DAIBAKUDAN = 111,

        /// <summary>
        /// Low HP
        /// </summary>
        HP_PINCH = 112,

        /// <summary>
        /// Hungry
        /// </summary>
        KUUFUKU = 113,

        /// <summary>
        /// Boosted
        /// </summary>
        KYOUKA = 114,

        /// <summary>
        /// Weakened
        /// </summary>
        JAKKA = 115,

        /// <summary>
        /// Quick
        /// </summary>
        KOUSOKU = 116,

        /// <summary>
        /// Slow
        /// </summary>
        DONSOKU = 117,

        /// <summary>
        /// Stair Seeker
        /// </summary>
        KAIDAN = 118,

        /// <summary>
        /// Power Charge
        /// </summary>
        XL_WAZA = 119,

        /// <summary>
        /// Flying
        /// </summary>
        XL_SORAWOTOBU = 120,

        /// <summary>
        ///
        /// </summary>
        JUKUSUI = 121,

        /// <summary>
        /// Ancient Crown
        /// </summary>
        AKASHI_INISHIE = 122,

        /// <summary>
        /// Food Fighter Crown
        /// </summary>
        AKASHI_HERAZU = 123,

        /// <summary>
        /// Treasure Crown
        /// </summary>
        AKASHI_SEKAI = 124,

        /// <summary>
        /// Golden Crown
        /// </summary>
        AKASHI_OUGON = 125,

        /// <summary>
        ///
        /// </summary>
        QUEST_SWEAT = 126,

        /// <summary>
        ///
        /// </summary>
        QUEST_SPREE = 127,

        /// <summary>
        /// Fainted
        /// </summary>
        QUEST_HP0 = 128,

        /// <summary>
        ///
        /// </summary>
        BOSS_MENEKI = 129,

        /// <summary>
        ///
        /// </summary>
        NIGEASHI_OBIE = 130,

        /// <summary>
        /// Completion Crown
        /// </summary>
        AKASHI_KONPU = 131,

        /// <summary>
        ///
        /// </summary>
        SLOW0 = 132,

        /// <summary>
        ///
        /// </summary>
        SLOW1 = 133,

        /// <summary>
        ///
        /// </summary>
        SLOW2 = 134,

        /// <summary>
        ///
        /// </summary>
        SLOW3 = 135,

        /// <summary>
        ///
        /// </summary>
        SLOW4 = 136,

        /// <summary>
        ///
        /// </summary>
        SLOW5 = 137,

        /// <summary>
        ///
        /// </summary>
        SLOW6 = 138,

        /// <summary>
        ///
        /// </summary>
        SLOW7 = 139,

        /// <summary>
        ///
        /// </summary>
        FAST0 = 140,

        /// <summary>
        ///
        /// </summary>
        FAST1 = 141,

        /// <summary>
        ///
        /// </summary>
        FAST2 = 142,

        /// <summary>
        ///
        /// </summary>
        FAST3 = 143,

        /// <summary>
        ///
        /// </summary>
        FAST4 = 144,

        /// <summary>
        ///
        /// </summary>
        FAST5 = 145,

        /// <summary>
        ///
        /// </summary>
        FAST6 = 146,

        /// <summary>
        ///
        /// </summary>
        FAST7 = 147,

        /// <summary>
        ///
        /// </summary>
        DONSOKU_CHECK = 148,

        /// <summary>
        ///
        /// </summary>
        JAM = 149,

        /// <summary>
        ///
        /// </summary>
        PLAYER_1 = 150,

        /// <summary>
        ///
        /// </summary>
        PLAYER_2 = 151,

        /// <summary>
        ///
        /// </summary>
        PLAYER_3 = 152,

        /// <summary>
        ///
        /// </summary>
        PLAYER_4 = 153,

        /// <summary>
        ///
        /// </summary>
        REST = 154,

        /// <summary>
        ///
        /// </summary>
        MAHI_SLOW = 155,

        /// <summary>
        ///
        /// </summary>
        QUEST_UPSET = 156,

        /// <summary>
        ///
        /// </summary>
        QUEST_LAUGH = 157,

        /// <summary>
        ///
        /// </summary>
        QUEST_SPEAK = 158,

        /// <summary>
        ///
        /// </summary>
        QUEST_ANGRY = 159,

        /// <summary>
        /// Rainbow
        /// </summary>
        NIJI = 160,

        /// <summary>
        /// Swamp
        /// </summary>
        SHITSUGEN = 161,

        /// <summary>
        /// Sea of Fire
        /// </summary>
        HINOUMI = 162,

        /// <summary>
        /// Dark Matter Charging
        /// </summary>
        DARKMATTER_F1P1 = 163,

        /// <summary>
        /// Dark Matter Charging
        /// </summary>
        DARKMATTER_F1P2 = 164,

        /// <summary>
        /// Dark Matter Charging
        /// </summary>
        DARKMATTER_F2P1 = 165,

        /// <summary>
        /// Dark Matter Charging
        /// </summary>
        DARKMATTER_F2P2 = 166,

        /// <summary>
        /// Dark Matter Charging
        /// </summary>
        DARKMATTER_F2P3 = 167,

        /// <summary>
        ///
        /// </summary>
        HELPER = 168,

        /// <summary>
        ///
        /// </summary>
        QUEST_FALL = 169,

        /// <summary>
        ///
        /// </summary>
        QUEST_MUTEKI = 170,

        /// <summary>
        ///
        /// </summary>
        KOGUNFUNTOU = 171,

        /// <summary>
        ///
        /// </summary>
        SEMAITOKORO = 172,

        /// <summary>
        ///
        /// </summary>
        SHIBORIDASU = 173,

        /// <summary>
        ///
        /// </summary>
        SUGOWAZA_UP = 174,

        /// <summary>
        /// Inviting
        /// </summary>
        INVITATION = 175,

        /// <summary>
        ///
        /// </summary>
        INUNDATION = 176,

        /// <summary>
        ///
        /// </summary>
        KYOUTEKI = 177,

        /// <summary>
        ///
        /// </summary>
        MAKUNOSHITA = 178,

        /// <summary>
        /// Aurora Veil
        /// </summary>
        AURORAVEIL = 179,

        /// <summary>
        /// Throat Chop
        /// </summary>
        JIGOKUZUKI = 180,

        /// <summary>
        /// Laser Focus
        /// </summary>
        TOGISUMASU = 181,

        END = 182
    }
}
