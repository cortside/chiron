using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Chiron.Registration.Customer.WebApi {
    /// <summary>
    /// The program main method initializing and running the webhost.
    /// </summary>
    public class Program {
        /// <summary>
        /// The program main method initializing and running the webhost.
        /// </summary>
        public static void Main(string[] args) {
            var host = new WebHostBuilder()
            .UseKestrel()
            .UseIISIntegration()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .Build();

            host.Run();
        }
    }
}
