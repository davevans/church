using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Church.Common.Extensions;
using Church.Common.Mapping;
using Church.Common.Structures;
using Church.Components.Core;
using Church.Host.Owin.Core.ViewModels;
using Church.Host.Owin.Core.ViewModels.Errors;

namespace Church.Host.Owin.Core.Controllers
{
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
        public HttpResponseMessage ChurchById(int churchId)
        {
            var church = _churchService.GetById(churchId);
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
        public HttpResponseMessage AddChurch([FromBody]ChurchViewModel churchViewModel)
        {
            try
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

                var church = Mapper.Map<ChurchViewModel, Components.Core.Model.Church>(churchViewModel);
                _churchService.Add(church);

                var responseViewModel = Mapper.Map<Components.Core.Model.Church, ChurchViewModel>(church);
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
        public HttpResponseMessage UpdateChurch(int churchId, [FromBody] ChurchViewModel churchViewModel)
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

            var existing = _churchService.GetById(churchId);
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
                _churchService.Update(church);
                var viewmodel = Mapper.Map<Components.Core.Model.Church, ChurchViewModel>(church);
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
        public HttpResponseMessage ChurchLocationsByChurchId(int churchId)
        {
            var church = _churchService.GetById(churchId);
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
        public HttpResponseMessage ChurchPeople(int churchId)
        {
            
            
            
            var church = _churchService.GetById(churchId);
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
        public HttpResponseMessage ArchiveChurch(int churchId)
        {
            var church = _churchService.GetById(churchId);
            if (church == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new NotFoundViewModel
                {
                    ErrorMessage = @"Church with Id:{0} not found.".FormatWith(churchId)
                }); 
            }

            _churchService.Archive(church);
            return Request.CreateResponse(HttpStatusCode.OK);
        }


    }
}
