using System;
using Cortside.Common.BootStrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Chiron.Admin.BootStrap.Installer {

    public class LogInstaller : IInstaller {

        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            //Log.Logger = new LoggerConfiguration()
            //.ReadFrom.AppSettings()
            //.CreateLogger();

            String filename = Guid.NewGuid().ToString();

            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            //.WriteTo.ColoredConsole(restrictedToMinimumLevel: LogEventLevel.Debug)
            //.WriteTo.RollingFile("../logs/" + filename + "-{Date}.log", outputTemplate: "{Timestamp:O} [{Level}] [{messageId}] {Message}{NewLine}{Exception}")
            .CreateLogger();
        }
    }
}
