using System;
using System.Data;

namespace Church.Common.Extensions
{
    public static class IDataReaderExtensions
    {
        public static string GetString(this IDataReader reader, string columnName)
        {
            return (reader[columnName] as string) ?? "";
        }

        public static DateTime? GetDateTime(this IDataReader reader, string columnName)
        {
            var objectValue = reader[columnName];

            if (objectValue != DBNull.Value)
            {
                return (DateTime)objectValue;
            }

            return null;
        }

        public static long? GetNullableInt64(this IDataReader reader, string columnName)
        {
            var objectValue = reader[columnName];

            if (objectValue == DBNull.Value)
            {
                return null;
            }
            return (long)objectValue;
        }

        public static int? GetNullableInt32(this IDataReader reader, string columnName)
        {
            var objectValue = reader[columnName];

            if (objectValue == DBNull.Value)
            {
                return null;
            }
            return (int)objectValue;
        }

        public static long GetInt64(this IDataReader reader, string columnName, long defaultValue = 0)
        {
            var objectValue = reader[columnName];

            if (objectValue == DBNull.Value)
            {
                return defaultValue;
            }
            return (long)objectValue;
        }


        public static int GetInt32(this IDataReader reader, string columnName, int defaultValue = 0)
        {
            var objectValue = reader[columnName];

            if (objectValue == DBNull.Value)
            {
                return defaultValue;
            }
            return (int)objectValue;
        }
    }
}
