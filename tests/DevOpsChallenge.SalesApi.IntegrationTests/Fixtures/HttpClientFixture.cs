using System;
using System.Net.Http;

namespace DevOpsChallenge.SalesApi.IntegrationTests.Fixtures
{
    public class HttpClientFixture : IDisposable
    {
        public HttpClientFixture()
        {
            // Determine if specific configuration has been provided
            string endpoint = Environment.GetEnvironmentVariable("SALES_API_ENDPOINT") ?? "http://localhost:5000";

            // Initialise a HTTP client
            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new Uri(endpoint);
        }

        public HttpClient HttpClient { get; }

        public void Dispose()
        {
            this.HttpClient?.Dispose();
        }
    }
}
