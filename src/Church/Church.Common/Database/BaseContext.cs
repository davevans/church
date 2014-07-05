using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Common.Database
{
    public abstract class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        public BaseContext() : base("Church")
        {
            System.Data.Entity.Database.SetInitializer<TContext>(null);
        }
    }
}
