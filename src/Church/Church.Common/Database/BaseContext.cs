using System.Data.Entity;

namespace Church.Common.Database
{
    public abstract class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        protected BaseContext() : base("Church")
        {
            System.Data.Entity.Database.SetInitializer<TContext>(null);
        }
    }
}
