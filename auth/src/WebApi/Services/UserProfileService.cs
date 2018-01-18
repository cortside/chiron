using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Chiron.Auth.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Security.Claims;
using IdentityModel;

namespace Chiron.Auth.Services {
    public class UserProfileService : IProfileService {
	readonly IUserDbContextFactory contextFactory;

	public UserProfileService(IUserDbContextFactory contextFactory) {
	    this.contextFactory = contextFactory;
	}

	public async Task GetProfileDataAsync(ProfileDataRequestContext context) {
	    using (var ctx = contextFactory.NewUserDbContext()) {
		var user = await GetUser(ctx, context.Subject);
		var claims = BuildClaims(user, context.RequestedClaimTypes);
		context.IssuedClaims = claims;
	    }
	}

	public async Task IsActiveAsync(IsActiveContext context) {
	    using (var ctx = contextFactory.NewUserDbContext()) {
		var user = await GetUser(ctx, context.Subject);
		var now = DateTime.UtcNow;
		var inEffect = user.EffectiveDate <= now &&
		    (!user.ExpirationDate.HasValue || user.ExpirationDate >= now);
		context.IsActive = inEffect && user.UserStatus == "Active";
	    }
	}

	private async Task<User> GetUser(IUserDbContext ctx, IPrincipal principal) {
	    var id = (principal.Identity as ClaimsIdentity);
	    var sub = id.FindFirst("sub");

	    if (sub == null)
		throw new InvalidOperationException("sub claim is missing.");

	    var userId = int.Parse(sub.Value);

	    var user = await ctx.Users
		.Include(x => x.UserRoles)
		    .ThenInclude(ur => ur.Role)
		.FirstOrDefaultAsync(x => x.UserId == userId);
	    return user;
	}

	private List<Claim> BuildClaims(User user, IEnumerable<string> requestedClaims) {
	    var claims = new List<Claim>();
	    Action<string, string> addClaim = (type, value) => {
		claims.Add(new Claim(type, value));
	    };

	    addClaim("sub", user.UserId.ToString());
	    addClaim("name", user.Username);

	    if (requestedClaims != null) {
		foreach (var requested in requestedClaims) {
		    //TODO: If there were requested claims, we would add them here.
		    switch (requested) {
			case "role": {
				foreach (var ur in user.UserRoles) {
				    addClaim(requested, ur.Role.Name);
				}
			    }
			    break;
			default:
			    break;
		    }
		}
	    }

	    return claims;
	}
    }
}
