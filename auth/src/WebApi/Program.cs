using System;
using System.IO;
using Chiron.Auth.WebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Chiron.Auth.WebApi {

    public class Program {
        /// <summary>
        /// Load logging configuration for the app
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddJsonFile("config.json", false, true)
            .AddJsonFile("build.json", false, true)
            .AddEnvironmentVariables()
            .Build();

        /// <summary>
        /// Startup method
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Main(string[] args) {
            var config = Configuration;
            var build = Configuration.GetSection("Build").Get<Build>();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("service", System.Reflection.Assembly.GetEntryAssembly().FullName)
                .Enrich.WithProperty("build-version", build.Version)
                .Enrich.WithProperty("build-tag", build.Tag)
                .CreateLogger();


            try {
                Log.Information("Starting web host");

                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseConfiguration(config)
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .Build();

                host.Run();
                return 0;
            } catch (Exception ex) {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            } finally {
                Log.CloseAndFlush();
            }
        }
    }
}
