using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace SkyEditor.RomEditor.Infrastructure
{
    public static class DictionaryExtensions
    {
#if NETSTANDARD2_0
#nullable disable
        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }

        public static int GetValueOrDefault<TKey>(this IReadOnlyDictionary<TKey, int> dictionary, TKey key)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            return default;
        }
#nullable restore
#endif
    }
}
