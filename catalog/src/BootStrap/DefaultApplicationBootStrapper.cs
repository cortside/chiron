using Cortside.Common.BootStrap;
using Chiron.Catalog.BootStrap.Installer;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Cortside.Common.IoC;

namespace Chiron.Catalog.BootStrap {

    public class DefaultApplicationBootStrapper : BootStrapper {

	public DefaultApplicationBootStrapper() {
	    installers = new List<IInstaller> {
		new LogInstaller(),
		new SqlInstaller(),
		new QueryInstaller()
		};
	}

	protected override IServiceProvider InternalInitialize(IConfigurationBuilder config, IServiceCollection services, IInstaller[] installers) {
	    var configuration = config.Build();
	    DI.SetConfiguration(configuration);

	    foreach (var i in installers) {
		i.Install(services, configuration);
	    }

	    services.AddSingleton<IConfigurationRoot>(configuration);
	    /* This line causes a runtime error due to api changes in .net standard 2.
	     * Hence, this method is overriden - so that this version of the code is calling BuildServiceProvider() from the right place.
	     */
	    var serviceProvider = services.BuildServiceProvider(); 
	    DI.SetContainer(serviceProvider);
	    return serviceProvider;
	}
    }
}
