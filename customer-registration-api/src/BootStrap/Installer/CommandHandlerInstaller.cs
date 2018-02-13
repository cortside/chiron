using System.Linq;
using System.Reflection;
using Chiron.Registration.Customer.Command.Handler;
using Cortside.Common.BootStrap;
using Cortside.Common.Command;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chiron.Registration.Customer.BootStrap.Installer {
    public class CommandHandlerInstaller : IInstaller {
        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            typeof(CreateCustomerHandler)
            .GetTypeInfo().Assembly.GetTypes()
            .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && t.Name.EndsWith("Handler"))
            .ToList().ForEach(x => {
                var iType = x.GetInterfaces().First();
                services.AddSingleton(iType, x);
            });
        }
    }
}
