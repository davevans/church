using System.Net;
using System.Net.Http;
using Church.Common.Extensions;
using Church.Common.Structures;
using Church.Components.Core;
using Church.Components.Core.Model;
using Church.Host.Owin.Core;
using Church.Host.Owin.Core.ViewModels;
using Church.Host.Owin.Core.ViewModels.Errors;
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

            [Test]
            public void ReturnsBadRequestForDuplicateChurchName()
            {
                //arrange
                var churchViewModel = new ChurchViewModel
                {
                    Name = "MyChurch",
                };

                var mockChurchService = MockRepository.GenerateStub<IChurchService>();
                Container.Register(typeof(IChurchService), mockChurchService);
                mockChurchService.Expect(x => x.Add(Arg<Components.Core.Model.Church>.Is.Anything))
                                 .Throw(new ErrorException(Types.Core.ChurchErrors.DuplicateChurchName));

                //ACT
                var response = Server.HttpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;

                //ASSERT
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected HttpStatusCode of BadRequest");
            }

            [Test]
            public void ReturnsBadRequestForDuplicateChurchNameWithErrorMessages()
            {
                //arrange
                var churchViewModel = new ChurchViewModel
                {
                    Name = "MyChurch",
                };

                var mockChurchService = MockRepository.GenerateStub<IChurchService>();
                Container.Register(typeof(IChurchService), mockChurchService);
                mockChurchService.Expect(x => x.Add(Arg<Components.Core.Model.Church>.Is.Anything))
                                 .Throw(new ErrorException(Types.Core.ChurchErrors.DuplicateChurchName));

                //ACT
                var response = Server.HttpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;
                var badrequestViewModel = JsonConvert.DeserializeObject<BadRequestViewModel>(response.Content.ReadAsStringAsync().Result);

                //ASSERT
                Assert.IsNotNull(badrequestViewModel);
                Assert.IsNotEmpty(badrequestViewModel.Errors, "Expected error messages");

            }

        }

        [TestFixture]
        public class UpdateChurchTests : ChurchControllerTests
        {

            [Test]
            public void ReturnsBadRequestForChurchWithNoTimeZone()
            {
                //arrange
                const int churchId = 123;
                var churchViewModel = new ChurchViewModel
                {
                    Name = "MyChurch",
                };

                var mockChurchService = MockRepository.GenerateStub<IChurchService>();
                Container.Register(typeof(IChurchService), mockChurchService);

                //ACT
                var response = Server.HttpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;

                //ASSERT
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, @"Expected bad request for null TimeZone.");
            }

            [Test]
            public void ReturnsBadRequestWithErrorsForNullName()
            {
                //arrange
                const int churchId = 123;
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
                var response = Server.HttpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;
                var badRequestViewModel = JsonConvert.DeserializeObject<BadRequestViewModel>(response.Content.ReadAsStringAsync().Result);

                //ASSERT
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, @"Expected bad request for null Name.");
                Assert.IsNotNull(badRequestViewModel.ErrorMessage, "Expected error message on badRequestViewModel");
                Assert.IsNotEmpty(badRequestViewModel.Errors, "Expected non empty error collection on badRequestViewModel");
            }

            [Test]
            public void ReturnsNotFoundIfChurchDoesntExist()
            {
                //arrange
                const int churchId = 123;
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
                Container.Register(typeof(IChurchService), mockChurchService);
                mockChurchService.Expect(x => x.GetById(churchId)).Return(null);

                //ACT
                var response = Server.HttpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;

                //Assert
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expected 404 for church that doesnt exist.");
            }

            [Test]
            public void ReturnsBadRequestForDuplicateName()
            {
                //Arrange
                const int churchId = 123;
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
                Container.Register(typeof(IChurchService), mockChurchService);

                mockChurchService.Expect(x => x.GetById(churchId))
                                 .Return(new Components.Core.Model.Church
                                 {
                                    Name = "Foo",
                                    TimeZone = new TimeZone {Id = 20, Name = "Sydney"}
                                 });

                mockChurchService.Expect(x => x.Update(Arg<Components.Core.Model.Church>.Is.Anything))
                                 .Throw(new ErrorException(Types.Core.ChurchErrors.DuplicateChurchName));
                                 

                //ACT
                var response = Server.HttpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;

                //Assert
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected bad request for duplicate church name.");
                mockChurchService.VerifyAllExpectations();
            }

            [Test]
            public void ReturnsOkForSuccessfulUpdate()
            {
                //Arrange
                const int churchId = 123;
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
                Container.Register(typeof(IChurchService), mockChurchService);

                mockChurchService.Expect(x => x.GetById(churchId))
                                 .Return(new Components.Core.Model.Church
                                 {
                                     Name = "Foo",
                                     TimeZone = new TimeZone { Id = 20, Name = "Sydney" }
                                 });

                //ACT
                var response = Server.HttpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;

                //Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected 200 OK for successful update.");
                mockChurchService.AssertWasCalled(x => x.Update(Arg<Components.Core.Model.Church>.Is.Anything));
                mockChurchService.VerifyAllExpectations();
            }

            [Test]
            public void ChurchObjectPassedToReposHasRequestedChurchId()
            {
                //Arrange
                const int churchId = 123;
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
                Container.Register(typeof(IChurchService), mockChurchService);
                mockChurchService.Expect(x => x.GetById(churchId))
                                .Return(new Components.Core.Model.Church
                                {
                                    Name = "Foo",
                                    TimeZone = new TimeZone { Id = 20, Name = "Sydney" }
                                });

                mockChurchService.Expect(x => x.Update(Arg<Components.Core.Model.Church>.Is.Anything));

                //Act
                var response = Server.HttpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;

                //Assert
                mockChurchService.AssertWasCalled(x => x.Update(Arg<Components.Core.Model.Church>.Matches(z => z.Id == churchId)));
                mockChurchService.VerifyAllExpectations();

            }
        }
    }

    

    
}
