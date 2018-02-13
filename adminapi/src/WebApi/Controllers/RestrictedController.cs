using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chiron.Admin.WebApi.Controllers {
    public class RestrictedController : Controller {
        [HttpGet]
        [Authorize]
        [Route("api/restricted")]
        public int[] Get() {
            return new int[] { 1, 2, 3 };
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("api/admin")]
        public int[] GetAdmin() {
            return new int[] { 4, 5, 6 };
        }

        [HttpGet]
        [Authorize(Roles = "clerk")]
        [Route("api/clerk")]
        public int[] GetClerk() {
            return new int[] { 7, 8, 9 };
        }

        [HttpGet]
        [Authorize]
        [Route("api/roles")]
        public List<KeyValuePair<string, string>> GetRoles() {

            var claims = new List<KeyValuePair<string, string>>();
            foreach (var claim in User.Claims) {
                claims.Add(new KeyValuePair<string, string>(claim.Type, claim.Value));
            }
            return claims;
        }

        [HttpGet]
        [Authorize]
        [Route("api/claims")]
        public object GetClaims() {
            //var claims = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();

            return User.Claims.Select(c =>
                new {
                    subject = c.Subject.Name,
                    type = c.Type,
                    value = c.Value
                });
        }

    }
}
