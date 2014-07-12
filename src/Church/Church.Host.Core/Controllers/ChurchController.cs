using System.Web.Http;
using Church.Components.Core;

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
        public Model.Core.Church Get(int id)
        {
            return _churchService.GetById(id);
        }
    }
}
