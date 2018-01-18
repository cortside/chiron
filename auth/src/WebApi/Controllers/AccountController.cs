using Chiron.Auth.Models;
using Chiron.Auth.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Chiron.Auth.Controllers {
    public class AccountController : Controller {
        readonly IAuthenticator authenticator;
        readonly IIdentityServerInteractionService idsService;
        readonly IConfigurationRoot config;
        readonly ILogger<AccountController> logger;

        public AccountController(IAuthenticator authenticator, IIdentityServerInteractionService idsService, IConfigurationRoot config, ILogger<AccountController> logger) {
            this.authenticator = authenticator;
            this.idsService = idsService;
            this.config = config;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl) {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model) {
            if (!ModelState.IsValid) { return View(model); }

            var loginInfo = new LoginInfo {
                Username = model.Username,
                Password = model.Password
            };
            var user = await authenticator.AuthenticateAsync(loginInfo);

            if (user != null) {
                AuthenticationProperties props = null;
                if (model.RememberLogin) {
                    props = new AuthenticationProperties {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(new TimeSpan(999, 0, 0, 0)) // expire at the heat death of the universe. Or in 999 days.
                    };
                }
                await HttpContext.SignInAsync(user.UserId.ToString(), user.Username, props);
                logger.LogInformation($"Return URL: {model.ReturnUrl}");
                if (idsService.IsValidReturnUrl(model.ReturnUrl)) {
                    return Redirect(model.ReturnUrl);
                }

                return Redirect(config["defaultUrl"]); //Return to 
            }

            ModelState.AddModelError("", "Invalid Credentials."); //Shouldn't hardcode error messages like this here.
            return View(model);
        }

        [HttpGet, HttpPost]
        public async Task<IActionResult> Logout() {
            // delete local authentication cookie
            await HttpContext.SignOutAsync();
            return Redirect(config["defaultUrl"]); //TODO: Don't do this for production.
        }

        //TODO: Do we need registration stuff?
    }
}
