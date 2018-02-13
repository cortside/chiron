using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chiron.Auth.Data;
using Chiron.Auth.EventHandlers;
using Chiron.Auth.Services;
using Cortside.Common.DomainEvent;
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            var logger = loggerFactory.CreateLogger<Startup>();
            //InitializeDatabase(app);

            //var serilog = new LoggerConfiguration()
            //            .MinimumLevel.Verbose()
            //            .Enrich.FromLogContext()
            //            .WriteTo.File(@"identityserver4_log.log");

            //serilog.WriteTo.Console(outputTemplate: "[{Timestamp:o} {Level}] {SourceContext} - {Message}{NewLine}{Exception}{NewLine}");


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
                    // TODO: subtract PathBase from Path if needed.
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
                .UseMvc(routes => {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });

            var eventTypeLookup = RegisterMessageTypes();
            var receiver = ServiceProvider.GetService<IDomainEventReceiver>();
            receiver.Receive(eventTypeLookup);
        }

        private void InitializeDatabase(IApplicationBuilder app) {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any()) {
                    foreach (var client in Config.GetClients()) {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any()) {
                    foreach (var resource in Config.GetIdentityResources()) {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any()) {
                    foreach (var resource in Config.GetApiResources()) {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }

        private void MigrateDatabase(IApplicationBuilder app) {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
            }
        }
    }
}
