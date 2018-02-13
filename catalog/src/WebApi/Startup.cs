using Cortside.Common.BootStrap;
using Cortside.Common.IoC;
using Cortside.Common.Web.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Chiron.Catalog.BootStrap;
using Chiron.Catalog.BootStrap.Installer;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Chiron.Catalog.WebApi {

    public class Startup {
	private readonly BootStrapper bootstrapper = null;
	private readonly IHostingEnvironment env;

	public Startup(IHostingEnvironment hostingEnv) {
	    this.env = hostingEnv;
	    //convert Enums to Strings (instead of Integer) globally
	    JsonConvert.DefaultSettings = (() => {
		var settings = new JsonSerializerSettings();
		settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
		return settings;
	    });

	    bootstrapper = new DefaultApplicationBootStrapper();
	    bootstrapper.AddInstaller(new WebApiInstaller());
	}

	public void ConfigureServices(IServiceCollection services) {
	    services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetPreflightMaxAge(new TimeSpan(0, 15, 0)).Build()));

	    services
		.AddMvc()
		.AddJsonOptions(options => {
		    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		    options.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
		    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		});

	    services.AddRouting(options => {
		options.LowercaseUrls = true;
	    });

	    var config = new ConfigurationBuilder()
		.SetBasePath(env.ContentRootPath)
		.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
		.AddJsonFile("config.json", true, true)
		.AddJsonFile("build.json", true, true)
		.AddEnvironmentVariables();

	    bootstrapper.InitIoCContainer(config, services);
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
	    loggerFactory.AddConsole(DI.Configuration.GetSection("Logging"));
	    loggerFactory.AddFile("Logs/{Date}.txt");
	    loggerFactory.AddDebug();

	    app.UseCors("AllowAll");

	    app.UseStaticFiles();
	    app.UseExceptionHandler();
	    app.UseGenericExceptionHandler().UseHttpException();

	    var authConfig = DI.Configuration.GetSection("Auth");
	    app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
		{
		    Authority = authConfig.GetValue<string>("Provider"),
		    RequireHttpsMetadata = authConfig.GetValue<bool>("RequireHttpsMetadata"),
		    ApiName = authConfig.GetValue<string>("ApiName")
		});
	    app.UseMvc();
	}
    }
}
