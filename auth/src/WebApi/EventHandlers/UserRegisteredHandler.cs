using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chiron.Auth.Data;
using Cortside.Common.DomainEvent;
using Microsoft.Extensions.Logging;

namespace Chiron.Auth.EventHandlers {
    public class UserRegisteredHandler : IDomainEventHandler<UserRegisteredEvent> {
        private readonly ILogger<UserRegisteredHandler> logger;
        private readonly IUserDbContextFactory contextFactory;

        public UserRegisteredHandler(IUserDbContextFactory contextFactory, ILogger<UserRegisteredHandler> logger) {
            this.logger = logger;
            this.contextFactory = contextFactory;
        }

        public async Task Handle(UserRegisteredEvent @event) {
            logger.LogInformation($"Received event: {@event.FirstName} {@event.LastName} ({@event.Email}) has registered.");

            var newUser = new User {
                EffectiveDate = DateTime.UtcNow,
                UserStatus = "Active",
                Username = @event.Email,
                Password = @event.Password,
                Salt = @event.Salt,
                TermsOfUseAcceptanceDate = @event.AgreeToTerms,
                CreateDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow
            };

            using (var ctx = contextFactory.NewUserDbContext()) {
                //Probably not a great way of doing this, need a user for audit fields.
                var theFirstUser = ctx.Users.OrderBy(x => x.UserId).First();
                var role = ctx.Roles.FirstOrDefault(r => r.Name == @event.Role);

                if (role == null) {
                    role = new Role {
                        Name = @event.Role,
                        Description = @event.Role,
                        CreateDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow,
                        CreateUserId = theFirstUser.UserId,
                        LastModifiedUserId = theFirstUser.UserId
                    };
                    ctx.AddRole(role);
                }

                newUser.CreateUserId = theFirstUser.UserId;
                newUser.LastModifiedUserId = theFirstUser.UserId;
                newUser.UserRoles = new List<UserRole> {
                    new UserRole {
                        User = newUser,
                        Role = role
                    }
                };

                ctx.AddUser(newUser);

                await ctx.SaveChangesAsync();
            }
        }
    }
}
