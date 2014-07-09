using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Church.Host.Core
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var httpConfig = new Church.Host.Core.HttpConfiguration();
            appBuilder.UseWebApi(httpConfig);
        }
    }
}
