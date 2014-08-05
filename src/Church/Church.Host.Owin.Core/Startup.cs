using System.Web.Http;
using Church.Common.Extensions;
using Church.Common.Logging;
using Church.Common.Service;
using Church.Common.Settings;
using Church.Components.Core;
using Church.Components.Core.Repository;
using Microsoft.Owin;
using Owin;
using TinyIoC;

[assembly: OwinStartup(typeof(Church.Host.Owin.Core.Startup))]
namespace Church.Host.Owin.Core
{
    public class Startup
    {
        public static  HttpConfiguration HttpConfiguration;
        public void Configuration(IAppBuilder appBuilder)
        {

            var container = new TinyIoCContainer();

            HttpConfiguration = new HttpConfiguration();
            appBuilder.UseWebApi(HttpConfiguration);

            RegisterComponents(container);
            MappingConfiguration.Configure();

            HttpConfiguration.MapHttpAttributeRoutes();
            HttpConfiguration.SetDependencyResolver(container);

            //start IServices
            var services = container.ResolveAll<IService>();
            services.ForEach(s => s.Start());
        }

        void RegisterComponents(TinyIoCContainer container)
        {
            container.Register<ISettingsProvider, AppSettingsProvider>();
            container.Register<ILogger, Log4NetLogger>();
            container.Register<IChurchRepository, ChurchRepository>().AsSingleton();
            container.Register<IChurchService, ChurchService>().AsSingleton();
            container.Register<IPersonRepository, PersonRepository>().AsSingleton();
            container.Register<IPersonService, PersonService>().AsSingleton();
        }
    }
}
