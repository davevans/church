namespace Church.Components.Core
{
    public class ChurchService : IChurchService
    {
        private readonly Repository.ICoreRepository _coreRepository;
        public ChurchService(Repository.ICoreRepository coreRepository)
        {
            _coreRepository = coreRepository;
        }

        public Model.Core.Church GetById(int churchId)
        {
            return _coreRepository.GetById(churchId);
        }
    }
}
