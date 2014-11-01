using System;
using System.Net.Http;
using Newtonsoft.Json;
using Thinktecture.IdentityModel.Client;

namespace Church.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            const string tokenUrl = @"http://localhost:12345/token";
            //const string resourceUrl = @"http://localhost:12345/api/church/1";

            var client = new OAuth2Client(new Uri(tokenUrl));
            var tokenResponse = client.RequestResourceOwnerPasswordAsync("dav", "dav").Result;

            Console.WriteLine("got token {0}.", tokenResponse.AccessToken);

            var httpClient = new HttpClient();
            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var newUser = new AddUserRequestViewModel
            {
                PersonId = 1,
                Password = "Tristessa1"
            };


            var jsonResponse = httpClient.PostAsJsonAsync(@"http://localhost:12345/api/user", newUser).Result;

            var response = JsonConvert.DeserializeObject<AddUserResponseViewModel>(jsonResponse.Content.ReadAsStringAsync().Result);
            Console.WriteLine("Added personId:{0}, userId:{1}, created:{2}.", response.PersonId, response.Id, response.Created);

            
            Console.ReadLine();
        }
    }

    public class AddUserRequestViewModel
    {
        public long PersonId { get; set; }
        public string Password { get; set; }
    }

    public class AddUserResponseViewModel
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public DateTime Created { get; set; }
    }
}
