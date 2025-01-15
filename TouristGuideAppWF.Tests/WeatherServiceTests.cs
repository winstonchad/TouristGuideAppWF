using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using TouristGuideAppWF.Services;
using Xunit;

namespace TouristGuideAppWF.Tests
{
    public class WeatherServiceTests
    {
        private readonly WeatherService _weatherService; // Instance of WeatherService for testing
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler; // Mocked HTTP message handler
        private readonly HttpClient _httpClient; // Mocked HTTP client
        private readonly IConfiguration _configuration; // Configuration for API key

        public WeatherServiceTests()
        {
            // Mocking HTTP message handler for simulating API responses
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Create an HTTP client using the mocked handler
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            // Creating in-memory configuration for the weather API key
            var configData = new Dictionary<string, string>
            {
                { "WeatherApi:ApiKey", "test-api-key" } // Mock API key for testing
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configData)
                .Build();

            // Initializing WeatherService with mocked dependencies
            _weatherService = new WeatherService(_httpClient, _configuration);
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldReturnWeatherInfo()
        {
            // Mocked JSON response for the weather API
            var responseContent = "{ \"weather\": [{ \"description\": \"clear sky\" }], \"main\": { \"temp\": 25.0 } }";

            // Setup mock HTTP response for API call
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), // Any request
                    ItExpr.IsAny<CancellationToken>())   // Any cancellation token
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK, // Mock successful response
                    Content = new StringContent(responseContent) // Mock response body
                });

            // Act: Call GetWeatherAsync with test city and coordinates
            var result = await _weatherService.GetWeatherAsync("London", 51.5074, -0.1278);

            // Assert: Ensure that the response contains expected values
            Assert.Contains("clear sky", result); // Verify weather description
            Assert.Contains("25°C", result); // Verify temperature
        }
    }
}
