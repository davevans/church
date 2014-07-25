using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Church.Common.Extensions;
using Church.Common.Mapping;
using Church.Components.Core;
using Church.Host.Owin.Core.ViewModels;

namespace Church.Host.Owin.Core.Controllers
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

        [HttpPost]
        [Route("api/church")]
        public HttpResponseMessage AddChurch([FromBody]ChurchViewModel churchViewModel)
        {
            //todo: validate churchViewModel

            var church = Mapper.Map<ChurchViewModel, Model.Core.Church>(churchViewModel);
            _churchService.Add(church);

            var responseViewModel = Mapper.Map<Model.Core.Church, ChurchViewModel>(church);
            return Request.CreateResponse(HttpStatusCode.Created, responseViewModel);
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
