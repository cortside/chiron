using Chiron.Auth.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chiron.Auth.Services {
    public class Authenticator : IAuthenticator {
        readonly IUserDbContextFactory dbFactory;
        readonly IHashProvider hashProvider;

        public Authenticator(IUserDbContextFactory dbFactory, IHashProvider hashProvider) {
            this.dbFactory = dbFactory;
            this.hashProvider = hashProvider;
        }

        public async Task<User> AuthenticateAsync(LoginInfo info) {
            using (var ctx = dbFactory.NewUserDbContext()) {
                var user = await ctx.Users.FirstOrDefaultAsync(x => x.Username == info.Username);
                var pwSalt = (info?.Password ?? string.Empty) + (user?.Salt ?? string.Empty);
                var pwHash = hashProvider.ComputeHash(pwSalt);

                if (user?.Password != pwHash) { user = null; }

                return user;
            }
        }
    }
}
