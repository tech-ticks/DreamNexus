using SkyEditor.RomEditor.Infrastructure.Automation.CSharp;
using SkyEditor.RomEditor.Infrastructure.Automation.Lua;
using SkyEditor.RomEditor.Infrastructure;
using System;
using System.Collections.Generic;

namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public class DungeonStatusIndexLuaExpressionGenerator : ILuaExpressionGenerator
    {
        public DungeonStatusIndexLuaExpressionGenerator(ICommonStrings? commonStrings = null)
        {
            this.commonStrings = commonStrings;
        }

        private readonly ICommonStrings? commonStrings;

        public string Generate(object? obj)
        {
            if (!(obj is DungeonStatusIndex index))
            {
                throw new ArgumentException("Unsupported value type");
            }

            string? friendlyName = commonStrings?.DungeonStatuses?.GetValueOrDefault(index);
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
    [LuaExpressionGenerator(typeof(DungeonStatusIndexLuaExpressionGenerator))]
    [CSharpExpressionGenerator(typeof(DungeonStatusIndexCSharpExpressionGenerator))]
    public enum DungeonStatusIndex
    {
        NONE = 0,

        /// <summary>
        /// Clear
        /// </summary>
        HARE = 1,

        /// <summary>
        /// Sunny
        /// </summary>
        HIZASHIGATSUYOI = 2,

        /// <summary>
        /// Rain
        /// </summary>
        AME = 3,

        /// <summary>
        /// Hail
        /// </summary>
        ARARE = 4,

        /// <summary>
        /// Sandstorm
        /// </summary>
        SUNAARASHI = 5,

        /// <summary>
        /// Mud Sport
        /// </summary>
        DOROASOBI = 6,

        /// <summary>
        /// Water Sport
        /// </summary>
        MIZUASOBI = 7,

        /// <summary>
        /// Defused
        /// </summary>
        FUHATSU = 8,

        /// <summary>
        /// Thief Alert
        /// </summary>
        DOROBOU = 9,

        /// <summary>
        /// Trick Room
        /// </summary>
        TRICKROOM = 10,

        /// <summary>
        /// Magic Room
        /// </summary>
        MAGICROOM = 11,

        /// <summary>
        /// Monster House
        /// </summary>
        HOUSE = 12,

        /// <summary>
        /// Shop
        /// </summary>
        SHOP = 13,

        /// <summary>
        /// Nullified
        /// </summary>
        TOKUSEIFUUJI = 14,

        /// <summary>
        /// Luminous
        /// </summary>
        HIKARINOTAMA = 15,

        /// <summary>
        /// 
        /// </summary>
        RDM = 16, // Also rain? (used in Sky Tower)

        /// <summary>
        /// Heavy Rain
        /// </summary>
        OOAME = 17,

        /// <summary>
        /// Extremely Harsh Sunlight
        /// </summary>
        OOHIDERI = 18,

        /// <summary>
        /// Strong Winds
        /// </summary>
        RANKIRYUU = 19,

        /// <summary>
        /// Ineffective Weather
        /// </summary>
        TENKIMUKOU = 20,

        /// <summary>
        /// Gravity
        /// </summary>
        JUURYOKU = 21,

        /// <summary>
        /// Wonder Room
        /// </summary>
        WONDERROOM = 22,

        /// <summary>
        /// Ion Deluge
        /// </summary>
        PLASMASHOWER = 23,

        /// <summary>
        /// Grassy Terrain
        /// </summary>
        GRASSFIELD = 24,

        /// <summary>
        /// Misty Terrain
        /// </summary>
        MISTFIELD = 25,

        /// <summary>
        /// Electric Terrain
        /// </summary>
        EREKIFIELD = 26,

        /// <summary>
        /// Happy Hour
        /// </summary>
        HAPPYTIME = 27,

        /// <summary>
        /// Apathetic
        /// </summary>
        TEKIMUYOKU = 28,

        /// <summary>
        /// Enemy Discord
        /// </summary>
        RENKEIFUUJI = 29,

        /// <summary>
        /// Disabled
        /// </summary>
        WAZAKINSHI = 30,

        /// <summary>
        /// Weather Lock
        /// </summary>
        TENKIKOTEI = 31,

        /// <summary>
        /// Rainbow Light
        /// </summary>
        NIJIIRO = 32,

        /// <summary>
        /// Flooded
        /// </summary>
        INUNDATION = 33,

        /// <summary>
        /// Rare Quality Radar
        /// </summary>
        SUGOWAZA_UP = 34,

        /// <summary>
        /// Radar
        /// </summary>
        JIGOKUMIMI = 35,

        /// <summary>
        /// Scanning
        /// </summary>
        SENRIGAN = 36,

        END = 37
    }
}
