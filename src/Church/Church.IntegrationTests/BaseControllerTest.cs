using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Church.Host.Owin.Core;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using SimpleInjector;
using Thinktecture.IdentityModel.Client;

namespace Church.IntegrationTests
{
    public abstract class BaseControllerTest
    {
        protected TestServer Server;
        protected Container Container;

        [TestFixtureSetUp]
        public void FixtureInit()
        {
            Server = TestServer.Create<Startup>();
        }

        [TestFixtureTearDown]
        public void FixtureDispose()
        {
            Server.Dispose();
        }

        protected async Task<string> GetAuthToken()
        {
            IEnumerable<KeyValuePair<string, string>> oauthDictionary = new Dictionary<string, string>()
            {
                {"grant_type","password"},
                {"username","dav"},
                {"password","dav"}
            };

            HttpResponseMessage response = Server.HttpClient.PostAsync(@"/token", (HttpContent)new FormUrlEncodedContent(oauthDictionary)).Result;
            TokenResponse tokenResponse;
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest)
            {
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                tokenResponse = new TokenResponse(content);
            }
            else
                tokenResponse = new TokenResponse(response.StatusCode, response.ReasonPhrase);

            return tokenResponse.AccessToken;
        }

        public HttpClient GetHttpClientWithAuthentication()
        {
            var token = GetAuthToken().Result;
            var httpClient = Server.HttpClient;
            httpClient.SetBearerToken(token);
            return httpClient;
        }
    }
}
