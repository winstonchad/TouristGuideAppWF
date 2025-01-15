using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using TouristGuideAppWF.Services;
using Xunit;

namespace TouristGuideAppWF.Tests
{
    public class GeminiServiceTests
    {
        private readonly GeminiService _geminiService;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler; // Mock handler for HTTP requests
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration; // Configuration for API key

        public GeminiServiceTests()
        {
            // Create a mock HTTP handler to simulate API requests
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Initialize an HttpClient using the mock handler
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            // Set up test configuration with a fake API key
            var configData = new Dictionary<string, string>
            {
                { "GoogleAi:ApiKey", "test-api-key" } // Fake API key for testing
            };

            // Build the test configuration
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configData) // Store config data in memory
                .Build();

            // Initialize the GeminiService with mocked dependencies
            _geminiService = new GeminiService(_httpClient, _configuration);
        }

        [Fact]
        public async Task GetTouristAttractionsAsync_ShouldReturnTouristInfo()
        {
            // Simulated response from the Gemini API containing tourist attractions
            var responseContent = "{ \"candidates\": [{ \"content\": { \"parts\": [{ \"text\": \"Eiffel Tower, Louvre Museum\" }] } }] }";

            // Configure the mock HTTP handler to return a predefined response
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK, // Simulate a successful response (200 OK)
                    Content = new StringContent(responseContent) // Provide the fake response content
                });

            // Call the method under test
            var result = await _geminiService.GetTouristAttractionsAsync("Paris");

            // Verify that the result contains expected attractions
            Assert.Contains("Eiffel Tower", result);
            Assert.Contains("Louvre Museum", result);
        }
    }
}
