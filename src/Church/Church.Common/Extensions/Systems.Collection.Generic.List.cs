using System;
using System.Collections.Generic;

namespace Church.Common.Extensions
{
	public static class ListExtensions
	{
        public static List<T> ForEach<T>(this List<T> list, Action<T> action)
        {
            if (list == null || action == null || list.Count == 0)
            {
                return list;
            }

            foreach (var item in list)
            {
                action(item);
            }

            return list;
        }
	}
}
