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
    public class AddSaleTests : IClassFixture<SqliteDatabaseTestFixture>
    {
        public AddSaleTests(SqliteDatabaseTestFixture fixture)
        {
            this.Fixture = fixture;
        }

        protected SqliteDatabaseTestFixture Fixture { get; }

        [Fact]
        public async Task Can_Add_Sale()
        {
            // ARRANGE
            // Service provider/scope
            using IServiceScope scope = this.Fixture.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;

            // Services
            ISaleService saleService = services.GetRequiredService<ISaleService>();

            // Data
            string saleTransactionId = Guid.NewGuid().ToString("N", null);
            DateTime saleDate = DateTime.UtcNow.Date;
            double saleAmount = new Random().NextDouble() * 100;
            string saleNotes = "Test sale";

            // ACT
            // Add sale
            SaleDetails sale = await saleService.AddSaleAsync(
                saleTransactionId,
                saleDate,
                saleAmount,
                saleNotes).ConfigureAwait(false);

            // ASSERT
            Assert.NotNull(sale);
            Assert.Equal(saleTransactionId, sale.TransactionId);
            Assert.Equal(saleDate, sale.Date);
            Assert.Equal(saleAmount, sale.Amount);
            Assert.Equal(saleNotes, sale.Notes);
        }

        [Fact]
        public async Task Cannot_Add_Duplicate_Sale_Transaction_Id()
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
            // First sale
            SaleDetails sale1 = await saleService.AddSaleAsync(
                saleTransactionId,
                DateTime.UtcNow.Date,
                new Random().NextDouble() * 100,
                "Test duplicate sale 1").ConfigureAwait(false);

            // Duplicate sale
            Func<Task> task = async () => await saleService.AddSaleAsync(
                saleTransactionId,
                DateTime.UtcNow.Date,
                new Random().NextDouble() * 100,
                "Test duplicate sale 2").ConfigureAwait(false);

            // ASSERT
            await Assert.ThrowsAsync<SaleExceptions.AlreadyExists>(task);
        }

        [Fact]
        public async Task Cannot_Add_Future_Sale()
        {
            // ARRANGE
            // Service provider/scope
            using IServiceScope scope = this.Fixture.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;

            // Services
            ISaleService saleService = services.GetRequiredService<ISaleService>();

            // ACT
            Func<Task> task = async () => await saleService.AddSaleAsync(
                Guid.NewGuid().ToString("N", null),
                DateTime.UtcNow.Date.AddDays(2),
                new Random().NextDouble() * 100,
                "Test future sale").ConfigureAwait(false);

            // ASSERT
            await Assert.ThrowsAsync<SaleExceptions.DateInvalid>(task);
        }
    }
}
