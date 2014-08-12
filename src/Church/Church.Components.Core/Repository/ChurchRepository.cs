﻿using System;
using System.Data;
using System.Data.SqlClient;
using Church.Common.Extensions;
using Church.Common.Logging;
using Church.Common.Settings;
using Church.Common.Structures;
using Church.Common.Xml;
using Church.Components.Core.Model;
using Church.Types.Core;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Church.Components.Core.Repository
{
    public class ChurchRepository : IChurchRepository
    {
        private readonly ILogger _logger;
        private readonly SqlDatabase _sqlDatabase;
        private readonly ILogWriter _logWriter;

        public ChurchRepository(ISettingsProvider settingsProvider, ILogger logger)
        {
            var coreSettings = settingsProvider.GetSetting<CoreSettings>();
            _logger = logger;
            _sqlDatabase = new SqlDatabase(coreSettings.Database);
            _logWriter = logger.With(GetType(), LogLevel.Debug);
        }

        public Model.Church GetById(int churchId)
        {

            var command = _sqlDatabase.GetStoredProcCommand("Core.ChurchGetById");
            _sqlDatabase.AddInParameter(command, "Id", DbType.Int32, churchId);

            using (var reader = _sqlDatabase.ExecuteReader(command))
            {
                return ChurchFromReader(reader);
            }
        }

        public bool TryAdd(Model.Church church, out Error error)
        {
            error = default(Error);
            try
            {
                var command = _sqlDatabase.GetStoredProcCommand("Core.ChurchInsert");

                _sqlDatabase.AddInParameter(command, "Name", DbType.String, church.Name);
                _sqlDatabase.AddInParameter(command, "TimeZoneId", DbType.Int32, church.TimeZone.Id);
                _sqlDatabase.AddOutParameter(command, "Id", DbType.Int32, 4);
                _sqlDatabase.ExecuteNonQuery(command);

                var id = (int)_sqlDatabase.GetParameterValue(command, "Id");
                church.Id = id;

                _logWriter.Log("Added church. New Id:{0}.", church.Id);
                return true;
            }
            catch (SqlException sex)
            {
                if (sex.Number == Common.Database.SqlErrorCodes.ConstraintViolation)
                {
                    error = ChurchErrors.DuplicateChurchName;
                    _logWriter.Log("Failed to INSERT church: {0}. Church with name '{1}' already exists.".FormatWith(church.ToXmlString(), church.Name));
                }
                else
                {
                    error = ChurchErrors.UnknownError;
                    _logger.Exception(GetType(), "Failed to INSERT church: {0}.".FormatWith(church.ToXmlString(XmlOptions.Lean)), sex);
                }
            }
            return false;
        }

        public bool TryUpdate(Model.Church church, out Error error)
        {
            try
            {
                error = default(Error);
                var command = _sqlDatabase.GetStoredProcCommand(@"Core.ChurchUpdate");
                _sqlDatabase.AddInParameter(command, "Id", DbType.Int32, church.Id);
                _sqlDatabase.AddInParameter(command, "Name", DbType.String, church.Name);
                _sqlDatabase.AddInParameter(command, "TimeZoneId", DbType.Int32, church.TimeZone.Id);
                _sqlDatabase.AddInParameter(command, "IsArchived", DbType.Boolean, church.IsArchived);
                _sqlDatabase.ExecuteNonQuery(command);
                return true;
                
            }
            catch (SqlException sex)
            {
                if (sex.Number == Common.Database.SqlErrorCodes.ConstraintViolation)
                {
                    error = ChurchErrors.DuplicateChurchName;
                    _logWriter.Log("Failed to UPDATE church: {0}. Church With name '{1}' already exists.".FormatWith(church.ToXmlString(), church.Name));
                }
                else
                {
                    error = ChurchErrors.UnknownError;
                    _logger.Exception(GetType(), "Failed to UPDATE church: {0}.".FormatWith(church.ToXmlString()), sex);
                }
            }
            return false;
        }


        /* object loaders */
        private Model.Church ChurchFromReader(IDataReader reader)
        {
            var church = new Model.Church();
            var firstRowloaded = false;

            while (reader.Read())
            {
                if (!firstRowloaded)
                {
                    church.Id = (int) reader["Id"];
                    church.Name = reader.GetString("Name");
                    church.Created = (DateTime)reader["Created"];
                    church.LastUpdated = (DateTime)reader["LastUpdated"];
                    church.IsArchived = (bool) reader["IsArchived"];
                    church.TimeZone = new Model.TimeZone
                    {
                        Id = (int) reader["TimeZoneId"],
                        Name = reader.GetString("TimeZoneName")
                    };
                    firstRowloaded = true;
                }

                if (reader["LocationId"] != DBNull.Value)
                {
                    church.Locations.Add(new Location
                    {
                        Id = (int)reader["LocationId"],
                        Name = reader.GetString("LocationName"),
                        Address = AddressFromReader(reader)
                    });

                }
            }

            //if first row is never loaded - then proc returned no records, so we return null
            return firstRowloaded ? church : null;
        }

        private Address AddressFromReader(IDataReader reader)
        {
            return new Address
            {
                Id = (int)reader["AddressId"],
                City = reader.GetString("AddressCity"),
                Country = reader.GetString("AddressCountry"),
                PostCode = reader.GetString("AddressPostCode"),
                State = reader.GetString("AddressState"),
                Street1 = reader.GetString("AddressStreet1"),
                Street2 = reader.GetString("AddressStreet2")
            };
        }

    }
}
