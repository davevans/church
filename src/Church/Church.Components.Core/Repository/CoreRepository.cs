using System.Linq;
using System.Data.Entity;

namespace Church.Components.Core.Repository
{
    public class CoreRepository : ICoreRepository
    {
        private readonly CoreContext _dbContext;
        public CoreRepository()
        {
            _dbContext = new CoreContext();
        }

        public Model.Church GetById(int churchId)
        {
            return _dbContext.Churches
                .Include(x => x.Locations.Select(a => a.Address))
                .Include(x => x.TimeZone)
                .FirstOrDefault(x => x.Id == churchId);
        }

        public void Add(Model.Church church)
        {
            _dbContext.Churches.Add(church);
            _dbContext.SaveChanges();
        }
    }
}
