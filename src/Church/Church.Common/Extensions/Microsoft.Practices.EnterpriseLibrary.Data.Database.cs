using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Church.Common.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Church.Common.Extensions
{
	public static class DatabaseExtensions
	{
        private const int DEADLOCK_SQL_ERROR_NUMBER = 1205;

        public static void ExecuteNonQueryWithRetry(
            this SqlDatabase database, 
            ILogger logger,
            DbCommand dbCommand, 
            int retryCount = 3, 
            int retryDelay = 100
            )
        {
            var deadLockCount = 0;

            while (true)
            {
                try
                {
                    database.ExecuteNonQuery(dbCommand);
                    return;
                }
              catch (SqlException sqlException)
                {
                    if (sqlException.Number == DEADLOCK_SQL_ERROR_NUMBER)
                    {
                        deadLockCount++;

                        if (deadLockCount >= retryCount)
                        {
                            logger.Exception(typeof(DatabaseExtensions), "Deadlock occurred. Retry limit reached. Command: " + dbCommand.CommandText, sqlException);
                            throw;
                        }

                        logger.Exception(typeof(DatabaseExtensions), "Deadlock occurred. Will retry. Command: " + dbCommand.CommandText, sqlException);
                        Thread.Sleep(retryDelay);

                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

	    public static Task<IDataReader> ExecuteReaderAsync(this SqlDatabase db, DbCommand command)
	    {
	        return Task<IDataReader>.Factory.FromAsync(db.BeginExecuteReader, db.EndExecuteReader, command, null);
	    }

	    public static Task<int> ExecuteNonQueryAsync(this SqlDatabase db, DbCommand command)
	    {
	        return Task<int>.Factory.FromAsync(db.BeginExecuteNonQuery, db.EndExecuteNonQuery, command, null);
	    }
	}

   
}
