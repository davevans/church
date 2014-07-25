using System.Net;
using System.Net.Http;
using Church.Components.Core;
using Church.Host.Owin.Core;
using Church.Host.Owin.Core.ViewModels;
using Church.Model.Core;
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

        [Test]
        public void AddChurch_Returns201WhenChurchSucesfullyCreated()
        {
            //ARRANGE
            var churchToAdd = new ChurchViewModel
            {
                Name = "NewChurch",
                TimeZone = new TimeZone
                {
                    Id = 20,
                    Name = "Sydney"
                }
            };

            const int newChurchId = 10;
            var mockChurchService = MockRepository.GenerateMock<IChurchService>();
            mockChurchService.Stub(x => x.Add(Arg<Model.Core.Church>.Is.Anything))
                             .WhenCalled(c => ((Model.Core.Church) c.Arguments[0]).Id = newChurchId);

            //ACT
            var requestBody = JsonConvert.SerializeObject(churchToAdd);
            var response = _server.HttpClient.PostAsJsonAsync("/api/church", new StringContent(requestBody)).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;

            //ASSERT
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var actualChurchViewModel = JsonConvert.DeserializeObject<ChurchViewModel>(responseJson);
            Assert.IsNotNull(actualChurchViewModel, "Expected not null");
            Assert.AreEqual(actualChurchViewModel.Name, churchToAdd.Name);
            Assert.AreEqual(actualChurchViewModel.Id, newChurchId, "Expected an Id on church repsonse of " + newChurchId);
        }
    }

    

    
}
