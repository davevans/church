using System;
using System.Collections.Generic;
using System.Data;
using Church.Common.Extensions;
using Microsoft.SqlServer.Server;


namespace Church.Common.Database
{
    public static class TableTypes
    {
        public static IEnumerable<SqlDataRecord> GetList<T>(IEnumerable<T> source, SqlMetaData meta, Action<SqlDataRecord,T> set)
        {
            var definition = new[] { meta };

            var result = new List<SqlDataRecord>();

            if (source != null)
            {
                source.ForEach(a =>
                {
                    var record = new SqlDataRecord(definition);
                    set(record, a);
                    result.Add(record);
                });
            }
            return result;

        }

        public static IEnumerable<SqlDataRecord> GetLargeIntegerListWithRowOrder(IEnumerable<long> source)
        {
            //var definition = new[] { new SqlMetaData("Id", SqlDbType.Int), new SqlMetaData("RowId", SqlDbType.BigInt), };
            //return GetList(source, SqlDbType.BigInt, "Id", (r, a) => r.SetInt64(0, a));
            var definition = new[] {
                new SqlMetaData("Id", SqlDbType.BigInt),
                new SqlMetaData("RowId", SqlDbType.BigInt),
            };

            var result = new List<SqlDataRecord>();

            if (source != null)
            {
                var count = 1;

                source.ForEach(a =>
                {
                    var record = new SqlDataRecord(definition);
                    record.SetInt64(0, a);
                    record.SetInt64(1, count++);
                    result.Add(record);
                });
            }
            return result;

        }

        public static IEnumerable<SqlDataRecord> GetNameValuePairList(IEnumerable<KeyValuePair<string, string>> source)
        {
            var definition = new[] {
                new SqlMetaData("Name", SqlDbType.VarChar, 50),
                new SqlMetaData("Value", SqlDbType.VarChar, SqlMetaData.Max),
            };

            var result = new List<SqlDataRecord>();
            if (source != null)
            {
                source.ForEach(kvp =>
                               {
                                   var record = new SqlDataRecord(definition);
                                   record.SetSqlString(0, kvp.Key);
                                   record.SetSqlString(1, kvp.Value);
                                   result.Add(record);
                               });
            }
            return result;
        }


        public static IEnumerable<SqlDataRecord> GetLargeIntegerList(IEnumerable<long> source)
        {
            return GetList(source, new SqlMetaData("Id", SqlDbType.BigInt), (r, a) => r.SetInt64(0, a));
        }

        public static IEnumerable<SqlDataRecord> GetSmallIntegerList(IEnumerable<short> source)
        {
            return GetList(source, new SqlMetaData("Id", SqlDbType.SmallInt), (r, a) => r.SetInt16(0, a));
        }

        public static IEnumerable<SqlDataRecord> GetIntegerList(IEnumerable<int> source)
        {
            return GetList(source, new SqlMetaData("Id", SqlDbType.Int), (r, a) => r.SetInt32(0, a));
        }

        public static IEnumerable<SqlDataRecord> GetUniqueIdentifierList(IEnumerable<Guid> source)
        {
            return GetList(source, new SqlMetaData("Id", SqlDbType.UniqueIdentifier), (r, a) => r.SetGuid(0, a));
        }

        public static IEnumerable<SqlDataRecord> GetUniqueIdentifierList(IEnumerable<string> source)
        {
            return GetList(source,  new SqlMetaData("Id", SqlDbType.UniqueIdentifier), (r, a) => r.SetGuid(0, Guid.Parse(a)));
        }

        public static IEnumerable<SqlDataRecord> GetStringList(IEnumerable<string> source)
        {
            return GetList(source, new SqlMetaData("Value", SqlDbType.NVarChar, -1), (r, a) => r.SetSqlString(0, a));
        }

        public static IEnumerable<SqlDataRecord> GetNameValuePair(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            return GetNameValuePairList(keyValuePairs);
        }
        
    }
}
