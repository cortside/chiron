using Cortside.Common.BootStrap;
using Cortside.Common.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Chiron.Catalog.Command.Model.ViewModels;
using System.Collections.Generic;
using Chiron.Catalog.Query;
using Chiron.Catalog.Query.Handler;
using Chiron.Catalog.Query.Model;

namespace Chiron.Catalog.BootStrap.Installer {

    public class QueryInstaller : IInstaller {

        public void Install(IServiceCollection services, IConfigurationRoot configuration) {
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
            services.AddTransient<IQueryHandler<GetItemsQuery, IEnumerable<SimpleItemModel>>, GetItemsHandler>();
        }
    }
}
