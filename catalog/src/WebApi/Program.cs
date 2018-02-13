using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Chiron.Catalog.WebApi {
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
