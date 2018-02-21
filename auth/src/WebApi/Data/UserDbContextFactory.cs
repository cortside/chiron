using System;
using Microsoft.Extensions.DependencyInjection;

namespace Chiron.Auth.WebApi.Data {
    public class UserDbContextFactory : IUserDbContextFactory {
        readonly IServiceProvider serviceProvider;

        public UserDbContextFactory(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }

        public IUserDbContext NewUserDbContext() {
            return serviceProvider.GetService<IUserDbContext>();
        }
    }
}
