using Church.Common.Mapping;
using Church.Components.Core;
using Church.Components.Core.Repository;
using Owin;
using System.Web.Http;
using TinyIoC;

namespace Church.Host.Core
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {

            var container = new TinyIoCContainer();

            var httpConfig = new HttpConfiguration();
            appBuilder.UseWebApi(httpConfig);

            RegisterComponents(container);
            RegisterMappings();

            httpConfig.MapHttpAttributeRoutes();
            httpConfig.SetDependencyResolver(container);
        }

        void RegisterComponents(TinyIoCContainer container)
        {
            container.Register<ICoreRepository, CoreRepository>().AsSingleton();
            container.Register<IChurchService, ChurchService>().AsSingleton();
        }

        private void RegisterMappings()
        {
            Mapper.CreateMap<Model.Core.Church, ViewModels.ChurchViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(d => d.Id))
                .ForMember(x => x.Name, o => o.MapFrom(d => d.Name))
                .ForMember(x => x.TimeZone, o => o.MapFrom(d => d.TimeZone));

            Mapper.CreateMap<Model.Core.Address, ViewModels.AddressViewModel>();

            Mapper.CreateMap<Model.Core.Location, ViewModels.LocationViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(x => x.Id))
                .ForMember(d => d.Name, o => o.MapFrom(x => x.Name))
                .ForMember(d => d.Address, o => o.MapFrom(x => x.Address));
        }
    }
}
