// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chiron.Auth.WebApi.Controllers.Grants {
    /// <summary>
    /// This sample controller allows a user to revoke grants given to clients
    /// </summary>
    [SecurityHeaders]
    [Authorize(AuthenticationSchemes = IdentityServer4.IdentityServerConstants.DefaultCookieAuthenticationScheme)]
    public class ClaimsController : Controller {

        public ClaimsController() {
        }

        /// <summary>
        /// Show list of grants
        /// </summary>
        [HttpGet]
        public IActionResult Index() {
            return View("Index");
        }

    }
}
