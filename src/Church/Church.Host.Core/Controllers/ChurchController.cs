using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Church.Common.Mapping;
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
            if (church == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Model.Core.Church, ChurchViewModel>(church);
        }

        [HttpGet]
        [Route("api/church/{churchId}/locations")]
        public IEnumerable<LocationViewModel> ChurchLocationsByChurchId(int churchId)
        {
            var church = _churchService.GetById(churchId);
            if (church == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.MapList<Model.Core.Location, LocationViewModel>(church.Locations);
        }
    }
}
