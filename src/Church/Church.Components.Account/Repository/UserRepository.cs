using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Church.Common.Extensions;
using Church.Common.Logging;
using Church.Common.Settings;
using Church.Common.Structures;
using Church.Common.Xml;
using Church.Components.Account.Model;
using Church.Types.Core;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Church.Components.Account.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger _logger;
        private readonly SqlDatabase _sqlDatabase;

        public UserRepository(ISettingsProvider settingsProvider, ILogger logger)
        {
            _logger = logger;
            var settings = settingsProvider.GetSetting<AccountSettings>();
            _sqlDatabase = new SqlDatabase(settings.Database);
        }

        public async Task<Result<IEnumerable<User>>> GetAllActiveUsersAsync()
        {
            try
            {
                IEnumerable<User> users;
                var command = _sqlDatabase.GetStoredProcCommand("Account.UserGetAllActive");
                using (var reader = await _sqlDatabase.ExecuteReaderAsync(command))
                {
                    users = UserFromReader(reader);
                }
                return Result<IEnumerable<User>>.Success(users);
            }
            catch (SqlException sex)
            {
                var error = ChurchErrors.UnknownError;
                _logger.Exception(GetType(), "Failed to get all active users:", sex);
                return Result<IEnumerable<User>>.Failure(error);
            }
        }

        public async Task<Result<User>> AddUserAsync(User userToAdd)
        {
            try
            {
                var command = _sqlDatabase.GetStoredProcCommand("Account.UserInsert");
                _sqlDatabase.AddInParameter(command, "PersonId", DbType.Int64, userToAdd.PersonId);
                _sqlDatabase.AddInParameter(command, "HashedPassword", DbType.String, userToAdd.HashedPassword);
                _sqlDatabase.AddInParameter(command, "Salt", DbType.String, userToAdd.Salt);
                _sqlDatabase.AddInParameter(command, "IsActive", DbType.Boolean, userToAdd.IsActive);
                _sqlDatabase.AddOutParameter(command, "Id", DbType.Int64, 8);

                await _sqlDatabase.ExecuteNonQueryAsync(command);

                var id = (long)_sqlDatabase.GetParameterValue(command, "Id");
                userToAdd.Id = id;

                return Result<User>.Success(userToAdd);
            }
            catch (SqlException sex)
            {
                Error error;
                if (sex.Number == Common.Database.SqlErrorCodes.ConstraintViolation)
                {
                    error = AccountErrors.DuplicatePersonId;
                    _logger.Exception(GetType(), "Failed to INSERT user: {0}. User with personId {1} already exists.".FormatWith(userToAdd.ToXmlString(XmlOptions.Lean), userToAdd.PersonId), sex);
                }
                else
                {
                    error = ChurchErrors.UnknownError;
                    _logger.Exception(GetType(), "Failed to INSERT User: {0}.".FormatWith(userToAdd.ToXmlString(XmlOptions.Lean)), sex);
                }

                return Result<User>.Failure(error);
            }
            
        }

        private IEnumerable<User> UserFromReader(IDataReader reader)
        {
            var users = new List<User>();
            while (reader.Read())
            {
                var user = new User
                {
                    Id = long.Parse(reader["Id"].ToString()),
                    PersonId = long.Parse(reader["PersonId"].ToString()),
                    IsActive = bool.Parse(reader["IsActive"].ToString()),
                    Created = DateTime.Parse(reader["Created"].ToString()),
                    HashedPassword = reader["HashedPassword"].ToString(),
                    Salt = reader["Salt"].ToString()
                };

                users.Add(user);
            }
            return users;
        }
    }
}
