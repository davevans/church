using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Church.Model.Core;

namespace Church.Host.Core.Controllers
{
    public class ChurchController : ApiController
    {
        [HttpGet]
        public Church.Model.Core.Church Get(int id)
        {
            return new Church.Model.Core.Church { Id = 1, Name = "Erko", TimeZone = new Church.Model.Core.TimeZone { Id = 1, Name = "Whatevs" } };
        }
    }
}
