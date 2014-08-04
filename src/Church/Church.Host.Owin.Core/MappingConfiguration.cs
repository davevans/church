using Church.Common.Mapping;
using Church.Host.Owin.Core.ViewModels;

namespace Church.Host.Owin.Core
{
    internal static class MappingConfiguration
    {
        internal static void Configure()
        {
            Mapper.CreateMap<Components.Core.Model.TimeZone, TimeZoneViewModel>();
            Mapper.CreateMap<TimeZoneViewModel, Components.Core.Model.TimeZone>();

            Mapper.CreateMap<Components.Core.Model.Church, ChurchViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(d => d.Id))
                .ForMember(x => x.Name, o => o.MapFrom(d => d.Name))
                .ForMember(x => x.TimeZone, o => o.MapFrom(d => d.TimeZone))
                .ForMember(x => x.Created, o => o.MapFrom(d => d.Created))
                .ForMember(x => x.LastUpdated, o => o.MapFrom(d => d.LastUpdated));

            Mapper.CreateMap<ChurchViewModel, Components.Core.Model.Church>()
                .ForMember(x => x.Id, o => o.MapFrom(d => d.Id))
                .ForMember(x => x.Name, o => o.MapFrom(d => d.Name))
                .ForMember(x => x.TimeZone, o => o.MapFrom(d => d.TimeZone))
                .ForMember(x => x.Created, o => o.MapFrom(d => d.Created))
                .ForMember(x => x.LastUpdated, o => o.MapFrom(d => d.LastUpdated))
                .ForMember(x => x.IsArchived, o => o.UseValue(false));

            Mapper.CreateMap<Components.Core.Model.Address, AddressViewModel>();

            Mapper.CreateMap<Components.Core.Model.Location, LocationViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(x => x.Id))
                .ForMember(d => d.Name, o => o.MapFrom(x => x.Name))
                .ForMember(d => d.Address, o => o.MapFrom(x => x.Address));
        }
    }
}