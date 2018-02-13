using System;
using Chiron.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Chiron.Admin.WebApi.Controllers {

    [Route("api/[controller]")]
    public class SettingsController : Controller {
        private readonly IConfigurationRoot config;

        public SettingsController(IConfigurationRoot config) {
            this.config = config;
        }

        [HttpGet]
        public Settings Get() {
            var s = new Settings();

            s.Deployment = config["DEPLOYMENT"];
            s.App = config["SERVICE"];
            s.Config = config["CONFIG"];
            s.Build = new Build();
            s.Build.Date = DateTime.Parse(config.GetSection("build")["timestamp"]);
            s.Build.Version = config.GetSection("build")["version"];
            s.Build.Tag = config.GetSection("build")["tag"];
            return s;
        }
    }
}
