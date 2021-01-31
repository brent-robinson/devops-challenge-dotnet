using DevOpsChallenge.SalesApi.IntegrationTests.Fixtures;
using DevOpsChallenge.SalesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DevOpsChallenge.SalesApi.IntegrationTests
{
    [Trait("Type", "Integration")]
    public class SaleTests : IClassFixture<HttpClientFixture>
    {
        public SaleTests(HttpClientFixture fixture)
        {
            this.Fixture = fixture;
        }

        protected HttpClientFixture Fixture { get; }

        [Fact]
        public async Task Can_Add_Then_Find_Multiple_Sales_For_The_Previous_Calendar_Month()
        {
            // ARRANGE
            HttpClient client = this.Fixture.HttpClient;

            // Prepare data
            DateTime endDate = DateTime.UtcNow.Date;
            DateTime startDate = endDate.AddMonths(-1);

            Random random = new Random();

            List<string> transactionIds = new List<string>();

            string run = Guid.NewGuid().ToString("N", null).Substring(0, 8);

            // ACT
            // Create sales
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Create data
                SaleDto payload = new SaleDto
                {
                    TransactionId = Guid.NewGuid().ToString("N", null),
                    Date = date.ToString("yyyy-MM-dd"),
                    Amount = Math.Round(random.NextDouble() * 100, 2),
                    Notes = "Integration test data " + run,
                };

                // Capture generated transaction IDs
                transactionIds.Add(payload.TransactionId);

                // Create sale
                HttpResponseMessage createSaleHttpResponse = await client.PostAsJsonAsync("api/sales", payload).ConfigureAwait(false);
                createSaleHttpResponse.EnsureSuccessStatusCode();
            }

            // Search for sales
            string endpoint = string.Format("api/sales?from={0}&to={1}", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            HttpResponseMessage querySalesHttpResponse = await client.GetAsync(endpoint).ConfigureAwait(false);
            querySalesHttpResponse.EnsureSuccessStatusCode();
            SaleSummaryDto[] salesData = await querySalesHttpResponse.Content.ReadAsAsync<SaleSummaryDto[]>();

            // ASSERT
            Assert.NotNull(salesData);
            foreach (string transactionId in transactionIds)
            {
                Assert.Contains(transactionId, salesData.Select(x => x.TransactionId));
            }
        }
    }
}
