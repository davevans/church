using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Church.Components.Core;
using Church.Host.Core.ViewModels;

namespace Church.Host.Core.Controllers
{
    public class ChurchController : ApiController
    {
        private readonly IChurchService _churchService;
        public ChurchController(IChurchService churchService)
        {
            _churchService = churchService;
        }

        [HttpGet]
        [Route("api/church/{id}")]
        public ChurchViewModel ChurchById(int id)
        {
            var church = _churchService.GetById(id);
            return new ChurchViewModel
            {
                Id = church.Id,
                Name = church.Name,
                TimeZone = church.TimeZone
            };
        }

        [HttpGet]
        [Route("api/church/{churchId}/locations")]
        public IEnumerable<LocationViewModel> ChurchLocationsByChurchId(int churchId)
        {
            var church = _churchService.GetById(churchId);
            var locationViewModels = church.Locations.Select(x => new LocationViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = new AddressViewModel
                {
                    Street1 = x.Address.Street1,
                    Street2 = x.Address.Street2,
                    City = x.Address.City,
                    PostCode = x.Address.PostCode,
                    Country = x.Address.Country,
                    State = x.Address.State,
                    Id = x.Address.Id
                }
            });
            return locationViewModels;
        }
    }
}
