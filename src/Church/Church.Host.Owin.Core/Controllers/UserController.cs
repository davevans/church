using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Church.Common.Extensions;
using Church.Common.Logging;
using Church.Common.Structures;
using Church.Common.Xml;
using Church.Components.Account;
using Church.Components.Account.Model;
using Church.Host.Owin.Core.ViewModels;
using Church.Host.Owin.Core.ViewModels.Errors;

namespace Church.Host.Owin.Core.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly ILogWriter _debugLogWriter;

        public UserController(IUserService userService, ILogger logger)
        {
            _userService = userService;
            _logger = logger;
            _debugLogWriter = _logger.With(GetType(), LogLevel.Debug);
        }

        [HttpPost]
        [Route("api/user")]
        public async Task<HttpResponseMessage> AddUser([FromBody] AddUserRequestViewModel userRequestViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new BadRequestViewModel(ModelState));
                }

                var newUser = await _userService.AddUserFromPerson(userRequestViewModel.PersonId, userRequestViewModel.Password);
                var addUserResponseViewModel = Mapper.Map<User, AddUserResponseViewModel>(newUser);

                _debugLogWriter.Log("Returning new user response. {0}.", addUserResponseViewModel.ToXmlString());
                return Request.CreateResponse(HttpStatusCode.Created, addUserResponseViewModel);
            }
            catch (ErrorException errorException)
            {
                if (errorException.Error.Code == Types.Core.AccountErrors.DUPLICATE_PERSON_ID)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new BadRequestViewModel
                    {
                        Errors = new List<string> { @"A user already exists for person with id {0}.".FormatWith(userRequestViewModel.PersonId) }
                    });
                }

                throw;
            }
        }
    }
}
