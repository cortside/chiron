using System;
using Chiron.Registration.Customer.BootStrap;
using Chiron.Registration.Customer.BootStrap.Installer;
using Chiron.Registration.Customer.WebApi.Filters;
using Cortside.Common.BootStrap;
using Cortside.Common.IoC;
using Cortside.Common.Web.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Chiron.Registration.Customer.WebApi {

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

        public IServiceProvider ConfigureServices(IServiceCollection services) {

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "Customer Registration API", Version = "v1" });
            });

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetPreflightMaxAge(new TimeSpan(0, 15, 0)).Build()));

            services
            .AddMvc(options => {
                options.Filters.Add(typeof(ValidationErrorFilterAttribute));
            })
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

            var cr = config.Build();
            var authConfig = cr.GetSection("Auth");

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme =
                                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                                           JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => {
                o.Authority = authConfig.GetValue<string>("Provider");
                o.Audience = authConfig.GetValue<string>("ApiName");
                o.RequireHttpsMetadata = authConfig.GetValue<bool>("RequireHttpsMetadata");
            });

            return bootstrapper.InitIoCContainer(config, services);
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
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Registration API.");
            });
        }
    }
}
