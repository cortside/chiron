using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Chiron.Auth.WebApi.Data;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;

namespace Chiron.Auth.WebApi.Services {
    public class UserProfileService : IProfileService {
        readonly IUserDbContextFactory contextFactory;

        public UserProfileService(IUserDbContextFactory contextFactory) {
            this.contextFactory = contextFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context) {
            // TODO: how to know if user is external??
            using (var ctx = contextFactory.NewUserDbContext()) {
                var user = await GetUser(ctx, context.Subject);
                if (user != null) {
                    var claims = BuildClaims(user, context.RequestedClaimTypes);
                    context.IssuedClaims = claims;
                } else {
                    var claims = new List<Claim>();
                    Action<string, string> addClaim = (type, value) => {
                        claims.Add(new Claim(type, value));
                    };

                    addClaim("sub", "SUBJECT_ID");
                    addClaim("name", "IDENTITY_NAME");
                    context.IssuedClaims = claims;
                }
            }
        }

        public async Task IsActiveAsync(IsActiveContext context) {
            // TODO: how to know if user is external??
            using (var ctx = contextFactory.NewUserDbContext()) {
                var user = await GetUser(ctx, context.Subject);
                if (user != null) {
                    var now = DateTime.UtcNow;
                    var inEffect = user.EffectiveDate <= now &&
                        (!user.ExpirationDate.HasValue || user.ExpirationDate >= now);
                    context.IsActive = inEffect && user.UserStatus == "Active";
                } else {
                    context.IsActive = true;
                }
            }
        }

        private async Task<User> GetUser(IUserDbContext ctx, IPrincipal principal) {
            var id = (principal.Identity as ClaimsIdentity);
            var sub = id.FindFirst("sub");

            if (sub == null) {
                throw new InvalidOperationException("sub claim is missing.");
            }

            var userId = sub.Value;

            var user = await ctx.Users
            .Include(x => x.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(x => x.UserClaims)
            .FirstOrDefaultAsync(x => x.UserId.ToString() == userId);
            return user;
        }

        private List<Claim> BuildClaims(User user, IEnumerable<string> requestedClaims) {
            var claims = new List<Claim>();
            Action<string, string> addClaim = (type, value) => {
                claims.Add(new Claim(type, value));
            };

            if (!user.UserClaims.Any(x => x.Type == JwtClaimTypes.Subject)) {
                addClaim(JwtClaimTypes.Subject, user.UserId.ToString());
            }
            if (!user.UserClaims.Any(x => x.Type == JwtClaimTypes.Name)) {
                addClaim(JwtClaimTypes.Name, user.Username);
            }

            foreach (var uc in user.UserClaims ?? new List<UserClaim>()) {
                addClaim(uc.Type, uc.Value);
            }

            // TODO: doing this regardless to test
            foreach (var ur in user.UserRoles ?? new List<UserRole>()) {
                addClaim(JwtClaimTypes.Role, ur.Role.Name);
            }

            //if (requestedClaims != null) {
            //    foreach (var requested in requestedClaims) {
            //        //TODO: If there were requested claims, we would add them here.
            //        switch (requested) {
            //            case "role": {
            //                    foreach (var ur in user.UserRoles) {
            //                        addClaim(requested, ur.Role.Name);
            //                    }
            //                }
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}

            return claims;
        }
    }
}
