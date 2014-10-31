using System;
using System.Web.Http;
using Church.Common.Extensions;
using Church.Common.Logging;
using Church.Common.Service;
using Church.Common.Settings;
using Church.Components.Account;
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
        public static Container Container;

        public void Configuration(IAppBuilder appBuilder)
        {
            var container = new Container();

            HttpConfiguration = new HttpConfiguration();
            MappingConfiguration.Configure();
            HttpConfiguration.MapHttpAttributeRoutes();

            SetContainer(container);

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
            var services = Container.GetAllInstances<IService>();
            services.ForEach(s => s.Start());
        }

        /// <summary>
        /// SetContainer allows us to override the IoC container for unit and integration testing
        /// </summary>
        /// <param name="container"></param>
        public static void SetContainer(Container container)
        {
            Container = container;
            HttpConfiguration.SetDependencyResolver(container);
            RegisterComponents();
        }

        static void RegisterComponents()
        {
            Container.RegisterSingle<IAuthenticationService, AuthenticationService>();
            Container.RegisterSingle<OAuthAuthorizationServerProvider, ChurchOAuthAuthorizationServerProvider>();
            Container.RegisterSingle<ISettingsProvider, AppSettingsProvider>();
            Container.RegisterSingle<ILogger, Log4NetLogger>();
            Container.RegisterSingle<IChurchRepository, ChurchRepository>();
            Container.RegisterSingle<IPersonRepository, PersonRepository>();

            //register IServices
            Container.RegisterSingle<IChurchService, ChurchService>();
            Container.RegisterSingle<IPersonService, PersonService>();

            //register as IService
            Container.RegisterAll<IService>(new[]
            {
                typeof(ChurchService),
                typeof(PersonService)
            });

        }
    }
}
