using Microsoft.Owin.Hosting;
using System;

namespace Church.Host.Core
{
    class Program
    {
        static void Main()
        {
            const string baseAddress = "http://localhost:9000/";
            using (WebApp.Start<Startup>(baseAddress))
            {

                Console.WriteLine("Listening at {0}.", baseAddress);
                Console.ReadLine();
            }
        }
    }
}
