using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TinyIoC;

namespace Church.Host.Core
{
    public class HttpConfiguration : System.Web.Http.HttpConfiguration
    {
        public HttpConfiguration()
        {            
            ConfigureJsonSerialization();

            //configure dependency resolution
            var container = new TinyIoCContainer();
            this.DependencyResolver = new Church.Common.Web.TinyIoCWebApiResolver(container);
        }

        void RegisterComponents(TinyIoCContainer container)
        {
            //container.Register<
        }


        void ConfigureJsonSerialization()
        {
            Formatters.Add(new BrowserJsonFormatter());
        }
    }

    public class BrowserJsonFormatter : JsonMediaTypeFormatter
    {
        public BrowserJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            this.SerializerSettings.Formatting = Formatting.Indented;
            this.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}
