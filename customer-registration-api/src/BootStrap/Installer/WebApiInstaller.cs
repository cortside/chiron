using Cortside.Common.BootStrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chiron.Registration.Customer.BootStrap.Installer {

    public class WebApiInstaller : IInstaller {

        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            services.AddMvc();
        }
    }
}
