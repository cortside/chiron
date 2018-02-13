using Microsoft.Extensions.Configuration;
using Chiron.Catalog.Models;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Chiron.Catalog.WebApi.Controllers {

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
	    s.Config= config["CONFIG"];
	    s.Build = new Build();
	    s.Build.Date = DateTime.Parse(config.GetSection("build")["timestamp"]);
	    s.Build.Version = config.GetSection("build")["version"];

	    return s;
	}
    }
}
