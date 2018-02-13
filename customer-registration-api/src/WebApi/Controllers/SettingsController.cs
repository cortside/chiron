using System;
using Chiron.Registration.Customer.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Chiron.Registration.Customer.WebApi.Controllers {

    [Route("api/[controller]")]
    public class SettingsController : Controller {
        private readonly IConfiguration config;

        public SettingsController(IConfigurationRoot config) {
            this.config = config;
        }

        [HttpGet]
        public SettingsModel Get() {
            var s = new SettingsModel() {
                Deployment = config["DEPLOYMENT"],
                App = config["SERVICE"],
                Config = config["CONFIG"],
                Build = new Build()
            };
            s.Build.Date = DateTime.Parse(config.GetSection("build")["timestamp"]);
            s.Build.Version = config.GetSection("build")["version"];

            return s;
        }
    }
}
