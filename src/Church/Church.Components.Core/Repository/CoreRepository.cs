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

        public Model.Core.Church GetById(int churchId)
        {
            return _dbContext.Churches
                .Include(x => x.Locations.Select(a => a.Address))
                .Include(x => x.TimeZone)
                .FirstOrDefault();
            
        }
    }
}
