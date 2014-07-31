﻿using System.Net;
using System.Net.Http;
using Church.Components.Core;
using Church.Host.Owin.Core;
using Church.Host.Owin.Core.ViewModels;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using Rhino.Mocks;
using TinyIoC;

namespace Church.IntegrationTests
{
    
    public class ChurchControllerTests
    {
        protected TestServer Server;
        protected TinyIoCContainer Container;

        [TestFixtureSetUp]
        public void FixtureInit()
        {
            Server = TestServer.Create<Startup>();
            Container = new TinyIoCContainer();

            Startup.HttpConfiguration.SetDependencyResolver(Container);
        }

        [TestFixtureTearDown]
        public void FixtureDispose()
        {
            Server.Dispose();
        }

        [TestFixture]
        public class GetChurchByIdTests : ChurchControllerTests
        {
            [Test]
            public void ReturnsChurchWithMatchingIdAndName()
            {
                //ARANGE
                const int churchId = 1;
                const string fakeName = "FakeChurch";

                var mockChurchService = MockRepository.GenerateMock<IChurchService>();
                mockChurchService.Expect(x => x.GetById(churchId)).Return(new Components.Core.Model.Church
                {
                    Id = churchId,
                    Name = fakeName
                });

                Container.Register(typeof(IChurchService), mockChurchService);


                //ACT
                var response = Server.HttpClient.GetAsync("/api/church/" + churchId).Result;
                var json = response.Content.ReadAsStringAsync().Result;

                //ASSERT
                var actual = JsonConvert.DeserializeObject<Components.Core.Model.Church>(json);
                Assert.AreEqual(churchId, actual.Id);
                Assert.AreEqual(fakeName, actual.Name);
            }

            [Test]
            public void Returns404ForUnknownChurchId()
            {
                //ARANGE
                const int churchId = 1;

                var mockChurchService = MockRepository.GenerateMock<IChurchService>();
                mockChurchService.Expect(x => x.GetById(churchId)).Return(null);
                Container.Register(typeof(IChurchService), mockChurchService);

                //ACT
                var response = Server.HttpClient.GetAsync("/api/church/" + churchId).Result;

                //ASSERT
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expected 404.");
            }
        }

        [TestFixture]
        public class AddChurchTests : ChurchControllerTests
        {
            [Test]
            public void ReturnsCreatedHttpStatusCode()
            {
                //arrange
                var churchViewModel = new ChurchViewModel
                {
                    Name = "Foo",
                    TimeZone = new TimeZoneViewModel
                    {
                        Id = 20,
                        Name = "Sydney"
                    }
                };

                var mockChurchService = MockRepository.GenerateMock<IChurchService>();
                mockChurchService.Expect(x => x.Add(Arg<Components.Core.Model.Church>.Matches(c => c.Name == "Foo")));
                Container.Register(typeof (IChurchService), mockChurchService);

                //ACT
                var response = Server.HttpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;

                //ASSERT
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "Expected HttpStatusCode of CREATED");
                mockChurchService.VerifyAllExpectations();
            }

            [Test]
            public void ReturnsChurchWithId()
            {
                //arrange
                var churchViewModel = new ChurchViewModel
                {
                    Name = "Foo",
                    TimeZone = new TimeZoneViewModel
                    {
                        Id = 20,
                        Name = "Sydney"
                    }
                };

                var mockChurchService = MockRepository.GenerateStub<IChurchService>();
                mockChurchService.Stub(x => x.Add(Arg<Components.Core.Model.Church>.Is.Anything))
                                 .Callback((Components.Core.Model.Church c) =>
                                 {
                                     c.Id = 101;
                                     return true;
                                 });


                Container.Register(typeof(IChurchService), mockChurchService);

                //ACT
                var response = Server.HttpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;
                var newChurch = JsonConvert.DeserializeObject<ChurchViewModel>(response.Content.ReadAsStringAsync().Result);

                //ASSERT
                Assert.AreEqual(101, newChurch.Id, "expected id 101");
            }

            [Test]
            public void ReturnsChurchWithSameNameAndTimeZoneAsRequest()
            {
                //arrange
                var churchViewModel = new ChurchViewModel
                {
                    Name = "St Dav",
                    TimeZone = new TimeZoneViewModel
                    {
                        Id = 20,
                        Name = "Sydney"
                    }
                };

                var mockChurchService = MockRepository.GenerateStub<IChurchService>();
                mockChurchService.Stub(x => x.Add(Arg<Components.Core.Model.Church>.Is.Anything)).Callback((Components.Core.Model.Church c) => { c.Id = 101; return true; });
                Container.Register(typeof(IChurchService), mockChurchService);

                //ACT
                var response = Server.HttpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;
                var newChurch = JsonConvert.DeserializeObject<ChurchViewModel>(response.Content.ReadAsStringAsync().Result);

                //ASSERT
                Assert.AreEqual(101, newChurch.Id, "expected id 101");
                Assert.AreEqual("St Dav", newChurch.Name);
                Assert.AreEqual(20, newChurch.TimeZone.Id);
                Assert.AreEqual("Sydney", newChurch.TimeZone.Name);

            }

            [Test]
            public void ReturnsBadRequestForChurchWithNoName()
            {
                //arrange
                var churchViewModel = new ChurchViewModel
                {
                    Name = null,
                    TimeZone = new TimeZoneViewModel
                    {
                        Id = 20,
                        Name = "Sydney"
                    }
                };

                var mockChurchService = MockRepository.GenerateStub<IChurchService>();
                Container.Register(typeof(IChurchService), mockChurchService);

                //ACT
                var response = Server.HttpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;

                //ASSERT
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected HttpStatusCode of BadRequest");
            }

            [Test]
            public void ReturnsBadRequestForChurchWithNoTimeZone()
            {
                //arrange
                var churchViewModel = new ChurchViewModel
                {
                    Name = "MyChurch",
                };

                var mockChurchService = MockRepository.GenerateStub<IChurchService>();
                Container.Register(typeof(IChurchService), mockChurchService);

                //ACT
                var response = Server.HttpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;

                //ASSERT
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected HttpStatusCode of BadRequest");
            }

        }







    }

    

    
}
