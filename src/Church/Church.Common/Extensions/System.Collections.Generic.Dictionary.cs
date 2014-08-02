using System;
using System.Collections.Generic;

namespace Church.Common.Extensions
{
	public static class DictionaryExtensions
	{
        public static void ForEach<K, V>(this Dictionary<K, V> dictionary, Action<K, V> action)
        {
            if (dictionary == null || action == null)
            {
                return;
            }

            foreach (var kv in dictionary)
            {
                action(kv.Key, kv.Value);
            }
        }

        public static V Get<K, V>(this Dictionary<K, V> dic, K key, V def = default(V))
        {
            V output;
            return dic.TryGetValue(key, out output) ? output : def;
        }

        public static V GetAdd<K, V>(this Dictionary<K,V> dic, K key, Func<V> add)
        {
            V output;
            if (dic.TryGetValue(key, out output))
            {
                return output;
            }

            return dic[key] = add();
        }
	}
}
