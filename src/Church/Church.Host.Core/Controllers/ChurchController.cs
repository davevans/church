using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Church.Common.Extensions;
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
        [Route("api/church/{churchId}")]
        public HttpResponseMessage ChurchById(int churchId)
        {
            var church = _churchService.GetById(churchId);
            return church == null ?
                Request.CreateErrorResponse(HttpStatusCode.NotFound, "Church {0} not found.".FormatWith(churchId))
                : Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Model.Core.Church, ChurchViewModel>(church));
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
