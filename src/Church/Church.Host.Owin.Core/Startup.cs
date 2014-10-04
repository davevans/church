using System;
using System.Web.Http;
using Church.Common.Extensions;
using Church.Common.Logging;
using Church.Common.Service;
using Church.Common.Settings;
using Church.Components.Core;
using Church.Components.Core.Repository;
using Church.Host.Owin.Core.Authentication;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SimpleInjector;


[assembly: OwinStartup(typeof(Church.Host.Owin.Core.Startup))]
namespace Church.Host.Owin.Core
{
    public class Startup
    {
        public static  HttpConfiguration HttpConfiguration;
        private static Container _container;

        public void Configuration(IAppBuilder appBuilder)
        {
            var container = new Container();

            HttpConfiguration = new HttpConfiguration();
            

            MappingConfiguration.Configure();
            HttpConfiguration.MapHttpAttributeRoutes();

            SetContainer(container);


            //Auth
            appBuilder.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),
                Provider = new ChurchOAuthAuthorizationServerProvider()
            });

            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            appBuilder.UseWebApi(HttpConfiguration);

            //start IServices
            var services = _container.GetAllInstances<IService>();
            services.ForEach(s => s.Start());
        }

        public static void SetContainer(Container container)
        {
            _container = container;
            HttpConfiguration.SetDependencyResolver(container);
            RegisterComponents();
        }

        static void RegisterComponents()
        {
            _container.RegisterSingle<ISettingsProvider, AppSettingsProvider>();
            _container.RegisterSingle<ILogger, Log4NetLogger>();
            _container.RegisterSingle<IChurchRepository, ChurchRepository>();
            _container.RegisterSingle<IPersonRepository, PersonRepository>();

            //register IServices
            _container.RegisterSingle<IChurchService, ChurchService>();
            _container.RegisterSingle<IPersonService, PersonService>();

            //register as IService
            _container.RegisterAll<IService>(new[]
            {
                typeof(ChurchService),
                typeof(PersonService)
            });

        }
    }
}
