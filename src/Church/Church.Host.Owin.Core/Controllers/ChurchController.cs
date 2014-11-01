using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Church.Common.Extensions;
using Church.Common.Mapping;
using Church.Common.Structures;
using Church.Components.Core;
using Church.Host.Owin.Core.ViewModels;
using Church.Host.Owin.Core.ViewModels.Errors;

namespace Church.Host.Owin.Core.Controllers
{
    [Authorize]
    public class ChurchController : ApiController
    {
        private readonly IChurchService _churchService;
        private readonly IPersonService _personService;

        public ChurchController(IChurchService churchService, IPersonService personService)
        {
            _churchService = churchService;
            _personService = personService;
        }

        [HttpGet]
        [Route("api/church/{churchId}")]
        public async Task<HttpResponseMessage> ChurchById(int churchId)
        {
            var church = await _churchService.GetByIdAsync(churchId);
            if (church == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new NotFoundViewModel
                {
                    ErrorMessage = @"Church with Id:{0} not found.".FormatWith(churchId)
                });
            }

            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Components.Core.Model.Church, ChurchViewModel>(church));
        }

        [HttpPost]
        [Route("api/church")]
        public async Task<HttpResponseMessage> AddChurch([FromBody]ChurchViewModel churchViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new BadRequestViewModel(ModelState));
                }

                var church = Mapper.Map<ChurchViewModel, Components.Core.Model.Church>(churchViewModel);
                var newlyCreatedChurch = await _churchService.AddAsync(church);

                var responseViewModel = Mapper.Map<Components.Core.Model.Church, ChurchViewModel>(newlyCreatedChurch);
                return Request.CreateResponse(HttpStatusCode.Created, responseViewModel);
            }
            catch (ErrorException errorException)
            {
                if (errorException.Error.Code == Types.Core.ChurchErrors.DUPLICATE_CHURCH_NAME)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new BadRequestViewModel
                    {
                        Errors = new List<string> { @"The church name '{0}' already exists. Church names must be unique.".FormatWith(churchViewModel.Name) }
                    });
                }

                throw;
            }
        }

        [HttpPut]
        [Route("api/church/{churchId}")]
        public async Task<HttpResponseMessage> UpdateChurch(int churchId, [FromBody] ChurchViewModel churchViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new BadRequestViewModel
                {
                    Errors = ModelState.Values.SelectMany(e => e.Errors)
                                              .Select(x => x.ErrorMessage)
                                              .ToList()
                });
            }

            var existing = await _churchService.GetByIdAsync(churchId);
            if (existing == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new NotFoundViewModel
                {
                    ErrorMessage = @"Church with Id:{0} not found.".FormatWith(churchId)
                }); 
            }

            try
            {
                churchViewModel.Id = churchId;
                var church = Mapper.Map<ChurchViewModel, Components.Core.Model.Church>(churchViewModel);
                var updated = await _churchService.UpdateAsync(church);
                var viewmodel = Mapper.Map<Components.Core.Model.Church, ChurchViewModel>(updated);
                return Request.CreateResponse(HttpStatusCode.OK, viewmodel);
            }
            catch (ErrorException errorException) 
            {
                if (errorException.Error.Code == Types.Core.ChurchErrors.DUPLICATE_CHURCH_NAME)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new BadRequestViewModel
                    {
                        Errors = new List<string> { @"The church name '{0}' already exists. Church names must be unique.".FormatWith(churchViewModel.Name) }
                    });
                }

                throw;
            }
        }

        [HttpGet]
        [Route("api/church/{churchId}/locations")]
        public async Task<HttpResponseMessage> ChurchLocationsByChurchId(int churchId)
        {
            var church = await _churchService.GetByIdAsync(churchId);
            if (church == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new NotFoundViewModel
                {
                    ErrorMessage = @"Church with Id:{0} not found.".FormatWith(churchId)
                });
            }
            var locationViewModels = Mapper.MapList<Components.Core.Model.Location, LocationViewModel>(church.Locations);
            return Request.CreateResponse(HttpStatusCode.OK, locationViewModels);
        }

        [HttpGet]
        [Route("api/church/{churchId}/people")]
        public async Task<HttpResponseMessage> ChurchPeople(int churchId)
        {
            var church = await _churchService.GetByIdAsync(churchId);
            if (church == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new NotFoundViewModel
                {
                    ErrorMessage = @"Church with Id:{0} not found.".FormatWith(churchId)
                });
            }

            var people = _personService.GetPeopleByChurchId(churchId);
            var personViewModels = Mapper.MapList<Components.Core.Model.Person, PersonViewModel>(people);

            return Request.CreateResponse(HttpStatusCode.OK, personViewModels);
        }



        [HttpDelete]
        [Route("api/church/{churchId}")]
        public async Task<HttpResponseMessage> ArchiveChurch(int churchId)
        {
            var church = await _churchService.GetByIdAsync(churchId);
            if (church == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new NotFoundViewModel
                {
                    ErrorMessage = @"Church with Id:{0} not found.".FormatWith(churchId)
                }); 
            }

            await _churchService.ArchiveAsync(church);
            return Request.CreateResponse(HttpStatusCode.OK);
        }


    }
}
