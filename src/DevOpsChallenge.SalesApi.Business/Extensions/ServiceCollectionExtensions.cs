using DevOpsChallenge.SalesApi.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpsChallenge.SalesApi.Business
{
    /// <summary>
    /// Configure the dependency injection context.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure the dependency injection context for the business layer.
        /// </summary>
        /// <param name="services">The dependency injection context.</param>
        /// <returns>The dependency injection context for chaining.</returns>
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            // Services
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}
