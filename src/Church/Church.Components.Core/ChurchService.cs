using System;
using Church.Common.Logging;
using Church.Common.Structures;
using Church.Common.Xml;


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
            if (!_churchRepository.TryAdd(church, out error))
            {
                throw new ErrorException("Failed to add church.", error);
            }

            _debug.Log("Successfully added church.{0}{1}", Environment.NewLine, church.ToXmlString());
        }

        public void Update(Model.Church church)
        {
            Error error;
            if (!_churchRepository.TryUpdate(church, out error))
            {
                throw new ErrorException("Failed to update church.", error);
            }
            _debug.Log("Successfully updated church.{0}{1}", Environment.NewLine, church.ToXmlString());
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
