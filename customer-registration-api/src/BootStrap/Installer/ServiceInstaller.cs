using Chiron.Registration.Customer.Command.Handler;
using Cortside.Common.BootStrap;
using Cortside.Common.DomainEvent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chiron.Registration.Customer.BootStrap.Installer {
    public class ServiceInstaller : IInstaller {
        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            services.AddSingleton<IHashProvider, HashProvider>();
            services.AddSingleton<IDomainEventPublisher, DomainEventPublisher>();
            var config = configuration.GetSection("ServiceBus");
            var settings = new ServiceBusSettings {
                Address = config.GetValue<string>("Address"),
                AppName = config.GetValue<string>("AppName"),
                Protocol = config.GetValue<string>("Protocol"),
                PolicyName = config.GetValue<string>("Policy"),
                Key = config.GetValue<string>("Key"),
                Namespace = config.GetValue<string>("Namespace")
            };
            services.AddSingleton(settings);
        }
    }
}
