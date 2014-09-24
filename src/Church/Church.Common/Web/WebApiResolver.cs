using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using SimpleInjector;


namespace Church.Common.Web
{
    public class WebApiResolver : IDependencyResolver
    {
        private readonly Container _container;

        public WebApiResolver(Container container)
        {
            _container = container;
        }


        public void Dispose()
        {
            
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.GetInstance(serviceType);
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
                return _container.GetAllInstances(serviceType);
            }
            catch (Exception)
            {
                return Enumerable.Empty<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }
    }
}
