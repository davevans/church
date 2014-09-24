using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Church.Common.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleInjector;

namespace Church.Host.Owin.Core
{
    public class HttpConfiguration : System.Web.Http.HttpConfiguration
    {
        public HttpConfiguration()
        {
            
            ConfigureJsonSerialization();
            IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
        }

        public void SetDependencyResolver(Container container)
        {
            DependencyResolver = new WebApiResolver(container);
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
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            SerializerSettings.Formatting = Formatting.Indented;
            SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}
