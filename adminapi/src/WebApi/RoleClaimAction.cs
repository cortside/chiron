using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Newtonsoft.Json.Linq;

namespace Chiron.Admin.WebApi {
    internal class RoleClaimAction : ClaimAction {
        public RoleClaimAction()
            : base("role", ClaimValueTypes.String) {
        }

        public override void Run(JObject userData, ClaimsIdentity identity, string issuer) {
            var tokens = userData.SelectTokens("role");
            IEnumerable<string> roles;

            foreach (var token in tokens) {
                if (token is JArray) {
                    var jarray = token as JArray;
                    roles = jarray.Values<string>();
                } else
                    roles = new string[] { token.Value<string>() };

                foreach (var role in roles) {
                    Claim claim = new Claim("role", role, ValueType, issuer);
                    if (!identity.HasClaim(c => c.Subject == claim.Subject && c.Value == claim.Value)) {
                        identity.AddClaim(claim);
                    }
                }
            }
        }
    }
}
