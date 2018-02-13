using Cortside.Common.BootStrap;
using Cortside.Common.Command;
using Cortside.Common.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chiron.Catalog.BootStrap.Installer {

    public class WebApiInstaller : IInstaller {

	public void Install(IServiceCollection services, IConfigurationRoot configuration) {
	    services.AddMvc();
	}
    }
}
