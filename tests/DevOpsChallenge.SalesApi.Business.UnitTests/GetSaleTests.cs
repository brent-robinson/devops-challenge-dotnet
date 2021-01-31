using DevOpsChallenge.SalesApi.Business.Exceptions;
using DevOpsChallenge.SalesApi.Business.Models;
using DevOpsChallenge.SalesApi.Business.Services;
using DevOpsChallenge.SalesApi.Business.UnitTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DevOpsChallenge.SalesApi.Business.UnitTests
{
    [Trait("Type", "Unit")]
    public class GetSaleTests : IClassFixture<SqliteDatabaseTestFixture>
    {
        public GetSaleTests(SqliteDatabaseTestFixture fixture)
        {
            this.Fixture = fixture;
        }

        protected SqliteDatabaseTestFixture Fixture { get; }

        [Fact]
        public async Task Get_Get_Sale_By_Transaction_Id()
        {
            // ARRANGE
            // Service provider/scope
            using IServiceScope scope = this.Fixture.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;

            // Services
            ISaleService saleService = services.GetRequiredService<ISaleService>();

            // Data
            string saleTransactionId = Guid.NewGuid().ToString("N", null);

            // ACT
            // Add sale
            SaleDetails addedSale = await saleService.AddSaleAsync(
                saleTransactionId,
                DateTime.UtcNow.Date,
                new Random().NextDouble() * 100,
                "Test sale").ConfigureAwait(false);

            // Lookup sale
            SaleDetails foundSale = await saleService.GetSaleAsync(saleTransactionId).ConfigureAwait(false);

            // ASSERT
            Assert.NotNull(foundSale);
            Assert.Equal(saleTransactionId, foundSale.TransactionId);
            Assert.Equal(addedSale.TransactionId, foundSale.TransactionId);
            Assert.Equal(addedSale.Date, foundSale.Date);
            Assert.Equal(addedSale.Amount, foundSale.Amount);
            Assert.Equal(addedSale.Notes, foundSale.Notes);
        }

        [Fact]
        public async Task Cannot_Get_Sale_That_Does_Not_Exist()
        {
            // ARRANGE
            // Service provider/scope
            using IServiceScope scope = this.Fixture.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;

            // Services
            ISaleService saleService = services.GetRequiredService<ISaleService>();

            // ACT
            Func<Task> task = async () => await saleService.GetSaleAsync(Guid.NewGuid().ToString("N", null)).ConfigureAwait(false);

            // ASSERT
            await Assert.ThrowsAsync<SaleExceptions.NotFound>(task);
        }
    }
}
