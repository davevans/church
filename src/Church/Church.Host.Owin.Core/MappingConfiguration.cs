using Church.Common.Mapping;
using Church.Components.Account.Model;
using Church.Components.Core.Model;
using Church.Host.Owin.Core.ViewModels;

namespace Church.Host.Owin.Core
{
    internal static class MappingConfiguration
    {
        internal static void Configure()
        {
            Mapper.CreateMap<TimeZone, TimeZoneViewModel>();
            Mapper.CreateMap<TimeZoneViewModel, TimeZone>();

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

            Mapper.CreateMap<Address, AddressViewModel>();

            Mapper.CreateMap<Location, LocationViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(x => x.Id))
                .ForMember(d => d.Name, o => o.MapFrom(x => x.Name))
                .ForMember(d => d.Address, o => o.MapFrom(x => x.Address));


            Mapper.CreateMap<Person, PersonViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(d => d.Id))
                .ForMember(x => x.FirstName, o => o.MapFrom(d => d.FirstName))
                .ForMember(x => x.LastName, o => o.MapFrom(d => d.LastName))
                .ForMember(x => x.MiddleName, o => o.MapFrom(d => d.MiddleName))
                .ForMember(x => x.DateOfBirthDay, o => o.MapFrom(d => d.DateOfBirthDay))
                .ForMember(x => x.DateOfBirthMonth, o => o.MapFrom(d => d.DateOfBirthMonth))
                .ForMember(x => x.DateOfBirthYear, o => o.MapFrom(d => d.DateOfBirthYear))
                .ForMember(x => x.Occupation, o => o.MapFrom(x => x.Occupation))
                .ForMember(x => x.TimeZone, o => o.MapFrom(x => x.TimeZone))
                .ForMember(x => x.Gender, o => o.MapFrom(x => x.Gender));


            Mapper.CreateMap<User, AddUserResponseViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(d => d.Id))
                .ForMember(x => x.PersonId, o => o.MapFrom(d => d.PersonId));

        }
    }
}