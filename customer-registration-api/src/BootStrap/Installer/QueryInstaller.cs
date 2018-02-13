using Cortside.Common.BootStrap;
using Cortside.Common.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chiron.Registration.Customer.BootStrap.Installer {

    public class QueryInstaller : IInstaller {

        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        }
    }
}
