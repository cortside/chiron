//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Chiron.Admin.WebApi {
//    public class ClaimsAuthorizationAttribute : AuthorizationFilterAttribute {
//        public string ClaimType { get; set; }
//        public string ClaimValue { get; set; }

//        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken) {

//            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

//            if (!principal.Identity.IsAuthenticated) {
//                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
//                return Task.FromResult<object>(null);
//            }

//            if (!(principal.HasClaim(x => x.Type == ClaimType && x.Value == ClaimValue))) {
//                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
//                return Task.FromResult<object>(null);
//            }

//            //User is Authorized, complete execution
//            return Task.FromResult<object>(null);

//        }
//    }
//}
