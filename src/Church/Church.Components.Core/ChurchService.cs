using System;
using System.Threading.Tasks;
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

        public async Task<Model.Church> GetByIdAsync(int churchId)
        {
            return await _churchRepository.GetByIdAsync(churchId);
        }

        public async Task<Model.Church> AddAsync(Model.Church church)
        {
            var result = await _churchRepository.TryAddAsync(church);
            if (!result.IsSuccess)
            {
                throw new ErrorException("Failed to add church.", result.Error);
            }

            _debug.Log("Successfully added church.{0}{1}", Environment.NewLine, church.ToXmlString());
            return result.Value;
        }

        public async Task<Model.Church> UpdateAsync(Model.Church church)
        {
            var result = await _churchRepository.TryUpdateAsync(church);
            if (!result.IsSuccess)
            {
                throw new ErrorException("Failed to update church.", result.Error);
            }

            _debug.Log("Successfully updated church.{0}{1}", Environment.NewLine, church.ToXmlString());
            return result.Value;
        }

        public async Task ArchiveAsync(Model.Church church)
        {
            church.IsArchived = true;
            await UpdateAsync(church);
            _debug.Log("Successfully archived church.{0}{1}", Environment.NewLine, church.ToXmlString());
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
