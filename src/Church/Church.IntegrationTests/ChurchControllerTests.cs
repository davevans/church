using System.Net;
using Church.Components.Core;
using Church.Host.Core;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using Rhino.Mocks;
using TinyIoC;

namespace Church.IntegrationTests
{
    [TestFixture]
    public class ChurchControllerTests
    {
        private TestServer _server;
        private TinyIoCContainer _container;

        [TestFixtureSetUp]
        public void FixtureInit()
        {
            _server = TestServer.Create<Startup>();
            _container = new TinyIoCContainer();

            Startup.HttpConfiguration.SetDependencyResolver(_container);
        }

        [TestFixtureTearDown]
        public void FixtureDispose()
        {
            _server.Dispose();
        }

        [Test]
        public void GetChurchById_ReturnsChurchWithMatchingIdAndName()
        {
            //ARANGE
            const int churchId = 1;
            const string fakeName = "FakeChurch";

            var mockChurchService = MockRepository.GenerateMock<IChurchService>();
            mockChurchService.Expect(x => x.GetById(churchId)).Return(new Model.Core.Church
            {
                Id = churchId,
                Name = fakeName
            });

            _container.Register(typeof (IChurchService), mockChurchService);


            //ACT
            var response = _server.HttpClient.GetAsync("/api/church/" + churchId).Result;
            var json = response.Content.ReadAsStringAsync().Result;

            //ASSERT
            var actual = JsonConvert.DeserializeObject<Model.Core.Church>(json);
            Assert.AreEqual(churchId, actual.Id);
            Assert.AreEqual(fakeName, actual.Name);
        }

        [Test]
        public void GetChurchById_Returns404ForUnknownChurchId()
        {
            //ARANGE
            const int churchId = 1;

            var mockChurchService = MockRepository.GenerateMock<IChurchService>();
            mockChurchService.Expect(x => x.GetById(churchId)).Return(null);
            _container.Register(typeof(IChurchService), mockChurchService);

            //ACT
            var response = _server.HttpClient.GetAsync("/api/church/" + churchId).Result;

            //ASSERT
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expected 404.");
        }
    }

    

    
}
