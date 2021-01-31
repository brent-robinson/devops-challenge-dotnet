using DevOpsChallenge.SalesApi.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DevOpsChallenge.SalesApi.Database
{
    /// <summary>
    /// Extensions for configuring the dependency injection context.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure the database services in the dependency injection context.
        /// </summary>
        /// <param name="services">The dependency injection context.</param>
        /// <param name="options">The options for configuring the database connection.</param>
        /// <returns>The dependendency injection context.</returns>
        public static IServiceCollection AddDatabase(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            // Context
            services.AddDbContext<DatabaseContext>(options);

            // Repositories
            services.AddScoped<ISaleRepository, SaleRepository>();

            return services;
        }
    }
}
