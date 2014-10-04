using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Client;

namespace Church.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string tokenUrl = @"http://localhost:12345/token";
            const string resourceUrl = @"http://localhost:12345/api/church/1";

            var client = new OAuth2Client(new Uri(tokenUrl));
            var tokenResponse = client.RequestResourceOwnerPasswordAsync("dav", "dav").Result;

            Console.WriteLine("got token {0}.", tokenResponse.AccessToken);

            var httpClient = new HttpClient();
            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var json = httpClient.GetStringAsync(resourceUrl).Result;

            Console.WriteLine("Result {0}.", json);

            Console.ReadLine();
        }
    }
}
