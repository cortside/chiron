using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chiron.Auth.WebApi.Data;
using Chiron.Auth.WebApi.EventHandlers;
using Chiron.Auth.WebApi.Services;
using Cortside.Common.DomainEvent;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Chiron.Auth.WebApi {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"config.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public IServiceProvider ServiceProvider { private set; get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services) {
            // Add framework services.
            services.AddMvc();
            services.AddSingleton(Configuration);

            ConfigureLocalServices(services);

            ConfigureIdentityServer(services);

            ServiceProvider = services.BuildServiceProvider();

            return ServiceProvider;
        }

        private void ConfigureLocalServices(IServiceCollection services) {
            var config = Configuration.GetSection("ServiceBus");
            var settings = new ServiceBusSettings {
                Address = config.GetValue<string>("Address"),
                AppName = config.GetValue<string>("AppName"),
                Protocol = config.GetValue<string>("Protocol"),
                PolicyName = config.GetValue<string>("Policy"),
                Key = config.GetValue<string>("Key"),
                Namespace = config.GetValue<string>("Namespace")
            };

            services.AddSingleton(settings);
            services.AddSingleton<IDomainEventReceiver, DomainEventReceiver>();
            services.AddSingleton<IHashProvider, HashProvider>();
            services.AddSingleton<IAuthenticator, Authenticator>();

            Assembly.GetEntryAssembly().GetTypes()
                .Where(x => (x.Name.EndsWith("Service") || x.Name.EndsWith("Handler") || x.Name.EndsWith("Factory"))
                    && x.GetTypeInfo().IsClass
                    && !x.GetTypeInfo().IsAbstract
                    && x.GetInterfaces().Any())
                .ToList().ForEach(x => {
                    x.GetInterfaces().ToList()
                        .ForEach(i => services.AddSingleton(i, x));
                });
        }

        private IDictionary<string, Type> RegisterMessageTypes() {
            return new Dictionary<string, Type> {
                { "Chiron.Registration.Customer.Event.CustomerRegisteredEvent", typeof(UserRegisteredEvent) },
                { "Chiron.Registration.Clerk.Event.ClerkRegisteredEvent", typeof(UserRegisteredEvent) }
            };
        }

        private void ConfigureIdentityServer(IServiceCollection services) {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var authConnString = Configuration.GetConnectionString("AuthConnString");
            services.AddDbContext<UserDbContext>(o => o.UseSqlServer(authConnString), ServiceLifetime.Transient);
            services.AddTransient<IUserDbContext>(x => x.GetRequiredService<UserDbContext>());

            string publicOrigin = null;
            if (!String.IsNullOrWhiteSpace(Configuration["publicOrigin"])) {
                publicOrigin = Configuration["publicOrigin"];
            }

            string issuerUri = null;
            if (!String.IsNullOrWhiteSpace(Configuration["issuerUri"])) {
                issuerUri = Configuration["issuerUri"];
            }

            IIdentityServerBuilder idsBuilder;
            if (!String.IsNullOrWhiteSpace(publicOrigin)) {
                idsBuilder = services.AddIdentityServer(options => {
                    options.PublicOrigin = publicOrigin;
                });
            } else if (!String.IsNullOrWhiteSpace(issuerUri)) {
                idsBuilder = services.AddIdentityServer(options => {
                    options.IssuerUri = issuerUri;
                });
            } else {
                idsBuilder = services.AddIdentityServer();
            }

            idsBuilder.AddDeveloperSigningCredential() //TODO: DO NOT USE THIS FOR PRODUCTION; USE A CERT!
                .AddConfigurationStore(options => {
                    options.DefaultSchema = "auth";
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(authConnString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                })
                .AddOperationalStore(options => {
                    options.DefaultSchema = "auth";
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(authConnString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    //options.EnableTokenCleanup = true;
                    //options.TokenCleanupInterval = 30;
                });

            services.AddAuthentication()
            .AddOpenIdConnect("AAD", "Azure Active Directory", options => {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                options.Authority = "https://login.microsoftonline.com/common";
                options.ClientId = "d0abb606-ebd5-4939-903f-f2c702ff40b0";
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = false
                };
                options.GetClaimsFromUserInfoEndpoint = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            var logger = loggerFactory.CreateLogger<Startup>();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            // to handle x-forwarded-prefix header when backend is mapped to virtual path on frontend
            app.Use((context, next) => {
                var prefix = context.Request.Headers["x-forwarded-prefix"];
                if (!StringValues.IsNullOrEmpty(prefix)) {
                    logger.LogInformation($"X-Forwarded-Prefix={prefix}, Path={context.Request.Path}");
                    context.Request.PathBase = PathString.FromUriComponent(prefix.ToString());
                }
                return next();
            });

            // to handle x-forwarded-* headers
            var fordwardedHeaderOptions = new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(fordwardedHeaderOptions);

            app
                .UseStaticFiles()
                .UseIdentityServer()
                .UseAuthentication()
                .UseMvc(routes => {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });

            // message consumer for registered users
            var eventTypeLookup = RegisterMessageTypes();
            var receiver = ServiceProvider.GetService<IDomainEventReceiver>();
            receiver.Receive(eventTypeLookup);
        }
    }
}
