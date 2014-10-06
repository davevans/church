using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Church.Common.Extensions;
using Church.Common.Structures;
using Church.Components.Core;
using Church.Host.Owin.Core;
using Church.Host.Owin.Core.ViewModels;
using Church.Host.Owin.Core.ViewModels.Errors;
using Newtonsoft.Json;
using NUnit.Framework;
using Rhino.Mocks;
using SimpleInjector;
using Container = SimpleInjector.Container;
using TimeZone = Church.Components.Core.Model.TimeZone;

namespace Church.IntegrationTests
{
    
    public class ChurchControllerTests : BaseControllerTest
    {

        [SetUp]
        public void Setup() //runs before every test
        {
            Container = new Container(new ContainerOptions
            {
                AllowOverridingRegistrations = true
            });

            Startup.SetContainer(Container);

            Container.RegisterSingle(MockRepository.GenerateStub<IChurchService>());
            Container.RegisterSingle(MockRepository.GenerateStub<IPersonService>());

            var mockAuthService = MockRepository.GenerateStub<IAuthenticationService>();
            mockAuthService.Stub(x => x.Authenticate(null, null)).IgnoreArguments().Return(true);
            Container.RegisterSingle(mockAuthService);

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
                var fakeChurch = new Components.Core.Model.Church
                {
                    Id = churchId,
                    Name = fakeName
                };

                var mockChurchService = MockRepository.GenerateMock<IChurchService>();
                mockChurchService.Expect(x => x.GetByIdAsync(churchId)).Return(Task.FromResult(fakeChurch));

                Container.RegisterSingle(mockChurchService);

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.GetAsync("/api/church/" + churchId).Result;
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
                mockChurchService.Expect(x => x.GetByIdAsync(churchId)).Return(Task.FromResult((Components.Core.Model.Church)null));
                Container.RegisterSingle(mockChurchService);

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.GetAsync("/api/church/" + churchId).Result;

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

                var newChurch = new Components.Core.Model.Church
                {
                    Id = 101,
                    Name = "Foo",
                    TimeZone = new TimeZone
                    {
                        Id = 20,
                        Name = "Sydney"
                    }
                };

                var mockChurchService = MockRepository.GenerateMock<IChurchService>();
                mockChurchService.Expect(x => x.AddAsync(Arg<Components.Core.Model.Church>.Matches(c => c.Name == "Foo")))
                                 .Return(Task.FromResult(newChurch));

                Container.RegisterSingle(mockChurchService);

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;

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

                var fakeResult = new Components.Core.Model.Church
                {
                    Id = 101
                };

                var mockChurchService = MockRepository.GenerateStub<IChurchService>();
                mockChurchService.Stub(x => x.AddAsync(Arg<Components.Core.Model.Church>.Is.Anything))
                                 .Return(Task.FromResult(fakeResult));

                Container.RegisterSingle(mockChurchService);

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;
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
                mockChurchService.Stub(x => x.AddAsync(Arg<Components.Core.Model.Church>.Is.Anything))
                                    .Return(Task.FromResult(new Components.Core.Model.Church
                                    {
                                        Id = 101,
                                        Name = "St Dav",
                                        TimeZone = new TimeZone
                                        {
                                            Id = 20,
                                            Name = "Sydney"
                                        }
                                    }));            
                    
                Container.RegisterSingle(mockChurchService);

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;
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
                Container.RegisterSingle(mockChurchService);

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;

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
                Container.RegisterSingle(mockChurchService);

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;

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
                Container.RegisterSingle(mockChurchService);
                mockChurchService.Expect(x => x.AddAsync(Arg<Components.Core.Model.Church>.Is.Anything))
                                 .Throw(new ErrorException(Types.Core.ChurchErrors.DuplicateChurchName));

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;

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
                Container.RegisterSingle(mockChurchService);
                mockChurchService.Expect(x => x.AddAsync(Arg<Components.Core.Model.Church>.Is.Anything))
                                 .Throw(new ErrorException(Types.Core.ChurchErrors.DuplicateChurchName));

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PostAsJsonAsync("/api/church", churchViewModel).Result;
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
                Container.RegisterSingle(mockChurchService);

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;

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
                Container.RegisterSingle(mockChurchService);

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;
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
                Container.RegisterSingle(mockChurchService);
                mockChurchService.Expect(x => x.GetByIdAsync(churchId)).Return(Task.FromResult((Components.Core.Model.Church)null));

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;

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
                Container.RegisterSingle(mockChurchService);

                mockChurchService.Expect(x => x.GetByIdAsync(churchId))
                                 .Return(Task.FromResult(new Components.Core.Model.Church
                                 {
                                     Name = "Foo",
                                     TimeZone = new TimeZone { Id = 20, Name = "Sydney" }
                                 }));

                mockChurchService.Expect(x => x.UpdateAsync(Arg<Components.Core.Model.Church>.Is.Anything))
                                 .Throw(new ErrorException(Types.Core.ChurchErrors.DuplicateChurchName));
                                 

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;

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
                    Name = "FooNewName",
                    TimeZone = new TimeZoneViewModel
                    {
                        Id = 20,
                        Name = "Sydney"
                    }
                };

                var mockChurchService = MockRepository.GenerateStub<IChurchService>();
                Container.RegisterSingle(mockChurchService);

                mockChurchService.Expect(x => x.GetByIdAsync(churchId))
                                 .Return(Task.FromResult(new Components.Core.Model.Church
                                 {
                                     Name = "FooOldName",
                                     TimeZone = new TimeZone { Id = 20, Name = "Sydney" }
                                 }));

                mockChurchService.Expect(x => x.UpdateAsync(Arg<Components.Core.Model.Church>.Matches(y => y.Name == "FooNewName")))
                                 .Return(Task.FromResult(new Components.Core.Model.Church
                                 {
                                     Name = "FooNewName",
                                     TimeZone = new TimeZone
                                     {
                                         Id = 20, 
                                         Name = "Sydney"
                                     }
                                 }));

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;

                //Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected 200 OK for successful update.");
                mockChurchService.AssertWasCalled(x => x.UpdateAsync(Arg<Components.Core.Model.Church>.Is.Anything));
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
                Container.RegisterSingle(mockChurchService);
                mockChurchService.Expect(x => x.GetByIdAsync(churchId)).Return(Task.FromResult(new Components.Core.Model.Church
                {
                    Name = "Foo",
                    TimeZone = new TimeZone { Id = 20, Name = "Sydney" }
                }));

                mockChurchService.Expect(x => x.UpdateAsync(Arg<Components.Core.Model.Church>.Is.Anything));

                //Act
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.PutAsJsonAsync("/api/church/{0}".FormatWith(churchId), churchViewModel).Result;

                //Assert
                mockChurchService.AssertWasCalled(x => x.UpdateAsync(Arg<Components.Core.Model.Church>.Matches(z => z.Id == churchId)));
                mockChurchService.VerifyAllExpectations();

            }
        }

        [TestFixture]
        public class ArchiveChurchTests : ChurchControllerTests
        {
            [Test]
            public void ReturnsNotFoundForUnknownChurchId()
            {
                //arrange
                const int churchId = 123;

                var mockChurchService = MockRepository.GenerateStub<IChurchService>();
                Container.RegisterSingle(mockChurchService);
                mockChurchService.Expect(x => x.GetByIdAsync(churchId)).Return(Task.FromResult((Components.Core.Model.Church)null));

                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.DeleteAsync("/api/church/{0}".FormatWith(churchId)).Result;

                //Assert
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expected 404 for deleting a church that doesnt exist.");
            }

            [Test]
            public void ReturnsOkForSuccessfulArchive()
            {
                //arrange
                const int churchId = 123;

                var mockChurchService = MockRepository.GenerateStub<IChurchService>();
                Container.RegisterSingle(mockChurchService);
                mockChurchService.Expect(x => x.GetByIdAsync(churchId)).Return(Task.FromResult(new Components.Core.Model.Church
                {
                    Id = churchId,
                    Name = "Foo",
                    IsArchived = false,
                    TimeZone = new TimeZone
                    {
                        Id = 20,    
                        Name = "Sydney"
                    }
                }));

                mockChurchService.Expect(x => x.ArchiveAsync(Arg<Components.Core.Model.Church>.Matches(c => c.Id == churchId)))
                                 .Return(Task.FromResult(new Components.Core.Model.Church
                                 {
                                    Id  = churchId,
                                    Name = "Foo",
                                    IsArchived = false,
                                    TimeZone = new TimeZone
                                    {
                                        Id = 20,
                                        Name = "Sydney"
                                    }
                                 }));


                //ACT
                var httpClient = GetHttpClientWithAuthentication();
                var response = httpClient.DeleteAsync("/api/church/{0}".FormatWith(churchId)).Result;

                //Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected 200 OK for successful archive");
            }
        }
    }

    

    
}
