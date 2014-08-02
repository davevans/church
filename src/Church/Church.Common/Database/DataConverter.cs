using System;

namespace Church.Common.Database
{
    public class DataConverter
    {
        public static object GetNullDateTime(DateTime? value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value.Value;
        }

        public static object NullableToDb<T>(T value) where T:class 
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }

        public static T NullableFromDb<T>(object value, Func<string, T> converter)
        {
            if (value == DBNull.Value)
            {
                return default(T);
            }
            return converter(value.ToString());
        }
    }
}
