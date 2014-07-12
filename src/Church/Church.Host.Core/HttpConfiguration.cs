using Church.Components.Core;
using Church.Components.Core.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
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
            DependencyResolver = new Common.Web.TinyIoCWebApiResolver(container);
            RegisterComponents(container);
        }

        void RegisterComponents(TinyIoCContainer container)
        {
            container.Register<ICoreRepository, CoreRepository>().AsSingleton();
            container.Register<IChurchService, ChurchService>().AsSingleton();
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
