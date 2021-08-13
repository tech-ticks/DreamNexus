using System;

namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
    public enum LanguageType
    {
        JP = 0,
        EN = 1,
        FR = 2,
        GE = 3,
        IT = 4,
        SP = 5,
        MAX = 6
    }

    public static class LanguageTypeExtensions
    {
        public static string GetFriendlyName(this LanguageType language)
        {
            switch (language)
            {
                case LanguageType.JP:
                    return "Japanese";
                case LanguageType.EN:
                    return "English";
                case LanguageType.FR:
                    return "French";
                case LanguageType.GE:
                    return "German";
                case LanguageType.IT:
                    return "Italian";
                case LanguageType.SP:
                    return "Spanish";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language));
            }
        }
    }
}
