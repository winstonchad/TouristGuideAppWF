using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using TouristGuideAppWF.Services;
using Xunit;

namespace TouristGuideAppWF.Tests
{
    public class NominatimServiceTests
    {
        private readonly NominatimService _nominatimService;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler; // Mock for HTTP requests
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration; // Configuration for testing

        public NominatimServiceTests()
        {
            // Create a mock HTTP handler to simulate API responses
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Initialize an HttpClient using the mock handler
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://nominatim.openstreetmap.org/") // Set base URL
            };

            // Set up a test configuration with dummy values
            var configData = new Dictionary<string, string>
            {
                { "TestKey", "TestValue" } // This key-value pair is just a placeholder for testing
            };

            // Build an in-memory configuration for dependency injection
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configData)
                .Build();

            // Initialize the NominatimService with the mocked dependencies
            _nominatimService = new NominatimService(_httpClient, _configuration);
        }

        [Fact]
        public async Task GetCoordinatesAsync_ShouldReturnCoordinates()
        {
            // Simulated response from the Nominatim API containing latitude and longitude for "London"
            var responseContent = "[{\"lat\": \"51.5074\", \"lon\": \"-0.1278\"}]";

            // Configure the mock HTTP handler to return a predefined response
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK, // Simulate a successful HTTP response (200 OK)
                    Content = new StringContent(responseContent) // Provide the fake JSON response
                });

            // Call the method under test with "London"
            var result = await _nominatimService.GetCoordinatesAsync("London");

            // Assert that the returned coordinates match the expected values
            Assert.Equal(51.5074, result.latitude);
            Assert.Equal(-0.1278, result.longitude);
        }
    }
}
