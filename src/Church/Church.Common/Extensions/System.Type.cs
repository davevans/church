using System;

namespace Church.Common.Extensions
{
	public static class TypeExtensions
	{
        public static bool IsNullableValueType(this Type type)
        {
            var underlying = Nullable.GetUnderlyingType(type);

            return underlying != null && underlying.IsValueType;
        }
	}
}
