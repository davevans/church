using System.Collections.Generic;
using System.Linq;
using Church.Common.Extensions;
using Church.Common.Logging;
using Church.Components.Core.Model;
using Church.Components.Core.Repository;

namespace Church.Components.Core
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogWriter _debug;
        public PersonService(ILogger logger, IPersonRepository personRepository)
        {
            _personRepository = personRepository;
            _debug = logger.With(GetType(), LogLevel.Debug);
        }

        public void Start()
        {
            _debug.Log("Starting PersonService.");
        }

        public void Stop()
        {
            _debug.Log("Stopping PersonService.");
        }

        public IEnumerable<Person> GetPeopleByChurchId(int churchId)
        {
            var people = _personRepository.GetByChurchId(churchId).ToList();
            _debug.Log("Found {0} people in church Id:{1}.".FormatWith(people.Count, churchId));
            return people;
        }
    }
}