using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;

namespace Church.Common.Web
{
    public class TinyIoCWebApiResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        private TinyIoCContainer _container;

        public TinyIoCWebApiResolver(TinyIoCContainer container)
        {
            _container = container;
        }

        public System.Web.Http.Dependencies.IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return Enumerable.Empty<object>();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
