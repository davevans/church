namespace Church.Components.Core
{
    public class ChurchService : IChurchService
    {
        private readonly Repository.ICoreRepository _coreRepository;
        public ChurchService(Repository.ICoreRepository coreRepository)
        {
            _coreRepository = coreRepository;
        }

        public Model.Church GetById(int churchId)
        {
            return _coreRepository.GetById(churchId);
        }

        public void Add(Model.Church church)
        {
            _coreRepository.Add(church);
        }
    }
}
