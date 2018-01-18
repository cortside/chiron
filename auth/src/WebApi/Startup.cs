using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Chiron.Auth.Data;
using IdentityServer4.EntityFramework.DbContexts;
using Cortside.Common.DomainEvent;
using Chiron.Auth.Services;
using Chiron.Auth.EventHandlers;
using IdentityServer4;

namespace Chiron.Auth {
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
                { "Chiron.Registration.Recruiter.Event.RecruiterRegisteredEvent", typeof(UserRegisteredEvent) },
                { "Chiron.Registration.HiringManager.Event.HiringManagerRegisteredEvent", typeof(UserRegisteredEvent) }
            };
        }

        private void ConfigureIdentityServer(IServiceCollection services) {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var authConnString = Configuration.GetConnectionString("AuthConnString");
            services.AddDbContext<UserDbContext>(o => o.UseSqlServer(authConnString), ServiceLifetime.Transient);
            services.AddTransient<IUserDbContext>(x => x.GetRequiredService<UserDbContext>());
            services.AddIdentityServer()
                .AddDeveloperSigningCredential() //TODO: DO NOT USE THIS FOR PRODUCTION; USE A CERT!
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
            //InitializeDatabase(app);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddFile("Logs/{Date}.txt");
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

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
            }
        }
    }
}
