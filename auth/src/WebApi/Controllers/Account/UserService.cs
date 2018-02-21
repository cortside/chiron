using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chiron.Auth.WebApi.Data;
using IdentityModel;
using Microsoft.Extensions.Logging;

namespace Chiron.Auth.WebApi.Controllers.Account {
    public class UserService {
        private readonly IUserDbContextFactory contextFactory;
        private readonly ILogger logger;

        public UserService(IUserDbContextFactory contextFactory, ILogger logger) {
            this.contextFactory = contextFactory;
            this.logger = logger;
        }

        /// <summary>
        /// Finds the user by subject identifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        public User FindBySubjectId(Guid subjectId) {
            using (var ctx = contextFactory.NewUserDbContext()) {
                return ctx.Users.FirstOrDefault(x => x.UserId == subjectId);
            }
        }

        /// <summary>
        /// Finds the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public User FindByUsername(string username) {
            using (var ctx = contextFactory.NewUserDbContext()) {
                return ctx.Users.FirstOrDefault(x => x.Username == username);
            }
        }

        /// <summary>
        /// Finds the user by external provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public User FindByExternalProvider(string provider, string userId) {
            using (var ctx = contextFactory.NewUserDbContext()) {
                return ctx.Users.FirstOrDefault(x => x.ProviderName == provider && x.ProviderSubjectId == userId);
            }
        }

        /// TODO: look at combining with the UserRegisteredHandler
        /// <summary>
        /// Automatically provisions a user.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        public User AutoProvisionUser(string provider, string userId, List<Claim> claims) {
            // create a list of claims that we want to transfer into our store
            var filtered = new List<Claim>();

            foreach (var claim in claims) {
                // if the external system sends a display name - translate that to the standard OIDC name claim
                if (claim.Type == ClaimTypes.Name) {
                    filtered.Add(new Claim(JwtClaimTypes.Name, claim.Value));
                }
                // if the JWT handler has an outbound mapping to an OIDC claim use that
                else if (JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(claim.Type)) {
                    filtered.Add(new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[claim.Type], claim.Value));
                }
                // copy the claim as-is
                else {
                    filtered.Add(claim);
                }
            }

            // if no display name was provided, try to construct by first and/or last name
            if (!filtered.Any(x => x.Type == JwtClaimTypes.Name)) {
                var first = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value;
                var last = filtered.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value;
                if (first != null && last != null) {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
                } else if (first != null) {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first));
                } else if (last != null) {
                    filtered.Add(new Claim(JwtClaimTypes.Name, last));
                }
            }

            // create a new unique subject id
            var sub = Guid.NewGuid();

            // check if a 'upn' (User Principal Name) claim is available otherwise check if display name is available, otherwise fallback to subject id
            var name = filtered.FirstOrDefault(c => c.Type == "upn")?.Value ?? filtered.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value ?? sub.ToString();

            // create new user
            var user = new User {
                UserId = sub,
                Username = name,
                ProviderName = provider,
                ProviderSubjectId = userId,

                // set to someting since values are required (TODO?)
                Password = "EXTERNAL",
                Salt = "",

                EffectiveDate = DateTime.UtcNow,
                UserStatus = "Active",
                TermsOfUseAcceptanceDate = DateTime.UtcNow,
                CreateDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow
            };

            logger.LogInformation($"About to create new user: {name}");
            using (var ctx = contextFactory.NewUserDbContext()) {
                //Probably not a great way of doing this, need a user for audit fields.
                var theFirstUser = ctx.Users.OrderBy(x => x.UserId).First();
                user.CreateUserId = theFirstUser.UserId;
                user.LastModifiedUserId = theFirstUser.UserId;

                /*
                // TODO: get role(s) from groups
                var roleName = "customer";
                var role = ctx.Roles.FirstOrDefault(r => r.Name == roleName);

                if (role == null) {
                    role = new Role {
                        Name = roleName,
                        Description = roleName,
                        CreateDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        CreateUserId = theFirstUser.UserId,
                        LastModifiedUserId = theFirstUser.UserId
                    };
                    ctx.AddRole(role);
                }

                user.UserRoles = new List<UserRole> {
                    new UserRole {
                        User = user,
                        Role = role
                    }
                };
                */

                user.UserClaims = new List<UserClaim>();
                foreach (var c in filtered) {
                    var claim = new UserClaim() {
                        UserId = user.UserId,
                        ProviderName = provider,
                        Type = c.Type,
                        Value = c.Value
                    };
                    user.UserClaims.Add(claim);
                }

                ctx.AddUser(user);
                ctx.SaveChanges();
            }

            return user;
        }
    }

}
