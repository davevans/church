using Church.Common.Logging;
using Church.Common.Structures;


namespace Church.Components.Core
{
    public class ChurchService : IChurchService
    {
        private readonly Repository.IChurchRepository _churchRepository;
        private readonly ILogWriter _debug;

        public ChurchService(Repository.IChurchRepository churchRepository, ILogger logger)
        {
            _churchRepository = churchRepository;
            _debug = logger.With(GetType(), LogLevel.Debug);
        }

        public Model.Church GetById(int churchId)
        {
            return _churchRepository.GetById(churchId);
        }

        public void Add(Model.Church church)
        {
            Error error;
            bool success = _churchRepository.TryAdd(church, out error);
        }

        public void Start()
        {
            _debug.Log("Starting ChurchService.");
        }

        public void Stop()
        {
            _debug.Log("Stopping ChurchService.");
        }
    }
}
