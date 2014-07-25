using System.Web.Http;
using Church.Common.Mapping;
using Church.Components.Core;
using Church.Components.Core.Repository;
using Church.Host.Owin.Core.ViewModels;
using Owin;
using TinyIoC;

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
            Mapper.CreateMap<Model.Core.Church, ChurchViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(d => d.Id))
                .ForMember(x => x.Name, o => o.MapFrom(d => d.Name))
                .ForMember(x => x.TimeZone, o => o.MapFrom(d => d.TimeZone));

            Mapper.CreateMap<ChurchViewModel, Model.Core.Church>()
                .ForMember(x => x.Id, o => o.MapFrom(d => d.Id))
                .ForMember(x => x.Name, o => o.MapFrom(d => d.Name))
                .ForMember(x => x.TimeZone, o => o.MapFrom(d => d.TimeZone));

            Mapper.CreateMap<Model.Core.Address, AddressViewModel>();

            Mapper.CreateMap<Model.Core.Location, LocationViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(x => x.Id))
                .ForMember(d => d.Name, o => o.MapFrom(x => x.Name))
                .ForMember(d => d.Address, o => o.MapFrom(x => x.Address));
        }
    }
}
