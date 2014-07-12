namespace Church.Components.Core.Repository
{
    public class CoreRepository : ICoreRepository
    {
        private readonly CoreContext _dbContext;
        public CoreRepository()
        {
            _dbContext = new CoreContext();
        }

        public Model.Core.Church GetById(int churchId)
        {
            var church = _dbContext.Chuches.Find(churchId);
            return church;
        }
    }
}
