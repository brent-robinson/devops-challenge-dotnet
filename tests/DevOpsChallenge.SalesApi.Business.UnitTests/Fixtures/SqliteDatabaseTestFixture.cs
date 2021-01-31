using DevOpsChallenge.SalesApi.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;

namespace DevOpsChallenge.SalesApi.Business.UnitTests.Fixtures
{
    public class SqliteDatabaseTestFixture : IDisposable
    {
        public SqliteDatabaseTestFixture()
        {
            // Initialise DI context
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddBusiness();

            // Each connection is to an isolated in memory SQLite database (i.e. calling this twice results in two databases - allowing concurrent usage of two fixture instances without database conflicts)
            this.DatabaseConnection = new SqliteConnection("Filename=:memory:");
            this.DatabaseConnection.Open();

            // Configure database
            serviceCollection.AddDatabase(o =>
            {

                // SQLite
                o.UseSqlite(this.DatabaseConnection, s =>
                    s.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName));

                // Options
                o.EnableSensitiveDataLogging();
                o.EnableDetailedErrors();
            });

            // Build service provider
            this.ServiceProvider = serviceCollection.BuildServiceProvider();

            // Ensure database is created
            using IServiceScope scope = this.ServiceProvider.CreateScope();
            DatabaseContext databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            databaseContext.Database.EnsureCreated();
        }

        protected IServiceProvider ServiceProvider { get; }

        protected DbConnection DatabaseConnection { get; set; }

        public IServiceProvider Services => this.ServiceProvider;

        public void Dispose()
        {
            // Tear down database
            using (IServiceScope scope = this.ServiceProvider.CreateScope())
            {
                DatabaseContext databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                databaseContext.Database.EnsureDeleted();
            }

            // Disconnect SQLite
            this.DatabaseConnection?.Close();
            this.DatabaseConnection?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
