using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Church.Common.Extensions
{
	public static class SqlDatabaseExtensions
	{
        public static T LoadObject<T>(this SqlDatabase database, DbCommand command, Func<IDataReader, T> load)
        {
            using (var reader = database.ExecuteReader(command))
            {
                if(reader.Read())
                {
                  return load(reader);
                }
            }

            return default(T);
        }

	    public static List<T> LoadObjects<T>(this SqlDatabase database, DbCommand command, Func<IDataReader, T> load)
        {
            var results = new List<T>();

            using (var reader = database.ExecuteReader(command))
            {
                while(reader.Read())
                {
                    results.Add(load(reader));
                }
            }

            return results;
        }

        public static T LoadObjects<T>(this SqlDatabase database, DbCommand command, Action<IDataReader, T> load) where T : new()
        {
            var results = new T();

            using (var reader = database.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    load(reader, results);
                }
            }

            return results;
        }
	}
}
