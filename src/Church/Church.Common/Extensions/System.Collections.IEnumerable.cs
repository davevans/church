using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Church.Common.Extensions
{
	public static class IEnumerableExtensions
	{
        public static decimal WeightedAverage<T>(this IEnumerable<T> source, Func<T, decimal> quantity, Func<T, decimal> weight)
        {
            decimal total = 0;
            decimal totalQuantity = 0;

            foreach (var item in source)
            {
                var w = weight(item);
                var q = quantity(item);

                total += (w * q);
                totalQuantity += q;
            }

            if (totalQuantity == 0)
            {
                return 0;
            }

            return total / totalQuantity;
        }

        public static void ForEach<T>(this IEnumerable<T> enumberable, Action<T> a)
        {
            foreach (T val in enumberable)
            {
                a(val);
            }
        }

        public static ConcurrentDictionary<TKey,TValue> ToConcurrentDictionary<TKey,TValue>(this IEnumerable<TValue> source, Func<TValue, TKey> keySelected)
        {
            var cd = new ConcurrentDictionary<TKey, TValue>();

            foreach (var value in source)
            {
                var key = keySelected(value);
                cd[key] = value;
            }

            return cd;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumberable)
        {
            return new HashSet<T>(enumberable);
        }

        public static string ToCsv<T>(this IEnumerable<T> source)
        {
            return source == null ? "" : string.Join(",", source);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
          Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
           Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var knownKeys = new HashSet<TKey>(comparer);

            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childSelector)
        {
            foreach (var item in source)
            {
                yield return item;
                foreach (var child in childSelector(item).Flatten(childSelector))
                {
                    yield return child;
                }
            }
        }

        public static IEnumerable<TSource> DuplicatesBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                // Yield it if the key hasn't actually been added - i.e. it
                // was already in the set
                if (!seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

	}
}
