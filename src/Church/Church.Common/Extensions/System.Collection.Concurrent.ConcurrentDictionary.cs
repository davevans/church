using System.Collections.Concurrent;

namespace Church.Common.Extensions
{
    public static class ConcurrentDictionaryExtensions
    {
        public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey,TValue> cd, TKey key)
        {
            TValue temp;
            return cd.TryRemove(key, out temp);
        }
    }
}
