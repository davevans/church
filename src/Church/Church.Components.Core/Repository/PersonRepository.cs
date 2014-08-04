using System;
using System.Collections.Generic;
using System.Data;
using Church.Common.Extensions;
using Church.Common.Logging;
using Church.Common.Settings;
using Church.Components.Core.Model;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Church.Components.Core.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ILogWriter _debug;
        private readonly SqlDatabase _sqlDatabase;

        public PersonRepository(ISettingsProvider settingsProvider, ILogger logger)
        {
            var coreSettings = settingsProvider.GetSetting<CoreSettings>();
            _sqlDatabase = new SqlDatabase(coreSettings.Database);

            _debug = logger.With(GetType(), LogLevel.Debug);
        }


        public IEnumerable<Person> GetByChurchId(int churchId)
        {
            var command = _sqlDatabase.GetStoredProcCommand("Core.PersonByChurchId");
            _sqlDatabase.AddInParameter(command, "ChurchId", DbType.Int32);

            var people = _sqlDatabase.LoadObjects(command, ToPerson);
            _debug.Log("Returned {0} in church Id:{1}.", people.Count, churchId);
            return people;
        }

        public Person GetById(long personId)
        {
            throw new NotImplementedException();
        }

        #region DataReaderToObjects

        private static Person ToPerson(IDataReader reader)
        {
            var person = new Person();
            person.Id = (int) reader["Id"];
            person.FirstName = reader.GetString("FirstName");
            person.MiddleName = reader.GetString("MiddleName");
            person.LastName = reader.GetString("LastName");
            person.DateOfBirthDay = (int?) reader["DateOfBirthDay"];
            person.DateOfBirthMonth = (int?) reader["DateOfBirthMonth"];
            person.DateOfBirthYear = (int?) reader["DateOfBirthYear"];
            person.Gender = (bool) reader["IsMale"] ? Gender.Male : Gender.Female;
            person.IsArchived = (bool) reader["IsArchived"];
            person.TimeZone = new Model.TimeZone
            {
                Id = (int)reader["TimeZoneId"],
                Name = reader.GetString("TimeZoneName")
            };
            return person;
        }

        #endregion
    }
}
