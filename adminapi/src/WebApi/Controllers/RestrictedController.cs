using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolicyServer.Client;

namespace Chiron.Admin.WebApi.Controllers {
    public class RestrictedController : Controller {

        private readonly IPolicyServerClient _client;

        public RestrictedController(IPolicyServerClient client) {
            _client = client;
        }

        [HttpGet]
        [Authorize]
        [Route("api/whoami")]
        public async Task<object> WhoAmI() {
            // get roles and permission for current user
            var result = await _client.EvaluateAsync(User);
            var roles = result.Roles;
            var permissions = result.Permissions;

            return new {
                Username = User.Identity.Name,
                IdentityRoles = User.Claims.Where(c => c.Type == "role").Select(c => c.Value),
                PolicyRoles = roles,
                Permissions = permissions,
                Claims = User.Claims.Select(c =>
                new {
                    type = c.Type,
                    value = c.Value
                })
            };
        }

        [HttpGet]
        [Authorize] // doesn't matter who or what role, just that user was authenticated
        [Route("api/restricted")]
        public int[] Get() {
            return new int[] { 1, 2, 3 };
        }

        [HttpGet]
        [Authorize(Roles = "admin")] // this is a role returned from ids
        [Route("api/admin")]
        public int[] GetAdmin() {
            return new int[] { 4, 5, 6 };
        }

        [HttpGet]
        [Authorize(Roles = "clerk")] // this is a role returned from ids
        [Route("api/clerk")]
        public int[] GetClerk() {
            return new int[] { 7, 8, 9 };
        }

        [HttpGet]
        [Authorize("DoOther")]  // this is a permission defined by policyserver
        [Route("api/other")]
        public int[] GetOther() {
            return new int[] { 100, 200, 300 };
        }

        [HttpGet]
        [Authorize(Roles = "sysadmin")] // this is a role defined in policyserver
        [Route("api/sysadmin")]
        public int[] GetSysAdmin() {
            return new int[] { 9999 };
        }
    }
}
