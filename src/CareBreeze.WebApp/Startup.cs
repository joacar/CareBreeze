using AutoMapper;
using CareBreeze.Core;
using CareBreeze.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace CareBreeze.WebApp
{
    public class Startup
    {
        private readonly string _basePath;

        public Startup(IHostingEnvironment env)
        {
            Console.WriteLine(env.ContentRootPath);
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _basePath = env.ContentRootPath;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO Why no option using an IDbContextFactory ?
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<CareBreezeDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("CareBreeze"), o =>
                    {
                        o.MigrationsAssembly("CareBreeze.Data");
                    }));
            // Add framework services.
            services.AddMvc()
                 .AddJsonOptions(options =>
                 {
                     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                     options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                     options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                 });

            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IDataImportReader, JsonDataFileImporterReader>();
            services.AddScoped<IDataImportPersister, JsonDataImportPersister>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapAreaRoute(
                    "apiArea",
                    "api",
                    "{area:exists}/{controller=Patient}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            // Seed data
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CareBreezeDbContext>();
                if (context.Database.EnsureCreated())
                {
                    context.SeedEnumeration().Wait();
                    // Init data
                    var persister = app.ApplicationServices.GetRequiredService<IDataImportPersister>();
                    persister.Persist(Path.Combine(AppContext.BaseDirectory, "InitializationData.json"));
                }
            }
        }
    }
}
