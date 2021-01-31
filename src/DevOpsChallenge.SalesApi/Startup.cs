using DevOpsChallenge.SalesApi.Business;
using DevOpsChallenge.SalesApi.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace DevOpsChallenge.SalesApi
{
    /// <summary>
    /// Start the web application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Configure the dependency injection container.
        /// </summary>
        /// <param name="services">The dependency injection service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // --------------------------------------------------------------------------------
            // DOMAIN
            // --------------------------------------------------------------------------------

            // Business
            services.AddBusiness();

            // --------------------------------------------------------------------------------
            // DATABASE
            // --------------------------------------------------------------------------------

            // Database Context Options
            void DbContextOptionsBuilder(DbContextOptionsBuilder builder) =>
                builder.UseSqlServer(this.Configuration.GetConnectionString("Database"), o =>
                    o.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName));

            // Databases
            services.AddDatabase(DbContextOptionsBuilder);

            // --------------------------------------------------------------------------------
            // HTTP PIPELINE
            // --------------------------------------------------------------------------------

            // MVC
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Swagger
            services.AddSwaggerGen(c =>
            {
                // Information
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "sales-api", Version = "v1" });

                // Comments
                string xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                if (File.Exists(xmlCommentsFilePath))
                {
                    c.IncludeXmlComments(xmlCommentsFilePath);
                }

                // Operation IDs - required for some client code generation tools
                c.CustomOperationIds(x => (x.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
            });
        }

        /// <summary>
        /// Configure the HTTP pipeline and some services.
        /// </summary>
        /// <param name="app">The application pipeline builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Development mode
            if (env.IsDevelopment())
            {
                // Show exceptions
                app.UseDeveloperExceptionPage();

                // Swagger JSON
                app.UseSwagger();

                // Swagger UI
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "sales-api" + ' ' + "v1");
                });
            }

            // Endpoint routing
            app.UseRouting();

            // Endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
