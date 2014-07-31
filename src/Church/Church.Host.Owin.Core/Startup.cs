using System.Web.Http;
using Church.Common.Mapping;
using Church.Components.Core;
using Church.Components.Core.Repository;
using Church.Host.Owin.Core.ViewModels;
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
            RegisterMappings();

            HttpConfiguration.MapHttpAttributeRoutes();
            HttpConfiguration.SetDependencyResolver(container);
        }

        void RegisterComponents(TinyIoCContainer container)
        {
            container.Register<ICoreRepository, CoreRepository>().AsSingleton();
            container.Register<IChurchService, ChurchService>().AsSingleton();
        }

        private void RegisterMappings()
        {
            Mapper.CreateMap<Components.Core.Model.TimeZone, TimeZoneViewModel>();
            Mapper.CreateMap<TimeZoneViewModel, Components.Core.Model.TimeZone>();

            Mapper.CreateMap<Components.Core.Model.Church, ChurchViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(d => d.Id))
                .ForMember(x => x.Name, o => o.MapFrom(d => d.Name))
                .ForMember(x => x.TimeZone, o => o.MapFrom(d => d.TimeZone));

            Mapper.CreateMap<ChurchViewModel, Components.Core.Model.Church>()
                .ForMember(x => x.Id, o => o.MapFrom(d => d.Id))
                .ForMember(x => x.Name, o => o.MapFrom(d => d.Name))
                .ForMember(x => x.TimeZone, o => o.MapFrom(d => d.TimeZone));

            Mapper.CreateMap<Components.Core.Model.Address, AddressViewModel>();

            Mapper.CreateMap<Components.Core.Model.Location, LocationViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(x => x.Id))
                .ForMember(d => d.Name, o => o.MapFrom(x => x.Name))
                .ForMember(d => d.Address, o => o.MapFrom(x => x.Address));
        }
    }
}
